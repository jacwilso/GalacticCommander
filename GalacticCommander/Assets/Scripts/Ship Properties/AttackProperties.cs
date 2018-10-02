using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    private int requirement;
    public int Requirement => requirement;

    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WeaponType => weaponType;

    [SerializeField, EnumFlag]
    private ResistanceTypes damageTypes;
    public ResistanceTypes DamageTypes => damageTypes;

    [SerializeField]
    public AccuracyCurve curve;

    [SerializeField]
    [Range(0, 100)]
    private int accuracy;

    [SerializeField]
    private Vector2Int[] damage;

    public int Accuracy => accuracy;
    [NonSerialized]
    public Vector2Int Damage;

    [Header("SFX"), SerializeField]
    private AudioSource hitSFX;
    [SerializeField]
    private AudioSource missSFX, fireSFX;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
    public AudioSource FireSFX => fireSFX;

    [System.Serializable]
    public struct AccuracyCurve
    {
        [SerializeField]
        private int middleRange, steepness, offset;

        public float Calculate(float distance)
        {
            float rawOdds = (1f + middleRange) / ((1f + middleRange) + (1f + distance));
            return 1f / (1f + Mathf.Exp(rawOdds * steepness + offset));
        }
    }

    private void OnEnable()
    {
        SumDamage();
    }

    // Function

    private void SumDamage()
    {
        Vector2Int dmg = Vector2Int.zero;
        for (int i = 0; i < damage.Length; i++)
        {
            dmg += damage[i];
        }
        Damage = dmg;
        //Debug.Log(Damage);
    }
}