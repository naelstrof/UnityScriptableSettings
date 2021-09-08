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
    public void Start() {
        System.Array.Sort(settings, (a,b)=>{return a.group.TableEntryReference.ToString().CompareTo(b.group.TableEntryReference.ToString());});
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
