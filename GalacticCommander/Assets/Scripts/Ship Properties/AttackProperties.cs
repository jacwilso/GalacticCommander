using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    public AttackStat this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return front;
                case 1: return back;
                case 2: return top;
                case 3: return bottom;
                case 4: return left;
                case 5: return right;
                default:
                    Debug.LogError("AttackProperties::[] out of bounds");
                    return front;
            }
        }
    }

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

    [SerializeField]
    public AccuracyCurve accuracy;

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

        [SerializeField]
        private Vector2Int[] damage;

        public int Accuracy => accuracy;
        [NonSerialized]
        public Vector2Int Damage;
        public void SumDamage()
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
        front.SumDamage();
        back.SumDamage();
        top.SumDamage();
        bottom.SumDamage();
        left.SumDamage();
        right.SumDamage();
    }
}