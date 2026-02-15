using UnityEngine;

namespace UnityScriptableSettings {

[CreateAssetMenu(fileName = "New Scriptable Setting Group", menuName = "Unity Scriptable Setting/New Grouping", order = 0)]
public class SettingGroup : ScriptableObject {
    [SerializeField] private ScriptableSettingString label;
    public ScriptableSettingString GetLabel() {
        return label;
    }
}

}