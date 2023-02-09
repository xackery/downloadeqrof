using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROF_Downloader
{
    internal class Download
    {
        readonly static Mutex mux = new Mutex();
        public static Process proc;

        public static void SetSteamGuard(string code)
        {
            mux.WaitOne();
            if (proc != null) {
                proc.StandardInput.Write(code+"\n");
            }
            mux.ReleaseMutex();
        }

        public static void Kill()
        {
            mux.WaitOne();
            if (proc != null)
            {
                proc.Kill();
                proc = null;
            }
            mux.ReleaseMutex();
        }

        public static async Task Start(string username, string password)
        {
            await UtilityLibrary.Download(0, 1, "https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.4.7/depotdownloader-2.4.7.zip", "cache", "depotdownloader-2.4.7.zip", 2);
            await UtilityLibrary.Extract(1, 2, "cache", "depotdownloader-2.4.7.zip", "", $"{Application.StartupPath}\\DepotDownloader.exe", 2);

            StatusLibrary.SetStatusBar($"Starting DepotDownloader...");
            try
            {
                mux.WaitOne();
                proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = $"{Application.StartupPath}\\DepotDownloader.exe",
                        WorkingDirectory = $"{Application.StartupPath}",
                        Arguments = $"-app 205710 -depot 205711 -manifest 1926608638440811669 -username \"{username}\" -password \"{password}\" -dir .",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    }
                };
                

                proc.OutputDataReceived += new DataReceivedEventHandler((object src, DataReceivedEventArgs earg) =>
                {
                    string line = earg.Data;
                    if (line == null)
                    {
                        return;
                    }
                    if (line.Contains("This account is protected by Steam Guard."))
                    {
                        StatusLibrary.SetStatusBar($"Steam Guard!");
                    }
                    if (line.StartsWith("Pre-allocating"))
                    {
                        StatusLibrary.SetStatusBar($"Pre-allocating files...");
                    }
                    if (line.Contains("%"))
                    {
                        var percent = line.Substring(1, 2);
                        int.TryParse(percent, out int progress);
                        StatusLibrary.SetProgress(progress);
                        StatusLibrary.SetStatusBar($"Downloading...");
                    }
                    if (line.Contains("Total downloaded:"))
                    {
                        StatusLibrary.SetStatusBar(line);
                    }
                    StatusLibrary.Log($"{line}");
                });

                proc.ErrorDataReceived += new DataReceivedEventHandler((object src, DataReceivedEventArgs earg) =>
                {
                    string line = earg.Data;
                    if (line == null)
                    {
                        return;
                    }
                    StatusLibrary.Log($"error: {line}");

                    if (line.Contains("Unhandled exception"))
                    {
                        StatusLibrary.SetStatusBar(line.Substring(21));
                    }
                });
                proc.StartInfo.EnvironmentVariables["PATH"] = UtilityLibrary.EnvironmentPath();
                proc.Start();
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();
                mux.ReleaseMutex();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                string result = $"Failed DepotDownloader: {e.Message}";
                StatusLibrary.SetStatusBar(result);
                MessageBox.Show(result, "Completed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            proc = null;
        }
    }
}
