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

                // Initialize synchronization distance calculations based on multiplayer status
                InitializeSyncDistanceCalculations(250000);

                // Additional setup tasks for servers
                if (IsServer)
                {
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
            catch (Exception ex) { SessionLog.Line($"Exception in BeforeStart: {ex}"); }
        }

        /// <summary>
        /// Overrides the base class method to initialize the Space Engineers Operating System (S.E.O.S.) log
        /// and configure initial server-related status.
        /// </summary>
        /// <param name="sessionComponent">The session component object builder.</param>
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            try
            {
                // Log initialization of S.E.O.S. log
                SessionLog.Line($"{Bot} Initializing S.E.O.S. Log");
                SessionLog.Init(LogName + ".log");

                // Configure initial server-related status variables
                InitializeServerStatus();
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
                // Additional setup tasks for non-dedicated servers
                if (!DedicatedServer)
                UpdateCustomControlsSubscription(false);
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


