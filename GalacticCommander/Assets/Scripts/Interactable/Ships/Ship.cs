using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Ship : MonoBehaviour, IDamageable, IInteractable
{
    public event Action DeathEvent;
    public event Action DamageEvent;
    public event Action StartTurnEvent;
    public event Action EndTurnEvent;

    public GhostShip Ghost => ghost;

    public ShipProperties properties;

    protected FiringZone zone;
    protected GhostShip ghost;

    protected virtual void Start()
    {
        zone = GetComponent<FiringZone>();
        ghost = GetComponentInChildren<GhostShip>();
        ghost.Init();
        TurnOrder.Instance.Subscribe(this);
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        // Debug.Log(hit.damage + " " + properties.Health.Value + " " + properties.ShieldStrength.Value);
        properties.ShieldStrength.Value -= hit.damage;
        properties.Health.Value += Mathf.Min(0, properties.ShieldStrength.Value);

        properties.ShieldStrength.Value = Mathf.Max(0, properties.ShieldStrength.Value);
        properties.Health.Value = Mathf.Max(0, properties.Health.Value);
        DamageEvent?.Invoke();
        if (properties.Health.Value == 0)
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

    public virtual void StartTurn()
    {
        StartTurnEvent?.Invoke();
    }

    public void EndTurn()
    {
        EndTurnEvent?.Invoke();
        //ghostShip.End();
    }

    public abstract void Select();
    // {
    //     throw new NotImplementedException();
    // }

    public abstract void Deselect();
    // {
    //     throw new NotImplementedException();
    // }
}