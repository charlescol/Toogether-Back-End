using System;

namespace AppEvent.Event
{
    using EventItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.GeneralEvent>;
    public class Event_Replaced_Event : Base.DomainEvent<Guid, Guid>
    {
        public Event_Replaced_Event(EventItem @event) : base(Guid.NewGuid(), DateTime.Now, @event)
        {
            Version = "1.0";
            AggregateId = @event._id;
        }
    }
}
