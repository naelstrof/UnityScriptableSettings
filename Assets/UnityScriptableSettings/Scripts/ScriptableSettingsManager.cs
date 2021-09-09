using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScriptableSettings {
public class ScriptableSettingsManager : MonoBehaviour {
    public static ScriptableSettingsManager instance;
    public ScriptableSetting[] settings;
    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public ScriptableSetting GetSetting(string name) {
        foreach(ScriptableSetting s in settings) {
            if (s.name == name) {
                return s;
            }
        }
        return null;
    }
    public void Start() {
        System.Array.Sort(settings, (a,b)=>{return a.group.name.ToString().CompareTo(b.group.name.ToString());});
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
    void OnDestroy() {
        Save();
    }
}

}
