using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(PositionHandleExample)), CanEditMultipleObjects]
//public class PositionHandleExampleEditor : Editor
//{
//    protected virtual void OnSceneGUI()
//    {
//        PositionHandleExample example = (PositionHandleExample)target;

//        EditorGUI.BeginChangeCheck();
//        Vector3 newTargetPosition = Handles.PositionHandle(example.targetPosition, Quaternion.identity);
//        if (EditorGUI.EndChangeCheck())
//        {
//            Undo.RecordObject(example, "Change Look At Target Position");
//            example.targetPosition = newTargetPosition;
//            example.Update();
//        }
//    }
//}

[CustomEditor(typeof(PositionHandleExample)), CanEditMultipleObjects]
public class PositionHandleExampleEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        PositionHandleExample example = (PositionHandleExample)target;

        float size = HandleUtility.GetHandleSize(example.targetPosition) * 0.5f;
        Vector3 snap = Vector3.one * 0.5f;
        
        EditorGUI.BeginChangeCheck();
        Vector3 newTargetPosition = Handles.FreeMoveHandle(example.targetPosition, Quaternion.identity, size, snap, Handles.CubeHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(example, "Change Look At Target Position");
            example.targetPosition = newTargetPosition;
            example.Update();
        }
    }
}