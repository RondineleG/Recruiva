namespace Recruiva.Core.Messaging;

public sealed record RabbitMQConfiguration(
    string HostName,
    int Port,
    string UserName,
    string Password,
    string VirtualHost,
    int RequestedHeartbeat,
    bool AutomaticRecoveryEnabled,
    int NetworkRecoveryInterval);
