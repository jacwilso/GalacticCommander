using System;
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
    [Tooltip("Whether the effect should be flat added, percentage add, or percentage multiply.")]
    private StatModType modifierType;
    [SerializeField]
    private float effect;
    [NonSerialized]
    private float Effect;
    [SerializeField]
    [Tooltip("Number of turns the effect will last.")]
    private float duration;

    public override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
        if (statType == StatType.Debuff)
        {
            Effect = -1f * effect;
        }
    }

    public override void Ability(Ship target)
    {
        switch(propertyObject)
        {
            case PropertyObject.Ship:
                Stat modifyStat = (Stat)target.properties.GetType().GetField(parameter).GetValue(target.properties);
                modifyStat.AddModifier(new StatModifier(Effect, modifierType));
                break;
            case PropertyObject.Attack:
                //TODO
                break;
            case PropertyObject.Movement:
                //TODO
                break;
        }
        throw new System.NotImplementedException();
    }
}

public enum PropertyObject
{
    Ship,
    Attack,
    Movement
}