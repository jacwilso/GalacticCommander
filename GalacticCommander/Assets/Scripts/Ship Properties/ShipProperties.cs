using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship")]
public class ShipProperties : ScriptableObject
{
    [Header("Ship")]
    [SerializeField]
    private int health;
    public int Health => health;
    [SerializeField]
    private int armor;
    public int Armor => armor;
    [SerializeField]
    [Range(0, 100)]
    private int evasion;
    public int Evasion => evasion;
    [SerializeField]
    private int energy;
    public int Energy => energy;
    //[SerializeField]
    //private float speed;
    //public float Speed => speed;
    [Header("Shield")]
    [SerializeField]
    private int shieldStrength;
    public int ShieldStrength => shieldStrength;
    [SerializeField]
    private int shieldRegen;
    public int ShieldRegen => shieldRegen;
    [Header("Actions")]
    public MovementProperties movement;
    public List<AttackProperties> attacks;
    public List<AbilityProperties> abilities;
}
