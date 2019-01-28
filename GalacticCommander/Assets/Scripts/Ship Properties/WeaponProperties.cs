#pragma warning disable 0649

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Weapon")]
public class WeaponProperties : ActionProperties
{
    [SerializeField]
    WeaponType weaponType = WeaponType.Laser;
    public WeaponType WeaponType => weaponType;

    [SerializeField, EnumFlag]
    DamageType damageTypes = DamageType.Kinetic;
    public DamageType DamageTypes => damageTypes;

    [SerializeField, Range(0, 100)]
    int accuracy;
    public int Accuracy => accuracy;

    [SerializeField]
    Vector2Int[] damage = new Vector2Int[Enum.GetValues(typeof(DamageType)).Length];
    [NonSerialized]
    public Dictionary<DamageType, Vector2Int> Damage;

    Vector2Int hullDamage;
    public Vector2Int HullDamage => hullDamage;

    [Header("SFX")]
    [SerializeField]
    AudioSource hitSFX = null;
    [SerializeField]
    AudioSource missSFX = null;
    [SerializeField]
    AudioSource fireSFX = null;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
    public AudioSource FireSFX => fireSFX;

    protected override void OnEnable()
    {
        base.OnEnable();
        var types = Enum.GetValues(typeof(DamageType));
        Damage = new Dictionary<DamageType, Vector2Int>(types.Length);
        Vector2 dmg = Vector2Int.zero;
        for (int i = 0; i < types.Length; i++) {
            var type = (DamageType)types.GetValue(i);
            Damage.Add(type, damage[i]);
            dmg += new Vector2(damage[i].x, damage[i].y) * DamageTypeEffect.HullEffect(type);
        }
        hullDamage = new Vector2Int((int)dmg.x, (int)dmg.y);
    }
}