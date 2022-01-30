using Aggregate.Event;
using AggregateBase.Service;
using AppEvent.Base;
using AppEvent.Event;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace EventCommandHandler.EventConsumer
{
    class EventConsumer
    {
        private readonly IEventStoreService<Guid, Guid, EventAggregate> _service;
        public EventConsumer(IEventStoreService<Guid, Guid, EventAggregate> service)
        {
            _service = service;
        }

         [FunctionName("EventConsumer")]
         public void User([RabbitMQTrigger("EventInternQueue", ConnectionStringSetting = "RabbitMQConnection")] string item, ILogger log)
         {
             try
             {
                 var @event = JsonConvert.DeserializeObject<DomainEvent<Guid, Guid>>(item);
                 if (@event != null)
                 {
                     switch (@event.Name)
                     {
                         // si on recoit un évement via la file d'attente, on suppose que les regles métier ont déjà été appliquées
                         case nameof(EventUser_Participation_Event) :
                             {
                                 _service.AddEvent(@event);
                                 break;
                             }
                         case nameof(End_EventUser_Participation_Event):
                             {
                                 _service.AddEvent(@event);
                                 break;
                             }
                     }
                 }
             }
             catch (Exception e)
             {
                 log.LogError(e.Message);
             }
         }
    }
}
