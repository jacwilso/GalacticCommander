using UnityEngine;

[CreateAssetMenu()]
public class EnemyAI : ScriptableObject
{
    [MinValue(0)]
    public float minTargetDist;
    [MinValue(0)]
    public float minDistToMove;

    enum MovementType
    {
        Closest, Weakest, Center
    }
    [SerializeField]
    private MovementType movementType;

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

    private Vector3 MoveToPosition(Transform shipTransform)
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

    private Vector3 CenteralPosition(Transform shipTransform)
    {
        Vector3 center = new Vector3();
        PlayerShip[] players = FindObjectsOfType<PlayerShip>();
        for (int i = 0; i < players.Length; i++)
        {
            Vector3 dir = players[i].transform.position - shipTransform.position;
            center += (players[i].transform.position + dir.normalized * minTargetDist);
        }
        center /= players.Length;
        return center;
    }
    #endregion

    private PlayerShip WeakestTarget()
    {
        float min = Mathf.Infinity;
        PlayerShip[] players = FindObjectsOfType<PlayerShip>();
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

    private PlayerShip ClosestTarget(Transform shipTransform)
    {
        float min = Mathf.Infinity;
        PlayerShip[] players = FindObjectsOfType<PlayerShip>();
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
}