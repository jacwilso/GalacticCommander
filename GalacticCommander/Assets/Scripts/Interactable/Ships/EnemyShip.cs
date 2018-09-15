using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    private StatUI statUI;
    private Ship ship;
    private float cachedAccuracy;
    private Vector2Int cachedDamage;

    protected override void Start()
    {
        base.Start();
        ship = GetComponent<Ship>();
        statUI = GetComponentInChildren<StatUI>();
        GetComponent<Ship>().StartTurnEvent += Move;
        DamageEvent += statUI.UpdateDisplay;
    }

    public override void Select()
    {
        return;
    }

    public override void Deselect()
    {
        return;
    }


    private void Move()
    {
        Debug.Log("move");
        transform.position += transform.forward;
        TurnOrder.Instance.EndTurn();
    }

    public void SelectAttack()
    {
        Ship active = TurnOrder.Instance.Current;
        FiringZone.Face face = active.GetComponent<FiringZone>().FrustrumFace(transform.position);
        CalculateDamage(face);
        CalculateAccuracy(face);
        statUI.AttackDisplay(cachedAccuracy, cachedDamage);
    }

    public void DoAttack()
    {
        if (Random.Range(0, 1) < cachedAccuracy)
        {
            GotHitParams hit = new GotHitParams();
            hit.assailant = TurnOrder.Instance.Current.gameObject;
            hit.damage = Random.Range(cachedDamage.x, cachedDamage.y);
            hit.direction = hit.assailant.transform.position - transform.position;
            Damaged(hit);
        }
    }

    private void CalculateDamage(FiringZone.Face face)
    {
        Ship active = TurnOrder.Instance.Current;
        active.properties.damage.BaseValue = active.properties.activeWeapon[(int)face].Damage.x;
        cachedDamage.x = (int)active.properties.damage.Value;
        active.properties.damage.BaseValue = active.properties.activeWeapon[(int)face].Damage.y;
        cachedDamage.y = (int)active.properties.damage.Value;
    }

    private void CalculateAccuracy(FiringZone.Face face)
    {
        Ship active = TurnOrder.Instance.Current;
        float range = Vector3.Distance(transform.position, active.transform.position);
        active.properties.accuracy.BaseValue = active.properties.activeWeapon[(int)face].Accuracy;
        cachedAccuracy = (active.properties.accuracy.Value - ship.properties.Evasion.Value) * active.properties.activeWeapon.accuracy.Calculate(range);
        cachedAccuracy = Mathf.Max(cachedAccuracy, 0);
    }
}
