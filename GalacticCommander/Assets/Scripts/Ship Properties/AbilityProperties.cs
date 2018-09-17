using System;
using UnityEngine;

public abstract class AbilityProperties : ActionProperties
{
    [SerializeField]
    private int turnCooldown;
    public int TurnCooldown => turnCooldown;

    [SerializeField]
    private int requirement;
    public int Requirement => requirement;

    [SerializeField]
    private int range;
    public int Range => range;

    [SerializeField]
    private TargetType target;
    public TargetType Target => target;

    private int turnsTillUse;
    public bool Ready => turnsTillUse == 0;

    [SerializeField]
    private int aiPriority;
    public int AIPriority => aiPriority;

    public abstract void Ability(Ship target);

    [Flags]
    public enum TargetType
    {
        Self = (0 << 1),
        SelfAOE = (0 << 2), // you + allies
        Ally = (0 << 3),
        AllyAOE = (0 << 4),
        Enemy = (0 << 5),
        EnemyAOE = (0 << 6),
        All = ~0
    }
}