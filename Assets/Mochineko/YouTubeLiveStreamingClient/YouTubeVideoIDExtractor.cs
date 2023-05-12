#nullable enable
using System.Text.RegularExpressions;

namespace Mochineko.YouTubeLiveStreamingClient
{
    /// <summary>
    /// Provides a method to extract a video ID from a video URL.
    /// </summary>
    public static class YouTubeVideoIDExtractor
    {
        /// <summary>
        /// Tries to extract a video ID from a raw video ID or a video URL.
        /// </summary>
        /// <param name="videoIDOrURL"></param>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public static bool TryExtractVideoId(string videoIDOrURL, out string videoId)
        {
            var regexForRegularUrl = new Regex(@"youtube\.com.*(\?v=|\/v\/)([^&]+)");
            var regexForShortUrl = new Regex(@"youtu\.be\/([^?&]+)");
            var regexForVideoId = new Regex(@"^[a-zA-Z0-9_-]{11}$"); // Standard YouTube video ID format

            var matchForRegularUrl = regexForRegularUrl.Match(videoIDOrURL);
            if (matchForRegularUrl.Success)
            {
                videoId = matchForRegularUrl.Groups[2].Value;
                return true;
            }

            var matchForShortUrl = regexForShortUrl.Match(videoIDOrURL);
            if (matchForShortUrl.Success)
            {
                videoId = matchForShortUrl.Groups[1].Value;
                return true;
            }

            var matchForVideoId = regexForVideoId.Match(videoIDOrURL);
            if (matchForVideoId.Success)
            {
                videoId = videoIDOrURL;
                return true;
            }
            else
            {
                videoId = string.Empty;
                return false;
            }
        }
    }
}