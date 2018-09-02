using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class MaxValueAttribute : PropertyAttribute
{
    public float maxVal;

    public MaxValueAttribute(int maxVal)
    {
        this.maxVal = maxVal;
    }

    public MaxValueAttribute(float maxVal)
    {
        this.maxVal = maxVal;
    }
}
