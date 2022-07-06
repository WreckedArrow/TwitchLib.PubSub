using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Extensions;

namespace TwitchLib.PubSub.Models.Responses.Messages
{
    /// <summary>
    /// Predictions model constructor.
    /// Implements the <see cref="MessageData" />
    /// </summary>
    /// <seealso cref="MessageData" />
    /// <inheritdoc />
    public class BroadcastSettingsUpdateEvent : MessageData
    {
        /// <summary>
        /// Prediction Type
        /// </summary>
        /// <value>The type</value>
        public BroadcastSettingsUpdateType Type { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelLogin { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string OldStatus { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string Status { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string OldGame { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string Game { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public int? OldGameId { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public int? GameId { get; protected set; }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public BroadcastSettingsUpdateEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            Type = (BroadcastSettingsUpdateType)Enum.Parse(typeof(BroadcastSettingsUpdateType), json.SelectToken("type").ToString().Replace("_", ""), true);
            switch (Type)
            {
                case BroadcastSettingsUpdateType.BroadcastSettingsUpdate:
                    {
                        ChannelId = json.SelectToken("channel_id").ToString();
                        ChannelLogin = json.SelectToken("channel").ToString();
                        OldStatus = json.SelectToken("old_status").ToString();
                        Status = json.SelectToken("status").ToString();
                        OldGame = json.SelectToken("old_game").ToString();
                        Game = json.SelectToken("game").ToString();
                        OldGameId = int.TryParse(json.SelectToken("old_game_id").ToString(), out int val) ? val : (int?)null;
                        GameId = int.TryParse(json.SelectToken("game_id").ToString(), out val) ? val : (int?)null;
                        break;
                    }
            }
        }
    }
}
