using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace UnityScriptableSettings {
    [CreateAssetMenu(fileName = "New Localized Dropdown", menuName = "Unity Scriptable Setting/Localized Dropdown", order = 0)]
    public class ScriptableSettingLocalizedDropdown : ScriptableSetting {
        public LocalizedString[] dropdownOptions;
        public override void OnValidate() {
            minValue = 0;
            maxValue = Mathf.Max(dropdownOptions.Length-1,0f);
            base.OnValidate();
        }
    }
}
