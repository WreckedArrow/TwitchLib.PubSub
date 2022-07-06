using System;
using System.Collections.Generic;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Models.Responses.Messages;

namespace TwitchLib.PubSub.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing arguments of on prediction event.
    /// </summary>
    public class OnHypeTrainCooldownExpirationArgs : EventArgs
    {
        /// <summary>
        /// The channel Id it came from
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId;
    }
}
