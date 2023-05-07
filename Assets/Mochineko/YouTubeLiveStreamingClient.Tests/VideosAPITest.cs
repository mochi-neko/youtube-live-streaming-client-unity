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
    internal sealed class VideosAPITest
    {
        [TestCase("xCyeWN6pSNc")]
        [RequiresPlayMode(false)]
        public async Task GetLiveStreamingDetailsAsyncTest(string videoID)
        {
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/YouTubeLiveStreamingClient.Tests/YouTubeAPIKey.txt");

            var httpClient = new HttpClient();
            var apiKey = await File.ReadAllTextAsync(apiKeyPath, CancellationToken.None);

            var result = await VideosAPI.GetLiveStreamingDetailsAsync(
                httpClient,
                apiKey,
                videoID,
                CancellationToken.None);

            if (result.Failure)
            {
                Debug.LogError(result.ExtractMessage());
                Assert.Fail();
                return;
            }

            Debug.Log($"Success get live streaming details from video ID:{videoID} to result:{JsonConvert.SerializeObject(result.Unwrap())}.");
        }
    }
}