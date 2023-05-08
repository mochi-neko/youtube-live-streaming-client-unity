#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class SuperStickerMetadata
    {
        [JsonProperty("stickerId"), JsonRequired]
        public string StickerId { get; private set; } = string.Empty;
        
        [JsonProperty("altText"), JsonRequired]
        public string AltText { get; private set; } = string.Empty;
        
        [JsonProperty("altTextLanguage"), JsonRequired]
        public string AltTextLanguage { get; private set; } = string.Empty;
    }
}