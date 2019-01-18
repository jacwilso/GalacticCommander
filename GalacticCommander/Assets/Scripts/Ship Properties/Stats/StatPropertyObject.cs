using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class StatPropertyObject : ScriptableObject, ISerializationCallbackReceiver
{
    public virtual void OnAfterDeserialize()
    {
        this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(param => typeof(Stat).IsAssignableFrom(param.FieldType) && param.GetCustomAttribute(typeof(AssignableStatAttribute)) != null).ToList()
            .ForEach(param =>
            {
                Attribute.IsDefined(param, typeof(NonSerializedAttribute));
                string name = Char.ToLowerInvariant(param.Name[0]) + param.Name.Substring(1);
                FieldInfo info = this.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                {
                    param.SetValue(this, new Stat(Convert.ToSingle(info.GetValue(this))));
                }
                else
                {
                    param.SetValue(this, new Stat(0));
                    Debug.LogWarning($"StatPropertyObject::{param.Name} cannot be modified, no base name exists.");
                }
            });
    }

    public virtual void OnBeforeSerialize() { }
}
