using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    [SerializeField]
    EnemyAI aI = null;

    StatUI statUI;

    protected override void Start()
    {
        base.Start();
        statUI = GetComponentInChildren<StatUI>();
        // StartTurnEvent += Move;
        DamageEvent += statUI.UpdateDisplay;
    }

    public override void Select() { }

    public override void Deselect() { }

    public override void StartTurn()
    {
        base.StartTurn();
        Move();
    }

    #region  Movement
    void Move()
    {
        Vector3 moveTo = aI.Move(this);
        StartCoroutine(ExecuteMovement(moveTo));
    }

    IEnumerator ExecuteMovement(Vector3 moveTo)
    {
        float t = 0f;
        Vector3 startPos = transform.position, endPos = moveTo;
        // Quaternion startRot = transform.rotation, endRot = transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            // transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }
        TurnOrder.Instance.EndTurn();
    }
    #endregion

    #region Attacked
    public void SelectAttack()
    {
        Ship active = TurnOrder.Instance.Current;
        FiringZone.Face face = active.GetComponent<FiringZone>().FrustrumFace(transform.position);
        cachedAccuracy = AttackerAccuracy();
        cachedDamage = AttackerDamage(face);
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
        else { Debug.Log("Miss"); }
    }
    #endregion
}
