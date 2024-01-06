using Sandbox.ModAPI;
using System;
using System.IO;

namespace SEOS.Network.Base
{
    using ProtoBuf;
    using Sandbox.Game.Entities;
    using Sandbox.ModAPI;
    using VRage.Game.Entity;
    using SEOS.Network.Enforcement;


    [ProtoInclude(3, typeof(DataGlobalEnforce))]
    [ProtoInclude(4, typeof(DataAdminEnforce))]



    [ProtoContract]
    public abstract class PacketBase
    {
        [ProtoMember(1)] public ulong SenderId;

        [ProtoMember(2)] public long EntityId;

        private MyEntity _ent;

        internal MyEntity Entity
        {
            get
            {
                if (EntityId == 0) return null;

                if (_ent == null) _ent = MyEntities.GetEntityById(EntityId, true);

                if (_ent == null || _ent.MarkedForClose) return null;
                return _ent;
            }
        }

        public PacketBase(long entityId = 0)
        {
            SenderId = MyAPIGateway.Multiplayer.MyId;
            EntityId = entityId;
        }

        /// <summary>
        /// Called when this packet is received on this machine
        /// </summary>
        /// <param name="rawData">the bytes from the packet, useful for relaying or other stuff without needing to re-serialize the packet</param>
        public abstract bool Received(bool isServer);
    }

}

namespace SEOS.Network
{
    public class NetworkLog
    {
        private static NetworkLog _instance = null;
        private TextWriter _file = null;
        private string _fileName = "";

        private NetworkLog()
        {
        }

        private static NetworkLog GetInstance()
        {
            if (NetworkLog._instance == null)
            {
                NetworkLog._instance = new NetworkLog();
            }

            return _instance;
        }

        public static bool Init(string name)
        {

            bool output = false;

            if (GetInstance()._file == null)
            {

                try
                {
                    MyAPIGateway.Utilities.ShowNotification(name, 5000);
                    GetInstance()._fileName = name;
                    GetInstance()._file = MyAPIGateway.Utilities.WriteFileInLocalStorage(name, typeof(NetworkLog));
                    output = true;
                }
                catch (Exception e)
                {
                    MyAPIGateway.Utilities.ShowNotification(e.Message, 5000);
                }
            }
            else
            {
                output = true;
            }

            return output;
        }

        public static void Line(string text)
        {
            try
            {
                if (GetInstance()._file != null)
                {
                    var time = $"{DateTime.Now:MM-dd-yy_HH-mm-ss-fff} - ";
                    GetInstance()._file.WriteLine(time + text);
                    GetInstance()._file.Flush();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void Chars(string text)
        {
            try
            {
                if (GetInstance()._file != null)
                {
                    GetInstance()._file.Write(text);
                    GetInstance()._file.Flush();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void CleanLine(string text)
        {
            try
            {
                if (GetInstance()._file != null)
                {
                    GetInstance()._file.WriteLine(text);
                    GetInstance()._file.Flush();
                }
            }
            catch (Exception e)
            {
            }
        }

        public static void Close()
        {
            try
            {
                if (GetInstance()._file != null)
                {
                    GetInstance()._file.Flush();
                    GetInstance()._file.Close();
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}

