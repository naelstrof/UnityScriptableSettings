using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace UnityScriptableSettings {
    public abstract class Setting : ScriptableObject {
        [Tooltip("Label of the option. Can be null")]
        [SerializeField,FormerlySerializedAs("localizedName")] private LocalizedString localizedLabel;
        [Tooltip("Backup label of the option. Only necessary if the localized label is null.")]
        [SerializeField] private string backupLabel;
        public string GetLabel() {
            if (localizedLabel == null) {
                if (string.IsNullOrEmpty(backupLabel)) {
                    return name;
                }
                return backupLabel;
            }
            return localizedLabel.GetLocalizedString();
        }
        
        public bool TryGetLocalizedLabel(out LocalizedString localizedLabel) {
            localizedLabel = this.localizedLabel;
            return this.localizedLabel != null;
        }
        
        [Tooltip("Name of the group that the setting belongs to (audio, graphics, gameplay...")]
        public SettingGroup group;

        public abstract void ResetToDefault();
        public abstract void Save();
        public abstract void Load();
        public virtual void OnValidate() { }
    }
}