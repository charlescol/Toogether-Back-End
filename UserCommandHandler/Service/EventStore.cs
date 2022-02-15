using AggregateBase.Service;
using Newtonsoft.Json;
using System;
using AppEvent.Base;
using Aggregate.User;

namespace UserCommand.Service
{
    public class EventStore : IEventStoreService<Guid, Guid, UserAggregate>
    {
        private readonly Model.UserCommandDBContext _context;
        public EventStore(Model.UserCommandDBContext context)
        {
            _context = context;
        }
        public void AddEvent(UserAggregate aggregate)
        {
            Console.WriteLine(aggregate.GetUncommittedEvents());
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
            return new DomainEvent<Guid, Guid>(@event.Id, @event.AggregateId, @event.Version, @event.Name, @event.Occured, @event.Data);
        }
    }
}

