using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DamageProfile))]
public class ShipTypeModifierEditor : Editor
{
    DamageProfile modifier;
    static readonly string[] SIDE_NAMES = { "Back", "Left", "Bottom", "Front", "Top", "Right" };

    void OnEnable()
    {
        modifier = target as DamageProfile;
    }

    public override void OnInspectorGUI()
    {
        SerializedProperty mod = serializedObject.FindProperty("modifier");
        for (int i = 0; i < SIDE_NAMES.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(SIDE_NAMES[i]);
            modifier.enabled[i] = EditorGUILayout.Toggle(name, modifier.enabled[i], GUILayout.MaxWidth(25));
            GUI.enabled = modifier.enabled[i];
            // modifier.modifier[i] = EditorGUILayout.IntField(modifier.modifier[i]);
            modifier.profile[i] = EditorGUILayout.IntSlider(modifier.profile[i], -100, 100);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}
