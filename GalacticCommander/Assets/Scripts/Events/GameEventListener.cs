using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent Event;
    [SerializeField]
    private UnityEvent Raise, Lower;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
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