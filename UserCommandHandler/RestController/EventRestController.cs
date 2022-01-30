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
using Aggregate.User;

namespace UserCommandHandler.RestController
{
    class EventRestController
    {
        private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
        public EventRestController(IEventStoreService<Guid, Guid, UserAggregate> service)
        {
            _service = service;
        }
        [FunctionName("Participation_Command")]
        public async Task<IActionResult> NewUserFollower(
               [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
               ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<AppModel.Command.User.Participation_Command>(content);
                new UserCommand.CommandHandler.UserParticipation_CommandHandler(_service).Handle(command);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }
            return new OkObjectResult(null);
        }
        [FunctionName("End_Participation_Command")]
        public async Task<IActionResult> EndUserFollower(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
                ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<AppModel.Command.User.End_Participation_Command>(content);
                new UserCommand.CommandHandler.End_UserParticipation_CommandHandler(_service).Handle(command);
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
