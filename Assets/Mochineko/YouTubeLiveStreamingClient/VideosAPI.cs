#nullable enable
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Extensions.NewtonsoftJson;
using Mochineko.Relent.Result;
using Mochineko.Relent.UncertainResult;
using Mochineko.YouTubeLiveStreamingClient.Responses;

namespace Mochineko.YouTubeLiveStreamingClient
{
    /// <summary>
    /// Videos API of YouTube Data API v3.
    /// See also https://developers.google.com/youtube/v3/docs/videos
    /// </summary>
    public static class VideosAPI
    {
        private const string EndPoint = "/videos";
        
        /// <summary>
        /// Gets video information by video ID.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="apiKey"></param>
        /// <param name="videoID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UncertainResultPatternMatchException"></exception>
        /// <exception cref="ResultPatternMatchException"></exception>
        public static async UniTask<IUncertainResult<VideosAPIResponse>>
            GetVideoInformationAsync(
                HttpClient httpClient,
                string apiKey,
                string videoID,
                CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return UncertainResults.FailWithTrace<VideosAPIResponse>(
                    $"Failed because {nameof(apiKey)} is null or empty.");
            }
            
            if (string.IsNullOrEmpty(videoID))
            {
                return UncertainResults.FailWithTrace<VideosAPIResponse>(
                    $"Failed because {nameof(videoID)} is null or empty.");
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace<VideosAPIResponse>(
                    "Retryable because cancellation has been already requested.");
            }

            // Build query parameters
            var parameters = new Dictionary<string, string>()
            {
                ["part"] = "snippet,liveStreamingDetails",
                ["id"] = videoID,
                ["key"] = apiKey,
            };
            var queryParameters = await new FormUrlEncodedContent(parameters)
                .ReadAsStringAsync();

            // Build request message
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                YouTubeAPIURL.BaseURL + EndPoint + "?" + queryParameters);

            // Send request
            HttpResponseMessage responseMessage;
            var apiResult = await UncertainTryFactory
                .TryAsync<HttpResponseMessage>(async innerCancellationToken
                    => await httpClient.SendAsync(requestMessage, innerCancellationToken))
                .CatchAsRetryable<HttpResponseMessage, HttpRequestException>(exception
                    => $"Retryable because -> {exception}.")
                .CatchAsRetryable<HttpResponseMessage, OperationCanceledException>(exception
                    => $"Retryable because -> {exception}.")
                .CatchAsFailure<HttpResponseMessage, Exception>(exception
                    => $"Failure because -> {exception}.")
                .ExecuteAsync(cancellationToken);
            switch (apiResult)
            {
                case IUncertainSuccessResult<HttpResponseMessage> apiSuccess:
                    responseMessage = apiSuccess.Result;
                    break;

                case IUncertainRetryableResult<HttpResponseMessage> apiRetryable:
                    return UncertainResults.RetryWithTrace<VideosAPIResponse>(
                        $"Retryable because -> {apiRetryable.Message}.");

                case IUncertainFailureResult<HttpResponseMessage> apiFailure:
                    return UncertainResults.FailWithTrace<VideosAPIResponse>(
                        $"Failed because -> {apiFailure.Message}.");

                default:
                    throw new UncertainResultPatternMatchException(nameof(apiResult));
            }

            // Succeeded
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseJson))
                {
                    return UncertainResults.FailWithTrace<VideosAPIResponse>(
                        $"Failed because response string was null or empty.");
                }
                
                var deserializeResult = RelentJsonSerializer
                    .Deserialize<VideosAPIResponse>(responseJson);
                switch (deserializeResult)
                {
                    case ISuccessResult<VideosAPIResponse> deserializeSuccess:
                        return UncertainResults.Succeed(deserializeSuccess.Result);

                    case IFailureResult<VideosAPIResponse> deserializeFailure:
                        return UncertainResults.FailWithTrace<VideosAPIResponse>(
                            $"Failed to deserialize json to dictionary because -> {deserializeFailure.Message}, JSON:{responseJson}.");

                    default:
                        throw new ResultPatternMatchException(nameof(deserializeResult));
                }
            }
            // Quota limit exceeded error
            else if (responseMessage.StatusCode is HttpStatusCode.TooManyRequests)
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                return new LimitExceededResult<VideosAPIResponse>(
                    $"Failed because the API key has exceeded quota limit with status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}, response:{errorResponse}.");
            }
            // Retryable
            else if ((int)responseMessage.StatusCode is >= 500 and <= 599)
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                return UncertainResults.RetryWithTrace<VideosAPIResponse>(
                    $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}, response:{errorResponse}.");
            }
            // Response error
            else
            {
                var errorResponse = await responseMessage.Content.ReadAsStringAsync();
                return UncertainResults.FailWithTrace<VideosAPIResponse>(
                    $"Failed because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}, response:{errorResponse}."
                );
            }
        }
    }
}