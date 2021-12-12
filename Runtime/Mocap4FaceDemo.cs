using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

namespace Facemoji
{
    public class Mocap4FaceDemo : MonoBehaviour
    {
        public bool debugView = true;
        public string apiKey;

        private GUIStyle _style;
        private GUIStyle _BarStyle;
        private GUIStyle _BgBarStyle;

        private float[] blendShapesInput;
        private string[] blendShapesNames;

        private void Start()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }

            FacemojiAPI.Instance.Initialize(apiKey,
                OnActivate, OnBlendShapeNames, OnBlendShapeValues, OnHeadRotation);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                FacemojiAPI.Instance.Resume();
            else
                FacemojiAPI.Instance.Pause();
        }

        private void OnApplicationQuit()
        {
            FacemojiAPI.Instance.Destroy();
        }

        private void OnGUI()
        {
            if (!debugView) return;

            var rect = new Rect(20, 100, 100, 40);
            if (_style == null)
            {
                _style = new GUIStyle();
                _style.fontSize = 30;
                _style.normal.textColor = Color.white;

                _BarStyle = new GUIStyle();
                _BarStyle.normal.background = Texture2D.whiteTexture;
                _BgBarStyle = new GUIStyle();
                _BgBarStyle.normal.background = Texture2D.grayTexture;
            }

            GUI.Label(rect, "blendShapesInput: " + blendShapesInput, _style);
            rect.y += 40;

            if (blendShapesInput != null && blendShapesNames != null)
            {
                for (int i = 0; i < blendShapesNames.Length; i++)
                {
                    rect.x = 20;
                    rect.width = 200f;
                    GUI.Label(rect, blendShapesNames[i], _style);
                    rect.x = 300;
                    GUI.Label(rect, blendShapesInput[i].ToString(), _style);
                    rect.x = 600;
                    GUI.Box(rect, "", _BgBarStyle);
                    rect.width = 200f * blendShapesInput[i];
                    GUI.Box(rect, "", _BarStyle);

                    rect.y += 40;
                }
            }
        }

        protected virtual void OnActivate(bool activated)
        {
            if (activated)
            {
                Debug.Log("Facemoji: API activation was successful");
                FacemojiAPI.Instance.Resume();
            }
            else
            {
                Debug.Log("Facemoji: API activation failed");
            }
        }

        protected virtual void OnBlendShapeNames(string[] names)
        {
            var result = names.Aggregate("Facemoji: OnBlendShapeNames: ", (current, item) => current + (item + ", "));
            Debug.Log(result);
            blendShapesNames = names;
        }

        protected virtual void OnBlendShapeValues(float[] input)
        {
            blendShapesInput = input;
        }

        protected virtual void OnHeadRotation(Quaternion rot)
        {
        }
    }
}