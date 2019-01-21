using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipProperties))]
public class ShipPropertiesEditor : Editor
{
    ShipProperties ship;
    bool foldout = true;
    Editor modifierEditor;

    void OnEnable()
    {
        ship = target as ShipProperties;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        if (ship.Profile)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, ship.Profile);
            if (foldout)
            {
                CreateCachedEditor(ship.Profile, null, ref modifierEditor);
                modifierEditor.OnInspectorGUI();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}