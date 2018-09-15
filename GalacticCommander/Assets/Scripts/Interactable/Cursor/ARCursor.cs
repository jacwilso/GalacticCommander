using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
//using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARCursor : MonoBehaviour
{
    public static ARCursor Instance => instance;

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

    public Vector2 InputPosition
    {
        get { return screenPos; }
    }

    public event Action DeselectEvent, SelectEvent;

    private static ARCursor instance;

    private Camera cam;
    private IInteractable selected;
    private PointerEventData pointerData;
    private EventSystem eventSys;
    private Vector2 screenPos;

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
            screenPos = Input.mousePosition;
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            screenPos = Input.GetTouch(0).position;
#endif
            IInteractable newSelected = null;
            IInteractable oldSelected = selected;

            RaycastHit hit;
            pointerData.position = screenPos;
            List<RaycastResult> results = new List<RaycastResult>();
            eventSys.RaycastAll(pointerData, results);
            if (results.Count > 0)
            {
                newSelected = results[0].gameObject.GetComponent<IInteractable>();
                newSelected?.Select();
                return;
            }
            else if (Physics.Raycast(cam.ScreenPointToRay(screenPos), out hit))
            {
                newSelected = hit.transform.GetComponent<IInteractable>();
            }

            Deselect();
            if (newSelected != null && newSelected != oldSelected)
            {
                selected = newSelected;
                newSelected?.Select();
                SelectEvent?.Invoke();
            }
        }
    }

    public void Deselect()
    {
        DeselectEvent?.Invoke();
        selected?.Deselect();
        selected = null;
    }
}