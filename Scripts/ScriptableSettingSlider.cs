using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScriptableSettings {
    [CreateAssetMenu(fileName = "New Generic Slider", menuName = "Unity Scriptable Setting/Slider", order = 0)]
    public class ScriptableSettingSlider : ScriptableSetting {
        public bool wholeNumbers = false;
    }
}
