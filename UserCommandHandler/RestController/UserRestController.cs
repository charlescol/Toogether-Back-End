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

namespace UserCommand
{
    namespace RestController
    {
        public class UserRestController
        {
            private readonly IEventStoreService<Guid, Guid, UserAggregate> _service;
            public UserRestController(IEventStoreService<Guid, Guid, UserAggregate> service)
            {
                _service = service;
            }
            [FunctionName("Create_Command")]
            public async Task<IActionResult> CreateUser(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
                ILogger log)
            {
                try
                {
                    EventPublisher.EventPublisher.Init();
                    var content = await new StreamReader(req.Body).ReadToEndAsync();

                    var command = JsonConvert.DeserializeObject<AppModel.Command.User.Create_Command>(content);
                    new CommandHandler.UserCreate_CommandHandler(_service).Handle(command);
                }
                catch (Exception e)
                {
                    log.LogError(e.Message);
                    return new BadRequestResult();
                }
                return new OkObjectResult(null);
            }

            [FunctionName("Replace_Command")]
            public async Task<IActionResult> ReplaceUser(
                [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
                ILogger log)
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                try
                {
                    var command = JsonConvert.DeserializeObject<AppModel.Command.User.Replace_Command>(content);
                    new CommandHandler.UserReplace_CommandHandler(_service).Handle(command);
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
}
