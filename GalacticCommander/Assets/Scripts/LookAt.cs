using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    string targetTag = null;
    [SerializeField]
    bool enabledInitially = true;
    [SerializeField]
    bool inverse = false;
    [SerializeField]
    Vector3Bool lockAxis;

    bool isEnabled;

    void Start()
    {
        if (target == null && targetTag != string.Empty)
            target = GameObject.FindGameObjectWithTag(targetTag).transform;
        if (target == null)
        {
            this.enabled = false;
            Debug.LogWarning(gameObject.name + " LookAt disabled");
        }
        isEnabled = enabledInitially;
    }

    void Update()
    {
        Vector3 position = transform.position;
        //for (int i = 0; i < 3; i++)
        //{
        //    if (lockAxis[i])
        //    {
        //        position[i] = target.transform.position[i];
        //    }
        //}
        if (isEnabled)
        {
            transform.rotation = Quaternion.LookRotation((inverse ? -1 : 1) * (target.transform.position - position));
        }
    }
}
