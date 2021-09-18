using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "Resolution Setting", menuName = "Unity Scriptable Setting/Specific/Resolution Setting", order = 1)]
[OverrideDropdown]
[OverrideDefaultValue]
public class ScriptableResolutionSetting : ScriptableSettingDropdown {
    public override void SetValue(float value) {
        Resolution r = Screen.resolutions[Mathf.RoundToInt(value)];
        if (Screen.currentResolution.width != r.width || Screen.currentResolution.height != r.height || Screen.currentResolution.refreshRate != r.refreshRate) {
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRate);
        }
        base.SetValue(value);
    }
    public override void Save() {
        Resolution r = Screen.currentResolution;
        PlayerPrefs.SetInt ("Screenmanager Resolution Height", r.height);
        PlayerPrefs.SetInt ("Screenmanager Resolution Width", r.width);
        PlayerPrefs.SetInt ("Screenmanager Refresh Rate", r.refreshRate);
    }
    public override void Load() {
        int height = PlayerPrefs.GetInt ("Screenmanager Resolution Height", Screen.resolutions[0].height);
        int width = PlayerPrefs.GetInt ("Screenmanager Resolution Width", Screen.resolutions[0].width);
        int refreshRate = PlayerPrefs.GetInt ("Screenmanager Refresh Rate", Screen.resolutions[0].refreshRate);
        bool foundResolution = false;
        for(int i=0;i<Screen.resolutions.Length;i++) {
            if (Screen.resolutions[i].width == width && Screen.resolutions[i].height == height && Screen.resolutions[i].refreshRate == refreshRate) {
                SetValue(i);
                foundResolution = true;
                break;
            }
        }

        if (!foundResolution) {
            Screen.SetResolution(width, height, Screen.fullScreenMode, refreshRate);
        }

        int count = Screen.resolutions.Length;
        minValue = 0;
        maxValue = count-1;
        defaultValue = maxValue;
        dropdownOptions = new string[count];
        for(int i=0;i<Screen.resolutions.Length;i++) {
            dropdownOptions[i] = Screen.resolutions[i].width + "x" + Screen.resolutions[i].height + "_" + Screen.resolutions[i].refreshRate;
        }
        return;
    }
}

}