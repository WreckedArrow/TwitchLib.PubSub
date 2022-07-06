using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum HypeTrainEventType
    /// </summary>
    public enum HypeTrainDifficulty
    {
        /// <summary>EASY</summary>
        [EnumMember(Value = "EASY")]
        Easy,
        /// <summary>MEDIUM</summary>
        [EnumMember(Value = "MEDIUM")]
        Medium,
        /// <summary>HARD</summary>
        [EnumMember(Value = "HARD")]
        Hard,
        /// <summary>SUPER HARD</summary>
        [EnumMember(Value = "SUPER HARD")]
        SuperHard,
        /// <summary>HARD</summary>
        [EnumMember(Value = "INSANE")]
        Insane,
    }
}
