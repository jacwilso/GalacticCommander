using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxValueAttribute))]
public class MinMaxValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinMaxValueAttribute minMax = attribute as MinMaxValueAttribute;

        position = EditorGUI.PrefixLabel(position, label);

        float floatWidth = 50f;
        float padding = 5f;
        float width = position.width - 2 * (floatWidth + padding);

        Rect minField = new Rect(position.x, position.y, floatWidth, position.height);
        position.x += floatWidth + padding;
        Rect sliderField = new Rect(position.x, position.y, width, position.height);
        position.x += width + padding;
        Rect maxField = new Rect(position.x, position.y, floatWidth, position.height);

        Vector2 vec;
        if (property.propertyType == SerializedPropertyType.Vector2)
        {
            vec = property.vector2Value;
            vec.x = EditorGUI.FloatField(minField, vec.x);
            vec = MinMaxSlider(sliderField, minMax, vec);
            position.x += 50;
            vec.y = EditorGUI.FloatField(maxField, vec.y);
            property.vector2Value = vec;
        }
        else if (property.propertyType == SerializedPropertyType.Vector2Int)
        {
            vec = property.vector2Value;
            vec.x = EditorGUI.FloatField(minField, vec.x);
            vec = MinMaxSlider(position, minMax, property.vector2IntValue);
            vec.y = EditorGUI.FloatField(maxField, vec.y);
            property.vector2IntValue = new Vector2Int((int)vec.x, (int)vec.y);
        }
    }

    private Vector2 MinMaxSlider(Rect position, MinMaxValueAttribute minMax, Vector2 vec)
    {
        EditorGUI.MinMaxSlider(position, ref vec.x, ref vec.y, minMax.minVal, minMax.maxVal);
        return vec;
    }
}
