# mocap4face-unity 
This is an unofficial Unity integration for [mocap4face by Alter](https://github.com/facemoji/mocap4face) (Facemoji).



## Setup

Suggested to clone this as submodule into your packages folder


```
git submodule add https://github.com/BennyKok/mocap4face-unity.git Packages/mocap4face-unity 
```

### Android Requirements
- This package uses [External Dependency Manager for Unity](https://github.com/googlesamples/unity-jar-resolver), please set it up first.
- The mocap4face aar library is not included, you will need to download in the Facemoji discord server #Dev channel ([message link](https://discord.com/channels/904757187522478131/905871468972343307/907051992915001345)), which require 0.2.0+, after downloading, put it into the Plugin/Android folder in your Assets dir.

![](.screenshots/2021-11-18-23-26-52.png)

- If you encoutered error like duplicated kotlin lib, try enabling these options in player settings -> publishing, and then force resolve once again.

![image](https://user-images.githubusercontent.com/18395202/149381299-dda66e37-8b7e-4dda-bf87-63a8e2f96752.png)

### iOS Requirements
- Please download the `mocap4face` iOS framework from the [Facemoji developer portal](https://studio.facemoji.co) and place it in 

## Expected Result

### Android and iOS
Before running the demo scene on your device, put in the API key in the inspector.

![](.screenshots/Screenshot_2021-12-12_155632.png)

Then you will see the blendshapes view as below.

<img src=".screenshots/Screenshot_20211212-155331.png" width="300"/>

## Development
Please chime in the Alter Discord for further discussion if you would like to help out with this Unity integration!

You can find the discord link in the page here -> [mocap4face GitHub Page](https://github.com/facemoji/mocap4face).
