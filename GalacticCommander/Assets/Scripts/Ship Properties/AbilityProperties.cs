using UnityEngine;

public abstract class AbilityProperties : ActionProperties
{
    [SerializeField]
    private int turnCooldown;
    public int TurnCooldown => turnCooldown;

    public abstract void Ability(Ship target);
}