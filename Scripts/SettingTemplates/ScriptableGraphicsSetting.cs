using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "Quality Setting", menuName = "Unity Scriptable Setting/Specific/Quality Setting", order = 1)]
public class ScriptableGraphicsSetting : ScriptableSettingLocalizedDropdown {
    public override void SetValue(float value) {
        QualitySettings.SetQualityLevel(Mathf.RoundToInt(value));
        if (value == 0) {
            Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1;
        } else {
            Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3;
        }
        base.SetValue(value);
    }
    public override void Save() {
        PlayerPrefs.SetInt("UnityGraphicsQuality", Mathf.RoundToInt(internalValue));
    }
    public override void Load() {
        SetValue(PlayerPrefs.GetInt("UnityGraphicsQuality", QualitySettings.GetQualityLevel()));
    }
}

}