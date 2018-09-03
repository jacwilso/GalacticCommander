﻿using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    public AttackStat front, back, top, bottom, left, right;

    [SerializeField]
    private int requirement;
    public int Requirement => requirement;

    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType => weaponType;

    [SerializeField, EnumFlag]
    private ResistanceTypes damageTypes;
    public ResistanceTypes DamageTypes => damageTypes;

    [Header("SFX"), SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private AudioSource missSFX, fireSFX;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
    public AudioSource FireSFX => fireSFX;

    [System.Serializable]
    public struct AttackStat
    {
        [SerializeField]
        [Range(0, 100)]
        private int accuracy;
        [NonSerialized]
        public Stat Accuracy;

        [SerializeField]
        private Vector2Int[] damage;
        [NonSerialized]
        public Stat[] Damage;
    }
}