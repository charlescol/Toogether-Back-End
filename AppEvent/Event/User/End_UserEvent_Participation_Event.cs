﻿using System;

namespace AppEvent.Event
{
    using RestrictedEventItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.RestrictedEvent>;
    public class End_UserEvent_Participation_Event : Base.DomainEvent<Guid, Guid>
    {
        public End_UserEvent_Participation_Event(System.Guid aggregateId, RestrictedEventItem @event) : base(Guid.NewGuid(), DateTime.Now, @event)
        {
            Version = "1.0";
            AggregateId = aggregateId;
        }
    }
}