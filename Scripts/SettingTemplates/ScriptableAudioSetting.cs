using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "New Audio Setting", menuName = "Unity Scriptable Setting/Specific/Audio Exposed Parameter Setting", order = 1)]
public class ScriptableAudioSetting : ScriptableSettingSlider {
    public AudioMixer audioMixer;
    public string audioExposedParamName;
    public bool isVolumeParameter = true;
    public override void SetValue(float value) {
        if (isVolumeParameter) {
            audioMixer.SetFloat(audioExposedParamName, Mathf.Log(Mathf.Max(value,0.01f))*20f);
        } else {
            audioMixer.SetFloat(audioExposedParamName, value);
        }
        base.SetValue(value);
    }
}

}