using RabbitMQ.Client;

namespace Shop.Infrastructure.EventBus
{
    public static class RabbitMqConnectionFactory
    {
        public static IConnection CreateConnection(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(config["RabbitMQ:Port"] ?? "5672"),
                UserName = config["RabbitMQ:Username"] ?? "guest",
                Password = config["RabbitMQ:Password"] ?? "guest",
                VirtualHost = config["RabbitMQ:VHost"] ?? "/",
                DispatchConsumersAsync = false
            };
            return factory.CreateConnection("shop-app");
        }
    }
}
