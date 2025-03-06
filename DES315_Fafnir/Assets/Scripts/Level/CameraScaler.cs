using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Code stolen from the following forum answer on 03/03/2025
// https://gamedev.stackexchange.com/questions/144575/how-to-force-keep-the-aspect-ratio-and-specific-resolution-without-stretching-th

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour {

    // Set this to your target aspect ratio, eg. (16, 9) or (4, 3).
    public static Vector2 targetAspect = new(16, 9);
    Camera _camera;
    float checkAspect;

    void Start () {
        _camera = GetComponent<Camera>();
        checkAspect = _camera.aspect;
        UpdateCrop();
    }

    // Update the Crop when the window size changes
    public void Update() {

        if (checkAspect != _camera.aspect) {

            checkAspect = _camera.aspect;
            UpdateCrop();

        }

    }

    // Call this method if your window size or target aspect change.
    public void UpdateCrop() {
        // Determine ratios of screen/window & target, respectively.
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = targetAspect.x / targetAspect.y;

        if(Mathf.Approximately(screenRatio, targetRatio)) {
            // Screen or window is the target aspect ratio: use the whole area.
            _camera.rect = new Rect(0, 0, 1, 1);
        }
        else if(screenRatio > targetRatio) {
            // Screen or window is wider than the target: pillarbox.
            float normalizedWidth = targetRatio / screenRatio;
            float barThickness = (1f - normalizedWidth)/2f;
            _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
        }
        else {
            // Screen or window is narrower than the target: letterbox.
            float normalizedHeight = screenRatio / targetRatio;
            float barThickness = (1f - normalizedHeight) / 2f;
            _camera.rect = new Rect(0, barThickness, 1, normalizedHeight);
        }
    }
}
