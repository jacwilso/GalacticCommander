using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class DebugCamera : MonoBehaviour {
    [SerializeField]
    GameObject arCamera;

    void Start() {
        gameObject.SetActive(false);
    }

    void Update() {
        gameObject.SetActive(true);
        arCamera.SetActive(false);
    }
}
#endif