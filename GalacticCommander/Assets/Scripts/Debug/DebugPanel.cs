using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
#if _DEBUG
    public static DebugPanel Instance => instance;

    static DebugPanel instance;

    public bool alwaysHit;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("DebugPanel::Awake Singleton.");
        instance = this;
    }
#endif
}
