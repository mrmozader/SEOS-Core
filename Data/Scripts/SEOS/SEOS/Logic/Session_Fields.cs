namespace SEOS.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using VRage.Game.ModAPI;
    using VRage.ModAPI;
    using VRage.Utils;
    using SEOS.Network.Esentials;
    //using SEOS.Network.Burners.SeralizedValues;
    //using SEOS.Network.Terminals.SerializedValues;
    //using SEOS.Hardware.Burners;
    //using SEOS.Hardware.Terminals;
    //using SEOS.Hardware.Thrusters;
    using System.IO;
    using Sandbox.ModAPI;
    using Sandbox.Definitions;

    public partial class Session
    {

        
        internal MyStringId LabelStringId = new MyStringId();
        
        public uint Tick;
        private int _count = -1;
        private int _lCount;
        private int _eCount;

        internal const string Bot = "ZBOT:";
        internal const string LogName = "A .01 S.E.O.S.";
        internal string Message = "";
        public static int ver = 420;

        public DateTime Date = DateTime.Today;
        public DateTime Time = DateTime.Now;


        internal long LastTerminalId { get; set; }
        public IMyPlayer LocalPlayer { get; set; }
        internal bool BurnerAction { get; set; }
        internal bool ROMBurnerAction { get; set; }
        internal bool OSPadAction { get; set; }
        internal bool CoreAction { get; set; }
        internal bool TerminalAction { get; set; }
        internal string vanilla { get; set; }
        internal bool UplinkAction { get; set; }

        public MyStringHash BlankOS = MyStringHash.GetOrCompute("BlankOS");
        public MyStringHash BlankOSDisp = MyStringHash.GetOrCompute("Blank OS Pad");

        public MyStringHash ThrustOS = MyStringHash.GetOrCompute("ThrustOS");
        public MyStringHash ThrustOSDisp = MyStringHash.GetOrCompute("Thruster SEOS-OS");

        public MyStringHash BlankEPROM = MyStringHash.GetOrCompute("BlankEPROM");
        public MyStringHash BlankEPROMDisp = MyStringHash.GetOrCompute("Blank EPROM");

        public MyStringHash ThrusterEPROM = MyStringHash.GetOrCompute("ThrusterEPROM");
        public MyStringHash ThrusterEPROMDisp = MyStringHash.GetOrCompute("Thruster SEOS-OS EPROM");


        //internal readonly List<GridCores> GridCores = new List<GridCores>();
        //internal readonly ConcurrentDictionary<GridCores, bool> FunctionalCores = new ConcurrentDictionary<GridCores, bool>();
        //internal readonly List<ROM_Burners> ROM_Burners = new List<ROM_Burners>();
        //internal readonly List<OS_Burners> OS_Burners = new List<OS_Burners>();
        //internal readonly List<SEOS_Terminals> SEOS_Terminals = new List<SEOS_Terminals>();
        //internal readonly List<UpLinks> Uplinks = new List<UpLinks>();
        //internal readonly List<OS_Pads> OS_Pads = new List<OS_Pads>();
        internal bool AdminMessage { get; set; }
        internal bool AdminLog { get; set; }
        internal bool SecurityInit { get; set; }
        internal bool OSBurnerControl { get; set; }
        internal bool ROMBurnerControl { get; set; }

        internal bool GridCoreControl { get; set; }
        internal bool UplinkControl { get; set; }
        internal bool TerminalControl { get; set; }

        private double _syncDistSqr;

        public bool Enabled = true;
        public const ushort PACKET_ID = 49431;

        public readonly Guid SEOSThrusterState_Guid = new Guid("ee8e385d-37bb-42a9-bfb1-96e66bc61e6c");
        public readonly Guid SEOSThrusterSettings_Guid = new Guid("ceefe4d4-6b04-4edc-93d0-1d7631261911");

        public readonly Guid OSBurnerState_Guid = new Guid("4e3c0624-ab55-4a26-99e8-a38039fde656");
        public readonly Guid OSBurnerSettings_Guid = new Guid("bb8e7ffe-5c05-4e58-b589-eef0ccef7e66");

        public readonly Guid ROMBurnerState_Guid = new Guid("d2686842-f4a5-486f-8e77-46058bf9b11f");
        public readonly Guid ROMBurnerSettings_Guid = new Guid("0a28bbff-ab2d-4612-be67-f0dacbbaf719");

        public readonly Guid TerminalstateGuid = new Guid("2444211c-e89b-4119-b56c-e78df283de1e");
        public readonly Guid GSFTerminalsettingsGuid = new Guid("237771a7-a051-46b7-af64-a69fa0587f07");

       // public readonly Guid OSPadState_Guid = new Guid("a83296cc-c85e-4760-83be-a5f3d8c0a993");
       // public readonly Guid OSPadSettings_Guid = new Guid("3d1f3a0c-ba2d-4aa1-971d-e3d52c62bb8a");


        internal bool MpActive { get; set; }
        internal bool IsServer { get; set; }
        internal bool DedicatedServer { get; set; }
        internal ulong WorkshopId { get; set; }

        internal readonly ConcurrentDictionary<long, IMyPlayer> Players = new ConcurrentDictionary<long, IMyPlayer>();
        //internal static OSBurnerEnforced_Values OSBurnerEnforcement { get; set; } = new OSBurnerEnforced_Values();
        //internal static ROMBurnerEnforced_Values ROMBurnerEnforcement { get; set; } = new ROMBurnerEnforced_Values();
        //internal static SEOSTerminalEnforced_Values SEOSTerminalEnforcement { get; set; } = new SEOSTerminalEnforced_Values();

        //internal static Dictionary<string, SEOSTerminalSettings_Values> ConfigLibrary { get; set; } = new Dictionary<string, SEOSTerminalSettings_Values>();
        internal static Mod ModEnforcement { get; set; } = new Mod();

        //internal static Dictionary<string, List<SEOSConfig_Template>> BlockConfigLibrary { get; set; } = new Dictionary<string, List<SEOSConfig_Template>>();

        internal static ConcurrentDictionary<ulong, Admin> Admins = new ConcurrentDictionary<ulong, Admin>();
        internal static List<MyTerminalControlComboBoxItem> Configs = new List<MyTerminalControlComboBoxItem>();

        internal static bool ROMBurnerEnforceInit;
        internal static bool OSBurnerEnforceInit;
        internal static bool SEOSTerminalEnforceInit;
        internal static bool ModEnforceInit;
        internal static bool AdminEnforceInit;

        internal double SinkDistSqr { get; private set; }
        internal double SinkBufferedDistSqr { get; private set; }
        internal double SinkDist { get; private set; }

        internal static Session Instance { get; private set; }


        internal readonly HashSet<ulong> ApprovedMods = new HashSet<ulong>()
        {
            1701914678,
            1520968121,
        };

        internal readonly HashSet<ulong> ApprovedAdmins = new HashSet<ulong>()
        {
            76561198143476458,
            76561197972170961,
        };

        public class SessionLog
        {
            private static SessionLog _instance = null;
            private TextWriter _file = null;
            private string _fileName = "";

            private SessionLog()
            {
            }

            private static SessionLog GetInstance()
            {
                if (SessionLog._instance == null)
                {
                    SessionLog._instance = new SessionLog();
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
                        GetInstance()._file = MyAPIGateway.Utilities.WriteFileInLocalStorage(name, typeof(SessionLog));
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
}
