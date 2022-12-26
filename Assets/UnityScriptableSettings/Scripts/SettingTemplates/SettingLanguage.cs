using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {
    
[CreateAssetMenu(fileName = "New Language Setting", menuName = "Unity Scriptable Setting/Language", order = 54)]
public class SettingLanguage : SettingDropdown {
    public override void SetValue(int value) {
        SettingsManager.StaticStartCoroutine(ChangeLanguage(value));
    }
    private IEnumerator ChangeLanguage(int value) {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
        base.SetValue(value);
    }
    public override void Load() {
        SettingsManager.StaticStartCoroutine(OverrideDropdownWithLanguages());
    }
    private IEnumerator OverrideDropdownWithLanguages() {
        yield return LocalizationSettings.InitializationOperation;
        dropdownOptions = new string[LocalizationSettings.AvailableLocales.Locales.Count];
        for(int i=0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++) {
            dropdownOptions[i] = LocalizationSettings.AvailableLocales.Locales[i].name;
        }
        int detectedLocale = PlayerPrefs.GetInt(name, -1);
        if (detectedLocale != -1) {
            SetValue(detectedLocale);
        }
    }
}

}