using Newtonsoft.Json;

namespace SCP.Core.DTO
{
    public class ChildEvaluationDto
    {
        [JsonProperty("childId")]
        public string ChildId { get; set; }
        [JsonProperty("firstName")]
        public string ChildFirstName { get; set; }
        [JsonProperty("lastName")]
        public string ChildLastName { get; set; }
        [JsonProperty("goodness")]
        public int Goodness { get; set; }

        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(ChildFirstName)}={ChildFirstName}, {nameof(ChildLastName)}={ChildLastName}, {nameof(Goodness)}={Goodness}";
        }
    }
}
