using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DamageProfile))]
public class DamageProfileEditor : Editor
{
    DamageProfile dmgProfile;
    static readonly string[] SIDE_NAMES = { "Back", "Left", "Bottom", "Front", "Top", "Right" };

    void OnEnable()
    {
        dmgProfile = target as DamageProfile;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty modifierArr = serializedObject.FindProperty("modifier");
        SerializedProperty enabledArr = serializedObject.FindProperty("enabled");
        for (int i = 0; i < SIDE_NAMES.Length; i++)
        {
            var enabled = enabledArr.GetArrayElementAtIndex(i);
            var modifier = modifierArr.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(SIDE_NAMES[i]);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(enabled, GUIContent.none, GUILayout.MaxWidth(20));
            if (EditorGUI.EndChangeCheck())
            {
                if (!enabled.boolValue) modifier.intValue = 0;
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(modifier, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                enabled.boolValue = modifier.intValue > 0;
            }
            EditorGUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
