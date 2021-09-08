using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "Target Framerate Setting", menuName = "Unity Scriptable Setting/Specific/Target Framerate Setting", order = 1)]
public class ScriptableFramerateTargetSetting : ScriptableSettingSlider {
    public override void SetValue(float value) {
        int fps = Mathf.RoundToInt(value);
        if (fps == Mathf.RoundToInt(maxValue)) {
            Application.targetFrameRate = -1;
        } else {
            Application.targetFrameRate = Mathf.RoundToInt(value);
        }
        base.SetValue(value);
    }
}

}