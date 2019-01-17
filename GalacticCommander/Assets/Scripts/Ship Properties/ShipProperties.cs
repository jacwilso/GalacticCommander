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
    int health;
    [NonSerialized]
    public Stat Health;

    [SerializeField]
    int armor;
    [NonSerialized]
    public Stat Armor;

    [SerializeField]
    [Range(0, 100)]
    int evasion;
    [NonSerialized]
    public Stat Evasion;

    [SerializeField]
    int actionPoints;
    [NonSerialized]
    public Stat ActionPoints;

    [SerializeField]
    int initiative;
    [NonSerialized]
    public Stat Initiative;

    [Header("Shield Stats")]
    [SerializeField]
    int shieldStrength;
    [NonSerialized]
    public Stat ShieldStrength;

    [SerializeField]
    int shieldRegen;
    [NonSerialized]
    public Stat ShieldRegen;

    [Header("Actions")]
    public MovementProperties movement;
    public AbilityProperties[] network;
    public AttackProperties[] weapons;
    public AbilityProperties[] engines;
    public AbilityProperties[] structure;
    public AbilityProperties[] energy;
    public AbilityProperties[] personnel;
    ActionProperties[] actions;

    public ShipTypeModifier modifier;

    [Header("Other")]
    [SerializeField]
    Sprite icon;
    public Sprite Icon => icon;
    [SerializeField]
    ParticleSystem explosion;
    public ParticleSystem Explosion => explosion;


    // NON SERIALIZED
    [NonSerialized]
    public Stat accuracy = new Stat(0),
        damage = new Stat(0);
    [NonSerialized]
    public AttackProperties activeWeapon;

    public void CreateInstance()
    {
        movement = ScriptableObject.Instantiate(movement);
        actions = GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[])
            .SelectMany(param => param.GetValue(this) as ActionProperties[]).ToArray();
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null)
            {
                actions[i] = ScriptableObject.Instantiate(actions[i]);
            }
        }
    }

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
        /*
        GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[]).ToList()
            .ForEach(param =>
            {
                ActionProperties[] actions = param.GetValue(this) as ActionProperties[];
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] != null)
                    {
                        Debug.Log(actions[i].TurnCooldown);
                        availableActions.Add(actions[i].Cost <= currentAP && actions[i].TurnCooldown == 0);
                    }
                }
            });
            */
        for (int i = 0; i < actions.Length; i++)
        {
            if (actions[i] != null)
            {
                availableActions.Add(actions[i].Cost <= currentAP && actions[i].TurnCooldown == 0);
            }
        }
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
        return actions[option - 1];
        /*return GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[])
            .SelectMany(param => param.GetValue(this) as ActionProperties[]).ToArray()[option - 1];*/
    }
}
