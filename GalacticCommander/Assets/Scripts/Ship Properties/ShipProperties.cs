using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

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
    public AbilityProperties[] energy;
    public AbilityProperties[] personnel;

    public ShipTypeModifier modifier;

    [Header("Other")]
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField]
    private ParticleSystem explosion;
    public ParticleSystem Explosion => explosion;


    // NON SERIALIZED
    [NonSerialized]
    public Stat accuracy = new Stat(0),
        damage = new Stat(0);
    [NonSerialized]
    public AttackProperties activeWeapon;

    // Functions

    public List<Sprite> GetIcons()
    {
        List<Sprite> icons = new List<Sprite>
        {
            movement.Icon
        };

        GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[]).ToList()
            .ForEach(param =>
            {
                ActionProperties[] actions = param.GetValue(this) as ActionProperties[];
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                    {
                        icons.Add(actions[i].Icon);
                    }
                }
            });
        return icons;
    }

    public List<bool> AvailableActions(int currentAP)
    {
        List<bool> availableActions = new List<bool>
        {
            movement.Cost <= currentAP
        };

        GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[]).ToList()
            .ForEach(param =>
            {
                ActionProperties[] actions = param.GetValue(this) as ActionProperties[];
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                    {
                        availableActions.Add(actions[i].Cost <= currentAP);
                    }
                }
            });
        return availableActions;
    }

    public List<AbilityProperties> GetAbilities()
    {
        return GetType().GetFields()
                   .Where(field => field.GetValue(this) is AbilityProperties[])
                   .SelectMany(param => param.GetValue(this) as AbilityProperties[]).ToList();
    }

    public ActionProperties GetAction(int option)
    {
        if (option == 0)
        {
            return movement;
        }
        return GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[])
            .SelectMany(param => param.GetValue(this) as ActionProperties[]).ToArray()[option - 1];
    }
}
