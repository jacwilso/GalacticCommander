using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringZone : MonoBehaviour {

    public Vector3 InnerScale => innerScale;
    public Vector3 OuterScale => outerScale;

#if UNITY_EDITOR
	public Transform target;
#endif

	[SerializeField]
    private Vector3 innerScale = Vector3.one, 
        outerScale = 3f * Vector3.one;

    private Plane[,] frustums;

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

	private int[] winding =
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

	private void OnValidate()
    {
        if (innerScale.x > outerScale.x || innerScale.y > outerScale.y || innerScale.z > outerScale.z)
            Debug.LogError("Inner scale must be smaller than outer scale.");
    }

    private void OnEnable()
    {
		RecalculateFrustum();
    }

	public void RecalculateFrustum()
	{
		if (frustums == null)
		{
			frustums = new Plane[6, 5];
		}

		Vector3 pos = transform.position;
		for (int i = 0; i < 6; i++)
		{
			int indx = i * 11;
			frustums[i, 0] = new Plane(
				pos + 0.5f * (transform.rotation * Vector3.Scale(innerScale, cubeVerticies[winding[indx]])),
				pos + 0.5f * (transform.rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]])),
				pos + 0.5f * (transform.rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]]))
			);
			for (int j = 0; j < 4; j++)
				frustums[i, j + 1] = new Plane(
					pos + 0.5f * (transform.rotation * Vector3.Scale(outerScale, cubeVerticies[winding[++indx]])),
					pos + 0.5f * (transform.rotation * Vector3.Scale(innerScale, cubeVerticies[winding[indx]])),
					pos + 0.5f * (transform.rotation * Vector3.Scale(innerScale, cubeVerticies[winding[++indx]]))
				);
		}
	}

	public Face FrustrumFace(Vector3 point)
    {
        for (int i = 0; i < 6; i++)
        {
            bool inFront = true;
            for (int j = 0; j < 5; j++)
            {
                //Debug.Log(i + ", " + j + " " + frustrums[i, j].GetSide(point));
                if (!frustums[i,j].GetSide(point))
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
        Back = 0,
        Left = 1,
        Bottom = 2,
        Front = 3,
        Top = 4,
        Right = 5,
        None,
    }
}
