using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipTypeModifier))]
public class ShipTypeModifierEditor : Editor
{
    readonly string[] sides = { "front", "back", "left", "right", "top", "bottom" };

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < sides.Length; i++)
        {
            string name = ObjectNames.NicifyVariableName(sides[i]);
            SerializedProperty enable = serializedObject.FindProperty("enable" + name);
            SerializedProperty side = serializedObject.FindProperty(sides[i]);
            EditorGUILayout.BeginHorizontal();
            enable.boolValue = EditorGUILayout.Toggle(name, enable.boolValue);
            GUI.enabled = enable.boolValue;
            side.intValue = EditorGUILayout.IntSlider(side.intValue, 0, 100);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}