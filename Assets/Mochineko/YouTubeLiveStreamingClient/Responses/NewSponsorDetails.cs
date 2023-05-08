#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class NewSponsorDetails
    {
        [JsonProperty("memberLevelName"), JsonRequired]
        public string MemberLevelName { get; private set; } = string.Empty;
        
        [JsonProperty("isUpgrade")]
        public bool IsUpgrade { get; private set; }
    }
}