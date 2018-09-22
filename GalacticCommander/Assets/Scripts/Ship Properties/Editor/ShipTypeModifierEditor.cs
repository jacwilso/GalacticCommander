using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipTypeModifier))]
public class ShipTypeModifierEditor : Editor
{
    private ShipTypeModifier modifier;
    private readonly string[] sides = { "Back", "Left", "Bottom", "Front", "Top", "Right" };

    private void OnEnable()
    {
        modifier = target as ShipTypeModifier;
    }

    public override void OnInspectorGUI()
    {
        SerializedProperty mod = serializedObject.FindProperty("modifier");
        for (int i = 0; i < sides.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(sides[i]);
            modifier.enable[i] = EditorGUILayout.Toggle(name, modifier.enable[i]);
            GUI.enabled = modifier.enable[i];
            modifier.modifier[i] = EditorGUILayout.IntField(modifier.modifier[i]);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}
