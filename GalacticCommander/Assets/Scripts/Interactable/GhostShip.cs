using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private Vector3 handleOffset;

    private bool selected;
    private Plane planeOffset;
    private Vector3 camPos;
    private Quaternion camRot;
    private Camera cam;
    private Vector3 toHandle;

    private void Start ()
    {
        cam = Camera.main;
        toHandle = handle.position - transform.position;
    }

    private void Update()
    {
        if (selected)
        {
            transform.rotation = cam.transform.rotation;
            Vector3 handlePos = cam.transform.position + transform.forward * (cam.nearClipPlane + 0.01f);

            Vector3 toHandleWorld = handle.transform.TransformDirection(toHandle);
            transform.position = handlePos - toHandleWorld;
        }
    }

    public void Select()
    {
        selected = true;
        gameObject.SetActive(true);
    }
    
    public void Deselect()
    {
        selected = false;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        handle.localPosition = toHandle;
        transform.localRotation = Quaternion.identity;
    }
}
