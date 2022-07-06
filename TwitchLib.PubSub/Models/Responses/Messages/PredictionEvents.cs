using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    public class PredictionEvents : MessageData
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PredictionType Type;

        [JsonProperty("data")]
        public PredictionData Data;

        public class UserInfo
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("user_id")]
            public string UserId;

            [JsonProperty("user_display_name")]
            public string UserDisplayName;

            [JsonProperty("extension_client_id")]
            public string ExtensionClientId;
        }
        public class PredictionResult
        {
            [JsonProperty("type")]
            public string Type;

            [JsonProperty("points_won")]
            public int? PointsWon;

            [JsonProperty("is_acknowledged")]
            public bool IsAcknowledged;
        }

        public class TopPredictor
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("event_id")]
            public string EventId;

            [JsonProperty("outcome_id")]
            public string OutcomeId;

            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("points")]
            public int Points;

            [JsonProperty("predicted_at")]
            public string PredictedAt;

            [JsonProperty("updated_at")]
            public string UpdatedAt;

            [JsonProperty("user_id")]
            public string UserId;

            [JsonProperty("result")]
            public PredictionResult Result;

            [JsonProperty("user_display_name")]
            public string UserDisplayName;
        }

        public class Badge
        {
            [JsonProperty("version")]
            public string Version;

            [JsonProperty("set_id")]
            public string SetId;
        }

        public class Outcome
        {
            [JsonProperty("id")]
            public Guid Id;

            [JsonProperty("color")]
            public string Color;

            [JsonProperty("title")]
            public string Title;

            [JsonProperty("total_points")]
            public long TotalPoints;

            [JsonProperty("total_users")]
            public long TotalUsers;

            [JsonProperty("top_predictors")]
            public List<TopPredictor> TopPredictors;

            [JsonProperty("badge")]
            public Badge Badge;
        }

        public class Event
        {
            [JsonProperty("id")]
            public Guid Id;

            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("created_at")]
            public DateTime? CreatedAt;

            [JsonProperty("created_by")]
            public UserInfo CreatedBy;

            [JsonProperty("ended_at")]
            public DateTime? EndedAt;

            [JsonProperty("ended_by")]
            public UserInfo EndedBy;

            [JsonProperty("locked_at")]
            public DateTime? LockedAt;

            [JsonProperty("locked_by")]
            public UserInfo LockedBy;

            [JsonProperty("outcomes")]
            public List<Outcome> Outcomes;

            [JsonProperty("prediction_window_seconds")]
            public int PredictionWindowSeconds;

            [JsonProperty("status")]
            [JsonConverter(typeof(StringEnumConverter))]
            public PredictionStatus Status;

            [JsonProperty("title")]
            public string Title;

            [JsonProperty("winning_outcome_id")]
            public Guid? WinningOutcomeId;
        }

        public class PredictionData
        {
            [JsonProperty("timestamp")]
            public string Timestamp;

            [JsonProperty("event")]
            public Event Event;
        }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public PredictionEvents(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            Type = Helpers.ToEnum<PredictionType>(json.SelectToken("type").ToString());
            Data = json.SelectToken("data").ToObject<PredictionData>();
        }
    }
}
