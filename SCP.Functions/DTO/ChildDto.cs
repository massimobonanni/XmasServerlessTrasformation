using System;
using System.Collections.Generic;
using System.Text;

namespace SCP.Functions.DTO
{
    public class ChildDto
    {
        public string ChildId { get; set; }
        public string ChildFirstName { get; set; }
        public string ChildLastName { get; set; }
        public int GoodnessCoefficient { get; set; }

        public LetterDto CurrentLetter { get; set; }
    }
}
