using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AggregateBase;
using AppEvent.Base;
using AppEvent.Event;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace UserCommand.EventPublisher
{
    public class EventPublisher 
    {
        static ServiceBusClient client;
        static ServiceBusSender sender;
        public struct Queue
        {
            public const string UserQueryQueue = "userqueryqueue";
            public const string EventInternQueue = "eventinternqueue";
        }
        public static async Task SendAsync(DomainEvent<Guid, Guid> @event, string queue = Queue.UserQueryQueue)
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
