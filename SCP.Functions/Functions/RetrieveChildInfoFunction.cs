using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using SCP.Core;
using SCP.Core.DTO;
using SCP.Functions.Extensions;

namespace SCP.Functions.Functions
{
    public static class RetrieveChildInfoFunction
    {
        [FunctionName(Constants.RetrieveChildrenInfoFunction)]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Children")]HttpRequest req,
            [Table("childrenTable", "Children", Connection = "StorageAccount")] CloudTable childTable,
            ILogger log)
        {
            log.LogDebug($"[START FUNCTION] --> {Constants.RetrieveChildrenInfoFunction}");

            if (!req.Query.ContainsKey("idChild"))
            {
                return new BadRequestResult();
            }
            string idChild = req.Query["idChild"];

            var children = new List<ChildDto>();
            try
            {
                var childRow = await childTable.GetChildByIdAsync(idChild);
                if (childRow == null)
                {
                    log.LogDebug($"{Constants.RetrieveChildActivityFunction} child {idChild} not found");
                    return new NotFoundResult();
                }

                children.Add(childRow.ToChildDto());
            }
            catch (Exception ex)
            {
                log.LogError("Error during retriving children", ex);
                return new InternalServerErrorResult();
            }

            return new OkObjectResult(children);

        }
    }
}
