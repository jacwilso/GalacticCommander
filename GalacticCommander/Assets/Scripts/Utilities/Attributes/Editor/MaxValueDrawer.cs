using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MaxValueAttribute))]
public class MaxValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MaxValueAttribute maxAttrib = attribute as MaxValueAttribute;

        position = EditorGUI.PrefixLabel(position, label);
        if (property.propertyType == SerializedPropertyType.Float)
        {
            property.floatValue = Mathf.Min(maxAttrib.maxVal, EditorGUI.FloatField(position, property.floatValue));
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = (int)Mathf.Min(maxAttrib.maxVal, EditorGUI.IntField(position, property.intValue));
        }
    }
}
