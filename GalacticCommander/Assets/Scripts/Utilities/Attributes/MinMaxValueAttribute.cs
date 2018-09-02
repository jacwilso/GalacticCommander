using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class MinMaxValueAttribute : PropertyAttribute
{
    public float minVal, maxVal;

    public MinMaxValueAttribute(int minVal, int maxVal)
    {
        this.minVal = minVal;
        this.maxVal = maxVal;
    }

    public MinMaxValueAttribute(float minVal, float maxVal)
    {
        this.minVal = minVal;
        this.maxVal = maxVal;
    }
}
