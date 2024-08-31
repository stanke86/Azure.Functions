namespace QueueExample.Services;

public class ServiceSettings
{
    public const string SectionName = "ServiceBus";
    public string ConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string FullyQualifiedNamespace { get; set; } = string.Empty;
}