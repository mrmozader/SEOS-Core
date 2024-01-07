
namespace SEOS.Core
{
    using SEOS.Information;
    using Sandbox.ModAPI;
    using System;

    public partial class Session
    {

        /// <summary>
        /// Executes security protocols based on the current state of the mod.
        /// Checks if the mod is approved and logs a corresponding message if true.
        /// Additionally, checks if debug mode is enabled, sets the Workshop ID accordingly, and logs the corresponding message. 
        /// </summary>
        /// <remarks>
        /// This method is responsible for executing security protocols based on the mod's state.
        /// It first checks if the mod is approved, using the IsModApproved() method. If approved,
        /// it logs a message indicating that the mod is in "Published Mode." Next, it checks if debug mode is enabled by calling the IsDebugModeEnabled() method.
        /// If debug mode is enabled, it sets the Workshop ID based on the player's Steam ID and logs a message indicating that the mod is in "Debug Mode."
        /// </remarks>
        /// <exception cref="Exception">Thrown if any errors occur during execution, and the details are logged using LogMessage().</exception>
        public void ExecuteSecurityProtocols()
        {
            try
            {
                // Check if the mod is approved and log the corresponding message.
                if (IsModApproved())
                {
                    LogMessage($"{Bot} Published Mode");
                }

                // Check if debug mode is enabled and set the Workshop ID accordingly, then log the corresponding message.
                if (IsDebugModeEnabled())
                {
                    SetWorkshopIdFromPlayer(MyAPIGateway.Session.Player.SteamUserId);
                    LogMessage($"{Bot} Debug Mode");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that may occur during execution.
                LogMessage($"Error in ExecuteSecurityProtocols: {ex.Message}");
            }
        }


        /// <summary>
        /// Checks if the mod is published on the Steam Workshop.
        /// </summary>
        /// <returns>True if the mod is published, otherwise false.</returns>
        public bool IsModPublished()
        {
            try
            {
                return MyAPIGateway.Session.WorkshopId.HasValue;
            }
            catch (Exception ex)
            {
                LogMessage($"Error in IsModPublished: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the mod is approved based on the list of allowed mods.
        /// </summary>
        /// <returns>True if the mod is approved, otherwise false.</returns>
        public bool IsModApproved()
        {
            try
            {
                foreach (var mod in MyAPIGateway.Session.Mods)
                {
                    if (SEOSI.GetModIDs().Contains(mod.PublishedFileId))
                    {
                        WorkshopId = mod.PublishedFileId;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogMessage($"Error in IsModApproved: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if debug mode is enabled based on the server type and player's admin status. Returns true if debug mode is enabled, otherwise false.
        /// </summary>
        /// <returns>True if debug mode is enabled, otherwise false. If the mod is running on a dedicated server,
        /// debug mode is disabled. If running on a server and the player is an admin, sets AdminLog to true. Logs any exceptions during execution.</returns>
        public bool IsDebugModeEnabled()
        {
            try
            {
                // If the mod is running on a dedicated server, debug mode is not enabled.
                if (DedicatedServer)
                    return false;

                // If the mod is running on a server and the player is an admin, set AdminLog to true and return true.
                if (IsServer && IsAdmin())
                {
                    AdminLog = true;
                    return true;
                }

                // If neither condition is met, debug mode is not enabled.
                return false;
            }
            catch (Exception ex)
            {
                // Log any exceptions that may occur during execution.
                LogMessage($"Error in IsDebugModeEnabled: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the current player is an admin.
        /// </summary>
        /// <returns>True if the player is an admin, otherwise false.</returns>
        public bool IsAdmin() => SEOSI.GetAdminIDs().Contains(MyAPIGateway.Session.Player.SteamUserId);

        /// <summary>
        /// Sets the workshop ID based on the player's admin status.
        /// </summary>
        /// <param name="devId">The player's Steam ID.</param>
        public void SetWorkshopIdFromPlayer(ulong devId)
        {
            WorkshopId = IsAdmin() ? SEOSI.GetAdmin(devId).ModId : devId;
            ShowMessage($"Set Workshop ID: {WorkshopId}" + (IsAdmin() ? " From Player" : ""));
        }

        /// <summary>
        /// Checks if the mod is compromised based on the current state, taking into account debug mode and approval status.
        /// If debug mode is enabled, it displays a welcome message to the player and sets the SecurityInit flag to true,
        /// indicating that the security has been initialized. In this case, the method returns false as the mod is not compromised.
        /// If debug mode is not enabled and the mod is not approved, it calls DisplayModStatus to show a detailed mod status message,
        /// and returns true, indicating that the mod is compromised. If neither of these conditions is met, it sets SecurityInit to true
        /// and returns false. The method is wrapped in a try-catch block to handle any potential exceptions during execution,
        /// logging an error message if an exception occurs and returning false.
        /// </summary>
        /// <returns>True if the mod is compromised, otherwise false.</returns>
        public bool IsModCompromised()
        {
            try
            {
                // Check if debug mode is enabled.
                if (IsDebugModeEnabled())
                {
                    // Display a welcome message and set the SecurityInit flag to true.
                    ShowMessage($"Welcome: {MyAPIGateway.Session.Player.DisplayName}");
                    SecurityInit = true;
                    return false;
                }

                // Check if the mod is not approved.
                if (!IsDebugModeEnabled() && !IsModApproved())
                {
                    // Display a detailed mod status message and return true indicating that the mod is compromised.
                    DisplayModStatus("Mod Is Compromised");
                    return true;
                }

                // If neither of the above conditions is met, set SecurityInit to true and return false.
                SecurityInit = true;
                return false;
            }
            catch (Exception ex)
            {
                // If an error occurs during execution, log an error message and return false.
                LogMessage($"Error in IsModCompromised: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Displays detailed information about the mod's status based on the specified message type.
        /// The method switches between different message types, including "Network," "Security," "Compromised,"
        /// and "Admin Message," to provide specific details about the mod's configuration and operational state.
        /// For each message type, the method utilizes ShowMessage to display the information in-game and LogMessage
        /// to log the details in the Space Engineers' log system. The method is wrapped in a try-catch block to handle
        /// any potential exceptions during execution, logging an error message if an exception occurs.
        /// </summary>
        /// <param name="messageType">The type of message to display.</param>
        public void DisplayModStatus(string messageType)
        {
            try
            {
                // Switch between different message types to display specific mod status details.
                switch (messageType)
                {
                    case "Network":
                        ShowMessage($"\n Debug Mode: {IsDebugModeEnabled()} - \n Compromised: {IsModCompromised()}");
                        LogMessage($"{Bot} - Mod ID: {WorkshopId} - Debug Mode: {IsDebugModeEnabled()} - Compromised: {IsModCompromised()}");
                        break;

                    case "Security":
                        ShowMessage($"\n Server: {IsServer} - \n Dedicated: {DedicatedServer} - \n MpActive: {MpActive}");
                        LogMessage($"{Bot} Server: {IsServer} - Dedicated: {DedicatedServer} - MpActive: {MpActive}");
                        break;

                    case "Compromised":
                        ShowMessage($" {MyAPIGateway.Session.Player.DisplayName} Unlicensed software detected");
                        ShowMessage("Please contact SEOS team to license a copy of this code for your mods");
                        ShowMessage("Disabling Internal logic");
                        break;

                    case "Admin Message":
                        DisplayAdminMessage();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during execution, log an error message.
                LogMessage($"Error in DisplayModStatus: {ex.Message}");
            }
        }


        /// <summary>
        /// Displays detailed information about the admin's configuration on the mission screen.
        /// This method retrieves data from the Admins and ModEnforcement dictionaries and formats it
        /// into a message, including the global log, mod ID, mod name, version, license number, admin role,
        /// admin initialization date, admin logging status, MP animation status, and animation visual distance.
        /// The formatted message is then displayed on the mission screen in the admin mode.
        /// In case of any errors during the execution, the method catches exceptions and logs an error message.
        /// </summary>
        private void DisplayAdminMessage()
        {
            try
            {
                // Retrieve admin and mod information from dictionaries.
                var admin = Admins[MyAPIGateway.Multiplayer.MyId];
                var mod = ModEnforcement;

                // Format the message with detailed admin and mod data.
                Message = $"\n[Global Log]: {mod.GlobalLog} " +
                          $"\n[Mod ID]: {admin.ModId} " +
                          $"\n[Mod Name]: {mod.ModName} " +
                          $"\n[Mod Version]: {mod.Version} " +
                          $"\n[Mod License #]: {mod.Liscense} " +
                          $"\n[Admin Role]: {admin.Role} " +
                          $"\n[Admin Init Date]: {admin.Established} " +
                          $"\n[Admin Logging]: {admin.Plog} " +
                          $"\n[Animate OS_Burners MP]: {mod.MpAnimate} " +
                          $"\n[Animation Visual Distance]: {mod.Vdist}";

                // Display the formatted message on the mission screen in admin mode.
                MyAPIGateway.Utilities.ShowMissionScreen(ModEnforcement.ModName, "Admin Mode:", $"{MyAPIGateway.Session.Player.DisplayName} Admin Config", Message, null, $"Continue");
            }
            catch (Exception ex)
            {
                // If an error occurs during execution, log an error message.
                LogMessage($"Error in DisplayAdminMessage: {ex.Message}");
            }
        }


        /// <summary>
        /// Displays a message using Space Engineers' in-game message system.
        /// </summary>
        /// <param name="message">The message to display.</param>
        private void ShowMessage(string message) => MyAPIGateway.Utilities.ShowMessage(Bot, message);

        /// <summary>
        /// Logs a message using the Space Engineers' log system.
        /// </summary>
        /// <param name="message">The message to log.</param>
        private void LogMessage(string message) => SessionLog.Line(message);


    }
}
