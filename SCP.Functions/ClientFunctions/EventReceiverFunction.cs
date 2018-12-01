using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCP.Core;
using SCP.Core.DTO;

namespace SCP.Functions.ClientFunctions
{
    public static class EventReceiverFunction
    {
        [FunctionName(Constants.EventReceiverClientFunction)]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            var queryStrings = req.RequestUri.ParseQueryString();
            var instanceId = queryStrings["instanceId"];
            var eventName = queryStrings["eventName"];

            log.LogInformation($"EventReceiver --> Event {eventName} received for instance {instanceId}!");

            await starter.RaiseEventAsync(instanceId, eventName, true);

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
