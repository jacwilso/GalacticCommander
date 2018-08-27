using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnOrder : MonoBehaviour {

    public static TurnOrder Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private TurnIcon current;

    private static TurnOrder instance;

    private List<Ship> initiative;
    private int indx;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple turn order scripts.");
        instance = this;
    }

    private void Start()
    {
        initiative = new List<Ship>();
    }

    public void Subscribe(Ship ship)
    {
        initiative.Add(ship);
    }

    public void Unsubscribe(Ship ship)
    {
        initiative.Remove(ship);
    }

    public void DetermineIntiative()
    {
        indx = 0;
        initiative.OrderBy(ship => ship.properties.Initiative.value);
        StartShipTurn();
    }

    public void StartShipTurn()
    {
        current.icon.sprite = initiative[indx].properties.Icon;
        initiative[indx].StartTurn();
    }

    public void EndShipTurn()
    {
        indx++;
        if (indx >= initiative.Count)
            DetermineIntiative();
        else
            StartShipTurn();
    }
}
