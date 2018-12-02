using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace SCP.Functions.Functions
{
    public static class ReportGenerator
    {
        [FunctionName("ReportGenerator")]
        public static async Task Run([TimerTrigger("%RepoterScheduleTriggerTime%")]TimerInfo myTimer,
            [Table("giftTable", "Gifts", Connection = "StorageAccount")] CloudTable giftTable,
            IBinder reportBinder,
            ILogger log)
        {
            log.LogInformation($"ReportGenerator executed at: {DateTime.Now}");

            var giftsToOrder = await giftTable.GetGiftsToOrderAsync();

            if (giftsToOrder.Any())
            {
                var reportDate = DateTime.Now;

                using (var reportBlob = reportBinder.Bind<TextWriter>(new BlobAttribute($"reports/{reportDate:yyyyMMddHHmmss}.rep")))
                {
                    reportBlob.WriteLine($"ChildId;Gift Name;Gift Brand");

                    foreach (var gift in giftsToOrder)
                    {
                        log.LogInformation($"{gift}");
                        reportBlob.WriteLine($"{gift.ChildId};{gift.GiftName};{gift.GiftBrand}");
                        gift.IsOrdered = true;
                    }
                }

                await giftTable.OrderGiftsAsync(giftsToOrder);
            }
            else
            {
                log.LogWarning($"Nothing to report!");
            }

        }
    }
}
