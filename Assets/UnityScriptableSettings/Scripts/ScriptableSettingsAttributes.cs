using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityScriptableSettings {
public class SettingDefaultValueAttribute : PropertyAttribute {}
public class OverrideDefaultValue : System.Attribute {}

#if UNITY_EDITOR
public class SettingPropertyDrawer : PropertyDrawer {
    protected virtual bool ShouldShow(SerializedProperty property) {
        return true;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (ShouldShow(property)) {
            EditorGUI.PropertyField(position, property, label, true); //fun fact: base.OnGUI doesn't work! Check for yourself!
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if(ShouldShow(property)) {
            return base.GetPropertyHeight(property, label);
        } else {
            return 0;
        }
    }
}
[CustomPropertyDrawer(typeof (SettingDefaultValueAttribute))]
public class DefaultValueAttributeDrawer : SettingPropertyDrawer {
    protected override bool ShouldShow(SerializedProperty property) {
        Type type = property.serializedObject.targetObject.GetType();
        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(type);  // Reflection.  
        // Displaying output.  
        foreach (System.Attribute attr in attrs)  {  
            if (attr is OverrideDefaultValue) {  
                return false;
            }
        }
        return true;
    }
}

#endif
}