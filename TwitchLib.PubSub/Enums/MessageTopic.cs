using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchLib.PubSub.Enums
{
    /// <summary>
    /// Not Really An Enum, MessageTopic
    /// </summary>
    public static class MessageTopic
    {
        public const string
            Following = "following",
            ChatModeratorActions = "chat_moderator_actions",
            UserModerationNotifications = "user-moderation-notifications",
            AutomodQueue = "automod-queue",
            ChannelExtV1 = "channel-ext-v1",
            ChannelBitsEventsV1 = "channel-bits-events-v1",
            ChannelBitsEventsV2 = "channel-bits-events-v2",
            UserBitsUpdatesV1 = "user-bits-updates-v1", // no events?
            HypeTrainEventsV1 = "hype-train-events-v1", // Needs testing
            CreatorGoalsEventsV1 = "creator-goals-events-v1", // Partial Progress
            CommunityBoostEventsV1 = "community-boost-events-v1", // no events?
            ChannelDropEvents = "channel-drop-events", // Probably not TODO
            ChannelSubGiftsV1 = "channel-sub-gifts-v1",
            ChannelCheerEventsPublicV1 = "channel-cheer-events-public-v1", // no events?
            ChannelBountyBoardEventsCTA = "channel-bounty-board-events.cta", // no events?
            StreamChatRoomV1 = "stream-chat-room-v1", // Seems good
            VideoPlaybackById = "video-playback-by-id",
            Whispers = "whispers",
            CommunityPointsChannelV1 = "community-points-channel-v1",
            ChannelPointsChannelV1 = "channel-points-channel-v1",
            LeaderboardEventsV1 = "leaderboard-events-v1",
            Raid = "raid",
            Polls = "polls",
            PredictionsChannelV1 = "predictions-channel-v1",
            ChannelSubscribeEventsV1 = "channel-subscribe-events-v1", // no events?
            Presence = "presence", // presence.[userId] // no events?
            PvWatchPartyEvents = "pv-watch-party-events", // pv-watch-party-events.[channelId] // no events... probably needs a watch party
            StreamChangeByChannel = "stream-change-by-channel", // stream-change-by-channel.[channelId] // no events?
            ExtensionControl = "extension-control", // extension-control.[channelId] // extension-control.[extensionId] // no events?
            BroadcastSettingsUpdate = "broadcast-settings-update", // broadcast-settings-update.[channelId] // Stream title changes?
            RadioEventsV1 = "radio-events-v1", // radio-events-v1.[channelId] // no events?
            OnsiteNotifications = "onsite-notifications", // onsite-notifications.[userId] // site notifications - bot shouldn't have any so not gonna bother with it
            UserPropertiesUpdate = "user-properties-update", // user-properties-update.[userId] // no events?
            ChatroomsUserV1 = "chatrooms-user-v1", // chatrooms-user-v1.[userId] // no events?
            UserPreferencesUpdateV1 = "user-preferences-update-v1", // user-preferences-update-v1.[userId] // no events?
            UserUnbanRequests = "user-unban-requests", // user-unban-requests.[userId].[channelId]
            ChannelUnbanRequests = "channel-unban-requests", // channel-unban-requests.[userId].[channelId]
            LowTrustUsers = "low-trust-users", // low-trust-users.[userId].[channelId] // no events?
            Ads = "ads", // ads.[channelId]
            AdPropertyRefresh = "ad-property-refresh", // ad-property-refresh.[channelId] // no events?
            UserCampaignEvents = "user-campaign-events", // user-campaign-events.[userId] // no events?
            CommunityMomentsChannelV1 = "community-moments-channel-v1", // community-moments-channel-v1.[channelId] // no events?
            PrivateCallout = "private-callout"; // private-callout.[userId].[channelId] // no events?
                                                // user-drop-events.87332232
                                                // user-preferences-update-v1.87332232
                                                // user-extensionpurchase-events.87332232
                                                // bits-ext-v1-transaction.42022015-uaw3vx1k0ttq74u9b2zfvt768eebh1
                                                // bits-ext-v1-user-transaction.87332232.42022015-uaw3vx1k0ttq74u9b2zfvt768eebh1

    }
}