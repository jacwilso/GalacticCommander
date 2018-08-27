using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    private AttackStat front, back, top, bottom, left, right;

    [SerializeField]
    private int energy;
    public int Energy => energy;

    [SerializeField]
    private AudioSource hitSFX, missSFX, fireSFX;
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
        private int damage;
        [NonSerialized]
        public Stat Damage;
    }
}
    