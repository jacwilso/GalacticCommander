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
    protected bool displayMovement;
    protected int turnAP;
    public int TurnAP => turnAP;

    protected virtual void Start()
    {
        zone = GetComponent<FiringZone>();
        ghost = GetComponentInChildren<GhostShip>();
        ghost.Init();
        TurnOrder.Instance.Subscribe(this);
    }

    protected virtual void OnDrawGizmos()
    {
        if (displayMovement)
        {
            Gizmos.color = new Color(0, 1f, 0, 0.25f);
            Gizmos.DrawSphere(transform.position, properties.movement.Speed.Value * turnAP);
        }
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
        turnAP = (int)properties.ActionPoints.Value;
        StartTurnEvent?.Invoke();
    }

    public void EndTurn()
    {
        EndTurnEvent?.Invoke();
    }

    public abstract void Select();

    public abstract void Deselect();
}