using AggregateBase;
using AggregateBase.Service;
using System;
using System.Collections.Generic;
using AppEvent.Event;
using Aggregate.User;
using System.Threading.Tasks;

namespace UserCommand.CommandHandler
{
    using RestrictedUserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.RestrictedPublicUser>;
    using UserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.PrivateUser>;
    class End_UserFollow_CommandHandler : ICommandHandler<AppModel.Command.User.End_Follow_Command>
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public End_UserFollow_CommandHandler(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.User.End_Follow_Command command)
        {
            var aggregate = new UserAggregate(new List<RestrictedUserItem> { command.UserFollower, command.UserFollowed });
            var @event_follower = new End_FollowerFollowed_Follow_Event(command.UserFollower._id, command.UserFollowed);
            var @event_followed = new End_FollowedFollower_Followed_Event(command.UserFollowed._id, command.UserFollower);
            aggregate.RaiseEvent(@event_follower);
            aggregate.RaiseEvent(@event_followed);
            _service.AddEvent(aggregate);
            if (Publish)
            {
                Task.Run(() => EventPublisher.EventPublisher.SendAsync(@event_follower))
                    .ContinueWith((p) => EventPublisher.EventPublisher.SendAsync(@event_followed), TaskContinuationOptions.ExecuteSynchronously);
                ;
            }
        }
    }
}
