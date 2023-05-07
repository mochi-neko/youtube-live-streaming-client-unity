#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient
{
    [JsonObject]
    public sealed class LiveStreamingDetails
    {
        [JsonProperty("actualStartTime"), JsonRequired]
        public DateTime ActualStartTime { get; private set; }

        [JsonProperty("scheduledStartTime"), JsonRequired]
        public DateTime ScheduledStartTime { get; private set; }

        [JsonProperty("concurrentViewers"), JsonRequired]
        public int ConcurrentViewers { get; private set; }

        [JsonProperty("activeLiveChatId"), JsonRequired]
        public string ActiveLiveChatId { get; private set; } = string.Empty;
    }
}