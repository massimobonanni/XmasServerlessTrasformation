using System;
using System.Collections.Generic;
using System.Text;
using SCP.Core.DTO;

namespace SCP.Functions.Extensions
{
    public static class DtoExtensions
    {
        public static ChildDto ToChildDto(this ChildRow childRow)
        {
            return new ChildDto()
            {
                ChildId = childRow.ChildId,
                ChildFirstName = childRow.FirstName,
                ChildLastName = childRow.LastName,
                Goodness = childRow.Goodness
            };
        }
    }
}
