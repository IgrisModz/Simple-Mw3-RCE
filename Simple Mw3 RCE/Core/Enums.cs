namespace Simple_Mw3_RCE.Core
{
    public enum MovementType
    {
        None = 0,
        Noclip = 1,
        UFO = 2,
        Freeze = 4,
    }

    public enum DamageFlag
    {
        None = 0x1000,
        GodMode = 0x1001,
        GodModeWithBlood = 0x1002,
        NoKnockback = 0x1008,
        GodModeNoKB = 0x1009,
        GodModeNoKBwB = 0x100A,
    }
}
