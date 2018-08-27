using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARCursor : MonoBehaviour
{
    public static ARCursor Instance
    {
        get { return instance; }
    }

    public IInteractable Selected
    {
        get { return selected; }
        set
        {
            Deselect();
            selected = value;
            selected.Select();
        }
    }

    private static ARCursor instance;

    private Camera cam;
    private IInteractable selected;
    private PointerEventData pointerData;
    private EventSystem eventSys;

    Vector3 pos, dir;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple cursors.");
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        eventSys = EventSystem.current;
        pointerData = new PointerEventData(null);
    }

    private void Update()
    {
#if UNITY_EDITOR && !INSTANT_PREVIEW
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = Input.mousePosition;
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 screenPos = Input.GetTouch(0).position;
#endif
            pointerData.position = screenPos;
            List<RaycastResult> results = new List<RaycastResult>();
            eventSys.RaycastAll(pointerData, results);
            if (results.Count > 0)
                return;

            IInteractable oldSelection = selected;
            Deselect();

            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(screenPos), out hit))
            {
                selected = hit.transform.GetComponent<IInteractable>();
                if (oldSelection != null && oldSelection == selected)
                    Deselect();
                else
                    selected?.Select();
            }
        }
    }

    public void Deselect()
    {
        selected?.Deselect();
        selected = null;
    }
}