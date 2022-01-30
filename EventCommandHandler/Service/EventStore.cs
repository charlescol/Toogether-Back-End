using Aggregate.Event;
using AggregateBase.Service;
using AppEvent.Base;
using Newtonsoft.Json;
using System;

namespace EventCommand.Service
{
    public class EventStore : IEventStoreService<Guid, Guid, EventAggregate>
    {
        private readonly Model.EventCommandDBContext _context;
        public EventStore(Model.EventCommandDBContext context)
        {
            _context = context;
        }
        public void AddEvent(EventAggregate aggregate) 
        {
            foreach (DomainEvent<Guid, Guid> @event in aggregate.GetUncommittedEvents())
            {
                AddEvent(@event);
            }
        }
        public void AddEvent(DomainEvent<Guid, Guid> @event)
        {
            Model.EventStore newLine = new Model.EventStore(@event.Id, @event.AggregateId, @event.Occured,
                    @event.Name, @event.Version, JsonConvert.SerializeObject(@event.Data));
            _context.Add(newLine);
            _context.SaveChanges();
        }
        public DomainEvent<Guid, Guid> GetEvent(Guid EventID)
        {
            Model.EventStore @event = _context.EventStore.Find(EventID);
            return new DomainEvent<Guid, Guid>(@event.Id,@event.AggregateId, @event.Version, @event.Name, @event.Occured, @event.Data);
        }
    }
}

