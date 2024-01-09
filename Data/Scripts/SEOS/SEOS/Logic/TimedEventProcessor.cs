namespace SEOS.Core
{
    using Sandbox.ModAPI;
    using SEOS.ConfigManager;
    using SEOS.Information;
    using SEOS.Network.Esentials;
    using System;
    using System.Collections.Generic;
    using VRage.Game;
    using VRage.Game.Components;

    /// <summary>
    /// The Session class extends MySessionComponentBase and manages pre-simulation updates in the Space Engineers Operating System (SEOS) mod.
    /// It includes dictionaries (SecondTriggers and HourTriggers) for time-based triggers and associated actions,
    /// providing an efficient way to execute custom actions at specific intervals.
    /// </summary>
    public partial class Session : MySessionComponentBase
    {
        /// <summary>
        /// Dictionaries containing time-based triggers and associated actions for the Space Engineers Operating System (SEOS) mod.
        /// These dictionaries are used to efficiently manage and execute custom actions during pre-simulation updates.
        /// </summary>
        readonly Dictionary<int, Action> SecondTriggers = new Dictionary<int, Action>
{
    { 10, () => HandleTrigger("10 seconds", Handle10SecondTrigger) },
    { 20, () => HandleTrigger("20 seconds", Handle20SecondTrigger) },
    { 30, () => HandleTrigger("30 seconds", Handle30SecondTrigger) },
    { 40, () => HandleTrigger("40 seconds", Handle40SecondTrigger) },
    { 50, () => HandleTrigger("50 seconds", Handle50SecondTrigger) },
    { 60, () => HandleTrigger("1 minute", Handle1MinuteTrigger) }
};

        /// <summary>
        /// Dictionaries containing time-based triggers and associated actions for longer intervals in SEOS mod.
        /// These dictionaries are utilized during pre-simulation updates for executing specific actions at designated intervals.
        /// </summary>
        readonly Dictionary<int, Action> HourTriggers = new Dictionary<int, Action>
{
    { 6, () => HandleTrigger("10 minutes", Handle10MinuteTrigger) },
    { 12, () => HandleTrigger("20 minutes", Handle20MinuteTrigger) },
    { 18, () => HandleTrigger("30 minutes", Handle30MinuteTrigger) },
    { 24, () => HandleTrigger("40 minutes", Handle40MinuteTrigger) },
    { 30, () => HandleTrigger("50 minutes", Handle50MinuteTrigger) },
    { 36, () => HandleTrigger("1 hour", Handle1HourTrigger) }
};



        /// <summary>
        /// Generic handler method for executing actions associated with time-based triggers in the SEOS mod.
        /// It logs the start and completion of handling the trigger, executes the provided action, and logs any exceptions encountered.
        /// </summary>
        /// <param name="trigger">The description of the trigger being handled.</param>
        /// <param name="action">The action to be executed when the trigger is activated.</param>
        static void HandleTrigger(string trigger, Action action)
        {
            try
            {
                // Log the start of handling the trigger
                SessionLog.Line($"Handling trigger: {trigger}");

                // Execute the provided action
                action.Invoke();

                // Log the completion of handling the trigger
                SessionLog.Line($"Handled trigger: {trigger}");
            }
            catch (Exception ex)
            {
                // Log any exceptions during trigger handling
                SessionLog.Line($"Exception while handling trigger {trigger}: {ex.Message}");
            }
        }


        // Example delegate methods for triggers
        static void Handle10SecondTrigger()
        {
            // Custom action for 10 seconds
            SessionLog.Line("Handling 10 seconds");

        }

        static void Handle20SecondTrigger()
        {
            // Custom action for 20 seconds
            SessionLog.Line("Handling 20-second trigger");
        }

        static void Handle30SecondTrigger()
        {
            // Custom action for 30 seconds
            SessionLog.Line("Handling 30-second trigger");
        }

        static void Handle40SecondTrigger()
        {
            // Custom action for 40 seconds
            SessionLog.Line("Handling 40-second trigger");
        }

        static void Handle50SecondTrigger()
        {
            // Custom action for 50 seconds
            SessionLog.Line("Handling 50-second trigger");
        }

        /// <summary>
        /// Handles the custom action triggered every 1 minute during the pre-simulation updates of the Space Engineers Operating System (SEOS) mod.
        /// This method checks if the current instance is not a server, if admin enforcement is not yet initialized, and if the current player ID is included
        /// in the list of admin IDs obtained from the SEOS Integration (SEOSI). If all conditions are met, it requests admin enforcement using the player ID.
        /// The action also logs the handling of the 1-minute trigger.
        /// </summary>
        static void Handle1MinuteTrigger()
        {
            // Check if the current instance is not a server, admin enforcement is not yet initialized, and the player ID is in the list of admin IDs
            if (!Instance.IsServer && !AdminEnforceInit && SEOSI.GetAdminIDs().Contains(MyAPIGateway.Multiplayer.MyId))
            {
                // Request admin enforcement using the player ID
                Instance.RequestAdminEnforcement(MyAPIGateway.Multiplayer.MyId);
            }

            // Log the handling of the 1-minute trigger
            SessionLog.Line("Handling 1-minute trigger");
        }


        // Add more delegate methods for other second triggers

        /// <summary>
        /// Handles the custom action triggered every 10 minutes during the pre-simulation updates of the Space Engineers Operating System (SEOS) mod.
        /// This method checks if the current instance is not a server and if an admin message is set. If both conditions are met, it displays the mod status
        /// with the label "Admin Message" and resets the admin message flag to prevent repetitive display. The action also logs the handling of the 10-minute trigger.
        /// </summary>
        static void Handle10MinuteTrigger()
        {
            // Check if the current instance is not a server and an admin message is set
            if (!Instance.IsServer && Instance.AdminMessage)
            {
                // Display the mod status with the "Admin Message" label
                Instance.DisplayModStatus("Admin Message");

                // Reset the admin message flag to prevent displaying it repeatedly
                Instance.AdminMessage = false;
            }

            // Log the handling of the 10-minute trigger
            SessionLog.Line("Handling 10-minute trigger");
        }


        static void Handle20MinuteTrigger()
        {
            // Custom action for 20 minutes
            SessionLog.Line("Handling 20-minute trigger");
        }

        static void Handle30MinuteTrigger()
        {
            // Custom action for 30 minutes
            SessionLog.Line("Handling 30-minute trigger");
        }

        static void Handle40MinuteTrigger()
        {
            // Custom action for 40 minutes
            SessionLog.Line("Handling 40-minute trigger");
        }

        static void Handle50MinuteTrigger()
        {
            // Custom action for 50 minutes
            SessionLog.Line("Handling 50-minute trigger");
        }

        static void Handle1HourTrigger()
        {
            // Custom action for 1 hour
            SessionLog.Line("Handling 1-hour trigger");
        }

        // Add more delegate methods for other hour triggers


    }
}
