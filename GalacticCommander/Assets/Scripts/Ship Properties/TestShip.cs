using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShip : MonoBehaviour
{
    public ShipProperties ship;
    public bool damage;

    void Start()
    {
        ship = ShipProperties.Instantiate(ship);
        if (damage) ship.Hull.Value -= 1;
        Debug.Log(ship.Hull.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
