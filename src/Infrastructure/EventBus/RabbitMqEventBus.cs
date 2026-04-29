using RabbitMQ.Client;
using Shop.Application.Interfaces;
using System.Text.Json;

namespace Shop.Infrastructure.EventBus
{
    public sealed class RabbitMqEventBus : IEventBus, IDisposable
    {
        private const string ExchangeName = "shop.events";

        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqEventBus> _logger;

        public RabbitMqEventBus(IConnection connection, ILogger<RabbitMqEventBus> logger)
        {
            _connection = connection;
            _logger = logger;

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true);
        }

        public Task PublishAsync<T>(T @event, CancellationToken ct = default) where T : IIntegrationEvent
        {
            var routingKey = RoutingKeyFor<T>();
            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.ContentType = "application/json";
            props.MessageId = @event.EventId.ToString();
            props.Timestamp = new AmqpTimestamp(
                new DateTimeOffset(@event.OccurredOn).ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                basicProperties: props,
                body: body);

            _logger.LogInformation("[EventBus] Published {EventType} (id={EventId}) ---> routing key '{Key}'", 
                typeof(T).Name, @event.EventId, routingKey);

            return Task.CompletedTask;
        }

        public static string RoutingKeyFor<T>() =>
            typeof(T).Name.EndsWith("Event")
                ? typeof(T).Name[..^"Event".Length]
                : typeof(T).Name;

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
