#nullable enable
using System.IO;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using Mochineko.YouTubeLiveStreamingClient.Responses;
using UniRx;
using UnityEngine;

namespace Mochineko.YouTubeLiveStreamingClient.Samples
{
    internal sealed class LiveChatMessagesCollectionDemo : MonoBehaviour
    {
        [SerializeField]
        private string apiKeyPath = string.Empty;

        [SerializeField]
        private string videoIDOrURL = string.Empty;

        [SerializeField, Range(200, 2000)]
        private uint maxResultsOfMessages = 500;

        [SerializeField]
        private int intervalSeconds = 5;

        private static readonly HttpClient HttpClient = new();

        private LiveChatMessagesCollector? collector;

        private async void Start()
        {
            var apiKey = await File.ReadAllTextAsync(
                apiKeyPath,
                this.GetCancellationTokenOnDestroy());

            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("[YouTubeLiveStreamingClient.Samples] API Key is null or empty.");
                return;
            }

            var videoID = YouTubeVideoIDExtractor.ExtractIfURL(videoIDOrURL);
            if (string.IsNullOrEmpty(videoID))
            {
                Debug.LogError("[YouTubeLiveStreamingClient.Samples] Video ID is null or empty.");
                return;
            }

            collector = new LiveChatMessagesCollector(
                HttpClient,
                apiKey,
                videoID,
                maxResultsOfMessages,
                intervalSeconds);

            collector
                .OnVideoInformationUpdated
                .Subscribe(OnVideoInformationUpdated)
                .AddTo(this);

            collector
                .OnMessageCollected
                .Subscribe(OnMessageCollected)
                .AddTo(this);
            
            collector.BeginCollection();
        }

        private void OnDestroy()
        {
            collector?.Dispose();
        }
        
        private void OnVideoInformationUpdated(VideosAPIResponse response)
        {
            Debug.Log(
                $"[YouTubeLiveStreamingClient.Samples] Update video information: {response.Items[0].LiveStreamingDetails.ActiveLiveChatId}.");
        }

        private void OnMessageCollected(LiveChatMessageItem message)
        {
            Debug.Log(
                $"[YouTubeLiveStreamingClient.Samples] Collect message: [{message.Snippet.Type}] {message.Snippet.DisplayMessage} from {message.AuthorDetails.DisplayName} at {message.Snippet.PublishedAt}.");
        }
    }
}