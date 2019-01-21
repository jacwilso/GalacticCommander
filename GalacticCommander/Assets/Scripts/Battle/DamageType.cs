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
    public static readonly DamageType[] SHIELD_PRIORITY = {
        DamageType.Electromagnetic,
        DamageType.Thermal,
        DamageType.Kinetic,
        DamageType.Nuclear,
        DamageType.BioChemical,
    };

    public static int ShieldEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Kinetic:
                return -25;
            case DamageType.Thermal:
                return 25;
            case DamageType.Nuclear:
                return -50;
            case DamageType.Electromagnetic:
                return 50;
            case DamageType.BioChemical:
                return -100;
        }
        return 0;
    }

    public static int HullEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Kinetic:
                return 25;
            case DamageType.Thermal:
                return -25;
            case DamageType.Nuclear:
                return 50;
            case DamageType.Electromagnetic:
                return -50;
            case DamageType.BioChemical:
                return 100;
        }
        return 0;
    }
}