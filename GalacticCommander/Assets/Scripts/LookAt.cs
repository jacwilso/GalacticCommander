using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    private string tag;
    [SerializeField]
    private bool enabledInitially = true;
    [SerializeField]
    private bool inverse;
    [SerializeField]
    private Vector3Bool lockAxis;

    private bool isEnabled;

    private void Start()
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

    private void Update()
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
