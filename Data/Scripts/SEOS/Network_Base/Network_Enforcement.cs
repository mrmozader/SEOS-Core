
namespace SEOS.Network.Enforcement
{
    using ProtoBuf;
    using Sandbox.ModAPI;
    using SEOS.Core;
    using SEOS.Network.Base;
    using SEOS.Network.Esentials;


    [ProtoContract]
    public class DataGlobalEnforce : PacketBase
    {
        public DataGlobalEnforce() { }
        [ProtoMember(1)] public Mod GlobalEnforcement = null;
        public DataGlobalEnforce(long entityId, Mod OSBurnerEnforcement) : base(entityId) { GlobalEnforcement = OSBurnerEnforcement; }

        public override bool Received(bool isServer)
        {
            if (!isServer)
            {
                Session.ModEnforcement = GlobalEnforcement;
                Session.ModEnforceInit = true;
                NetworkLog.Line("[Global OSBurnerEnforcement Received]");
                return false;
            }

            NetworkLog.Line($" Server Sending Global OSBurnerEnforcement: " +
            $" Version:{Session.ModEnforcement.Version}" +
            $" ID:{Session.ModEnforcement.SenderId}");

            var data = new DataGlobalEnforce(0, Session.ModEnforcement);
            var bytes = MyAPIGateway.Utilities.SerializeToBinary(data);
            MyAPIGateway.Multiplayer.SendMessageTo(Session.PACKET_ID, bytes, GlobalEnforcement.SenderId);
            return false;
        }
    }

    [ProtoContract]
    public class DataAdminEnforce : PacketBase
    {
        public DataAdminEnforce() { }
        [ProtoMember(1)] public Admin AdminEnforcement = null;
        public DataAdminEnforce(long entityId, Admin OSBurnerEnforcement) : base(entityId) { AdminEnforcement = OSBurnerEnforcement; }

        public override bool Received(bool isServer)
        {
            if (!isServer)
            {

                if (Session.Admins.ContainsKey(AdminEnforcement.SenderId))
                {
                    Admin temp = new Admin();
                    Session.Admins.TryRemove(AdminEnforcement.SenderId, out temp);
                    Session.Admins.TryAdd(AdminEnforcement.SenderId, AdminEnforcement);
                    Session.AdminEnforceInit = true;
                    Session.Instance.AdminMessage = true;
                    Session.Instance.AdminLog = true;
                    NetworkLog.Line(
                    $"[Admin OSBurnerEnforcement Received Admin entry already listed] {AdminEnforcement.SenderId}" +
                    $"\n Mod Id: {AdminEnforcement.ModId} " +
                    $"\n Admin Log: {AdminEnforcement.Plog } " +
                    $"\n Admin Role: {AdminEnforcement.Role } " +
                    $"\n Admin Established: {AdminEnforcement.Established } " +
                    $"\n Version: {AdminEnforcement.Version } ");
                }
                else
                {
                    Session.Admins.TryAdd(AdminEnforcement.SenderId, AdminEnforcement);
                    Session.AdminEnforceInit = true;
                    Session.Instance.AdminMessage = true;
                    Session.Instance.AdminLog = true;
                    NetworkLog.Line(
                    $"[Admin OSBurnerEnforcement Received] {AdminEnforcement.SenderId}" +
                    $"\n Mod Id: {AdminEnforcement.ModId} " +
                    $"\n Admin Log: {AdminEnforcement.Plog } " +
                    $"\n Admin Role: {AdminEnforcement.Role } " +
                    $"\n Admin Established: {AdminEnforcement.Established } " +
                    $"\n Version: {AdminEnforcement.Version } ");
                }


                return false;
            }

            NetworkLog.Line($"[Sending Admin OSBurnerEnforcement] Sender id {AdminEnforcement.SenderId}");
            var data = new DataAdminEnforce(0, Session.Admins[AdminEnforcement.SenderId]);
            var bytes = MyAPIGateway.Utilities.SerializeToBinary(data);
            MyAPIGateway.Multiplayer.SendMessageTo(Session.PACKET_ID, bytes, AdminEnforcement.SenderId);
            Session.AdminEnforceInit = true;
            return false;

        }
    }
}
