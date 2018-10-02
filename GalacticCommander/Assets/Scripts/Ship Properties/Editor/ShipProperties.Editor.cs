using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipProperties))]
public class ShipPropertiesEditor : Editor
{
    ShipProperties ship;
    bool foldout = true;
    Editor modifierEditor;

    private void OnEnable()
    {
        ship = target as ShipProperties;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if (ship.modifier) {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, ship.modifier);
            if (foldout)
            {
                CreateCachedEditor(ship.modifier, null, ref modifierEditor);
                modifierEditor.OnInspectorGUI();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}