using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IDamageable {

    public ShipProperties properties;
    public event Action DeathEvent;

    private void Start()
    {
        TurnOrder.Instance.Subscribe(this);
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        properties.ShieldStrength.value -= hit.damage;
        properties.Health.value += Mathf.Min(0, properties.ShieldStrength.value);

        properties.ShieldStrength.value = Mathf.Max(0, properties.ShieldStrength.value);
        properties.Health.value = Mathf.Max(0, properties.Health.value);
        if (properties.Health.value == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Dead
        DeathEvent();
        TurnOrder.Instance.Unsubscribe(this);
    }

    public void StartTurn()
    {

    }

    private void EndTurn()
    {
        //ghostShip.End();
    }
}