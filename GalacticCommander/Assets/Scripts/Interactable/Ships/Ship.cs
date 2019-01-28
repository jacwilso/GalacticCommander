using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Ship : MonoBehaviour, IDamageable, IInteractable
{
    #region Events
    public event Action DeathEvent;
    public event Action DamageEvent;
    public event Action StartTurnEvent;
    public event Action EndTurnEvent;

    protected GameEvent PlayerAttackEvent;
    #endregion

    public ShipProperties properties;

    [SerializeField]
    protected GhostShip ghost;
    public GhostShip Ghost => ghost;

    protected FiringZone zone;
    public FiringZone Zone => zone;

    [SerializeField]
    WeaponProperties[] weapons;
    public WeaponProperties[] Weapons => weapons;
    protected ActionProperties[] actions;

    Stat accuracy = new Stat(0), 
        damage = new Stat(0);

    protected int currentAP;
    public int CurrentAP => currentAP;

    public float cachedAccuracy;
    public WeaponDamageRange cachedDamage;

    protected virtual void Awake()
    {
        ResourceRequest req = Resources.LoadAsync("Events/PlayerAttackEvent", typeof(GameEvent));
        PlayerAttackEvent = req.asset as GameEvent;
    }

    protected virtual void Start()
    {
        properties = ShipProperties.Instantiate(properties);
        zone = GetComponent<FiringZone>();
        TurnOrder.Instance.Subscribe(this);

        actions = new ActionProperties[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = WeaponProperties.Instantiate(weapons[i]);
            actions[i] = weapons[i];
        }
    }

    #region Damage
    public void UseWeapon(WeaponProperties weapon, Ship target) {
        weapon.Used();
        currentAP -= weapon.apCost;
        target.GetAttacked();
    }

    public void GetAttacked() // TODO I hate this name
    {
#if _DEBUG
        if (DebugPanel.Instance.alwaysHit || UnityEngine.Random.Range(0, 1) < cachedAccuracy)
#else
        if (UnityEngine.Random.Range(0, 1) < cachedAccuracy)
#endif
        {
            var assailant = TurnOrder.Instance.Current.gameObject;
            GotHitParams hit = new GotHitParams(
                (int)UnityEngine.Random.Range(cachedDamage.shieldRange.x, cachedDamage.shieldRange.y),
                (int)UnityEngine.Random.Range(cachedDamage.hullRange.x, cachedDamage.hullRange.y),
                assailant.transform.position - transform.position,
                assailant
            );
            Damaged(hit);
        }
        else { Debug.Log("Miss"); }
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        // Debug.Log(hit.damage + " " + properties.Hull.Value + " " + properties.Shield.Value);
        properties.Shield.Value -= hit.shieldDamage;
        properties.Shield.Value = Mathf.Max(0, properties.Shield.Value);
        // Debug.Log("hull -- " + properties.Hull.Value + " " + hit.hullDamage);
        properties.Hull.Value -= hit.hullDamage;
        properties.Hull.Value = Mathf.Max(0, properties.Hull.Value);
        // Debug.Log("hull -- " + properties.Hull.Value);

        Debug.Log($"Hull: {properties.Hull.Value} Shield: {properties.Shield.Value}");
        DamageEvent?.Invoke();
        if (properties.Hull.Value == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Dead
        DeathEvent?.Invoke();
        TurnOrder.Instance.Unsubscribe(this);
        // Instantiate<ParticleSystem>(properties.Explosion, transform);
    }

    public WeaponDamageRange CalculateDamageRange(WeaponProperties weapon, float accuracy, FiringZone.Face face, float targetShield, float priorShieldDamage = 0)
    {
        WeaponDamageRange damageRange = new WeaponDamageRange();
        damage.AddModifier(properties.Profile[(int)face]);
        // target has a shield - need to factor in each weapon types effectiveness
        if (targetShield - priorShieldDamage > 0)
        {
            // a list going from most effective to least effective against shields
            foreach (var shieldPriority in DamageTypeEffect.SHIELD_PRIORITY_LIST)
            {
                Vector2Int range = weapon.Damage[shieldPriority];
                // TODO add modifiers of effect
                // THIS IS PROBLEM AREA
                // TODO doing targetDamage.shieldRange.x is being conservative
                // doing targetDamage.shieldRange.y would be greedy
                if (damageRange.shieldRange.x >= targetShield - priorShieldDamage)
                {
                    damage.BaseValue = range.x;
                    damageRange.hullRange.x += damage.Value * DamageTypeEffect.HullEffect(shieldPriority);
                    damage.BaseValue = range.y;
                    damageRange.hullRange.y += damage.Value * DamageTypeEffect.HullEffect(shieldPriority);
                }
                // else if (damageRange.shieldRange.y >= targetShield - targetDamage.shieldRange.x)
                // {
                //     damage.BaseValue = range.x;
                //     damageRange.shieldRange.x += damage.Value;
                //     damage.BaseValue = range.y;
                //     damageRange.hullRange.y += damage.Value;
                // }
                else
                {
                    damage.BaseValue = range.x;
                    damageRange.shieldRange.x += damage.Value * DamageTypeEffect.ShieldEffect(shieldPriority);
                    damage.BaseValue = range.y;
                    damageRange.shieldRange.y += damage.Value * DamageTypeEffect.ShieldEffect(shieldPriority);
                }
            }
            damageRange *= accuracy;
            damageRange.shieldRange.x = Mathf.Clamp(damageRange.shieldRange.x, 0, targetShield);
            damageRange.shieldRange.y = Mathf.Clamp(damageRange.shieldRange.y, 0, targetShield);
        }
        else
        {
            // if the target has no shield it is simply hull damage
            // hull damage of a weapon can be precomputed and cached with effectivenesses
            damage.BaseValue = weapon.HullDamage.x;
            damageRange.hullRange.x = damage.Value;
            damage.BaseValue = weapon.HullDamage.y;
            damageRange.hullRange.y = damage.Value;
            damageRange *= accuracy;
        }
        damage.RemoveModifier(properties.Profile[(int)face]);
        return damageRange;
    }

    // NO DamageType Effectiveness
    // public Vector2Int CalculateDamageRange(FiringZone.Face face)
    // {
    //     Vector2Int damageRange = new Vector2Int();
    //     damage.AddModifier(properties.Profile[(int)face]);
    //     // if (shield) {

    //     // } else {
    //     damage.BaseValue = activeWeapon.HullDamage.x;
    //     damageRange.x = (int)damage.Value;
    //     damage.BaseValue = activeWeapon.HullDamage.y;
    //     damageRange.y = (int)damage.Value;
    //     damage.RemoveModifier(properties.Profile[(int)face]);
    //     // }
    //     return damageRange;
    // }

    public float CalculateAccuracy(WeaponProperties weapon, Vector3 targetPosition, float targetEvasion, Vector3? position = null)
    {
        float range = Vector3.Distance(targetPosition, position ?? transform.position);
        accuracy.BaseValue = weapon.Accuracy;
        float accuracyVal = (accuracy.Value - targetEvasion) / (100 * range); // TODO change
        accuracyVal = Mathf.Clamp(accuracyVal, 0, 1);
        return accuracyVal;
    }
    #endregion

    public virtual void StartTurn()
    {
        currentAP = Mathf.Min(currentAP + ShipDefaults.BASE_AP, ShipDefaults.MAX_AP);
        StartTurnEvent?.Invoke();
    }

    public virtual void EndTurn()
    {
        EndTurnEvent?.Invoke();
    }

    public abstract void Select();

    public abstract void Deselect();

    public List<bool> AvailableActions(int currentAP)
    {
        List<bool> availableActions = new List<bool>(actions.Length + 1);

        // TODO add movement here
        availableActions.Add(CurrentAP > 0);
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
            if (actions[i] != null &&
                actions[i].apCost <= CurrentAP &&
                actions[i].CurrentCooldown == 0)
            {
                availableActions.Add(actions[i]);
            }
        }
        return availableActions;
    }

    // public List<AbilityProperties> GetAbilities()
    // {
    //     return GetType().GetFields()
    //                .Where(field => field.GetValue(this) is AbilityProperties[])
    //                .SelectMany(param => param.GetValue(this) as AbilityProperties[]).ToList();
    // }

    public ActionProperties GetAction(int option)
    {
        return actions[option];
        /*return GetType().GetFields()
            .Where(field => field.GetValue(this) is ActionProperties[])
            .SelectMany(param => param.GetValue(this) as ActionProperties[]).ToArray()[option - 1];*/
    }
}