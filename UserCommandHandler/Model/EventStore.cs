using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserCommand.Model
{
    public partial class EventStore
    {
        public EventStore() { }
        public EventStore(Guid id, Guid aggregateID, DateTime occured, string name, string version, string data)
        {
            Id = id;
            AggregateId = aggregateID;
            Occured = occured;
            Name = name;
            Version = version;
            Data = data;
        }
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public DateTime Occured { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Data { get; set; }
    }
}
