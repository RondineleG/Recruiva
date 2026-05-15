using RabbitMQ.Client;

namespace Recruiva.Core.Messaging;

public interface IRabbitMQBus : IAsyncDisposable
{
    Task PublishAsync<T>(string queue, T message, CancellationToken cancellationToken = default);
    Task SubscribeAsync<T>(string queue, Func<T, Task> handler, CancellationToken cancellationToken = default);
    IConnection GetConnection();
}
