using System;
using UnityEngine;

namespace UnityScriptableSettings {
    
[CreateAssetMenu(fileName = "New Resolution", menuName = "Unity Scriptable Setting/Resolution", order = 55)]
public class SettingResolution : SettingDropdown {
    public override ScriptableSettingString[] GetLocalizedDropdowns() {
        int count = Screen.resolutions.Length;
        var dropdowns = new ScriptableSettingString[count];
        for(int i=0;i<count;i++) {
            dropdowns[i] = new ScriptableSettingString(Screen.resolutions[i].ToString());
        }
        return dropdowns;
    }

    public override void SetValue(int value) {
        Resolution r = Screen.resolutions[Mathf.RoundToInt(value)];
        if (Screen.currentResolution.width != r.width || Screen.currentResolution.height != r.height || Math.Abs(Screen.currentResolution.refreshRateRatio.value - r.refreshRateRatio.value) > 0.01f) {
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRateRatio);
        }
        base.SetValue(value);
    }
    
    public override void Save() {
        // Nothing to save, unity remembers the resolution automatically
    }
    
    public override void Load() {
        var currentResolution = Screen.currentResolution;
        int height = currentResolution.height;
        int width = currentResolution.width;
        double refreshRate = currentResolution.refreshRateRatio.value;
        
        for(int i=0;i<Screen.resolutions.Length;i++) {
            if (Screen.resolutions[i].width == width && Screen.resolutions[i].height == height && Math.Abs(Screen.resolutions[i].refreshRateRatio.value - refreshRate) < 0.01f) {
                selectedValue = i;
                break;
            }
        }
    }
}

}