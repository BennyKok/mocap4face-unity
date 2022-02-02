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

        private string[] blendShapesNames;

        private void Start()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }

            FacemojiMocap.Instance.Initialize(apiKey,
                OnActivate, OnBlendShapeNames, null, OnHeadRotation,OnDisconnected);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                FacemojiMocap.Instance.Resume();
            else
                FacemojiMocap.Instance.Pause();
        }

        private void OnApplicationQuit()
        {
            FacemojiMocap.Instance.Destroy();
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

            rect.y += 40;

            if (blendShapesNames != null)
            {
                for (int i = 0; i < blendShapesNames.Length; i++)
                {
                    float value = FacemojiMocap.Instance.GetBlendShapeValue(i);
                    rect.x = 20;
                    rect.width = 200f;
                    GUI.Label(rect, blendShapesNames[i], _style);
                    rect.x = 300;
                    GUI.Label(rect, value.ToString(), _style);
                    rect.x = 600;
                    GUI.Box(rect, "", _BgBarStyle);
                    rect.width = 200f * value;
                    GUI.Box(rect, "", _BarStyle);

                    rect.y += 40;
                }
            }
        }
        
        void OnDisconnected(bool connected)
        {
            if (!connected)
            {
                Debug.Log("Facemoji: Lost Face Focus");
                FacemojiMocap.Instance.Resume();
            }
            else
            {
                Debug.Log("Facemoji: Gain Face Focus");
            }
        }

        protected virtual void OnActivate(bool activated)
        {
            if (activated)
            {
                Debug.Log("Facemoji: API activation was successful");
                FacemojiMocap.Instance.Resume();
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

        protected virtual void OnHeadRotation(Quaternion rot)
        {
        }
    }
}