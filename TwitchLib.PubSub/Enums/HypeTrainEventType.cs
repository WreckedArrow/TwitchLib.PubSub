using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum HypeTrainEventType
    /// </summary>
    public enum HypeTrainEventType
    {
        /// <summary>Hype Train close to starting.</summary>
        [EnumMember(Value = "hype-train-approaching")]
        HypeTrainApproaching,
        /// <summary>Hype Train started.</summary>
        [EnumMember(Value = "hype-train-start")]
        HypeTrainStart,
        /// <summary>Hype Train update.</summary>
        [EnumMember(Value = "hype-train-progression")]
        HypeTrainProgression,
        /// <summary>Hype Train update.</summary>
        [EnumMember(Value = "hype-train-level-up")]
        HypeTrainLevelUp,
        /// <summary>Hype Train update.</summary>
        [EnumMember(Value = "hype-train-conductor-update")]
        HypeTrainConductorUpdate,
        /// <summary>Hype Train ended.</summary>
        [EnumMember(Value = "hype-train-end")]
        HypeTrainEnd,
        /// <summary>Hype Train cooldown expiration.</summary>
        [EnumMember(Value = "hype-train-cooldown-expiration")]
        HypeTrainCooldownExpiration,
    }
}
