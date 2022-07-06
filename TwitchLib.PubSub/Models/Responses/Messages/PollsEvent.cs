using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Extensions;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    /// <summary>
    /// Predictions model constructor.
    /// Implements the <see cref="MessageData" />
    /// </summary>
    /// <seealso cref="MessageData" />
    /// <inheritdoc />
    public class PollsEvent : MessageData
    {
        public struct PollSettings
        {
            public struct Options
            {
                [JsonProperty(PropertyName = "is_enabled")]
                public bool IsEnabled { get; internal set; }
                [JsonProperty(PropertyName = "cost")]
                public int? Cost { get; internal set; }
            }
            [JsonProperty(PropertyName = "multi_choice")]
            public Options MultiChoice { get; internal set; }
            [JsonProperty(PropertyName = "subscriber_only")]
            public Options SubscriberOnly { get; internal set; }
            [JsonProperty(PropertyName = "subscriber_multiplier")]
            public Options SubscriberMultiplier { get; internal set; }
            [JsonProperty(PropertyName = "bits_votes")]
            public Options BitsVotes { get; internal set; }
            [JsonProperty(PropertyName = "channel_points_votes")]
            public Options ChannelPointsVotes { get; internal set; }
        }
        public struct PollVotes
        {
            [JsonProperty(PropertyName = "total")]
            public int Total { get; internal set; }
            [JsonProperty(PropertyName = "bits")]
            public int Bits { get; internal set; }
            [JsonProperty(PropertyName = "channel_points")]
            public int ChannelPoints { get; internal set; }
            [JsonProperty(PropertyName = "base")]
            public int Base { get; internal set; }
        }
        public struct VoteTokens
        {
            [JsonProperty(PropertyName = "bits")]
            public int Bits { get; internal set; }
            [JsonProperty(PropertyName = "channel_points")]
            public int ChannelPoints { get; internal set; }
        }
        public struct PollChoice
        {
            [JsonProperty(PropertyName = "choice_id")]
            public Guid ChoiceId { get; internal set; }
            [JsonProperty(PropertyName = "title")]
            public string Title { get; internal set; }
            [JsonProperty(PropertyName = "votes")]
            public PollVotes Votes { get; internal set; }
            [JsonProperty(PropertyName = "tokens")]
            public VoteTokens Tokens { get; internal set; }
            [JsonProperty(PropertyName = "total_voters")]
            public int TotalVoters { get; internal set; }
        }
        /// <summary>
        /// Prediction Type
        /// </summary>
        /// <value>The type</value>
        public PollsEventType Type { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public Guid PollId { get; protected set; }
        /// <summary>
        /// Owned By - Channel Id
        /// </summary>
        /// <value>The channel id?</value>
        public string OwnedBy { get; protected set; }
        /// <summary>
        /// CreatedBy User Id
        /// </summary>
        /// <value>The user id of the user who created the poll.</value>
        public string CreatedBy { get; protected set; }
        /// <summary>
        /// Ended By User Id
        /// </summary>
        /// <value>The user id of the user who created the poll.</value>
        public string EndedBy { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public DateTime StartedAt { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public DateTime? EndedAt { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string Title { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public int DurationSeconds { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public PollSettings Settings { get; protected set; }
        /// <summary>
        /// Prediction time
        /// </summary>
        /// <value>The seconds the prediction runs, starts from <see cref="CreatedAt"/></value>
        public PollStatus Status { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public PollChoice[] Choices { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public PollVotes Votes { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public VoteTokens Tokens { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public int TotalVoters { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public int RemainingDurationMilliseconds { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string TopContributor { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string TopBitsContributor { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string TopChannelPointsContributor { get; protected set; }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public PollsEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            var eventData = json;
            // var eventData = json.SelectToken("data.message");
            Type = Helpers.ToEnum<PollsEventType>(eventData.SelectToken("type").ToString());

            var eventPollData = eventData.SelectToken("data.poll");
            PollId = Guid.Parse(eventPollData.SelectToken("poll_id").ToString());
            Title = eventPollData.SelectToken("title").ToString();
            Status = Helpers.ToEnum<PollStatus>(eventPollData.SelectToken("status").ToString());
            OwnedBy = eventPollData.SelectToken("owned_by").ToString();
            CreatedBy = eventPollData.SelectToken("created_by").ToString();
            StartedAt = DateTime.Parse(eventPollData.SelectToken("started_at").ToString());
            EndedBy = eventPollData.SelectToken("ended_by").ToString();
            var endedAtStr = eventPollData.SelectToken("ended_at").ToString();
            EndedAt = String.IsNullOrEmpty(endedAtStr) ? (DateTime?)null : DateTime.Parse(endedAtStr);

            Settings = eventPollData.SelectToken("settings").ToObject<PollSettings>();
            Choices = eventPollData.SelectToken("choices").ToObject<PollChoice[]>();
            Votes = eventPollData.SelectToken("votes").ToObject<PollVotes>();
            Tokens = eventPollData.SelectToken("tokens").ToObject<VoteTokens>();
            // Settings = Helpers.DeserializeFromJson<PollSettings>(eventPollData.SelectToken("settings").ToString());
            // Choices = Helpers.DeserializeFromJson<PollChoice[]>(eventPollData.SelectToken("choices").ToString());
            // Votes = Helpers.DeserializeFromJson<PollVotes>(eventPollData.SelectToken("votes").ToString());
            // Tokens = Helpers.DeserializeFromJson<VoteTokens>(eventPollData.SelectToken("tokens").ToString());
            TotalVoters = int.Parse(eventPollData.SelectToken("total_voters").ToString());
            RemainingDurationMilliseconds = int.Parse(eventPollData.SelectToken("remaining_duration_milliseconds").ToString());
            TopContributor = eventPollData.SelectToken("top_contributor").ToString();
            TopBitsContributor = eventPollData.SelectToken("top_bits_contributor").ToString();
            TopChannelPointsContributor = eventPollData.SelectToken("top_channel_points_contributor").ToString();
        }
    }
}
