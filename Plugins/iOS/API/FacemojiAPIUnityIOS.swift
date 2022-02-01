// Facemoji Mocap4Face API for Unity
// Fork of @BennyKok's Android wrapper.
// iOS port by Thomas Suarez (@tomthecarrot) from Teleportal (@0xTELEPORTAL)

import Foundation
import CoreMedia
import Mocap4Face

@objc public class FacemojiAPIUnityIOS: NSObject, VideoCaptureDelegate
{
    var blendshapeNames: [String] = []
    var blendshapeNamesCommitted: Bool = false
    var tracker: Future<FaceTracker?>!
    var capture: VideoCapture!
    
    @objc public func initialize(apiKey: String)
    {
        FacemojiAPI.initialize(apiKey).whenDone { activated in
            if activated
            {
                print("[Facemoji] API key validation Successful.")
            } else
            {
                print("[Facemoji] API key validation Failed.")
            }
            
            onActivate(activated)
        };
    }
    
    @objc public func pause()
    {
        if nil == self.capture
        {
            return
        }
        
        self.capture.stopCamera()
    }
    
    @objc public func resume()
    {
        create_camera_tracker()
        
        if nil == self.capture
        {
            return
        }
        
        self.capture.startCamera(position: .front)
    }
    
    @objc public func destroy()
    {
        self.pause()
    }
    
    @objc public func create_camera_tracker()
    {
        self.tracker = FaceTracker.createVideoTracker()
            .logError("[Facemoji] Error creating video tracker.")
        
        self.tracker.whenDone { t in
            if let tracker = t
            {
                DispatchQueue.main.async
                {
                    let names = tracker.blendshapeNames
                    for i in 0..<names.count
                    {
                        self.blendshapeNames.append(names[i])
                        addBlendShapeName(names[i])
                    }

                    if !self.blendshapeNamesCommitted
                    {
                        self.blendshapeNamesCommitted = true
                        commitBlendShapeNames()
                    }

                    setBlendShapeCount(UInt16(names.count))
                }
            }
        }
        
        self.capture = VideoCapture()
        self.capture.delegate = self
    }
    
    /// Called when a new camera frame arrives
    func sampleBufferAvailable(_ buffer: CMSampleBuffer, brightness: CGFloat) {
        guard case let tracker?? = self.tracker.currentValue else {
            // Face tracker not initialized yet
            return
        }

        // Run the face tracker and get the facial coefficients
        if let result = tracker.track(buffer)
        {
            DispatchQueue.main.async { [self] in
                for i in 0..<result.blendshapes.count
                {
                    let key = self.blendshapeNames[i]
                    if let val = result.blendshapes[key]
                    {
                        setBlendShapeValue(UInt16(i), val)
                    }
                }
            }
        } else
        {
            // Clear array
            resetBlendShapeValues()
        }
    }
}
