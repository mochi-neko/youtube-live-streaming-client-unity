#nullable enable
using System;

namespace Mochineko.YouTubeLiveStreamingClient
{
    /// <summary>
    /// Provides a method to extract a video ID from a video URL.
    /// </summary>
    public static class YouTubeVideoIDExtractor
    {
        /// <summary>
        /// Extracts a video ID from a video URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ExtractFromURL(string url)
        {
            var uri = new Uri(url);
            var query = uri.Query;
            var parameters = System.Web.HttpUtility.ParseQueryString(query);
            return parameters["v"];
        }

        /// <summary>
        /// Extracts a video ID from a video URL if the text is URL, otherwise returns the original text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExtractIfURL(string text)
        {
            if (Uri.TryCreate(text, UriKind.Absolute, out var uri))
            {
                var query = uri.Query;
                var parameters = System.Web.HttpUtility.ParseQueryString(query);
                return parameters["v"];
            }
            else
            {
                return text;
            }
        }
    }
}