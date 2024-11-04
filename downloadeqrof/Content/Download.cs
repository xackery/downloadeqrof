using MS.WindowsAPICodePack.Internal;
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
using System.Windows.Shapes;

namespace ROF_Downloader
{
    internal class Download
    {
        readonly static Mutex mux = new Mutex();
        public static Process proc;

        public static void SetSteamGuard(string code)
        {
            if (proc != null)
            {
                proc.StandardInput.Write(code + "\n");
            }
        }

        public static void Kill()
        {
            if (proc != null)
            {
                proc.Kill();
                proc = null;
            }
            StatusLibrary.SetStatusBar($"DepotDownloader killed");
        }

        public static async Task Start(string username, string password, string path)
        {
            await UtilityLibrary.Download(0, 1, "https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.4.7/depotdownloader-2.4.7.zip", path+"\\cache", "depotdownloader-2.4.7.zip", 2);
            await UtilityLibrary.Extract(1, 2, $"{path}\\cache", "depotdownloader-2.4.7.zip", $"{path}\\cache", $"{path}\\cache\\DepotDownloader.exe", 2);
  //          await UtilityLibrary.Download(0, 1, "https://github.com/SteamRE/DepotDownloader/releases/download/DepotDownloader_2.7.3/DepotDownloader-windows-x64.zip", path + "\\cache", "depotdownloader-2.7.3.zip", 2);
//            await UtilityLibrary.Extract(1, 2, $"{path}\\cache", "depotdownloader-2.7.3.zip", $"{path}\\cache", $"{path}\\cache\\DepotDownloader.exe", 2);

            StatusLibrary.SetStatusBar($"Starting DepotDownloader...");
            lock (mux)
            {
                try
                {
                    // sleep for 3 seconds
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = $"{path}\\cache\\DepotDownloader.exe",
                            WorkingDirectory = $"{path}\\cache",
                            Arguments = $"-app 205710 -depot 205711 -manifest 1926608638440811669 -username \"{username}\" -password \"{password}\" -dir \"{path}\"",
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
                        if (line.Contains("Total downloaded:"))
                        {
                            StatusLibrary.SetStatusBar($"Download Complete");
                            MessageBox.Show("Download Complete", "Finished Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (line.Contains("Connecting to Steam3....."))
                        {
                            StatusLibrary.SetStatusBar($"Connecting to Steam...");
                        }
                        if (line.Contains("into Steam3..."))
                        {
                            StatusLibrary.SetStatusBar($"Attempting to log in...");
                        }
                        if (line.Contains("This account is protected by Steam Guard."))
                        {
                            StatusLibrary.SetStatusBar($"Steam Guard!");
                        }
                        if (line.Contains("Retrying Steam3 connection"))
                        {
                            StatusLibrary.SetStatusBar($"Retrying Steam connection...");
                        }
                        if (line.StartsWith("Pre-allocating"))
                        {
                            StatusLibrary.SetStatusBar($"Pre-allocating files...");
                        }
                        if (line.Contains("Unable to login to Steam3:")) 
                        {
                            var message = line.Substring(line.IndexOf("Unable to login to Steam3:")+26);
                            StatusLibrary.SetStatusBar(message);

                            // take the remaining line and fail with that message
                            MessageBox.Show(message, "Failed to Download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (line.Contains("%"))
                        {
                            var percent = line.Substring(1, 2);
                            int.TryParse(percent, out int progress);
                            // split the string to first space, take second section
                            var split = line.Split(' ');
                            if (split.Length < 3)
                            {
                                return;
                            }

                            var fileName = split[2].Trim();
                            if (fileName.StartsWith(path))
                            {
                                fileName = fileName.Substring(path.Length);
                            }
                            if (fileName.StartsWith("\\"))
                            {
                               fileName = fileName.Substring(1);
                            }
                            // truncate the path
                            


                            StatusLibrary.SetProgress(progress);
                            StatusLibrary.SetStatusBar($"Downloading {fileName}...");
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
                        /*
                         * 
                        
                        StatusLibrary.Log($"error: {line}");

                        if (line.Contains("Unhandled exception"))
                        {
                            StatusLibrary.SetStatusBar(line.Substring(21));
                        }
                        */
                    });

                    proc.Exited += new EventHandler((object src, EventArgs earg) =>
                    {
                        int exitCode = proc.ExitCode;
                        if (exitCode == 0)
                        {
                            StatusLibrary.SetStatusBar($"Download completed");
                            return;
                        }
                        StatusLibrary.SetStatusBar($"DepotDownloader failed with exit code {exitCode}");
                        MessageBox.Show($"DepotDownloader failed with exit code {exitCode}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                    });
                    proc.StartInfo.EnvironmentVariables["PATH"] = UtilityLibrary.EnvironmentPath();
                    proc.Start();
                    proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();
                    proc.WaitForExit();
                }
                catch (Exception e)
                {
                    string result = $"Failed DepotDownloader: {e.Message}";
                    StatusLibrary.SetStatusBar(result);
                    MessageBox.Show(result, "Completed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (proc != null)
                    {
                        proc.Close();
                        proc = null;
                    }
                    StatusLibrary.SetStatusBar($"");
                }
                proc = null;
            }
        }
    }
}
