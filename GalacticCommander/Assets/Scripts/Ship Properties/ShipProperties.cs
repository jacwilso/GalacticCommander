using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public struct ShipDefaults
{
    public static readonly int BASE_AP = 4,
        MAX_AP = 8;
}

[CreateAssetMenu(menuName = "Ship Properties/Ship")]
public class ShipProperties : StatPropertyObject
{
    [Header("Hull")]
    [SerializeField]
    int hull = 0;
    [NonSerialized, AssignableStat]
    public Stat Hull;

    [SerializeField]
    int hullRegen;
    [NonSerialized, AssignableStat]
    public int HullRegen;

    [Header("Shield")]
    [SerializeField]
    int shield = 0;
    [NonSerialized, AssignableStat]
    public Stat Shield;

    [SerializeField]
    int shieldRegen = 0;
    [NonSerialized, AssignableStat]
    public Stat ShieldRegen;

    [Header("General Stats")]
    [SerializeField]
    [Range(0, 100)]
    int evasion = 0;
    [NonSerialized, AssignableStat]
    public Stat Evasion;

    [SerializeField]
    float speed = 0;
    [NonSerialized, AssignableStat]
    public Stat Speed;

    [SerializeField]
    int apRegen = ShipDefaults.BASE_AP;
    public int RegenAP => apRegen;

    [SerializeField]
    int initiative = 0;
    public int Initiative => initiative;

    [SerializeField]
    DamageProfile profile;
    public DamageProfile Profile => profile;

    [Header("Audio")]
    [SerializeField]
    AudioClip hullSfx;
    [SerializeField]
    AudioClip shieldSfx;

    [Header("Other")]
    [SerializeField]
    Sprite icon = null;
    public Sprite Icon => icon;
    [SerializeField, TextArea]
    string description;
}
