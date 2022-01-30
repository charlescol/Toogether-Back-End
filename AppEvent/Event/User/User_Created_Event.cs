using System;

namespace AppEvent.Event
{
    using UserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.PrivateUser>;
    public class User_Created_Event : Base.DomainEvent<Guid, Guid>
    {
        public User_Created_Event(UserItem user) : base(Guid.NewGuid(), DateTime.Now, user)
        {
            Version = "1.0";
            AggregateId = user._id;
        }
    }
}
