#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class UserBannedDetails
    {
        [JsonProperty("bannedUserDetails"), JsonRequired]
        public BannedUserDetails BannedUserDetails { get; private set; } = new();
        
        [JsonProperty("banType"), JsonRequired]
        public string BanType { get; private set; } = string.Empty;
        
        [JsonProperty("banDurationSeconds"), JsonRequired]
        public ulong BanDurationSeconds { get; private set; }
    }
}