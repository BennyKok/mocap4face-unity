# mocap4face-unity 
This is an unofficial initial Unity integration for [mocap4face by Facemoji](https://github.com/facemoji/mocap4face)

## Status

Currently in a very early stage, need to get the following things to complete before it is usable in Unity.

Android
- [x] Initialize the sdk from Unity to mocap4face android native sdk
- [x] Start the camera tracker from Unity
- [x] Get back the blend shapes data form native sdk
- [x] Demo scene
- [x] Better data passing, from Android to unity;

iOS
- [ ] Not yet started

## Setup

Suggested to clone this as submodule into your packages folder


```
git submodule add https://github.com/BennyKok/mocap4face-unity.git Packages/mocap4face-unity 
```

### Android Requirements
- This package uses [External Dependency Manager for Unity](https://github.com/googlesamples/unity-jar-resolver), please set it up first!
- The mocap4face aar library is not included, you will need to download in the Facemoji discord server #Dev channel ([message link](https://discord.com/channels/904757187522478131/905871468972343307/907051992915001345)), which require 0.2.0+, after downloading, put it into the Plugin/Android folder in your Assets dir.

![](.screenshots/2021-11-18-23-26-52.png)

- If you encoutered error like duplicated kotlin lib, try enabling these options in player settings -> publishing, and then force resolve once again.

![image](https://user-images.githubusercontent.com/18395202/149381299-dda66e37-8b7e-4dda-bf87-63a8e2f96752.png)



### iOS Requirements
- Please download the `mocap4face` iOS framework from the [Facemoji developer portal](https://studio.facemoji.co).

## Expected Result

### Android
Before running the demo scene on your android devices, put in the API key in the inspector.

![](.screenshots/Screenshot_2021-12-12_155632.png)

Then you will be seeing the blendshapes view as below.

<img src=".screenshots/Screenshot_20211212-155331.png" width="300"/>

### iOS
Not yet started



## Development

Please chime in the Facemoji discord for further discussion if you wanna help out the Unity integration!!

You can find the discord link in the page here -> [mocap4face GitHub Page](https://github.com/facemoji/mocap4face).
