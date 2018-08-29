using UnityEngine;

public abstract class AbilityProperties : ActionProperties
{
    [SerializeField]
    private int turnCooldown;
    public int TurnCooldown => turnCooldown;

	[SerializeField]
	private TargetType target;
	public TargetType Target => target;

    public abstract void Ability(Ship target);

	public enum TargetType
	{
		Self, Ally, Enemy
	}
}