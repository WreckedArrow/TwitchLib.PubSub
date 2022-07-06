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
    public class OnCreatorGoalUpdateArgs : EventArgs
    {
        /// <summary>
        /// Property representing the the Goal Id
        /// </summary>
        public string GoalId;

        /// <summary>
        /// The channel Id it came from
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId;
        /// <summary>
        /// Property representing the the prediction type
        /// </summary>
        public CreatorGoalsEvent CreatorGoalsEvent;

    }
}
