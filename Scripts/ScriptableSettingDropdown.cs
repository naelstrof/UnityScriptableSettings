using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace UnityScriptableSettings {
    [CreateAssetMenu(fileName = "New Dropdown", menuName = "Unity Scriptable Setting/Dropdown", order = 0)]
    public class ScriptableSettingDropdown : ScriptableSetting {
        public string[] dropdownOptions;
        public override void OnValidate() {
            minValue = 0;
            maxValue = Mathf.Max(dropdownOptions.Length-1,0);
            base.OnValidate();
        }
    }
}
