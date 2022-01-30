using AggregateBase;
using AppEvent.Event;
using System;

namespace Aggregate.Event
{
    using EventItem = AppModel.Storage.ReferencedItem<Guid, AppModel.Event.GeneralEvent>;
    using RestrictedEventItem = AppModel.Storage.ReferencedItem<Guid, AppModel.Event.RestrictedEvent>;
    public class EventAggregate : AggregateBase<Guid, Guid>
    {
        public EventItem Data { get; private set; }
        public EventAggregate(EventItem appEvent)
        {
            CheckRestrictedEventItem(appEvent.Item.Info.MainInfo);
        }
        public EventAggregate(RestrictedEventItem appEvent)
        {
            CheckRestrictedEventItem(appEvent.Item);
        }
        public void CheckRestrictedEventItem(AppModel.Event.RestrictedEvent appEvent)
        {
            if (String.IsNullOrEmpty(appEvent.Description)) throw new ArgumentNullException(nameof(appEvent.Description));
        }
        public void Apply(Event_Created_Event @event)
        {
            Data = (EventItem) @event.Data;
        }
        public void Apply(Event_Replaced_Event @event)
        {
            Data = (EventItem) @event.Data;
        }
    }
}
