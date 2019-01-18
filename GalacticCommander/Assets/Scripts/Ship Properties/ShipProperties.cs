using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

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
    int actionPoints = 0;
    public int ActionPoints => actionPoints;

    [SerializeField]
    int initiative = 0;
    public int Initiative => initiative;

    [Header("Actions")]
    public MovementProperties movement;
    public AttackProperties[] weapons;
    // public AbilityProperties[] abilities;
    ActionProperties[] actions;

    public DamageProfile profile;

    [Header("Other")]
    [SerializeField]
    Sprite icon = null;
    public Sprite Icon => icon;
    [SerializeField]
    ParticleSystem explosion = null;
    public ParticleSystem Explosion => explosion;

    [NonSerialized]
    public AttackProperties active;
    [NonSerialized]
    public Stat accuracy, damage;

    public void Init()
    {
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
