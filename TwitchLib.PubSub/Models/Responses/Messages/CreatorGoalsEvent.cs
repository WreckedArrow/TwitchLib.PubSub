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
    public class CreatorGoalsEvent : MessageData
    {
        /// <summary>
        /// Prediction Type
        /// </summary>
        /// <value>The type</value>
        public CreatorGoalEventType Type { get; protected set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public string GoalId { get; protected set; }
        /// <summary>
        /// Channel Id
        /// </summary>
        /// <value>The channel id</value>
        public string ChannelId { get; internal set; }
        /// <summary>
        /// Prediction Id
        /// </summary>
        /// <value>The id</value>
        public CreatorGoalContributionType ContributionType { get; protected set; }
        /// <summary>
        /// Prediction time
        /// </summary>
        /// <value>The seconds the prediction runs, starts from <see cref="CreatedAt"/></value>
        public CreatorGoalState State { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public string Description { get; protected set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <value>The title</value>
        public int CurrentContributions { get; protected set; }
        /// <summary>
        /// Prediction time
        /// </summary>
        /// <value>The seconds the prediction runs, starts from <see cref="CreatedAt"/></value>
        public int TargetContributions { get; protected set; }

        /// <summary>
        /// PredictionEvents constructor.
        /// </summary>
        /// <param name="jsonStr"></param>
        public CreatorGoalsEvent(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr);
            var eventData = json;
            // var eventData = json.SelectToken("data.message");
            Type = (CreatorGoalEventType)Enum.Parse(typeof(CreatorGoalEventType), eventData.SelectToken("type").ToString().Replace("_", ""), true);
            var eventGoalData = eventData.SelectToken("data.goal");
            GoalId = eventGoalData.SelectToken("id").ToString();
            ContributionType = (CreatorGoalContributionType)Enum.Parse(typeof(CreatorGoalContributionType), eventGoalData.SelectToken("contributionType").ToString(), true);
            State = (CreatorGoalState)Enum.Parse(typeof(CreatorGoalState), eventGoalData.SelectToken("state").ToString(), true);
            Description = eventGoalData.SelectToken("description").ToString();
            CurrentContributions = int.Parse(eventGoalData.SelectToken("currentContributions").ToString());
            TargetContributions = int.Parse(eventGoalData.SelectToken("targetContributions").ToString());
        }
    }
}
