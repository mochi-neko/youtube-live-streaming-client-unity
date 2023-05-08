#nullable enable
using System;

namespace Mochineko.YouTubeLiveStreamingClient
{
    public static class YouTubeVideoIDExtractor
    {
        public static string ExtractFromURL(string url)
        {
            var uri = new Uri(url);
            var query = uri.Query;
            var parameters = System.Web.HttpUtility.ParseQueryString(query);
            return parameters["v"];
        }
        
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