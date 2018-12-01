using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using SCP.Core;
using SCP.Core.DTO;
using TableAttribute = Microsoft.Azure.WebJobs.TableAttribute;

namespace SCP.Functions.ActivityFunctions
{
    public static class RetrieveChildFunction
    {
        [FunctionName(Constants.RetrieveChildActivityFunction)]
        public static async Task<ChildDto> RetrieveChild([ActivityTrigger] LetterDto letter,
            [Table("childrenTable", "Children", Connection = "StorageAccount")] CloudTable childTable,
            ILogger log)
        {
            log.LogInformation($"[START ACTIVITY] --> {Constants.RetrieveChildActivityFunction} for childId={letter.ChildId}");
            ChildDto child = null;
            try
            {
                var childRow = await childTable.GetChildByIdAsync(letter.ChildId);
                if (childRow == null)
                {
                    childRow = new ChildRow(letter.ChildId);
                    childRow.FirstName = letter.ChildFirstName;
                    childRow.LastName = letter.ChildLastName;
                    childRow.Goodness = 10;
                    if (!await childTable.InsertAsync(childRow))
                        childRow = null;
                }

                if (childRow != null)
                {
                    child = new ChildDto();
                    child.ChildFirstName = childRow.FirstName;
                    child.ChildId = childRow.ChildId;
                    child.ChildLastName = childRow.LastName;
                    child.CurrentLetter = letter;
                    child.Goodness = childRow.Goodness;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Error during retriving child");
                return null;
            }

            return child;
        }
    }
}
