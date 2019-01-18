using System;
using UnityEngine;

public abstract class AbilityProperties : ActionProperties
{
    [SerializeField]
    int requirement = 0;
    public int Requirement => requirement;

    [SerializeField]
    int range = 0;
    public int Range => range;

    [SerializeField]
    TargetType target = TargetType.All;
    public TargetType Target => target;

    int turnsTillUse = 0;
    public bool Ready => turnsTillUse == 0;

    [SerializeField]
    int aiPriority = 0;
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