#nullable enable
using Mochineko.Relent.UncertainResult;

namespace Mochineko.YouTubeLiveStreamingClient
{
    public sealed class LimitExceededResult<T>
        : IUncertainFailureResult<T>
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message { get; }
        
        public LimitExceededResult(string message)
        {
            Message = message;
        }
    }
}