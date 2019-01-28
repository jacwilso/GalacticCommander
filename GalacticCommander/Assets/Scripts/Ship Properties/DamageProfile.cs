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

    [SerializeField, Range(0, 100)]
    int[] modifier = new int[6];
    [SerializeField]
    bool[] enabled = { true, true, true, true, true, true };
    StatModifier front, back, left, right, top, bottom;

    void OnEnable()
    {
        back = new StatModifier(modifier[0], StatModType.PercentMult);
        left = new StatModifier(modifier[1], StatModType.PercentMult);
        bottom = new StatModifier(modifier[2], StatModType.PercentMult);
        front = new StatModifier(modifier[3], StatModType.PercentMult);
        top = new StatModifier(modifier[4], StatModType.PercentMult);
        right = new StatModifier(modifier[5], StatModType.PercentMult);
    }
}