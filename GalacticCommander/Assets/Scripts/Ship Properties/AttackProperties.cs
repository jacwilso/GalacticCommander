using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    [Range(0, 100)]
    private int accuracy;
    [NonSerialized]
    public ModifiableStat<int> Accuracy;

    [SerializeField]
    private int damage;
    [NonSerialized]
    public ModifiableStat<int> Damage;

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

    public void OnAfterDeserialize()
    {
        Accuracy = new ModifiableStat<int>(accuracy);
        Damage = new ModifiableStat<int>(damage);
    }
}