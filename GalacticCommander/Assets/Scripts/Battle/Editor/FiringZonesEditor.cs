using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FiringZone))]
public class FiringZoneEditor : Editor
{
    private bool showInnerHandles = true,
        showOuterHandles = true;
    private Transform testTarget;
    private static Transform staticTarget;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        EditorGUI.BeginChangeCheck();
        staticTarget = testTarget = EditorGUILayout.ObjectField("Target", testTarget, typeof(Transform), true) as Transform;
        showInnerHandles = EditorGUILayout.Toggle("Show Inner Handles", showInnerHandles);
        showOuterHandles = EditorGUILayout.Toggle("Show Outer Handles", showOuterHandles);
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    protected virtual void OnSceneGUI()
    {
        FiringZone zone = target as FiringZone;
        if (showInnerHandles)
        {
            BoxHandles(zone, ref zone.innerScale, zone.outerScale, false, "innerScale");
        }
        if (showOuterHandles)
        {
            BoxHandles(zone, ref zone.outerScale, zone.innerScale, true, "outerScale");
        }
    }

    private void BoxHandles(FiringZone zone, ref Vector3 scale, Vector3 compare, bool stayBigger, string propertyName)
    {
        float size = 0.025f;
        Handles.color = Color.green;
        MinValueAttribute minAttrib = zone.GetType().GetField(propertyName).GetCustomAttributes(typeof(MinValueAttribute), true)[0] as MinValueAttribute;
        for (int i = 0; i < FiringZone.cubeVerticies.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 vec = Handles.FreeMoveHandle(
                zone.transform.position + 0.5f * Vector3.Scale(scale, FiringZone.cubeVerticies[i]),
                zone.transform.rotation,
                size, Vector3.one, Handles.DotHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(zone, "Undo resize: " + FiringZone.cubeVerticies[i]);
                Vector3 scaled = 2f * (vec - zone.transform.position);
                scaled = Vector3.Scale(scaled, FiringZone.cubeVerticies[i]);
                scaled = Vector3.Max(scaled, Vector3.one * minAttrib.minVal);
                for (int j = 0; j < 3; j++)
                {
                    if (stayBigger && scaled[j] <= compare[j])
                    {
                        scaled[j] = compare[j] + minAttrib.minVal;
                    }
                    else if (!stayBigger && scaled[j] >= compare[j])
                    {
                        scaled[j] = compare[j] - minAttrib.minVal;
                    }
                }
                scale = scaled;
            }
        }
    }

    [DrawGizmo(GizmoType.Active | GizmoType.NonSelected)]
    static void DrawGizmo(FiringZone scr, GizmoType gizmoType)
    {
        Matrix4x4 m = Gizmos.matrix;
        Gizmos.matrix = scr.transform.localToWorldMatrix;

        Gizmos.DrawWireCube(Vector3.zero, scr.InnerScale);
        Gizmos.DrawWireCube(Vector3.zero, scr.OuterScale);
        for (int i = 0; i < 8; i++)
        {
            Vector3 a = 0.5f * Vector3.Scale(scr.InnerScale, FiringZone.cubeVerticies[i]);
            Vector3 b = 0.5f * Vector3.Scale(scr.OuterScale, FiringZone.cubeVerticies[i]);
            Gizmos.DrawLine(a, b);
        }

        if (staticTarget == null)
        {
            return;
        }

        scr.RecalculateFrustum();

        FiringZone.Face face = scr.FrustrumFace(staticTarget.position);
        int[] winding =
        {
            // back
            6, 4, 0, 2,
            // left
            3, 2, 0, 1,
            // bottom
            5, 1, 0, 4,
            // front
            7, 3, 1, 5,
            // top
            7, 6, 2, 3,
            // right
            7, 5, 4, 6,
        };

        if (face != FiringZone.Face.None)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 a = 0.5f * Vector3.Scale(scr.OuterScale, FiringZone.cubeVerticies[winding[4 * (int)face + i]]);
                Vector3 b = 0.5f * Vector3.Scale(scr.OuterScale, FiringZone.cubeVerticies[winding[4 * (int)face + (i + 1) % 4]]);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(a, b);
            }
        }

        Gizmos.matrix = m;
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(scr.transform.position, staticTarget.position);
        }
    }
}
