
namespace SEOS.Core
{
    using SEOS.Information;
    using Sandbox.ModAPI;
    using System;
    using SEOS.ConfigManager;
    using SEOS.Network.Esentials;
    using VRage.Game.ModAPI;

    public partial class Session
    {
        /// <summary>
        /// Handles the event when a player connects to the server.
        /// </summary>
        /// <param name="id">The Steam ID of the connected player.</param>
        private void PlayerConnected(long id)
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
            catch (Exception ex)
            {
                LogMessage($"Exception in PlayerConnected: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the event when a player disconnects from the server.
        /// </summary>
        /// <param name="l">The Steam ID of the disconnected player.</param>
        private void PlayerDisconnected(long l)
        {
            try
            {
                IMyPlayer removedPlayer;

                Players.TryRemove(l, out removedPlayer);
                SessionLog.Line($"Removed player, new playerCount:{Players.Count}");
                AdminDisconnected(l);
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in PlayerDisconnected: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the event when an admin disconnects from the server.
        /// </summary>
        /// <param name="l">The Steam ID of the disconnected admin.</param>
        private void AdminDisconnected(long l)
        {
            try
            {
                Admin admin;
                var steamId = MyAPIGateway.Players.TryGetSteamId(l);
                Admins.TryRemove(steamId, out admin);
                SessionLog.Line($"Removed admin, new adminCount:{Admins.Count}");

            }
            catch (Exception ex)
            {
                LogMessage($"Exception in AdminDisconnected: {ex.Message}");
            }
        }

        /// <summary>
        /// Finds and adds a player to the Players dictionary if not already present, and adds admin if the player is an admin.
        /// </summary>
        /// <param name="player">The player to be added.</param>
        /// <param name="id">The Steam ID of the player.</param>
        /// <returns>Always returns false.</returns>
        private bool FindPlayer(IMyPlayer player, long id)
        {
            try
            {
                if (player.IdentityId == id)
                {
                    Players[id] = player;
                    if (SEOSI.GetAdminIDs().Contains(player.SteamUserId))
                    {
                        if (ConfigUtils.LoadedOnlineAdminConfigFile(player.SteamUserId))
                        {
                            SessionLog.Line($"Added admin: {player.DisplayName}, new adminCount:{Admins.Count}");

                            SessionLog.Line(
                                $"\n Admin : {player.SteamUserId} " +
                                $"\n Mod Id: {Admins[player.SteamUserId].ModId} " +
                                $"\n Admin Log: {Admins[player.SteamUserId].Plog} " +
                                $"\n Admin Role: {Admins[player.SteamUserId].Role} " +
                                $"\n Admin Established: {Admins[player.SteamUserId].Established} " +
                                $"\n Sender Id: {Admins[player.SteamUserId].SenderId} " +
                                $"\n Version: {Admins[player.SteamUserId].Version} ");
                        }
                    }
                    SessionLog.Line($"Added player: {player.DisplayName}, new playerCount:{Players.Count}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Exception in FindPlayer: {ex.Message}");
            }
            return false;
        }
    }
}