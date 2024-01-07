namespace SEOS.Core
{
    using System;
    using Sandbox.ModAPI;
    using Sandbox.Game;

    public partial class Session
    {
        /// <summary>
        /// Updates server-related status variables.
        /// </summary>
        /// <remarks>
        /// This method retrieves and updates the multiplayer status, server status, and dedicated server status.
        /// </remarks>
        public void UpdateServerStatus()
        {
            try
            {
                // Retrieve multiplayer status, server status, and dedicated server status
                MpActive = MyAPIGateway.Multiplayer.MultiplayerActive;
                IsServer = MyAPIGateway.Multiplayer.IsServer;
                DedicatedServer = MyAPIGateway.Utilities.IsDedicated;
            }
            catch (Exception ex)
            {
                LogMessage($"Error in UpdateServerStatus: {ex.Message}");
            }
        }

        /// <summary>
        /// Update and synchronize distance-related calculations.
        /// </summary>
        /// <param name="additionalDistance">Additional distance value.</param>
        public void UpdateSyncDistanceCalculations(int additionalDistance)
        {
            // Update distance-related variables
            SinkDist = MyAPIGateway.Session.SessionSettings.SyncDistance;
            SinkDistSqr = SinkDist * SinkDist;
            SinkBufferedDistSqr = SinkDistSqr + additionalDistance;

            // Log the updated distances
            SessionLog.Line($"{Bot} SinkDistSqr:{SinkDistSqr} - SinkBufferedDistSqr:{SinkBufferedDistSqr} - DistNorm:{SinkDist}");
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