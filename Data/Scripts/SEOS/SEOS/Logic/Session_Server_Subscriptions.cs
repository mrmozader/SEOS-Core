namespace SEOS.Core
{
    using System;
    using Sandbox.ModAPI;
    using Sandbox.Game;

    public partial class Session
    {
        /// <summary>
        /// Manages subscription to the multiplayer message handler.
        /// </summary>
        /// <param name="subscribe">Flag indicating whether to subscribe or unsubscribe.</param>
        /// <remarks>
        /// This method logs the subscription status and subscribes or unsubscribes from the multiplayer message handler based on the provided flag.
        /// </remarks>
        public void UpdateMessageHandlerSubscription(bool subscribe)
        {
            try
            {
                // Log the subscription status
                SessionLog.Line($"{Bot} Subscribe Message Handler: {subscribe}");

                // Subscribe or unsubscribe based on the provided flag
                if (subscribe)
                {
                    MyAPIGateway.Multiplayer.RegisterMessageHandler(PACKET_ID, ReceivedPacket);
                }
                else
                {
                    MyAPIGateway.Multiplayer.UnregisterMessageHandler(PACKET_ID, ReceivedPacket);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error in UpdateMessageHandlerSubscription: {ex.Message}");
            }
        }

        /// <summary>
        /// Manages subscription to player events.
        /// </summary>
        /// <param name="subscribe">Flag indicating whether to subscribe or unsubscribe.</param>
        public void UpdatePlayerEventsSubscription(bool subscribe)
        {
            // Log the subscription status
            SessionLog.Line($"{Bot} Subscribe PlayerEvents: {subscribe}");

            // Subscribe or unsubscribe based on the provided flag
            if (subscribe)
            {
                // Subscribe to player events
                MyVisualScriptLogicProvider.PlayerDisconnected += PlayerDisconnected;
                MyVisualScriptLogicProvider.PlayerRespawnRequest += PlayerConnected;

                // Add the player to the collection if not in dedicated server mode
                if (!DedicatedServer && IsServer)
                    Players.TryAdd(MyAPIGateway.Session.Player.IdentityId, MyAPIGateway.Session.Player);
            }
            else
            {
                // Unsubscribe from player events
                MyVisualScriptLogicProvider.PlayerDisconnected -= PlayerDisconnected;
                MyVisualScriptLogicProvider.PlayerRespawnRequest -= PlayerConnected;
            }
        }

        /// <summary>
        /// Manages subscription to custom controls.
        /// </summary>
        /// <param name="subscribe">Flag indicating whether to subscribe or unsubscribe.</param>
        public void UpdateCustomControlsSubscription(bool subscribe)
        {
            // Log the subscription status
            SessionLog.Line($"{Bot} Subscribe Custom Controls: {subscribe}");

            // Subscribe or unsubscribe based on the provided flag
            if (subscribe)
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
    }
}