using System;
using UnityEngine;

public abstract class ActionProperties : StatPropertyObject
{
    [SerializeField]
    private Sprite icon;
    public Sprite Icon => icon;

    [SerializeField]
    private int cost;
    public int Cost => cost;

    //public abstract void Action();
}