using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    [SerializeField]
    private GameEvent attackEvent;

    private ActionType action = ActionType.None;
    private int turnAP;
    public int TurnAP => turnAP;

    protected override void Start()
    {
        base.Start();
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

    public override void StartTurn()
    {
        base.StartTurn();
        turnAP = (int)properties.ActionPoints.Value;
        Debug.Log("Start Turn AP: " + turnAP);
    }

    public void Action(int segment)
    {
        // TODO
        //properties.GetAction(segment).Action();
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
                    case AbilityProperties.TargetType.Self:
                        break;
                    case AbilityProperties.TargetType.Ally:
                        break;
                    case AbilityProperties.TargetType.Enemy:
                        break;
                    case AbilityProperties.TargetType.All:
                        break;
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
        zone.RecalculateFrustum();
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
