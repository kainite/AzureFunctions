using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.VisualBasic.CompilerServices;

namespace Powercode_FixPonctuation
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string phrase = req.Query["phrase"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            phrase = phrase ?? data?.phrase;

            string[] punctuation = { ".", "?", "!", " .", " ?", " !", ". ", "? ", "! ", ",", ":", ";", " ,", " :", " ;", ", ", ": ", "; " };

            foreach (string final in punctuation)
            {
                phrase = phrase.Replace(final, final.Trim() + " ");
                  
            }

            phrase = string.Join(" ", phrase.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));


            string responseMessage = string.IsNullOrEmpty(phrase)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"{phrase}";

            return new OkObjectResult(responseMessage);
        }
    }
}
