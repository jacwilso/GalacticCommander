﻿using System;
using UnityEngine;

public abstract class ActionProperties : StatPropertyObject
{
    [SerializeField]
    Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    int cost;
    public int Cost => cost;

    [SerializeField]
    int turnCooldown;
    [NonSerialized]
    public int TurnCooldown;

    GameEvent endRound;

    protected void OnEnable()
    {
        ResourceRequest req = Resources.LoadAsync("Events/EndRound", typeof(GameEvent));
        endRound = req.asset as GameEvent;
        endRound.RegisterListener(Cooldown);
    }

    public void Used()
    {
        TurnCooldown = turnCooldown;
    }
    public void Cooldown()
    {
        TurnCooldown = Mathf.Max(TurnCooldown - 1, 0);
        Debug.Log(TurnCooldown);
    }
}