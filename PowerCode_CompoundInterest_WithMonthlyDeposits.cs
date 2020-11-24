using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace PowerCode_DepositInterest
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string inicialDeposit = req.Query["inicialDeposit"];
            string depositContribution = req.Query["depositContribution"];
            string contributions = req.Query["contributions"];
            string rate = req.Query["rate"];
            string years = req.Query["years"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            inicialDeposit = inicialDeposit ?? data?.inicialDeposit;
            depositContribution = depositContribution ?? data?.depositContribution;
            contributions = contributions ?? data?.contributions;
            rate = rate ?? data?.rate;
            years = years ?? data?.years;
            
            float ys = float.Parse(years);
            float id = float.Parse(inicialDeposit);
            float dc = float.Parse(depositContribution);
            float rt = float.Parse(rate);
            float ct = float.Parse(contributions);
            float td = 0;
            float ti = 0;

            for (int y=1;y<=ys; y++)
            {
                if (y < 2)
                {
                    td = id + ct * dc;
                    ti = rt / 100f * id;
                }
                else
                {
                    td = td + ct * dc;
                    ti = rt / 100f * id;
                }

                id += (rt / 100f * id)+(ct * dc);    
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Total after "+ years + " : " + id);
            sb.Append("| Total deposit : " + td);
            sb.Append("| Total interest  : " + ti);
            string s = sb.ToString();

            string responseMessage = string.IsNullOrEmpty(s.ToString())
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : s.ToString();
            return new OkObjectResult(responseMessage);
        }
    }
}
