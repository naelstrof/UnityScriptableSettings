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

    public override int GetValue() {
        // This is awful, but if the user hasn't selected a locale, try to load the value that's just-- currently selected.
        if (selectedValue == -1) {
            for(int i=0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++) {
                if (LocalizationSettings.AvailableLocales.Locales[i].name == LocalizationSettings.SelectedLocale.name) {
                    return i;
                }
            }
        }

        if (selectedValue == -1) {
            return 0;
        }

        return base.GetValue();
    }

    public override void Save() {
        if (selectedValue == -1) {
            return;
        }
        base.Save();
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