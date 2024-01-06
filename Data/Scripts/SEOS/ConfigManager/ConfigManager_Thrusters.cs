namespace SEOS.ConfigManager.Thrusters
{
    using System.Collections.Generic;
    using Sandbox.ModAPI;
    using SEOS.Core;
    using SEOS.Information;
    using SEOS.Network.Terminals.SerializedValues;
    using SEOS.Network.Thrusters.SeralizedValues;
    using VRageMath;

    internal static class SEOSThruster
    {
        public static void UpdateOutdatedThrusterConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);
            var _newEnforcement = new SEOSThrusterEnforcement_Values();

            //+ $"V{version}"

            if (MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(SEOSThrusterEnforcement_Values)))
            { MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(SEOSThrusterEnforcement_Values)); return; }

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + ".cfg", typeof(OldSEOSThrusterEnforcement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + ".cfg", typeof(OldSEOSThrusterEnforcement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<OldSEOSThrusterEnforcement_Values>(unPackCfg.ReadToEnd());

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
                MyAPIGateway.Utilities.DeleteFileInLocalStorage(name + ".cfg", typeof(OldSEOSThrusterEnforcement_Values));
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(_newEnforcement);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();

                Session.SessionLog.Line($"Updating config file for {name + $"V{version}" + ".cfg"} ");
                MyAPIGateway.Utilities.ShowMessage("Updating cfg", $"{name + $"V{version}" + ".cfg"}");
            }

        }

        public static void PrepSEOSThrusterConfigFile(string name, int version)
        {

            var _info = SEOSI.GetInfo(name);
            var _power = SEOSI.GetPower(name);
            var _speed = SEOSI.GetSpeed(name);
            var _effects = SEOSI.GetEffect(name);
            var _emissives = SEOSI.GetEmissives(name);

            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
                var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<SEOSThrusterEnforcement_Values>(unPackCfg.ReadToEnd());
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name] = unPackedData;

                if (unPackedData.UpdateConfig) unPackedData.Version += 1;
                if (unPackedData.Version == version) return;

                #region Info
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].info.Name = unPackedData.info.Name != "" ? unPackedData.info.Name : _info.Name;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].info.BlockClass = unPackedData.info.BlockClass != "" ? unPackedData.info.BlockClass : _info.BlockClass;
                #endregion

                #region Power
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].power.StrengthMultiplier = unPackedData.power.StrengthMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.StrengthMultiplier;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].power.PowerConsumptionMultiplier = unPackedData.power.PowerConsumptionMultiplier < 0 ? unPackedData.power.PowerConsumptionMultiplier : _power.PowerConsumptionMultiplier;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].power.BlockReplacementMultiplier = unPackedData.power.BlockReplacementMultiplier < 0 ? unPackedData.power.StrengthMultiplier : _power.BlockReplacementMultiplier;
                #endregion

                #region Speed

                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].speed.Max = unPackedData.speed.Max < 0f ? unPackedData.speed.Max : _speed.Max;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].speed.Min = unPackedData.speed.Min < 0f ? unPackedData.speed.Min : _speed.Min;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].speed.IncreaseAmmount = unPackedData.speed.IncreaseAmmount < 0 ? unPackedData.speed.IncreaseAmmount : _speed.IncreaseAmmount;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].speed.DecreaseAmmount = unPackedData.speed.DecreaseAmmount < 0 ? unPackedData.speed.DecreaseAmmount : _speed.DecreaseAmmount;
                #endregion

                #region Effects

                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].effects.soundMotor = unPackedData.effects.soundMotor != "" ? unPackedData.effects.soundMotor : _effects.soundMotor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].effects.soundArmature = unPackedData.effects.soundArmature != "" ? unPackedData.effects.soundArmature : _effects.soundArmature;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].effects.primaryEffect = unPackedData.effects.primaryEffect != "" ? unPackedData.effects.primaryEffect : _effects.primaryEffect;
                #endregion

                #region Emissives
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.VanillaEmissiveName = !unPackedData.emissives.VanillaEmissiveName.Equals("") ? unPackedData.emissives.VanillaEmissiveName : _emissives.VanillaEmissiveName;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.PowerEmissiveName = !unPackedData.emissives.PowerEmissiveName.Equals("") ? unPackedData.emissives.PowerEmissiveName : _emissives.PowerEmissiveName;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.ScaleEmissiveName = !unPackedData.emissives.ScaleEmissiveName.Equals("") ? unPackedData.emissives.ScaleEmissiveName : _emissives.ScaleEmissiveName;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.VanillaEmissiveColor = !unPackedData.emissives.VanillaEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.VanillaEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.PowerEmissiveColor = !unPackedData.emissives.PowerEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.PowerEmissiveColor : _emissives.PowerEmissiveColor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.ScaleEmissiveColor = !unPackedData.emissives.ScaleEmissiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ScaleEmissiveColor : _emissives.ScaleEmissiveColor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.ActiveColor = !unPackedData.emissives.ActiveColor.Equals(Color.Transparent) ? unPackedData.emissives.ActiveColor : _emissives.ActiveColor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.IdleColor = !unPackedData.emissives.IdleColor.Equals(Color.Transparent) ? unPackedData.emissives.IdleColor : _emissives.IdleColor;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].emissives.SuspendedColor = !unPackedData.emissives.SuspendedColor.Equals(Color.Transparent) ? unPackedData.emissives.SuspendedColor : _emissives.SuspendedColor;
                #endregion

                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].Version = version;
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name].UpdateConfig = false;

                unPackedData = null;
                unPackCfg.Close();
                unPackCfg.Dispose();
                var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
                var newData = MyAPIGateway.Utilities.SerializeToXML(Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name]);
                newCfg.Write(newData);
                newCfg.Flush();
                newCfg.Close();



                Session.SessionLog.Line($"Modifying config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Modify cfg", $"{name}");
            }
            else
            {
                SEOSThrusterEnforcement_Values tempEnforce = new SEOSThrusterEnforcement_Values();
                tempEnforce.info = SEOSI.GetInfo(name);
                tempEnforce.power = SEOSI.GetPower(name);
                tempEnforce.speed = SEOSI.GetSpeed(name);
                tempEnforce.effects = SEOSI.GetEffect(name);
                tempEnforce.emissives = SEOSI.GetEmissives(name);
                tempEnforce.Version = version;
                tempEnforce.UpdateConfig = false;
                if (Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement.Equals(null)) Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement = new Dictionary<string, SEOSThrusterEnforcement_Values>();
                Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement.Add(name, tempEnforce);

                var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
                var data = MyAPIGateway.Utilities.SerializeToXML(Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name]);
                cfg.Write(data);
                cfg.Flush();
                cfg.Close();

                Session.SessionLog.Line($"Generateing new config file for {name} ");
                Session.SessionLog.Line("----------------------------------------");
                MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{name + $"V{version}" }");
            }
        }

        public static void ReadSEOSThrusterConfigFile(string name, int version)
        {
            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
            Session.SessionLog.Line($"Reading {name + $"V{version}"  }.cfg");
            if (!dsCfgExists)
            {
                Session.SessionLog.Line($" {name + $"V{version}"}.cfg does not exist");
                return;
            }
            var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSThrusterEnforcement_Values));
            var data = MyAPIGateway.Utilities.SerializeFromXML<SEOSThrusterEnforcement_Values>(cfg.ReadToEnd());
            Session.SEOSThrusterEnforcement.SEOSThrusterEnforcement[name] = data;
            Session.SessionLog.Line($"Applying {name + $"V{version}"}.cfg settings ver:{data.Version}");
        }

    }

    internal static class Thrusters
    {


        public static void PrepThrusterConfig(string name, int version, SEOSTerminalSettings_Values values)
        {
            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSTerminalSettings_Values));
            if (dsCfgExists)
            {
                MyAPIGateway.Utilities.ShowMessage(name, $"\n Config Exists Canceling batch: ?");
                return;
            }
            var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSTerminalSettings_Values));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
            var data = MyAPIGateway.Utilities.SerializeToXML(values);
            MyAPIGateway.Utilities.ShowMessage(name, $"\n :" + data.ToString());
            cfg.Write(data);
            cfg.Flush();
            cfg.Close();

            Session.SessionLog.Line($"Generateing new config file for {name} ");
            Session.SessionLog.Line("----------------------------------------");
            MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{name + $"V{version}" }");
        }

        public static string GetThrusterConfig(string name, int version)
        {
            var data = "";
            var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSTerminalSettings_Values));
            if (dsCfgExists)
            {
                var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(name + $"V{version}" + ".cfg", typeof(SEOSTerminalSettings_Values));
                data = unPackCfg.ReadToEnd();
                //MyAPIGateway.Utilities.ShowMessage(name, $"\n Data:" + data);

            }
            return data;
        }
    }
}