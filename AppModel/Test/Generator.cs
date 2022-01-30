using System;
using System.Collections.Generic;

namespace AppModel.Test
{
    public class Eventgenerator
    {
        public static List<Storage.ReferencedItem<Guid, Event.MapEvent>> Generate(int number = 0)
        {
            var list = new List<Storage.ReferencedItem<Guid, Event.MapEvent>>();
            Random random_int = new Random();
            for (int i = 0; i < number; i++)
            {
                int pinMapCode = random_int.Next(1, 50);
                double pinRay = random_int.NextDouble() * random_int.Next(1, 20);
                double positionX = random_int.NextDouble() * random_int.Next(1, 200);
                double positionY = random_int.NextDouble() * random_int.Next(1, 200);
                var @event = new Storage.ReferencedItem<System.Guid, Event.MapEvent>(Guid.NewGuid(), new Event.MapEvent(pinMapCode, pinRay, positionX, positionY));
                @event.Item.MainInfo = new Event.RestrictedEvent("", "", "", DateTime.Now, UserGenerator.Generate(1)[0]);
                list.Add(@event);
            }
            return list;
        }
    }
    public class UserGenerator
    {
        public static List<Storage.ReferencedItem<Guid, User.RestrictedPublicUser>> Generate(int number)
        {
            var list = new List<Storage.ReferencedItem<Guid, User.RestrictedPublicUser>>();
            for (int i = 0; i < number; i++)
            {
                var user = new User.RestrictedPublicUser("", "", "");
                list.Add(new Storage.ReferencedItem<Guid, User.RestrictedPublicUser>(Guid.NewGuid(), user));
            }
            return list;
        }
    }
}
