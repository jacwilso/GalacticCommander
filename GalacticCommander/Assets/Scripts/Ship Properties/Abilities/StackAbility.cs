using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ability/Stack")]
public class StackAbility : AbilityProperties
{
    [Space(10)]
    [SerializeField]
    List<AbilityProperties> abilities = null;

    public override void Ability(Ship target)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] == this || abilities[i] == null)
                Debug.LogWarning("Stack ability, " + this.name + ", has an invalid item at index: " + i);
            else
                abilities[i].Ability(target);
        }
    }
}