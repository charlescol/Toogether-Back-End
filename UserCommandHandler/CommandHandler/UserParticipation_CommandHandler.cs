using AggregateBase;
using AggregateBase.Service;
using System;
using AppEvent.Event;
using Aggregate.User;
using System.Threading.Tasks;

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
            var userEvent_ParticipationEvent = new UserEvent_Participation_Event(command.User._id, command.Event);
            var eventUser_ParticipationEvent = new EventUser_Participation_Event(command.Event._id, command.User);

            aggregate.RaiseEvent(userEvent_ParticipationEvent);
            _service.AddEvent(aggregate);
            if (Publish)
            {
                Task.Run(() => EventPublisher.EventPublisher.SendAsync(userEvent_ParticipationEvent))
                    .ContinueWith((p) => EventPublisher.EventPublisher.SendAsync(eventUser_ParticipationEvent, EventPublisher.EventPublisher.Queue.EventInternQueue),
                    TaskContinuationOptions.ExecuteSynchronously);
            }
        }
    }
}
