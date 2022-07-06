using System;
using System.Collections.Generic;
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
    public class GiftSubEvent : MessageData
    {
        /// <summary>
        /// Prediction Type
        /// </summary>
        /// <value>The type</value>
        public GiftSubType Type { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public Guid Id { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public string UserId { get; protected set; }
        /// <summary>
        /// Prediction time
        /// </summary>
        /// <value>The seconds the prediction runs, starts from <see cref="CreatedAt"/></value>
        public string Tier { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string DisplayName { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string UserName { get; protected set; }
        /// <summary>
        /// Prediction time
        /// </summary>
        /// <value>The seconds the prediction runs, starts from <see cref="CreatedAt"/></value>
        public int Count { get; protected set; }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public GiftSubEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            var eventData = json;
            // var eventData = json.SelectToken("data.message");
            Type = (GiftSubType)Enum.Parse(typeof(GiftSubType), eventData.SelectToken("type").ToString().Replace("-", ""), true);
            Id = Guid.Parse(eventData.SelectToken("uuid").ToString());
            ChannelId = eventData.SelectToken("channel_id").ToString();
            UserId = eventData.SelectToken("user_id").ToString();
            UserName = eventData.SelectToken("user_name").ToString();
            DisplayName = eventData.SelectToken("display_name").ToString();
            Tier = eventData.SelectToken("tier").ToString();
            Count = int.Parse(eventData.SelectToken("count").ToString());
        }
    }
}
