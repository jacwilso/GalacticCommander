using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShip : MonoBehaviour, IInteractable
{
    [SerializeField]
    Transform handle = null;
    [SerializeField]
    Vector3 handleOffset;

    Ship ship;
    bool selected;
    Camera cam;
    Vector3 toHandle;

    void Start()
    {
        ship = GetComponentInParent<Ship>();

        cam = Camera.main;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        toHandle = handle.position - transform.position;

        if (ARCursor.Instance == null || ARCursor.Instance.Selected != this)
            gameObject.SetActive(false);
    }

    void Update()
    {
        if (selected)
        {
            float distSq = Vector3.SqrMagnitude(transform.parent.position - transform.position);
            float speedSq = ship.properties.Speed.Value;
            speedSq *= speedSq;
            int cost = Mathf.CeilToInt(distSq / speedSq);
            PlayerShip player = (PlayerShip)ship;
            if (cost <= player.CurrentAP)
            {
                transform.rotation = cam.transform.rotation;
                Vector3 handlePos = cam.transform.position + transform.forward * (cam.nearClipPlane + 0.01f);

                Vector3 toHandleWorld = handle.transform.TransformDirection(toHandle);
                transform.position = handlePos - toHandleWorld;
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
        float speedSq = ship.properties.Speed.Value;
        speedSq *= speedSq;
        int cost = Mathf.CeilToInt(distSq / speedSq);
        PlayerShip player = (PlayerShip)ship;
        if (cost > player.CurrentAP)
        {
            ConfirmationUI.Instance.Activate(false, true);
        }
        else if (ConfirmationUI.Instance.gameObject.activeSelf)
        {
            player.CacheAP = cost;
            ConfirmationUI.Instance.Activate(true);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false); // TODO remove
        transform.localPosition = Vector3.zero;
        handle.localPosition = toHandle;
        transform.localRotation = Quaternion.identity;
    }
}
