#nullable enable
namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    // ReSharper disable InconsistentNaming
    public enum LiveChatMessageType
    {
        chatEndedEvent,
        messageDeletedEvent,
        sponsorOnlyModeEndedEvent,
        sponsorOnlyModeStartedEvent,
        newSponsorEvent,
        memberMilestoneChatEvent,
        superChatEvent,
        superStickerEvent,
        textMessageEvent,
        tombstone,
        userBannedEvent,
        membershipGiftingEvent,
        giftMembershipReceivedEvent,
    }
}