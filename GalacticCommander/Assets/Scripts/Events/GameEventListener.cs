using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    GameEvent Event;
    [SerializeField]
    UnityEvent Raise, Lower;

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