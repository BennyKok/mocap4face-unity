using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facemoji
{
    public class FacemojiMocap
    {

        private static IFacemojiNativePlugin instance;

        public static IFacemojiNativePlugin Instance
        {
            get
            {
                if (null == instance)
                {
#if UNITY_ANDROID
                    instance = new FacemojiNativePluginAndroid();
#elif UNITY_IOS
                    instance = new FacemojiNativePluginIOS();
#endif
                }

                return instance;
            }
        }

        private FacemojiMocap()
        {
        }
    }
}