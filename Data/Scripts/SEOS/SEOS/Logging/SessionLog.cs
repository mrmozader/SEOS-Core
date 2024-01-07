using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Components;

namespace SEOS.Core
{
    public partial class Session : MySessionComponentBase
    {
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
