using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SCP.Core;
using SCP.Core.DTO;
using SendGrid.Helpers.Mail;

namespace SCP.Functions.ActivityFunctions
{
    public static class SendMailToSupportFunction
    {
        [FunctionName(Constants.SendMailToSupportActivityFunction)]
        public static bool SendMailToCustomer([ActivityTrigger] DurableActivityContext inputs,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
            IBinder invoiceBinder,
            ILogger log,
            ExecutionContext context)
        {
            (string orchestratoId, ChildDto child) payload = inputs.GetInput<(string, ChildDto)>();

            log.LogInformation($"[START ACTIVITY] --> SendMailToCustomerDurable for child: {payload.child}");

            var supportEmail = context.GetValue("SupportEmail");
            if (string.IsNullOrWhiteSpace(supportEmail))
                throw new Exception();
            var eventReceiverUrl = context.GetValue("ChildEventReceiverUri");
            if (string.IsNullOrWhiteSpace(eventReceiverUrl))
                throw new Exception();

            message = CreateMailMessage(payload.child, payload.orchestratoId, supportEmail, eventReceiverUrl);

            return true;
        }

        private static SendGridMessage CreateMailMessage(ChildDto child, string orchestratorId, string supportEmail, string eventReceiverUrl)
        {
            var message = new SendGridMessage()
            {
                Subject = $"SCP - Alert for {child.ChildFirstName } {child.ChildLastName}",
                From = new EmailAddress("noreply@scp.net")
            };

            message.AddTo(new EmailAddress(supportEmail));

            var strBuilder = new StringBuilder();
            strBuilder.AppendLine(
                $"We received a gift request from {child.ChildFirstName} {child.ChildLastName} [{child.ChildId}].");
            strBuilder.AppendLine();
            strBuilder.AppendLine(
                $"The requested gift is: {child.CurrentLetter.GiftName } {child.CurrentLetter.GiftBrand }");
            strBuilder.AppendLine();
            strBuilder.AppendLine(
                $"The coefficient of goodness of {child.ChildFirstName } is {child.Goodness}.");
            strBuilder.AppendLine();
            strBuilder.AppendLine();
            strBuilder.AppendLine(
                $"To approve the request: {CreateEventReceiverUrl(eventReceiverUrl,orchestratorId,Constants.GiftApprovedEvent)}");
            strBuilder.AppendLine();
            strBuilder.AppendLine(
                $"To decline the request: {CreateEventReceiverUrl(eventReceiverUrl,orchestratorId,Constants.GiftRejectedEvent)}");
            strBuilder.AppendLine();

            message.AddContent("text/plain", strBuilder.ToString());

            return message;
        }

        private static string CreateEventReceiverUrl(string eventReceiverUrl,string orchestratorId,string eventName)
        {
            if (eventReceiverUrl.Contains("code"))
            {
                return $"{eventReceiverUrl}&instanceId={orchestratorId}&eventName={Constants.GiftApprovedEvent}";
            }
            else
            {
                return $"{eventReceiverUrl}?instanceId={orchestratorId}&eventName={Constants.GiftApprovedEvent}";
            }
        }
    }
}
