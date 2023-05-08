#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class PageInfo
    {
        [JsonProperty("totalResults"), JsonRequired]
        public uint TotalResults { get; private set; }

        [JsonProperty("resultsPerPage"), JsonRequired]
        public uint ResultsPerPage { get; private set; }
    }
}