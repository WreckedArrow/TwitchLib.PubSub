using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    public class AdsEvent : MessageData
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AdsEventType Type;

        [JsonProperty("data")]
        public AdsData Data;

        public class AdsData
        {
            [JsonProperty("jitter_buckets")]
            public int JitterBuckets;

            [JsonProperty("jitter_time")]
            public int JitterTime;

            [JsonProperty("warmup_time")]
            public int WarmupTime;

            [JsonProperty("commercial_id")]
            public string CommercialId;

            [JsonProperty("weighted_buckets")]
            public List<int> WeightedBuckets;
        }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public AdsEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            Type = Helpers.ToEnum<AdsEventType>(json.SelectToken("type").ToString());
            Data = json.SelectToken("data").ToObject<AdsData>();
        }
    }
}
