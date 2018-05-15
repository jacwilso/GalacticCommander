using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : LookAt
{
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
    }
}
