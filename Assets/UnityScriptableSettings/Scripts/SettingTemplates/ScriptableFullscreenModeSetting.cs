using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "Fullscreen Mode Setting", menuName = "Unity Scriptable Setting/Specific/Fullscreen Mode Setting", order = 1)]
public class ScriptableFullscreenModeSetting : ScriptableSettingLocalizedDropdown {
    public override void SetValue(float value) {
        switch(Mathf.FloorToInt(value)) {
            case 0: Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.MaximizedWindow; break;
            case 3: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
        base.SetValue(value);
    }
    public override void Save() {
        PlayerPrefs.SetInt("Screenmanager Fullscreen mode", (int)Screen.fullScreenMode);
    }
    public override void Load() {
        minValue = 0;
        maxValue = 3;
        SetValue(PlayerPrefs.GetInt("Screenmanager Fullscreen mode", (int)Screen.fullScreenMode));
    }
}

}