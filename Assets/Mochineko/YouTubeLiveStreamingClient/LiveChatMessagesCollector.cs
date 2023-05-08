#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.UncertainResult;
using Mochineko.YouTubeLiveStreamingClient.Responses;
using UniRx;
using UnityEngine;

namespace Mochineko.YouTubeLiveStreamingClient
{
    public sealed class LiveChatMessagesCollector : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly string videoID;
        private readonly uint maxResultsOfMessages;
        private readonly int intervalSeconds;
        private readonly CancellationTokenSource cancellationTokenSource;
        
        private readonly List<LiveChatMessageItem> messages = new();

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
            this.cancellationTokenSource = new();
        }

        public void Dispose()
        {
            cancellationTokenSource.Dispose();
        }

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
            }
            else
            {
                await PollLiveChatMessagesAsync(liveChatID, cancellationToken);
            }
        }

        private async UniTask GetLiveChatIDAsync(CancellationToken cancellationToken)
        {
            Debug.Log($"[YouTubeLiveStreamingClient] Getting live chat ID...");
            
            var result = await VideosAPI.GetLiveStreamingDetailsAsync(
                httpClient,
                apiKey,
                videoID,
                cancellationToken);

            switch (result)
            {
                case IUncertainSuccessResult<LiveStreamingDetails> success:
                    var liveChatID = success.Result.ActiveLiveChatId;
                    if (!string.IsNullOrEmpty(liveChatID))
                    {
                        Debug.Log($"[YouTubeLiveStreamingClient] Succeeded to get live chat ID:{liveChatID}.");
                        this.liveChatID = liveChatID;
                    }
                    else
                    {
                        Debug.Log($"[YouTubeLiveStreamingClient] LiveChatID is null or empty.");
                    }
                    break;

                case IUncertainRetryableResult<LiveStreamingDetails> retryable:
                    Debug.Log($"[YouTubeLiveStreamingClient] Retryable failed to get live chat ID because -> {retryable.Message}.");
                    break;

                case IUncertainFailureResult<LiveStreamingDetails> failure:
                    Debug.LogError($"[YouTubeLiveStreamingClient] Failed to get live chat ID because -> {failure.Message}");
                    break;
                
                default:
                    throw new UncertainResultPatternMatchException(nameof(result));
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
                    Debug.Log($"[YouTubeLiveStreamingClient] Succeeded to get live chat messages: {success.Result.Items.Count} messages with next page token:{success.Result.NextPageToken}.");
                    response = success.Result;
                    this.nextPageToken = response.NextPageToken;
                    break;

                case IUncertainRetryableResult<LiveChatMessagesAPIResponse> retryable:
                    Debug.Log($"[YouTubeLiveStreamingClient] Retryable failed to get live chat messages because -> {retryable.Message}.");
                    return;

                case IUncertainFailureResult<LiveChatMessagesAPIResponse> failure:
                    Debug.LogError($"[YouTubeLiveStreamingClient] Failed to get live chat messages because -> {failure.Message}");
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
                messages.Add(item);
                onMessageCollected.OnNext(item);
            }

            await UniTask.SwitchToThreadPool();
        }
    }
}