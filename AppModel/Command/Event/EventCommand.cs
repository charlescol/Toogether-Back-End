using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.Command.Event
{
    using EventItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.Event.GeneralEvent>;
    public class Create_Command : ICommand
    {
        public EventItem AppEvent { get; private set; }
        public Create_Command(EventItem appEvent)
        {
            AppEvent = appEvent;
        }
    }
    public class Replace_Command : ICommand
    {
        public EventItem AppEvent { get; private set; }
        public Replace_Command(EventItem appEvent)
        {
            AppEvent = appEvent;
        }
    }
}
