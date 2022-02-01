// Facemoji Mocap4Face API for Unity
// Fork of @BennyKok's Android wrapper.
// iOS port by Thomas Suarez (@tomthecarrot) from Teleportal (@0xTELEPORTAL)

#if UNITY_IOS

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Facemoji
{
    public class FacemojiNativePluginIOS: IFacemojiNativePlugin
    {
        ///// FUNCTION INVOCATION | MANAGED -> NATIVE /////

        [DllImport("__Internal")]
        private static extern void facemoji_init(string apiKey);

        [DllImport("__Internal")]
        private static extern void facemoji_pause();

        [DllImport("__Internal")]
        private static extern void facemoji_resume();

        [DllImport("__Internal")]
        private static extern void facemoji_destroy();

        [DllImport("__Internal")]
        private static extern void facemoji_create_camera_tracker();

        [DllImport("__Internal")]
        private static extern unsafe float* facemoji_get_blendshape_value_array_ptr();

        ///// FUNCTION INVOCATION | NATIVE -> MANAGED /////

        delegate void OnActivateFuncType(bool isActive);

        delegate void AddBlendShapeNameFuncType(string newBlendShapeName);

        delegate void CommitBlendShapeNamesFuncType();

        [DllImport("__Internal")]
        private static extern void RegisterCallback_OnActivate(OnActivateFuncType func);

        [DllImport("__Internal")]
        private static extern void RegisterCallback_AddBlendShapeName(AddBlendShapeNameFuncType func);

        [DllImport("__Internal")]
        private static extern void RegisterCallback_CommitBlendShapeNames(CommitBlendShapeNamesFuncType func);

        ///// RUNTIME OBJECTS AND REFERENCES /////

        private static FacemojiNativePluginIOS instance;

        private Action<bool> onActivateAction;
        private Action<string[]> onBlendShapeNamesAction;
        private Action<float[]> onBlendShapeValuesAction;

        private List<string> blendShapeNames;
        private float[] blendShapeValues;
        private unsafe float* blendShapeValuesPtr;

        public void Initialize(string apiKey,
            Action<bool> onActivateAction,
            Action<string[]> onBlendShapeNamesAction,
            Action<float[]> onBlendShapeValuesAction,
            Action<Quaternion> onHeadRotationAction,
            Action<bool> onDisconnectedAction
        )
        {
            FacemojiNativePluginIOS.instance = this;

            this.onActivateAction = onActivateAction;
            this.onBlendShapeNamesAction = onBlendShapeNamesAction;
            this.onBlendShapeValuesAction = onBlendShapeValuesAction;

            this.blendShapeNames = new List<string>();
            
            // Register function pointers on native side
            RegisterCallback_OnActivate(OnActivate);
            RegisterCallback_AddBlendShapeName(AddBlendShapeName);
            RegisterCallback_CommitBlendShapeNames(CommitBlendShapeNames);

            // Call to native Objective C++
            facemoji_init(apiKey);

            // Pointer access is wrapped in a separate `unsafe` method
            InitBlendShapeValuesPointer();

            Debug.Log("[Facemoji] Initialized.");
        }

        private unsafe void InitBlendShapeValuesPointer()
        {
            // Call to native Objective C++
            this.blendShapeValuesPtr = facemoji_get_blendshape_value_array_ptr();
        }

        ///// C# PUBLIC METHODS /////

        public void Pause()
        {
            if (Application.isEditor)
            {
                return;
            }

            // Call to native Objective C++
            facemoji_pause();

            Debug.Log("[Facemoji] Paused.");
        }
        
        public void Resume()
        {
            if (Application.isEditor)
            {
                return;
            }
            
            // Call to native Objective C++
            facemoji_resume();

            Debug.Log("[Facemoji] Resumed.");
        }

        public void Destroy()
        {
            if (Application.isEditor)
            {
                return;
            }
            
            // Call to native Objective C++
            facemoji_destroy();

            Debug.Log("[Facemoji] Destroyed.");
        }

        public void CreateCameraTracker()
        {
            if (Application.isEditor)
            {
                return;
            }
            
            // Call to native Objective C++
            facemoji_create_camera_tracker();

            Debug.Log("[Facemoji] Created camera tracker.");
        }

        public unsafe float GetBlendShapeValue(int idx)
        {
            if (null == this.blendShapeValues)
            {
                Debug.LogError("[Facemoji] Cannot get blendshape value: managed values array is not initialized.");
                return -1;
            }

            if (idx >= this.blendShapeValues.Length)
            {
                Debug.LogError("[Facemoji] Cannot get blendshape value: managed values array is not long enough.");
                return -1;
            }

            // Retrieve value at index in native array.
            float val = *(this.blendShapeValuesPtr + idx);

            // Store that value in the managed array.
            // -----
            // TODO: this array currently serves no purpose on the iOS side, so we choose
            // to not write to it at the moment. Ideally we would use a Unity `NativeArray`
            // instance to point directly at the contiguous block of memory, which is allocated
            // in the native Objective C++ code. For now, the method invocation solution works,
            // and we'll save that optimization for the future if needed. Open-source contributions
            // are welcome here especially!
            // -----
            // this.blendShapeValues[idx] = val;            

            return val;
        }

        ///// Objective C++ INTEROP /////

        [AOT.MonoPInvokeCallback(typeof(bool))]
        static void OnActivate(bool isActive)
        {
            FacemojiNativePluginIOS I = FacemojiNativePluginIOS.instance;

            if (null == I)
            {
                Debug.LogError("[Facemoji] Cannot call OnActivate(): native plugin is not initialized.");
                return;
            }

            if (null == I.onActivateAction)
            {
                Debug.LogError("[Facemoji] Cannot call OnActivate(): there is no Action set.");
                return;
            }

            I.onActivateAction(isActive);
        }

        [AOT.MonoPInvokeCallback(typeof(string))]
        static void AddBlendShapeName(string newBlendShapeName)
        {
            FacemojiNativePluginIOS I = FacemojiNativePluginIOS.instance;

            if (null == I)
            {
                Debug.LogError("[Facemoji] Cannot call AddBlendShapeName(): native plugin is not initialized.");
                return;
            }

            if (null == newBlendShapeName)
            {
                Debug.LogError("[Facemoji] Blendshape name is null. Skipping add.");
                return;
            }

            // Append new blend shape
            I.blendShapeNames.Add(newBlendShapeName);
        }

        [AOT.MonoPInvokeCallback(typeof(void))]
        static void CommitBlendShapeNames()
        {
            FacemojiNativePluginIOS I = FacemojiNativePluginIOS.instance;

            if (null == I)
            {
                Debug.LogError("[Facemoji] Cannot call CommitBlendShapeNames(): native plugin is not initialized.");
                return;
            }

            // Apply blend shape names
            if (null == I.onBlendShapeNamesAction)
            {
                Debug.LogError("[Facemoji] There is no Action set for `onBlendShapeNames`.");
            }
            else
            {
                string[] blendShapeNames = I.blendShapeNames.ToArray();
                I.onBlendShapeNamesAction(blendShapeNames);
            }

            // Apply blend shape values
            I.blendShapeValues = new float[I.blendShapeNames.Count];

            if (null == I.onBlendShapeValuesAction)
            {
                Debug.LogError("[Facemoji] There is no Action set for `onBlendShapeValues`.");
            }
            else
            {
                I.onBlendShapeValuesAction(I.blendShapeValues);
            }
        }
    }
}

#endif