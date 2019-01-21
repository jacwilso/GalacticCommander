using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringZone : MonoBehaviour
{

    public Vector3 InnerScale => innerScale;
    public Vector3 OuterScale => outerScale;

    // [SerializeField]
    [MinValue(0.1f)]
    public Vector3 innerScale = Vector3.one,
        outerScale = 3f * Vector3.one;

    Plane[,] frustums;

    public static Vector3[] cubeVerticies =
    {
        new Vector3(-1, -1, -1),
        new Vector3(-1, -1, 1),
        new Vector3(-1, 1, -1),
        new Vector3(-1, 1, 1),
        new Vector3(1, -1, -1),
        new Vector3(1, -1, 1),
        new Vector3(1, 1, -1),
        new Vector3(1, 1, 1),
    };

    int[] winding =
   {
        // back
        6, 4, 0,
        2, 0,
        6, 2,
        4, 6,
        1, 4, 
        // left
        3, 2, 0,
        3, 1,
        1, 0,
        0, 2,
        2, 3, 
        // bottom
        5, 1, 0,
        4, 0,
        1, 5,
        0, 1,
        5, 4, 
        // front
        7, 3, 1,
        1, 3,
        7, 5,
        3, 7,
        5, 1, 
        // top
        7, 6, 2,
        3, 2,
        2, 6,
        6, 7,
        7, 3, 
        // right
        7, 5, 4,
        6, 4,
        7, 6,
        5, 7,
        4, 5,
    };

    void OnValidate()
    {
        if (innerScale.x > outerScale.x || innerScale.y > outerScale.y || innerScale.z > outerScale.z)
            Debug.LogError("Inner scale must be smaller than outer scale.");
    }

    void OnEnable()
    {
        RecalculateFrustum(transform.position, transform.rotation);
    }

    public void RecalculateFrustum(Vector3 position, Quaternion rotation)
    {
        if (frustums == null)
        {
            frustums = new Plane[6, 5];
        }

        for (int i = 0; i < 6; i++)
        {
            int indx = i * 11;
            frustums[i, 0] = new Plane(
                position + 0.5f * (rotation * Vector3.Scale(innerScale, cubeVerticies[winding[indx]])),
                position + 0.5f * (rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]])),
                position + 0.5f * (rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]]))
            );
            for (int j = 0; j < 4; j++)
                frustums[i, j + 1] = new Plane(
                    position + 0.5f * (rotation * Vector3.Scale(outerScale, cubeVerticies[winding[++indx]])),
                    position + 0.5f * (rotation * Vector3.Scale(innerScale, cubeVerticies[winding[indx]])),
                    position + 0.5f * (rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]]))
                );
        }
    }

    public Face FrustrumFace(Vector3 target)
    {
        for (int i = 0; i < 6; i++)
        {
            bool inFront = true;
            for (int j = 0; j < 5; j++)
            {
                //Debug.Log(i + ", " + j + " " + frustrums[i, j].GetSide(point));
                if (!frustums[i, j].GetSide(target))
                {
                    inFront = false;
                    break;
                }
            }
            if (inFront)
            {
                return (Face)i;
            }
        }
        return Face.None;
    }

    public enum Face
    {
        None = -1,
        Back = 0,
        Left = 1,
        Bottom = 2,
        Front = 3,
        Top = 4,
        Right = 5,
    }
}
