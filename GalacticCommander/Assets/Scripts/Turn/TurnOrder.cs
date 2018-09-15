using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnOrder : MonoBehaviour
{
    public event Action EndRoundEvent;

    public static TurnOrder Instance => instance;

    public Ship Current
    {
        get { return indx < 0 ? null : initiative[indx]; }
    }

    [SerializeField]
    private TurnIcon currentIcon;
    [SerializeField]
    private ShipPointer pointer;

    private static TurnOrder instance;

    private List<Ship> initiative;
    private int indx = -1;

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
        EndRoundEvent += EndRound;
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
        initiative = initiative.OrderByDescending(ship => ship.properties.Initiative.Value).ToList();
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
            EndRoundEvent?.Invoke();
        else
            StartTurn();
    }

    public void EndRound()
    {
        indx = -1;
    }
}
