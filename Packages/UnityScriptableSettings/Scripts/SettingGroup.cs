using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "New Scriptable Setting Group", menuName = "Unity Scriptable Setting/New Grouping", order = 0)]
public class SettingGroup : ScriptableObject {
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
}

}