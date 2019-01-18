using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    GameEvent Event = null;
    [SerializeField]
    UnityEvent Raise = null,
        Lower = null;

    void OnEnable()
    {
        Event.RegisterListener(this);
    }

    void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Raise.Invoke();
    }

    public void OnEventLowered()
    {
        Lower.Invoke();
    }
}