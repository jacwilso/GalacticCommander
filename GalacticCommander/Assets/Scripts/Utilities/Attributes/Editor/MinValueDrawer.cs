using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinValueAttribute))]
public class MinValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinValueAttribute minAttrib = attribute as MinValueAttribute;

        position = EditorGUI.PrefixLabel(position, label);
        if (property.propertyType == SerializedPropertyType.Float)
        {
            property.floatValue = Mathf.Max(minAttrib.minVal, EditorGUI.FloatField(position, property.floatValue));
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = (int)Mathf.Max(minAttrib.minVal, EditorGUI.IntField(position, property.intValue));
        }
    }
}
