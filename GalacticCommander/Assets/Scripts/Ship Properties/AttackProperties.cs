#pragma warning disable 0649

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    int requirement;
    public int Requirement => requirement;

    [SerializeField]
    WeaponType weaponType = WeaponType.Laser;
    public WeaponType WeaponType => weaponType;

    [SerializeField, EnumFlag]
    DamageType damageTypes = DamageType.Kinetic;
    public DamageType DamageTypes => damageTypes;

    // [SerializeField]
    public AccuracyCurve curve;

    [SerializeField]
    [Range(0, 100)]
    int accuracy = 0;

    [SerializeField]
    Vector2Int[] damage = null;

    public int Accuracy => accuracy;
    [NonSerialized]
    public Vector2Int Damage;

    [Header("SFX"), SerializeField]
    AudioSource hitSFX;
    [SerializeField]
    AudioSource missSFX = null,
        fireSFX = null;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
    public AudioSource FireSFX => fireSFX;

    [System.Serializable]
    public struct AccuracyCurve
    {
        [SerializeField]
        int middleRange, steepness, offset;

        public float Calculate(float distance)
        {
            float rawOdds = (1f + middleRange) / ((1f + middleRange) + (1f + distance));
            return 1f / (1f + Mathf.Exp(rawOdds * steepness + offset));
        }
    }

    new void OnEnable()
    {
        SumDamage();
    }

    void SumDamage()
    {
        Vector2Int dmg = Vector2Int.zero;
        for (int i = 0; i < damage.Length; i++)
        {
            dmg += damage[i];
        }
        Damage = dmg;
    }
}