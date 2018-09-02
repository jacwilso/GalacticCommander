using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class EnemyShip : MonoBehaviour
{
    private StatUI statUI;

    private void Start()
    {
        statUI = GetComponentInChildren<StatUI>();
        GetComponent<Ship>().StartTurnEvent += Move;
    }

    private void Move()
    {
        transform.position += transform.forward;
        TurnOrder.Instance.EndTurn();
    }

    private void Attack()
    {

    }
}
