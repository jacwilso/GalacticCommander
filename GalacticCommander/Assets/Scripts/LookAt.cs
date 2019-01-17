using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    string tag;
    [SerializeField]
    bool enabledInitially = true;
    [SerializeField]
    bool inverse;
    [SerializeField]
    Vector3Bool lockAxis;

    bool isEnabled;

    void Start()
    {
        if (target == null && tag != string.Empty)
            target = GameObject.FindGameObjectWithTag(tag).transform;
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
