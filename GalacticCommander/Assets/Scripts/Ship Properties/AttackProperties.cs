using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Attack")]
public class AttackProperties : ActionProperties
{
    [SerializeField]
    [Range(0, 100)]
    private int accuracy;
    [SerializeField]
    private int damage;
    [SerializeField]
    [Range(-90f, 90f)]
    private float arc;
    public float Arc => arc;
    //[SerializeField]
    //private Transform weapon;
    //public Transform Weapon => weapon;
    [SerializeField]
    private AudioSource hitSFX, missSFX;
    public AudioSource HitSFX => hitSFX;
    public AudioSource MissSFX => missSFX;
}