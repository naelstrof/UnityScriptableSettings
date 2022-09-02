using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace UnityScriptableSettings {
public class SettingsManager : MonoBehaviour {
    private static SettingsManager instance;
    [SerializeField, SerializeReference]
    private List<Setting> settings;

    private ReadOnlyCollection<Setting> readOnlySettings;
    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        readOnlySettings = settings.AsReadOnly();
    }

    public static Coroutine StaticStartCoroutine(IEnumerator routine) {
        return instance.StartCoroutine(routine);
    }

    public Setting GetSetting(string name) {
        foreach(Setting s in settings) {
            if (s.name == name) {
                return s;
            }
        }
        return null;
    }

    public static ReadOnlyCollection<Setting> GetSettings() {
        return instance.readOnlySettings;
    }

    public void Start() {
        settings.Sort((a,b)=>String.Compare(a.group.name.ToString(), b.group.name.ToString(), StringComparison.Ordinal));
        foreach(var setting in settings) {
            setting.Load();
        }
    }
    public void Save() {
        foreach(var setting in settings) {
            setting.Save();
        }
        PlayerPrefs.Save();
    }
    public void ResetToDefault(SettingGroup group) {
        foreach(var setting in settings) {
            if (setting.group == group || group == null) {
                setting.ResetToDefault();
            }
        }
    }
    public void ResetToDefault() {
        foreach(var setting in settings) {
            setting.ResetToDefault();
        }
    }
    void OnDestroy() {
        Save();
    }
}

}
