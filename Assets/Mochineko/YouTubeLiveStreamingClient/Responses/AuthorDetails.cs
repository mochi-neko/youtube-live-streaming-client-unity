#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class AuthorDetails
    {
        [JsonProperty("channelId"), JsonRequired]
        public string ChannelId { get; private set; } = string.Empty;
        
        [JsonProperty("channelUrl"), JsonRequired]
        public string ChannelUrl { get; private set; } = string.Empty;
        
        [JsonProperty("displayName"), JsonRequired]
        public string DisplayName { get; private set; } = string.Empty;
        
        [JsonProperty("profileImageUrl"), JsonRequired]
        public string ProfileImageUrl { get; private set; } = string.Empty;
        
        [JsonProperty("isVerified"), JsonRequired]
        public bool IsVerified { get; private set; }
        
        [JsonProperty("isChatOwner"), JsonRequired]
        public bool IsChatOwner { get; private set; }
        
        [JsonProperty("isChatSponsor"), JsonRequired]
        public bool IsChatSponsor { get; private set; }
        
        [JsonProperty("isChatModerator"), JsonRequired]
        public bool IsChatModerator { get; private set; }
    }
}