using System;
using UnityEngine;

public abstract class ActionProperties : StatPropertyObject
{
    [SerializeField]
    Sprite icon = null;
    public Sprite Icon => icon;

    [SerializeField]
    int cost = 0;
    public int apCost => cost;

    [SerializeField]
    int turnCooldown = 0;
    public int TurnCooldown => turnCooldown;

    int currentCooldown;
    public int CurrentCooldown => currentCooldown;

    GameEvent endRound;

    protected void OnEnable()
    {
        ResourceRequest req = Resources.LoadAsync("Events/EndRound", typeof(GameEvent));
        endRound = req.asset as GameEvent;
        endRound.RegisterListener(Cooldown);
    }

    public void Used()
    {
        currentCooldown = turnCooldown;
    }
    public void Cooldown()
    {
        currentCooldown = Mathf.Max(currentCooldown - 1, 0);
        Debug.Log(currentCooldown);
    }
}