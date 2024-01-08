namespace SEOS.Core
{
    using System;
    using Sandbox.ModAPI;
    using VRageMath;
    using SEOS.Network.Base;
    using SEOS.Network.Enforcement;

    /// <summary>
    /// Partial class representing the main session component responsible for managing various functionalities.
    /// </summary>
    public partial class Session
    {
        /// <summary>
        /// Requests global enforcement settings from the server.
        /// </summary>
        internal void RequestGlobalEnforcement(ulong requestorId)
        {
            try
            {
                ModEnforcement.SenderId = requestorId;
                ModEnforcement.Version = ver;
                var bytes = MyAPIGateway.Utilities.SerializeToBinary(new DataGlobalEnforce(0, ModEnforcement));
                MyAPIGateway.Multiplayer.SendMessageToServer(PACKET_ID, bytes, true);
            }
            catch (Exception ex) { SessionLog.Line($"Exception in RequestGlobalEnforcement: {ex}"); }
        }

        /// <summary>
        /// Requests admin enforcement settings from the server.
        /// </summary>
        internal void RequestAdminEnforcement(ulong requestorId)
        {
            try
            {
                Admins[requestorId].SenderId = requestorId;
                Admins[requestorId].Version = ver;

                var bytes = MyAPIGateway.Utilities.SerializeToBinary(new DataAdminEnforce(0, Admins[requestorId]));
                MyAPIGateway.Multiplayer.SendMessageToServer(PACKET_ID, bytes, true);
            }
            catch (Exception ex) { SessionLog.Line($"Exception in RequestAdminEnforcement: {ex}"); }
        }

        /// <summary>
        /// Sends a packet to all clients within range of the given functional block.
        /// </summary>
        internal void PacketizeToClientsInRange(IMyFunctionalBlock block, PacketBase packet)
        {
            try
            {
                var bytes = MyAPIGateway.Utilities.SerializeToBinary(packet);
                var localSteamId = MyAPIGateway.Multiplayer.MyId;
                foreach (var p in Players.Values)
                {
                    var id = p.SteamUserId;
                    if (id != localSteamId && id != packet.SenderId && Vector3D.DistanceSquared(p.GetPosition(), block.PositionComp.WorldAABB.Center) <= SinkBufferedDistSqr)
                        MyAPIGateway.Multiplayer.SendMessageTo(PACKET_ID, bytes, p.SteamUserId);
                }
            }
            catch (Exception ex) { SessionLog.Line($"Exception in PacketizeToClientsInRange: {ex}"); }
        }

        /// <summary>
        /// Processes a received packet and sends it to all clients within range.
        /// </summary>
        private void ReceivedPacket(byte[] rawData)
        {
            try
            {
                var packet = MyAPIGateway.Utilities.SerializeFromBinary<PacketBase>(rawData);
                if (packet.Received(IsServer) && packet.Entity != null)
                {
                    var localSteamId = MyAPIGateway.Multiplayer.MyId;
                    foreach (var p in Players.Values)
                    {
                        var id = p.SteamUserId;
                        if (id != localSteamId && id != packet.SenderId && Vector3D.DistanceSquared(p.GetPosition(), packet.Entity.PositionComp.WorldAABB.Center) <= SinkBufferedDistSqr)
                            MyAPIGateway.Multiplayer.SendMessageTo(PACKET_ID, rawData, p.SteamUserId);
                    }
                }
            }
            catch (Exception ex) { SessionLog.Line($"Exception in ReceivedPacket: {ex}"); }
        }
    }
}
