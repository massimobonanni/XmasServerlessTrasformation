using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SCP.Core;
using SCP.Core.DTO;

namespace SCP.Functions.Functions
{
    public static class EvaluateChildFunction
    {
        [FunctionName(Constants.EvaluateChildFunction)]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "EvaluateChild")]HttpRequest req,
            [Table(TableNames.ChildrenTable, "Children", Connection = "StorageAccount")] CloudTable childTable,
            ILogger log)
        {
            log.LogDebug($"[START FUNCTION] --> {Constants.RetrieveChildActivityFunction}");

            string jsonContent = await new StreamReader(req.Body).ReadToEndAsync();

            ChildEvaluationDto child = null;
            try
            {
                child = JsonConvert.DeserializeObject<ChildEvaluationDto>(jsonContent);
                log.LogInformation($"{Constants.RetrieveChildActivityFunction} for child {child}");

                var childRow = await childTable.GetChildByIdAsync(child.ChildId);
                if (childRow == null)
                {
                    log.LogDebug($"{Constants.RetrieveChildActivityFunction} child {child} not found");
                    childRow = new ChildRow(child.ChildId);
                    childRow.FirstName = child.ChildFirstName;
                    childRow.LastName = child.ChildLastName;
                }

                childRow.Goodness = child.Goodness;

                await childTable.InsertOrReplaceAsync(childRow);
            }
            catch (Exception ex)
            {
                log.LogError("Error during child evaluation operation", ex);
                return new BadRequestObjectResult("Error during child evaluation operation");
            }

            return new OkResult();

        }
    }
}
