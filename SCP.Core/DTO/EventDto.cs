using Newtonsoft.Json;

namespace SCP.Core.DTO
{
    public class EventDto
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }
        [JsonProperty("eventName")]
        public string EventName { get; set; }
    }
}
