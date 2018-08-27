using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(FiringZone))]
public class DebugFiringZone : MonoBehaviour {

    [SerializeField]
    private Transform test;

    private FiringZone zone;
    private FiringZone.Face face;

    private void OnEnable()
    {
        zone = GetComponent<FiringZone>();
    }

    private void Update()
    {
        if (test == null || zone == null)
            return;

        face = zone.FrustrumFace(test.position);
        Debug.Log(face);
    }

    //private void OnDrawGizmosSelected()
    private void OnDrawGizmos()
    {
        if (zone == null)
            return;

        Gizmos.DrawWireCube(transform.position, zone.InnerScale);
        Gizmos.DrawWireCube(transform.position, zone.OuterScale);
        for (int i = 0; i < 8; i++)
        {
            Vector3 a = transform.position + 0.5f * Vector3.Scale(zone.InnerScale, FiringZone.cubeVerticies[i]);
            Vector3 b = transform.position + 0.5f * Vector3.Scale(zone.OuterScale, FiringZone.cubeVerticies[i]);
            Gizmos.DrawLine(a, b);
        }

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
            for (int i = 0; i <= 4; i++)
            {
                int indx = 4 * (int)face;
                Vector3 a = transform.position + 0.5f * Vector3.Scale(zone.OuterScale, FiringZone.cubeVerticies[indx++]);
                Vector3 b = transform.position + 0.5f * Vector3.Scale(zone.OuterScale, FiringZone.cubeVerticies[indx % 4]);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(a, b);
            }
        }

        if (test == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, test.position);
    }
}