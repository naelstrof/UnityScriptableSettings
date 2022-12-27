using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {
    
[CreateAssetMenu(fileName = "New Language Setting", menuName = "Unity Scriptable Setting/Language", order = 54)]
public class SettingLanguage : SettingDropdown {
    public override void SetValue(int value) {
        base.SetValue(value);
        // -1 means the user hasn't specifically selected a locale.
        if (value == -1) {
            return;
        }
        SettingsManager.StaticStartCoroutine(ChangeLanguage(value));
    }
    private IEnumerator ChangeLanguage(int value) {
        if (value == -1) {
            yield break;
        }
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
        SetValue(detectedLocale);
    }
}

}