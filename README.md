# VTS-Sharp
A Unity C# client interface for creating VTube Studio Plugins with the [official Vtube Studio API](https://github.com/DenchiSoft/VTubeStudio)!

## About
This library is maintained by Tom "Skeletom" Farro. If you need to contact him, the best way to do so is via [Twitter](https://www.twitter.com/fomtarro) or by leaving an issue ticket on this repo. Currently, this library is still in beta, but should be fully functional.

## Usage

In order to start making a plugin, simply make a class which extends `VTSPlugin`. In your class, call the `Initialize` method and pass in your preferred implementations of a JSON utility and of a websocket. Thats it. From there, you can call any method found in the [official Vtube Studio API](https://github.com/DenchiSoft/VTubeStudio).

You can find an example of custom plugin creation in the `Examples` folder, which also includes default implemntations of the aformentioned JSON utility and websocket connector.

Because this library simply acts as an client interface for the official API, please check out the official API's readme for in-depth explanations about the API functionality.

## Design Pattern and Considerations

In order to afford the most flexibility (and to be as decoupled from Unity as possible), the `VTSPlugin` class requires that implemntations of a JSON utility and of a websocket be provided. There are bare-bones examples provided by default, but the design pattern makes it easy for you to swap them out for better/more robust/more platform compliant alternatives, via the following method:

```
    public void Initialize(IWebSocket webSocket, IJsonUtility jsonUtility)
```

Because the VTube Studio API is websocket-based, all calls to it are inherently asynchronous. Therefore, this library follows a callback-based design pattern.

Take, for example, the following method signature, found in the `VTSPlugin` class:

```
    public void GetAPIState(Action<VTSStateData> onSuccess, Action<VTSErrorData> onError)
```
The method accepts two callbacks, `onSuccess` and `onError`. 

Upon the request being processed by Vtube Studio, 
one of these two callbacks will be invoked, depending on if the request was successful or not. 

Additionally, the callback will be passed a single argument, which represents the response data. Each method in the plugin API has a unique data type for its `onSuccess` callback, so the callbacks can be tailored for the type of call being made, in a strongly-typed way.