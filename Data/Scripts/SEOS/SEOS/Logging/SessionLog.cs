namespace SEOS.Core
{
    using Sandbox.ModAPI;
    using System;
    using System.IO;
    using VRage.Game.Components;

    /// <summary>
    /// Main session component for managing logging functionality.
    /// </summary>
    public partial class Session : MySessionComponentBase
    {
        /// <summary>
        /// Class responsible for handling logging operations.
        /// </summary>
        public class SessionLog
        {
            private static SessionLog _instance = null;
            private TextWriter _file = null;
            private string _fileName = "";

            /// <summary>
            /// Private constructor to enforce singleton pattern.
            /// </summary>
            private SessionLog() { }

            /// <summary>
            /// Gets the singleton instance of SessionLog.
            /// </summary>
            /// <returns>SessionLog instance</returns>
            private static SessionLog GetInstance()
            {
                if (SessionLog._instance == null)
                {
                    SessionLog._instance = new SessionLog();
                }

                return _instance;
            }

            /// <summary>
            /// Initializes the logging system.
            /// </summary>
            /// <param name="name">Name of the log file</param>
            /// <returns>True if initialization is successful, false otherwise</returns>
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

            /// <summary>
            /// Writes a line to the log file.
            /// </summary>
            /// <param name="text">Text to be written</param>
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
                    // Handle any exceptions that may occur during logging
                }
            }

            /// <summary>
            /// Writes characters to the log file without a newline.
            /// </summary>
            /// <param name="text">Text to be written</param>
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
                    // Handle any exceptions that may occur during logging
                }
            }

            /// <summary>
            /// Writes a clean line to the log file.
            /// </summary>
            /// <param name="text">Text to be written</param>
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
                    // Handle any exceptions that may occur during logging
                }
            }

            /// <summary>
            /// Closes the log file.
            /// </summary>
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
                    // Handle any exceptions that may occur during logging
                }
            }
        }
    }
}
