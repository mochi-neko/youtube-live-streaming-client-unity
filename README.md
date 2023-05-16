# youtube-live-streaming-client-unity
A client library for YouTube [Data](https://developers.google.com/youtube/v3/getting-started)/[Live Streaming](https://developers.google.com/youtube/v3/live/docs) API v3 to get [live chat messages](https://developers.google.com/youtube/v3/live/docs/liveChatMessages) for Unity.

## Features

- Videos API to get live chat ID by video ID.
- Live streaming messages API by live chat ID.
- Live streaming messages collection as UniRx event.

## How to import by UnityPackageManager

Add dependencies:

```json
{
    "dependencies": {
        "com.mochineko.youtube-live-streaming-client": "https://github.com/mochi-neko/youtube-live-streaming-client-unity.git?path=/Assets/Mochineko/YouTubeLiveStreamingClient#0.2.0",
        "com.mochineko.relent": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent#0.2.0",
        "com.mochineko.relent.extensions.newtonsoft-json": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent.Extensions/NewtonsofJson#0.2.0",
        "com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
        "com.neuecc.unirx": "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts",
        ...
    }
}
```

to your `manifest.json`.

## How to use

Standard workflow is as follows.

1. Publish your API key of YouTube Data API v3.
2. Get live chat ID from videos API with API key and video ID.
3. Get live chat messages from live streaming messages API with API key and live chat ID.

You can select from 2 ways to do above workflow.
- Directly use `VideosAPI` and `LiveChatMessagesAPI`.
  - See [test code](https://github.com/mochi-neko/youtube-live-streaming-client-unity/blob/main/Assets/Mochineko/YouTubeLiveStreamingClient.Tests/LiveChatMessagesAPITest.cs).
- Use `LiveChatMessagesCollector` that polls `LiveChatMessagesAPI` and provides messages as event.
  - See [sample code](https://github.com/mochi-neko/youtube-live-streaming-client-unity/blob/main/Assets/Mochineko/YouTubeLiveStreamingClient.Samples/LiveChatMessagesCollectionDemo.cs). 

## Changelog

See [CHANGELOG](https://github.com/mochi-neko/youtube-live-streaming-client-unity/blob/main/CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](https://github.com/mochi-neko/youtube-live-streaming-client-unity/blob/main/NOTICE.md).

## License

Licensed under the [MIT](https://github.com/mochi-neko/youtube-live-streaming-client-unity/blob/main/LICENSE) license.
