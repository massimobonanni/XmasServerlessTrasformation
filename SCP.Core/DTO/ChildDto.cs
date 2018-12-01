namespace SCP.Core.DTO
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
