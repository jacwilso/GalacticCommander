#pragma warning disable 0649

using System;
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
    public Vector2Int[] Damage => damage;

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

    void Start()
    {
        SumDamage();
    }

    // TODO Remove
    void SumDamage()
    {
        Vector2Int dmg = Vector2Int.zero;
        var damageTypeEnum = Enum.GetValues(typeof(DamageType));
        for (int i = 0; i < damage.Length; i++)
        {
            dmg += damage[i] * DamageTypeEffect.HullEffect((DamageType)damageTypeEnum.GetValue(i));
        }
        hullDamage = dmg;
    }
}