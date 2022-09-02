using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityScriptableSettings {
public class CameraUserOptionLoader : MonoBehaviour {
    public SettingInt antiAliasing;
    private UniversalAdditionalCameraData camData;
    private Camera cam;
    // Start is called before the first frame update
    void Awake() {
        cam = GetComponent<Camera>();
        camData = GetComponent<UniversalAdditionalCameraData>();
    }
    void OnEnable() {
        antiAliasing.changed -= OnValueChanged;
        antiAliasing.changed += OnValueChanged;
        OnValueChanged(antiAliasing.GetValue());
    }
    void OnDisable() {
        antiAliasing.changed -= OnValueChanged;
    }
    void OnValueChanged(int value) {
        if (cam == null) {
            return;
        }
        cam.allowMSAA = (value != 0f);
        camData.antialiasing = value == 0 ? AntialiasingMode.None : AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        switch(value-1) {
            case 0: camData.antialiasingQuality = AntialiasingQuality.Low; break;
            case 1: camData.antialiasingQuality = AntialiasingQuality.Medium; break;
            case 2: camData.antialiasingQuality = AntialiasingQuality.High; break;
        }
    }
}

}
