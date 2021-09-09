using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "New Scriptable Setting Group", menuName = "Unity Scriptable Setting/New Grouping", order = -1)]
public class ScriptableSettingGroup : ScriptableObject {
    public LocalizedString localizedName;
}

}