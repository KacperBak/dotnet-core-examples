using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-function-linux-custom-image
/// </summary>
namespace HelloAzureFunctions
{
    public static class MyHttpTrigger
    {
        /// <summary>
        /// Auto generated method by 'func new --name MyHttpTrigger --template "HttpTrigger"'
        /// </summary>
        /// <param name="[HttpTrigger(AuthorizationLevel.Function"></param>
        /// <param name="null"></param>
        /// <returns></returns>
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

    public static class HeaderNames
    {
        public const string CustomStaticHeader = "custom_static_header";
        public const string CustomDynamicHeader = "custom_dynamic_header";
    }


    public static class MyHttpTrigger2
    {
        /// <summary>
        /// Playground endpoint
        /// </summary>
        /// <param name="[HttpTrigger(AuthorizationLevel.Anonymous">public available</param>
        /// <param name="null"></param>
        /// <returns></returns>
        [FunctionName("MyHttpTrigger2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            // access headers
            if (req.Headers.ContainsKey(HeaderNames.CustomStaticHeader)){
                log.LogInformation($"{HeaderNames.CustomStaticHeader}: '{0}'", req.Headers[$"{HeaderNames.CustomStaticHeader}"]);
            }

            // create custom dynamic headers
            Dictionary<string, string> customHeaders = new Dictionary<string, string>();
            customHeaders.Add($"{HeaderNames.CustomDynamicHeader}_{DateTime.Now.Millisecond}", $"{DateTime.Now.Millisecond}");

            // Creating the result
            return (ActionResult)new CustomObjectResult(customHeaders, null);
        }
    }
}
