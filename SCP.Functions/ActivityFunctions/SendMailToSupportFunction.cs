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
        public static bool SendMailToCustomer([ActivityTrigger] ChildDto child,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message,
            IBinder invoiceBinder,
            ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> SendMailToCustomerDurable for order: {order.orderId}");
            log.LogInformation($"File Processed : {order.fileName}");
            log.LogInformation($"Order: {order}");
            log.LogInformation($"Customer mail: {order.custEmail}");
            using (var inputBlob = invoiceBinder.Bind<TextReader>(new BlobAttribute(order.fileName)))
            {
                message = CreateMailMessage(order, inputBlob);
            }
            return true;
        }


    }
}
