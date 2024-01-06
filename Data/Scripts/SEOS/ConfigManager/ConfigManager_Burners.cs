

namespace SEOS.ConfigManager.Burners
{
    using System.Collections.Generic;
    using Sandbox.ModAPI;
    using SEOS.Core;
    using SEOS.Information;
    using SEOS.Network.Burners.SeralizedValues;
    using SEOS.Network.Terminals.SerializedValues;
    using VRageMath;

    internal static class OSBurner
    {
        public static List<string> GetOSConfig(string name)
        {
            List<string> names = new List<string>();

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(SEOSTerminalSettings_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + ".cfg", typeof(SEOSTerminalSettings_Values));

            }
            return names;
        }

        public static void UpdateOutdatedConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);
            var _newEnforcement = new OSBurnerEnfocement_Values();

            //+ $"V{version}"

            if (MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(OSBurnerEnfocement_Values)))
            { MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(OSBurnerEnfocement_Values)); return; }

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(OldOSBurnerEnfocement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + ".cfg", typeof(OldOSBurnerEnfocement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<OldOSBurnerEnfocement_Values>(unPackCfg.ReadToEnd());

                if (unPackedData.info == null)
                    _newEnforcement.info = _info;
                else
                    _newEnforcement.info = unPackedData.info;

                if (unPackedData.power == null)
                    _newEnforcement.power = _power;
                else
                    _newEnforcement.power = unPackedData.power;

                if (unPackedData.speed == null)
                    _newEnforcement.speed = _speed;
                else
                    _newEnforcement.speed = unPackedData.speed;

                if (unPackedData.effects == null)
                    _newEnforcement.effects = _effects;
                else
                    _newEnforcement.effects = unPackedData.effects;

                _newEnforcement.emissives = _emissives;

                _newEnforcement.Version = unPackedData.Version;

                _newEnforcement.UpdateConfig = false;

                unPackedData = null;
                unPackCfg.Close();
                unPackCfg.Dispose();
                MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(OldOSBurnerEnfocement_Values));
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(_newEnforcement);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();

                Session.SessionLog.Line($"Updating config file for {name + $"V{version}" + ".cfg"} ");
                MyAPIGateway.Utilities.ShowMessage("Updating cfg", $"{name + $"V{version}" + ".cfg"}");
            }

        }

        public static void PrepOSBurnerConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<OSBurnerEnfocement_Values>(unPackCfg.ReadToEnd());
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name] = unPackedData;

                if (unPackedData.UpdateConfig) unPackedData.Version += 1;
                if (unPackedData.Version == version) return;

                #region Info
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].info.Name = unPackedData.info.Name != "" ? unPackedData.info.Name : _info.Name;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].info.BlockClass = unPackedData.info.BlockClass != "" ? unPackedData.info.BlockClass : _info.BlockClass;
                #endregion

                #region Power
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].power.StrengthMultiplier = unPackedData.power.StrengthMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.StrengthMultiplier;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].power.PowerConsumptionMultiplier = unPackedData.power.PowerConsumptionMultiplier < 0 ? unPackedData.power.PowerConsumptionMultiplier : _power.PowerConsumptionMultiplier;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].power.BlockReplacementMultiplier = unPackedData.power.BlockReplacementMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.BlockReplacementMultiplier;
                #endregion

                #region Speed

                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].speed.Max = unPackedData.speed.Max < 0f ? unPackedData.speed.Max : _speed.Max;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].speed.Min = unPackedData.speed.Min < 0f ? unPackedData.speed.Min : _speed.Min;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].speed.IncreaseAmmount = unPackedData.speed.IncreaseAmmount < 0 ? unPackedData.speed.IncreaseAmmount : _speed.IncreaseAmmount;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].speed.DecreaseAmmount = unPackedData.speed.DecreaseAmmount < 0 ? unPackedData.speed.DecreaseAmmount : _speed.DecreaseAmmount;
                #endregion

                #region Effects

                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].effects.soundMotor = unPackedData.effects.soundMotor != "" ? unPackedData.effects.soundMotor : _effects.soundMotor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].effects.soundArmature = unPackedData.effects.soundArmature != "" ? unPackedData.effects.soundArmature : _effects.soundArmature;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].effects.primaryEffect = unPackedData.effects.primaryEffect != "" ? unPackedData.effects.primaryEffect : _effects.primaryEffect;
                #endregion

                #region Emissives
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.VanillaEmissiveName = !unPackedData.emissives.VanillaEmissiveName.Equals("") ? unPackedData.emissives.VanillaEmissiveName : _emissives.VanillaEmissiveName;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.PowerEmissiveName = !unPackedData.emissives.PowerEmissiveName.Equals("") ? unPackedData.emissives.PowerEmissiveName : _emissives.PowerEmissiveName;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.ScaleEmissiveName = !unPackedData.emissives.ScaleEmissiveName.Equals("") ? unPackedData.emissives.ScaleEmissiveName : _emissives.ScaleEmissiveName;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.VanillaEmissiveColor = !unPackedData.emissives.VanillaEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.VanillaEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.PowerEmissiveColor = !unPackedData.emissives.PowerEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.PowerEmissiveColor : _emissives.PowerEmissiveColor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.ScaleEmissiveColor = !unPackedData.emissives.ScaleEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ScaleEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.ActiveColor = !unPackedData.emissives.ActiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ActiveColor : _emissives.ActiveColor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.IdleColor = !unPackedData.emissives.IdleColor.Equals(Color.Transparent) ? unPackedData.emissives.IdleColor : _emissives.IdleColor;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].emissives.SuspendedColor = !unPackedData.emissives.SuspendedColor.Equals(Color.Transparent) ? unPackedData.emissives.SuspendedColor : _emissives.SuspendedColor;
                #endregion

                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].Version = version;
                Session.OSBurnerEnforcement.OSBurnerEnforcement[name].UpdateConfig = false;

                unPackedData = null;
                unPackCfg.Close();
                unPackCfg.Dispose();
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(Session.OSBurnerEnforcement.OSBurnerEnforcement[name]);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();



                Session.SessionLog.Line($"Modifying config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Modify cfg", $"{name}");
            }
            else
            {
                OSBurnerEnfocement_Values tempEnforce = new OSBurnerEnfocement_Values();
                tempEnforce.info = SEOSI.GetInfo(name);
                tempEnforce.power = SEOSI.GetPower(name);
                tempEnforce.speed = SEOSI.GetSpeed(name);
                tempEnforce.effects = SEOSI.GetEffect(name);
                tempEnforce.emissives = SEOSI.GetEmissives(name);
                tempEnforce.Version = version;
                tempEnforce.UpdateConfig = false;
                if (Session.OSBurnerEnforcement.OSBurnerEnforcement.Equals(null)) Session.OSBurnerEnforcement.OSBurnerEnforcement = new Dictionary<string, OSBurnerEnfocement_Values>();
                Session.OSBurnerEnforcement.OSBurnerEnforcement.Add(name, tempEnforce);

                var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
                var data = MyAPIGateway.Utilities.SerializeToXML(Session.OSBurnerEnforcement.OSBurnerEnforcement[name]);
                cfg.Write(data);
                cfg.Flush();
                cfg.Close();

                Session.SessionLog.Line($"Generateing new config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{name + $"V{version}" }");
            }
        }

        public static void ReadOSBurnerConfigFile(string name, int version)
        {
            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
            Session.SessionLog.Line($"Reading {name + $"V{version}"  }.cfg");
            if (!dsCfgExists)
            {
                Session.SessionLog.Line($" {name + $"V{version}"}.cfg does not exist");
                return;
            }
            var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(OSBurnerEnfocement_Values));
            var data = MyAPIGateway.Utilities.SerializeFromXML<OSBurnerEnfocement_Values>(cfg.ReadToEnd());
            Session.OSBurnerEnforcement.OSBurnerEnforcement[name] = data;
            Session.SessionLog.Line($"Applying {name + $"V{version}"}.cfg settings ver:{data.Version}");
        }

    }

    internal static class ROMBurner
    {
        public static List<string> GetROMConfig(string name)
        {
            List<string> names = new List<string>();

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(SEOSTerminalSettings_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + ".cfg", typeof(SEOSTerminalSettings_Values));

            }
            return names;
        }

        public static void UpdateOutdatedROMConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);
            var _newEnforcement = new ROMBurnerEnfocement_Values();

            //+ $"V{version}"

            if (MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(ROMBurnerEnfocement_Values)))
            { MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(ROMBurnerEnfocement_Values)); return; }

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(OldROMBurnerEnfocement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + ".cfg", typeof(OldROMBurnerEnfocement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<OldROMBurnerEnfocement_Values>(unPackCfg.ReadToEnd());

                if (unPackedData.info == null)
                    _newEnforcement.info = _info;
                else
                    _newEnforcement.info = unPackedData.info;

                if (unPackedData.power == null)
                    _newEnforcement.power = _power;
                else
                    _newEnforcement.power = unPackedData.power;

                if (unPackedData.speed == null)
                    _newEnforcement.speed = _speed;
                else
                    _newEnforcement.speed = unPackedData.speed;

                if (unPackedData.effects == null)
                    _newEnforcement.effects = _effects;
                else
                    _newEnforcement.effects = unPackedData.effects;

                _newEnforcement.emissives = _emissives;

                _newEnforcement.Version = unPackedData.Version;

                _newEnforcement.UpdateConfig = false;

                unPackedData = null;
                unPackCfg.Close();
                unPackCfg.Dispose();
                MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(OldROMBurnerEnfocement_Values));
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(_newEnforcement);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();

                Session.SessionLog.Line($"Updating config file for {name + $"V{version}" + ".cfg"} ");
                MyAPIGateway.Utilities.ShowMessage("Updating cfg", $"{name + $"V{version}" + ".cfg"}");
            }

        }

        public static void PrepROMBurnerConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<ROMBurnerEnfocement_Values>(unPackCfg.ReadToEnd());
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name] = unPackedData;

                if (unPackedData.UpdateConfig) unPackedData.Version += 1;
                if (unPackedData.Version == version) return;

                #region Info
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].info.Name = unPackedData.info.Name != "" ? unPackedData.info.Name : _info.Name;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].info.BlockClass = unPackedData.info.BlockClass != "" ? unPackedData.info.BlockClass : _info.BlockClass;
                #endregion

                #region Power
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].power.StrengthMultiplier = unPackedData.power.StrengthMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.StrengthMultiplier;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].power.PowerConsumptionMultiplier = unPackedData.power.PowerConsumptionMultiplier < 0 ? unPackedData.power.PowerConsumptionMultiplier : _power.PowerConsumptionMultiplier;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].power.BlockReplacementMultiplier = unPackedData.power.BlockReplacementMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.BlockReplacementMultiplier;
                #endregion

                #region Speed

                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].speed.Max = unPackedData.speed.Max < 0f ? unPackedData.speed.Max : _speed.Max;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].speed.Min = unPackedData.speed.Min < 0f ? unPackedData.speed.Min : _speed.Min;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].speed.IncreaseAmmount = unPackedData.speed.IncreaseAmmount < 0 ? unPackedData.speed.IncreaseAmmount : _speed.IncreaseAmmount;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].speed.DecreaseAmmount = unPackedData.speed.DecreaseAmmount < 0 ? unPackedData.speed.DecreaseAmmount : _speed.DecreaseAmmount;
                #endregion

                #region Effects

                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].effects.soundMotor = unPackedData.effects.soundMotor != "" ? unPackedData.effects.soundMotor : _effects.soundMotor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].effects.soundArmature = unPackedData.effects.soundArmature != "" ? unPackedData.effects.soundArmature : _effects.soundArmature;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].effects.primaryEffect = unPackedData.effects.primaryEffect != "" ? unPackedData.effects.primaryEffect : _effects.primaryEffect;
                #endregion

                #region Emissives
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.VanillaEmissiveName = !unPackedData.emissives.VanillaEmissiveName.Equals("") ? unPackedData.emissives.VanillaEmissiveName : _emissives.VanillaEmissiveName;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.PowerEmissiveName = !unPackedData.emissives.PowerEmissiveName.Equals("") ? unPackedData.emissives.PowerEmissiveName : _emissives.PowerEmissiveName;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.ScaleEmissiveName = !unPackedData.emissives.ScaleEmissiveName.Equals("") ? unPackedData.emissives.ScaleEmissiveName : _emissives.ScaleEmissiveName;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.VanillaEmissiveColor = !unPackedData.emissives.VanillaEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.VanillaEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.PowerEmissiveColor = !unPackedData.emissives.PowerEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.PowerEmissiveColor : _emissives.PowerEmissiveColor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.ScaleEmissiveColor = !unPackedData.emissives.ScaleEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ScaleEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.ActiveColor = !unPackedData.emissives.ActiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ActiveColor : _emissives.ActiveColor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.IdleColor = !unPackedData.emissives.IdleColor.Equals(Color.Transparent) ? unPackedData.emissives.IdleColor : _emissives.IdleColor;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].emissives.SuspendedColor = !unPackedData.emissives.SuspendedColor.Equals(Color.Transparent) ? unPackedData.emissives.SuspendedColor : _emissives.SuspendedColor;
                #endregion

                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].Version = version;
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name].UpdateConfig = false;

                unPackedData = null;
                unPackCfg.Close();
                unPackCfg.Dispose();
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name]);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();



                Session.SessionLog.Line($"Modifying config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Modify cfg", $"{name}");
            }
            else
            {
                ROMBurnerEnfocement_Values tempEnforce = new ROMBurnerEnfocement_Values();
                tempEnforce.info = SEOSI.GetInfo(name);
                tempEnforce.power = SEOSI.GetPower(name);
                tempEnforce.speed = SEOSI.GetSpeed(name);
                tempEnforce.effects = SEOSI.GetEffect(name);
                tempEnforce.emissives = SEOSI.GetEmissives(name);
                tempEnforce.Version = version;
                tempEnforce.UpdateConfig = false;
                if (Session.ROMBurnerEnforcement.ROMBurnerEnforcement.Equals(null)) Session.ROMBurnerEnforcement.ROMBurnerEnforcement = new Dictionary<string, ROMBurnerEnfocement_Values>();
                Session.ROMBurnerEnforcement.ROMBurnerEnforcement.Add(name, tempEnforce);

                var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
                var data = MyAPIGateway.Utilities.SerializeToXML(Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name]);
                cfg.Write(data);
                cfg.Flush();
                cfg.Close();

                Session.SessionLog.Line($"Generateing new config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{name + $"V{version}" }");
            }
        }

        public static void ReadROMBurnerConfigFile(string name, int version)
        {
            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
            Session.SessionLog.Line($"Reading {name + $"V{version}"  }.cfg");
            if (!dsCfgExists)
            {
                Session.SessionLog.Line($" {name + $"V{version}"}.cfg does not exist");
                return;
            }
            var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(ROMBurnerEnfocement_Values));
            var data = MyAPIGateway.Utilities.SerializeFromXML<ROMBurnerEnfocement_Values>(cfg.ReadToEnd());
            Session.ROMBurnerEnforcement.ROMBurnerEnforcement[name] = data;
            Session.SessionLog.Line($"Applying {name + $"V{version}"}.cfg settings ver:{data.Version}");
        }
    }

}

