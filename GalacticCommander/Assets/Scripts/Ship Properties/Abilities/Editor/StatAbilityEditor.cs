using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(StatAbility))]
[CanEditMultipleObjects]
public class StatAbilityEditor : Editor
{

    private List<string> _properties = new List<string>();
    private int _index = 0;

    private List<string> shipProperties, //= new List<string>(),
        attackProperties; //= new List<string>();

    private void OnEnable()
    {
        shipProperties = CreateInstance<ShipProperties>().GetType().GetFields().Where(param => param.FieldType.IsGenericType && param.FieldType.GetGenericTypeDefinition() == typeof(ModifiableStat<>)).Select(param => ObjectNames.NicifyVariableName(param.Name)).ToList();

        attackProperties = CreateInstance<AttackProperties>().GetType().GetFields().Where(param => param.FieldType.IsGenericType && param.FieldType.GetGenericTypeDefinition() == typeof(ModifiableStat<>)).Select(param => ObjectNames.NicifyVariableName(param.Name)).ToList();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        StatAbility statAbility = target as StatAbility;

        statAbility.propertyObject = (PropertyObject)EditorGUILayout.EnumPopup("Property Object", statAbility.propertyObject);
        switch (statAbility.propertyObject)
        {
            case PropertyObject.Ship:
                _properties = shipProperties;
                
                break;
            case PropertyObject.Attack:
                _properties = attackProperties;
                break;
        }
        _index = EditorGUILayout.Popup("Parameter", _index, _properties.ToArray());
        statAbility.parameter = _properties[_index];
        EditorUtility.SetDirty(target);
    }
}