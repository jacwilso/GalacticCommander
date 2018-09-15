using UnityEngine;

public interface IDamageable
{
    void Damaged(GotHitParams hit);
    void Death();
}

public struct GotHitParams
{
    public int damage;
    public Vector3 direction;
    public GameObject assailant;

    public GotHitParams(int dmg, Vector3 dir, GameObject assail)
    {
        damage = dmg;
        direction = dir;
        assailant = assail;
    }
}
