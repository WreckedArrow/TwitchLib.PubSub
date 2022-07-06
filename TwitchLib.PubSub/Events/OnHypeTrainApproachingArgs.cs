using System;
using System.Collections.Generic;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Models.Responses.Messages;
using static TwitchLib.PubSub.Models.Responses.Messages.HypeTrainEvent;

namespace TwitchLib.PubSub.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing arguments of on prediction event.
    /// </summary>
    public class OnHypeTrainApproachingArgs : EventArgs
    {
        /// <summary>
        /// Property representing the the Goal Id
        /// </summary>
        public Guid HypeTrainId;

        /// <summary>
        /// The channel Id it came from
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId;
        /// <summary>
        /// Property representing the the prediction type
        /// </summary>
        public HypeTrainApproaching Data;

    }
}
