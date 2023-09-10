# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## Added
- Add multiple API key option.

## Changed
- Change API result for rate limit exceeded (= 429:TooManyRequests) to original result:`LimitExceededResult<T>`.

## [0.2.5] - 2023-09-10

### Fixed
- Fix cancellation in `LiveStreamingMessagesCollector`.
- Fix disposing in `LiveStreamingMessagesCollector`.

## [0.2.4] - 2023-07-24

### Fixed
- Fix `VideoThumbnailDetails.Maxres` to be optional.
- Fix `LiveStreamingDetails.ActuralStartTime` to be optional.

## [0.2.3] - 2023-07-12

### Fixed
- Fix dependencies in `package.json`.

## [0.2.2] - 2023-06-27

### Fixed
- Fix `userComment` to optional in `SuperChatDetails`.

### Changed
- Set dependencies to `package.json`.

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