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
        /// Checks for mod compromise and displays mod status accordingly.
        /// </summary>
        /// <remarks>
        /// This method first checks if the mod is compromised. If compromised, it displays security-related mod status
        /// and prioritizes addressing the security concern. If the mod is not compromised, it checks if debug mode is enabled.
        /// If debug mode is enabled, it displays network-related mod status. Additional code can be added for any other mod-related checks or actions.
        /// </remarks>
        private void CheckAndDisplayModStatus()
        {
            try
            {
                // Check if the mod is compromised
                if (IsModCompromised())
                {
                    // If compromised, display security-related mod status
                    DisplayModStatus("Security");

                    // Exit the method to prioritize addressing the security concern
                    return;
                }
                else
                {
                    // If not compromised, check if debug mode is enabled
                    if (IsDebugModeEnabled())
                    {
                        // If debug mode is enabled, display network-related mod status
                        DisplayModStatus("Network");
                    }
                    // No need for an else statement here as the method continues
                    // to execute when the mod is not compromised and debug mode is disabled
                }
                // Additional code can be added here for any other mod-related checks or actions
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions that occur during mod status checking
                LogMessage($"Error in CheckAndDisplayModStatus: {ex.Message}");
            }
        }


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

        /// <summary>
        /// Perform additional setup tasks for servers.
        /// </summary>
        /// <remarks>
        /// This method checks if the current instance is a server. If it is, it initializes configurations based on
        /// terminal names obtained from SEOSI. If not a server, it requests global enforcement using the player's ID
        /// and initializes admin if not already an admin. Any exceptions during the process are logged and handled.
        /// </remarks>
        private void AdditionalSetupTasksForServers()
        {
            try
            {
                // Additional setup tasks for servers
                if (IsServer)
                {
                    // Initialize configurations based on terminal names
                    InitializeConfigs(SEOSI.GetTerminalNames());
                }
                else
                {
                    // Request global enforcement if not a server
                    RequestGlobalEnforcement(MyAPIGateway.Multiplayer.MyId);

                    // Initialize admin if not already an admin
                    InitializeAdmin();
                }
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions that occur during additional setup tasks
                LogMessage($"Error in AdditionalSetupTasksForServers: {ex.Message}");
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