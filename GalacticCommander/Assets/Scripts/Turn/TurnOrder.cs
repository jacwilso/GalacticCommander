using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurnOrder : MonoBehaviour
{
    public static TurnOrder Instance
    {
        get { return instance; }
    }

    public Ship Current => initiative[indx];

    [SerializeField]
    private TurnIcon currentIcon;
    [SerializeField]
    private ShipPointer pointer;

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
        if (initiative == null)
        {
            initiative = new List<Ship>();
        }
    }

    public void Subscribe(Ship ship)
    {
        if (initiative == null)
        {
            initiative = new List<Ship>();
        }
        initiative.Add(ship);
    }

    public void Unsubscribe(Ship ship)
    {
        initiative.Remove(ship);
    }

    public void DetermineIntiative()
    {
        indx = 0;
        initiative = initiative.OrderByDescending(ship => ship.properties.Initiative.value).ToList();
        StartTurn();
    }

    private void StartTurn()
    {
        currentIcon.icon.sprite = Current.properties.Icon;
        Current.StartTurn();
        //pointer.PointTo(initiative[indx]); // TODO
    }

    public void EndTurn()
    {
        Current.EndTurn();
        indx++;
        if (indx >= initiative.Count)
            DetermineIntiative();
        else
            StartTurn();
    }
}
