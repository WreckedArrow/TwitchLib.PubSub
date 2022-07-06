using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum PredictionType
    /// </summary>
    public enum StreamChatType
    {
        /// <summary>The channel being hosted changes.</summary>
        [EnumMember(Value = "host_target_change")]
        HostTargetChange,
        /// <summary>The channel being hosted changes.</summary>
        [EnumMember(Value = "host_target_change_v2")]
        HostTargetChangeV2,
        /// <summary>Rich embed content.</summary>
        [EnumMember(Value = "chat_rich_embed")]
        ChatRichEmbed,
        [EnumMember(Value = "updated_room")]
        UpdatedRoom,
    }
}
