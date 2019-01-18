using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Damage Profile")]

public class DamageProfile : ScriptableObject
{
    public StatModifier this[int indx]
    {
        get
        {
            switch (indx)
            {
                case 0: return back;
                case 1: return left;
                case 2: return bottom;
                case 3: return front;
                case 4: return top;
                case 5: return right;
                default:
                    Debug.LogError("DamageProfile::[] cannot return StatModifier for that face");
                    return front;
            }
        }
    }

    [MinValue(0)]
    public int[] profile = new int[6];
    public bool[] enabled = { true, true, true, true, true, true };
    StatModifier front, back, left, right, top, bottom;

    void OnEnable()
    {
        back = new StatModifier(profile[0], StatModType.PercentAdd);
        left = new StatModifier(profile[1], StatModType.PercentAdd);
        bottom = new StatModifier(profile[2], StatModType.PercentAdd);
        front = new StatModifier(profile[3], StatModType.PercentAdd);
        top = new StatModifier(profile[4], StatModType.PercentAdd);
        right = new StatModifier(profile[5], StatModType.PercentAdd);
    }
}