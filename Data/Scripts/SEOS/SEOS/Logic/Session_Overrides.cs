namespace SEOS.Core
{
    using Sandbox.ModAPI;
    using SEOS.ConfigManager;
    using SEOS.Information;
    using SEOS.Network.Esentials;
    using System;
    using VRage.Game;
    using VRage.Game.Components;

    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, int.MinValue)]
    public partial class Session : MySessionComponentBase
    {
        /// <summary>
        /// Overrides the base class method to perform a comprehensive set of initialization tasks before the game starts.
        /// This method is responsible for updating server-related status variables, subscribing to essential multiplayer and player events,
        /// executing security protocols, checking for mod compromise and displaying mod status, handling various setup tasks based on server type,
        /// including configuration file preparation and reading, generating admin configurations in debug mode, and ensuring global enforcement
        /// for non-server instances. Additionally, it manages synchronization distance calculations, custom control subscriptions,
        /// and admin enforcement requests. The method is designed to provide a robust and modular initialization process tailored to the needs
        /// and requirements of the Space Engineers Operating System (SEOS) mod.
        /// </summary>
        public override void BeforeStart()
        {
            try
            {
                // Update server-related status variables
                UpdateServerStatus();

                // Subscribe to multiplayer message handler
                UpdateMessageHandlerSubscription(true);

                // Subscribe to player-related events
                UpdatePlayerEventsSubscription(true);

                // Execute security protocols and handle compromise situations
                ExecuteSecurityProtocols();

                // Check for mod compromise and display mod status accordingly
                if (IsModCompromised())
                {
                    DisplayModStatus("Security");
                    return;
                }
                else
                {
                    // Display network-related mod status if debug mode is enabled
                    if (IsDebugModeEnabled()) DisplayModStatus("Network");
                }

                // Additional setup tasks for non-dedicated servers
                if (!DedicatedServer)
                    UpdateCustomControlsSubscription(true);

                // Update synchronization distance calculations based on multiplayer status
                UpdateSyncDistanceCalculations(250000);

                // Additional setup tasks for servers
                if (IsServer)
                {
                    SessionLog.Line($"{Bot} Updated Publisher Config");

                    // Prepare and read configuration files for various terminal types
                    foreach (string name in SEOSI.GetTerminalNames())
                    {
                        SessionLog.Line($"{Bot} Updated SEOS Config :{name}");
                        // Uncomment and complete the following lines for each terminal type
                        /*
                        if (name.Equals("ROMBurner"))
                        {
                            //ROMBurner.UpdateOutdatedROMConfigFile(name, ver);
                            //ROMBurner.PrepROMBurnerConfigFile(name, ver);
                            //ROMBurner.ReadROMBurnerConfigFile(name, ver);
                        }
                        if (name.Equals("OSBurner"))
                        {
                            //OSBurner.UpdateOutdatedConfigFile(name, ver);
                            //OSBurner.PrepOSBurnerConfigFile(name, ver);
                            //OSBurner.ReadOSBurnerConfigFile(name, ver);
                        }
                        if (name.Equals("SEOSTerminal"))
                        {
                            //SEOSTerminal.UpdateOutdatedConfigFile(name, ver);
                            //SEOSTerminal.PrepTerminalConfigFile(name, ver);
                            //SEOSTerminal.ReadTerminalConfigFile(name, ver);
                        }
                        if (name.Equals("SEOSThruster"))
                        {
                            //SEOSThruster.UpdateOutdatedThrusterConfigFile(name, ver);
                            //SEOSThruster.PrepSEOSThrusterConfigFile(name, ver);
                            //SEOSThruster.ReadSEOSThrusterConfigFile(name, ver);
                        }
                        */
                    }

                    // Generate admin configuration if debug mode is enabled and mod is not published
                    if (IsDebugModeEnabled() && !IsModPublished())
                    {
                        SessionLog.Line($"{Bot} Generating Admin Config");
                        Admins.TryAdd(MyAPIGateway.Session.Player.SteamUserId, SEOSI.GetAdmin(MyAPIGateway.Session.Player.SteamUserId));
                        ConfigUtils.PrepAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId, ver);
                        ConfigUtils.ReadAdminConfigFile(MyAPIGateway.Session.Player.SteamUserId);
                    }
                }
                else
                {
                    // Request global enforcement if not a server
                    RequestGlobalEnforcement(MyAPIGateway.Multiplayer.MyId);

                    // Generate dummy admin configuration if not already an admin
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
            }
            catch (Exception ex) { SessionLog.Line($"Exception in BeforeStart: {ex}"); }
        }

        /// <summary>
        /// Overrides the base class method to initialize the Space Engineers Operating System (S.E.O.S.) log.
        /// </summary>
        /// <param name="sessionComponent">The session component object builder.</param>
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            try
            {
                // Log initialization of S.E.O.S. log
                SessionLog.Line($"{Bot} Initialize S.E.O.S. Log");
                SessionLog.Init(LogName + ".log");
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in Init: {ex.Message}");
            }
        }

        /// <summary>
        /// Overrides the base class method to perform pre-simulation updates. Manages various counts and triggers,
        /// including checking for security initialization, updating the tick count, and handling admin enforcement requests.
        /// Displays admin messages and resets counters accordingly.
        /// </summary>
        public override void UpdateBeforeSimulation()
        {
            try
            {
                // Check if security is initialized
                if (!SecurityInit) return;

                // Update tick count based on elapsed play time
                Tick = (uint)MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds / MyEngineConstants.UPDATE_STEP_SIZE_IN_MILLISECONDS;

                // Increment counts and handle triggers
                if (_count++ == 59)
                {
                    _count = 0;
                    _lCount++;

                    // Handle 10-second triggers
                    if (_lCount == 10)
                    {
                        _lCount = 0;
                        _eCount++;

                        // Request admin enforcement if conditions are met
                        if (!IsServer && !AdminEnforceInit && SEOSI.GetAdminIDs().Contains(MyAPIGateway.Multiplayer.MyId))
                            RequestAdminEnforcement(MyAPIGateway.Multiplayer.MyId);

                        // Handle 10-minute triggers
                        if (_eCount == 10)
                        {
                            // Display admin message if set
                            if (!IsServer && AdminMessage)
                            {
                                DisplayModStatus("Admin Message");
                                AdminMessage = false;
                            }
                            _eCount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in UpdateBeforeSimulation: {ex.Message}");
            }
        }

        /// <summary>
        /// Overrides the base class method to load mod data.
        /// </summary>
        public override void LoadData()
        {
            try
            {
                Instance = this;
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in LoadData: {ex.Message}");
            }
        }

        /// <summary>
        /// Overrides the base class method to unload mod data.
        /// </summary>
        protected override void UnloadData()
        {
            try
            {
                Instance = null;
                UpdateMessageHandlerSubscription(false);
                UpdatePlayerEventsSubscription(false);
                SessionLog.Line("Logging stopped.");
                SessionLog.Close();
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in UnloadData: {ex.Message}");
            }
        }
    }
}


