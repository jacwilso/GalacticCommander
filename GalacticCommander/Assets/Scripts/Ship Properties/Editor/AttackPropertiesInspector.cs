using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(AttackProperties))]
public class AttackPropertiesInspector : Editor
{
    AttackProperties attack;

    public override void OnInspectorGUI()
    {
        attack = target as AttackProperties;

        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;
        string[] excludeProps = new string[] { "m_Script", "damage" };
        DrawPropertiesExcluding(serializedObject, excludeProps);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
        SerializedProperty[] properties = new SerializedProperty[excludeProps.Length - 1];
        List<ResistanceTypes> types = GetTypeList(attack.DamageTypes);

        EditorGUI.indentLevel++;
        SerializedProperty prop = serializedObject.FindProperty("damage");
        for (int j = 0; j < types.Count; j++)
        {
            if (prop.arraySize <= j)
            {
                prop.InsertArrayElementAtIndex(j);
            }
            SerializedProperty vecProp = prop.GetArrayElementAtIndex(j);
            Vector2 vec = vecProp.vector2IntValue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(ObjectNames.NicifyVariableName(types[j].ToString()));
            vec.x = EditorGUILayout.IntField((int)vec.x);
            EditorGUILayout.MinMaxSlider(ref vec.x, ref vec.y, 0, Mathf.Max(vec.y + 50f, 100f));
            vec.y = EditorGUILayout.IntField((int)vec.y);
            EditorGUILayout.EndHorizontal();

            vec.x = Mathf.Max(vec.x, 0);
            vec.y = Mathf.Max(vec.y, vec.x + 1);
            vecProp.vector2IntValue = new Vector2Int((int)vec.x, (int)vec.y);
        }
        for (int j = types.Count; j < prop.arraySize; j++)
        {
            prop.DeleteArrayElementAtIndex(j);
        }
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }

    List<ResistanceTypes> GetTypeList(ResistanceTypes types)
    {
        List<ResistanceTypes> result = new List<ResistanceTypes>();
        foreach (ResistanceTypes r in Enum.GetValues(typeof(ResistanceTypes)))
        {
            if (types.HasFlag(r))
            {
                result.Add(r);
            }
        }
        return result;
    }
}
