using System;

namespace AppEvent.Event
{
    using RestrictedUserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.RestrictedPublicUser>;
    public class End_EventUser_Participation_Event : Base.DomainEvent<Guid, Guid>
    {
        public End_EventUser_Participation_Event(System.Guid aggregateId, RestrictedUserItem user) : base(Guid.NewGuid(), DateTime.Now, user)
        {
            Version = "1.0";
            AggregateId = aggregateId;
        }
    }
}
