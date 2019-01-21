using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    [SerializeField]
    EnemyAI aI = null;
    [SerializeField]
    StatUI statUI;

    [Header("AI")]
    [SerializeField]
    NavField field;
    [SerializeField, Tooltip("Minimum accuracy required for AI to perform")]
    int minAccuracy = 50;
    [SerializeField, Tooltip("Minimum value for AI to consider a turn of full move/storing points")]
    int minActionValue = 10;

    protected override void Start()
    {
        base.Start();
        statUI = GetComponentInChildren<StatUI>();
        // StartTurnEvent += Move;
        DamageEvent += statUI.UpdateDisplay;
        PlayerAttackEvent.RegisterListener(PlayerWeaponDisplay);
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
        TurnOrder.Instance.EndTurn(); // TODO remove
    }
    #endregion

    #region Attacked
    public void PlayerWeaponDisplay()
    {
        Ship active = TurnOrder.Instance.Current;
        cachedAccuracy = active.CalculateAccuracy(transform.position, properties.Evasion.Value);
        cachedDamage = active.CalculateDamageRange(active.Zone.FrustrumFace(transform.position));
        statUI.AttackDisplay(cachedAccuracy, cachedDamage);
    }
    #endregion

    [ContextMenu("Evaluate AI")]
    void EvaluateAI()
    {
        int weaponCnt = Weapons.Length;
        int shipCnt = PlayerShip.PlayerShips.Count;
        Vector3 position = transform.position;

        // Caching the accuracy and face the ship is on
        Tuple<float, FiringZone.Face>[] targetParams = new Tuple<float, FiringZone.Face>[weaponCnt * shipCnt];
        Ship.WeaponDamageRange[] targetDamage = new Ship.WeaponDamageRange[shipCnt];
        // Things the AI will do
        List<Tuple<ActionType, int, int>> actionList = new List<Tuple<ActionType, int, int>>(CurrentAP);
        // Best action
        float maxActionValue = 0;
        for (int i = 0; i < CurrentAP; i++)
        {
            // AP after a move (ie each iteration is assuming 1 movement)
            int ap = CurrentAP - i;

            // Calculate + cache the accuracy of a weapon to each ship and the face the ship is on from the ship
            // This is just setup
            // TODO this can be put off into the recursion loop
            for (int j = 0; j < shipCnt; j++)
            {
                Ship target = PlayerShip.PlayerShips[j];
                FiringZone.Face face = Zone.FrustrumFace(target.transform.position);
                for (int k = 0; k < weaponCnt; k++)
                {
                    if (Weapons[k].apCost > CurrentAP - i) continue;
                    float acc = CalculateAccuracy(target.transform.position,
                        target.properties.Evasion.Value,
                        position);
                    if (acc < minAccuracy) continue;
                    targetParams[k * shipCnt + j] = new Tuple<float, FiringZone.Face>(acc, face);
                }
            }

            // List of each actions to perform (in order of action) => 
            // ActionType (move/weapon/ability), movement units or weapon index, ship to target
            actionList.Add(new Tuple<ActionType, int, int>(ActionType.Move, i, 0));
            int actionListCnt = actionList.Count;
            var actionListClone = new List<Tuple<ActionType, int, int>>(actionList);
            // iterate through each weapon to evaluate the possible damage
            for (int j = 0; j < weaponCnt; j++)
            {
                // calculate the value of chosing this weapon 1st (recursive function, will go until ap runs out)
                // returns maximum outcome and action list to achieve this
                float actionsValue = WeaponValue(ap, j, targetParams, targetDamage, actionListClone);
                if (actionsValue > maxActionValue)
                {
                    maxActionValue = actionsValue;
                    actionList = actionListClone;
                }
            }
            // "moves" the ship in direction of vector field
            for (int j = 0; j < properties.Speed.Value; j++)
            {
                position += field.GetDirectionFromPosition(position);
            }
            // recalculates the planes of attack (assumes no rotation currently)
            Zone.RecalculateFrustum(position, transform.rotation);
        }
        // if max action isn't that great just do full movement (can be changed to save ap)
        if (maxActionValue < minActionValue) actionList = new List<Tuple<ActionType, int, int>> {
                new Tuple<ActionType, int, int>(ActionType.Move, CurrentAP, 0)
        };

        Debug.Log("Action List Order:");
        foreach (var action in actionList)
        {
            Debug.Log($"Action: {action.Item1}, move/accuracy: {action.Item2}, target {action.Item3}");
        }
    }

    float WeaponValue(int ap, int weaponIndex,
        Tuple<float, FiringZone.Face>[] targetParams,
        Ship.WeaponDamageRange[] targetDamage,
        List<Tuple<ActionType, int, int>> actionList)
    {
        // can't afford to use this weapon
        if (ap < Weapons[weaponIndex].apCost || Weapons[weaponIndex].CurrentCooldown > 0) return 0;

        ActiveWeapon = Weapons[weaponIndex];
        int shipCnt = PlayerShip.PlayerShips.Count;
        float maxDmgSum = 0;
        int maxShip = -1;
        WeaponDamageRange maxRange = null;
        // Find the ship that would take the most damage from this weapon
        for (int i = 0; i < shipCnt; i++)
        {
            Ship target = PlayerShip.PlayerShips[i];
            int index = weaponIndex * shipCnt + i;
            if (targetParams[index].Item1 < minAccuracy) continue;

            // Where the damage is calculated
            // Vector2 dmg = CalculateDamageRange(targetDamage);
            // dmg *= targetParams[index].Item1;
            // float dmgSum = dmg.x + dmg.y;
            WeaponDamageRange dmgRange = CalculateDamageRange(targetDamage[i], targetParams[index].Item2, target.properties.Shield.Value);
            dmgRange *= targetParams[index].Item1;
            float dmgSum = dmgRange.DamageSum();
            if (dmgSum > maxDmgSum)
            {
                maxDmgSum = dmgSum;
                maxShip = i;
                maxRange = dmgRange;
            }
        }
        if (maxDmgSum <= 0) return 0;
        targetDamage[maxShip] += maxRange;
        actionList.Add(new Tuple<ActionType, int, int>(ActionType.Weapon, weaponIndex, maxShip));
        ap -= ActiveWeapon.apCost;

        int weaponCnt = Weapons.Length;
        float maxWeaponValue = 0;
        int actionListCnt = actionList.Count;
        var actionListClone = new List<Tuple<ActionType, int, int>>(actionList);
        // Find what next set of actions would get the most benefit
        for (int i = 0; i < weaponCnt; i++)
        {
            // recursion
            float weaponDmg = WeaponValue(ap, i, targetParams, targetDamage, actionListClone);
            if (weaponDmg > maxWeaponValue)
            {
                maxWeaponValue = weaponDmg;
                actionList = actionListClone;
            }
            actionListClone.RemoveRange(actionListCnt, actionListClone.Count - actionListCnt);
        }
        return maxDmgSum + maxWeaponValue;
    }
}
