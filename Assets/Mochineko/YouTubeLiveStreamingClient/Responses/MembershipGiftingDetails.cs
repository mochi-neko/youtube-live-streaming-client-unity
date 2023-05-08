#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class MembershipGiftingDetails
    {
        [JsonProperty("giftMembershipsCount"), JsonRequired]
        public int GiftMembershipsCount { get; private set; }

        [JsonProperty("giftMembershipsLevelName"), JsonRequired]
        public string GiftMembershipsLevelName { get; private set; } = string.Empty;
    }
}