namespace QueueExample.Services;

public interface IQueueService
{
    /// <summary>
    /// Processes a queue item.
    /// </summary>
    Task HandleQueueItemAsync(BinaryData data);

    /// <summary>Sends a message to a queue.</summary>
    /// <param name="dataToSerialize">The class instance to serialize</param>
    /// <param name="delayInSeconds">The number of seconds to delay the message</param>
    Task SendMessageAsync<T>(T dataToSerialize, int delayInSeconds) where T : class;

    /// <summary>Sends a message to a queue.</summary>
    /// <param name="messageData">The message to send</param>
    /// <param name="delayInSeconds">The number of seconds to delay the message</param>
    Task SendMessageAsync(string messageData, int delayInSeconds);
}