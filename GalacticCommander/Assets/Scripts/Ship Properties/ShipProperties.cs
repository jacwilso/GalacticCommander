using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship")]
public class ShipProperties : StatPropertyObject
{
    [Header("Ship")]
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
    private int energy;
    public int Energy => energy;

    [Header("Shield")]
    [SerializeField]
    private int shieldStrength;
    [NonSerialized]
    public Stat ShieldStrength;

    [SerializeField]
    private int shieldRegen;
    public int ShieldRegen => shieldRegen;

    [Header("Actions")]
    public MovementProperties movement;
    public List<AttackProperties> attacks;
    public List<AbilityProperties> abilities;
}
