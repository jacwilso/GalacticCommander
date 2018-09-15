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
    private Camera cam;
    private Vector3 toHandle;

    private void Start()
    {
        cam = Camera.main;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

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
        ConfirmationUI.Instance.Activate(false);
    }

    public void Deselect()
    {
        selected = false;
        ConfirmationUI.Instance.Activate(true);
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        handle.localPosition = toHandle;
        transform.localRotation = Quaternion.identity;
    }
}
