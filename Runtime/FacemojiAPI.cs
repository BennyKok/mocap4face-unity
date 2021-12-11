using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FacemojiAPI
{
    public Dictionary<string, float> blendShapesInput = new Dictionary<string, float>();

#if UNITY_ANDROID
    private static readonly string facemojiClassName = "co.facemoji.api.FacemojiAPI";
    private static readonly string facemojiBridgeClassName = "co.facemoji.mocap4face.FacemojiAPIUnityAndroid";
    private AndroidJavaObject facemojiAPIClass;
    private AndroidJavaClass unityAndroidPlayer;
    private AndroidJavaObject activity;
    private bool isCameraCreated = false;


    class OnActivateListener : AndroidJavaProxy
    {
        public Action<bool> onActivateAction;
        public Action<List<string>> onBlendShapeNamesAction;
        public Action<string, float> onBlendShapeValuesAction;

        public OnActivateListener(
            Action<bool> onActivateAction,
            Action<List<string>> onBlendShapeNamesAction,
            Action<string, float> onBlendShapeValuesAction
        ) : base(
            "co.facemoji.mocap4face.FacemojiAPIUnityAndroid$OnActivateListener")
        {
            this.onActivateAction = onActivateAction;
            this.onBlendShapeNamesAction = onBlendShapeNamesAction;
            this.onBlendShapeValuesAction = onBlendShapeValuesAction;
        }

        void onActivate(bool activated) => onActivateAction?.Invoke(activated);
        void onBlendShapeNames(List<string> names) => onBlendShapeNamesAction?.Invoke(names);
        void onBlendShapeValues(string key, float value) => onBlendShapeValuesAction?.Invoke(key, value);
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

    private FacemojiAPI()
    {
    }

    public void Initialize(string apiKey)
    {
        if (Application.isEditor)
            return;

#if UNITY_ANDROID
        Debug.Log("Facemoji: Initialize from Unity");
        facemojiAPIClass = new AndroidJavaObject(facemojiBridgeClassName);
        unityAndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = unityAndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        facemojiAPIClass.Call("initialize",
            apiKey,
            activity,
            new OnActivateListener(OnActivate, OnBlendShapeNames, OnBlendShapeValues)
        );
#endif
    }

    void OnActivate(bool activated)
    {
        if (activated)
        {
            Debug.Log("Facemoji: API activation was successful");
            CreateCameraTracker();
        }
        else
        {
            Debug.Log("Facemoji: API activation failed");
        }
    }

    void OnBlendShapeNames(List<string> names)
    {
        var result = names.Aggregate("Facemoji: OnBlendShapeNames: ", (current, item) => current + (item + ", "));
        Debug.Log(result);
    }

    void OnBlendShapeValues(string key, float value)
    {
        // var result = input.Aggregate("Facemoji: OnBlendShapeValues: ",
        //     (current, item) => current + ($"{item.Key} : {item.Value}" + "\n"));
        Debug.Log($"{key} : {value}");
        blendShapesInput[key] = value;
    }

    public void Pause()
    {
        if (Application.isEditor)
            return;

#if UNITY_ANDROID
        if (facemojiAPIClass == null) return;
        facemojiAPIClass.Call("pause");
#endif
        Debug.Log("Facemoji: Pause");
    }

    public void Resume()
    {
        if (Application.isEditor)
            return;

        if (facemojiAPIClass == null) return;
        if (!isCameraCreated)
        {
            CreateCameraTracker();
        }
#if UNITY_ANDROID
        facemojiAPIClass.Call("resume");
#endif
        Debug.Log("Facemoji: Resume");
    }

    public void Destroy()
    {
        if (Application.isEditor)
            return;

#if UNITY_ANDROID
        facemojiAPIClass.Call("destroy");
#endif
        Debug.Log("Facemoji: Destroy");
    }

    public void CreateCameraTracker()
    {
        if (Application.isEditor)
            return;

#if UNITY_ANDROID
        Debug.Log("Facemoji: CreateCameraTracker");
        facemojiAPIClass.Call("createCameraTracker", activity);
        isCameraCreated = true;
#endif
    }
}