using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;

namespace HelloAzureFunctions
{
    public static class MyHttpTrigger
    {
        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }

    public static class MyHttpTrigger2
    {
        [FunctionName("MyHttpTrigger2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // access headers
            log.LogInformation("host: '{0}'", req.GetTypedHeaders().Host);
            log.LogInformation("blub: '{0}'", req.Headers["blub"]);

            // Creating the result
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = System.Net.HttpStatusCode.BadRequest;

            // setting custom headers
            return new ResponseMessageResult(httpResponseMessage);

        }
    }

    public static class OkObjectResultWithHeader{

    }
}
