using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public float BaseValue
    {
        get { return baseValue; }
        set
        {
            baseValue = value;
            isDirty = true;
        }
    }

    public float MaxValue
    {
        get
        {
            if (isDirty)
            {
                isDirty = false;
                statValue = CalculateStat();
            }
            return statValue;
        }
    }

    public List<StatModifier> StatModifiers
    {
        get { return statModifiers; }
    }

    public float value;

    private float baseValue;
    private readonly List<StatModifier> statModifiers  = new List<StatModifier>();
    private bool isDirty;
    private float statValue;

    public Stat(float baseValue)
    {
        this.baseValue =  value = baseValue;
    }

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    public void RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
        }
    }

    public void RemoveAllModifiersFromSource(object source)
    {
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                statModifiers.RemoveAt(i);
            }
        }
    }

    private float CalculateStat()
    {
        float value = baseValue;
        float sumPercentAdd = 0;
        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            switch(mod.Type)
            {
                case StatModType.Flat:
                    value += mod.Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        value *= 1 + sumPercentAdd;
                    }
                    break;
                case StatModType.PercentMult:
                    value *= 1 + mod.Value;
                    break;
            }
            value += statModifiers[i].Value;
        }
        return (float)Math.Round(value, 4);
    }


    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        } else if (a.Order > b.Order)
        {
            return 1;
        }
        return 0;
    }
}
