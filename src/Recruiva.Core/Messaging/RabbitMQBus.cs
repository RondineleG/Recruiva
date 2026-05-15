using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Recruiva.Core.Messaging;

public sealed class RabbitMQBus : IRabbitMQBus
{
    private readonly RabbitMQConfiguration _config;
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMQBus(RabbitMQConfiguration config)
    {
        _config = config;
        _connectionFactory = new ConnectionFactory
        {
            HostName = config.HostName,
            Port = config.Port,
            UserName = config.UserName,
            Password = config.Password,
            VirtualHost = config.VirtualHost,
            RequestedHeartbeat = TimeSpan.FromSeconds(config.RequestedHeartbeat),
            AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = TimeSpan.FromMilliseconds(config.NetworkRecoveryInterval)
        };
    }

    public IConnection GetConnection()
    {
        if (_connection is { IsOpen: true })
        {
            return _connection;
        }

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateChannel();

        return _connection;
    }

    public async Task PublishAsync<T>(string queue, T message, CancellationToken cancellationToken = default)
    {
        var connection = GetConnection();
        var channel = connection.CreateChannel();

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queue,
            mandatory: false,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async Task SubscribeAsync<T>(string queue, Func<T, Task> handler, CancellationToken cancellationToken = default)
    {
        var connection = GetConnection();
        var channel = connection.CreateChannel();

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(body);

            if (message is not null)
            {
                await handler(message);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
        };

        await channel.BasicConsumeAsync(
            queue: queue,
            autoAck: false,
            consumer: consumer,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
