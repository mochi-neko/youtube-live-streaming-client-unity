#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class VideoSnippet
    {
        [JsonProperty("publishedAt"), JsonRequired]
        public DateTime PublishedAt { get; private set; }

        [JsonProperty("channelId"), JsonRequired]
        public string ChannelId { get; private set; } = string.Empty;

        [JsonProperty("title"), JsonRequired]
        public string Title { get; private set; } = string.Empty;

        [JsonProperty("description"), JsonRequired]
        public string Description { get; private set; } = string.Empty;

        [JsonProperty("thumbnails"), JsonRequired]
        public VideoThumbnails Thumbnails { get; private set; } = new();

        [JsonProperty("channelTitle"), JsonRequired]
        public string ChannelTitle { get; private set; } = string.Empty;

        [JsonProperty("tags")]
        public string[]? Tags { get; private set; }

        [JsonProperty("categoryId"), JsonRequired]
        public string CategoryId { get; private set; } = string.Empty;
        
        [JsonProperty("liveBroadcastContent"), JsonRequired]
        public string LiveBroadcastContent { get; private set; } = string.Empty;
        
        [JsonProperty("defaultLanguage")]
        public string? DefaultLanguage { get; private set; }
        
        [JsonProperty("localized"), JsonRequired]
        public Localized Localized { get; private set; } = new();
        
        [JsonProperty("defaultAudioLanguage")]
        public string? DefaultAudioLanguage { get; private set; }
    }
}