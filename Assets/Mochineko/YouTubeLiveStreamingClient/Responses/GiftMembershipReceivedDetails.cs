#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class GiftMembershipReceivedDetails
    {
        [JsonProperty("memberLevelName"), JsonRequired]
        public string MemberLevelName { get; private set; } = string.Empty;
        
        [JsonProperty("gifterChannelId"), JsonRequired]
        public string GifterChannelId { get; private set; } = string.Empty;
        
        [JsonProperty("associatedMembershipGiftingMessageId"), JsonRequired]
        public string AssociatedMembershipGiftingMessageId { get; private set; } = string.Empty;
    }
}