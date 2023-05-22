# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## Added
- Add multiple API key option.

## Changed
- Change API result for 429(TooManyRequests) status code to original result:`LimitExceededResult<T>`.

## [0.2.1] - 2023-05-22

## Changed
- Revert thread of publishing events on `LiveStreamingMessagesCollector` to the main thread.

## [0.2.0] - 2023-05-16

## Added
- Add verbose option to `LiveStreamingMessagesCollector`.
- Add dynamic interval option to `LiveStreamingMessagesCollector`.
- Add string validations to `LiveStreamingMessagesCollector`.

## Changed
- Change thread of publishing events on `LiveStreamingMessagesCollector` to a thread pool.

## Fixed
- Fix type of interval option of `LiveStreamingMessagesCollector` from `int` to `float`.

## [0.1.2] - 2023-05-16

## Removed
- Remove messages cache to prevent excessive memory usage.

## [0.1.1] - 2023-05-12

## Fixed
- Improve parameters requirement of videos API.
- Improve video ID extraction.

## [0.1.0] - 2023-05-08

### Added
- Add videos API to get live chat ID.
- Add live streaming messages API.
- Add live streaming messages collector with UniTask and UniRx.