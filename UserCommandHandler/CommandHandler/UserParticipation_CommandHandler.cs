using AggregateBase;
using AggregateBase.Service;
using System;
using AppEvent.Event;
using Aggregate.User;

namespace UserCommand.CommandHandler
{
    class UserParticipation_CommandHandler : ICommandHandler<AppModel.Command.User.Participation_Command>
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public UserParticipation_CommandHandler(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.User.Participation_Command command)
        {
            var aggregate = new UserAggregate(command.User);
            var userEvent_Participation_Event = new UserEvent_Participation_Event(command.User._id, command.Event);
            var eventUser_participationEvent = new EventUser_Participation_Event(command.Event._id, command.User);

            aggregate.RaiseEvent(userEvent_Participation_Event);
            _service.AddEvent(aggregate);
            if (Publish)
            {
                EventPublisher.EventPublisher.SendMessage(userEvent_Participation_Event);
                EventPublisher.EventPublisher.SendMessage(eventUser_participationEvent, EventPublisher.EventPublisher.Exchange.FanoutExchange);
            }
        }
    }
}
