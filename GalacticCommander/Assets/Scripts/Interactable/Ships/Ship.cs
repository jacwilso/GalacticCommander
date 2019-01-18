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
    #endregion

    public ShipProperties properties;

    protected GhostShip ghost;
    public GhostShip Ghost => ghost;

    protected FiringZone zone;
    public FiringZone Zone => zone;

    protected int turnAP;
    public int TurnAP => turnAP;

    protected float cachedAccuracy;
    protected Vector2Int cachedDamage;

    protected virtual void Start()
    {
        properties = ShipProperties.Instantiate(properties);
        zone = GetComponent<FiringZone>();
        ghost = GetComponentInChildren<GhostShip>();
        ghost.Init();
        TurnOrder.Instance.Subscribe(this);


    }

    #region Damage
    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        // Debug.Log(hit.damage + " " + properties.Health.Value + " " + properties.ShieldStrength.Value);
        properties.Shield.Value -= hit.damage;
        properties.Hull.Value += Mathf.Min(0, properties.Shield.Value);

        properties.Shield.Value = Mathf.Max(0, properties.Shield.Value);
        properties.Hull.Value = Mathf.Max(0, properties.Hull.Value);
        Debug.Log(properties.Hull.Value + " " + properties.Shield.Value);
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
        Instantiate<ParticleSystem>(properties.Explosion, transform);
    }

    public Vector2Int AttackerDamage(FiringZone.Face face)
    {
        Vector2Int damage = new Vector2Int();
        Ship attacker = TurnOrder.Instance.Current;
        attacker.properties.damage.AddModifier(attacker.properties.profile[(int)face]);
        attacker.properties.damage.BaseValue = attacker.properties.active.Damage.x;
        damage.x = (int)attacker.properties.damage.Value;
        attacker.properties.damage.BaseValue = attacker.properties.active.Damage.y;
        damage.y = (int)attacker.properties.damage.Value;
        attacker.properties.damage.RemoveModifier(attacker.properties.profile[(int)face]);
        return damage;
    }

    public float AttackerAccuracy()
    {
        Ship attacker = TurnOrder.Instance.Current;
        float range = Vector3.Distance(transform.position, attacker.transform.position);
        attacker.properties.accuracy.BaseValue = attacker.properties.active.Accuracy;
        float accuracy = (attacker.properties.accuracy.Value - properties.Evasion.Value) * attacker.properties.active.curve.Calculate(range);
        accuracy = Mathf.Max(accuracy, 0);
        return accuracy;
    }
    #endregion

    public virtual void StartTurn()
    {
        turnAP = properties.ActionPoints;
        StartTurnEvent?.Invoke();
    }

    public void EndTurn()
    {
        EndTurnEvent?.Invoke();
    }

    public abstract void Select();

    public abstract void Deselect();
}