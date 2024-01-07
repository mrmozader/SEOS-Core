using System;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using System.IO;
using Sandbox.Game;
//using SEOS.Hardware;
using SEOS.Information;
using SEOS.Network.Esentials;
using SEOS.ConfigManager;
//using SEOS.ConfigManager.Burners;
//using SEOS.ConfigManager.Terminals;
//using SEOS.ConfigManager.Thrusters;
using Sandbox.Definitions;

namespace SEOS.Core
{


    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, int.MinValue)]
    public partial class Session : MySessionComponentBase
    {

        #region Simulation / Init
        public override void BeforeStart()
        {
            try
            {
                preInit();

                if (!DedicatedServer && IsServer)
                    Players.TryAdd(MyAPIGateway.Session.Player.IdentityId, MyAPIGateway.Session.Player);

                subscribeMessageHandler(true);
                subscribePlayerEvents(true);

                SecurityProtocols();

                if (Compromised())
                {
                    ModStatus("Security");
                    return;
                }
                else
                {
                    if(DevEnabled()) ModStatus("Network");
                }

                if (!DedicatedServer)
                    subscribeCustomControls(true);

                if (MpActive)
                {
                   syncDist(250000);
                }
                else
                {
                    syncDist(250000);
                }

                if (IsServer)
                {
                    SessionLog.Line($"{Bot} Updated Publisher Config");
                    ConfigUtils.PrepPublisherConfigFile(WorkshopId, ver);
                    ConfigUtils.ReadPublisherConfigFile(WorkshopId);
                    
                    foreach (string name in SEOSI.GetTerminalNames())
                    {
                        if (name.Equals("ROMBurner"))
                        {
                            SessionLog.Line($"{Bot} Updated SEOS Config");
                            //ROMBurner.UpdateOutdatedROMConfigFile(name, ver);
                            //ROMBurner.PrepROMBurnerConfigFile(name, ver);
                            //ROMBurner.ReadROMBurnerConfigFile(name, ver);
                        }
                        if (name.Equals("OSBurner"))
                        {
                            SessionLog.Line($"{Bot} Updated SEOS Config");

                            //OSBurner.UpdateOutdatedConfigFile(name, ver);
                            // OSBurner.PrepOSBurnerConfigFile(name, ver);
                            //OSBurner.ReadOSBurnerConfigFile(name, ver);
                        }
                        if (name.Equals("SEOSTerminal"))
                        {
                            SessionLog.Line($"{Bot} Updated SEOS Config");

                            //SEOSTerminal.UpdateOutdatedConfigFile(name, ver);
                            //SEOSTerminal.PrepTerminalConfigFile(name, ver);
                            //SEOSTerminal.ReadTerminalConfigFile(name, ver);
                        }
                        if (name.Equals("SEOSThruster"))
                        {
                            SessionLog.Line($"{Bot} Updated SEOS Config");

                            //SEOSThruster.UpdateOutdatedThrusterConfigFile(name, ver);
                            //SEOSThruster.PrepSEOSThrusterConfigFile(name, ver);
                            //SEOSThruster.ReadSEOSThrusterConfigFile(name, ver);
                        }

                    }

                    if (DevEnabled() && !IsPublished())
                    {
                        SessionLog.Line($"{Bot} Generateing Admin Config");
                        Admins.TryAdd(MyAPIGateway.Session.Player.SteamUserId, SEOSI.GetAdmin(MyAPIGateway.Session.Player.SteamUserId));

                        ConfigUtils.PrepAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId, ver); 
                         ConfigUtils.ReadAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId);
                    }
                }
                else
                {
                    RequestGlobalEnforcement(MyAPIGateway.Multiplayer.MyId);
                    //RequestOSBurnerEnforcement(MyAPIGateway.Multiplayer.MyId);
                    //RequestROMBurnerEnforcement(MyAPIGateway.Multiplayer.MyId);
                    //RequestSEOSTerminalEnforcement(MyAPIGateway.Multiplayer.MyId);
                    //RequestSEOSThrusterEnforcement(MyAPIGateway.Multiplayer.MyId);

                    if (!Admins.ContainsKey(MyAPIGateway.Multiplayer.MyId) && SEOSI.GetAdminIDs().Contains(MyAPIGateway.Multiplayer.MyId))
                    {
                        
                        Admin AdminEnforcement = new Admin();
                        AdminEnforcement.SenderId = MyAPIGateway.Multiplayer.MyId;
                        AdminEnforcement.Version = 0;
                        AdminEnforcement.Established = 0;
                        AdminEnforcement.ModId = 0;
                        AdminEnforcement.Role = 0;
                        AdminEnforcement.Plog = false;
                        Admins.TryAdd(MyAPIGateway.Multiplayer.MyId, AdminEnforcement);
                        if(Admins[MyAPIGateway.Multiplayer.MyId].SenderId == MyAPIGateway.Multiplayer.MyId)
                            SessionLog.Line($"{Bot} Generated Dummy Admin Config");
                    }
                }
            }
            catch (Exception ex) { SessionLog.Line($"Exception in BeforeStart: {ex}"); }
        }



        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {

        }
 
        public override void UpdateBeforeSimulation()
        {
            try
            {
                if(!SecurityInit) return;
                
                Tick = (uint)MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds / MyEngineConstants.UPDATE_STEP_SIZE_IN_MILLISECONDS;

                if (_count++ == 59)
                {
                    _count = 0;
                    _lCount++;
                    if (_lCount == 10)
                    {
                        _lCount = 0;
                        _eCount++;
                        if (!IsServer && !AdminEnforceInit && SEOSI.GetAdminIDs().Contains(MyAPIGateway.Multiplayer.MyId))
                            RequestAdminEnforcement(MyAPIGateway.Multiplayer.MyId);
                        


                        if (_eCount == 10)
                        {
                            if(!IsServer && AdminMessage)
                            {
                               ModStatus("Admin Message");
                                AdminMessage = false;
                            }
                            _eCount = 0;
                        }
                    }
                }
            }
            catch (Exception ex) { SessionLog.Line($"Exception in SessionBeforeSim: {ex}"); }
        }
        #endregion

        #region Player Events

        public void preInit()
        {

            SessionLog.Line($"{Bot} Initialize SEOS Client");
            SessionLog.Init(LogName + ".log");

            MpActive = MyAPIGateway.Multiplayer.MultiplayerActive;
            IsServer = MyAPIGateway.Multiplayer.IsServer;
            DedicatedServer = MyAPIGateway.Utilities.IsDedicated;

        }

        public void subscribeMessageHandler(bool sub)
        {
            if(sub)
            {
                MyAPIGateway.Multiplayer.RegisterMessageHandler(PACKET_ID, ReceivedPacket);
            }
            else
            {
                MyAPIGateway.Multiplayer.UnregisterMessageHandler(PACKET_ID, ReceivedPacket);

            }
        }

        public void subscribePlayerEvents(bool sub)
        {
            if (sub)
            {
                MyVisualScriptLogicProvider.PlayerDisconnected += PlayerDisconnected;
                MyVisualScriptLogicProvider.PlayerRespawnRequest += PlayerConnected;
            }
            else
            {
                MyVisualScriptLogicProvider.PlayerDisconnected -= PlayerDisconnected;
                MyVisualScriptLogicProvider.PlayerRespawnRequest -= PlayerConnected;
            }

        }

        public void subscribeCustomControls(bool sub)
        {
            if (sub)
            {
                MyAPIGateway.TerminalControls.CustomControlGetter += TerminalControls_CustomControlGetter;
                MyAPIGateway.TerminalControls.CustomActionGetter += TerminalControls_CustomActionGetter;
            }
            else
            {
                MyAPIGateway.TerminalControls.CustomControlGetter -= TerminalControls_CustomControlGetter;
                MyAPIGateway.TerminalControls.CustomActionGetter -= TerminalControls_CustomActionGetter;              
            }
        }

        public void syncDist(int dist)
        {
            SinkDist = MyAPIGateway.Session.SessionSettings.SyncDistance;
            SinkDistSqr = SinkDist * SinkDist;
            SinkBufferedDistSqr = SinkDistSqr + dist;
            SessionLog.Line($"{Bot} SinkDistSqr:{SinkDistSqr} - SinkBufferedDistSqr:{SinkBufferedDistSqr} - DistNorm:{SinkDist}");
        }

        private void  PlayerConnected(long id)
        {
            try
            {
                if (Players.ContainsKey(id))
                {
                    SessionLog.Line($"Player id({id}) already exists");
                    return;
                }
                MyAPIGateway.Multiplayer.Players.GetPlayers(null, myPlayer => FindPlayer(myPlayer, id));
            }
            catch (Exception ex) { SessionLog.Line($"Exception in PlayerConnected: {ex}"); }
        }

        private void PlayerDisconnected(long l)
        {
            try
            {
                IMyPlayer removedPlayer;

                Players.TryRemove(l, out removedPlayer);
                SessionLog.Line($"Removed player, new playerCount:{Players.Count}");
                AdminDisconnected(l);
            }
            catch (Exception ex) { SessionLog.Line($"Exception in PlayerDisconnected: {ex}"); }
        }
 
        private void AdminDisconnected(long l)
        {
            try
            {
                Admin admin;
                var steamId = MyAPIGateway.Players.TryGetSteamId(l);
                Admins.TryRemove(steamId, out admin);
                SessionLog.Line($"Removed admin, new adminCount:{Admins.Count}");
    
            }
            catch (Exception ex) { SessionLog.Line($"Exception in AdmDisconnected: {ex}"); }
        }

        private bool FindPlayer(IMyPlayer player, long id)
        {
            if (player.IdentityId == id)
            {
                Players[id] = player;
                if ( SEOSI.GetAdminIDs().Contains(player.SteamUserId) )
                {
                    if (ConfigUtils.LoadedOnlineAdminConfigFile(player.SteamUserId))
                    {
                        SessionLog.Line($"Added admin: {player.DisplayName}, new adminCount:{Admins.Count}");

                        SessionLog.Line(
                            $"\n Admin : {player.SteamUserId } " +
                            $"\n Mod Id: {Admins[player.SteamUserId].ModId } " +
                            $"\n Admin Log: {Admins[player.SteamUserId].Plog } " +
                            $"\n Admin Role: {Admins[player.SteamUserId].Role } " +
                            $"\n Admin Established: {Admins[player.SteamUserId].Established } " +
                            $"\n Sender Id: {Admins[player.SteamUserId].SenderId } " +
                            $"\n Version: {Admins[player.SteamUserId].Version } ");
                    }
                }
                SessionLog.Line($"Added player: {player.DisplayName}, new playerCount:{Players.Count}");
            } 
            return false;
        }

        #endregion
        
        #region Data Management
        public string ModPath()
        {
            var modPath = ModContext.ModPath;
            SessionLog.Line(modPath);
            return modPath;
        }

        public override void LoadData()
        {
            Instance = this;
        }

        protected override void UnloadData()
        {
            Instance = null;
            subscribeMessageHandler(false);
            subscribePlayerEvents(false);
            SessionLog.Line("Logging stopped.");
            SessionLog.Close();
        }
        #endregion
    }



}


