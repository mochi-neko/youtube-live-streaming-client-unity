#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class MessageDeletedDetails 
    {
        [JsonProperty("deletedMessageId"), JsonRequired]
        public string DeletedMessageId { get; private set; } = string.Empty;
    }
}