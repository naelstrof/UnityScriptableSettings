using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "VSync Setting", menuName = "Unity Scriptable Setting/Specific/VSync Setting", order = 1)]
public class ScriptableVSyncSetting : ScriptableSettingLocalizedDropdown {
    [Header("VSync can be from 0 to 4, being Off, Single Buffered, Double Buffered, Triple Buffered, and Quad buffered. I usually just put two localized strings Off, and On.")]
    public float ignorethis;
    public override void SetValue(float value) {
        QualitySettings.vSyncCount = Mathf.RoundToInt(value);
        base.SetValue(value);
    }
    public override void Load() {
        minValue = 0;
        maxValue = 4;
        base.Load();
    }
}

}