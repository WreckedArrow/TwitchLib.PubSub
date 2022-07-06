using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public class HypeTrainEvent : MessageData
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HypeTrainEventType Type;
        public string ChannelId;
        public HypeTrainApproaching Approaching;
        public HypeTrainStart Start;
        public HypeTrainProgression Progression;
        public HypeTrainConductorUpdate ConductorUpdate;
        public HypeTrainLevelUp LevelUp;
        public HypeTrainEnd End;

        public class HypeTrainApproaching
        {
            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("goal")]
            public int Goal;

            [JsonProperty("events_remaining_durations")]
            public Dictionary<string, int> EventsRemainingDurations;

            [JsonProperty("level_one_rewards")]
            public Reward[] LevelOneRewards;

            [JsonProperty("creator_color")]
            public string CreatorColor;

            [JsonProperty("participants")]
            public string[] Participants;

            [JsonProperty("approaching_hype_train_id")]
            public Guid ApproachingHypeTrainId;

            [JsonProperty("is_boost_train")]
            public bool IsBoostTrain;
        }

        public class HypeTrainStart
        {
            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("id")]
            public Guid Id;

            [JsonProperty("started_at")]
            public long StartedAt;

            [JsonProperty("expires_at")]
            public long ExpiresAt;

            [JsonProperty("updated_at")]
            public long UpdatedAt;

            [JsonProperty("ended_at")]
            public long? EndedAt;

            [JsonProperty("ending_reason")]
            public string EndingReason;

            [JsonProperty("config")]
            public Config Config;
        }
        public class HypeTrainProgression
        {
            [JsonProperty("user_id")]
            public string UserId;

            [JsonProperty("user_login")]
            public string UserLogin;

            [JsonProperty("user_display_name")]
            public string UserDisplayName;

            [JsonProperty("user_profile_image_url")]
            public string UserProfileImageUrl;

            [JsonProperty("sequence_id")]
            public int SequenceId;

            [JsonProperty("action")]
            public string Action;

            [JsonProperty("source")]
            public string Source;

            [JsonProperty("quantity")]
            public int Quantity;

            [JsonProperty("progress")]
            public Progress Progress;

            [JsonProperty("is_boost_train")]
            public bool IsBoostTrain;
        }
        public class HypeTrainConductorUpdate
        {
            [JsonProperty("source")]
            public string Source;

            [JsonProperty("user")]
            public Conductor User;

            [JsonProperty("participations")]
            public BitsSubsRates Participations;
        }

        public class HypeTrainLevelUp
        {
            [JsonProperty("time_to_expire")]
            public long TimeToExpire;

            [JsonProperty("progress")]
            public Progress Progress;

            [JsonProperty("is_boost_train")]
            public bool IsBoostTrain;
        }
        public class HypeTrainEnd
        {
            [JsonProperty("ended_at")]
            public long EndedAt;

            [JsonProperty("ending_reason")]
            public string EndingReason;

            [JsonProperty("is_boost_train")]
            public bool IsBoostTrain;
        }

        public class Config
        {
            [JsonProperty("channel_id")]
            public string ChannelId;

            [JsonProperty("is_enabled")]
            public bool IsEnabled;

            [JsonProperty("is_whitelisted")]
            public bool IsWhitelisted;

            [JsonProperty("kickoff")]
            public Kickoff Kickoff;

            [JsonProperty("cooldown_duration")]
            public long CooldownDuration;

            [JsonProperty("level_duration")]
            public long LevelDuration;

            [JsonProperty("difficulty")]
            public HypeTrainDifficulty Difficulty;

            [JsonProperty("reward_end_date")]
            public DateTime? RewardEndDate;

            [JsonProperty("participation_conversion_rates")]
            public BitsSubsRates ParticipationConversionRates;

            [JsonProperty("notification_thresholds")]
            public BitsSubsRates NotificationThresholds;

            [JsonProperty("difficulty_settings")]
            public Dictionary<HypeTrainDifficulty, Level[]> DifficultySettings;

            [JsonProperty("conductor_rewards")]
            public ConductorRewards ConductorRewards;
        }

        public class ConductorRewards
        {
            [JsonProperty("BITS")]
            public ConductorRewardGroup Bits;

            [JsonProperty("SUBS")]
            public ConductorRewardGroup Subs;

            [JsonProperty("callout_emote_id")]
            public string CalloutEmoteId;

            [JsonProperty("callout_emote_token")]
            public string CalloutEmoteToken;

            [JsonProperty("use_creator_color")]
            public bool UseCreatorColor;

            [JsonProperty("primary_hex_color")]
            public string PrimaryHexColor;

            [JsonProperty("use_personalized_settings")]
            public bool UsePersonalizedSettings;

            [JsonProperty("has_conductor_badges")]
            public bool HasConductorBadges;

            [JsonProperty("boost_train_config")]
            public BoostTrainConfig BoostTrainConfig;

            [JsonProperty("participations")]
            public BitsSubsRates Participations;

            [JsonProperty("conductors")]
            public Dictionary<string, Conductor[]> Conductors;

            [JsonProperty("progress")]
            public Progress Progress;

            [JsonProperty("is_boost_train")]
            public bool IsBoostTrain;
        }
        public class BoostTrainConfig
        {
            [JsonProperty("twitch_impressions")]
            public TwitchImpressions TwitchImpressions;
        }
        public class TwitchImpressions
        {
            [JsonProperty("EASY")]
            public int Easy;

            [JsonProperty("MEDIUM")]
            public int Medium;

            [JsonProperty("HARD")]
            public int Hard;

            [JsonProperty("SUPER HARD")]
            public int SuperHard;

            [JsonProperty("INSANE")]
            public int Insane;
        }

        public struct ConductorRewardGroup
        {
            [JsonProperty("CURRENT")]
            public Reward[] Current;
            [JsonProperty("FORMER")]
            public Reward[] Former;
        }
        public class Kickoff
        {
            [JsonProperty("num_of_events")]
            public int NumOfEvents;

            [JsonProperty("min_points")]
            public int MinPoints;

            [JsonProperty("duration")]
            public long Duration;
        }

        public class Conductor
        {
            [JsonProperty("id")]
            public string Id;

            [JsonProperty("login")]
            public string Login;

            [JsonProperty("display_name")]
            public string DisplayName;

            [JsonProperty("profile_image_url")]
            public string ProfileImageUrl;
        }

        public class Reward
        {
            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public HypeTrainRewardType Type;

            [JsonProperty("id")]
            public string Id;

            [JsonProperty("group_id")]
            public string GroupId;

            [JsonProperty("reward_level")]
            public int RewardLevel;

            [JsonProperty("set_id")]
            public Guid? SetId;

            [JsonProperty("badge_id")]
            public string BadgeId;

            [JsonProperty("token")]
            public string Token;

            [JsonProperty("image_url")]
            public string ImageUrl;
        }

        public class Level
        {
            [JsonProperty("value")]
            public int Value;

            [JsonProperty("goal")]
            public int Goal;

            [JsonProperty("rewards")]
            public Reward[] Rewards;

            [JsonProperty("impressions")]
            public int Impressions;
        }

        public class Progress
        {
            [JsonProperty("level")]
            public Level Level;

            [JsonProperty("value")]
            public int Value;

            [JsonProperty("goal")]
            public int Goal;

            [JsonProperty("total")]
            public int Total;

            [JsonProperty("remaining_seconds")]
            public int RemainingSeconds;
        }

        public struct BitsSubsRates
        {
            [JsonProperty("BITS.CHEER")]
            public int? BitsCheer { get; internal set; }
            [JsonProperty("BITS.EXTENSION")]
            public int? BitsExtension { get; internal set; }
            [JsonProperty("BITS.POLL")]
            public int? BitsPoll { get; internal set; }
            [JsonProperty("SUBS.TIER_1_GIFTED_SUB")]
            public int? SubsTier1GiftedSub { get; internal set; }
            [JsonProperty("SUBS.TIER_1_SUB")]
            public int? SubsTier1Sub { get; internal set; }
            [JsonProperty("SUBS.TIER_2_GIFTED_SUB")]
            public int? SubsTier2GiftedSub { get; internal set; }
            [JsonProperty("SUBS.TIER_2_SUB")]
            public int? SubsTier2Sub { get; internal set; }
            [JsonProperty("SUBS.TIER_3_GIFTED_SUB")]
            public int? SubsTier3GiftedSub { get; internal set; }
            [JsonProperty("SUBS.TIER_3_SUB")]
            public int? SubsTier3Sub { get; internal set; }
        }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public HypeTrainEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            Type = Helpers.ToEnum<HypeTrainEventType>(json.SelectToken("type").ToString());
            var eventTrainData = json.SelectToken("data");
            switch (Type)
            {
                case HypeTrainEventType.HypeTrainApproaching:
                    {
                        Approaching = eventTrainData.ToObject<HypeTrainApproaching>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainStart:
                    {
                        Start = eventTrainData.ToObject<HypeTrainStart>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainLevelUp:
                    {
                        LevelUp = eventTrainData.ToObject<HypeTrainLevelUp>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainProgression:
                    {
                        Progression = eventTrainData.ToObject<HypeTrainProgression>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainConductorUpdate:
                    {
                        ConductorUpdate = eventTrainData.ToObject<HypeTrainConductorUpdate>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainEnd:
                    {
                        End = eventTrainData.ToObject<HypeTrainEnd>();
                        return;
                    }
                case HypeTrainEventType.HypeTrainCooldownExpiration:
                    {
                        // No data.
                        return;
                    }
            }
        }
    }
}
