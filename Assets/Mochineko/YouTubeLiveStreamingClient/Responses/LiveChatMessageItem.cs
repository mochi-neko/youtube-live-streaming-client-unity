#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class LiveChatMessageItem
    {
        [JsonProperty("kind"), JsonRequired]
        public string Kind { get; private set; } = string.Empty;
        
        [JsonProperty("etag"), JsonRequired]
        public string Etag { get; private set; } = string.Empty;
        
        [JsonProperty("id"), JsonRequired]
        public string Id { get; private set; } = string.Empty;
        
        [JsonProperty("snippet"), JsonRequired]
        public LiveChatMessageSnippet Snippet { get; private set; } = new();
        
        [JsonProperty("authorDetails"), JsonRequired]
        public AuthorDetails AuthorDetails { get; private set; } = new();
    }
}