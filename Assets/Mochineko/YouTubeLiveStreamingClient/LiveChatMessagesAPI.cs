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
using UnityEngine;

namespace Mochineko.YouTubeLiveStreamingClient
{
    public static class LiveChatMessagesAPI
    {
        private const string EndPoint = "/liveChat/messages";

        public static async UniTask<IUncertainResult<LiveChatMessagesAPIResponse>>
            GetLiveChatMessagesAsync(
                HttpClient httpClient,
                string apiKey,
                string liveChatID,
                CancellationToken cancellationToken,
                string? pageToken = null,
                uint? maxResults = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
                    $"Failed because {nameof(apiKey)} is null or empty.");
            }

            if (string.IsNullOrEmpty(liveChatID))
            {
                return UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
                    $"Failed because {nameof(liveChatID)} is null or empty.");
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace<LiveChatMessagesAPIResponse>(
                    "Retryable because cancellation has been already requested.");
            }

            // Build path parameters
            var parameters = new Dictionary<string, string>()
            {
                ["part"] = "id,snippet,authorDetails",
                ["liveChatId"] = liveChatID,
                ["key"] = apiKey,
            };

            if (!string.IsNullOrEmpty(pageToken))
            {
                parameters.Add("pageToken", pageToken);
            }

            if (maxResults != null)
            {
                parameters.Add("maxResults", maxResults.ToString());
            }

            var pathParameters = await new FormUrlEncodedContent(parameters)
                .ReadAsStringAsync();

            // Build request message
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                YouTubeAPIURL.BaseURL + EndPoint + "?" + pathParameters);

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
                    return UncertainResults.RetryWithTrace<LiveChatMessagesAPIResponse>(
                        $"Retryable because -> {apiRetryable.Message}.");

                case IUncertainFailureResult<HttpResponseMessage> apiFailure:
                    return UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
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
                    return UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
                        $"Failed because response string was null or empty.");
                }
                
                Debug.Log($"[YouTubeLiveStreamingClient] Live chat messages API response from live chat ID:{liveChatID},\n{responseJson}.");

                var deserializeResult = RelentJsonSerializer
                    .Deserialize<LiveChatMessagesAPIResponse>(responseJson);
                return deserializeResult switch
                {
                    ISuccessResult<LiveChatMessagesAPIResponse> deserializeSuccess
                        => UncertainResults.Succeed(deserializeSuccess.Result),

                    IFailureResult<LiveChatMessagesAPIResponse> deserializeFailure
                        => UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
                            $"Failed to deserialize json to dictionary because -> {deserializeFailure.Message}, JSON:{responseJson}."),

                    _ => throw new ResultPatternMatchException(nameof(deserializeResult))
                };
            }
            // Retryable
            else if (responseMessage.StatusCode is HttpStatusCode.TooManyRequests
                     || (int)responseMessage.StatusCode is >= 500 and <= 599)
            {
                return UncertainResults.RetryWithTrace<LiveChatMessagesAPIResponse>(
                    $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}.");
            }
            // Response error
            else
            {
                return UncertainResults.FailWithTrace<LiveChatMessagesAPIResponse>(
                    $"Failed because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}."
                );
            }
        }
    }
}