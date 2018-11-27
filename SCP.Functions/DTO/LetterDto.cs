using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCP.Functions.DTO
{
    public class LetterDto
    {
        [JsonProperty("childId")]
        public string ChildId { get; set; }
        [JsonProperty("childFirstName")]
        public string ChildFirstName { get; set; }
        [JsonProperty("childLastName")]
        public string ChildLastName { get; set; }
        [JsonProperty("giftName")]
        public string GiftName { get; set; }
        [JsonProperty("giftBrand")]
        public string GiftBrand { get; set; }
        [JsonProperty("sentTimestamp")]
        public DateTime SentTimestamp { get; set; }
    }
}
