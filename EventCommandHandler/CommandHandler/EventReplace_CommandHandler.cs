using AggregateBase;
using AggregateBase.Service;
using AppEvent.Event;
using System;
using Aggregate.Event;
using System.Threading.Tasks;

namespace EventCommand.CommandHandler
{
    public class EventReplace_CommandHandler : ICommandHandler<AppModel.Command.Event.Replace_Command>
    {
        private readonly IEventStoreService<Guid, Guid, EventAggregate> _service;
        public EventReplace_CommandHandler(IEventStoreService<Guid, Guid, EventAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.Event.Replace_Command command)
        {
            var aggregate = new EventAggregate(command.AppEvent);
            var @event = new Event_Replaced_Event(command.AppEvent);
            aggregate.RaiseEvent(@event);
            _service.AddEvent(aggregate);
            if(Publish) Task.Run(() => EventPublisher.EventPublisher.SendAsync(@event));
        }
    }
}
