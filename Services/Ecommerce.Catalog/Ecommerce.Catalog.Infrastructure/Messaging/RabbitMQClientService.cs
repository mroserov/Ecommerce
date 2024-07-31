using RabbitMQ.Client;

namespace Ecommerce.Catalog.Infrastructure.Messaging;

public class RabbitMQClientService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQClientService()
    {
        var factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "guest", Password = "guest" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public IModel Channel => _channel;

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
