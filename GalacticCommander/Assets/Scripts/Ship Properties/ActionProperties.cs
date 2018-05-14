using UnityEngine;

public abstract class ActionProperties : ScriptableObject
{
    [SerializeField]
    private int cost;
    public int Cost => cost;
    [SerializeField]
    private int range;
    public int Range => range;
}