//using System;
//
//[AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
//public class ModifiableStatAttribute : Attribute
//{
//    private dynamic value;

//    public dynamic Value
//    {
//        get { return value; }
//    }

//    public ModifiableStatAttribute()
//    {
//    }

//    public void Store(dynamic value)
//    {
//        this.value = value;
//    }
//}

public struct ModifiableStat<T>
{
    public T value;
    public T max;

    public ModifiableStat(T val)
    {
        this.max = this.value = val;
    }
}