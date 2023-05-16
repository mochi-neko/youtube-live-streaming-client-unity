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
        private readonly int intervalSeconds;
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
            uint maxResultsOfMessages,
            int intervalSeconds)
        {
            this.httpClient = httpClient;
            this.apiKey = apiKey;
            this.videoID = videoID;
            this.maxResultsOfMessages = maxResultsOfMessages;
            this.intervalSeconds = intervalSeconds;
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
            Debug.Log($"[YouTubeLiveStreamingClient] Getting live chat ID from video ID:{videoID}...");

            var result = await VideosAPI.GetVideoInformationAsync(
                httpClient,
                apiKey,
                videoID,
                cancellationToken);

            VideosAPIResponse response;
            switch (result)
            {
                case IUncertainSuccessResult<VideosAPIResponse> success:
                    response = success.Result;
                    break;

                case IUncertainRetryableResult<VideosAPIResponse> retryable:
                    Debug.Log(
                        $"[YouTubeLiveStreamingClient] Retryable failed to get live chat ID because -> {retryable.Message}.");
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
                Debug.Log($"[YouTubeLiveStreamingClient] Any items are not found in response from video ID:{videoID}.");
                return;
            }

            var liveChatID = response.Items[0].LiveStreamingDetails.ActiveLiveChatId;
            if (!string.IsNullOrEmpty(liveChatID))
            {
                Debug.Log($"[YouTubeLiveStreamingClient] Succeeded to get live chat ID:{liveChatID} from video ID:{videoID}.");
                this.liveChatID = liveChatID;
                onVideoInformationUpdated.OnNext(response);
            }
            else
            {
                Debug.Log($"[YouTubeLiveStreamingClient] LiveChatID is null or empty from video ID:{videoID}.");
            }
        }

        private async UniTask PollLiveChatMessagesAsync(
            string liveChatID,
            CancellationToken cancellationToken)
        {
            Debug.Log($"[YouTubeLiveStreamingClient] Polling live chat messages...");

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
                    Debug.Log(
                        $"[YouTubeLiveStreamingClient] Succeeded to get live chat messages: {success.Result.Items.Count} messages with next page token:{success.Result.NextPageToken}.");
                    response = success.Result;
                    this.nextPageToken = response.NextPageToken;
                    break;

                case IUncertainRetryableResult<LiveChatMessagesAPIResponse> retryable:
                    Debug.Log(
                        $"[YouTubeLiveStreamingClient] Retryable failed to get live chat messages because -> {retryable.Message}.");
                    return;

                case IUncertainFailureResult<LiveChatMessagesAPIResponse> failure:
                    Debug.LogError(
                        $"[YouTubeLiveStreamingClient] Failed to get live chat messages because -> {failure.Message}");
                    return;

                default:
                    throw new UncertainResultPatternMatchException(nameof(result));
            }

            try
            {
                // Send events on the main thread
                await UniTask.SwitchToMainThread(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[YouTubeLiveStreamingClient] Cancelled to switch to main thread.");
                return;
            }

            foreach (var item in response.Items)
            {
                onMessageCollected.OnNext(item);
            }

            await UniTask.SwitchToThreadPool();
        }
    }
}