using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Movement")]
public class MovementProperties : ActionProperties
{
    [SerializeField]
    float speed;
    [NonSerialized]
    public Stat Speed;
}