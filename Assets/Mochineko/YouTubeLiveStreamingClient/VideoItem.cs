#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient
{
    [JsonObject]
    internal sealed class VideoItem
    {
        [JsonProperty("kind"), JsonRequired] public string Kind { get; private set; } = string.Empty;

        [JsonProperty("etag"), JsonRequired] public string Etag { get; private set; } = string.Empty;

        [JsonProperty("id"), JsonRequired] public string Id { get; private set; } = string.Empty;

        [JsonProperty("liveStreamingDetails"), JsonRequired]
        public LiveStreamingDetails LiveStreamingDetails { get; private set; } = new();
    }
}