# mocap4face-unity 
This is an unofficial initial Unity integration for [mocap4face by Facemoji](https://github.com/facemoji/mocap4face)

## Status

Currently in a very early stage got the following things to complete before it is usable in Unity level.

Android
- [x] Initialize the sdk from Unity to mocap4face android native sdk
- [ ] Start the camera tracker from Unity
- [ ] Get back the blend shapes data form native sdk

iOS
- [ ] Not yet started

## Setup

Suggested to clone this as submodule into your packages folder


```
git submodule add https://github.com/BennyKok/mocap4face-unity.git Packages\mocap4face-unity 
```

### Android Requirements
- This package uses [External Dependency Manager for Unity](https://github.com/googlesamples/unity-jar-resolver), please set it up first!
- The mocap4face aar library is not included, you will need to download in the Facemoji discord server #Dev channel, which require 0.2.0+, after downloading, put it into the Android folder as below.
![](.screenshots/2021-11-18-23-26-52.png)




### iOS Requirements
- Not yet started

## Expected Result

### Android
With everything setup correctly, if you run on Android, and create a script to call out the Initialize with your api key, you will be seeing the log, and the native SDK should be initialized.
```csharp
FacemojiAPI.Instance.Initialize("YOU API KEY");
```
![](.screenshots/2021-11-18-233537.png)

### iOS
Not yet started



## Development

Please chime in the Facemoji discord for further discussion if you wanna help out the Unity integration!!

You can find the discord link in the page here -> [mocap4face GitHub Page](https://github.com/facemoji/mocap4face).