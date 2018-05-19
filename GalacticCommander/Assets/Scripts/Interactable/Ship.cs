using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInteractable, IDamageable
{
    [System.Serializable]
    private class AttackPositions
    {
        public AttackProperties 
            top, 
            bottom, 
            left, 
            right, 
            front, 
            back;
    }

    [Header("Properties")]
    [SerializeField]
    private ShipProperties general;
    [SerializeField]
    private MovementProperties movement;
    [SerializeField]
    private AttackPositions attacks;
    [SerializeField]
    private List<AbilityProperties> abilites;
    [SerializeField]
    private GhostShip ghostShip;

    private BezierCurve flightPath;

    private void Start ()
    {
        ghostShip.gameObject.SetActive(false);
        ghostShip.transform.localPosition= Vector3.zero;
        ghostShip.transform.localRotation = Quaternion.identity;
    }

    public void Select()
    {
        if (TurnBehaviour.instance.Turn == TurnBehaviour.TurnEnum.Player)
        {
            UISelectors.instance.transform.position = transform.position;
            UISelectors.instance.gameObject.SetActive(true);
            TurnBehaviour.instance.ActionPhase += Action;
            TurnBehaviour.instance.EndPhase += End;
        } else
        {
            // TODO
        }
    }

    public void Deselect()
    {
        ghostShip.Deselect();
    }

    public void Damaged(GotHitParams hit)
    {
        // TODO include armor values
        general.ShieldStrength.value -= hit.damage;
        general.Health.value += Mathf.Min(0, general.ShieldStrength.value);

        general.ShieldStrength.value = Mathf.Max(0, general.ShieldStrength.value);
        general.Health.value = Mathf.Max(0, general.Health.value);
        if (general.Health.value == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        // Dead
    }

    private void Action()
    {
        StartCoroutine(ExecuteMovement());
    }

    private void End()
    {
        ghostShip.transform.localPosition = Vector3.zero;
        ghostShip.transform.localRotation = Quaternion.identity;
    }

    public void Movement()
    {
        ghostShip.gameObject.SetActive(true);
        ghostShip.Select();
    }

    private IEnumerator ExecuteMovement()
    {
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
    }
}