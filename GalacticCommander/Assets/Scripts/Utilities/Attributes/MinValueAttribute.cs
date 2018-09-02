using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class MinValueAttribute : PropertyAttribute
{
    public float minVal;

    public MinValueAttribute(int minVal)
    {
        this.minVal = minVal;
    }

    public MinValueAttribute(float minVal)
    {
        this.minVal = minVal;
    }
}
