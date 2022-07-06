using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub.Common;
using TwitchLib.PubSub.Enums;
using TwitchLib.PubSub.Events;
using TwitchLib.PubSub.Interfaces;
using TwitchLib.PubSub.Models;
using TwitchLib.PubSub.Models.Responses.Messages;
using TwitchLib.PubSub.Models.Responses.Messages.AutomodCaughtMessage;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;
using TwitchLib.PubSub.Models.Responses.Messages.UserModerationNotifications;
using Timer = System.Timers.Timer;

namespace TwitchLib.PubSub
{
    /// <summary>
    /// Class representing interactions with the Twitch PubSub
    /// Implements the <see cref="ITwitchPubSub" />
    /// </summary>
    /// <seealso cref="ITwitchPubSub" />
    public class TwitchPubSub : ITwitchPubSub
    {
        /// <summary>
        /// The socket
        /// </summary>
        private readonly WebSocketClient _socket;
        /// <summary>
        /// The previous requests
        /// </summary>
        private readonly List<PreviousRequest> _previousRequests = new List<PreviousRequest>();
        /// <summary>
        /// The previous requests semaphore
        /// </summary>
        private readonly Semaphore _previousRequestsSemaphore = new Semaphore(1, 1);
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TwitchPubSub> _logger;
        /// <summary>
        /// The ping timer
        /// </summary>
        private readonly Timer _pingTimer = new Timer();
        /// <summary>
        /// The pong timer
        /// </summary>
        private readonly Timer _pongTimer = new Timer();
        /// <summary>
        /// The pong received
        /// </summary>
        private bool _pongReceived = false;
        /// <summary>
        /// The topic list
        /// </summary>
        private readonly List<string> _topicList = new List<string>();

        private readonly Dictionary<string, string> _topicToChannelId = new Dictionary<string, string>();

        #region Events
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub Service is connected.
        /// </summary>
        public event EventHandler OnPubSubServiceConnected;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub Service has an error.
        /// </summary>
        public event EventHandler<OnPubSubServiceErrorArgs> OnPubSubServiceError;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub Service is closed.
        /// </summary>
        public event EventHandler OnPubSubServiceClosed;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives any response.
        /// </summary>
        public event EventHandler<OnListenResponseArgs> OnListenResponse;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice a viewer gets a timeout.
        /// </summary>
        public event EventHandler<OnTimeoutArgs> OnTimeout;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice a viewer gets banned.
        /// </summary>
        public event EventHandler<OnBanArgs> OnBan;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice a message was deleted.
        /// </summary>
        public event EventHandler<OnMessageDeletedArgs> OnMessageDeleted;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice a viewer gets unbanned.
        /// </summary>
        public event EventHandler<OnUnbanArgs> OnUnban;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice a viewer gets a timeout removed.
        /// </summary>
        public event EventHandler<OnUntimeoutArgs> OnUntimeout;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that the channel being listened to is hosting another channel.
        /// </summary>
        public event EventHandler<OnHostArgs> OnHost;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that Sub-Only Mode gets turned on.
        /// </summary>
        public event EventHandler<OnSubscribersOnlyArgs> OnSubscribersOnly;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that Sub-Only Mode gets turned off.
        /// </summary>
        public event EventHandler<OnSubscribersOnlyOffArgs> OnSubscribersOnlyOff;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that chat gets cleared.
        /// </summary>
        public event EventHandler<OnClearArgs> OnClear;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that Emote-Only Mode gets turned on.
        /// </summary>
        public event EventHandler<OnEmoteOnlyArgs> OnEmoteOnly;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that Emote-Only Mode gets turned off.
        /// </summary>
        public event EventHandler<OnEmoteOnlyOffArgs> OnEmoteOnlyOff;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that the chat option R9kBeta gets turned on.
        /// </summary>
        public event EventHandler<OnR9kBetaArgs> OnR9kBeta;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that the chat option R9kBeta gets turned off.
        /// </summary>
        public event EventHandler<OnR9kBetaOffArgs> OnR9kBetaOff;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice of a bit donation.
        /// </summary>
        public event EventHandler<OnBitsReceivedArgs> OnBitsReceived;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives a bits message.
        /// </summary>
        public event EventHandler<OnBitsReceivedV2Args> OnBitsReceivedV2;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that the stream of the channel being listened to goes online.
        /// </summary>
        public event EventHandler<OnStreamUpArgs> OnStreamUp;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice that the stream of the channel being listened to goes offline.
        /// </summary>
        public event EventHandler<OnStreamDownArgs> OnStreamDown;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice view count has changed.
        /// </summary>
        public event EventHandler<OnViewCountArgs> OnViewCount;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives a whisper.
        /// </summary>
        public event EventHandler<OnWhisperArgs> OnWhisper;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when the channel being listened to gets a subscription.
        /// </summary>
        public event EventHandler<OnChannelSubscriptionArgs> OnChannelSubscription;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives a message sent to the specified extension on the specified channel.
        /// </summary>
        public event EventHandler<OnChannelExtensionBroadcastArgs> OnChannelExtensionBroadcast;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a user follows the designated channel.
        /// </summary>
        public event EventHandler<OnFollowArgs> OnFollow;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a custom reward has been created on the specified channel.
        ///</summary>
        [Obsolete("This event fires on an undocumented/retired/obsolete topic.", false)]
        public event EventHandler<OnCustomRewardCreatedArgs> OnCustomRewardCreated;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a custom reward has been changed on the specified channel.
        ///</summary>
        [Obsolete("This event fires on an undocumented/retired/obsolete topic.", false)]
        public event EventHandler<OnCustomRewardUpdatedArgs> OnCustomRewardUpdated;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a reward has been deleted on the specified channel.
        /// </summary>
        [Obsolete("This event fires on an undocumented/retired/obsolete topic.", false)]
        public event EventHandler<OnCustomRewardDeletedArgs> OnCustomRewardDeleted;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a reward has been redeemed on the specified channel.
        /// </summary>
        [Obsolete("This event fires on an undocumented/retired/obsolete topic. Consider using OnChannelPointsRewardRedeemed", false)]
        public event EventHandler<OnRewardRedeemedArgs> OnRewardRedeemed;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives a message indicating a channel points reward was redeemed.
        /// </summary>
        public event EventHandler<OnChannelPointsRewardRedeemedArgs> OnChannelPointsRewardRedeemed;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when the leaderboard changes for subs.
        /// </summary>
        public event EventHandler<OnLeaderboardEventArgs> OnLeaderboardSubs;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when the leaderboard changes for Bits.
        /// </summary>
        public event EventHandler<OnLeaderboardEventArgs> OnLeaderboardBits;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a channel prepares a raid
        /// </summary>
        public event EventHandler<OnRaidUpdateArgs> OnRaidUpdate;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a channel prepares a raid
        /// </summary>
        public event EventHandler<OnRaidUpdateV2Args> OnRaidUpdateV2;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives notice when a channel starts the raid
        /// </summary>
        public event EventHandler<OnRaidGoArgs> OnRaidGo;
        /// <inheritdoc />
        /// <summary>
        /// Fires when PubSub receives any data from Twitch
        /// </summary>
        public event EventHandler<OnLogArgs> OnLog;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that the stream is playing a commercial.
        /// </summary>
        public event EventHandler<OnCommercialArgs> OnCommercial;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPredictionArgs> OnPredictionCreated;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPredictionArgs> OnPredictionUpdated;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnStreamChatHostTargetChange> OnStreamChatHostTargetChange;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnStreamChatRichEmbed> OnStreamChatRichEmbed;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnStreamChatUpdatedRoom> OnStreamChatUpdatedRoom;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnGiftSubArgs> OnCommunitySub;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnCreatorGoalUpdateArgs> OnCreatorGoalUpdate;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnBroadcastSettingsUpdateArgs> OnBroadcastSettingsUpdate;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPollEventArgs> OnPollCreated;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPollEventArgs> OnPollUpdated;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPollEventArgs> OnPollCompleted;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPollEventArgs> OnPollArchived;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnPollEventArgs> OnPollTerminated;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        public event EventHandler<OnAdsEventArgs> OnAdsMidroll;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when Automod updates a held message.
        /// </summary>
        public event EventHandler<OnAutomodCaughtMessageArgs> OnAutomodCaughtMessage;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnAutomodCaughtUserMessage> OnAutomodCaughtUserMessage;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainApproachingArgs> OnHypeTrainApproaching;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainStartArgs> OnHypeTrainStarted;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainProgressionArgs> OnHypeTrainProgression;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainLevelUpArgs> OnHypeTrainLevelUp;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainConductorUpdateArgs> OnHypeTrainConductorUpdate;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainEndArgs> OnHypeTrainEnd;
        /// <inheritdoc/>
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        public event EventHandler<OnHypeTrainCooldownExpirationArgs> OnHypeTrainCooldownExpiration;
        #endregion

        /// <summary>
        /// Constructor for a client that interface's with Twitch's PubSub system.
        /// </summary>
        /// <param name="logger">Optional ILogger param to enable logging</param>
        public TwitchPubSub(ILogger<TwitchPubSub> logger = null)
        {
            _logger = logger;

            var options = new ClientOptions { ClientType = ClientType.PubSub };
            _socket = new WebSocketClient(options);

            _socket.OnConnected += Socket_OnConnected;
            _socket.OnError += OnError;
            _socket.OnMessage += OnMessage;
            _socket.OnDisconnected += Socket_OnDisconnected;

            _pongTimer.Interval = 15000; //15 seconds, we should get a pong back within 10 seconds.
            _pongTimer.Elapsed += PongTimerTick;
        }

        /// <summary>
        /// Handles the <see cref="E:OnError" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OnErrorEventArgs"/> instance containing the event data.</param>
        private void OnError(object sender, OnErrorEventArgs e)
        {
            _logger?.LogError($"OnError in PubSub Websocket connection occured! Exception: {e.Exception}");
            OnPubSubServiceError?.Invoke(this, new OnPubSubServiceErrorArgs { Exception = e.Exception });
        }

        /// <summary>
        /// Handles the <see cref="E:OnMessage" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OnMessageEventArgs"/> instance containing the event data.</param>
        private void OnMessage(object sender, OnMessageEventArgs e)
        {
            _logger?.LogDebug($"Received Websocket OnMessage: {e.Message}");
            OnLog?.Invoke(this, new OnLogArgs { Data = e.Message });
            ParseMessage(e.Message);
        }

        /// <summary>
        /// Handles the OnDisconnected event of the Socket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Socket_OnDisconnected(object sender, EventArgs e)
        {
            _logger?.LogWarning("PubSub Websocket connection closed");
            _pingTimer.Stop();
            _pongTimer.Stop();
            OnPubSubServiceClosed?.Invoke(this, null);
        }

        /// <summary>
        /// Handles the OnConnected event of the Socket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Socket_OnConnected(object sender, EventArgs e)
        {
            _logger?.LogInformation("PubSub Websocket connection established");
            _pingTimer.Interval = 180000;
            _pingTimer.Elapsed += PingTimerTick;
            _pingTimer.Start();
            OnPubSubServiceConnected?.Invoke(this, null);
        }

        /// <summary>
        /// Pings the timer tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void PingTimerTick(object sender, ElapsedEventArgs e)
        {
            //Reset pong state.
            _pongReceived = false;

            //Send ping.
            var data = new JObject(
                new JProperty("type", "PING")
            );
            _socket.Send(data.ToString());

            //Start pong timer.
            _pongTimer.Start();
        }

        /// <summary>
        /// Pongs the timer tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private void PongTimerTick(object sender, ElapsedEventArgs e)
        {
            //Stop the pong timer.
            _pongTimer.Stop();

            if (_pongReceived)
            {
                //If we received a pong we're good.
                _pongReceived = false;
            }
            else
            {
                //Otherwise we're disconnected so close the socket.
                _socket.Close();
            }
        }

        /// <summary>
        /// Parses the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ParseMessage(string message)
        {
            var type = Helpers.ParseJson(message).SelectToken("type")?.ToString();

            switch (type?.ToLower())
            {
                case "response":
                    Models.Responses.Response resp;
                    try
                    {
                        resp = new Models.Responses.Response(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error parsing response: {message}\n{ex}");
                        break;
                    }
                    // var resp = new Models.Responses.Response(message);
                    if (_previousRequests.Count != 0)
                    {
                        bool handled = false;
                        _previousRequestsSemaphore.WaitOne();
                        try
                        {
                            for (int i = 0; i < _previousRequests.Count;)
                            {
                                var request = _previousRequests[i];
                                if (string.Equals(request.Nonce, resp.Nonce, StringComparison.CurrentCulture))
                                {
                                    //Remove the request.
                                    _previousRequests.RemoveAt(i);
                                    _topicToChannelId.TryGetValue(request.Topic, out var requestChannelId);
                                    OnListenResponse?.Invoke(this, new OnListenResponseArgs { Response = resp, Topic = request.Topic, Successful = resp.Successful, ChannelId = requestChannelId });
                                    handled = true;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                        finally
                        {
                            _previousRequestsSemaphore.Release();
                        }
                        if (handled) return;
                    }
                    break;
                case "message":
                    Models.Responses.Message msg;
                    try
                    {
                        msg = new Models.Responses.Message(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error parsing message: {message}\n{ex}");
                        break;
                    }
                    // var msg = new Models.Responses.Message(message);
                    _topicToChannelId.TryGetValue(msg.Topic, out var channelId);
                    channelId = channelId ?? "";
                    switch (msg.Topic.Split('.')[0])
                    {
                        case MessageTopic.UserModerationNotifications:
                            var userModerationNotifications = msg.MessageData as UserModerationNotifications;
                            switch (userModerationNotifications.Type)
                            {
                                case UserModerationNotificationsType.AutomodCaughtMessage:
                                    var automodCaughtMessage = userModerationNotifications.Data as Models.Responses.Messages.UserModerationNotificationsTypes.AutomodCaughtMessage;
                                    OnAutomodCaughtUserMessage?.Invoke(this, new OnAutomodCaughtUserMessage { ChannelId = channelId, UserId = msg.Topic.Split('.')[2], AutomodCaughtMessage = automodCaughtMessage });
                                    break;
                                case UserModerationNotificationsType.Unknown:
                                    break;
                            }
                            return;
                        case MessageTopic.AutomodQueue:
                            var automodQueue = msg.MessageData as AutomodQueue;
                            switch (automodQueue.Type)
                            {
                                case AutomodQueueType.CaughtMessage:
                                    var caughtMessage = automodQueue.Data as AutomodCaughtMessage;
                                    OnAutomodCaughtMessage?.Invoke(this, new OnAutomodCaughtMessageArgs { ChannelId = channelId, AutomodCaughtMessage = caughtMessage });
                                    break;
                                case AutomodQueueType.Unknown:
                                    UnaccountedFor($"Unknown automod queue type. Msg: {automodQueue.RawData}");
                                    break;
                            }
                            return;
                        case MessageTopic.ChannelSubscribeEventsV1:
                            var subscription = msg.MessageData as ChannelSubscription;
                            OnChannelSubscription?.Invoke(this, new OnChannelSubscriptionArgs { Subscription = subscription, ChannelId = channelId });
                            return;
                        case MessageTopic.Whispers:
                            var whisper = (Whisper)msg.MessageData;
                            OnWhisper?.Invoke(this, new OnWhisperArgs { Whisper = whisper, ChannelId = channelId });
                            return;
                        case MessageTopic.ChatModeratorActions:
                            var cma = msg.MessageData as ChatModeratorActions;
                            var reason = "";
                            switch (cma?.ModerationAction.ToLower())
                            {
                                case "timeout":
                                    if (cma.Args.Count > 2)
                                        reason = cma.Args[2];
                                    OnTimeout?.Invoke(this, new OnTimeoutArgs
                                    {
                                        TimedoutBy = cma.CreatedBy,
                                        TimedoutById = cma.CreatedByUserId,
                                        TimedoutUserId = cma.TargetUserId,
                                        TimeoutDuration = TimeSpan.FromSeconds(int.Parse(cma.Args[1])),
                                        TimeoutReason = reason,
                                        TimedoutUser = cma.Args[0],
                                        ChannelId = channelId
                                    });
                                    return;
                                case "ban":
                                    if (cma.Args.Count > 1)
                                        reason = cma.Args[1];
                                    OnBan?.Invoke(this, new OnBanArgs { BannedBy = cma.CreatedBy, BannedByUserId = cma.CreatedByUserId, BannedUserId = cma.TargetUserId, BanReason = reason, BannedUser = cma.Args[0], ChannelId = channelId });
                                    return;
                                case "delete":
                                    OnMessageDeleted?.Invoke(this, new OnMessageDeletedArgs { DeletedBy = cma.CreatedBy, DeletedByUserId = cma.CreatedByUserId, TargetUserId = cma.TargetUserId, TargetUser = cma.Args[0], Message = cma.Args[1], MessageId = cma.Args[2], ChannelId = channelId });
                                    return;
                                case "unban":
                                    OnUnban?.Invoke(this, new OnUnbanArgs { UnbannedBy = cma.CreatedBy, UnbannedByUserId = cma.CreatedByUserId, UnbannedUserId = cma.TargetUserId, UnbannedUser = cma.Args[0], ChannelId = channelId });
                                    return;
                                case "untimeout":
                                    OnUntimeout?.Invoke(this, new OnUntimeoutArgs { UntimeoutedBy = cma.CreatedBy, UntimeoutedByUserId = cma.CreatedByUserId, UntimeoutedUserId = cma.TargetUserId, UntimeoutedUser = cma.Args[0], ChannelId = channelId });
                                    return;
                                case "host":
                                    OnHost?.Invoke(this, new OnHostArgs { HostedChannel = cma.Args[0], Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "subscribers":
                                    OnSubscribersOnly?.Invoke(this, new OnSubscribersOnlyArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "subscribersoff":
                                    OnSubscribersOnlyOff?.Invoke(this, new OnSubscribersOnlyOffArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "clear":
                                    OnClear?.Invoke(this, new OnClearArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "emoteonly":
                                    OnEmoteOnly?.Invoke(this, new OnEmoteOnlyArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "emoteonlyoff":
                                    OnEmoteOnlyOff?.Invoke(this, new OnEmoteOnlyOffArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "r9kbeta":
                                    OnR9kBeta?.Invoke(this, new OnR9kBetaArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                                case "r9kbetaoff":
                                    OnR9kBetaOff?.Invoke(this, new OnR9kBetaOffArgs { Moderator = cma.CreatedBy, ChannelId = channelId });
                                    return;
                            }
                            break;
                        case MessageTopic.ChannelBitsEventsV1:
                            if (msg.MessageData is ChannelBitsEvents cbe)
                            {
                                OnBitsReceived?.Invoke(this, new OnBitsReceivedArgs
                                {
                                    BitsUsed = cbe.BitsUsed,
                                    ChannelId = cbe.ChannelId,
                                    ChannelName = cbe.ChannelName,
                                    ChatMessage = cbe.ChatMessage,
                                    Context = cbe.Context,
                                    Time = cbe.Time,
                                    TotalBitsUsed = cbe.TotalBitsUsed,
                                    UserId = cbe.UserId,
                                    Username = cbe.Username
                                });
                                return;
                            }
                            break;
                        case MessageTopic.ChannelBitsEventsV2:
                            if (msg.MessageData is ChannelBitsEventsV2 cbev2)
                            {
                                OnBitsReceivedV2?.Invoke(this, new OnBitsReceivedV2Args
                                {
                                    IsAnonymous = cbev2.IsAnonymous,
                                    BitsUsed = cbev2.BitsUsed,
                                    ChannelId = cbev2.ChannelId,
                                    ChannelName = cbev2.ChannelName,
                                    ChatMessage = cbev2.ChatMessage,
                                    Context = cbev2.Context,
                                    Time = cbev2.Time,
                                    TotalBitsUsed = cbev2.TotalBitsUsed,
                                    UserId = cbev2.UserId,
                                    UserName = cbev2.UserName
                                });
                                return;
                            }
                            break;
                        // case MessageTopic.ChannelCommerceEventsV1:
                        //     if (msg.MessageData is ChannelCommerceEvents cce)
                        //     {
                        //         OnChannelCommerceReceived?.Invoke(this, new OnChannelCommerceReceivedArgs
                        //         {

                        //             Username = cce.Username,
                        //             DisplayName = cce.DisplayName,
                        //             ChannelName = cce.ChannelName,
                        //             UserId = cce.UserId,
                        //             ChannelId = cce.ChannelId,
                        //             Time = cce.Time,
                        //             ItemImageURL = cce.ItemImageURL,
                        //             ItemDescription = cce.ItemDescription,
                        //             SupportsChannel = cce.SupportsChannel,
                        //             PurchaseMessage = cce.PurchaseMessage
                        //         });
                        //         return;
                        //     }
                        //     break;
                        case MessageTopic.ChannelExtV1:
                            var cEB = msg.MessageData as ChannelExtensionBroadcast;
                            OnChannelExtensionBroadcast?.Invoke(this, new OnChannelExtensionBroadcastArgs { Messages = cEB.Messages, ChannelId = channelId });
                            return;
                        case MessageTopic.VideoPlaybackById:
                            var vP = msg.MessageData as VideoPlayback;
                            switch (vP?.Type)
                            {
                                case VideoPlaybackType.StreamDown:
                                    OnStreamDown?.Invoke(this, new OnStreamDownArgs { ServerTime = vP.ServerTime, ChannelId = channelId });
                                    return;
                                case VideoPlaybackType.StreamUp:
                                    OnStreamUp?.Invoke(this, new OnStreamUpArgs { PlayDelay = vP.PlayDelay, ServerTime = vP.ServerTime, ChannelId = channelId });
                                    return;
                                case VideoPlaybackType.ViewCount:
                                    OnViewCount?.Invoke(this, new OnViewCountArgs { ServerTime = vP.ServerTime, Viewers = vP.Viewers, ChannelId = channelId });
                                    return;
                                case VideoPlaybackType.Commercial:
                                    OnCommercial?.Invoke(this, new OnCommercialArgs { ServerTime = vP.ServerTime, Length = vP.Length, ChannelId = channelId });
                                    return;
                            }
                            break;
                        case MessageTopic.Following:
                            var f = (Following)msg.MessageData;
                            f.FollowedChannelId = msg.Topic.Split('.')[1];
                            OnFollow?.Invoke(this, new OnFollowArgs { FollowedChannelId = f.FollowedChannelId, DisplayName = f.DisplayName, UserId = f.UserId, Username = f.Username });
                            return;
                        case MessageTopic.CommunityPointsChannelV1:
                            var cpc = msg.MessageData as CommunityPointsChannel;
                            switch (cpc?.Type)
                            {
                                case CommunityPointsChannelType.RewardRedeemed:
                                    OnRewardRedeemed?.Invoke(this, new OnRewardRedeemedArgs { TimeStamp = cpc.TimeStamp, ChannelId = cpc.ChannelId, Login = cpc.Login, DisplayName = cpc.DisplayName, Message = cpc.Message, RewardId = cpc.RewardId, RewardTitle = cpc.RewardTitle, RewardPrompt = cpc.RewardPrompt, RewardCost = cpc.RewardCost, Status = cpc.Status, RedemptionId = cpc.RedemptionId });
                                    return;
                                case CommunityPointsChannelType.CustomRewardUpdated:
                                    OnCustomRewardUpdated?.Invoke(this, new OnCustomRewardUpdatedArgs { TimeStamp = cpc.TimeStamp, ChannelId = cpc.ChannelId, RewardId = cpc.RewardId, RewardTitle = cpc.RewardTitle, RewardPrompt = cpc.RewardPrompt, RewardCost = cpc.RewardCost });
                                    return;
                                case CommunityPointsChannelType.CustomRewardCreated:
                                    OnCustomRewardCreated?.Invoke(this, new OnCustomRewardCreatedArgs { TimeStamp = cpc.TimeStamp, ChannelId = cpc.ChannelId, RewardId = cpc.RewardId, RewardTitle = cpc.RewardTitle, RewardPrompt = cpc.RewardPrompt, RewardCost = cpc.RewardCost });
                                    return;
                                case CommunityPointsChannelType.CustomRewardDeleted:
                                    OnCustomRewardDeleted?.Invoke(this, new OnCustomRewardDeletedArgs { TimeStamp = cpc.TimeStamp, ChannelId = cpc.ChannelId, RewardId = cpc.RewardId, RewardTitle = cpc.RewardTitle, RewardPrompt = cpc.RewardPrompt });
                                    return;
                            }
                            return;
                        case MessageTopic.ChannelPointsChannelV1:
                            var channelPointsChannel = msg.MessageData as ChannelPointsChannel;
                            switch (channelPointsChannel.Type)
                            {
                                case ChannelPointsChannelType.RewardRedeemed:
                                    var rewardRedeemed = channelPointsChannel.Data as RewardRedeemed;
                                    OnChannelPointsRewardRedeemed?.Invoke(this, new OnChannelPointsRewardRedeemedArgs { ChannelId = rewardRedeemed.Redemption.ChannelId, RewardRedeemed = rewardRedeemed });
                                    break;
                                case ChannelPointsChannelType.Unknown:
                                    UnaccountedFor($"Unknown channel points type. Msg: {channelPointsChannel.RawData}");
                                    break;
                            }
                            return;
                        case MessageTopic.LeaderboardEventsV1:
                            var lbe = msg.MessageData as LeaderboardEvents;
                            switch (lbe?.Type)
                            {
                                case LeaderBoardType.BitsUsageByChannel:
                                    OnLeaderboardBits?.Invoke(this, new OnLeaderboardEventArgs { ChannelId = lbe.ChannelId, TopList = lbe.Top });
                                    return;
                                case LeaderBoardType.SubGiftSent:
                                    OnLeaderboardSubs?.Invoke(this, new OnLeaderboardEventArgs { ChannelId = lbe.ChannelId, TopList = lbe.Top });
                                    return;
                            }
                            return;
                        case MessageTopic.ChannelSubGiftsV1:
                            var giftSub = msg.MessageData as GiftSubEvent;
                            switch (giftSub?.Type)
                            {
                                case GiftSubType.MysteryGiftPurchase:
                                    OnCommunitySub?.Invoke(this, new OnGiftSubArgs { ChannelId = giftSub.ChannelId, Id = giftSub.Id, GiftSubEvent = giftSub });
                                    return;
                                case null:
                                    UnaccountedFor("Gift Sub Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Gift Sub Type: {giftSub.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.CreatorGoalsEventsV1:
                            var creatorGoal = msg.MessageData as CreatorGoalsEvent;
                            creatorGoal.ChannelId = msg.Topic.Split('.')[1];
                            switch (creatorGoal?.Type)
                            {
                                case CreatorGoalEventType.GoalUpdated:
                                    OnCreatorGoalUpdate?.Invoke(this, new OnCreatorGoalUpdateArgs { ChannelId = creatorGoal.ChannelId, GoalId = creatorGoal.GoalId, CreatorGoalsEvent = creatorGoal });
                                    return;
                                case null:
                                    UnaccountedFor("Creator Goal Event Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Creator Goal Event Type: {creatorGoal.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.HypeTrainEventsV1:
                            var hypeTrain = msg.MessageData as HypeTrainEvent;
                            var hypeTrainChannelId = msg.Topic.Split('.')[1];
                            switch (hypeTrain?.Type)
                            {
                                case HypeTrainEventType.HypeTrainApproaching:
                                    OnHypeTrainApproaching?.Invoke(this, new OnHypeTrainApproachingArgs { ChannelId = hypeTrainChannelId, HypeTrainId = hypeTrain.Approaching.ApproachingHypeTrainId, Data = hypeTrain.Approaching });
                                    return;
                                case HypeTrainEventType.HypeTrainStart:
                                    OnHypeTrainStarted?.Invoke(this, new OnHypeTrainStartArgs { ChannelId = hypeTrainChannelId, HypeTrainId = hypeTrain.Start.Id, Data = hypeTrain.Start });
                                    return;
                                case HypeTrainEventType.HypeTrainProgression:
                                    OnHypeTrainProgression?.Invoke(this, new OnHypeTrainProgressionArgs { ChannelId = hypeTrainChannelId, Data = hypeTrain.Progression });
                                    return;
                                case HypeTrainEventType.HypeTrainLevelUp:
                                    OnHypeTrainLevelUp?.Invoke(this, new OnHypeTrainLevelUpArgs { ChannelId = hypeTrainChannelId, Data = hypeTrain.LevelUp });
                                    return;
                                case HypeTrainEventType.HypeTrainConductorUpdate:
                                    OnHypeTrainConductorUpdate?.Invoke(this, new OnHypeTrainConductorUpdateArgs { ChannelId = hypeTrainChannelId, Data = hypeTrain.ConductorUpdate });
                                    return;
                                case HypeTrainEventType.HypeTrainEnd:
                                    OnHypeTrainEnd?.Invoke(this, new OnHypeTrainEndArgs { ChannelId = hypeTrainChannelId, Data = hypeTrain.End });
                                    return;
                                case HypeTrainEventType.HypeTrainCooldownExpiration:
                                    OnHypeTrainCooldownExpiration?.Invoke(this, new OnHypeTrainCooldownExpirationArgs { ChannelId = hypeTrainChannelId });
                                    return;
                                case null:
                                    UnaccountedFor("Hype Train Event Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Hype Train Event Type: {hypeTrain.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.BroadcastSettingsUpdate:
                            var broadcastUpdate = msg.MessageData as BroadcastSettingsUpdateEvent;
                            switch (broadcastUpdate?.Type)
                            {
                                case BroadcastSettingsUpdateType.BroadcastSettingsUpdate:
                                    OnBroadcastSettingsUpdate?.Invoke(this, new OnBroadcastSettingsUpdateArgs { ChannelId = broadcastUpdate.ChannelId, BroadcastSettingsUpdateEvent = broadcastUpdate, });
                                    return;
                                case null:
                                    UnaccountedFor("Broadcast Update Event Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Broadcast Update Event Type: {broadcastUpdate.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.Polls:
                            var poll = msg.MessageData as PollsEvent;
                            switch (poll?.Type)
                            {
                                case PollsEventType.PollCreate:
                                    OnPollCreated?.Invoke(this, new OnPollEventArgs { ChannelId = poll.OwnedBy, PollId = poll.PollId, PollsEvent = poll });
                                    return;
                                case PollsEventType.PollUpdate:
                                    OnPollUpdated?.Invoke(this, new OnPollEventArgs { ChannelId = poll.OwnedBy, PollId = poll.PollId, PollsEvent = poll });
                                    return;
                                case PollsEventType.PollComplete:
                                    OnPollCompleted?.Invoke(this, new OnPollEventArgs { ChannelId = poll.OwnedBy, PollId = poll.PollId, PollsEvent = poll });
                                    return;
                                case PollsEventType.PollArchive:
                                    OnPollArchived?.Invoke(this, new OnPollEventArgs { ChannelId = poll.OwnedBy, PollId = poll.PollId, PollsEvent = poll });
                                    return;
                                case PollsEventType.PollTerminate:
                                    OnPollTerminated?.Invoke(this, new OnPollEventArgs { ChannelId = poll.OwnedBy, PollId = poll.PollId, PollsEvent = poll });
                                    return;
                                case null:
                                    UnaccountedFor("Poll Event Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Poll Event Type: {poll.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.Ads:
                            var ads = msg.MessageData as AdsEvent;
                            var adsChannelId = msg.Topic.Split('.')[1];
                            switch (ads?.Type)
                            {
                                case AdsEventType.MidrollRequest:
                                    OnAdsMidroll?.Invoke(this, new OnAdsEventArgs { ChannelId = adsChannelId, Type = ads.Type, Data = ads.Data });
                                    return;
                                case null:
                                    UnaccountedFor("Ads Event Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Ads Event Type: {ads.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.Raid:
                            var r = msg.MessageData as RaidEvents;
                            switch (r?.Type)
                            {
                                case RaidType.RaidUpdate:
                                    OnRaidUpdate?.Invoke(this, new OnRaidUpdateArgs { Id = r.Id, ChannelId = r.ChannelId, TargetChannelId = r.TargetChannelId, AnnounceTime = r.AnnounceTime, RaidTime = r.RaidTime, RemainingDurationSeconds = r.RemainigDurationSeconds, ViewerCount = r.ViewerCount });
                                    return;
                                case RaidType.RaidUpdateV2:
                                    OnRaidUpdateV2?.Invoke(this, new OnRaidUpdateV2Args { Id = r.Id, ChannelId = r.ChannelId, TargetChannelId = r.TargetChannelId, TargetLogin = r.TargetLogin, TargetDisplayName = r.TargetDisplayName, TargetProfileImage = r.TargetProfileImage, ViewerCount = r.ViewerCount });
                                    return;
                                case RaidType.RaidGo:
                                    OnRaidGo?.Invoke(this, new OnRaidGoArgs { Id = r.Id, ChannelId = r.ChannelId, TargetChannelId = r.TargetChannelId, TargetLogin = r.TargetLogin, TargetDisplayName = r.TargetDisplayName, TargetProfileImage = r.TargetProfileImage, ViewerCount = r.ViewerCount });
                                    return;
                            }
                            return;
                        case MessageTopic.PredictionsChannelV1:
                            var pred = msg.MessageData as PredictionEvents;
                            switch (pred?.Type)
                            {
                                case PredictionType.EventCreated:
                                    OnPredictionCreated?.Invoke(this, new OnPredictionArgs { Id = pred.Data.Event.Id, ChannelId = pred.Data.Event.ChannelId, Data = pred.Data, Type = pred.Type });
                                    return;
                                case PredictionType.EventUpdated:
                                    OnPredictionUpdated?.Invoke(this, new OnPredictionArgs { Id = pred.Data.Event.Id, ChannelId = pred.Data.Event.ChannelId, Data = pred.Data, Type = pred.Type });
                                    return;
                                case null:
                                    UnaccountedFor("Prediction Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Prediction Type: {pred.Type}");
                                    break;
                            }
                            return;
                        case MessageTopic.StreamChatRoomV1:
                            var streamChat = msg.MessageData as StreamChatEvent;
                            var streamChatChannelId = msg.Topic.Split('.')[1];
                            switch (streamChat?.Type)
                            {
                                case StreamChatType.HostTargetChangeV2:
                                    OnStreamChatHostTargetChange?.Invoke(this, new OnStreamChatHostTargetChange { ChannelId = streamChatChannelId, HostTargetChange = streamChat.HostTargetChange });
                                    return;
                                case StreamChatType.ChatRichEmbed:
                                    OnStreamChatRichEmbed?.Invoke(this, new OnStreamChatRichEmbed { ChannelId = streamChatChannelId, ChatRichEmbed = streamChat.ChatRichEmbed });
                                    return;
                                case StreamChatType.UpdatedRoom:
                                    OnStreamChatUpdatedRoom?.Invoke(this, new OnStreamChatUpdatedRoom { ChannelId = streamChatChannelId, UpdatedRoom = streamChat.UpdatedRoomData });
                                    return;
                                case null:
                                    UnaccountedFor("Stream Chat Type: null");
                                    break;
                                default:
                                    UnaccountedFor($"Stream Chat Type: {streamChat.Type}");
                                    break;
                            }
                            return;
                    }
                    break;
                case "pong":
                    _pongReceived = true;
                    return;
                case "reconnect": _socket.Close(); break;
            }
            UnaccountedFor(message);
        }

        /// <summary>
        /// The random
        /// </summary>
        private static readonly Random Random = new Random();
        /// <summary>
        /// Generates the nonce.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GenerateNonce()
        {
            return new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Listens to topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        private void ListenToTopic(string topic)
        {
            _topicList.Add(topic);
        }

        /// <summary>
        /// Listen to multiple topics.
        /// </summary>
        /// <param name="topics">The topics</param>
        private void ListenToTopics(params string[] topics)
        {
            foreach (var topic in topics)
            {
                _topicList.Add(topic);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the topics.
        /// </summary>
        /// <param name="oauth">The oauth.</param>
        /// <param name="unlisten">if set to <c>true</c> [unlisten].</param>
        public void SendTopics(string oauth = null, bool unlisten = false)
        {
            if (oauth != null && oauth.Contains("oauth:"))
            {
                oauth = oauth.Replace("oauth:", "");
            }

            var nonce = GenerateNonce();

            var topics = new JArray();
            _previousRequestsSemaphore.WaitOne();
            try
            {
                foreach (var val in _topicList)
                {
                    _previousRequests.Add(new PreviousRequest(nonce, PubSubRequestType.ListenToTopic, val));
                    topics.Add(new JValue(val));
                }
            }
            finally
            {
                _previousRequestsSemaphore.Release();
            }

            var jsonData = new JObject(
                new JProperty("type", !unlisten ? "LISTEN" : "UNLISTEN"),
                new JProperty("nonce", nonce),
                new JProperty("data",
                    new JObject(
                        new JProperty("topics", topics)
                        )
                    )
                );
            if (oauth != null)
            {
                ((JObject)jsonData.SelectToken("data"))?.Add(new JProperty("auth_token", oauth));
            }

            _socket.Send(jsonData.ToString());

            _topicList.Clear();
        }

        /// <summary>
        /// Unaccounted for.
        /// </summary>
        /// <param name="message">The message.</param>
        private void UnaccountedFor(string message)
        {
            _logger?.LogWarning($"[TwitchPubSub] Unaccount for: {message}");
        }

        #region Listeners
        /// <inheritdoc />
        /// <summary>
        /// Sends a request to listenOn follows coming into a specified channel.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        public void ListenToFollows(string channelId)
        {
            var topic = $"{MessageTopic.Following}.{channelId}";
            _topicToChannelId[topic] = channelId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends a request to listenOn timeouts and bans in a specific channel
        /// </summary>
        /// <param name="userId">A moderator's twitch account's ID (can be fetched from TwitchApi)</param>
        /// <param name="channelId">Channel ID who has previous parameter's moderator (can be fetched from TwitchApi)</param>
        public void ListenToChatModeratorActions(string userId, string channelId)
        {
            var topic = $"{MessageTopic.ChatModeratorActions}.{userId}.{channelId}";
            _topicToChannelId[topic] = channelId;
            ListenToTopic(topic);
        }

        public void ListenToUserModerationNotifications(string myTwitchId, string channelTwitchId)
        {
            var topic = $"{MessageTopic.UserModerationNotifications}.{myTwitchId}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends a request to listen to Automod queued messages in a specific channel
        /// </summary>
        /// <param name="userTwitchId">A moderator's twitch account's ID</param>
        /// <param name="channelTwitchId">Channel ID who has previous parameter's moderator</param>
        public void ListenToAutomodQueue(string userTwitchId, string channelTwitchId)
        {
            var topic = $"{MessageTopic.AutomodQueue}.{userTwitchId}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends a request to ListenOn EBS broadcasts sent to a specific extension on a specific channel.
        /// </summary>
        /// <param name="channelId">Id of the channel that the extension lives on.</param>
        /// <param name="extensionId">The extension identifier.</param>
        public void ListenToChannelExtensionBroadcast(string channelId, string extensionId)
        {
            var topic = $"{MessageTopic.ChannelExtV1}.{channelId}-{extensionId}-broadcast";
            _topicToChannelId[topic] = channelId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listenOn bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        [Obsolete("This topic is deprecated by Twitch. Please use ListenToBitsEventsV2()", false)]
        public void ListenToBitsEvents(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelBitsEventsV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToBitsEventsV2(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelBitsEventsV2}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="myTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToUserBitsEvents(string myTwitchId)
        {
            var topic = $"{MessageTopic.UserBitsUpdatesV1}.{myTwitchId}";
            _topicToChannelId[topic] = myTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToHypeTrain(string channelTwitchId)
        {
            var topic = $"{MessageTopic.HypeTrainEventsV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToCreatorGoals(string channelTwitchId)
        {
            var topic = $"{MessageTopic.CreatorGoalsEventsV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToCommunityBoost(string channelTwitchId)
        {
            var topic = $"{MessageTopic.CommunityBoostEventsV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToDrops(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelDropEvents}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToSubGifts(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelSubGiftsV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToCheers(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelCheerEventsPublicV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToBountyBoardEvents(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelBountyBoardEventsCTA}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        public void ListenToStreamChatRoom(string channelTwitchId)
        {
            var topic = $"{MessageTopic.StreamChatRoomV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        // /// <inheritdoc />
        // /// <summary>
        // /// Sends request to listenOn channel commerce events in specific channel
        // /// </summary>
        // /// <param name="channelTwitchId">Channel Id of channel to listen to commerce events on.</param>
        // public void ListenToCommerce(string channelTwitchId)
        // {
        //     var topic = $"{MessageTopic.ChannelCommerceEventsV1}.{channelTwitchId}";
        //     _topicToChannelId[topic] = channelTwitchId;
        //     ListenToTopic(topic);
        // }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listenOn video playback events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Id of channel to listen to playback events in.</param>
        public void ListenToVideoPlayback(string channelTwitchId)
        {
            var topic = $"{MessageTopic.VideoPlaybackById}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to whispers from specific channel.
        /// </summary>
        /// <param name="userId">Channel to listen to whispers on.</param>
        public void ListenToWhispers(string userId)
        {
            var topic = $"{MessageTopic.Whispers}.{userId}";
            _topicToChannelId[topic] = userId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to rewards from specific channel.
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to rewards on.</param>
        [Obsolete("This method listens to an undocumented/retired/obsolete topic. Consider using ListenToChannelPoints()", false)]
        public void ListenToRewards(string channelTwitchId)
        {
            var topic = $"{MessageTopic.CommunityPointsChannelV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to channel points actions from specific channel.
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to rewards on.</param>
        public void ListenToChannelPoints(string channelTwitchId)
        {
            var topic = $"{MessageTopic.ChannelPointsChannelV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to leaderboards from specific channel.
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to leaderboards on.</param>
        public void ListenToLeaderboards(string channelTwitchId)
        {
            var topicBits = $"{MessageTopic.LeaderboardEventsV1}.bits-usage-by-channel-v1-{channelTwitchId}-WEEK";
            var topicSubs = $"{MessageTopic.LeaderboardEventsV1}.sub-gift-sent-{channelTwitchId}-WEEK";
            _topicToChannelId[topicBits] = channelTwitchId;
            _topicToChannelId[topicSubs] = channelTwitchId;
            ListenToTopics(topicBits, topicSubs);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToRaid(string channelTwitchId)
        {
            var topicRaid = $"{MessageTopic.Raid}.{channelTwitchId}";
            _topicToChannelId[topicRaid] = channelTwitchId;
            ListenToTopic(topicRaid);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToPoll(string channelTwitchId)
        {
            var topicPoll = $"{MessageTopic.Polls}.{channelTwitchId}";
            _topicToChannelId[topicPoll] = channelTwitchId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToPresence(string userId)
        {
            var topicPoll = $"{MessageTopic.Presence}.{userId}";
            _topicToChannelId[topicPoll] = userId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToAds(string channelId)
        {
            var topicAds = $"{MessageTopic.Ads}.{channelId}";
            var topicAdProperties = $"{MessageTopic.AdPropertyRefresh}.{channelId}";
            _topicToChannelId[topicAds] = channelId;
            _topicToChannelId[topicAdProperties] = channelId;
            ListenToTopics(topicAds, topicAdProperties);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToStreamChange(string channelId)
        {
            var topicPoll = $"{MessageTopic.StreamChangeByChannel}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToBroadcastSettingsUpdate(string channelId)
        {
            var topicPoll = $"{MessageTopic.BroadcastSettingsUpdate}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToWatchParty(string channelId)
        {
            var topicPoll = $"{MessageTopic.PvWatchPartyEvents}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToExtensionControlByChannel(string channelId)
        {
            var topicPoll = $"{MessageTopic.ExtensionControl}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToRadioEvents(string channelId)
        {
            var topicPoll = $"{MessageTopic.RadioEventsV1}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToOnsiteNotifications(string userId)
        {
            var topicPoll = $"{MessageTopic.OnsiteNotifications}.{userId}";
            _topicToChannelId[topicPoll] = userId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToUserProperties(string userId)
        {
            var topicProperties = $"{MessageTopic.UserPropertiesUpdate}.{userId}";
            var topicPreferences = $"{MessageTopic.UserPreferencesUpdateV1}.{userId}";
            _topicToChannelId[topicProperties] = userId;
            _topicToChannelId[topicPreferences] = userId;
            ListenToTopics(topicProperties, topicPreferences);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToUnbanRequests(string userId, string channelId)
        {
            var topicUserUnban = $"{MessageTopic.UserUnbanRequests}.{userId}.{channelId}";
            var topicChannelUnban = $"{MessageTopic.ChannelUnbanRequests}.{userId}.{channelId}";
            _topicToChannelId[topicUserUnban] = channelId;
            _topicToChannelId[topicChannelUnban] = channelId;
            ListenToTopics(topicUserUnban, topicChannelUnban);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToChatroomUser(string userId)
        {
            var topicPoll = $"{MessageTopic.ChatroomsUserV1}.{userId}";
            _topicToChannelId[topicPoll] = userId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToLowTrustUsers(string userId, string channelId)
        {
            var topicPoll = $"{MessageTopic.LowTrustUsers}.{userId}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToUserCampaignEvents(string userId)
        {
            var topicPoll = $"{MessageTopic.UserCampaignEvents}.{userId}";
            _topicToChannelId[topicPoll] = userId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToCommunityMoments(string channelId)
        {
            var topicPoll = $"{MessageTopic.CommunityMomentsChannelV1}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        public void ListenToPrivateCallout(string userId, string channelId)
        {
            var topicPoll = $"{MessageTopic.PrivateCallout}.{userId}.{channelId}";
            _topicToChannelId[topicPoll] = channelId;
            ListenToTopic(topicPoll);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to channel subscriptions.
        /// </summary>
        /// <param name="channelId">Id of the channel to listen to.</param>
        public void ListenToSubscriptions(string channelId)
        {
            var topic = $"{MessageTopic.ChannelSubscribeEventsV1}.{channelId}";
            _topicToChannelId[topic] = channelId;
            ListenToTopic(topic);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends request to listen to channel predictions.
        /// </summary>
        /// <param name="channelTwitchId"></param>
        public void ListenToPredictions(string channelTwitchId)
        {
            var topic = $"{MessageTopic.PredictionsChannelV1}.{channelTwitchId}";
            _topicToChannelId[topic] = channelTwitchId;
            ListenToTopic(topic);
        }
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Method to connect to Twitch's PubSub service. You MUST listen toOnConnected event and listen to a Topic within 15 seconds of connecting (or be disconnected)
        /// </summary>
        public void Connect()
        {
            _socket.Open();
        }

        /// <inheritdoc />
        /// <summary>
        /// What do you think it does? :)
        /// </summary>
        public void Disconnect()
        {
            _socket.Close();
        }

        /// <inheritdoc />
        /// <summary>
        /// This method will send passed json text to the message parser in order to allow forOn-demand parser testing.
        /// </summary>
        /// <param name="testJsonString">The test json string.</param>
        public void TestMessageParser(string testJsonString)
        {
            ParseMessage(testJsonString);
        }
    }
}
