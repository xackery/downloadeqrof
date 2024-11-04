using AutoUpdaterDotNET;
using Microsoft.Win32;
using MySqlConnector;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Xml.Linq;
using YamlDotNet.Core.Tokens;
//using System.Windows.Shell;

namespace ROF_Downloader
{
    public partial class MainForm : Form
    {
        Regex descriptionLinkRegex = new Regex(@"(.*)\[(.*)\]\((.*)\)(.*)");
        string lastDescription;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Fippy does not need admin access.\nRestart fippy without running as an Administrator.","Admin Mode Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            
            StatusLibrary.InitLog();
            StatusLibrary.SetEvent("Initialization");
            StatusLibrary.SetScope("MainForm.Load");

            int darkThemeValue = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1);
            bool isDarkTheme = darkThemeValue == 0;
            if (isDarkTheme)
            {
                var preference = Convert.ToInt32(true);
                WinLibrary.DwmSetWindowAttribute(this.Handle,
                                      WinLibrary.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
                                      ref preference, sizeof(uint));
                ChangeTheme(this.Controls, true);
            }

            lblIntro.Text = "This program will download the Rain of Fear 2 client for you from Steam.\n\n" +
                "Please enter your Steam username and password.\n" +
                "If you have Steam Guard enabled, you will be prompted to enter the code.\n" +
                "Please note that this program will not store your username or password.\n" +
                "Use at your own risk.\n";

            StatusType context;

            context = StatusType.StatusBar;
            StatusLibrary.SubscribeText(context, new EventHandler<string>((object src, string value) => { Invoke((MethodInvoker)delegate 
            { 
                lblStatusBar.Text = value;
                StatusLibrary.Log($"StatusBar: {value}");
                if (value == "Steam Guard!")
                {
                    btnSteamGuard.Focus();
                    grpSteamGuard.Visible = true;                    
                    lblDescription.Visible = false;
                }
            }); }));

            StatusLibrary.SubscribeProgress(new StatusLibrary.ProgressHandler((int value) => { Invoke((MethodInvoker)delegate { 
                prgStatus.Visible = (value != 100);
                lblDescription.Visible = (value == 100);
                prgStatus.Value = value; 
            }); }));
            StatusLibrary.SubscribeDescription(new StatusLibrary.DescriptionHandler((string value) => { Invoke((MethodInvoker)delegate {
                if (lastDescription == value) {
                    return;
                }
                if (value == null)
                {
                    lastDescription = value;
                    lblDescription.Tag = "";
                    lblDescription.Text = "";
                    return;
                }
                lastDescription = value;
                lblDescription.Tag = "";
                var area = new LinkArea();
                var lines = value.Split('\n');
                bool isFirstLine = true;

                foreach (var line in lines)
                {
                    MatchCollection matches = descriptionLinkRegex.Matches(line);
                    if (matches.Count == 0)
                    {
                        if (isFirstLine)
                        {
                            lblDescription.Text = line;
                            isFirstLine = false;
                        } else
                        {
                            lblDescription.Text += "\n"+line;
                        }                        
                        continue;
                    }

                    if (isFirstLine)
                    {
                        lblDescription.Text = matches[0].Groups[1].Value;
                        isFirstLine = false;
                    }
                    else
                    {
                        lblDescription.Text += "\n"+matches[0].Groups[1].Value;
                    }
                    
                    area.Start = lblDescription.Text.Length;
                    area.Length = matches[0].Groups[2].Value.Length;
                    lblDescription.Text += matches[0].Groups[2].Value;
                    lblDescription.Tag = matches[0].Groups[3].Value;
                    lblDescription.Text += matches[0].Groups[4].Value;
                }
                lblDescription.LinkArea = area;
            }); }));

            try
            {
                MakeSubfolders();
            } catch (Exception ex)
            {
                StatusLibrary.Log($"Failed to create subfolders: {ex.Message}");
                StatusLibrary.SetScope("Make subfolders");
                MessageBox.Show($"Failed to make subfolders: {ex.Message}", "Make Subfolders", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            StatusLibrary.SetStatusBar("Ready");

        }

        private void MakeSubfolders()
        {
            string[] paths =
            {
                Application.StartupPath + "\\cache",                
            };
            foreach (string path in paths) {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        private void FixClick(object sender, EventArgs e)
        {

        }

        private void FixAllClick(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control == null)
            {
                MessageBox.Show($"failed to fix all click due to unknown control {sender} (expected Control)", "FixAll Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StatusType? status = control.Tag as StatusType?;
            if (status == null)
            {
                MessageBox.Show($"failed to fix all click (no tag)", "FixAll Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Type t = Type.GetType($"ROF_Downloader.{status}");
            if (t == null)
            {
                MessageBox.Show($"failed to fix all click (class {status} does not exist)", "FixAll Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var method = t.GetMethod("FixAll", BindingFlags.Static | BindingFlags.Public);
            if (method == null)
            {
                MessageBox.Show($"failed to fix all click (class {status} has no method FixAll)", "FixAll Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            method.Invoke(sender, null);
        }


        public void ChangeTheme(Control.ControlCollection container, bool isDarkMode)
        {
            
            foreach (Control component in container)
            {
                
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
        }


        private void contextSystrayMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnUsername_MouseMove(object sender, MouseEventArgs e)
        {
            StatusLibrary.SetDescription("Username for steam");
        }


        private void btnDownload_MouseMove(object sender, MouseEventArgs e)
        {
            StatusLibrary.SetDescription("Begin the steam RoF2 download");
        }

        private void lblDescription_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lblDescription.Tag == null)
            {
                return;
            }
            if (lblDescription.Tag.ToString() == "")
            {
                return;
            }
            Process.Start("explorer.exe", lblDescription.Tag.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (btnDownload.Text == "Cancel")
            {
                StatusLibrary.Cancel();
                StatusLibrary.SetStatusBar("Cancelled");
                return;
            }
            if (!validatePath())
            {
                return;
            }

            lblIntro.Visible = false;
            lblDescription.Visible = true;
            txtPassword.Enabled = false;
            txtPath.Enabled = false;
            txtUsername.Enabled = false;
            btnBrowse.Enabled = false;
            btnDownload.Text = "Cancel";
            btnDownload.Visible = false;
            Task.Run(async () =>
            {
                StatusLibrary.LockUI();
                await Download.Start(txtUsername.Text, txtPassword.Text, txtPath.Text);
                StatusLibrary.UnlockUI();
                Invoke((MethodInvoker)delegate
                {
                    lblIntro.Visible = true;
                    lblDescription.Visible = false;
                    txtPassword.Enabled = true;
                    txtUsername.Enabled = true;
                    txtPath.Enabled = true;
                    btnDownload.Text = "Download";
                    btnBrowse.Enabled = true;
                    btnDownload.Visible = true;
                    Debug.Print("Release UI");
                });

            });
           
        }

        private bool validatePath()
        {
            var path = txtPath.Text;
            if (path.Length == 0)
            {
                MessageBox.Show("Please enter a path", "Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (path == null)
            {
                MessageBox.Show("Please enter a path", "Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!Directory.Exists(path))
            {
                MessageBox.Show($"The path {path} does not exist", "Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (File.Exists(path))
            {
                MessageBox.Show($"The path {path} is a file, not a folder", "Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void btnSteamGuard_Click(object sender, EventArgs e)
        {
            Download.SetSteamGuard(txtSteamGuardCode.Text);
            grpSteamGuard.Visible = false;
            lblDescription.Visible = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StatusLibrary.Cancel();
            Download.Kill();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (diaFolderBrowse.SelectedPath == "")
            {

                diaFolderBrowse.SelectedPath =  System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            }

            if (txtPath.Text != "")
            {
                diaFolderBrowse.SelectedPath = txtPath.Text;
            }


            var result = diaFolderBrowse.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = diaFolderBrowse.SelectedPath;
                if (path == null)
                {
                    return;
                }
                txtPath.Text = path;
                if (!validatePath())
                {
                    txtPath.Text = "";
                    return;
                }
            }
        }
    }
}




