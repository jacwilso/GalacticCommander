using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship")]
public class ShipProperties : StatPropertyObject
{
    [Header("General Stats")]
    [SerializeField]
    private int health;
    [NonSerialized]
    public Stat Health;

    [SerializeField]
    private int armor;
    [NonSerialized]
    public Stat Armor;

    [SerializeField]
    [Range(0, 100)]
    private int evasion;
    [NonSerialized]
    public Stat Evasion;

    [SerializeField]
    private int actionPoints;
    [NonSerialized]
    public Stat ActionPoints;

    [SerializeField]
    private int initiative;
    [NonSerialized]
    public Stat Initiative;

    [Header("Shield Stats")]
    [SerializeField]
    private int shieldStrength;
    [NonSerialized]
    public Stat ShieldStrength;

    [SerializeField]
    private int shieldRegen;
    public int ShieldRegen => shieldRegen;

    [Header("Actions")]
    public MovementProperties movement;
	public AbilityProperties[] network;
    public AttackProperties[] weapons;
    public AbilityProperties[] engines;
	public AbilityProperties[] structure;
	public AbilityProperties[] personnel;

	[Header("Other")]
    private Sprite icon;
    public Sprite Icon => icon;
}
