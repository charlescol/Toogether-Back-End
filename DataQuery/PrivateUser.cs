using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace DataQuery
{
    using UserItem = AppModel.Storage.ReferencedItem<System.Guid, AppModel.User.PrivateUser>;
    public class PrivateUser
    {
        private readonly IMongoDatabase _database;
        public PrivateUser(IMongoDatabase database)
        {
            _database = database;
        }
        [FunctionName("User")]
        public IActionResult GetPrivateUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var id = Guid.Parse(req.Query["id"]);
                var collection = _database.GetCollection<UserItem>("UserQueryCollection");
                var result = collection.Find(x => x._id == id).FirstOrDefault();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }
        }
    }

}
