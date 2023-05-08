using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mochineko.YouTubeLiveStreamingClient.Tests
{
    [TestFixture]
    internal sealed class LiveChatMessagesAPITest
    {
        [TestCase("tcRvI1rSokk")]
        [RequiresPlayMode(false)]
        public async Task GetLiveStreamingDetailsAsyncTest(string videoID)
        {
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/YouTubeLiveStreamingClient.Tests/YouTubeAPIKey.txt");

            var httpClient = new HttpClient();
            var apiKey = await File.ReadAllTextAsync(apiKeyPath, CancellationToken.None);

            var result = await VideosAPI.GetVideoInformationAsync(
                httpClient,
                apiKey,
                videoID,
                CancellationToken.None);
            
            var liveChatID = result.Unwrap().Items[0].LiveStreamingDetails.ActiveLiveChatId;
            
            var liveChatMessagesResult = await LiveChatMessagesAPI.GetLiveChatMessagesAsync(
                httpClient,
                apiKey,
                liveChatID,
                CancellationToken.None);

            if (liveChatMessagesResult.Failure)
            {
                Debug.LogError(
                    $"Failed get live chat messages from live chat ID:{liveChatID} because -> {liveChatMessagesResult.ExtractMessage()}.");
                Assert.Fail();
            }
            
            var liveChatMessages = liveChatMessagesResult.Unwrap().Items;
            foreach (var message in liveChatMessages)
            {
                Debug.Log($"{message.Snippet.Type}:{message.AuthorDetails.DisplayName} -> {message.Snippet.DisplayMessage} at {message.Snippet.PublishedAt}.");
            }
        }
    }
}