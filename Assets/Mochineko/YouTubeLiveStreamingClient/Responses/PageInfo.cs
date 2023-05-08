#nullable enable
using Newtonsoft.Json;

namespace Mochineko.YouTubeLiveStreamingClient.Responses
{
    [JsonObject]
    public sealed class PageInfo
    {
        [JsonProperty("totalResults"), JsonRequired]
        public int TotalResults { get; private set; }

        [JsonProperty("resultsPerPage"), JsonRequired]
        public int ResultsPerPage { get; private set; }
    }
}