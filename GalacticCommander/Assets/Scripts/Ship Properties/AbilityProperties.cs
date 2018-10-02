using System;
using UnityEngine;

public abstract class AbilityProperties : ActionProperties
{
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
    public enum TargetType : int
    {
        Self = (1 << 0),
        SelfAOE = (1 << 1), // you + allies
        Ally = (1 << 2),
        AllyAOE = (1 << 3),
        Enemy = (1 << 4),
        EnemyAOE = (1 << 5),
        All = ~0
    }
}