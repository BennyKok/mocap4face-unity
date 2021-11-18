package co.facemoji.mocap4face

import android.content.Context
import android.util.Log
import co.facemoji.api.FacemojiAPI
import co.facemoji.io.ApplicationContext

class FacemojiAPIUnityAndroid {
    fun initialize(apiKey: String, context: Context, onActivateListener: OnActivateListener) {
        Log.v("Facemoji: ", "Facemoji: Initialize in Native")
        FacemojiAPI.initialize(apiKey, ApplicationContext(context.applicationContext))
            .whenDone { activated ->
                Log.v("Facemoji: ", "Facemoji: onActivateListener.onActivate $activated")
                onActivateListener.onActivate(activated);
            }
    }

    fun createCameraTracker() {
        
    }

    interface OnActivateListener {
        fun onActivate(activated: Boolean)
    }
}