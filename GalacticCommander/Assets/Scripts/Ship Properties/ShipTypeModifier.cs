using UnityEngine;

[CreateAssetMenu(menuName = "Ship Properties/Ship Type Modifier")]

public class ShipTypeModifier : ScriptableObject
{
    public StatModifier this[int indx]
    {
        get
        {
            switch (indx)
            {
                case 0: return backStat;
                case 1: return leftStat;
                case 2: return bottomStat;
                case 3: return frontStat;
                case 4: return topStat;
                case 5: return rightStat;
                default:
                    Debug.LogError("ShipTypeModifier::[] cannot return StatModifier for that face");
                    return frontStat;
            }
        }
    }

    [MinValue(0)]
    public int[] modifier = new int[6];
    public bool[] enable = new bool[6];
    StatModifier frontStat, backStat, leftStat, rightStat, topStat, bottomStat;

    void OnEnable()
    {
        backStat = new StatModifier(modifier[0], StatModType.PercentAdd);
        leftStat = new StatModifier(modifier[1], StatModType.PercentAdd);
        bottomStat = new StatModifier(modifier[2], StatModType.PercentAdd);
        frontStat = new StatModifier(modifier[3], StatModType.PercentAdd);
        topStat = new StatModifier(modifier[4], StatModType.PercentAdd);
        rightStat = new StatModifier(modifier[5], StatModType.PercentAdd);
    }
}