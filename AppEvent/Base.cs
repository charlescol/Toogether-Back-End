using System;

namespace AppEvent.Base
{
    public class DomainEvent<TId, AId>
    {
        public TId Id { get; set; }
        public AId AggregateId { get; set; }
        public DateTime Occured { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public object Data { get; set; }
        public DomainEvent() { }
        public DomainEvent(TId id, DateTime occured, object data)
        {
            Name = this.GetType().Name;
            Id = id;
            Occured = occured;
            Data = data;
        }
        public DomainEvent(TId id, AId aggregateID, string name, string version, DateTime occured, object data) : this(id, occured, data)
        {
            AggregateId = aggregateID;
            Version = version;
            Name = name;
        }
    }
    public interface IDomainEventPublisher
    { }
}
