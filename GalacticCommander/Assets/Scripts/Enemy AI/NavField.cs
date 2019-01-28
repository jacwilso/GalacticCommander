using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NavField : MonoBehaviour
{
    // struct NavNode
    // {
    //     byte cost;
    //     Vector3 dir;
    //     Transform target;
    // };

    [SerializeField]
    Vector3Int size;

    byte[] pathMtx, costMtx;
    Vector3[] dirMtx;
    // Transform[] nodeTarget;

    [SerializeField] // TODO remove
    List<Transform> targets = null;
    public List<Transform> Targets {
        get {return targets;}
        set {
            isDirty = true;
            targets = value;
        }
    }
    bool isDirty;

    static readonly Vector3Int VECTOR3INT_FORWARD = new Vector3Int(0, 0, 1);

    void OnEnable()
    {
        int len = size.x * size.y * size.z;
        pathMtx = new byte[len];
        costMtx = new byte[len];
        dirMtx = new Vector3[len];
        // nodeTarget = new Transform[len];
    }

    public void GenerateField() {
        Targets = PlayerShip.PlayerShips.ConvertAll(x => x.transform); // TODO REMOVE
        if (isDirty) {
            isDirty = false;
            GeneratePathField();
            GenerateDirections();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Field")]
    void EditorGenerateField()
    {
        if (targets == null) return;
        OnEnable();
        GeneratePathField();
        GenerateDirections();
    }

    void OnDrawGizmos()
    {
        if (pathMtx == null || pathMtx?.Length == 0 || 
            dirMtx == null || dirMtx?.Length == 0) 
            EditorGenerateField();
        int index = -1;
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    index++;
                    Vector3 center = new Vector3(x, y, z);
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(center, center + 0.25f * new Vector3(dirMtx[index].x, dirMtx[index].y, dirMtx[index].z));
                    Handles.Label(center, $"[{index}] {pathMtx[index]}");
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(center, 0.02f);
                }
            }
        }
    }
#endif

    int IndexFromPosition(Vector3Int position)
    {
        return position.z * size.x * size.y + position.y * size.x + position.x;
    }

    bool InvalidIndex(int index)
    {
        return index < 0 || index > pathMtx.Length - 1;
    }

    bool InvalidPosition(Vector3Int position)
    {
        for (int i = 0; i < 3; i++)
            if (position[i] < 0 || position[i] > size[i] - 1) return true;
        return false;
    }

    Vector3Int PositionToNavPosition(Vector3 position)
    {
        Vector3Int intPos = new Vector3Int((int)Mathf.Round(position.x),
                (int)Mathf.Round(position.y),
                (int)Mathf.Round(position.z));
        intPos.x = Mathf.Clamp(intPos.x, 0, size.x - 1);
        intPos.y = Mathf.Clamp(intPos.y, 0, size.y - 1);
        intPos.z = Mathf.Clamp(intPos.z, 0, size.z - 1);
        return intPos;
    }

    void GeneratePathField()
    {
        for (int i = 0; i < pathMtx.Length; i++)
        {
            pathMtx[i] = 255;
        }
        foreach (Transform target in targets)
        {
            Vector3Int position = PositionToNavPosition(target.position);

            int index = IndexFromPosition(position);
            pathMtx[index] = 0;

            bool[] visited = new bool[size.x * size.y * size.z];
            Queue<Vector3Int> toVisit = new Queue<Vector3Int>();

            visited[index] = true;
            toVisit.Enqueue(position);
            while (toVisit.Count > 0)
            {
                position = toVisit.Dequeue();
                index = IndexFromPosition(position);
                CalculatePathCost(visited, toVisit, index, position + Vector3Int.left);
                CalculatePathCost(visited, toVisit, index, position + Vector3Int.right);
                CalculatePathCost(visited, toVisit, index, position + Vector3Int.up);
                CalculatePathCost(visited, toVisit, index, position + Vector3Int.down);
                CalculatePathCost(visited, toVisit, index, position + VECTOR3INT_FORWARD);
                CalculatePathCost(visited, toVisit, index, position - VECTOR3INT_FORWARD);
            }
        }
    }

    void CalculatePathCost(bool[] visited, Queue<Vector3Int> toVisit, int atIndex, Vector3Int visit)
    {
        int visitIndex = IndexFromPosition(visit);
        if (InvalidPosition(visit) || InvalidIndex(visitIndex) || visited[visitIndex])
        {
            return;
        }
        pathMtx[visitIndex] = (byte)(Mathf.Min(pathMtx[atIndex] + 1, pathMtx[visitIndex]) + costMtx[visitIndex]);
        visited[visitIndex] = true;
        toVisit.Enqueue(visit);
    }

    void GenerateDirections()
    {
        int index = 0;
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    byte minCost = 255;
                    int minZeros = 4;
                    Vector3 minDir = Vector3.zero;
                    Vector3Int position = new Vector3Int(x, y, z);
                    if (pathMtx[index] != 0) // TODO this should point at the target
                    {
                        for (int i = -1; i < 2; i++)
                        {
                            int iZero = Mathf.Abs(i);
                            for (int j = -1; j < 2; j++)
                            {
                                int jZero = Mathf.Abs(j);
                                for (int k = -1; k < 2; k++)
                                {
                                    Vector3Int dir = new Vector3Int(i, j, k);
                                    byte cost = CalculateDirectionCost(position, dir);
                                    int zeros = iZero + jZero + Mathf.Abs(k);
                                    if (cost < minCost || (cost == minCost && zeros < minZeros))
                                    {
                                        minCost = cost;
                                        minDir = dir;
                                        minZeros = zeros;
                                    }
                                }
                            }
                        }
                    }
                    dirMtx[index] = minDir.normalized;
                    index++;
                }
            }
        }
    }

    byte CalculateDirectionCost(Vector3Int position, Vector3Int dir)
    {
        int index = IndexFromPosition(position + dir);
        if (InvalidPosition(position + dir) || InvalidIndex(index))
        {
            index = IndexFromPosition(position);
        }
        return pathMtx[index];
    }

    public Vector3 GetDirection(Vector3 position)
    {
        Vector3Int intPos = PositionToNavPosition(position);
        return dirMtx[IndexFromPosition(intPos)];
    }

    public byte GetPathCost(Vector3 position) {
        Vector3Int intPos = PositionToNavPosition(position);
        return pathMtx[IndexFromPosition(intPos)];
    }
}
