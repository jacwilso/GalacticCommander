using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Vector3Bool))]
public class Vector3BoolDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, label);

        BoolField(ref position, property, label, "x");
        BoolField(ref position, property, label, "y");
        BoolField(ref position, property, label, "z");
    }

    private void BoolField(ref Rect position, SerializedProperty property, GUIContent label, string str)
    {
        SerializedProperty xProp = property.FindPropertyRelative(str);

        label.text = str.ToUpper();
        EditorGUI.LabelField(position, label);
        position.x += 15;
        xProp.boolValue = EditorGUI.Toggle(position, xProp.boolValue);
        position.x += 25;
    }
}
