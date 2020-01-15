using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;


namespace Cosmos_HttpTrigger_RestApi
{
    public static class GetAllMeasurementsByDateAndDevice
    {
        [FunctionName("GetAllMeasurementsByDateAndDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAllMeasurementsByDateAndDevice/{deviceId}/{date}")] HttpRequest req,
            string deviceId, string date,
            [CosmosDB("%IoTCosmosDbName%", "%IoTCosmosDbContainerName%", ConnectionStringSetting = "CosmosConnectionString", SqlQuery = "SELECT * FROM c where c.PartitionKey = CONCAT({date},'-',{deviceId})")] IEnumerable<object> documents,
            ILogger log)
        {
            log.LogInformation($"C# GetAllMeasurementsByDateAndDevice HTTP trigger function processed a request. {deviceId} ");
            var docs = documents.ToList();

            if (docs.Count() == 0)
                return (ActionResult)new NotFoundObjectResult($"C# GetAllMeasurementsByDateAndDevice HTTP trigger: No documents found for these parameters\n deviceId: {deviceId} \n date: {date}");
                   
            log.LogInformation("C# GetAllMeasurementsByDateAndDevice HTTP trigger function processed a request.\n Found documents: " + docs.Count);

            return (ActionResult)new OkObjectResult(docs);
        }
    }
}
