using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueExample.Model;
using System.Text.Json;

namespace QueueExample.Services;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly ServiceSettings _settings;
    private ServiceBusClient? _client;

    /// <summary>Constructor</summary>
    public QueueService(ILogger<QueueService> logger, IOptions<ServiceSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    /// <summary>The service bus client.</summary>
    /// <remarks> I didn't put this in constructor because it does some work when constructed and I don't
    /// expect this class to be used 99% of the time when it's injected so I'm avoiding construction of the
    /// client till it is used by a call to the SendMessageAsync method.</remarks>
    private ServiceBusClient Client => _client ??= CreateServiceBusClient();

    /// <summary>
    /// Processes a queue item.
    /// </summary>
    public async Task HandleQueueItemAsync(BinaryData data)
    {
        Deal? someDeal = System.Text.Json.JsonSerializer.Deserialize<Deal>(data.ToString());
        if (someDeal != null && someDeal.IsGoodDeal == false)
        {
            someDeal.IsGoodDeal = true;
            await SendMessageAsync(someDeal, delayInSeconds: 120); // Put it back on the queue with a 2 minute delay
            _logger.LogWarning($"Bad deal re-queued: {_settings.QueueName}");
        }
        else
        {
            _logger.LogInformation($"Good deal: {_settings.QueueName}");
        }
    }

    /// <summary>Sends a message to a queue.</summary>
    /// <param name="dataToSerialize">The class instance to serialize</param>
    /// <param name="delayInSeconds">The number of seconds to delay the message</param>
    public async Task SendMessageAsync<T>(T dataToSerialize, int delayInSeconds) where T : class
    {
        await SendMessageAsync(JsonSerializer.Serialize(dataToSerialize), delayInSeconds);
    }

    /// <summary>Sends a message to a queue.</summary>
    /// <param name="messageData">The message to send</param>
    /// <param name="delayInSeconds">The number of seconds to delay the message</param>
    public async Task SendMessageAsync(string messageData, int delayInSeconds)
    {
        await using var sender = Client.CreateSender(_settings.QueueName);

        var message = new ServiceBusMessage(messageData)
        {
            ScheduledEnqueueTime = new DateTimeOffset(DateTime.UtcNow.AddSeconds(delayInSeconds))
        };

        await sender.SendMessageAsync(message);
    }


    /// <summary>Creates a service bus client based upon the settings.</summary>
    private ServiceBusClient CreateServiceBusClient()
    {
        if (string.IsNullOrWhiteSpace(_settings.ConnectionString) == false &&
            _settings.ConnectionString.Contains("SharedAccessKeyName"))
        {
            return new ServiceBusClient(_settings.ConnectionString);
        }

        if (string.IsNullOrWhiteSpace(_settings.FullyQualifiedNamespace) == false)
        {
            return new ServiceBusClient(_settings.FullyQualifiedNamespace, new DefaultAzureCredential());
        }

        throw new ArgumentException("The Service Bus connection string does not contain a SharedAccessKeyName, " +
            "so your intent must be to use a managed identity in the cloud and your identity to connect locally; " +
            "however, the Fully Qualified Namespace was not specified!");
    }

}