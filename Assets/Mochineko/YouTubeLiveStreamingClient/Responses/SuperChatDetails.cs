#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class SuperChatDetails
    {
        [JsonProperty("amountMicros"), JsonRequired]
        public string AmountMicros { get; private set; } = string.Empty;

        [JsonProperty("currency"), JsonRequired]
        public string Currency { get; private set; } = string.Empty;

        [JsonProperty("amountDisplayString"), JsonRequired]
        public string AmountDisplayString { get; private set; } = string.Empty;
        
        [JsonProperty("userComment")]
        public string? UserComment { get; private set; }

        [JsonProperty("tier"), JsonRequired]
        public uint Tier { get; private set; }

    }
}