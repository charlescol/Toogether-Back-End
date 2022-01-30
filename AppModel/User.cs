using System;
using System.Collections.Generic;

namespace AppModel
{
    namespace User
    {

        using UserRestrictedItem = AppModel.Storage.ReferencedItem<System.Guid, User.RestrictedPublicUser>;
        using EventRestrictedItem = AppModel.Storage.ReferencedItem<System.Guid, Event.RestrictedEvent>;
        struct Info
        {
            const string Version = "1.0";
        }
        public class PrivateUser 
        {
            public PrivateUser(string phoneNumber, string email, DateTime accountCreationDate, string authPlatform)
            {
                PhoneNumber = phoneNumber.Trim();
                Email = email.Trim();
                AccountCreationDate = accountCreationDate;
                AuthPlatform = authPlatform.Trim();
            }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public DateTime AccountCreationDate { get; set; }
            public string AuthPlatform { get; set; }
            public PublicUser Public { get; set; }
            
        }
        public class PublicUser 
        {
            public PublicUser(DateTime birtdayDate, string sex, uint age)
            {
                BirthdayDate = birtdayDate;
                Sex = sex;
            }
            public string Sex { get; set; }
            public string Description { get; set; }
            public RestrictedPublicUser MainInfo { get; set; }
            public DateTime BirthdayDate { get; set; }
            public List<UserRestrictedItem> Followers { get; set; } = new List<UserRestrictedItem>();
            public List<UserRestrictedItem> Follow { get; set; } = new List<UserRestrictedItem>();
            public List<string> Tags { get; set; } = new List<string>();
            public List<Blob> Blobs { get; set; } = new List<Blob>();
            public List<EventRestrictedItem> FavoriteEvents { get; set; } = new List<EventRestrictedItem>();
            public List<EventRestrictedItem> ParticipateEvents { get; set; } = new List<EventRestrictedItem>();
            public List<EventRestrictedItem> HistoryEvents { get; set; } = new List<EventRestrictedItem>();
        }
        public class RestrictedPublicUser 
        {
            public RestrictedPublicUser(string firstName, string lastName, string profilePictureUri)
            {
                FirstName = firstName.Trim();
                LastName = lastName.Trim();
                ProfilePictureUri = profilePictureUri.Trim();
            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfilePictureUri { get; set; }
        }
        public class Blob
        {
            public Event.Blob Info { get; set; }
            public DateTime DateAdded { get; set; }
        }
        public enum Sex
        {
            Male,
            Female,
            Other
        }
    }
}
