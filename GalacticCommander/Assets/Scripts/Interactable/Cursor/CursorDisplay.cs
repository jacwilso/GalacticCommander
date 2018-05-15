using UnityEngine;

[RequireComponent (typeof(ARCursor))]
public class CursorDisplay : MonoBehaviour
{
    [SerializeField]
    private int rotationSpeed = 15;

	private void Start ()
    {
        ARCursor.instance.Default += Default;
        ARCursor.instance.Hovering += Hover;
        ARCursor.instance.Selected += Default;
	}

    private void Default()
    {
        transform.rotation = Quaternion.identity;
    }
	
	private void Hover()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
