using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ability/Buff\u2215Debuff")]
public class StatAbility : AbilityProperties
{
    private enum StatType
    {
        Buff,
        Debuff
    }

    [Space(10)]
    [SerializeField]
    private StatType statType;
    [HideInInspector]
    public PropertyObject propertyObject;
    [HideInInspector]
    public string parameter;
    [SerializeField]
    private float effect;
    private float Effect;
    [SerializeField]
    [Tooltip("Number of turns the effect will last.")]
    private float duration;

    public void OnAfterDeserialize()
    {
        if (statType == StatType.Debuff)
        {
            Effect = -1f * effect;
        }
    }

    public override void Ability(Ship target)
    {
        throw new System.NotImplementedException();
    }
}

public enum PropertyObject
{
    Ship,
    Attack
}