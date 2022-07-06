using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum PredictionType
    /// </summary>
    public enum PollsEventType
    {
        /// <summary>Poll Created.</summary>
        [EnumMember(Value = "POLL_CREATE")]
        PollCreate,
        /// <summary>Poll Update.</summary>
        [EnumMember(Value = "POLL_UPDATE")]
        PollUpdate,
        /// <summary>Poll Complete.</summary>
        [EnumMember(Value = "POLL_COMPLETE")]
        PollComplete,
        /// <summary>Poll Archive.</summary>
        [EnumMember(Value = "POLL_ARCHIVE")]
        PollArchive,
        /// <summary>Poll Terminate.</summary>
        [EnumMember(Value = "POLL_TERMINATE")]
        PollTerminate,
        
    }
}
