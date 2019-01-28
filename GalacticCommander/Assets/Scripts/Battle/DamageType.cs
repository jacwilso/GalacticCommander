public enum DamageType
{
    Kinetic = 1 << 0,
    Thermal = 1 << 1,
    Nuclear = 1 << 2,
    Electromagnetic = 1 << 3,
    BioChemical = 1 << 4,
}

public class DamageTypeEffect
{
    public static readonly DamageType[] SHIELD_PRIORITY_LIST = {
        DamageType.Electromagnetic,
        DamageType.Thermal,
        DamageType.Kinetic,
        DamageType.Nuclear,
        DamageType.BioChemical,
    };

    public static float ShieldEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Kinetic:
                return 0.75f;
            case DamageType.Thermal:
                return 1.25f;
            case DamageType.Nuclear:
                return 0.5f;
            case DamageType.Electromagnetic:
                return 1.5f;
            case DamageType.BioChemical:
                return 0;
        }
        return 0;
    }

    public static float HullEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Kinetic:
                return 1.25f;
            case DamageType.Thermal:
                return 0.75f;
            case DamageType.Nuclear:
                return 1.5f;
            case DamageType.Electromagnetic:
                return 0.5f;
            case DamageType.BioChemical:
                return 2;
        }
        return 0;
    }
}