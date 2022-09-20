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
        Debug.Log($"Language set to {LocalizationSettings.SelectedLocale.name}.");
        base.SetValue(value);
    }
    public override void Load() {
        SettingsManager.StaticStartCoroutine(OverrideDropdownWithLanguages());
    }
    private IEnumerator OverrideDropdownWithLanguages() {
        yield return LocalizationSettings.InitializationOperation;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
            if (LocalizationSettings.SelectedLocale.name != LocalizationSettings.AvailableLocales.Locales[i].name) continue;
            defaultValue = i;
            Debug.Log($"Detected {LocalizationSettings.SelectedLocale.name} as default Locale.");
            break;
        }
        dropdownOptions = new string[LocalizationSettings.AvailableLocales.Locales.Count];
        for(int i=0;i<LocalizationSettings.AvailableLocales.Locales.Count;i++) {
            dropdownOptions[i] = LocalizationSettings.AvailableLocales.Locales[i].name;
        }
        base.Load();
    }
}

}