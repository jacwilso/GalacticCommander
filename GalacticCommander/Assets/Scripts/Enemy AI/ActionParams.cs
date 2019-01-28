class ActionParams {}

class MoveParams : ActionParams {
    public readonly int units;
    // might be better to cache all directions?
    public MoveParams(int u) {
        units = u;
    }
    public override string ToString() {
        return base.ToString() + $" == Units: {units}";
    }
}

class WeaponParams : ActionParams {
    public readonly int weapon;
    public readonly int target;
    public readonly float accuracy;
    public readonly WeaponDamageRange range;
    public WeaponParams(int w, int t, float a, WeaponDamageRange rng) {
        weapon = w;
        target = t;
        accuracy = a;
        range = rng;
    }
    public override string ToString() {
        return base.ToString() + $" == Weapon: {weapon}, Target: {target}, Accuracy: {accuracy}, Range => {range}";
    }
}

// class AbilityParams : ActionParams {} // TODO