using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumFlagAttribute))]
public class EnumFlagDrawer : PropertyDrawer
{
    // https://www.alanzucconi.com/2015/07/26/enum-flags-and-bitwise-operators/

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Enum targetEnum = GetBaseProperty<Enum>(property);
        Enum enumNew = EditorGUI.EnumFlagsField(position, label, targetEnum);
        property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
    }

    static T GetBaseProperty<T>(SerializedProperty prop)
    {
        // Separate the steps it takes to get to this property
        string[] separatedPaths = prop.propertyPath.Split('.');

        // Go down to the root of this serialized property
        System.Object reflectionTarget = prop.serializedObject.targetObject as object;
        // Walk down the path to get the target object
        foreach (var path in separatedPaths)
        {
            FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            reflectionTarget = fieldInfo.GetValue(reflectionTarget);
        }
        return (T)reflectionTarget;
    }
}
