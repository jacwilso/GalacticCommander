using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IDamageable
{
    public event Action DeathEvent;
    public event Action DamageEvent;
    public event Action StartTurnEvent;
    public event Action EndTurnEvent;

    public GhostShip Ghost => ghost;

    public ShipProperties properties;

    private GhostShip ghost;

    private void Start()
    {
        ghost = GetComponentInChildren<GhostShip>();
        ghost.Init();
        TurnOrder.Instance.Subscribe(this);
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        properties.ShieldStrength.value -= hit.damage;
        properties.Health.value += Mathf.Min(0, properties.ShieldStrength.value);

        properties.ShieldStrength.value = Mathf.Max(0, properties.ShieldStrength.value);
        properties.Health.value = Mathf.Max(0, properties.Health.value);
        DamageEvent?.Invoke();
        if (properties.Health.value == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Dead
        DeathEvent?.Invoke();
        TurnOrder.Instance.Unsubscribe(this);
    }

    public void StartTurn()
    {
        StartTurnEvent?.Invoke();
    }

    public void EndTurn()
    {
        EndTurnEvent?.Invoke();
        //ghostShip.End();
    }
}