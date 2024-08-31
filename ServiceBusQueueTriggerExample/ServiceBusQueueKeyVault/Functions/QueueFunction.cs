using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using QueueExample.Services;

namespace ServiceBusQueueKeyVault;

public class QueueFunction
{
    private readonly ILogger<QueueFunction> _logger;
    private readonly IQueueService _queueService;

    public QueueFunction(ILogger<QueueFunction> logger, IQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    [Function(nameof(QueueFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ServiceBus:QueueName%", Connection = "ServiceBus:ConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        // Log stuff
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Handle the queue item
        await _queueService.HandleQueueItemAsync(message.Body);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);

    }
}