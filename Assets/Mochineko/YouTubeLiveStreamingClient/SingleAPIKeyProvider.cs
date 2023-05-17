#nullable enable
namespace Mochineko.YouTubeLiveStreamingClient
{
    public sealed class SingleAPIKeyProvider
        : IAPIKeyProvider
    {
        public string APIKey { get; }

        public SingleAPIKeyProvider(string apiKey)
        {
            APIKey = apiKey;
        }

        public bool TryChangeKey()
            => false;
    }
}