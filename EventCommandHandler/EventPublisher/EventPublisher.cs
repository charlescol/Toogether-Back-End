using System;
using System.Threading.Tasks;
using AppEvent.Base;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace EventCommand.EventPublisher
{
    public class EventPublisher
    {
        static ServiceBusClient client;
        static ServiceBusSender sender;
        public struct Queue
        {
            public const string EventQueryQueue = "EventQueryQueue";
            public const string EventInternQueue = "EventInternQueue";
        }
        public static async Task SendAsync(DomainEvent<Guid, Guid> @event, string queue = Queue.EventQueryQueue)
        {
            client = new ServiceBusClient(Environment.GetEnvironmentVariable("AzureServiceBusConnection"));
            // Create the clients that we'll use for sending and processing messages.
            sender = client.CreateSender(queue);
            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            // try adding a message to the batch
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(@event))))
            {
                // if it is too large for the batch
                throw new Exception($"The message is too large to fit in the batch.");
            }
            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
