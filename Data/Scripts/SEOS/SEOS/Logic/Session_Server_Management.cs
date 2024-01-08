namespace SEOS.Core
{
    using System;
    using Sandbox.ModAPI;
    using Sandbox.Game;
    using System.Collections.Generic;
    using SEOS.Information;
    using ProtoBuf.Meta;
    using Sandbox.ModAPI.Interfaces.Terminal;
    using SEOS.ConfigManager;
    using SEOS.Network.Esentials;

    public partial class Session
    {
        /// <summary>
        /// Initializes server-related status variables.
        /// </summary>
        /// <remarks>
        /// This method retrieves and updates the multiplayer status, server status, and dedicated server status.
        /// </remarks>
        public void InitializeServerStatus()
        {
            try
            {
                // Retrieve multiplayer status, server status, and dedicated server status
                MpActive = MyAPIGateway.Multiplayer.MultiplayerActive;
                IsServer = MyAPIGateway.Multiplayer.IsServer;
                DedicatedServer = MyAPIGateway.Utilities.IsDedicated;

                // Log the updated status
                SessionLog.Line($"Multiplayer Active: {MpActive}, Is Server: {IsServer}, Dedicated Server: {DedicatedServer}");
            }
            catch (Exception ex)
            {
                LogMessage($"Error in InitializeServerStatus: {ex.Message}");
            }
        }

        /// <summary>
        /// Update and synchronize distance-related calculations.
        /// </summary>
        /// <param name="additionalDistance">Additional distance value.</param>
        public void InitializeSyncDistanceCalculations(int additionalDistance)
        {
            // Update distance-related variables
            SinkDist = MyAPIGateway.Session.SessionSettings.SyncDistance;
            SinkDistSqr = SinkDist * SinkDist;
            SinkBufferedDistSqr = SinkDistSqr + additionalDistance;

            // Log the updated distances
            SessionLog.Line($"{Bot} SinkDistSqr:{SinkDistSqr} - SinkBufferedDistSqr:{SinkBufferedDistSqr} - DistNorm:{SinkDist}");
        }

        public void InitializeConfigs(List<String> configs)
        {

            // Prepare and read configuration files for various terminal types
            foreach (var name in configs)
            {
                switch (name)
                {
                    case "ROMBurner":
                        SessionLog.Line($"{Bot} Initialized SEOS Config: {name}");
                        //ROMBurner.UpdateOutdatedROMConfigFile(name, ver);
                        //ROMBurner.PrepROMBurnerConfigFile(name, ver);
                        //ROMBurner.ReadROMBurnerConfigFile(name, ver);
                        break;


                    default:
                        // Generate admin configuration if debug mode is enabled and mod is not published
                        if (IsDebugModeEnabled() && !IsModPublished())
                        {
                            SessionLog.Line($"{Bot} Initialize Admin Config file");
                            Admins.TryAdd(MyAPIGateway.Session.Player.SteamUserId, SEOSI.GetAdmin(MyAPIGateway.Session.Player.SteamUserId));
                            ConfigUtils.PrepAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId, ver);
                            ConfigUtils.ReadAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId);
                        }
                        break;

                }
            }

        }

        public void InitializeAdmin()
        {
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
                if (Admins[MyAPIGateway.Multiplayer.MyId].SenderId == MyAPIGateway.Multiplayer.MyId)
                    SessionLog.Line($"{Bot} Generated Dummy Admin Config");
            }
        }

        /// <summary>
        /// Gets the mod path and logs it.
        /// </summary>
        /// <returns>The mod path.</returns>
        public string ModPath()
        {
            try
            {
                var modPath = ModContext.ModPath;
                SessionLog.Line(modPath);
                return modPath;
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in ModPath: {ex.Message}");
                return null;
            }
        }
    }
}