namespace SEOS.Information
{
    using System.Collections.Generic;
    using VRageMath;
    using SEOS.Network.Esentials;
    using System;

    public  static partial class SEOSI
    {

        private static readonly Dictionary<string, Info> _info = new Dictionary<string, Info>
        {
 
            ["OSBurner"] = new Info { Name = "OS Burner", BlockClass = "NULL" },
            ["ROMBurner"] = new Info { Name = "ROM Burner", BlockClass = "NULL" },
            ["SEOSTerminal"] = new Info { Name = "SEOS Terminal", BlockClass = "NULL" },
            ["SEOSThruster"] = new Info { Name = "SEOS Thruster", BlockClass = "NULL" }

        };

        private static readonly Dictionary<string, Power> _Power = new Dictionary<string, Power>
        {

            ["OSBurner"] = new Power { StrengthMultiplier = 1.0f, PowerConsumptionMultiplier = 1.0f, BlockReplacementMultiplier = 1.0f },
            ["ROMBurner"] = new Power { StrengthMultiplier = 1.0f, PowerConsumptionMultiplier = 1.0f, BlockReplacementMultiplier = 1.0f },
            ["SEOSTerminal"] = new Power { StrengthMultiplier = 1.0f, PowerConsumptionMultiplier = 1.0f, BlockReplacementMultiplier = 1.0f },
            ["SEOSThruster"] = new Power { StrengthMultiplier = 1.0f, PowerConsumptionMultiplier = 1.0f, BlockReplacementMultiplier = 1.0f },


        };

        private static readonly Dictionary<string, Speed> _Speed = new Dictionary<string, Speed>
        {
 
            ["OSBurner"] = new Speed { Max = .15f, Min = 0f, IncreaseAmmount = .001f, DecreaseAmmount = .001f },
            ["ROMBurner"] = new Speed { Max = .15f, Min = 0f, IncreaseAmmount = .001f, DecreaseAmmount = .001f },
            ["SEOSTerminal"] = new Speed { Max = .15f, Min = 0f, IncreaseAmmount = .001f, DecreaseAmmount = .001f },
            ["SEOSThruster"] = new Speed { Max = .15f, Min = 0f, IncreaseAmmount = .001f, DecreaseAmmount = .001f },


        };

        private static readonly Dictionary<string, Effects> _Effects = new Dictionary<string, Effects>
        {
  
            ["OSBurner"] = new Effects { soundMotor = "Motor", soundArmature = "Spin", primaryEffect = "Sparks" },
            ["ROMBurner"] = new Effects { soundMotor = "Motor", soundArmature = "Spin", primaryEffect = "Sparks" },
            ["SEOSTerminal"] = new Effects { soundMotor = "Motor", soundArmature = "Spin", primaryEffect = "Sparks" },
            ["SEOSThruster"] = new Effects { soundMotor = "Motor", soundArmature = "Spin", primaryEffect = "Sparks" },

        };

        private static readonly Dictionary<string, Emissives> _Emissives = new Dictionary<string, Emissives>
        {
 
            ["OSBurner"] = new Emissives { VanillaEmissiveName = "Emissive", VanillaEmissiveColor = Color.Green, PowerEmissiveName = "Emissive", PowerEmissiveColor = Color.Green, ScaleEmissiveName = "ScaleEmissive", ScaleEmissiveColor = Color.Green, ActiveColor = Color.Green, IdleColor = Color.Yellow, SuspendedColor = Color.Blue },
            ["ROMBurner"] = new Emissives { VanillaEmissiveName = "Emissive", VanillaEmissiveColor = Color.Green, PowerEmissiveName = "Emissive", PowerEmissiveColor = Color.Green, ScaleEmissiveName = "ScaleEmissive", ScaleEmissiveColor = Color.Green, ActiveColor = Color.Green, IdleColor = Color.Yellow, SuspendedColor = Color.Blue },
            ["SEOSTerminal"] = new Emissives { VanillaEmissiveName = "Emissive", VanillaEmissiveColor = Color.Green, PowerEmissiveName = "Emissive", PowerEmissiveColor = Color.Green, ScaleEmissiveName = "ScaleEmissive", ScaleEmissiveColor = Color.Green, ActiveColor = Color.Green, IdleColor = Color.Yellow, SuspendedColor = Color.Blue },
            ["SEOSThruster"] = new Emissives { VanillaEmissiveName = "Emissive", VanillaEmissiveColor = Color.Green, PowerEmissiveName = "Emissive", PowerEmissiveColor = Color.Green, ScaleEmissiveName = "ScaleEmissive", ScaleEmissiveColor = Color.Green, ActiveColor = Color.Green, IdleColor = Color.Yellow, SuspendedColor = Color.Blue },

        };

        private static readonly Dictionary<ulong, Admin> _Admin = new Dictionary<ulong, Admin>
        {
            [76561199441414915] = new Admin { ModId = 2030761420, Established = 0, Plog = true, Role = 0 },
            [76561197972170961] = new Admin { ModId = 2030761420, Established = 0, Plog = true, Role = 0 },

        };

        private static readonly Dictionary<ulong, Mod> _Mod = new Dictionary<ulong, Mod>
        {
            
            [2030761420] = new Mod { Liscense = "NKFNQdYFF5uJiIueeVlrMvgeFpA9EPWt", ModName = "SEOS", GlobalLog = false, MpAnimate = true, Vdist = 500 },
            [1729063426] = new Mod { Liscense = "NKFNQdYFF5uJiIueeVlrMvgeFpA9EPWt", ModName = "SEOS DEV", GlobalLog = true, MpAnimate = true, Vdist = 500 },
            [1520968121] = new Mod { Liscense = "fD7D7jvum7I1BID5HUhi8I87Sl2r3Agy", ModName = "REBEL GAMES", GlobalLog = false, MpAnimate = true, Vdist = 500 },

        };

        private static readonly Dictionary<String, ModGuid> _Modguid = new Dictionary<String, ModGuid>
        {

        };

        public static List<string> GetTerminalNames()
        {
            List<string> list = new List<string>();
            foreach (var key in _info.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public static List<ulong> GetModIDs()
        {
            List<ulong> list = new List<ulong>();
            foreach (var key in _Mod.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public static List<ulong> GetAdminIDs()
        {
            List<ulong> list = new List<ulong>();
            foreach (var key in _Admin.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public static Emissives GetEmissives(string name)
        {
            return _Emissives.GetValueOrDefault(name);
        }

        public static Admin GetAdmin(ulong Id)
        {
            return _Admin.GetValueOrDefault(Id);
        }

        public static Mod GetMod(ulong Id)
        {
            return _Mod.GetValueOrDefault(Id);
        }

        public static Info GetInfo(string name)
        {
            return _info.GetValueOrDefault(name);
        }

        public static Power GetPower(string name)
        {
            return _Power.GetValueOrDefault(name);
        }

        public static Speed GetSpeed(string name)
        {
            return _Speed.GetValueOrDefault(name);
        }

        public static Effects GetEffect(string name)
        {
            return _Effects.GetValueOrDefault(name);
        }
    }
}