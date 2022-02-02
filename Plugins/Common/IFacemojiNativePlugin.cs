using System;
using UnityEngine;

namespace Facemoji
{
    public interface IFacemojiNativePlugin
    {
        // This signature was kept for consistency with the Android side as-is.
        // It is unused in the iOS implementation.
        void Initialize(string apiKey,
            Action<bool> onActivateAction,
            Action<string[]> onBlendShapeNamesAction,
            Action<float[]> onBlendShapeValuesAction,
            Action<Quaternion> onHeadRotationAction,
            Action<bool> onDisconnectedAction
        );
        void Pause();
        void Resume();
        void Destroy();
        void CreateCameraTracker();
        float GetBlendShapeValue(int idx);
    }
}