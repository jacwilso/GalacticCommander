using UnityEngine;

[System.Serializable]
public class Vector3Bool
{
    public bool this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return x;
                case 1: return y;
                case 2: return z;
                default:
                    Debug.LogError("Vector3Bool::[] out of bounds");
                    return false;
            }
        }
    }
    public bool x, y, z;
}
