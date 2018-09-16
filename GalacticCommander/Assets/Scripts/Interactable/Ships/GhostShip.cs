using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform handle;
    [SerializeField]
    private Vector3 handleOffset;

    private Ship ship;
    private bool selected;
    private Camera cam;
    private Vector3 toHandle;

    private void Start()
    {
        ship = GetComponentInParent<Ship>();
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

            float distSq = Vector3.SqrMagnitude(transform.parent.position - transform.position);
            float speedSq = ship.properties.movement.Speed.Value;
            speedSq *= speedSq;
            int cost = Mathf.CeilToInt(distSq / speedSq);
            PlayerShip player = (PlayerShip)ship;
            if (cost > player.TurnAP)
            {
                Debug.Log("STOP");
                //TODO
            }
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
        float distSq = Vector3.SqrMagnitude(transform.parent.position - transform.position);
        float speedSq = ship.properties.movement.Speed.Value;
        speedSq *= speedSq;
        int cost = Mathf.CeilToInt(distSq / speedSq);
        PlayerShip player = (PlayerShip)ship;
        if (cost > player.TurnAP)
        {
            ConfirmationUI.Instance.Activate(false, true);
        }
        else if (ConfirmationUI.Instance.gameObject.activeSelf)
        {
            player.CacheAP = cost;
            ConfirmationUI.Instance.Activate(true);
        }
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
