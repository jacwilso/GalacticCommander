using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> listeners = new List<GameEventListener>();
    List<Action> actions = new List<Action>();

    public void Raise()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised();
        }
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i]();
        }
    }

    public void Lower()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventLowered();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void RegisterListener(Action action)
    {
        actions.Add(action);
    }

    public void UnregisterListener(Action action)
    {
        actions.Remove(action);
    }
}