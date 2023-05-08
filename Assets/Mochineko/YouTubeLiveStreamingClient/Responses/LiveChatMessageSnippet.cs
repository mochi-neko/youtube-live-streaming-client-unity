#nullable enable
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class LiveChatMessageSnippet
    {
        [JsonProperty("type"), JsonRequired, JsonConverter(typeof(StringEnumConverter))]
        public LiveChatMessageType Type { get; private set; }

        [JsonProperty("liveChatId"), JsonRequired]
        public string LiveChatId { get; private set; } = string.Empty;

        [JsonProperty("authorChannelId"), JsonRequired]
        public string AuthorChannelId { get; private set; } = string.Empty;

        [JsonProperty("publishedAt"), JsonRequired]
        public DateTime PublishedAt { get; private set; }

        [JsonProperty("hasDisplayContent"), JsonRequired]
        public bool HasDisplayContent { get; private set; }

        [JsonProperty("displayMessage"), JsonRequired]
        public string DisplayMessage { get; private set; } = string.Empty;

        [JsonProperty("textMessageDetails")]
        public TextMessageDetails? TextMessageDetails { get; private set; }

        [JsonProperty("messageDeletedDetails")]
        public MessageDeletedDetails? MessageDeletedDetails { get; private set; }
        
        [JsonProperty("userBannedDetails")]
        public UserBannedDetails? UserBannedDetails { get; private set; }
        
        [JsonProperty("memberMilestoneChatDetails")]
        public MemberMilestoneChatDetails? MemberMilestoneChatDetails { get; private set; }
        
        [JsonProperty("newSponsorDetails")]
        public NewSponsorDetails? NewSponsorDetails { get; private set; }
        
        [JsonProperty("superChatDetails")]
        public SuperChatDetails? SuperChatDetails { get; private set; }

        [JsonProperty("superStickerDetails")]
        public SuperStickerDetails? SuperStickerDetails { get; private set; }
        
        [JsonProperty("membershipGiftingDetails")]
        public MembershipGiftingDetails? MembershipGiftingDetails { get; private set; }
        
        [JsonProperty("giftMembershipReceivedDetails")]
        public GiftMembershipReceivedDetails? GiftMembershipReceivedDetails { get; private set; }
    }
}