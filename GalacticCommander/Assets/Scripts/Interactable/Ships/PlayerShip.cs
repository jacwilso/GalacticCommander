using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    public int CacheAP
    {
        set { cacheAP = value; }
    }

    GameEvent PlayerAttackEvent;

    ActionType action = ActionType.None;
    int cacheAP;

    void Awake()
    {
        ResourceRequest req = Resources.LoadAsync("Events/PlayerAttackEvent", typeof(GameEvent));
        PlayerAttackEvent = req.asset as GameEvent;
    }

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
        // Debug.Log(segment);
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
                properties.active = attack;
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
    }

    public void ConfirmMovement()
    {
        turnAP -= cacheAP;
        StartCoroutine(ExecuteMovement());
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent -= CancelMovement;
    }

    public void CancelMovement()
    {
        action = ActionType.None;
        Ghost.Hide();
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent -= CancelMovement;
    }

    IEnumerator ExecuteMovement()
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
    void SelectAttack()
    {
        action = ActionType.Attack;
        PlayerAttackEvent.Raise();
        ConfirmationUI.Instance.ConfirmEvent += ConfirmAttack;
        ConfirmationUI.Instance.CancelEvent += CancelAttack;
        ConfirmationUI.Instance.Activate(false, true);
        ARCursor.Instance.SelectEvent += CheckEnemy;
    }

    void CheckEnemy()
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

    void ConfirmAttack()
    {
        turnAP -= properties.active.Cost;
        properties.active.Used();
        EnemyShip enemy = (EnemyShip)ARCursor.Instance.Selected;
        if (enemy)
        {
            enemy.DoAttack();
            UnregisterAttackEvents();
        }
    }

    void CancelAttack()
    {
        UnregisterAttackEvents();
    }

    void UnregisterAttackEvents()
    {
        action = ActionType.None;
        PlayerAttackEvent.Lower();
        ARCursor.Instance.SelectEvent -= CheckEnemy;
        ConfirmationUI.Instance.ConfirmEvent -= ConfirmAttack;
        ConfirmationUI.Instance.CancelEvent -= CancelAttack;
        ConfirmationUI.Instance.Activate(false);
    }
    #endregion
}
