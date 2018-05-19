using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship")]
public class ShipProperties : ScriptableObject
{
    [Header("Ship")]
    [SerializeField]
    private int health;
    [NonSerialized]
    public ModifiableStat<int> Health;

    [SerializeField]
    private int armor;
    [NonSerialized]
    public ModifiableStat<int> Armor;

    [SerializeField]
    [Range(0, 100)]
    private int evasion;
    [NonSerialized]
    public ModifiableStat<int> Evasion;

    [SerializeField]
    private int energy;
    public int Energy => energy;

    [Header("Shield")]
    [SerializeField]
    private int shieldStrength;
    [NonSerialized]
    public ModifiableStat<int> ShieldStrength;

    [SerializeField]
    private int shieldRegen;
    [NonSerialized]
    public ModifiableStat<int> ShieldRegen;

    [Header("Actions")]
    public MovementProperties movement;
    public List<AttackProperties> attacks;
    public List<AbilityProperties> abilities;

    public void OnAfterDeserialize()
    {
        Health = new ModifiableStat<int>(health);
        Armor = new ModifiableStat<int>(armor);
        Evasion = new ModifiableStat<int>(evasion);
        ShieldStrength = new ModifiableStat<int>(shieldStrength);
        ShieldRegen = new ModifiableStat<int>(shieldRegen);
    }
}
