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
        /// Overrides the base class method for comprehensive initialization tasks before the game starts.
        /// Handles updates to server-related status, subscribes to multiplayer and player events, executes security protocols,
        /// checks for mod compromise and displays mod status, manages various setup tasks based on server type, including config handling,
        /// admin config generation in debug mode, and global enforcement for non-server instances. Additionally, deals with synchronization,
        /// custom control subscriptions, and admin enforcement requests. The method is designed for a robust and modular SEOS mod initialization.
        /// </summary>
        public override void BeforeStart()
        {
            try
            {
                // Subscribe to multiplayer message handler
                UpdateMessageHandlerSubscription(true);

                // Subscribe to player-related events
                UpdatePlayerEventsSubscription(true);

                // Additional setup tasks for non-dedicated servers
                if (!DedicatedServer) UpdateCustomControlsSubscription(true);

                // Execute security protocols and handle compromise situations
                ExecuteSecurityProtocols();

                // Check for mod compromise and display mod status accordingly
                CheckAndDisplayModStatus();

                // Additional setup tasks for servers
                AdditionalSetupTasksForServers();
            }
            catch (Exception ex)
            {
                // Log exceptions during BeforeStart initialization
                SessionLog.Line($"Exception in BeforeStart: {ex}");
            }
        }


        /// <summary>
        /// Overrides the base class method to initialize the Space Engineers Operating System (S.E.O.S.) log
        /// and configure initial server-related status. This method is crucial for setting up essential components
        /// before the game session begins. It starts by logging the initiation of the S.E.O.S. log, ensuring a record
        /// of significant events. The initialization then proceeds to configure server-related status variables,
        /// providing a foundation for monitoring and managing the server environment. Additionally, it establishes
        /// synchronization distance calculations based on the multiplayer status, ensuring optimal performance.
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

                // Initialize synchronization distance calculations based on multiplayer status
                InitializeSyncDistanceCalculations(250000);
            }
            catch (Exception ex)
            {
                // Log exceptions during initialization for troubleshooting
                LogMessage($"Exception in Init: {ex.Message}");
            }
        }



        /// <summary>
        /// This method overrides the base class to manage pre-simulation updates for the Space Engineers Operating System (SEOS) mod.
        /// It handles internal counters and triggers, including security checks, updating the tick count based on elapsed playtime, and
        /// managing admin enforcement requests. The method utilizes a time-based trigger system for efficient handling of periodic tasks,
        /// such as requesting admin enforcement, displaying admin messages, and resetting counters. Robust exception handling is implemented
        /// to log and report unexpected issues during the pre-simulation update process, ensuring the mod's functionality remains stable.
        /// </summary>
        public override void UpdateBeforeSimulation()
        {
            try
            {
                // Check if security is initialized before proceeding with updates
                if (!SecurityInit) return;

                // Update tick count based on elapsed play time
                Tick = (uint)(MyAPIGateway.Session.ElapsedPlayTime.TotalMilliseconds / MyEngineConstants.UPDATE_STEP_SIZE_IN_MILLISECONDS);

                // Increment counts and handle triggers
                if (_count++ == 59)
                {
                    _count = 0;
                    _lCount++;

                    // Execute action for the current count if it exists in the dictionary
                    Action action;
                    if (SecondTriggers.TryGetValue(_lCount, out action))
                    {
                        action.Invoke();
                    }

                    if (_lCount == 60)
                    {
                        _lCount = 0;
                        _eCount++;

                        // Execute action for the current count if it exists in the dictionary
                        Action hourAction;
                        if (HourTriggers.TryGetValue(_eCount, out hourAction))
                        {
                            hourAction.Invoke();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exceptions during the pre-simulation update for troubleshooting
                SessionLog.Line($"Exception in UpdateBeforeSimulation: {ex.Message}");
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


