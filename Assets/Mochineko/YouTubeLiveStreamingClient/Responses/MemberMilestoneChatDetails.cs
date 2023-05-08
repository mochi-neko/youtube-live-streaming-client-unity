#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class MemberMilestoneChatDetails
    {
        [JsonProperty("userComment"), JsonRequired]
        public string UserComment { get; private set; } = string.Empty;
        
        [JsonProperty("memberMonth"), JsonRequired]
        public uint MemberMonth { get; private set; }

        [JsonProperty("memberLevelName"), JsonRequired]
        public string MemberLevelName { get; private set; } = string.Empty;
    }
}