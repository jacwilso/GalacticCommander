using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInteractable, IDamageable
{
    [System.Serializable]
    private class AttackPositions
    {
        public AttackProperties 
            top, 
            bottom, 
            left, 
            right, 
            front, 
            back;
    }

    [Header("Properties")]
    [SerializeField]
    private ShipProperties general;
    [SerializeField]
    private MovementProperties movement;
    [SerializeField]
    private AttackPositions attacks;
    [SerializeField]
    private List<AbilityProperties> abilites;

    private int health, shieldStrength;

    private void Start ()
    {
        health = general.Health;
        shieldStrength = general.ShieldStrength;
	}

    public void Interact()
    {
        // TODO: ghost ship
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        shieldStrength -= hit.damage;
        health += Mathf.Min(0, shieldStrength);

        shieldStrength = Mathf.Max(0, shieldStrength);
        health = Mathf.Max(0, health);
        if (health == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Dead
    }
}