
namespace SEOS.Core
{
    using SEOS.Information;
    using Sandbox.ModAPI;

    public partial class Session
    {

        public void SecurityProtocols()
        {
            // Case: Live Mode 
            if (Approved())
            {
                SessionLog.Line($"{Bot} Published Mode ");
            }
                       
            // Case: Debug Mode Offline
            if (DevEnabled())
            {
                SetModId(MyAPIGateway.Session.Player.SteamUserId, true);
                SessionLog.Line($"{Bot} Debug Mode ");
            }
        }

        public bool IsPublished()
        {
            if (MyAPIGateway.Session.WorkshopId.HasValue)
            {
                return true;
            }
            return false;
        }

        public bool Approved()
        {
            foreach (var mod in MyAPIGateway.Session.Mods)
                if (SEOSI.GetModIDs().Contains(mod.PublishedFileId))
                {
                    WorkshopId = mod.PublishedFileId;
                    return true;
                }

            return false;
        }

        public bool DevEnabled()
        {
            if (DedicatedServer)
                return false;

            if (IsServer && SEOSI.GetAdminIDs().Contains(MyAPIGateway.Session.Player.SteamUserId))
            {
                AdminLog = true;
                return true;
            }

            else
                return false;
        }

        public bool AdminEnabled()
        {

            return false;
        }

        public void SetModId(ulong devId, bool fromPlayer)
        {
            if (fromPlayer)
            {
                WorkshopId = SEOSI.GetAdmin(devId).ModId;
                MyAPIGateway.Utilities.ShowMessage(Bot, $"Set WrokshopId:{WorkshopId} From Player");
            }
            else
            {
                WorkshopId = devId;
                MyAPIGateway.Utilities.ShowMessage(Bot, $"Set WrokshopId:{WorkshopId}");
            }
        }

        public bool Compromised()
        {

            if (DevEnabled())
            {
                MyAPIGateway.Utilities.ShowMessage(Bot, $"Welcome back: {MyAPIGateway.Session.Player.DisplayName}");
                SecurityInit = true;
                return false;
            }

            if(!DevEnabled() && !Approved())
            {
                ModStatus("Compromised");
                return true;
            }

            SecurityInit = true;
            return false;
        }

        public void ModStatus(string message)
        {
            switch(message)
            {
                case "Network":
                    MyAPIGateway.Utilities.ShowMessage(Bot, $"\n Dev Mode:{DevEnabled()} - \n Compromised:{Compromised()}");
                    SessionLog.Line($"{Bot} - Mod Id:{WorkshopId} - Dev Mode:{DevEnabled()} - Compromised:{Compromised()}");
                    break;

                case "Security":
                    MyAPIGateway.Utilities.ShowMessage(Bot, $"\n Server:{IsServer} - \n Dedicated:{DedicatedServer} - \n MpActive:{MpActive}");
                    SessionLog.Line($"{Bot} Server:{IsServer} - Dedicated:{DedicatedServer} - MpActive:{MpActive}");
                    break;

                case "Compromised":
                    MyAPIGateway.Utilities.ShowMessage(Bot, $" {MyAPIGateway.Session.Player.DisplayName} Unliscensed software detected ");
                    MyAPIGateway.Utilities.ShowMessage(Bot, "Please contact SEOS team to liscense a copy of this code for own mods");
                    MyAPIGateway.Utilities.ShowMessage(Bot, "Disableing Internal logic");
                    break;

                case "Admin Message":
                    var admin = Admins[MyAPIGateway.Multiplayer.MyId];
                    var mod = ModEnforcement;

                    Message = $"\n[Global Log]: " + mod.GlobalLog.ToString() +
                              $"\n[Mod Id]: " + admin.ModId.ToString() +
                              $"\n[Mod Name]: " + mod.ModName.ToString() +
                              $"\n[Mod Version]: " + mod.Version.ToString() +
                              $"\n[Mod Liscense #]: " + mod.Liscense.ToString() +
                              $"\n[Admin Role]: " + admin.Role.ToString() +
                              $"\n[Admin Init Date]: " + admin.Established.ToString() +
                              $"\n[Admin Logging]: " + admin.Plog.ToString() +
                              $"\n[Animate OS_Burners MP]: " + mod.MpAnimate.ToString() +
                              $"\n[Animation Visual Distance]: " + mod.Vdist.ToString();

                    MyAPIGateway.Utilities.ShowMissionScreen(ModEnforcement.ModName, "Admin Mode:", $"{MyAPIGateway.Session.Player.DisplayName} Admin Config", Message, null, $"Continue");
                   //Message = "";
                    break;

                default:
                    
                    break;
            }
        }

    }
}
