using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace UnityScriptableSettings {
    public abstract class Setting : ScriptableObject {
        [SerializeField] private ScriptableSettingString label;
        public ScriptableSettingString GetLabel() {
            return label;
        }
        
        [Tooltip("Name of the group that the setting belongs to (audio, graphics, gameplay...")]
        public SettingGroup group;

        public abstract void ResetToDefault();
        public abstract void Save();
        public abstract void Load();
        public virtual void OnValidate() { }
    }
}