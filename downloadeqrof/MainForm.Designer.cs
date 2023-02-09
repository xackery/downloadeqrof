namespace ROF_Downloader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.lblDescription = new System.Windows.Forms.LinkLabel();
            this.grp = new System.Windows.Forms.GroupBox();
            this.chkApplyTweaks = new System.Windows.Forms.CheckBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.grpSteamGuard = new System.Windows.Forms.GroupBox();
            this.lblSteamGuard = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSteamGuardCode = new System.Windows.Forms.TextBox();
            this.lblSteamCode = new System.Windows.Forms.Label();
            this.btnSteamGuard = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.grp.SuspendLayout();
            this.grpSteamGuard.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(384, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusBar
            // 
            this.lblStatusBar.Name = "lblStatusBar";
            this.lblStatusBar.Size = new System.Drawing.Size(69, 17);
            this.lblStatusBar.Text = "lblStatusBar";
            // 
            // prgStatus
            // 
            this.prgStatus.BackColor = System.Drawing.SystemColors.Control;
            this.prgStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prgStatus.Location = new System.Drawing.Point(0, 216);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(384, 23);
            this.prgStatus.TabIndex = 12;
            this.prgStatus.Visible = false;
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Location = new System.Drawing.Point(12, 113);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(360, 100);
            this.lblDescription.TabIndex = 14;
            this.lblDescription.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDescription_Click);
            // 
            // grp
            // 
            this.grp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grp.Controls.Add(this.grpSteamGuard);
            this.grp.Controls.Add(this.chkApplyTweaks);
            this.grp.Controls.Add(this.btnDownload);
            this.grp.Controls.Add(this.txtUsername);
            this.grp.Controls.Add(this.lblPassword);
            this.grp.Controls.Add(this.lblUsername);
            this.grp.Controls.Add(this.txtPassword);
            this.grp.Location = new System.Drawing.Point(12, 12);
            this.grp.Name = "grp";
            this.grp.Size = new System.Drawing.Size(360, 198);
            this.grp.TabIndex = 15;
            this.grp.TabStop = false;
            // 
            // chkApplyTweaks
            // 
            this.chkApplyTweaks.AutoSize = true;
            this.chkApplyTweaks.Location = new System.Drawing.Point(9, 39);
            this.chkApplyTweaks.Name = "chkApplyTweaks";
            this.chkApplyTweaks.Size = new System.Drawing.Size(159, 17);
            this.chkApplyTweaks.TabIndex = 5;
            this.chkApplyTweaks.Text = "Apply recommended tweaks";
            this.chkApplyTweaks.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(6, 62);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(345, 23);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(68, 13);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(195, 16);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 16);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(254, 13);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 0;
            // 
            // grpSteamGuard
            // 
            this.grpSteamGuard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSteamGuard.Controls.Add(this.btnSteamGuard);
            this.grpSteamGuard.Controls.Add(this.lblSteamCode);
            this.grpSteamGuard.Controls.Add(this.txtSteamGuardCode);
            this.grpSteamGuard.Controls.Add(this.label1);
            this.grpSteamGuard.Controls.Add(this.lblSteamGuard);
            this.grpSteamGuard.Location = new System.Drawing.Point(9, 92);
            this.grpSteamGuard.Name = "grpSteamGuard";
            this.grpSteamGuard.Size = new System.Drawing.Size(339, 100);
            this.grpSteamGuard.TabIndex = 18;
            this.grpSteamGuard.TabStop = false;
            this.grpSteamGuard.Text = "Steam Guard";
            this.grpSteamGuard.Visible = false;
            // 
            // lblSteamGuard
            // 
            this.lblSteamGuard.AutoSize = true;
            this.lblSteamGuard.Location = new System.Drawing.Point(7, 20);
            this.lblSteamGuard.Name = "lblSteamGuard";
            this.lblSteamGuard.Size = new System.Drawing.Size(218, 13);
            this.lblSteamGuard.TabIndex = 0;
            this.lblSteamGuard.Text = "Steam Guard was detected on this account. ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please enter the authentication code sent to your email address.";
            // 
            // txtSteamGuardCode
            // 
            this.txtSteamGuardCode.Location = new System.Drawing.Point(48, 71);
            this.txtSteamGuardCode.Name = "txtSteamGuardCode";
            this.txtSteamGuardCode.Size = new System.Drawing.Size(100, 20);
            this.txtSteamGuardCode.TabIndex = 2;
            // 
            // lblSteamCode
            // 
            this.lblSteamCode.AutoSize = true;
            this.lblSteamCode.Location = new System.Drawing.Point(7, 74);
            this.lblSteamCode.Name = "lblSteamCode";
            this.lblSteamCode.Size = new System.Drawing.Size(35, 13);
            this.lblSteamCode.TabIndex = 3;
            this.lblSteamCode.Text = "Code:";
            // 
            // btnSteamGuard
            // 
            this.btnSteamGuard.Location = new System.Drawing.Point(154, 71);
            this.btnSteamGuard.Name = "btnSteamGuard";
            this.btnSteamGuard.Size = new System.Drawing.Size(75, 23);
            this.btnSteamGuard.TabIndex = 4;
            this.btnSteamGuard.Text = "Submit";
            this.btnSteamGuard.UseVisualStyleBackColor = true;
            this.btnSteamGuard.Click += new System.EventHandler(this.btnSteamGuard_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.grp);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.prgStatus);
            this.Controls.Add(this.statusStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download EQ RoF2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grp.ResumeLayout(false);
            this.grp.PerformLayout();
            this.grpSteamGuard.ResumeLayout(false);
            this.grpSteamGuard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.LinkLabel lblDescription;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusBar;
        private System.Windows.Forms.GroupBox grp;
        private System.Windows.Forms.CheckBox chkApplyTweaks;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.GroupBox grpSteamGuard;
        private System.Windows.Forms.Button btnSteamGuard;
        private System.Windows.Forms.Label lblSteamCode;
        private System.Windows.Forms.TextBox txtSteamGuardCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSteamGuard;
    }
}

