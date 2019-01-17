using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    [SerializeField]
    NavField field;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + GetDirection());
    }

    Vector3 GetDirection()
    {
        return field.GetDirectionFromPosition(transform.position);
    }
}
