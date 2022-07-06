using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Models.Responses.Messages;
using TwitchLib.PubSub.Models.Responses.Messages.AutomodCaughtMessage;
using TwitchLib.PubSub.Models.Responses.Messages.UserModerationNotifications;

namespace TwitchLib.PubSub.Models.Responses
{
    /// <summary>
    /// PubSub Message model.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Topic that the message is relevant to.
        /// </summary>
        /// <value>The topic.</value>
        public string Topic { get; }
        /// <summary>
        /// Model containing data of the message.
        /// </summary>
        public readonly MessageData MessageData;

        /// <summary>
        /// PubSub Message model constructor.
        /// </summary>
        /// <param name="jsonStr">The json string.</param>
        public Message(string jsonStr)
        {
            var json = Helpers.ParseJson(jsonStr).SelectToken("data");
            Topic = json.SelectToken("topic")?.ToString();
            var encodedJsonMessage = json.SelectToken("message").ToString();
            switch (Topic?.Split('.')[0])
            {
                case MessageTopic.UserModerationNotifications:
                    MessageData = new UserModerationNotifications(encodedJsonMessage);
                    break;
                case MessageTopic.AutomodQueue:
                    MessageData = new AutomodQueue(encodedJsonMessage);
                    break;
                case MessageTopic.ChatModeratorActions:
                    MessageData = new ChatModeratorActions(encodedJsonMessage);
                    break;
                case MessageTopic.ChannelBitsEventsV1:
                    MessageData = new ChannelBitsEvents(encodedJsonMessage);
                    break;
                case MessageTopic.ChannelBitsEventsV2:
                    encodedJsonMessage = encodedJsonMessage.Replace("\\", "");
                    var dataEncoded = Helpers.ParseJson(encodedJsonMessage)["data"].ToString();
                    MessageData = JsonConvert.DeserializeObject<ChannelBitsEventsV2>(dataEncoded);
                    break;
                case MessageTopic.VideoPlaybackById:
                    MessageData = new VideoPlayback(encodedJsonMessage);
                    break;
                case MessageTopic.HypeTrainEventsV1:
                    MessageData = new HypeTrainEvent(encodedJsonMessage);
                    break;
                case MessageTopic.Whispers:
                    MessageData = new Whisper(encodedJsonMessage);
                    break;
                // case MessageTopic.ChannelCommerceEventsV1:
                //     MessageData = new ChannelCommerceEvents(encodedJsonMessage);
                //     break;
                case MessageTopic.ChannelSubscribeEventsV1:
                    MessageData = new ChannelSubscription(encodedJsonMessage);
                    break;
                case MessageTopic.StreamChatRoomV1:
                    MessageData = new StreamChatEvent(encodedJsonMessage);
                    break;
                case MessageTopic.ChannelExtV1:
                    MessageData = new ChannelExtensionBroadcast(encodedJsonMessage);
                    break;
                case MessageTopic.Following:
                    MessageData = new Following(encodedJsonMessage);
                    break;
                case MessageTopic.CommunityPointsChannelV1:
                    MessageData = new CommunityPointsChannel(encodedJsonMessage);
                    break;
                case MessageTopic.ChannelPointsChannelV1:
                    MessageData = new ChannelPointsChannel(encodedJsonMessage);
                    break;
                case MessageTopic.LeaderboardEventsV1:
                    MessageData = new LeaderboardEvents(encodedJsonMessage);
                    break;
                case MessageTopic.ChannelSubGiftsV1:
                    MessageData = new GiftSubEvent(encodedJsonMessage);
                    break;
                case MessageTopic.BroadcastSettingsUpdate:
                    MessageData = new BroadcastSettingsUpdateEvent(encodedJsonMessage);
                    break;
                case MessageTopic.Polls:
                    MessageData = new PollsEvent(encodedJsonMessage);
                    break;
                case MessageTopic.Ads:
                    MessageData = new AdsEvent(encodedJsonMessage);
                    break;
                case MessageTopic.CreatorGoalsEventsV1:
                    MessageData = new CreatorGoalsEvent(encodedJsonMessage);
                    break;
                case MessageTopic.Raid:
                    MessageData = new RaidEvents(encodedJsonMessage);
                    break;
                case MessageTopic.PredictionsChannelV1:
                    MessageData = new PredictionEvents(encodedJsonMessage);
                    break;
            }
        }
    }
}
