using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCP.Functions.DTO;

namespace SCP.Functions.ClientFunctions
{
    public static class LetterReceiverFunction
    {
        [FunctionName(Constants.LetterReceiverClientFunction)]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation($"LetterReceiver --> Letter received!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            string instanceId = null;
            LetterDto letter = null;
            try
            {
                letter = JsonConvert.DeserializeObject<LetterDto>(jsonContent);
                instanceId = await starter.StartNewAsync(Constants.LetterManagerOrchestratorFunction, letter);
                log.LogInformation($"Letter received - started orchestration with ID = '{instanceId}'.");
            }
            catch (Exception ex)
            {
                log.LogError("Error during letter received operation", ex);
            }

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
