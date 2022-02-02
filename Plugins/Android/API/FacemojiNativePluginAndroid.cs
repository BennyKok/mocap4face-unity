#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Facemoji
{
    public class FacemojiNativePluginAndroid: IFacemojiNativePlugin
    {
        private static FacemojiNativePluginAndroid I;
        private static readonly string facemojiClassName = "co.facemoji.api.FacemojiAPI";
        private static readonly string facemojiBridgeClassName = "co.facemoji.mocap4face.FacemojiAPIUnity";
        private AndroidJavaObject facemojiAPIClass;
        private AndroidJavaClass unityAndroidPlayer;
        private AndroidJavaObject activity;
        private bool isCameraCreated = false;
        private float[] valueList;

        class OnActivateListener : AndroidJavaProxy
        {
            // public Dictionary<string, float> blendShapesInput = new Dictionary<string, float>();
            public string[] namesList;
            public float[] emptyValueList;

            public Action<bool> onActivateAction;
            public Action<string[]> onBlendShapeNamesAction;
            public Action<float[]> onBlendShapeValuesAction;
            public Action<Quaternion> onHeadRotationAction;
            public Action<bool> onDisconnectedAction;

            private bool connected;

            public OnActivateListener(
                Action<bool> onActivateAction,
                Action<string[]> onBlendShapeNamesAction,
                Action<float[]> onBlendShapeValuesAction,
                Action<Quaternion> onHeadRotationAction,
                Action<bool> onDisconnectedAction
            ) : base(
                "co.facemoji.mocap4face.FacemojiAPIUnity$OnActivateListener")
            {
                this.onActivateAction = onActivateAction;
                this.onBlendShapeNamesAction = onBlendShapeNamesAction;
                this.onBlendShapeValuesAction = onBlendShapeValuesAction;
                this.onHeadRotationAction = onHeadRotationAction;
                this.onDisconnectedAction = onDisconnectedAction;
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
                if (input.Length == 0 && connected)
                {
                    connected = false;
                    onDisconnectedAction?.Invoke(connected);
                }
                if (input.Length > 0 && !connected)
                {
                    // on connected
                    connected = true;
                    onDisconnectedAction?.Invoke(connected);
                }
                
                if (input.Length == 0)
                    FacemojiNativePluginAndroid.I.valueList = emptyValueList;
                else
                    FacemojiNativePluginAndroid.I.valueList = input;
                onBlendShapeValuesAction?.Invoke(FacemojiNativePluginAndroid.I.valueList);
            }

            void onHeadRotation(float x, float y, float z, float w)
            {
                onHeadRotationAction?.Invoke(new Quaternion(x, y, z, w));
            }
        }

        public FacemojiNativePluginAndroid()
        {
            FacemojiNativePluginAndroid.I = this;
        }

        public void Initialize(string apiKey,
            Action<bool> onActivateAction,
            Action<string[]> onBlendShapeNamesAction,
            Action<float[]> onBlendShapeValuesAction,
            Action<Quaternion> onHeadRotationAction,
            Action<bool> onDisconnectedAction
        )
        {
            if (Application.isEditor)
            {
                return;
            }

            facemojiAPIClass = new AndroidJavaObject(facemojiBridgeClassName);
            unityAndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityAndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            facemojiAPIClass.Call("initialize",
                apiKey,
                activity,
                new OnActivateListener(onActivateAction, onBlendShapeNamesAction, onBlendShapeValuesAction,
                    onHeadRotationAction,onDisconnectedAction)
            );

            Debug.Log("[Facemoji] Initialized.");
        }

        public void Pause()
        {
            if (Application.isEditor)
            {
                return;
            }

            if (null == facemojiAPIClass) return;
            facemojiAPIClass.Call("pause");

            Debug.Log("[Facemoji] Paused.");
        }

        public void Resume()
        {
            if (Application.isEditor)
            {
                return;
            }

            if (null == facemojiAPIClass) return;
            if (!isCameraCreated)
            {
                CreateCameraTracker();
            }
            facemojiAPIClass.Call("resume");
            
            Debug.Log("[Facemoji] Resumed.");
        }

        public void Destroy()
        {
            if (Application.isEditor)
            {
                return;
            }

            facemojiAPIClass.Call("destroy");
            
            Debug.Log("[Facemoji] Destroyed.");
        }

        public void CreateCameraTracker()
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log("[Facemoji] CreateCameraTracker");
            facemojiAPIClass.Call("createCameraTracker", activity);
            isCameraCreated = true;

            Debug.Log("[Facemoji] Created camera tracker.");
        }

        public float GetBlendShapeValue(int idx)
        {
            // Included in the interface for parity with the iOS native plugin.
            return valueList[idx];
        }
    }
}

#endif