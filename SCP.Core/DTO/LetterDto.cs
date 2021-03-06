﻿using System;
using Newtonsoft.Json;

namespace SCP.Core.DTO
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

        [JsonProperty("sentTimestamp")] public DateTime SentTimestamp { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(ChildFirstName)}={ChildFirstName}, {nameof(ChildLastName)}={ChildLastName}, {nameof(GiftName)}={GiftName}, {nameof(GiftBrand)}={GiftBrand}, ";
        }
    }
}
