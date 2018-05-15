using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    private string tag;
    [SerializeField]
    private bool enabledInitially = true;

    private bool isEnabled;

    private void Start()
    {
        if (target == null && tag != string.Empty)
            target = GameObject.FindGameObjectWithTag(tag).transform;
        if (target == null)
            this.enabled = false;
        isEnabled = enabledInitially;
    }

    private void Update ()
    {
        if (isEnabled)
            transform.LookAt(target);
	}
}
