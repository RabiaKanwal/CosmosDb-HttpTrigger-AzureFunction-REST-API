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
using System.Linq;

namespace Cosmos_HttpTrigger_RestApi
{
    public static class GetSpecificMeasurementsByDateAndDevice
    {
        [FunctionName("GetSpecificMeasurementsByDateAndDevice")]
        public static async Task<IActionResult> Run(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetSpecificMeasurementsByDateAndDevice/{deviceId}/{date}/{sensorType}")] HttpRequest req,
            string deviceId, string date, string sensorType,
            [CosmosDB("%IoTCosmosDbName%", "%IoTCosmosDbContainerName%", ConnectionStringSetting = "CosmosConnectionString", SqlQuery = "SELECT * FROM c where c.PartitionKey = CONCAT({date},'-',{deviceId}) and c.MeasurementType = {sensorType}")] IEnumerable<object> documents,
            ILogger log)
        {
            log.LogInformation($"C# GetSpecificMeasurementsByDateAndDevice HTTP trigger function processed a request. {deviceId} - SensorType: {sensorType}");
            var docs = documents.ToList();

            if (docs.Count() == 0)
                return (ActionResult)new NotFoundObjectResult($"C# GetSpecificMeasurementsByDateAndDevice HTTP trigger: No documents found for these parameters\n deviceId: {deviceId} \n date: {date}");

            log.LogInformation("C# GetSpecificMeasurementsByDateAndDevice HTTP trigger function processed a request.\n Found documents: " + docs.Count);

            return (ActionResult)new OkObjectResult(docs);
        }
    }
}
