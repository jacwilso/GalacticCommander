using UnityEngine;
using UnityEditor;

public class FiringZoneGizmo {

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

		if (scr.target == null)
		{
			return;
		}

		scr.RecalculateFrustum();
		
		FiringZone.Face face = scr.FrustrumFace(scr.target.position);
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
			Gizmos.DrawLine(scr.transform.position, scr.target.position);
		}

	}
}
