using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    public int CacheAP
    {
        set { cacheAP = value; }
    }

    [SerializeField]
    private GameEvent attackEvent;

    private ActionType action = ActionType.None;
    private int cacheAP;

    public override void Select()
    {
        if (TurnOrder.Instance.Current == this && action == ActionType.None)
        {
            UIWheel.ActionWheel.Instance.worldPos = transform.position;
            UIWheel.ActionWheel.Instance.Activate(this, properties);
        }
    }

    public override void Deselect()
    {
        UIWheel.ActionWheel.Instance.Deselect();
    }

    public void Action(int segment)
    {
        ConfirmationUI.Instance.TurnAction();
        if (segment == 0)
        {
            SelectMovement();
        }
        else
        {
            ActionProperties action = properties.GetAction(segment);
            AttackProperties attack = action as AttackProperties;
            if (attack is AttackProperties)
            {
                properties.activeWeapon = attack;
                SelectAttack();
                // Display accuracies
            }
            else
            {
                AbilityProperties ability = action as AbilityProperties;
                switch (ability.Target)
                {
                    case AbilityProperties.TargetType.Self: break;
                    case AbilityProperties.TargetType.SelfAOE: break;
                    case AbilityProperties.TargetType.Ally: break;
                    case AbilityProperties.TargetType.AllyAOE: break;
                    case AbilityProperties.TargetType.Enemy: break;
                    case AbilityProperties.TargetType.EnemyAOE: break;
                    case AbilityProperties.TargetType.All: break;
                }
            }
        }
    }

    #region Movement
    public void SelectMovement()
    {
        action = ActionType.Movement;
        ARCursor.Instance.Selected = Ghost;
        ConfirmationUI.Instance.ConfirmEvent += ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent += CancelMovement;
        displayMovement = true;
    }

    public void ConfirmMovement()
    {
        turnAP -= cacheAP;
        StartCoroutine(ExecuteMovement());
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent -= CancelMovement;
        displayMovement = false;
    }

    public void CancelMovement()
    {
        action = ActionType.None;
        Ghost.Hide();
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent -= CancelMovement;
        displayMovement = false;
    }

    private IEnumerator ExecuteMovement()
    {
        float t = 0f;
        Vector3 startPos = transform.position, endPos = Ghost.transform.position;
        Quaternion startRot = transform.rotation, endRot = Ghost.transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            Ghost.transform.position = endPos;
            Ghost.transform.rotation = endRot;
            yield return null;
        }
        Ghost.Hide();
        zone.RecalculateFrustum(transform.position, transform.rotation);
        action = ActionType.None;

        /*
        flightPath = new BezierCurve(new Vector3[4]{
            transform.position,
            ghostShip.transform.position,
            transform.position,
            ghostShip.transform.position
        });
        ghostShip.gameObject.SetActive(false);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = flightPath.GetPoint(t);
            //Debug.Log(t + " " + flightPath.GetPoint(t));
            yield return null;
        }
        */
    }
    #endregion

    #region Attack
    private void SelectAttack()
    {
        action = ActionType.Attack;
        attackEvent.Raise();
        ConfirmationUI.Instance.ConfirmEvent += ConfirmAttack;
        ConfirmationUI.Instance.CancelEvent += CancelAttack;
        ConfirmationUI.Instance.Activate(false, true);
        ARCursor.Instance.SelectEvent += CheckEnemy;
    }

    private void CheckEnemy()
    {
        IInteractable iEnemy = ARCursor.Instance.Selected;
        if (iEnemy is EnemyShip)
        {
            EnemyShip enemy = iEnemy as EnemyShip;
            ConfirmationUI.Instance.Activate(true);
        }
        else
        {
            ConfirmationUI.Instance.Activate(false, true);
        }
    }

    private void ConfirmAttack()
    {
        turnAP -= properties.activeWeapon.Cost;
        EnemyShip enemy = (EnemyShip)ARCursor.Instance.Selected;
        if (enemy)
        {
            enemy.DoAttack();
            UnregisterAttackEvents();
        }
    }

    private void CancelAttack()
    {
        UnregisterAttackEvents();
    }

    private void UnregisterAttackEvents()
    {
        action = ActionType.None;
        attackEvent.Lower();
        ARCursor.Instance.SelectEvent -= CheckEnemy;
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmAttack;
        ConfirmationUI.Instance.CancelEvent -= CancelAttack;
        ConfirmationUI.Instance.Activate(false);
    }
    #endregion
}
