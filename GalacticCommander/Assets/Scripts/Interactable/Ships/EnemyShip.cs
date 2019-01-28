using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    // [SerializeField]
    // EnemyAI aiData = null;
    [SerializeField]
    StatUI statUI;

    [Header("AI")]
    [SerializeField]
    NavField field;
    [SerializeField, Tooltip("Minimum accuracy required for AI to perform")]
    int minAccuracy = 50;
    [SerializeField, Tooltip("Minimum value for AI to consider a turn of full move/storing points")]
    int minActionValue = 10;
    [SerializeField]
    int stoppingDist = 1;

    protected override void Start()
    {
        base.Start();
        statUI = GetComponentInChildren<StatUI>();
        // StartTurnEvent += EvaluateAI;
        DamageEvent += statUI.UpdateDisplay;
        PlayerAttackEvent.RegisterListener(PlayerWeaponDisplay);
    }

    public override void Select() { }

    public override void Deselect() { }

    public override void StartTurn()
    {
        base.StartTurn();
        var actions = EvaluateAI();
        StartCoroutine(ExecuteTurn(actions));
    }

    private IEnumerator ExecuteTurn(List<Tuple<ActionType, ActionParams>> actions) {
        yield return ExecuteMovement((actions[0].Item2 as MoveParams).units);
        // yield return Attack
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        ExecuteAttack(actions);
        TurnOrder.Instance.EndTurn();
    }

    #region  Movement
    IEnumerator ExecuteMovement(int units)
    {
        // TODO use stopping distance

        // Quaternion startRot = transform.rotation, endRot = transform.rotation;
        for (int i = 0; i < units * properties.Speed.Value; i++) {
            float t = 0f;
            Vector3 startPos = transform.position;
            var dir = field.GetDirection(transform.position);
            
            while (t < 1f)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, startPos + dir, t);
                // transform.rotation = Quaternion.Lerp(startRot, endRot, t);
                yield return null;
            }
        }
        yield return null;
    }
    #endregion

    #region Attack
    void ExecuteAttack(List<Tuple<ActionType, ActionParams>> actions) {
        foreach (var action in actions)
        {
            Debug.Log($"{action.Item1} {action.Item2}");
            switch(action.Item1) {
                case ActionType.Weapon:
                    var weaponAction = action.Item2 as WeaponParams;
                    var target = PlayerShip.PlayerShips[weaponAction.target];
                    target.cachedAccuracy = weaponAction.accuracy;
                    target.cachedDamage = weaponAction.range;
                    UseWeapon(Weapons[weaponAction.weapon], target);
                break;
                case ActionType.Ability: // TODO
                break;
                case ActionType.Move:
                break;
            }
        }
    }

    public void PlayerWeaponDisplay()
    {
        PlayerShip player = TurnOrder.Instance.Current as PlayerShip;
        cachedAccuracy = player.CalculateAccuracy(player.ActiveWeapon, transform.position, properties.Evasion.Value);
        cachedDamage = player.CalculateDamageRange(
            player.ActiveWeapon,
            cachedAccuracy, // TODO Do we show damage from accuracy?
            player.Zone.FrustrumFace(transform.position),
            properties.Shield.Value
        );
        statUI.DisplayAttack(cachedAccuracy, cachedDamage);
    }
    #endregion

    [ContextMenu("Evaluate AI")]
    List<Tuple<ActionType, ActionParams>> EvaluateAI()
    {
        currentAP = ShipDefaults.BASE_AP; // TODO REMOVE
        field.GenerateField(); // TODO REMOVE
        System.Text.StringBuilder str; // TODO REMOVE

        int weaponCnt = Weapons.Length;
        int shipCnt = PlayerShip.PlayerShips.Count;
        Vector3 position = transform.position;

        // Caching the accuracy and face the ship is on
        Tuple<float, FiringZone.Face>[] targetParams = new Tuple<float, FiringZone.Face>[weaponCnt * shipCnt];
        WeaponDamageRange[] targetDamage = new WeaponDamageRange[shipCnt];
        // Things the AI will do
        // List<Tuple<ActionType, int, int>> actionList = null;
        List<Tuple<ActionType, ActionParams>> actionList = null;
        // Best action
        float maxActionValue = 0;
        for (int i = 0; i < CurrentAP; i++)
        {
            // AP after a move (ie each iteration is assuming 1 movement)
            int ap = CurrentAP - i;
            Zone.RecalculateFrustum(position, transform.rotation);

            // Calculate + cache the accuracy of a weapon to each ship and the face the ship is on from the ship
            // This is just setup
            // TODO this can be put off into the recursion loop
            for (int j = 0; j < shipCnt; j++)
            {
                Ship target = PlayerShip.PlayerShips[j];
                FiringZone.Face face = Zone.FrustrumFace(target.transform.position);
                if (face == FiringZone.Face.None || properties.Profile[(int)face].Value == 0) {
                    for (int k = 0; k < weaponCnt; k++) {
                        targetParams[k * shipCnt + j] = new Tuple<float, FiringZone.Face>(0, FiringZone.Face.None);
                    }
                    Debug.Log("Skip face.");
                    continue;
                }
                System.Text.StringBuilder cacheStr = new System.Text.StringBuilder($"Ship: {j} => Face: {face}");
                targetDamage[j] = new WeaponDamageRange();
                for (int k = 0; k < weaponCnt; k++)
                {
                    if (Weapons[k].apCost > CurrentAP - i) continue;
                    float acc = CalculateAccuracy(
                        Weapons[k],
                        target.transform.position,
                        target.properties.Evasion.Value,
                        position
                    );
                    // if (acc < minAccuracy) continue;
                    cacheStr.Append($" Weapon: {k} => Accuracy: {acc}");
                    targetParams[k * shipCnt + j] = new Tuple<float, FiringZone.Face>(acc, face);
                }
                Debug.Log(cacheStr);
            }

            // List of each actions to perform (in order of action) => 
            // ActionType (move/weapon/ability), movement units or weapon index, ship to target

            int actionListCnt = 1;
            // var actionListClone = new List<Tuple<ActionType, int, int>>(CurrentAP){
            //     new Tuple<ActionType, int, int>(ActionType.Move, i, 0)
            // };
            var actionListClone = new List<Tuple<ActionType, ActionParams>>(CurrentAP){
                new Tuple<ActionType, ActionParams>(ActionType.Move, new MoveParams(i))
            };
            // iterate through each weapon to evaluate the possible damage
            for (int j = 0; j < weaponCnt; j++)
            {
                // calculate the value of chosing this weapon 1st (recursive function, will go until ap runs out)
                // returns maximum outcome and action list to achieve this
                float actionsValue = WeaponValue(ap, j, new int[weaponCnt], targetParams, targetDamage, actionListClone);
                if (actionsValue > maxActionValue)
                {
                    maxActionValue = actionsValue;
                    actionList = new List<Tuple<ActionType, ActionParams>>(actionListClone);
                }

                // str = new System.Text.StringBuilder($"Action List: {actionsValue} => {actionListClone.Count}\n");
                // foreach (var action in actionListClone)
                // {
                //     str.Append($"Action: {action.Item1}, ");
                //     switch (action.Item1) {
                //         case ActionType.Move:
                //             str.Append($"Units: {action.Item2}\n");
                //             break;
                //         case ActionType.Weapon:
                //             str.Append($"Weapon Index: {action.Item2}, Target: {action.Item3}\n");
                //             break;
                //     }
                // }
                // Debug.Log(str);
                actionListClone.RemoveRange(actionListCnt, actionListClone.Count - 1);
            }
            // "moves" the ship in direction of vector field
            for (int j = 0; j < properties.Speed.Value; j++)
            {
                position += field.GetDirection(position);
            }
            // Debug.Log($"position: {position}");
            // recalculates the planes of attack (assumes no rotation currently)
        }
        // if max action isn't that great just do full movement (can be changed to save ap)
        if (maxActionValue < minActionValue) actionList = new List<Tuple<ActionType, ActionParams>> {
                // new Tuple<ActionType, ActionParams>(ActionType.Move, new MoveParams(CurrentAP))
        };

        str = new System.Text.StringBuilder($"Action List: {maxActionValue} => {actionList.Count}\n");
        foreach (var action in actionList)
        {
            str.Append($"Action: {action.Item1}, {action.Item2}\n");
            // switch (action.Item1) {
            //     case ActionType.Move:
            //         str.Append(action.Item2 + "\n");
            //         break;
            //     case ActionType.Weapon:
            //         str.Append($"Weapon Index: {action.Item2}, Target: {action.Item3}\n");
            //         break;
            // }
        }
        Debug.Log(str);

        return actionList;
    }

    float WeaponValue(int ap, int weaponIndex,
        int[] weaponCooldowns,
        Tuple<float, FiringZone.Face>[] targetParams,
        WeaponDamageRange[] targetDamage,
        List<Tuple<ActionType, ActionParams>> actionList)
    {
        // can't afford to use this weapon
        if (ap < Weapons[weaponIndex].apCost || 
            Weapons[weaponIndex].CurrentCooldown > 0 ||
            weaponCooldowns[weaponIndex] > 0) 
            return 0;

        int shipCnt = PlayerShip.PlayerShips.Count;
        float maxDmgSum = 0;
        WeaponParams parameters = null;
        // Find the ship that would take the most damage from this weapon
        for (int i = 0; i < shipCnt; i++)
        {
            Ship target = PlayerShip.PlayerShips[i];
            int index = weaponIndex * shipCnt + i;
            if (targetParams[index] == null || 
            //     targetParams[index].Item1 < minAccuracy ||
                targetParams[index].Item2 == FiringZone.Face.None ||
                false) continue;

            // Where the damage is calculated
            // Vector2 dmg = CalculateDamageRange(targetDamage);
            // dmg *= targetParams[index].Item1;
            // float dmgSum = dmg.x + dmg.y;
            WeaponDamageRange dmgRange = CalculateDamageRange(
                Weapons[weaponIndex],
                targetParams[index].Item1, 
                targetParams[index].Item2, 
                target.properties.Shield.Value, 
                targetDamage[i].shieldRange.x
            );
            float dmgSum = dmgRange.DamageSum();
            if (dmgSum > maxDmgSum)
            {
                maxDmgSum = dmgSum;
                parameters = new WeaponParams(weaponIndex, i, targetParams[index].Item1, dmgRange);
            }
        }
        Debug.Log($"Damage Sum: {maxDmgSum} {parameters}");
        if (maxDmgSum <= 0 || parameters == null) return 0;
        targetDamage[parameters.target] += parameters.range;
        actionList.Add(new Tuple<ActionType, ActionParams>(ActionType.Weapon, parameters));
        ap -= Weapons[weaponIndex].apCost;
        weaponCooldowns[weaponIndex] = Weapons[weaponIndex].TurnCooldown;

return maxDmgSum;
        int weaponCnt = Weapons.Length;
        float maxWeaponValue = 0;
        int actionListCnt = actionList.Count;
        var actionListClone = new List<Tuple<ActionType, ActionParams>>(actionList);
        // Find what next set of actions would get the most benefit
        for (int i = 0; i < weaponCnt; i++)
        {
            // recursion
            float weaponDmg = WeaponValue(ap, i, weaponCooldowns, targetParams, targetDamage, actionListClone);
            if (weaponDmg > maxWeaponValue)
            {
                maxWeaponValue = weaponDmg;
                actionList = new List<Tuple<ActionType, ActionParams>>(actionListClone);
            }
            actionListClone.RemoveRange(actionListCnt, actionListClone.Count - actionListCnt);
        }
        return maxDmgSum + maxWeaponValue;
    }
}
