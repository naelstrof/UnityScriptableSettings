using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityScriptableSettings {
public class CameraUserOptionLoader : MonoBehaviour {
    public ScriptableSetting antiAliasing;
    private UniversalAdditionalCameraData camData;
    private Camera cam;
    // Start is called before the first frame update
    void Start() {
        antiAliasing.onValueChange -= OnValueChanged;
        antiAliasing.onValueChange += OnValueChanged;
        OnValueChanged(antiAliasing);
        cam = GetComponent<Camera>();
        camData = GetComponent<UniversalAdditionalCameraData>();
    }
    void OnEnable() {
        Start();
    }
    void OnDisable() {
        antiAliasing.onValueChange -= OnValueChanged;
    }
    void OnValueChanged(ScriptableSetting setting) {
        if (cam == null) {
            return;
        }
        cam.allowMSAA = (setting.value != 0f);
        camData.antialiasing = setting.value == 0 ? AntialiasingMode.None : AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        switch(Mathf.FloorToInt(setting.value)-1) {
            case 0: camData.antialiasingQuality = AntialiasingQuality.Low; break;
            case 1: camData.antialiasingQuality = AntialiasingQuality.Medium; break;
            case 2: camData.antialiasingQuality = AntialiasingQuality.High; break;
        }
    }
}

}
