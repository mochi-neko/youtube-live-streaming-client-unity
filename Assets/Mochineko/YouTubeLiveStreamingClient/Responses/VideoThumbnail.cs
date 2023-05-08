#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class VideoThumbnail
    {
        [JsonProperty("url"), JsonRequired]
        public string Url { get; private set; } = string.Empty;

        [JsonProperty("width"), JsonRequired]
        public uint Width { get; private set; }

        [JsonProperty("height"), JsonRequired]
        public uint Height { get; private set; }
    }
}