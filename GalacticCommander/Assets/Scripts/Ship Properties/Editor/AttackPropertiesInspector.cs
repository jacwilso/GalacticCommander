using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(AttackProperties))]
public class AttackPropertiesInspector : Editor
{
    private AttackProperties attack;

    public override void OnInspectorGUI()
    {
        attack = target as AttackProperties;

        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;
        string[] excludeProps = new string[] { "m_Script", "front", "back", "top", "bottom", "left", "right" };
        DrawPropertiesExcluding(serializedObject, excludeProps);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);
        SerializedProperty[] properties = new SerializedProperty[excludeProps.Length - 1];
        List<ResistanceTypes> types = GetTypeList(attack.DamageTypes);
        for (int i = 1; i < excludeProps.Length; i++)
        {
            SerializedProperty prop = serializedObject.FindProperty(excludeProps[i]);
            SerializedProperty it = prop.Copy();
            EditorGUILayout.PropertyField(prop);
            if (prop.isExpanded)
            {
                EditorGUI.indentLevel++;
                while (it.Next(true) && it.propertyPath.Split('.').Length < 3)
                {
                    SerializedProperty childProp = prop.FindPropertyRelative(it.name);
                    if (it.name != "damage")
                    {
                        EditorGUILayout.PropertyField(childProp);
                    }
                    else
                    {

                        for (int j = 0; j < types.Count; j++)
                        {
                            if (childProp.arraySize <= j)
                            {
                                childProp.InsertArrayElementAtIndex(j);
                            }
                            SerializedProperty vecProp = childProp.GetArrayElementAtIndex(j);
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
                    }

                }
                EditorGUI.indentLevel--;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private List<ResistanceTypes> GetTypeList(ResistanceTypes types)
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
