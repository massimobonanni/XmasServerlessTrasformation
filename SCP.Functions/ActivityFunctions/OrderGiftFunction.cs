using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using SCP.Core;
using SCP.Core.DTO;

namespace SCP.Functions.ActivityFunctions
{
    public static class OrderGiftFunction
    {
        [FunctionName(Constants.OrderGiftActivityFunction)]
        public static async Task<bool> OrderGift([ActivityTrigger] ChildDto child,
            [Table(TableNames.GiftsTable, "Gifts", Connection = "StorageAccount")] CloudTable giftTable,
            ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> {Constants.OrderGiftActivityFunction} for {child.ChildId}");
            bool retVal = false;
            if (child.CurrentLetter != null)
            {
                try
                {
                    var gift = new GiftRow(child.ChildId);
                    gift.GiftBrand = child.CurrentLetter.GiftBrand;
                    gift.GiftName = child.CurrentLetter.GiftName;
                    gift.IsOrdered = false;
                    retVal = await giftTable.InsertAsync(gift);
                }
                catch (Exception ex)
                {
                    log.LogError(ex, $"Error during Order Gift ");
                    retVal = false;
                }
            }
            return retVal;
        }
    }
}
