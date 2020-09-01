#r "Newtonsoft.Json"
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
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