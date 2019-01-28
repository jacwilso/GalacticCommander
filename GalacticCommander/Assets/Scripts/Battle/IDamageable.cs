using UnityEngine;

public interface IDamageable
{
    void Damaged(GotHitParams hit);
    void Death();
}

public struct GotHitParams
{
    public readonly int shieldDamage, hullDamage;
    public readonly Vector3 direction;
    public readonly GameObject assailant;

    public GotHitParams(int shieldDmg, int hullDmg, Vector3 dir, GameObject assail)
    {
        shieldDamage = shieldDmg;
        hullDamage = hullDmg;
        direction = dir;
        assailant = assail;
    }
}
