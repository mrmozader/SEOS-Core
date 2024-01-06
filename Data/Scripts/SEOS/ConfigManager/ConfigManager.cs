

namespace SEOS.ConfigManager
{
    using Sandbox.ModAPI;
    using SEOS.Core;
    using SEOS.Information;
    using SEOS.Network.Esentials;

    internal static class ConfigUtils
    {

            public static void PrepPublisherConfigFile(ulong Id, int version)
            {
                var _Mod = SEOSI.GetMod(Id);

                var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(Id + ".cfg", typeof(Mod));
                if (dsCfgExists)
                {
                    var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(Id + ".cfg", typeof(Mod));
                    var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<Mod>(unPackCfg.ReadToEnd());
                    Session.ModEnforcement = unPackedData;
                    Session.SessionLog.Line("----------------------------------------");
                    Session.SessionLog.Line($"unPackedData is: {unPackedData}\nServEnforced are: {Session.ModEnforcement}");

                    if (Session.ModEnforcement.Version == version) return;
                    Session.SessionLog.Line($"Regenerating outdated config, file version: {unPackedData.Version} - current version: {version}");

                    Session.ModEnforcement.GlobalLog = unPackedData.GlobalLog;
                    Session.ModEnforcement.MpAnimate = unPackedData.MpAnimate;
                    Session.ModEnforcement.Liscense = !unPackedData.Liscense.Equals("") ? unPackedData.Liscense : _Mod.Liscense;
                    Session.ModEnforcement.ModName = !unPackedData.ModName.Equals("") ? unPackedData.ModName : _Mod.ModName;
                    Session.ModEnforcement.Vdist = !unPackedData.Vdist.Equals(0) ? unPackedData.Vdist : _Mod.Vdist;
                    Session.ModEnforcement.Version = version;

                    unPackedData = null;
                    unPackCfg.Close();
                    unPackCfg.Dispose();
                    var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(Id + ".cfg", typeof(Mod));
                    var newData = MyAPIGateway.Utilities.SerializeToXML(Session.ModEnforcement);
                    newCfg.Write(newData);
                    newCfg.Flush();
                    newCfg.Close();

                    Session.SessionLog.Line($"Modifying config file for {Id.ToString()} ");
                    Session.SessionLog.Line("----------------------------------------");
                    MyAPIGateway.Utilities.ShowMessage("Modify cfg", $"{Id}");
                }
                else
                {
                    Mod tempEnforce = new Mod();
                    tempEnforce = SEOSI.GetMod(Id);
                    tempEnforce.Version = version;
                    if (Session.ModEnforcement.Equals(null)) Session.ModEnforcement = new Mod();
                    Session.ModEnforcement = tempEnforce;

                    var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(Id + ".cfg", typeof(Mod));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
                    var data = MyAPIGateway.Utilities.SerializeToXML(Session.ModEnforcement);
                    cfg.Write(data);
                    cfg.Flush();
                    cfg.Close();

                    Session.SessionLog.Line($"Generateing new config file for {Id} ");
                    Session.SessionLog.Line("----------------------------------------");
                    MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{Id}");

                }
            }
            public static void ReadPublisherConfigFile(ulong Id)
            {
                var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(Id + ".cfg", typeof(Mod));
                Session.SessionLog.Line($"Reading {Id}.cfg");
                if (!dsCfgExists)
                {
                    Session.SessionLog.Line($" {Id}.cfg does not exist");
                    return;
                }
                var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(Id + ".cfg", typeof(Mod));
                var data = MyAPIGateway.Utilities.SerializeFromXML<Mod>(cfg.ReadToEnd());
                Session.ModEnforcement = data;
                Session.SessionLog.Line($"Applying {Id}.cfg settings ver:{data.Version}");
            }

            public static void PrepAdminConfigFile(ulong Id, int version)
            {
                Session.SessionLog.Line($"Attempt");

                var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(Id + ".cfg", typeof(Admin));
                if (dsCfgExists)
                {
                    var _Admin = SEOSI.GetAdmin(Id);

                    var unPackCfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(Id + ".cfg", typeof(Admin));
                    var unPackedData = MyAPIGateway.Utilities.SerializeFromXML<Admin>(unPackCfg.ReadToEnd());
                    if (unPackedData != null)
                        Session.Admins.TryAdd(Id, unPackedData);
                    if (Session.Admins[Id].Version == version) return;
                    Session.SessionLog.Line($"Regenerating outdated config, file version: {unPackedData.Version} - current version: {version}");

                    Session.Admins[Id].Plog = unPackedData.Plog;
                    Session.Admins[Id].Role = unPackedData.Role < 0 ? unPackedData.Role : _Admin.Role;
                    Session.Admins[Id].Established = unPackedData.Established < 0 ? unPackedData.Established : _Admin.Established;
                    Session.Admins[Id].ModId = !unPackedData.ModId.Equals(0) ? unPackedData.ModId : _Admin.ModId;
                    Session.Admins[Id].Version = version;

                    unPackedData = null;
                    unPackCfg.Close();
                    unPackCfg.Dispose();
                    var newCfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(Id + ".cfg", typeof(Admin));
                    var newData = MyAPIGateway.Utilities.SerializeToXML(Session.Admins[Id]);
                    newCfg.Write(newData);
                    newCfg.Flush();
                    newCfg.Close();

                    Session.SessionLog.Line($"Modifying config file for {Id.ToString()} ");
                    Session.SessionLog.Line("----------------------------------------");
                    MyAPIGateway.Utilities.ShowMessage("Modify cfg", $"{Id}");
                }
                else
                {
                    Admin tempEnforce = new Admin();
                    tempEnforce = SEOSI.GetAdmin(Id);
                    tempEnforce.Version = version;
                    // Session.WeaponEnforce[name] = tempEnforce;
                    if (Session.Admins[Id].Equals(null)) Session.Admins[Id] = new Admin();
                    Session.Admins[Id] = tempEnforce;

                    var cfg = MyAPIGateway.Utilities.WriteFileInLocalStorage(Id + ".cfg", typeof(Admin));  //MyAPIGateway.Utilities.WriteFileInGlobalStorage(name + ".cfg");
                    var data = MyAPIGateway.Utilities.SerializeToXML(Session.Admins[Id]);
                    cfg.Write(data);
                    cfg.Flush();
                    cfg.Close();

                    Session.SessionLog.Line($"Generateing new config file for {Id} ");
                    Session.SessionLog.Line("----------------------------------------");
                    MyAPIGateway.Utilities.ShowMessage("Create cfg", $"{Id}");
                }
            }

            public static void ReadAdminConfigFile(ulong Id)
            {
                var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(Id + ".cfg", typeof(Admin));
                Session.SessionLog.Line($"Reading {Id}.cfg");
                if (!dsCfgExists)
                {
                    MyAPIGateway.Utilities.ShowMessage(Session.Bot, $"\n Config:{Id}.cfg does not exist");
                    Session.SessionLog.Line($" {Id}.cfg does not exist");
                    return;
                }
                var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(Id + ".cfg", typeof(Admin));
                var data = MyAPIGateway.Utilities.SerializeFromXML<Admin>(cfg.ReadToEnd());
                Session.Admins.TryAdd(Id, data);
                Session.SessionLog.Line($"Applying {Id}.cfg settings ver:{data.Version}");
            }

            public static bool LoadedOnlineAdminConfigFile(ulong Id)
            {
                var dsCfgExists = MyAPIGateway.Utilities.FileExistsInLocalStorage(Id + ".cfg", typeof(Admin));
                if (!dsCfgExists)
                {

                    Session.SessionLog.Line($" {Id}.cfg does not exist");
                    return false;
                }
                var cfg = MyAPIGateway.Utilities.ReadFileInLocalStorage(Id + ".cfg", typeof(Admin));
                var data = MyAPIGateway.Utilities.SerializeFromXML<Admin>(cfg.ReadToEnd());
                if (Session.Admins.ContainsKey(Id))
                {
                    Admin admin = new Admin();
                    Session.Admins.TryGetValue(Id, out admin);
                    if (admin != null)
                    {
                        admin.Established = data.Established;
                        admin.ModId = data.ModId;
                        admin.Plog = data.Plog;
                        admin.Role = data.Role;
                        Session.SessionLog.Line($" {Id}.cfg merged with config ");
                    }
                }
                else
                    Session.Admins.TryAdd(Id, data);
                Session.SessionLog.Line($" {Id}.cfg Loaded by config ");

                return true;
            }
       
    }
}



