namespace SCP.Core.DTO
{
    public class ChildDto
    {
        public string ChildId { get; set; }
        public string ChildFirstName { get; set; }
        public string ChildLastName { get; set; }
        public int Goodness { get; set; }

        public LetterDto CurrentLetter { get; set; }

        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(ChildFirstName)}={ChildFirstName}, {nameof(ChildLastName)}={ChildLastName}, {nameof(Goodness)}={Goodness}, {nameof(CurrentLetter)}=[{CurrentLetter}]";
        }
    }
}
