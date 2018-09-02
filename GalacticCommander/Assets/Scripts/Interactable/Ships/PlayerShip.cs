using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerShip : MonoBehaviour, IInteractable
{
    private Ship ship;
    private ActionType action = ActionType.None;

    public void Start()
    {
        ship = GetComponent<Ship>();
    }

    public void Select()
    {
        if (TurnOrder.Instance.Current == ship && action == ActionType.None)
        {
            UISelectors.Instance.transform.position = transform.position;
            UISelectors.Instance.Activate(ship.properties);
        }
    }

    public void Deselect()
    {
        UISelectors.Instance.gameObject.SetActive(false);
    }

    #region Movement
    public void SelectMovement()
    {
        action = ActionType.Movement;
        ARCursor.Instance.Selected = ship.Ghost;
    }

    public void ConfirmMovement()
    {
        StartCoroutine(ExecuteMovement());
    }

    public void CancelMovement()
    {
        action = ActionType.None;
        ship.Ghost.Hide();
    }

    private IEnumerator ExecuteMovement()
    {
        float t = 0f;
        Vector3 startPos = transform.position, endPos = ship.Ghost.transform.position;
        Quaternion startRot = transform.rotation, endRot = ship.Ghost.transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            ship.Ghost.transform.position = endPos;
            ship.Ghost.transform.rotation = endRot;
            yield return null;
        }
        ship.Ghost.Hide();
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

    #endregion
}
