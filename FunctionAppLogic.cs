using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppLogic
{
    public static class FunctionAppLogic
    {
        [FunctionName("FunctionAppLogic")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Website obj;

            obj = JsonConvert.DeserializeObject<Website>(requestBody);
            log.LogInformation((obj.id).ToString());
            log.LogInformation(obj.name);
            log.LogInformation((obj.url).ToString());

            return (ActionResult)new OkObjectResult(obj);
        }
        public class Website
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}
