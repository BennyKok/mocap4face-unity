// Facemoji Mocap4Face API for Unity
// Fork of @BennyKok's Android wrapper.
// iOS port by Thomas Suarez (@tomthecarrot) from Teleportal (@0xTELEPORTAL)

#include <Foundation/Foundation.h>
#include "UnityFramework/UnityFramework-Swift.h"

extern "C" {

    #pragma mark Configuration

    #define MAX_BLEND_SHAPE_COUNT 64

    #pragma mark Typedefs and statics

    typedef void (*OnActivateFuncType)(bool isActive);
    typedef void (*AddBlendShapeNameFuncType)(const char* newBlendShapeName);
    typedef void (*CommitBlendShapeNamesFuncType)();

    static OnActivateFuncType onActivateFunc;
    static AddBlendShapeNameFuncType addBlendShapeNameFunc;
    static CommitBlendShapeNamesFuncType commitBlendShapeNamesFunc;

    static FacemojiAPIUnityIOS *facemoji;

    static uint16_t blendShapeCount = 0;
    static float blendShapeValues[MAX_BLEND_SHAPE_COUNT];

    #pragma mark Exposed for Swift

    void onActivate(bool isActive)
    {
        if (nil == onActivateFunc)
        {
            NSLog(@"[Facemoji] OnActivate() is null in ObjC++");
            return;
        }
        
        onActivateFunc(isActive);
    }

    void addBlendShapeName(const char* newBlendShapeName)
    {
        if (nil == addBlendShapeNameFunc)
        {
            NSLog(@"[Facemoji] AddBlendShapeName() is null in ObjC++");
            return;
        }

        addBlendShapeNameFunc(newBlendShapeName);
    }

    void commitBlendShapeNames()
    {
        if (nil == commitBlendShapeNamesFunc)
        {
            NSLog(@"[Facemoji] CommitBlendShapeNames() is null in ObjC++");
            return;
        }

        commitBlendShapeNamesFunc();
    }

    void setBlendShapeCount(uint16_t count)
    {
        blendShapeCount = count;
    }

    void setBlendShapeValue(uint16_t idx, float newValue)
    {
        if (idx >= blendShapeCount)
        {
            NSLog(@"[Facemoji] Cannot set blendshape value: native values array is not initialized or long enough.");
            return;
        }

        blendShapeValues[idx] = newValue;
    }

    void resetBlendShapeValues()
    {
        for (uint16_t idx = 0; idx < blendShapeCount; idx++)
        {
            blendShapeValues[idx] = 0;
        }
    }

    #pragma mark Exposed for C#

    void RegisterCallback_OnActivate(OnActivateFuncType func)
    {
        onActivateFunc = func;
    }

    void RegisterCallback_AddBlendShapeName(AddBlendShapeNameFuncType func)
    {
        addBlendShapeNameFunc = func;
    }

    void RegisterCallback_CommitBlendShapeNames(CommitBlendShapeNamesFuncType func)
    {
        commitBlendShapeNamesFunc = func;
    }

    void facemoji_init(const char* apiKey)
    {
        NSString *apiKeyStr = [NSString stringWithCString: apiKey encoding: NSASCIIStringEncoding];
        
        facemoji = [[FacemojiAPIUnityIOS alloc] init];
        [facemoji initializeWithApiKey: apiKeyStr];
    }
    
    void facemoji_pause()
    {
        if (nil == facemoji)
        {
            return;
        }
        
        [facemoji pause];
    }

    void facemoji_resume()
    {
        if (nil == facemoji)
        {
            return;
        }
        
        [facemoji resume];
    }

    void facemoji_destroy()
    {
        if (nil == facemoji)
        {
            return;
        }
        
        [facemoji destroy];
    }

    void facemoji_create_camera_tracker()
    {
        if (nil == facemoji)
        {
            return;
        }
        
        [facemoji create_camera_tracker];
    }

    float* facemoji_get_blendshape_value_array_ptr()
    {
        if (nil == facemoji)
        {
            return 0;
        }
        
        return (float*) &blendShapeValues;
    }

}
