using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                HoverInput();
            }
        } else if (state == State.Hovering)
        {
            state = State.Default;
            Default?.Invoke();
        }
    }

    private void HoverInput()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
#else
        if (true) // Mobile
#endif
        {
            state = State.Selected;
            Selected?.Invoke();
            interact.Interact();
            // object selected
        }
    }
}