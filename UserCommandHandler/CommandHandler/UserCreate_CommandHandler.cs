using Aggregate.User;
using AggregateBase;
using AggregateBase.Service;
using AppEvent.Event;
using System;
using System.Threading.Tasks;

namespace UserCommand.CommandHandler
{
    public class UserCreate_CommandHandler : ICommandHandler<AppModel.Command.User.Create_Command>
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public UserCreate_CommandHandler(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.User.Create_Command command)
        {

            var aggregate = new UserAggregate(command.User);
            var @event = new User_Created_Event(command.User);
            aggregate.RaiseEvent(@event);
            _service.AddEvent(aggregate);
           Task.Run(() => EventPublisher.EventPublisher.SendAsync(@event));
        }
    }
}
