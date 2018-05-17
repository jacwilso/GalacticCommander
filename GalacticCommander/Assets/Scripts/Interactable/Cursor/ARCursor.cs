using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARCursor : MonoBehaviour
{
    public static ARCursor instance;

    private enum State
    {
        Default,
        Hovering,
        Selected
    }

    public delegate void CursorDelegate();
    public CursorDelegate Default, Hovering, Selected;

    public IInteractable Interact
    {
        get { return interact; }
    }

    private Transform cam;
    private State state;
    private IInteractable interact;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple cursors.");
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;
        if (state != State.Selected
            && Physics.Raycast(new Ray(cam.position, cam.forward), out hit))
        {
            interact = hit.transform.GetComponent<IInteractable>();
            if (interact != null)
            {
                state = State.Hovering;
                Hovering?.Invoke();
            }
        } else if (state == State.Hovering)
        {
            state = State.Default;
            Default?.Invoke();
        }
        InputUpdate();
    }

    private void InputUpdate()
    {
#if UNITY_EDITOR && !INSTANT_PREVIEW
        if (Input.GetMouseButtonDown(0))
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
        {
            if (state == State.Hovering)
            {
                state = State.Selected;
                Selected?.Invoke();
                interact.Select();
            } else if (state == State.Selected)
            {
                Deselect();
            }
        }
    }

    public void Deselect()
    {
        state = State.Default;
        interact?.Deselect();
    }
}