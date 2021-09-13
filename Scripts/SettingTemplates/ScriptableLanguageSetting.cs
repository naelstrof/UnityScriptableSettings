using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "Language Setting", menuName = "Unity Scriptable Setting/Specific/Language Setting", order = 1)]
[OverrideDropdown]
[OverrideDefaultValue]
public class ScriptableLanguageSetting : ScriptableSettingDropdown {
    public override void SetValue(float value) {
        if (ScriptableSettingsManager.instance == null) {
            throw new UnityException("Need ScriptableSettingsManager in the scene to set languages properly...");
        }
        ScriptableSettingsManager.instance.StartCoroutine(ChangeLanguage(Mathf.RoundToInt(value)));
    }
    private IEnumerator ChangeLanguage(int value) {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
        base.SetValue(value);
    }
    public override void Save() {
        // We don't care to save... Unity already manages language loading and preferences.
    }
    public override void Load() {
        if (ScriptableSettingsManager.instance == null) {
            throw new UnityException("Need ScriptableSettingsManager in the scene to load languages properly...");
        }
        ScriptableSettingsManager.instance.StartCoroutine(OverrideDropdownWithLanguages());
    }
    private IEnumerator OverrideDropdownWithLanguages() {
        yield return LocalizationSettings.InitializationOperation;
        minValue = 0;
        maxValue = LocalizationSettings.AvailableLocales.Locales.Count-1;
        for(int i=0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++) {
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i]) {
                defaultValue = i;
                break;
            }
        }
        dropdownOptions = new string[LocalizationSettings.AvailableLocales.Locales.Count];
        for(int i=0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++) {
            dropdownOptions[i] = LocalizationSettings.AvailableLocales.Locales[i].name;
        }
        internalValue = defaultValue;
    }
}

}