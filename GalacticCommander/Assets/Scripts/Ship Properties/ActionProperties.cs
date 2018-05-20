using System;
using UnityEngine;

public abstract class ActionProperties : StatPropertyObject
{
    [SerializeField]
    private int cost;
    public int Cost => cost;
    [SerializeField]
    private int range;
    [NonSerialized]
    public Stat Range;
}