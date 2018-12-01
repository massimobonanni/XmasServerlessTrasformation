using Newtonsoft.Json;

namespace SCP.Core.DTO
{
    public class ChildEvaluationDto
    {
        [JsonProperty("childId")]
        public string ChildId { get; set; }
        [JsonProperty("goodness")]
        public int Goodness { get; set; }

        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(Goodness)}={Goodness}";
        }
    }
}
