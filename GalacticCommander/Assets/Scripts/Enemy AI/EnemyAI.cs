using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyAI : ScriptableObject
{
    [MinValue(0)]
    public float minTargetDist;
    [MinValue(0)]
    public float minDistToMove;
    [Range(0, 100)]
    public float minAccReq;

    enum MovementType
    {
        Closest, Weakest, Center
    }
    [SerializeField]
    MovementType movementType;

    PlayerShip[] players;

    struct Action
    {
        float priority;
        List<ActionProperties> actions;
    };
    SortedList<float, Action> priorityAction;

    public void ActionList(EnemyShip ship)
    {
        players = FindObjectsOfType<PlayerShip>();
        EnemyShip[] allies = FindObjectsOfType<EnemyShip>();
        priorityAction = new SortedList<float, Action>();
        List<AttackProperties> weapons = ship.properties.weapons.OfType<AttackProperties>().ToList();
        List<AbilityProperties> abilities = ship.properties.GetAbilities();
        AbilityProperties.TargetType friendlyAbilityTypes = AbilityProperties.TargetType.Self |
            AbilityProperties.TargetType.SelfAOE |
            AbilityProperties.TargetType.Ally |
            AbilityProperties.TargetType.AllyAOE;
        AbilityProperties.TargetType enemyAbilityTypes = AbilityProperties.TargetType.Enemy |
            AbilityProperties.TargetType.EnemyAOE;
        // for (int i = 0; i < )


        // for (int i = 0; i < )
    }

    void CalculatePositionPriority(EnemyShip ship, EnemyShip[] allies,
       List<AbilityProperties> abilities, List<AttackProperties> weapons, int actionPoints,
       AbilityProperties.TargetType friendlyAbilityTypes, AbilityProperties.TargetType enemyAbilityTypes)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (ship.properties.weapons[i].Cost > actionPoints)
            {
                weapons.RemoveAt(i);
            }
        }

        float priority = 0;
        ship.Zone.RecalculateFrustum(ship.transform.position, ship.transform.rotation);
        for (int i = 0; i < players.Length; i++)
        {
            FiringZone.Face face = ship.Zone.FrustrumFace(players[i].transform.position);
            for (int j = 0; j < ship.properties.weapons.Length; i++)
            {
                ship.properties.activeWeapon = ship.properties.weapons[j];
                float accuracy = players[i].AttackerAccuracy();
                if (accuracy < minAccReq)
                {
                    continue;
                }
                Vector2 damage = players[i].AttackerDamage(face);
                priority += accuracy * 0.5f * (damage.y + damage.x);
            }
            float range = Vector3.Distance(ship.transform.position, players[i].transform.position);
            for (int j = 0; j < abilities.Count; j++)
            {
                if ((abilities[i].Target & friendlyAbilityTypes) == 0 && abilities[i].Ready && abilities[i].Range <= range)
                {
                    priority += abilities[i].AIPriority;
                }
            }
        }
        for (int i = 0; i < allies.Length; i++)
        {
            float range = Vector3.Distance(ship.transform.position, allies[i].transform.position);
            for (int j = 0; j < abilities.Count; j++)
            {
                if ((abilities[i].Target & enemyAbilityTypes) == 0 && abilities[i].Ready && abilities[i].Range <= range)
                {
                    priority += abilities[i].AIPriority;
                }
            }
        }
    }

    #region Movement
    public Vector3 Move(Ship ship)
    {
        Vector3 ideal = MoveToPosition(ship.transform);
        float maxDist = ship.properties.movement.Speed.Value * ship.TurnAP;
        float dist = Vector3.Distance(ship.transform.position, ideal);
        if (dist <= minDistToMove)
        {
            return ship.transform.position;
        }
        if (dist <= maxDist)
        {
            return ideal;
        }
        Vector3 dir = ideal - ship.transform.position;
        return ship.transform.position + dir.normalized * maxDist;
    }

    Vector3 MoveToPosition(Transform shipTransform)
    {
        PlayerShip player = null;
        switch (movementType)
        {
            case MovementType.Center:
                return CenteralPosition(shipTransform);
            case MovementType.Weakest:
                player = WeakestTarget();
                break;
            case MovementType.Closest:
                player = ClosestTarget(shipTransform);
                break;
        }
        Vector3 dir = player.transform.position - shipTransform.position;
        return player.transform.position + dir.normalized * minTargetDist;
    }

    Vector3 CenteralPosition(Transform shipTransform)
    {
        Vector3 center = new Vector3();
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 dir = players[i].transform.position - shipTransform.position;
            center += (players[i].transform.position + dir.normalized * minTargetDist);
        }
        center /= players.Length;
        return center;
    }
    #endregion

    #region Rules
    PlayerShip WeakestTarget()
    {
        float min = Mathf.Infinity;
        PlayerShip weakest = null;
        for (int i = 0; i < players.Length; i++)
        {
            // TODO weight?
            float health = players[i].properties.Health.Value + players[i].properties.ShieldStrength.Value;
            if (health < min)
            {
                min = health;
                weakest = players[i];
            }
        }
        return weakest;
    }

    PlayerShip ClosestTarget(Transform shipTransform)
    {
        float min = Mathf.Infinity;
        PlayerShip player = null;
        for (int i = 0; i < players.Length; i++)
        {
            float dist = Vector3.Distance(shipTransform.position, players[i].transform.position);
            if (dist < min)
            {
                min = dist;
                player = players[i];
            }
        }
        return player;
    }
    #endregion
}