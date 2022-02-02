# mocap4face-unity 
This is an unofficial Unity integration for [mocap4face by Alter](https://github.com/facemoji/mocap4face) (Facemoji).

## Repository Setup

### Option 1: Package Manager
It is suggested to use the Unity Package Manager to add this repository to your project, with the following URL:  
`git+https://github.com/BennyKok/mocap4face-unity.git`

In order for this to work, first [install Git LFS](https://git-lfs.github.com/) and [enable it in the Unity Package Manager](https://docs.unity3d.com/Manual/upm-git.html#git-lfs).

![Screenshot of Unity -> Window menu -> Package Manager](.screenshots/pacman1.png)
![Screenshot of Plus (+) button -> Add package from Git URL](.screenshots/pacman2.png)
![Screenshot of pasting in URL and Add button](.screenshots/pacman3.png)

### Option 2: Submodule

Alternatively, clone this repository as a submodule in your project's `Packages` directory:  
`git submodule add https://github.com/BennyKok/mocap4face-unity.git Packages/mocap4face-unity`

This option is _not_ recommended if you are working in a team or intend to redistribute the code,
since most of the `Packages/` directory is meant to be excluded from version control.

## Per-Platform Setup

### Android
- This package uses [External Dependency Manager for Unity](https://github.com/googlesamples/unity-jar-resolver), please set it up first.
- Download the `mocap4face` Android AAR library from the [Facemoji developer portal](https://studio.facemoji.co).
  - Minimum version is 0.2.0.
  - See this [Discord message](https://discord.com/channels/904757187522478131/905871468972343307/907051992915001345) for more details.
- After downloading, copy `mocap4face-x.x.x-SNAPSHOT.aar` into the `Assets/Plugins/Android/Library` directory in your project.
  - If any directory does not exist, create it.
- If you encoutered an error about a duplicated Kotlin library, enable these options in the `Publishing` section of Player Settings. Then in the Unity menu bar, choose `Assets > External Dependency Manager > Android Resolver > Force Resolve`.

![Screenshot of Android Build Settings](./.screenshots/android_build_settings.png)

### iOS
- Download the `mocap4face` iOS framework from the [Facemoji developer portal](https://studio.facemoji.co).
- After downloading, copy the contents of `Mocap4Face.xcframework/ios-arm64` into the `Assets/Plugins/iOS/Library` directory in your project.
  - If any directory does not exist, create it.

## Expected Result

### Android and iOS
Before running the demo scene on your device, put in the API key in the inspector.

![Screenshot of API key location](./.screenshots/api_key.png)

Then you will see the blendshapes view as below.

<img src=".screenshots/values.png" width="400"/>

## Development
Please chime in the Alter Discord for further discussion if you would like to help out with this Unity integration!

You can find the Discord link in the page here -> [mocap4face GitHub Page](https://github.com/facemoji/mocap4face).

## Credit
- Original Unity wrapper with Android support by [Benny Kok (@BennyKok)](https://github.com/BennyKok/).
- iOS port (including unsafe memory access) by [Thomas Suarez (@tomthecarrot)](https://github.com/tomthecarrot/) from [Teleportal (0xTELEPORTAL)](https://github.com/0xTELEPORTAL/).
