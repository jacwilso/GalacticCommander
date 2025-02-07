﻿using System;
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
                maxValue = value = CalculateStat();
            }
            return maxValue;
        }
    }

    public float Value
    {
        get
        {
            if (isDirty)
            {
                return MaxValue;
            }
            return value;
        }
        set { this.value = value; }
    }

    public List<StatModifier> StatModifiers
    {
        get { return statModifiers; }
    }

    float baseValue;
    readonly List<StatModifier> statModifiers = new List<StatModifier>();
    bool isDirty = true;
    float maxValue;
    float value;

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
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

    float CalculateStat()
    {
        float value = baseValue;
        float sumPercentAdd = 0;
        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            switch (mod.Type)
            {
                case StatModType.Flat:
                    value += mod.Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        value *= sumPercentAdd / 100f;
                        sumPercentAdd = 0;
                    }
                    break;
                case StatModType.PercentMult:
                    value *= mod.Value / 100f;
                    break;
            }
        }
        return (float)Math.Round(value, 4);
    }


    int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        }
        else if (a.Order > b.Order)
        {
            return 1;
        }
        return 0;
    }
}
