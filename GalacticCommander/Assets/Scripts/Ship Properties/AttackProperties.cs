using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    [Range(0, 100)]
    private int accuracy;
    [NonSerialized]
    public Stat Accuracy;

    [SerializeField]
    private int damage;
    [NonSerialized]
    public Stat Damage;

    [SerializeField]
    [Range(-90f, 90f)]
    private float arc;
    public float Arc => arc;

    //[SerializeField]
    //private Transform weapon;
    //public Transform Weapon => weapon;

    [SerializeField]
    private AudioSource hitSFX, missSFX, fireSFX;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
    public AudioSource FireSFX => fireSFX;
}