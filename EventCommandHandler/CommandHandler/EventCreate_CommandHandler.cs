using AggregateBase;
using AggregateBase.Service;
using AppEvent.Event;
using System;
using Aggregate.Event;

namespace EventCommand.CommandHandler
{
    public class EventCreate_CommandHandler : ICommandHandler<AppModel.Command.Event.Create_Command>
    {
        private readonly IEventStoreService<Guid, Guid, EventAggregate> _service;
        public EventCreate_CommandHandler(IEventStoreService<Guid, Guid, EventAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.Event.Create_Command command)
        {
            var aggregate = new EventAggregate(command.AppEvent);
            var @event = new Event_Created_Event(command.AppEvent);
            aggregate.RaiseEvent(@event);
            _service.AddEvent(aggregate);
            if(Publish) EventPublisher.EventPublisher.SendMessage(@event);
        }
    }
}
