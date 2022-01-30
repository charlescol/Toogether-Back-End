using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    using UserRestricted = AppModel.Storage.ReferencedItem<System.Guid, User.RestrictedPublicUser>;
    namespace Event
    {
        struct Info
        {
            const string Version = "1.0";
        }
        public enum EventType
        {
            Official,
            NonOfficial
        }
        public class GeneralEvent
        {
            public MapEvent Info { get; set; }
            public bool PublicationEnable { get; set; } 
        }

        public class MapEvent
        {
            public MapEvent(int pinOnMapCode, double pinRay, double positionX, double positionY)
            {
                PinOnMapCode = pinOnMapCode;
                PinRay = pinRay;
                PositionX = positionX;
                PositionY = positionY;
            }
            public int PinOnMapCode { get; set; }
            public double PinRay { get; set; }
            public double PositionX { get; set; }
            public double PositionY { get; set; }
            public RestrictedEvent MainInfo { get; set; }
        }

        public class RestrictedEvent
        {
            public RestrictedEvent(string title, string description, string address, DateTime date, UserRestricted organizer)
            {
                Title = title;
                Description = description;
                Address = address;
                Date = date;
                Organizer = organizer;
            }
            public string Title { get; set; }
            public string Description { get; set; }
            public EventType Type { get; set; }
            public string Address { get; set; }
            public DateTime Date { get; set; }
            public UserRestricted Organizer { get; set; }
            public List<ActorRole> Actors { get; set; } = new List<ActorRole>();
            public List<UserRestricted> Participants { get; set; } = new List<UserRestricted>();
            public List<Blob> Presentation { get; set; } = new List<Blob>();
            public List<string> Tags { get; set; } = new List<string>();
        }
        public class ActorRole
        {
            public string Role { get; set; }
            public UserRestricted Actor { get; set; }
        }
        public class Blob
        {
            public enum BlobType
            {
                Picture
            }
            public string Uri { get; set; }
            public RestrictedEvent Event { get; set; }
            public BlobType Type { get; set; }
        }
    }
}
