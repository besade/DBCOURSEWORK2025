using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Shop.Application.Events;
using Shop.Application.Interfaces;
using System.Text.Json;
using System.Text;

namespace Shop.Infrastructure.EventBus
{
    public sealed class RabbitMqConsumerService : BackgroundService
    {
        private const string ExchangeName = "shop.events";

        private static readonly IReadOnlyDictionary<string, (Type Event, Type Handler)> Subscriptions =
            new Dictionary<string, (Type, Type)>
            {
                ["OrderCreated"] = (typeof(OrderCreatedEvent), typeof(IEventHandler<OrderCreatedEvent>)),
                ["OrderStatusChanged"] = (typeof(OrderStatusChangedEvent), typeof(IEventHandler<OrderStatusChangedEvent>)),
                ["LowStockDetected"] = (typeof(LowStockDetectedEvent), typeof(IEventHandler<LowStockDetectedEvent>)),
            };

        private readonly IServiceProvider _services;
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMqConsumerService> _logger;
        private readonly List<IModel> _channels = new();

        public RabbitMqConsumerService(
            IServiceProvider services,
            IConnection connection,
            ILogger<RabbitMqConsumerService> logger)
        {
            _services = services;
            _connection = connection;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken ct)
        {
            foreach (var (routingKey, (eventType, _)) in Subscriptions)
            {
                var channel = _connection.CreateModel();
                var queueName = $"shop.{routingKey.ToLowerInvariant()}";

                channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
                channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queueName, ExchangeName, routingKey);
                channel.BasicQos(0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (_, ea) =>
                    ProcessMessage(channel, ea, eventType, routingKey, ct);

                channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
                _channels.Add(channel);

                _logger.LogInformation(
                    "[EventBus] Subscribed to '{Queue}' (routing key: {Key})",
                    queueName, routingKey);
            }

            return Task.CompletedTask;
        }

        private void ProcessMessage(IModel channel, BasicDeliverEventArgs ea, Type eventType, string routingKey, CancellationToken ct)
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = (IIntegrationEvent)JsonSerializer.Deserialize(json, eventType)!;

                _logger.LogDebug("[EventBus] Received {Key}: {Json}", routingKey, json);

                using var scope = _services.CreateScope();
                var handlerType = Subscriptions[routingKey].Handler;
                var handler = scope.ServiceProvider.GetRequiredService(handlerType);

                var method = handlerType.GetMethod("HandleAsync")!;
                ((Task)method.Invoke(handler, [@event, ct])!).GetAwaiter().GetResult();

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[EventBus] Error processing message with routing key '{Key}'. Re-queuing.",
                    routingKey);

                channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
            }
        }

        public override void Dispose()
        {
            foreach (var ch in _channels) ch.Dispose();
            base.Dispose();
        }
    }
}
