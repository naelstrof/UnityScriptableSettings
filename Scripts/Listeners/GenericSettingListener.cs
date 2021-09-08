using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityScriptableSettings {
    public class GenericSettingListener : MonoBehaviour {
        [System.Serializable]
        public class UnityFloatEvent : UnityEvent<float>{};
        public ScriptableSetting targetSetting;

        [Tooltip("When the setting is enabled, this event will fire.")]
        public UnityEvent OnSettingEnabled;
        [Tooltip("When the setting is disabled, this event will fire.")]
        public UnityEvent OnSettingDisabled;
        [Tooltip("When the setting is changed, this event will fire.")]
        public UnityFloatEvent OnSettingChanged;
        void Start() {
            if (targetSetting.GetValue() != targetSetting.minValue) {
                OnSettingEnabled.Invoke();
            } else {
                OnSettingDisabled.Invoke();
            }
            targetSetting.onValueChange -= SettingChanged;
            targetSetting.onValueChange += SettingChanged;
            SettingChanged(targetSetting);
        }
        void OnEnable() {
            Start();
        }
        void OnDisable() {
            targetSetting.onValueChange -= SettingChanged;
        }
        void SettingChanged(ScriptableSetting setting) {
            if (setting.GetValue() == setting.minValue) {
                OnSettingDisabled.Invoke();
            }
            if (setting.GetValue() != setting.minValue) {
                OnSettingEnabled.Invoke();
            }
            OnSettingChanged.Invoke(setting.GetValue());
        }
    }
}
