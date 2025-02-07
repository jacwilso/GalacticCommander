﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnOrder : MonoBehaviour
{
    public GameEvent EndRoundEvent;

    public static TurnOrder Instance => instance;

    public Ship Current
    {
        get { return indx < 0 ? null : initiative[indx]; }
    }

    [SerializeField]
    TurnIcon currentIcon = null;
    [SerializeField]
    ShipPointer pointer = null;
    [SerializeField]
    UnityEngine.UI.Button endTurnBtn;

    static TurnOrder instance;

    List<Ship> initiative;
    int indx = -1;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("Multiple turn order scripts.");
        instance = this;
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
        initiative = initiative.OrderByDescending(ship => ship.properties.Initiative).ToList();
        StartTurn();
    }

    void StartTurn()
    {
        currentIcon.icon.sprite = Current.properties.Icon;
        Current.StartTurn();
        endTurnBtn.gameObject.SetActive(Current is PlayerShip);
        //pointer.PointTo(initiative[indx]); // TODO
    }

    public void EndTurn()
    {
        Current.EndTurn();
        indx++;
        if (indx >= initiative.Count) {
            EndRoundEvent.Raise();
            EndRound();
        }
        else
            StartTurn();
    }

    public void EndRound()
    {
        indx = -1;
        DetermineIntiative();
    }
}
