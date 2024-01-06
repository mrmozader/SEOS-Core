namespace SEOS.Network.Esentials 
{ 
    using ProtoBuf;
    using VRageMath;
    using System.ComponentModel;

    [ProtoContract]
    public class Info
    {
        [ProtoMember(1), DefaultValue("Name")] public string Name = string.Empty;
        [ProtoMember(2), DefaultValue("Class")] public string BlockClass = string.Empty;
        public override string ToString() { return "Block Info"; }
    }

    [ProtoContract]
    public class Effects
    {
        [ProtoMember(1), DefaultValue("Spin")] public string soundMotor;
        [ProtoMember(2), DefaultValue("Motor")] public string soundArmature;
        [ProtoMember(3), DefaultValue("Sparks")] public string primaryEffect;
        public override string ToString() { return ""; }
    }

    [ProtoContract]
    public class Speed
    {
        [ProtoMember(1)] public float Max = 0f;
        [ProtoMember(2)] public float Min = 0f;
        [ProtoMember(3)] public float IncreaseAmmount= 0f;
        [ProtoMember(4)] public float DecreaseAmmount = 0f;
        public override string ToString() { return ""; }
    }

    [ProtoContract]
    public class Emissives
    {
        [ProtoMember(1)] public Color ActiveColor;
        [ProtoMember(2)] public Color IdleColor;
        [ProtoMember(3)] public Color SuspendedColor;
        [ProtoMember(4)] public Color VanillaEmissiveColor;
        [ProtoMember(5)] public Color PowerEmissiveColor;
        [ProtoMember(6)] public Color ScaleEmissiveColor;
        [ProtoMember(7)] public string VanillaEmissiveName;
        [ProtoMember(8)] public string PowerEmissiveName;
        [ProtoMember(9)] public string ScaleEmissiveName;
        public override string ToString() { return ""; }
    }

    [ProtoContract]
    public class Power
    {
        [ProtoMember(1), DefaultValue(-1f)] public float StrengthMultiplier;
        [ProtoMember(2), DefaultValue(-1f)] public float PowerConsumptionMultiplier;
        [ProtoMember(3), DefaultValue(-1f)] public float BlockReplacementMultiplier;
        public override string ToString() { return ""; }
    }

    [ProtoContract]
    public class OverDrive
    {
        [ProtoMember(1)] public bool OverDriveToggleVisible = false;
        [ProtoMember(2)] public bool OverDriveToggleModifiable = false;
        [ProtoMember(3)] public bool OverDriveToggleEnabled = false;

        [ProtoMember(4)] public bool OverDriveSliderVisible = false;
        [ProtoMember(5)] public bool OverDriveSliderModifiable = false;
        [ProtoMember(6)] public bool OverDriveSliderEnabled = false;

        [ProtoMember(7)] public float OverDriveMax = 0f;
        [ProtoMember(8)] public float OverDriveMin = 0f;
        [ProtoMember(9)] public float OverDriveValue = 0f;
        public override string ToString() { return "OverDrive"; }
    }

    [ProtoContract]
    public class AutoBurn
    {
        [ProtoMember(1)] public bool AutoBurnToggleVisible = false;
        [ProtoMember(2)] public bool AutoBurnToggleModifiable = false;
        [ProtoMember(3)] public bool AutoBurnToggleEnabled = false;
        [ProtoMember(4)] public bool AutoBurnSliderVisible = false;
        [ProtoMember(5)] public bool AutoBurnSliderModifiable = false;
        [ProtoMember(6)] public bool AutoBurnSliderEnabled = false;
        [ProtoMember(7)] public float AutoBurnDurationMax = 0f;
        [ProtoMember(8)] public float AutoBurnDurationMin = 0f;
        [ProtoMember(9)] public float AutoBurnDurationValue = 0f;
        public override string ToString() { return "AutoBurn"; }
    }

    [ProtoContract]
    public class AfterBurner
    {
        [ProtoMember(1)] public bool AutoBurnButtonVisible = false;
        [ProtoMember(2)] public bool AutoBurnButtonModifiable = false;
        [ProtoMember(3)] public bool AutoBurnButtonEnabled = false;
        public override string ToString() { return "AutoBurn"; }
    }

    [ProtoContract]
    public class Admin
    {
        [ProtoMember(1)] public bool Plog = true;
        [ProtoMember(2)] public int Role = 0;
        [ProtoMember(3)] public long Established = 0;
        [ProtoMember(4)] public ulong ModId = 0;
        [ProtoMember(5)] public int Version = -1;
        [ProtoMember(6)] public ulong SenderId = 0;
        public override string ToString() { return ""; }
    }

    [ProtoContract]
    public class Mod
    {
        [ProtoMember(1)] public bool GlobalLog = false;
        [ProtoMember(2)] public bool MpAnimate  = true;
        [ProtoMember(3)] public string ModName = "";
        [ProtoMember(4)] public string Liscense = "";
        [ProtoMember(5)] public int Vdist = 250000;
        [ProtoMember(6)] public int Version = -1;
        [ProtoMember(7)] public ulong SenderId = 0;
        public override string ToString() { return ""; }
    }

}

