using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.Command.User
{
    using UserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.PrivateUser>;
    using RestrictedUserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.RestrictedPublicUser>;
    using RestrictedEventItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.RestrictedEvent>;
    public class Create_Command : ICommand
    {
        public UserItem User { get; private set; }
        public Create_Command(UserItem user)
        {
            User = user;
        }
    }

    public class Replace_Command : ICommand
    {
        public UserItem User { get; private set; }
        public Replace_Command(UserItem user)
        {
            User = user;
        }
    }
    public class End_Participation_Command : ICommand
    {
        public RestrictedUserItem User { get; private set; }
        public RestrictedEventItem Event { get; private set; }
        public End_Participation_Command(RestrictedUserItem user, RestrictedEventItem @event)
        {
            User = user;
            Event = @event;
        }
    }
    public class Participation_Command : ICommand
    {
        public RestrictedUserItem User { get; private set; }
        public RestrictedEventItem Event { get; private set; }
        public Participation_Command(RestrictedUserItem user, RestrictedEventItem @event)
        {
            User = user;
            Event = @event;
        }
    }
    public class End_Follow_Command : ICommand
    {
        public UserItem UserFollowed { get; private set; }
        public UserItem UserFollower { get; private set; }
        public End_Follow_Command(UserItem userFollowed, UserItem userFollower)
        {
            UserFollowed = userFollowed;
            UserFollower = userFollower;
        }
    }
    public class Follow_Command : ICommand
    {
        public UserItem UserFollowed { get; private set; }
        public UserItem UserFollower { get; private set; }
        public Follow_Command(UserItem userFollowed, UserItem userFollower)
        {
            UserFollowed = userFollowed;
            UserFollower = userFollower;
        }
    }
}
