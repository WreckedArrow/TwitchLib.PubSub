using System;
using TwitchLib.PubSub.Events;

namespace TwitchLib.PubSub.Interfaces
{
    /// <summary>
    /// Interface ITwitchPubSub
    /// </summary>
    public interface ITwitchPubSub
    {
        /// <summary>
        /// Occurs when [on ban].
        /// </summary>
        event EventHandler<OnBanArgs> OnBan;
        /// <summary>
        /// Occurs when [on bits received].
        /// </summary>
        event EventHandler<OnBitsReceivedArgs> OnBitsReceived;
        /// <summary>
        /// Occurs when [on bits received].
        /// </summary>
        event EventHandler<OnBitsReceivedV2Args> OnBitsReceivedV2;
        /// <summary>
        /// Fires when PubSub receives a message indicating a channel points reward was redeemed.
        /// </summary>
        event EventHandler<OnChannelPointsRewardRedeemedArgs> OnChannelPointsRewardRedeemed;
        /// <summary>
        /// Occurs when [on channel extension broadcast].
        /// </summary>
        event EventHandler<OnChannelExtensionBroadcastArgs> OnChannelExtensionBroadcast;
        /// <summary>
        /// Occurs when [on channel subscription].
        /// </summary>
        event EventHandler<OnChannelSubscriptionArgs> OnChannelSubscription;
        /// <summary>
        /// Occurs when [on clear].
        /// </summary>
        event EventHandler<OnClearArgs> OnClear;
        /// <summary>
        /// Occurs when [on emote only].
        /// </summary>
        event EventHandler<OnEmoteOnlyArgs> OnEmoteOnly;
        /// <summary>
        /// Occurs when [on emote only off].
        /// </summary>
        event EventHandler<OnEmoteOnlyOffArgs> OnEmoteOnlyOff;
        /// <summary>
        /// Occurs when [on follow].
        /// </summary>
        event EventHandler<OnFollowArgs> OnFollow;
        /// <summary>
        /// Occurs when [on host].
        /// </summary>
        event EventHandler<OnHostArgs> OnHost;
        /// <summary>
        /// Occurs when [on message deleted].
        /// </summary>
        event EventHandler<OnMessageDeletedArgs> OnMessageDeleted;
        /// <summary>
        /// Occurs when [on listen response].
        /// </summary>
        event EventHandler<OnListenResponseArgs> OnListenResponse;
        /// <summary>
        /// Occurs when [on pub sub service closed].
        /// </summary>
        event EventHandler OnPubSubServiceClosed;
        /// <summary>
        /// Occurs when [on pub sub service connected].
        /// </summary>
        event EventHandler OnPubSubServiceConnected;
        /// <summary>
        /// Occurs when [on pub sub service error].
        /// </summary>
        event EventHandler<OnPubSubServiceErrorArgs> OnPubSubServiceError;
        /// <summary>
        /// Occurs when [on R9K beta].
        /// </summary>
        event EventHandler<OnR9kBetaArgs> OnR9kBeta;
        /// <summary>
        /// Occurs when [on R9K beta off].
        /// </summary>
        event EventHandler<OnR9kBetaOffArgs> OnR9kBetaOff;
        /// <summary>
        /// Occurs when [on stream down].
        /// </summary>
        event EventHandler<OnStreamDownArgs> OnStreamDown;
        /// <summary>
        /// Occurs when [on stream up].
        /// </summary>
        event EventHandler<OnStreamUpArgs> OnStreamUp;
        /// <summary>
        /// Occurs when [on subscribers only].
        /// </summary>
        event EventHandler<OnSubscribersOnlyArgs> OnSubscribersOnly;
        /// <summary>
        /// Occurs when [on subscribers only off].
        /// </summary>
        event EventHandler<OnSubscribersOnlyOffArgs> OnSubscribersOnlyOff;
        /// <summary>
        /// Occurs when [on timeout].
        /// </summary>
        event EventHandler<OnTimeoutArgs> OnTimeout;
        /// <summary>
        /// Occurs when [on unban].
        /// </summary>
        event EventHandler<OnUnbanArgs> OnUnban;
        /// <summary>
        /// Occurs when [on untimeout].
        /// </summary>
        event EventHandler<OnUntimeoutArgs> OnUntimeout;
        /// <summary>
        /// Occurs when [on view count].
        /// </summary>
        event EventHandler<OnViewCountArgs> OnViewCount;
        /// <summary>
        /// Occurs when [on whisper].
        /// </summary>
        event EventHandler<OnWhisperArgs> OnWhisper;
        /// <summary>
        /// Occurs when [on reward created]
        ///</summary>
        event EventHandler<OnCustomRewardCreatedArgs> OnCustomRewardCreated;
        /// <summary>
        /// Occurs when [on reward updated]
        ///</summary>
        event EventHandler<OnCustomRewardUpdatedArgs> OnCustomRewardUpdated;
        /// <summary>
        /// Occurs when [on reward deleted]
        /// </summary>
        event EventHandler<OnCustomRewardDeletedArgs> OnCustomRewardDeleted;
        /// <summary>
        /// Occurs when [on reward redeemed]
        /// </summary>
        event EventHandler<OnRewardRedeemedArgs> OnRewardRedeemed;
        /// <summary>
        /// Occurs when [on leaderboard subs].
        /// </summary>
        event EventHandler<OnLeaderboardEventArgs> OnLeaderboardSubs;
        /// <summary>
        /// Occurs when [on leaderboard bits].
        /// </summary>
        event EventHandler<OnLeaderboardEventArgs> OnLeaderboardBits;
        /// <summary>
        /// Occurs when [on raid update]
        /// </summary>
        event EventHandler<OnRaidUpdateArgs> OnRaidUpdate;
        /// <summary>
        /// Occurs when [on raid update v2]
        /// </summary>
        event EventHandler<OnRaidUpdateV2Args> OnRaidUpdateV2;
        /// <summary>
        /// Occurs when [on raid go]
        /// </summary>
        event EventHandler<OnRaidGoArgs> OnRaidGo;
        /// <summary>
        /// Occurs when [on log].
        /// </summary>
        event EventHandler<OnLogArgs> OnLog;
        /// <summary>
        /// Occurs when [on commercial].
        /// </summary>
        event EventHandler<OnCommercialArgs> OnCommercial;
        /// <summary>
        /// Occurs when [on prediction].
        /// </summary>
        event EventHandler<OnPredictionArgs> OnPredictionCreated;
        /// <summary>
        /// Occurs when [on prediction].
        /// </summary>
        event EventHandler<OnPredictionArgs> OnPredictionUpdated;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnStreamChatHostTargetChange> OnStreamChatHostTargetChange;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnStreamChatRichEmbed> OnStreamChatRichEmbed;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnStreamChatUpdatedRoom> OnStreamChatUpdatedRoom;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnGiftSubArgs> OnCommunitySub;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnCreatorGoalUpdateArgs> OnCreatorGoalUpdate;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnBroadcastSettingsUpdateArgs> OnBroadcastSettingsUpdate;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnPollEventArgs> OnPollCreated;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnPollEventArgs> OnPollUpdated;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnPollEventArgs> OnPollCompleted;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnPollEventArgs> OnPollArchived;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnPollEventArgs> OnPollTerminated;
        /// <summary>
        /// Fires when PubSub receives notice that a prediction has started or updated.
        /// </summary>
        event EventHandler<OnAdsEventArgs> OnAdsMidroll;
        /// <summary>
        /// Fires when Automod updates a held message.
        /// </summary>
        event EventHandler<OnAutomodCaughtMessageArgs> OnAutomodCaughtMessage;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnAutomodCaughtUserMessage> OnAutomodCaughtUserMessage;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainApproachingArgs> OnHypeTrainApproaching;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainStartArgs> OnHypeTrainStarted;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainProgressionArgs> OnHypeTrainProgression;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainLevelUpArgs> OnHypeTrainLevelUp;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainConductorUpdateArgs> OnHypeTrainConductorUpdate;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainEndArgs> OnHypeTrainEnd;
        /// <summary>
        /// Fires when a moderation event hits a user
        /// </summary>
        event EventHandler<OnHypeTrainCooldownExpirationArgs> OnHypeTrainCooldownExpiration;

        /// <summary>
        /// Connects this instance.
        /// </summary>
        void Connect();
        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Listens to bits events.
        /// </summary>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToBitsEvents(string channelTwitchId);
        /// <summary>
        /// Listens to extension channel broadcast messages.
        /// </summary>
        /// <param name="channelId">The channel twitch identifier.</param>
        /// <param name="extensionId">The extension identifier.</param>
        void ListenToChannelExtensionBroadcast(string channelId, string extensionId);
        /// <summary>
        /// Listens to chat moderator actions.
        /// </summary>
        /// <param name="myTwitchId">My twitch identifier.</param>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToChatModeratorActions(string myTwitchId, string channelTwitchId);
        /// <summary>
        /// Listens to chat moderator actions.
        /// </summary>
        /// <param name="myTwitchId">My twitch identifier.</param>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToUserModerationNotifications(string myTwitchId, string channelTwitchId);
        // /// <summary>
        // /// Listens to commerce events.
        // /// </summary>
        // /// <param name="channelTwitchId">The channel twitch identifier.</param>
        // void ListenToCommerce(string channelTwitchId);
        /// <summary>
        /// Listens to follows.
        /// </summary>
        /// <param name="channelId">The channel twitch identifier.</param>
        void ListenToFollows(string channelId);
        /// <summary>
        /// Listens to subscriptions.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        void ListenToSubscriptions(string channelId);
        /// <summary>
        /// Listens to video playback.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        void ListenToVideoPlayback(string channelName);
        /// <summary>
        /// Listens to whispers.
        /// </summary>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToWhispers(string channelTwitchId);
        /// <summary>
        /// Listens to rewards
        /// </summary>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToRewards(string channelTwitchId);
        /// <summary>
        /// Listens to leaderboards
        /// </summary>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToLeaderboards(string channelTwitchId);
        /// <summary>
        /// Listens to raids
        /// </summary>
        /// <param name="channelTwitchId">The channel twitch identifier.</param>
        void ListenToRaid(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="myTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToUserBitsEvents(string myTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToHypeTrain(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToCreatorGoals(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToCommunityBoost(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToDrops(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToSubGifts(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToCheers(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToBountyBoardEvents(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to bits events in specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel Id of channel to listen to bits on (can be fetched from TwitchApi)</param>
        void ListenToStreamChatRoom(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToPoll(string channelTwitchId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToPredictions(string channelTwitchId);
        /// <summary>
        /// Sends the topics.
        /// </summary>
        /// <param name="oauth">The oauth.</param>
        /// <param name="unlisten">if set to <c>true</c> [unlisten].</param>
        void SendTopics(string oauth = null, bool unlisten = false);
        /// <summary>
        /// Tests the message parser.
        /// </summary>
        /// <param name="testJsonString">The test json string.</param>
        void TestMessageParser(string testJsonString);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToPresence(string userId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToAds(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToStreamChange(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToBroadcastSettingsUpdate(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToWatchParty(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToExtensionControlByChannel(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToRadioEvents(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToOnsiteNotifications(string userId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToUserProperties(string userId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToUnbanRequests(string userId, string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToChatroomUser(string userId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToLowTrustUsers(string userId, string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToUserCampaignEvents(string userId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToCommunityMoments(string channelId);
        /// <summary>
        /// Sends request to listen to raids 'from' specific channel
        /// </summary>
        /// <param name="channelTwitchId">Channel to listen to raids get prepared on.</param>
        void ListenToPrivateCallout(string userId, string channelId);
    }
}
