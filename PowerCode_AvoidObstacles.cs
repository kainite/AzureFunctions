using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace PowerCode_AvoidObstacles
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string inputArray = req.Query["inputArray"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            inputArray = inputArray ?? data?.inputArray;

            int[] array = inputArray.Split(';').Select(n => Convert.ToInt32(n)).ToArray();

            Array.Sort<int>(array, new Comparison<int>((i1, i2) => i1.CompareTo(i2)));

            List<int> value = new List<int>();

            for(int i=1; i <= array[array.Length - 1]; i++)
            {
                if (!array.Contains(i))
                {
                    if (!value.Contains(i))
                        {
                        value.Add(i);
                        };
                };
            }


            string message = string.Join(",", value.ToArray());

            string responseMessage = string.IsNullOrEmpty(message)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"The values are: {message}";

            return new OkObjectResult(responseMessage);
        }
    }
}
