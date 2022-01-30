using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AggregateBase;
using AppEvent.Base;
using AppEvent.Event;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace UserCommand.EventPublisher
{
    public class EventPublisher
    {
        public struct Exchange
        {
            public const string TopicExchange = "TopicExchange";
            public const string FanoutExchange = "FanoutExchange";
        }
        public struct Queue
        {
            public const string EventQueryQueue = "EventQueryQueue";
            public const string UserQueryQueue = "UserQueryQueue";
            public const string EventInternQueue = "EventInternQueue";
        }
        public static void Init()
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(Environment.GetEnvironmentVariable("RabbitMQConnection"))
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    Dictionary<string, object> argument = new Dictionary<string, object>()
                    {
                        {"x-queue-type", "quorum"}
                    };
                    channel.ExchangeDeclare(Exchange.TopicExchange, ExchangeType.Topic, true);
                    channel.ExchangeDeclare(Exchange.FanoutExchange, ExchangeType.Fanout, true);
                    channel.QueueDeclare(Queue.EventQueryQueue, true, false, false, argument);
                    channel.QueueDeclare(Queue.EventInternQueue, true, false, false, argument);
                    channel.QueueDeclare(Queue.UserQueryQueue, true, false, false, argument);

                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(User_Created_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(User_Replaced_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(UserEvent_Participation_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(End_UserEvent_Participation_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(End_FollowedFollower_Followed_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(End_FollowerFollowed_Follow_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(FollowerFollowed_Follow_Event));
                    channel.QueueBind(Queue.UserQueryQueue, Exchange.TopicExchange, nameof(FollowedFollower_Followed_Event));

                    channel.QueueBind(Queue.EventQueryQueue, Exchange.TopicExchange, nameof(Event_Created_Event));
                    channel.QueueBind(Queue.EventQueryQueue, Exchange.TopicExchange, nameof(Event_Replaced_Event));
                    channel.QueueBind(Queue.EventQueryQueue, Exchange.TopicExchange, nameof(User_Replaced_Event));
                    channel.QueueBind(Queue.EventQueryQueue, Exchange.FanoutExchange, nameof(End_EventUser_Participation_Event));
                    channel.QueueBind(Queue.EventQueryQueue, Exchange.FanoutExchange, nameof(EventUser_Participation_Event));

                    channel.QueueBind(Queue.EventInternQueue, Exchange.FanoutExchange, nameof(End_EventUser_Participation_Event));
                    channel.QueueBind(Queue.EventInternQueue, Exchange.FanoutExchange, nameof(EventUser_Participation_Event));
                }
            }
        }
        public static void SendMessage(DomainEvent<Guid, Guid> @event, string exchange = Exchange.TopicExchange)
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(Environment.GetEnvironmentVariable("RabbitMQConnection"))
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.Persistent = false;

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                    channel.BasicPublish(exchange, @event.Name, props, body);
                }
            }
        }
    }
}
