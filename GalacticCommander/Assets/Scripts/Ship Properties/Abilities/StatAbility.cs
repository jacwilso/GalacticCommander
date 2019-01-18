using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ability/Buff\u2215Debuff")]
public class StatAbility : AbilityProperties
{
    enum StatType
    {
        Buff,
        Debuff
    }

    [Space(10)]
    [SerializeField]
    StatType statType = StatType.Buff;
    [HideInInspector]
    public PropertyObject propertyObject;
    [HideInInspector]
    public string parameter;
    [SerializeField]
    [Tooltip("Whether the effect should be flat added, percentage add, or percentage multiply.")]
    StatModType modifierType = StatModType.Flat;
    [SerializeField]
    float effect = 0;
    [NonSerialized]
    float Effect = 0;
    [SerializeField]
    [Tooltip("Number of turns the effect will last.")]
    float duration = 0;

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
        switch (propertyObject)
        {
            case PropertyObject.Ship:
                Stat modifyStat = (Stat)target.properties.GetType().GetField(parameter).GetValue(target.properties);
                modifyStat.AddModifier(new StatModifier(Effect, modifierType));
                break;
            //case PropertyObject.Attack:
            //    //TODO
            //    break;
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
    //Attack,
    Movement
}