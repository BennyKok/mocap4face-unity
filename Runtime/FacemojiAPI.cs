using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Facemoji
{
    public class FacemojiAPI
    {
#if UNITY_ANDROID
        private static readonly string facemojiClassName = "co.facemoji.api.FacemojiAPI";
        private static readonly string facemojiBridgeClassName = "co.facemoji.mocap4face.FacemojiAPIUnityAndroid";
        private AndroidJavaObject facemojiAPIClass;
        private AndroidJavaClass unityAndroidPlayer;
        private AndroidJavaObject activity;
        private bool isCameraCreated = false;

        class OnActivateListener : AndroidJavaProxy
        {
            // public Dictionary<string, float> blendShapesInput = new Dictionary<string, float>();
            public string[] namesList;
            public float[] valueList;
            public float[] emptyValueList;

            public Action<bool> onActivateAction;
            public Action<string[]> onBlendShapeNamesAction;
            public Action<float[]> onBlendShapeValuesAction;
            public Action<Quaternion> onHeadRotationAction;

            public OnActivateListener(
                Action<bool> onActivateAction,
                Action<string[]> onBlendShapeNamesAction,
                Action<float[]> onBlendShapeValuesAction,
                Action<Quaternion> onHeadRotationAction
            ) : base(
                "co.facemoji.mocap4face.FacemojiAPIUnityAndroid$OnActivateListener")
            {
                this.onActivateAction = onActivateAction;
                this.onBlendShapeNamesAction = onBlendShapeNamesAction;
                this.onBlendShapeValuesAction = onBlendShapeValuesAction;
                this.onHeadRotationAction = onHeadRotationAction;
            }

            void onActivate(bool activated) => onActivateAction?.Invoke(activated);

            void onBlendShapeNames(AndroidJavaObject names)
            {
                var size = names.Call<int>("size");

                namesList = new string[size];
                emptyValueList = new float[size];

                for (int i = 0; i < size; i++)
                {
                    var name = names.Call<string>("get", i);
                    namesList[i] = name;
                    emptyValueList[i] = 0;
                }

                onBlendShapeNamesAction?.Invoke(namesList);
            }

            void onBlendShapeValues(float[] input)
            {
                if (input.Length == 0)
                    valueList = emptyValueList;
                else
                    valueList = input;
                onBlendShapeValuesAction?.Invoke(valueList);
            }

            void onHeadRotation(float x, float y, float z, float w)
            {
                onHeadRotationAction?.Invoke(new Quaternion(x, y, z, w));
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

        private FacemojiAPI()
        {
        }

        public void Initialize(string apiKey,
            Action<bool> onActivateAction,
            Action<string[]> onBlendShapeNamesAction,
            Action<float[]> onBlendShapeValuesAction,
            Action<Quaternion> onHeadRotationAction
        )
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
                new OnActivateListener(onActivateAction, onBlendShapeNamesAction, onBlendShapeValuesAction,
                    onHeadRotationAction)
            );
#endif
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
}