using System;
using AppEvent.Base;
using AppEvent.Event;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DataQuery
{
    using UserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.PrivateUser>;
    using UserRestrictedItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.RestrictedPublicUser>;
    using EventItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.GeneralEvent>;
    using EventRestrictedItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.RestrictedEvent>;

    public class EventConsumer
    {
        private readonly IMongoDatabase _database;
        public EventConsumer(IMongoDatabase database)
        {
            _database = database;
        }

        [FunctionName("UserEventConsumer")]
        public void User([RabbitMQTrigger("UserQueryQueue", ConnectionStringSetting = "RabbitMQConnection")] string item, ILogger log)
        {
            try
            {
                var @event = JsonConvert.DeserializeObject<DomainEvent<Guid, Guid>>(item);
                if (@event != null)
                {
                    var collection = _database.GetCollection<UserItem>("UserQueryCollection");
                    switch (@event.Name)
                    {
                        case nameof(User_Created_Event):
                            {
                                var user = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserItem>();
                                collection.InsertOne(user);
                                break;
                            }
                        case nameof(User_Replaced_Event):
                            {
                                var user = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserItem>();
                                collection.ReplaceOne(Builders<UserItem>.Filter.Eq("_id", user._id), user);
                                break;
                            }
                        case nameof(FollowerFollowed_Follow_Event):
                            {
                                var restrictedUser = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), Builders<UserItem>.Update.Push(x => x.Item.Public.Follow, restrictedUser));
                                break;
                            }
                        case nameof(FollowedFollower_Followed_Event):
                            {
                                var restrictedUser = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), Builders<UserItem>.Update.Push(x => x.Item.Public.Followers, restrictedUser));
                                break;
                            }
                        case nameof(UserEvent_Participation_Event):
                            {
                                var restrictedEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<EventRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), Builders<UserItem>.Update.Push(x => x.Item.Public.ParticipateEvents, restrictedEvent));
                                break;
                            }
                        case nameof(End_FollowerFollowed_Follow_Event):
                            {
                                var restrictedUser = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), 
                                    Builders<UserItem>.Update.PullFilter(x => x.Item.Public.Follow, Builders<UserRestrictedItem>.Filter.Eq("_id", restrictedUser._id)));
                                break;
                            }
                        case nameof(End_FollowedFollower_Followed_Event):
                            {
                                var restrictedUser = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), 
                                    Builders<UserItem>.Update.PullFilter(x => x.Item.Public.Followers, Builders<UserRestrictedItem>.Filter.Eq("_id", restrictedUser._id)));
                                break;
                            }
                        case nameof(End_UserEvent_Participation_Event):
                            {
                                var restrictedEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<EventRestrictedItem>();
                                collection.UpdateOne(Builders<UserItem>.Filter.Eq("_id", @event.AggregateId), 
                                    Builders<UserItem>.Update.PullFilter(x => x.Item.Public.ParticipateEvents, Builders<EventRestrictedItem>.Filter.Eq("_id", restrictedEvent._id)));
                                break;
                            }
                    }
                }

            }
            catch (Exception e)
            {
                log.LogError(e.Message);
            }
        }
        [FunctionName("EventEventConsumer")]
        public void Event([RabbitMQTrigger("EventQueryQueue", ConnectionStringSetting = "RabbitMQConnection")] string item, ILogger log)
        {
            var @event = JsonConvert.DeserializeObject<DomainEvent<Guid, Guid>>(item);
            if (@event != null)
            {
                var collection = _database.GetCollection<EventItem>("EventQueryCollection");

                switch(@event.Name)
                {
                    case nameof(User_Replaced_Event):
                        {
                            var user = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserItem>();
                            collection.UpdateMany(Builders<EventItem>.Filter.Eq(x => x.Item.Info.MainInfo.Organizer._id, user._id), 
                                Builders<EventItem>.Update.Set(e => e.Item.Info.MainInfo.Organizer, new UserRestrictedItem(user._id, user.Item.Public.MainInfo)));
                            break;
                        }
                    case nameof(Event_Created_Event):
                        {
                            var appEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<EventItem>();
                            collection.InsertOne(appEvent);
                            break;
                        }
                    case nameof(Event_Replaced_Event):
                        {
                            var appEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<EventItem>();
                            collection.ReplaceOne(Builders<EventItem>.Filter.Eq("_id", appEvent._id), appEvent);
                            break;
                        }
                    case nameof(EventUser_Participation_Event):
                        {
                            var appEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                            collection.UpdateOne(Builders<EventItem>.Filter.Eq("_id", @event.AggregateId), Builders<EventItem>.Update.Push(x => x.Item.Info.MainInfo.Participants, appEvent));
                            break;
                        }
                    case nameof(End_EventUser_Participation_Event):
                        {
                            var appEvent = ((Newtonsoft.Json.Linq.JObject)@event.Data).ToObject<UserRestrictedItem>();
                            collection.UpdateOne(Builders<EventItem>.Filter.Eq("_id", @event.AggregateId),
                                Builders<EventItem>.Update.PullFilter(x => x.Item.Info.MainInfo.Participants, Builders<UserRestrictedItem>.Filter.Eq("_id", appEvent._id)));
                            break;
                        }
                }
            }
            
        }
    }
}
