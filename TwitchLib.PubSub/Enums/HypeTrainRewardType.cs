using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum HypeTrainEventType
    /// </summary>
    public enum HypeTrainRewardType
    {
        /// <summary>Emotes</summary>
        [EnumMember(Value = "EMOTE")]
        Emote,
        [EnumMember(Value = "BADGE")]
        Badge,
    }
}
