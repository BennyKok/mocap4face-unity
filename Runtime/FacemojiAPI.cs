using System;
using UnityEngine;
using UnityEngine.UI;

public class FacemojiAPI
{
#if UNITY_ANDROID
    private static readonly string facemojiClassName = "co.facemoji.api.FacemojiAPI";
    private static readonly string facemojiBridgeClassName = "co.facemoji.mocap4face.FacemojiAPIUnityAndroid";
    private AndroidJavaObject facemojiAPIClass;
    private AndroidJavaClass unityAndroidPlayer;
    private AndroidJavaObject cameraTracker;
    private AndroidJavaObject activity;


    class OnActivateListener : AndroidJavaProxy
    {
        public Action<bool> onActivateCallback;
        public OnActivateListener(Action<bool> onActivate) : base("co.facemoji.mocap4face.FacemojiAPIUnityAndroid$OnActivateListener")
        {
            onActivateCallback = onActivate;
        }
        void onActivate(bool activated)
        {
            onActivateCallback?.Invoke(activated);
        }
    }
#endif

    private static FacemojiAPI instance;

    public static FacemojiAPI Instance
    {
        get
        {
            if (instance == null)
                instance = new FacemojiAPI();

            return instance;
        }
    }

    private FacemojiAPI() { }

    public void Initialize(string apiKey)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("Facemoji: Initialize from Unity");
        facemojiAPIClass = new AndroidJavaObject(facemojiBridgeClassName);
        unityAndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = unityAndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        facemojiAPIClass.Call("initialize",
            apiKey,
            activity,
            new OnActivateListener((activated) =>
            {
                if (activated)
                {
                    Debug.Log("Facemoji: API activation was successful");
                }
                else
                {
                    Debug.Log("Facemoji: API activation failed");
                }
            })
        );
#endif
    }

    public void CreateCameraTracker()
    {
#if UNITY_ANDROID
        cameraTracker = new AndroidJavaObject("co.facemoji.io.ApplicationContext", activity);
        // cameraTracker.Call();
#endif
    }
}