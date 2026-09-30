using System;
using UnityEngine;
using UnityEngine.Localization;

namespace UnityScriptableSettings {
    [CreateAssetMenu(fileName = "New Bool", menuName = "Unity Scriptable Setting/Bool", order = 14)]
    public class SettingBool : Setting {
        public delegate void SettingBoolAction(bool newValue);
        public event SettingBoolAction changed;
        [SerializeField]
        protected bool defaultValue;
        protected bool selectedValue;
        [SerializeField] protected ScriptableSettingString offOption = new ScriptableSettingString("Off");
        [SerializeField] protected ScriptableSettingString onOption = new ScriptableSettingString("On");
        public virtual void SetValue(bool value) {
            if (selectedValue == value) {
                return;
            }
            selectedValue = value;
            changed?.Invoke(selectedValue);
        }
        public virtual bool GetValue() {
            return selectedValue;
        }

        public ScriptableSettingString GetLocalizedOffOption() {
            return offOption;
        }
        public ScriptableSettingString GetLocalizedOnOption() {
            return onOption;
        }

        public override void ResetToDefault() {
            SetValue(defaultValue);
        }

        public override void Save() {
            PlayerPrefs.SetInt(name, GetValue() ? 1 : 0);
        }
        public override void Load() {
            SetValue(PlayerPrefs.GetInt(name, defaultValue ? 1 : 0) == 1);
        }
        public void NotifyChange() {
            changed?.Invoke(selectedValue);
        }
    }
}