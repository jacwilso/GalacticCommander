using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerShip : Ship
{
    public int CacheAP
    {
        set { cacheAP = value; }
    }

    ActionType action = ActionType.None;
    int cacheAP;

    static List<PlayerShip> playerShips = new List<PlayerShip>();
    public static List<PlayerShip> PlayerShips => playerShips;

    void OnEnable()
    {
        playerShips.Add(this);
    }

    void OnDisable()
    {
        playerShips.Remove(this);
    }

    public override void Select()
    {
        if (TurnOrder.Instance.Current == this && action == ActionType.None)
        {
            UIWheel.ActionWheel.Instance.worldPos = transform.position;
            UIWheel.ActionWheel.Instance.Activate(this);
        }
    }

    public override void Deselect()
    {
        UIWheel.ActionWheel.Instance.Deselect();
    }

    #region Action Wheel
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
            ActionProperties action = GetAction(segment - 1);
            WeaponProperties attack = action as WeaponProperties;
            if (attack is WeaponProperties)
            {
                ActiveWeapon = attack;
                SelectedWeapon();
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

    public List<Sprite> GetIcons()
    {
        List<Sprite> icons = new List<Sprite>(actions.Length + 1);

        foreach (var action in actions)
        {
            if (action)
            {
                icons.Add(action.Icon);
            }
        }
        return icons;
    }
    #endregion

    #region Movement
    public void SelectMovement()
    {
        action = ActionType.Move;
        ARCursor.Instance.Selected = Ghost;
        ConfirmationUI.Instance.ConfirmEvent += ConfirmMovement;
        ConfirmationUI.Instance.CancelEvent += CancelMovement;
    }

    public void ConfirmMovement()
    {
        currentAP -= cacheAP;
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
    void SelectedWeapon()
    {
        action = ActionType.Weapon;
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

    // TODO: Move to Ship
    void ConfirmAttack()
    {
        currentAP -= ActiveWeapon.apCost;
        ActiveWeapon.Used();
        EnemyShip enemy = (EnemyShip)ARCursor.Instance.Selected;
        if (enemy)
        {
            enemy.GetAttacked();
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
