using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinValueAttribute))]
public class MinValueDrawer : PropertyDrawer
{
    private bool expand;
    private float height;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinValueAttribute minAttrib = attribute as MinValueAttribute;
        switch (property.propertyType)
        {
            case SerializedPropertyType.Float:
                position = EditorGUI.PrefixLabel(position, label);
                property.floatValue = Mathf.Max(minAttrib.minVal, EditorGUI.FloatField(position, property.floatValue));
                break;
            case SerializedPropertyType.Integer:
                position = EditorGUI.PrefixLabel(position, label);
                property.intValue = (int)Mathf.Max(minAttrib.minVal, EditorGUI.IntField(position, property.intValue));
                break;
            case SerializedPropertyType.Vector2:
                Vector2 v2 = EditorGUI.Vector2Field(position, label, property.vector2Value);
                property.vector3Value = Vector2.Max(Vector2.one * minAttrib.minVal, v2);
                expand = position.width < 310;
                break;
            case SerializedPropertyType.Vector2Int:
                Vector2Int v2Int = EditorGUI.Vector2IntField(position, label, property.vector2IntValue);
                property.vector2IntValue = Vector2Int.Max(Vector2Int.one * (int)minAttrib.minVal, v2Int);
                expand = position.width < 312;
                break;
            case SerializedPropertyType.Vector3:
                Vector3 v3 = EditorGUI.Vector3Field(position, label, property.vector3Value);
                property.vector3Value = Vector3.Max(Vector2.one * minAttrib.minVal, v3);
                expand = position.width < 312;
                break;
            case SerializedPropertyType.Vector3Int:
                Vector3Int v3Int = EditorGUI.Vector3IntField(position, label, property.vector3IntValue);
                property.vector3IntValue = Vector3Int.Max(Vector3Int.one * (int)minAttrib.minVal, v3Int);
                expand = position.width < 312;
                break;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + (expand ? 22 : 0);
    }
}
