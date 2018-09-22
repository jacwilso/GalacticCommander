using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipProperties))]
public class ShipPropertiesEditor : Editor
{
    ShipProperties ship;
    bool foldout;
    Editor modifierEditor;

    private void OnEnable()
    {
        ship = target as ShipProperties;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        foldout = EditorGUILayout.InspectorTitlebar(foldout, ship.modifier);
        if (foldout)
        {
            CreateCachedEditor(ship.modifier, null, ref modifierEditor);
            modifierEditor.OnInspectorGUI();
        }
        serializedObject.ApplyModifiedProperties();
    }
}