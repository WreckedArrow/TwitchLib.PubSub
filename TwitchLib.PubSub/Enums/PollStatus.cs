using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum PredictionType
    /// </summary>
    public enum PollStatus
    {
        /// <summary>Poll Active.</summary>
        [EnumMember(Value = "ACTIVE")]
        Active,
        /// <summary>Poll Completed.</summary>
        [EnumMember(Value = "COMPLETED")]
        Completed,
        /// <summary>Poll Archived.</summary>
        [EnumMember(Value = "ARCHIVED")]
        Archived,
        /// <summary>Poll Terminated.</summary>
        [EnumMember(Value = "TERMINATED")]
        Terminated,
    }
}
