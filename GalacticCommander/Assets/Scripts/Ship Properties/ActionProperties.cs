using System;
using UnityEngine;

public abstract class ActionProperties : StatPropertyObject
{
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private int cost;
    public int Cost => cost;

    [SerializeField]
    private int turnCooldown;
    [NonSerialized]
    public int TurnCooldown;

    private GameEvent endRound;

    protected void OnEnable() {
        ResourceRequest req = Resources.LoadAsync("Events/EndRound", typeof(GameEvent));
        endRound = req.asset as GameEvent;
        endRound.RegisterListener(Cooldown);
    }
    
    public void Used() {
        TurnCooldown = turnCooldown;
    }
    public void Cooldown() {
        TurnCooldown = Mathf.Max(TurnCooldown - 1, 0);
        Debug.Log(TurnCooldown);
    }
}