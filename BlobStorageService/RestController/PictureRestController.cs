using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageService.RestController
{
    public class BlobRestController
    {
        Service.IBlobStorage _service;
        public BlobRestController(Service.IBlobStorage service)
        {
            _service = service;
        }
        [FunctionName("ProfilePicture")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var content = await new StreamReader(req.Body).ReadToEndAsync();
                var blob = JsonConvert.DeserializeObject<AppModel.Query.SendBlobData>(content);
                var blob_uri = await _service.StoreImageInBlobServiceStorage(blob.Data, blob.NameWithExt, blob.Container);
                return new CreatedResult(blob_uri, new { name = blob.NameWithExt });
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestResult();
            }
        }
    }
}
