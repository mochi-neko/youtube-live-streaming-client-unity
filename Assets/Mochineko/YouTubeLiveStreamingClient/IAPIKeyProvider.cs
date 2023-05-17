#nullable enable

namespace Mochineko.YouTubeLiveStreamingClient
{
    public interface IAPIKeyProvider
    {
        string APIKey { get; }

        bool TryChangeKey();
    }
}