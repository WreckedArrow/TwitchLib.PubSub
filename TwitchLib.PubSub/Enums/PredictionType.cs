using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum PredictionType
    /// </summary>
    public enum PredictionType
    {
        /// <summary>When a prediction is started [Contains all information about the prediction]</summary>
        [EnumMember(Value = "event-created")]
        EventCreated,
        /// <summary>When there is a update for the prediction [contains information about the prediction] (contains top predictors or the outcome when finished)</summary>
        [EnumMember(Value = "event-updated")]
        EventUpdated
    }
}
