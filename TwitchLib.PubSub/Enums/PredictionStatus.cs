using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    public enum PredictionStatus
    {
        [EnumMember(Value = "CANCELED")]
        Canceled = -4,
        [EnumMember(Value = "CANCEL_PENDING")]
        CancelPending = -3,
        [EnumMember(Value = "RESOLVED")]
        Resolved = -2,
        [EnumMember(Value = "RESOLVE_PENDING")]
        ResolvePending = -1,
        [EnumMember(Value = "LOCKED")]
        Locked = 0,
        [EnumMember(Value = "ACTIVE")]
        Active = 1,
    }
}
