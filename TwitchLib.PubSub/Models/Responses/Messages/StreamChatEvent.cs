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
    public class StreamChatEvent : MessageData
    {
        public struct StreamChatHostTargetChange
        {
            [JsonProperty(PropertyName = "channel_id")]
            public string ChannelId { get; internal set; }
            [JsonProperty(PropertyName = "channel_login")]
            public string ChannelLogin { get; internal set; }
            [JsonProperty(PropertyName = "target_channel_id")]
            public string TargetChannelId { get; internal set; }
            [JsonProperty(PropertyName = "target_channel_login")]
            public string TargetChannelLogin { get; internal set; }
            [JsonProperty(PropertyName = "previous_target_channel_id")]
            public string PreviousTargetChannelId { get; internal set; }
            [JsonProperty(PropertyName = "num_viewers")]
            public int NumViewers { get; internal set; }
        }
        public struct StreamChatRichEmbed
        {
            public struct EmbedTwitchMetaData
            {
                public struct EmbedClipMetaData
                {
                    [JsonProperty(PropertyName = "game")]
                    public string Game { get; internal set; }
                    [JsonProperty(PropertyName = "channel_display_name")]
                    public string ChannelDisplayName { get; internal set; }
                    [JsonProperty(PropertyName = "slug")]
                    public string Slug { get; internal set; }
                    [JsonProperty(PropertyName = "id")]
                    public string Id { get; internal set; }
                    [JsonProperty(PropertyName = "broadcaster_id")]
                    public string BroadcasterId { get; internal set; }
                    [JsonProperty(PropertyName = "curator_id")]
                    public string CuratorId { get; internal set; }
                }
                [JsonProperty(PropertyName = "clip_metadata")]
                public EmbedClipMetaData ClipMetaData { get; internal set; }
            }
            [JsonProperty(PropertyName = "message_id")]
            public string MessageId { get; internal set; }
            [JsonProperty(PropertyName = "request_url")]
            public string RequestUrl { get; internal set; }
            [JsonProperty(PropertyName = "author_name")]
            public string AuthorName { get; internal set; }
            [JsonProperty(PropertyName = "thumbnail_url")]
            public string ThumbnailUrl { get; internal set; }
            [JsonProperty(PropertyName = "title")]
            public string Title { get; internal set; }
            [JsonProperty(PropertyName = "twitch_metadata")]
            public EmbedTwitchMetaData TwitchMetadata { get; internal set; }
        }
        public class PartialVerificationConfig
        {
            [JsonProperty("restrict_first_time_chatters")]
            public bool RestrictFirstTimeChatters;

            [JsonProperty("restrict_based_on_follower_age")]
            public bool RestrictBasedOnFollowerAge;

            [JsonProperty("restrict_based_on_account_age")]
            public bool RestrictBasedOnAccountAge;

            [JsonProperty("minimum_follower_age_in_minutes")]
            public int MinimumFollowerAgeInMinutes;

            [JsonProperty("minimum_account_age_in_minutes")]
            public int MinimumAccountAgeInMinutes;
        }

        public class AccountVerificationOptions
        {
            [JsonProperty("subscribers_exempt")]
            public bool SubscribersExempt;

            [JsonProperty("moderators_exempt")]
            public bool ModeratorsExempt;

            [JsonProperty("vips_exempt")]
            public bool VipsExempt;

            [JsonProperty("phone_verification_mode")]
            public int PhoneVerificationMode;

            [JsonProperty("email_verification_mode")]
            public int EmailVerificationMode;

            [JsonProperty("partial_phone_verification_config")]
            public PartialVerificationConfig PartialPhoneVerificationConfig;

            [JsonProperty("partial_email_verification_config")]
            public PartialVerificationConfig PartialEmailVerificationConfig;
        }

        public class Modes
        {
            [JsonProperty("followers_only_duration_minutes")]
            public int? FollowersOnlyDurationMinutes;

            [JsonProperty("emote_only_mode_enabled")]
            public bool EmoteOnlyModeEnabled;

            [JsonProperty("r9k_mode_enabled")]
            public bool R9kModeEnabled;

            [JsonProperty("subscribers_only_mode_enabled")]
            public bool SubscribersOnlyModeEnabled;

            [JsonProperty("verified_only_mode_enabled")]
            public bool VerifiedOnlyModeEnabled;

            [JsonProperty("slow_mode_duration_seconds")]
            public int? SlowModeDurationSeconds;

            [JsonProperty("slow_mode_set_at")]
            public DateTime? SlowModeSetAt;

            [JsonProperty("account_verification_options")]
            public AccountVerificationOptions AccountVerificationOptions;

            [JsonProperty("rules")]
            public object Rules;
        }

        public class Room
        {
            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("modes")]
            public Modes Modes;
        }

        public class StreamChatUpdatedRoom
        {
            [JsonProperty("room")]
            public Room Room;
        }

        /// <summary>
        /// Prediction Type
        /// </summary>
        /// <value>The type</value>
        public StreamChatType Type { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public StreamChatHostTargetChange HostTargetChange { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public StreamChatRichEmbed ChatRichEmbed { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public StreamChatUpdatedRoom UpdatedRoomData { get; protected set; }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public StreamChatEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            var eventData = json;
            // var eventData = json.SelectToken("data.message");
            Type = Helpers.ToEnum<StreamChatType>(eventData.SelectToken("type").ToString());
            switch (Type)
            {
                case StreamChatType.HostTargetChangeV2:
                    {
                        HostTargetChange = eventData.SelectToken("data").ToObject<StreamChatHostTargetChange>();
                        //HostTargetChange = Helpers.DeserializeFromJson<StreamChatHostTargetChange>(eventData.ToString());
                        break;
                    }
                case StreamChatType.ChatRichEmbed:
                    {
                        ChatRichEmbed = eventData.SelectToken("data").ToObject<StreamChatRichEmbed>();
                        //ChatRichEmbed = Helpers.DeserializeFromJson<StreamChatRichEmbed>(eventData.ToString());
                        break;
                    }
                case StreamChatType.UpdatedRoom:
                    {
                        UpdatedRoomData = eventData.SelectToken("data").ToObject<StreamChatUpdatedRoom>();
                        //ChatRichEmbed = Helpers.DeserializeFromJson<StreamChatRichEmbed>(eventData.ToString());
                        break;
                    }
            }
            // ChannelLogin = eventData.SelectToken("channel_login").ToString();
            // TargetChannelId = eventData.SelectToken("target_channel_id").ToString();
            // ChannelLogin = eventData.SelectToken("target_channel_login").ToString();
            // TargetChannelId = eventData.SelectToken("previous_target_channel_id").ToString();
            // NumViewers = int.Parse(eventData.SelectToken("num_viewers").ToString());
        }
    }
}
