using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AggregateBase.Service;
using Aggregate.Event;

namespace EventCommand
{
    public class EventRestController
    {
        private readonly IEventStoreService<Guid, Guid, EventAggregate> _service;
        public EventRestController(IEventStoreService<Guid, Guid, EventAggregate> service)
        {
            _service = service;
        }
        [FunctionName("Create_Command")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<AppModel.Command.Event.Create_Command>(content);
                new CommandHandler.EventCreate_CommandHandler(_service).Handle(command);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }
            return new OkObjectResult(null);
        }

        [FunctionName("Replace_Command")]
        public async Task<IActionResult> Replace(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<AppModel.Command.Event.Replace_Command>(content);
                new CommandHandler.EventReplace_CommandHandler(_service).Handle(command);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }
            return new OkObjectResult(null);
        }
    }
}
