using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityProperties : ActionProperties
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private int turnCooldown;
    public int TurnCooldown => turnCooldown;

    public abstract void Ability(Ship target);
}