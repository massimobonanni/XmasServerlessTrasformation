using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SCP.Core;
using SCP.Core.DTO;

namespace SCP.Functions.OrchestratorFunctions
{
    public static class LetterManagerFunction
    {
        [FunctionName(Constants.LetterManagerOrchestratorFunction)]
        public static async Task ManageLetter([OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log)
        {
            log.LogInformation($"[START ORCHESTRATOR] --> LetterManager!");
            var letter = context.GetInput<LetterDto>();

            ChildDto child = await context.CallActivityWithRetryAsync<ChildDto>(Constants.RetrieveChildActivityFunction,
                new RetryOptions(TimeSpan.FromSeconds(1), 10), letter);

            if (child == null)
                return;

            child.CurrentLetter = letter;

            if (child.Goodness >= 5)
            {
                await context.CallActivityAsync(Constants.OrderGiftActivityFunction, child);
            }
            else
            {
                await context.CallActivityAsync(Constants.SendMailToSupportActivityFunction, (context.InstanceId, child));
                var approvedGiftEvent = context.WaitForExternalEvent<bool>(Constants.GiftApprovedEvent);
                var rejectedGiftEvent = context.WaitForExternalEvent<bool>(Constants.GiftRejectedEvent);

                var eventFired = await Task.WhenAny(approvedGiftEvent, rejectedGiftEvent);
                if (eventFired == approvedGiftEvent)
                {
                    await context.CallActivityAsync(Constants.OrderGiftActivityFunction, child);
                }
                else if (eventFired == rejectedGiftEvent)
                {
                    await context.CallActivityAsync(Constants.RetrieveChildActivityFunction, child);
                }
            }

        }
    }
}
