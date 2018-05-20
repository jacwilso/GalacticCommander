using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(StatAbility))]
[CanEditMultipleObjects]
public class StatAbilityEditor : Editor
{
    private Dictionary<string, string> shipProperties = new Dictionary<string, string>(),
        attackProperties = new Dictionary<string, string>();

    private void OnEnable()
    {
        PropertyDictionary(typeof(ShipProperties), shipProperties);
        PropertyDictionary(typeof(AttackProperties), attackProperties);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Dictionary<string, string> _properties = null;
        int _index = 0;
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
        string[] keys = _properties.Keys.ToArray();
        _index = EditorGUILayout.Popup("Parameter", _index, keys);
        statAbility.parameter = _properties[keys[_index]];
        EditorUtility.SetDirty(target);
    }

    private void PropertyDictionary(Type obj, Dictionary<string, string> properties)
    {
        CreateInstance(obj).GetType().GetFields().Where(param => param.FieldType == typeof(Stat)).ToList().ForEach(param => {
            string var = ObjectNames.NicifyVariableName(param.Name);
            properties.Add(var, param.Name);
        });
    }
}