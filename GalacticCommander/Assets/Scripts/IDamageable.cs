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
}
