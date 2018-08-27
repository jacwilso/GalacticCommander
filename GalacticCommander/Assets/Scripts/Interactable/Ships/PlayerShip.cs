using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerShip : MonoBehaviour, IInteractable {

    [SerializeField]
    private GhostShip ghostShip;

    public void Start()
    {
        ghostShip.gameObject.SetActive(false);
        ghostShip.transform.localPosition = Vector3.zero;
        ghostShip.transform.localRotation = Quaternion.identity;
    }

    public void Select()
    {
        //if (TurnBehaviour.instance.Turn == TurnBehaviour.TurnEnum.Player)
        {
            //TODO Move to player ship
            UISelectors.Instance.transform.position = transform.position;
            UISelectors.Instance.gameObject.SetActive(true);
            //TurnBehaviour.instance.ActionPhase += Action;
            //TurnBehaviour.instance.EndPhase += End;
        }// else
        {
            // TODO
        }
    }

    public void Deselect()
    {
        UISelectors.Instance.gameObject.SetActive(false);
    }

    public void Movement()
    {
        ARCursor.Instance.Selected = ghostShip;
    }

    private IEnumerator ExecuteMovement()
    {
        float t = 0f;
        Vector3 startPos = transform.position, endPos = ghostShip.transform.position;
        Quaternion startRot = transform.rotation, endRot = ghostShip.transform.rotation;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            ghostShip.transform.position = endPos;
            ghostShip.transform.rotation = endRot;
            yield return null;
        }
        ghostShip.Hide();

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
}
