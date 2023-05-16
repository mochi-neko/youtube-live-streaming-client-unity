#nullable enable
using System;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.UncertainResult;
using Mochineko.YouTubeLiveStreamingClient.Responses;
using UniRx;
using UnityEngine;

namespace Mochineko.YouTubeLiveStreamingClient
{
    /// <summary>
    /// Collects and provides live chat messages from YouTube Data API v3.
    /// </summary>
    public sealed class LiveChatMessagesCollector : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly string videoID;
        private readonly uint maxResultsOfMessages;
        private readonly float intervalSeconds;
        private readonly bool verbose;
        private readonly CancellationTokenSource cancellationTokenSource = new();

        private readonly Subject<VideosAPIResponse> onVideoInformationUpdated = new();
        public IObservable<VideosAPIResponse> OnVideoInformationUpdated => onVideoInformationUpdated;

        private readonly Subject<LiveChatMessageItem> onMessageCollected = new();
        public IObservable<LiveChatMessageItem> OnMessageCollected => onMessageCollected;

        private bool isCollecting = false;
        private string? liveChatID = null;
        private string? nextPageToken = null;

        public LiveChatMessagesCollector(
            HttpClient httpClient,
            string apiKey,
            string videoID,
            uint maxResultsOfMessages = 500,
            float intervalSeconds = 5f,
            bool verbose = true)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException($"{nameof(apiKey)} must no be empty.");
            }

            if (string.IsNullOrEmpty(videoID))
            {
                throw new ArgumentException($"{nameof(videoID)} must no be empty.");
            }

            this.httpClient = httpClient;
            this.apiKey = apiKey;
            this.videoID = videoID;
            this.maxResultsOfMessages = maxResultsOfMessages;
            this.intervalSeconds = intervalSeconds;
            this.verbose = verbose;
        }

        public void Dispose()
        {
            cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Begins collecting live chat messages.
        /// </summary>
        public void BeginCollection()
        {
            if (isCollecting)
            {
                return;
            }

            isCollecting = true;

            BeginCollectionAsync(cancellationTokenSource.Token)
                .Forget();
        }

        private async UniTask BeginCollectionAsync(
            CancellationToken cancellationToken)
        {
            await UniTask.SwitchToThreadPool();

            while (!cancellationToken.IsCancellationRequested)
            {
                await UpdateAsync(cancellationToken);

                try
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(intervalSeconds),
                        cancellationToken: cancellationToken
                    );
                }
                // Catch cancellation
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async UniTask UpdateAsync(CancellationToken cancellationToken)
        {
            if (liveChatID == null)
            {
                await GetLiveChatIDAsync(cancellationToken);

                // Succeeded to get live chat ID
                if (liveChatID != null)
                {
                    await PollLiveChatMessagesAsync(liveChatID, cancellationToken);
                }
            }
            else
            {
                await PollLiveChatMessagesAsync(liveChatID, cancellationToken);
            }
        }

        private async UniTask GetLiveChatIDAsync(CancellationToken cancellationToken)
        {
            if (verbose)
            {
                Debug.Log($"[YouTubeLiveStreamingClient] Getting live chat ID from video ID:{videoID}...");
            }

            var result = await VideosAPI.GetVideoInformationAsync(
                httpClient,
                apiKey,
                videoID,
                cancellationToken);

            VideosAPIResponse response;
            switch (result)
            {
                case IUncertainSuccessResult<VideosAPIResponse> success:
                    if (verbose)
                    {
                        Debug.Log($"[YouTubeLiveStreamingClient] Succeeded to get video API response.");
                    }
                    response = success.Result;
                    break;

                case IUncertainRetryableResult<VideosAPIResponse> retryable:
                    if (verbose)
                    {
                        Debug.Log(
                            $"[YouTubeLiveStreamingClient] Retryable failed to get live chat ID because -> {retryable.Message}.");
                    }

                    return;

                case IUncertainFailureResult<VideosAPIResponse> failure:
                    Debug.LogError(
                        $"[YouTubeLiveStreamingClient] Failed to get live chat ID because -> {failure.Message}");
                    return;

                default:
                    throw new UncertainResultPatternMatchException(nameof(result));
            }

            if (response.Items.Count == 0)
            {
                if (verbose)
                {
                    Debug.Log($"[YouTubeLiveStreamingClient] No items are found in response from video ID:{videoID}.");
                }

                return;
            }

            var liveChatID = response.Items[0].LiveStreamingDetails.ActiveLiveChatId;
            if (!string.IsNullOrEmpty(liveChatID))
            {
                if (verbose)
                {
                    Debug.Log(
                        $"[YouTubeLiveStreamingClient] Succeeded to get live chat ID:{liveChatID} from video ID:{videoID}.");
                }

                this.liveChatID = liveChatID;
                onVideoInformationUpdated.OnNext(response);
            }
            else
            {
                Debug.LogError($"[YouTubeLiveStreamingClient] LiveChatID is null or empty from video ID:{videoID}.");
            }
        }

        private async UniTask PollLiveChatMessagesAsync(
            string liveChatID,
            CancellationToken cancellationToken)
        {
            if (verbose)
            {
                Debug.Log($"[YouTubeLiveStreamingClient] Polling live chat messages...");
            }

            var result = await LiveChatMessagesAPI.GetLiveChatMessagesAsync(
                httpClient,
                apiKey,
                liveChatID,
                cancellationToken,
                pageToken: nextPageToken,
                maxResults: maxResultsOfMessages);

            LiveChatMessagesAPIResponse response;
            switch (result)
            {
                case IUncertainSuccessResult<LiveChatMessagesAPIResponse> success:
                    if (verbose)
                    {
                        Debug.Log(
                            $"[YouTubeLiveStreamingClient] Succeeded to get live chat messages: {success.Result.Items.Count} messages with next page token:{success.Result.NextPageToken}.");
                    }

                    response = success.Result;
                    this.nextPageToken = response.NextPageToken;
                    break;

                case IUncertainRetryableResult<LiveChatMessagesAPIResponse> retryable:
                    if (verbose)
                    {
                        Debug.Log(
                            $"[YouTubeLiveStreamingClient] Retryable failed to get live chat messages because -> {retryable.Message}.");
                    }

                    return;

                case IUncertainFailureResult<LiveChatMessagesAPIResponse> failure:
                    Debug.LogError(
                        $"[YouTubeLiveStreamingClient] Failed to get live chat messages because -> {failure.Message}");
                    return;

                default:
                    throw new UncertainResultPatternMatchException(nameof(result));
            }

            foreach (var item in response.Items)
            {
                if (verbose)
                {
                    Debug.Log(
                        $"[YouTubeLiveStreamingClient] Collected live chat message: {item.Snippet.DisplayMessage} from {item.AuthorDetails.DisplayName} at {item.Snippet.PublishedAt}.");
                }
                
                onMessageCollected.OnNext(item);
            }
        }
    }
}