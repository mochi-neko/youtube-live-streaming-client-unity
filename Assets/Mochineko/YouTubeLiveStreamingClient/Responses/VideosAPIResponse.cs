#nullable enable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class VideosAPIResponse
    {
        [JsonProperty("kind"), JsonRequired]
        public string Kind { get; private set; } = string.Empty;

        [JsonProperty("etag"), JsonRequired]
        public string Etag { get; private set; } = string.Empty;

        [JsonProperty("items"), JsonRequired]
        public List<VideoItem> Items { get; private set; } = new();

        [JsonProperty("pageInfo"), JsonRequired]
        public PageInfo PageInfo { get; private set; } = new();
    }
}