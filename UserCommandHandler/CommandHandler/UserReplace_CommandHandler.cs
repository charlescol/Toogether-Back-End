using AggregateBase;
using AggregateBase.Service;
using System;
using AppEvent.Event;
using Aggregate.User;
using System.Threading.Tasks;

namespace UserCommand.CommandHandler
{
    public class UserReplace_CommandHandler : ICommandHandler<AppModel.Command.User.Replace_Command>
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public UserReplace_CommandHandler(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        public override void Handle(AppModel.Command.User.Replace_Command command)
        {
            var aggregate = new UserAggregate(command.User);
            var @event = new User_Replaced_Event(command.User);
            aggregate.RaiseEvent(@event);
            _service.AddEvent(aggregate);
            if (Publish) Task.Run(() => EventPublisher.EventPublisher.SendAsync(@event));
        }
    }
}
