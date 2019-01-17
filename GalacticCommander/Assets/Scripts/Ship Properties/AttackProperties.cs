using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    int requirement;
    public int Requirement => requirement;

    [SerializeField]
    WeaponType weaponType;
    public WeaponType WeaponType => weaponType;

    [SerializeField, EnumFlag]
    ResistanceTypes damageTypes;
    public ResistanceTypes DamageTypes => damageTypes;

    [SerializeField]
    public AccuracyCurve curve;

    [SerializeField]
    [Range(0, 100)]
    int accuracy;

    [SerializeField]
    Vector2Int[] damage;

    public int Accuracy => accuracy;
    [NonSerialized]
    public Vector2Int Damage;

    [Header("SFX"), SerializeField]
    AudioSource hitSFX;
    [SerializeField]
    AudioSource missSFX, fireSFX;
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

    void OnEnable()
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