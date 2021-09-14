using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Reflection;
#endif


namespace UnityScriptableSettings {
public class OverrideDropdown : System.Attribute {}

#if UNITY_EDITOR
[CustomEditor(typeof(ScriptableSetting), true)]
public class ScriptableSettingEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        bool hideDropdown = false;
        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(serializedObject.targetObject.GetType());  // Reflection.  
        // Displaying output.  
        foreach (System.Attribute attr in attrs)  {  
            if (attr is OverrideDropdown)  {  
                hideDropdown = true;
            }
        }
        bool hideRange = serializedObject.targetObject is ScriptableSettingDropdown || serializedObject.targetObject is ScriptableSettingLocalizedDropdown;
        SerializedProperty prop = serializedObject.GetIterator();
        if (prop.NextVisible(true)) {
            do {
                if (prop.name == "minValue" && hideRange) {
                    continue;
                }
                if (prop.name == "maxValue" && hideRange) {
                    continue;
                }
                if (prop.name == "dropdownOptions" && hideDropdown) {
                    continue;
                }
                EditorGUILayout.PropertyField(prop);
            } while (prop.NextVisible(false));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
public class ScriptableSetting : ScriptableObject {
    public delegate void ScriptableSettingValueChange(ScriptableSetting option);
    public ScriptableSettingValueChange onValueChange;
    [Tooltip("Label of the option.")]
    public LocalizedString localizedName;
    [Tooltip("Name of the group that the setting belongs to (audio, graphics, gameplay...")]
    public ScriptableSettingGroup group;
    protected float internalValue;
    public float GetValue() {
        return internalValue;
    }
    public float value {get => GetValue();}
    public virtual void SetValue(float value) {
        if (internalValue == value) {
            return;
        }
        float oldValue = internalValue;
        internalValue = value;
        if (onValueChange != null) {
            onValueChange(this);
        }

        if (oldValue == 0 && value != 0) {
            OnSettingEnabled.Invoke();
        }
        if (value == 0 && oldValue != 0) {
            OnSettingDisabled.Invoke();
        }
        OnSettingChanged.Invoke(internalValue);
    }
    public UnityEvent OnSettingDisabled;
    public UnityEvent OnSettingEnabled;
    [System.Serializable]
    public class UnityFloatEvent : UnityEvent<float> {}
    public UnityFloatEvent OnSettingChanged;

    [Tooltip("Value that gets used if no preferences are found.")]
    [SettingDefaultValue]
    public float defaultValue;
    [Tooltip("Value will be clamped with minValue as the lower bound. Only used for sliders.")]
    public float minValue;
    [Tooltip("Value will be clamped with maxValue as the upper bound. Only used for sliders.")]
    public float maxValue;
    // We call Save right when we exit the game in GameManager, just in case someone created settings that cause a crash or display issues on their system.
    public virtual void Save() {
        PlayerPrefs.SetFloat(name, internalValue);
    }
    // We call load in GameManager, as some resources are generally not ready on Awake or OnEnable (like the graphics and localization for example.)
    public virtual void Load() {
        SetValue(Mathf.Clamp(PlayerPrefs.GetFloat(name, defaultValue), minValue, maxValue));
    }
    public virtual void OnValidate() {
        defaultValue = Mathf.Clamp(defaultValue, minValue, maxValue);
    }
}

}