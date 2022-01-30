using AppEvent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregateBase
{

    public static class Config
    {
        public const float Version = 1.0f;
    }

    public interface IAggregateRoot<TId>
    {
    }

    public class AggregateBase<TId, AId> : IAggregateRoot<TId>
    {
        private IList<DomainEvent<TId, AId>> _uncommittedEvents = new List<DomainEvent<TId, AId>>();
        public short Version { get; protected set; } = 0;

        public void ClearUncommitedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public IEnumerable<DomainEvent<TId, AId>> GetUncommittedEvents()
        {
            return _uncommittedEvents.AsEnumerable();
        }

        public void RaiseEvent(DomainEvent<TId, AId> @event)
        {
            ApplyEvent(@event);
            _uncommittedEvents.Add(@event);
        }

        public void ApplyEvent(DomainEvent<TId, AId> @event)
        {
            if (!_uncommittedEvents.Any(x => Equals(x.Id, @event.Id)))
            {
                ((dynamic)this).Apply((dynamic)@event);
            }
        }
    }
    public abstract class ICommandHandler<Command> where Command : AppModel.Command.ICommand
    {
        public bool Publish { get; set; } = true;
        abstract public void Handle(Command command);
    }



    namespace Service
    {
        public interface IEventStoreService<TId, AId, AggregateType> where AggregateType : AggregateBase<TId, AId>
        {
            public void AddEvent(AggregateType aggregate);
            public void AddEvent(DomainEvent<Guid, Guid> @event);
            public DomainEvent<TId, AId> GetEvent(TId EventID);
        }
    }
}
