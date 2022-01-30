using AggregateBase;
using AggregateBase.Service;
using System;
using AppEvent.Event;
using Aggregate.User;

namespace UserCommand.CommandHandler
{
    class End_UserParticipation_CommandHandler : ICommandHandler<AppModel.Command.User.End_Participation_Command>
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public End_UserParticipation_CommandHandler(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.User.End_Participation_Command command)
        {
            var aggregate = new UserAggregate(command.User);
            var end_UserEvent_Participation_Event = new End_UserEvent_Participation_Event(command.User._id, command.Event);
            var end_EventUser_Participation_Event = new End_EventUser_Participation_Event(command.Event._id, command.User);
            aggregate.RaiseEvent(end_UserEvent_Participation_Event);
            _service.AddEvent(aggregate);
            if (Publish)
            {
                EventPublisher.EventPublisher.SendMessage(end_UserEvent_Participation_Event);
                EventPublisher.EventPublisher.SendMessage(end_EventUser_Participation_Event, EventPublisher.EventPublisher.Exchange.FanoutExchange);
            }
        }
    }
}

