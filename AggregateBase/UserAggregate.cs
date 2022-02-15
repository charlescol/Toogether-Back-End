using AggregateBase;
using System;
using AppEvent.Event;
using System.Collections.Generic;

namespace Aggregate.User
{
    using UserItem = AppModel.Storage.ReferencedItem<Guid, AppModel.User.PrivateUser>;
    using PublicUserItem = AppModel.Storage.ReferencedItem<Guid, AppModel.User.PublicUser>;
    using RestrictedUserItem = AppModel.Storage.ReferencedItem<Guid, AppModel.User.RestrictedPublicUser>;
    using RestrictedEventItem = AppModel.Storage.ReferencedItem<Guid, AppModel.Event.RestrictedEvent>;
    public class UserAggregate : AggregateBase<Guid, Guid>
    {
        public object Data { get; private set; }
        public UserAggregate(UserItem user)
        {
            CheckUserItem(user);
        }
        public UserAggregate(RestrictedUserItem user)
        {
            CheckRestrictedUserItem(user.Item);
        }
        public UserAggregate(PublicUserItem user)
        {
            CheckPublicUserItem(user.Item);
        }
        public UserAggregate(List<UserItem> users)
        {
            users.ForEach(item => CheckUserItem(item));
        }
        public UserAggregate(List<RestrictedUserItem> users)
        {
            users.ForEach(user => CheckRestrictedUserItem(user.Item));
        }
        public UserAggregate(List<PublicUserItem> users)
        {
            users.ForEach(user => CheckPublicUserItem(user.Item));
        }
        public void CheckUserItem(UserItem user)
        {
            if (String.IsNullOrEmpty(user.Item.PhoneNumber)) /*|| user.PhoneNumber.Length != 10)*/ throw new ArgumentNullException(nameof(user.Item.PhoneNumber.Length));
            CheckPublicUserItem(user.Item.Public);
            CheckRestrictedUserItem(user.Item.Public.MainInfo);
        }
        public void CheckPublicUserItem(AppModel.User.PublicUser user)
        {}
        public void CheckRestrictedUserItem(AppModel.User.RestrictedPublicUser user)
        {
            if (String.IsNullOrEmpty(user.FirstName)) throw new ArgumentNullException(nameof(user.FirstName));
            if (String.IsNullOrEmpty(user.LastName)) throw new ArgumentNullException(nameof(user.LastName));
        }
        public void Apply(User_Created_Event @event)
        {
            Data = (UserItem) @event.Data;
        }
        public void Apply(User_Replaced_Event @event)
        {
            Data = (UserItem) @event.Data;
        }
        public void Apply(FollowerFollowed_Follow_Event @event)
        {
            Data = (RestrictedUserItem) @event.Data;
        }
        public void Apply(FollowedFollower_Followed_Event @event)
        {
            Data = (RestrictedUserItem)@event.Data;
        }
        public void Apply(End_FollowerFollowed_Follow_Event @event)
        {
            Data = (RestrictedUserItem)@event.Data;
        }
        public void Apply(End_FollowedFollower_Followed_Event @event)
        {
            Data = (RestrictedUserItem)@event.Data;
        }
        public void Apply(UserEvent_Participation_Event @event)
        {
            Data = (RestrictedEventItem)@event.Data;
        }
        public void Apply(End_UserEvent_Participation_Event @event)
        {
            Data = (RestrictedEventItem)@event.Data;
        }
    }
}
