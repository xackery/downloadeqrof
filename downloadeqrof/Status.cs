using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using YamlDotNet.Core.Tokens;

namespace ROF_Downloader
{



    enum StatusType
    {
        Download, // Download status
        StatusBar, // controls the bottom status bar text
    }

    /// <summary>
    /// StatusLibrary is used to manage the various statuses tracked in launcher and is thread safe
    /// </summary>
    internal class StatusLibrary
    {        
        private static readonly object mux = new object();
        private static readonly object uiMux = new object();

        readonly static Dictionary<StatusType, Status> checks = new Dictionary<StatusType, Status>();

        public delegate void ProgressHandler(int value);
        static event ProgressHandler progressChange;
        static int progressValue;

        public delegate void DescriptionHandler(string value);
        static event DescriptionHandler descriptionChange;

        static string lastEvent;
        static string currentEvent;
        static string scope;

        /// <summary>
        /// When the UI is locked/unlocked, cancellation is fired. This is a thread safe operation to access
        /// </summary>
        static CancellationTokenSource cancelTokenSource;

        public static void InitLog() 
        {
            lock (mux)
            {
                using (var logw = File.Create("downloadeqrof.log"))
                {
                    string dirName = new DirectoryInfo($"{Application.StartupPath}").Name;
                    string rawMessage = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")} INFO Download EQ RoF v{Assembly.GetEntryAssembly().GetName().Version} ({dirName} Folder)\n";
                    logw.Write(Encoding.ASCII.GetBytes(rawMessage), 0, rawMessage.Length);
                    logw.Flush();
                }
            }
        }

        public static void Log(string message)
        {
            lock (mux)
            {
                try
                {
                    using (var logw = File.Open("downloadeqrof.log", FileMode.Append))
                    {
                        string rawMessage = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff")} INFO {message}\n";
                        logw.Write(Encoding.ASCII.GetBytes(rawMessage), 0, rawMessage.Length);
                        logw.Flush();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log: {ex.Message}");
                }
                Console.WriteLine(message);
            }
        }


        public static Status Get(StatusType name)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                return checks[name];
            }
        }

        public static void Add(StatusType name, Status value)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                checks[name] = value;
            }
        }

        /// <summary>
        /// LockUI should be called before doing any Fix or Download operation
        /// </summary>
        public static void LockUI()
        {
            lock (uiMux)
            {
                if (cancelTokenSource == null)
                {
                    cancelTokenSource = new CancellationTokenSource();
                    return;
                }
            }
        }

        /// <summary>
        /// UnlockUI cancels any currently running operations and restores UI
        /// </summary>
        public static void UnlockUI()
        {
            lock (uiMux)
            {
                StatusLibrary.Log("UnlockUI called");
                if (cancelTokenSource != null)
                {
                    cancelTokenSource.Cancel();
                }
                cancelTokenSource = new CancellationTokenSource();
                SetProgress(100);                
            }
        }

        public static void SetEvent(string name)
        {
            lock (uiMux)
            {
                lastEvent = currentEvent;
                currentEvent = name;
            }
        }

        public static void SetScope(string name)
        {
            lock (uiMux)
            {
                scope = name;
            }
        }

        /// <summary>
        /// Returns the current CancellationToken. No mutex lock occurs since it is thread safe
        /// </summary>
        public static CancellationToken CancelToken()
        {
            if (cancelTokenSource == null)
            {
                lock (mux)
                {
                    cancelTokenSource = new CancellationTokenSource();
                }
            }
            return cancelTokenSource.Token;
        }

        public static void Cancel()
        {
            if (cancelTokenSource == null)
            {
                return;
            }
            cancelTokenSource.Cancel();
        }


        public static bool IsFixNeeded(StatusType name)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                Status status = checks[name];
                bool isFixNeeded = status.IsFixNeeded;
                return isFixNeeded;
            }
        }

        public static void SetStatusBar(string value)
        {
            lock (uiMux)
            {
                StatusType name = StatusType.StatusBar;

                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                if (checks[name].Text != value) checks[name].Text = value;
            }
        }

        public static void SetIsFixNeeded(StatusType name, bool value)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }

                checks[name].IsFixNeeded = value;
            }
        }

        public static void SubscribeIsFixNeeded(StatusType name, EventHandler<bool> f)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                Status status = checks[name];
                status.IsFixNeededChange += f;
            }
        }

        public static string Text(StatusType name)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                string value = checks[name].Text;
                return value;
            }
        }

        public static void SetText(StatusType name, string value)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }

                checks[name].Text = value;
            }
        }

        public static void SubscribeText(StatusType name, EventHandler<string> f)
        {
            lock (mux)
            {
                if (!checks.ContainsKey(name))
                {
                    checks[name] = new Status();
                }
                Status status = checks[name];
                status.TextChange += f;
            }
        }

        public static int Progress()
        {
            lock (uiMux)
            {
                int value = progressValue;

                return value;
            }
        }

        public static void SetProgress(int value)
        {
            lock (uiMux)
            {
                progressValue = value;
                progressChange?.BeginInvoke(value, null, null);
            }
        }

        public static void SubscribeProgress(ProgressHandler f)
        {
            lock (uiMux)
            {
                progressChange += f;
            }
        }

        public static void SetDescription(string value)
        {
            lock (uiMux)
            {
                descriptionChange?.BeginInvoke(value, null, null);
            }
        }

        public static void SubscribeDescription(DescriptionHandler f)
        {
            lock (uiMux)
            {
                descriptionChange += f;
            }
        }

        public static string Description(StatusType name)
        {
            lock (uiMux)
            {
                if (!checks.ContainsKey(name))
                {
                    throw new System.Exception($"Status get description for {name} not found in dictionary");
                }
                string value = checks[name].Description;
                return value;
            }
        }

        public static void SetDescription(StatusType name, string value)
        {
            lock (uiMux)
            {
                if (!checks.ContainsKey(name))
                {
                    throw new System.Exception($"Status set description for {name} not found in dictionary");
                }

                checks[name].Description = value;
            }
        }

        /// <summary>
        /// Status represents a specific status of a tracked object, and is accessed via the StatusLibrary
        /// </summary>
        internal class Status
        {
            string text;
            public string Text { get { return text; } set { text = value; TextChange?.BeginInvoke(this, value, null, null); } }
            public event EventHandler<string> TextChange;

            bool isFixNeeded;
            public bool IsFixNeeded { get { return isFixNeeded; } set { isFixNeeded = value; IsFixNeededChange?.BeginInvoke(this, value, null, null); } }
            public event EventHandler<bool> IsFixNeededChange;

            string description;
            public string Description { get { return description; } set { description = value; } }
        }
    }
}
