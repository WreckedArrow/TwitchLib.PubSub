﻿using System;
using System.Collections.Generic;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Models;
using static TwitchLib.PubSub.Models.Responses.Messages.PredictionEvents;

namespace TwitchLib.PubSub.Events
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing arguments of on prediction event.
    /// </summary>
    public class OnPredictionArgs : EventArgs
    {
        /// <summary>
        /// Property representing the the prediction type
        /// </summary>
        public PredictionType Type;

        /// <summary>
        /// Property representing the the prediction Id
        /// </summary>
        public Guid Id;

        public string ChannelId;

        public PredictionData Data;
    }
}
