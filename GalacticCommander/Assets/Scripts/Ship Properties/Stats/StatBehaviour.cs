using System.Linq;
using System.Reflection;
using UnityEngine;

public class StatBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(param => typeof(ScriptableObject).IsAssignableFrom(param.FieldType)).ToList()
            .ForEach(param => {
                object o = param.GetValue(this);
                if (o != null)
                {
                    param.SetValue(this, Instantiate(o as ScriptableObject));
                }
            });
    }
}
