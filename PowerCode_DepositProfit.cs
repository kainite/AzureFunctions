using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PowerCode_DepositProfit
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string deposit = req.Query["deposit"];
            string rate = req.Query["rate"];
            string threshold = req.Query["threshold"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            deposit = deposit ?? data?.deposit;
            rate = rate ?? data?.rate;
            threshold = threshold ?? data?.threshold;

            int years = 0;
            bool yearend =false;

            float d = float.Parse(deposit);
            for (int y = 1;!yearend; y++)
            {
                if ((d += float.Parse(rate) / 100f * d) >= float.Parse(threshold))
                {
                   years = y;
                    yearend = true;
                }
            }

            string responseMessage = string.IsNullOrEmpty(years.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"It will take {years.ToString()} years.";

            return new OkObjectResult(responseMessage);
        }
    }
}
