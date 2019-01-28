using UnityEngine;

public class WeaponDamageRange
{
    public Vector2 hullRange, shieldRange;

    public WeaponDamageRange()
    {
        hullRange = new Vector2();
        shieldRange = new Vector2();
    }

    public WeaponDamageRange(Vector2 hull, Vector2 shield)
    {
        hullRange = hull;
        shieldRange = shield;
    }

    public static WeaponDamageRange operator+(WeaponDamageRange a, WeaponDamageRange b)
    {
        return new WeaponDamageRange(
            a.hullRange + b.hullRange,
            a.shieldRange + b.shieldRange
        );
    }

    public static WeaponDamageRange operator*(WeaponDamageRange a, float b)
    {
        return new WeaponDamageRange(
            a.hullRange * b,
            a.shieldRange * b
        );
    }

    public float DamageSum()
    {
        return hullRange.x + hullRange.y + shieldRange.x + shieldRange.y;
    }

    public override string ToString() {
        return $"Shield: {shieldRange.x} - {shieldRange.y} = Hull: {hullRange.x} - {hullRange.y}";
    }
}