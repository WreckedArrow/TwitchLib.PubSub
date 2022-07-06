using System.Runtime.Serialization;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Enum PredictionType
    /// </summary>
    public enum AdsEventType
    {
        /// <summary>When a prediction is started [Contains all information about the prediction]</summary>
        [EnumMember(Value = "midroll_request")]
        MidrollRequest,
    }
}
