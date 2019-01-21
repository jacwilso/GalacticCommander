using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(WeaponProperties))]
public class WeaponPropertiesInspector : Editor
{
    WeaponProperties attack;

    public override void OnInspectorGUI()
    {
        attack = target as WeaponProperties;

        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;
        string[] excludeProps = new string[] { "m_Script", "damage" };
        DrawPropertiesExcluding(serializedObject, excludeProps);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
        SerializedProperty[] properties = new SerializedProperty[excludeProps.Length - 1];
        var dmgTypeNames = Enum.GetNames(typeof(DamageType));
        var dmgTypeVals = Enum.GetValues(typeof(DamageType));

        EditorGUI.indentLevel++;
        SerializedProperty prop = serializedObject.FindProperty("damage");
        for (int j = 0; j < dmgTypeNames.Length; j++)
        {
            SerializedProperty vecProp = prop.GetArrayElementAtIndex(j);
            if (!attack.DamageTypes.HasFlag((DamageType)dmgTypeVals.GetValue(j)))
            {
                vecProp.vector2IntValue = Vector2Int.zero;
                continue;
            }
            Vector2 vec = vecProp.vector2IntValue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(dmgTypeNames[j]);
            vec.x = EditorGUILayout.IntField((int)vec.x);
            EditorGUILayout.MinMaxSlider(ref vec.x, ref vec.y, 0, Mathf.Max(vec.y + 50f, 100f));
            vec.y = EditorGUILayout.IntField((int)vec.y);
            EditorGUILayout.EndHorizontal();

            vec.x = Mathf.Max(vec.x, 0);
            vec.y = Mathf.Max(vec.y, vec.x + 1);
            vecProp.vector2IntValue = new Vector2Int((int)vec.x, (int)vec.y);
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
