using System;

namespace AppModel
{
    namespace Message
    {
        struct Info
        {
            const string Version = "1.0";
        }

        public class Message<Tid>
        {
            public Message(Tid senderID, Tid targetID, string textContent, DateTime date)
            {
                Date = date;
                SenderID = senderID;
                TargetID = targetID;
                TextContent = textContent;
            }
            public Tid SenderID { get; set; }
            public Tid TargetID { get; set; }
            public string TextContent { get; set; }
            public DateTime Date { get; set; }
            public bool VisibleSender { get; set; } = true;
            public bool VisibleTarget { get; set; } = true;
        }
    }
}
