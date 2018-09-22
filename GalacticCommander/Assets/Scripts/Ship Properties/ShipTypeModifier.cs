using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship Type Modifier")]

public class ShipTypeModifier : ScriptableObject
{
    [Range(0, 100)]
    public int front, back, left, right, top, bottom;
    public bool enableFront, enableBack, enableLeft, enableRight, enableTop, enableBottom;
}