using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : MonoBehaviour, IInteractable
{
    private bool selected;
    private Plane planeOffset;
    private Vector3 camPos;
    private Quaternion camRot;
    private Transform cam;

    private void Start ()
    {
        cam = Camera.main.transform;
	}

    private void Update()
    {
        if (selected)
        {
            float dist = Vector3.Dot(cam.position - transform.position, planeOffset.normal);
            Vector3 offset = (cam.position - camPos);
            transform.position += (dist - planeOffset.distance) * planeOffset.normal;
            transform.position += offset - Vector3.Dot(offset, planeOffset.normal) * planeOffset.normal;
            camPos = cam.position;
        }
    }

    public void Select()
    {
        selected = true;
        Vector3 normal = (cam.position - transform.position).normalized;
        float dist = Vector3.Distance(cam.position, transform.position);
        planeOffset = new Plane(normal, dist);
        camPos = cam.position;
    }
    
    public void Deselect()
    {
        selected = false;
    }
}
