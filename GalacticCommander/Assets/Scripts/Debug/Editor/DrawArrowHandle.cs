using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawArrow))]
public class DrawArrowEditor : Editor
{
    void OnSceneGUI()
    {
        DrawArrow t = target as DrawArrow;

        if (t == null || t.From == null || t.To == null)
            return;

        Vector3[] arrowHead = new Vector3[3];
        Vector3[] arrowLine = new Vector3[2];
        Vector3 start = t.From.transform.position;
        Vector3 end = t.To.transform.position;

        Vector3 forward = (end - start).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        float size = HandleUtility.GetHandleSize(end);
        float width = size * 0.1f;
        float height = size * 0.3f;

        arrowHead[0] = end;
        arrowHead[1] = end - forward * height + right * width;
        arrowHead[2] = end - forward * height - right * width;

        arrowLine[0] = start;
        arrowLine[1] = end - forward * height;

        Handles.color = Color.red;
        Handles.DrawAAPolyLine(arrowLine);
        Handles.DrawAAConvexPolygon(arrowHead);
    }
}