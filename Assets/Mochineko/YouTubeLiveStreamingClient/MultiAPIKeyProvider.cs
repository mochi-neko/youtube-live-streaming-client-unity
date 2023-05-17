#nullable enable
using System;

namespace Mochineko.YouTubeLiveStreamingClient
{
    public sealed class MultiAPIKeyProvider
        : IAPIKeyProvider
    {
        public string APIKey => apiKeys[index];

        private readonly string[] apiKeys;
        private int index = 0;

        public MultiAPIKeyProvider(string[] apiKeys)
        {
            if (apiKeys.Length == 0)
            {
                throw new ArgumentException($"{nameof(apiKeys)} must not be empty.");
            }

            this.apiKeys = apiKeys;
        }

        public bool TryChangeKey()
        {
            index++;

            return index < apiKeys.Length;
        }
    }
}