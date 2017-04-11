/*
 * Copyright (c) 2016 Karol Sebesta
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * This software is inspired by Jim Radford's http://www.jimradford.com
 * SuperPutty and various http://stackoverflow.com/ user ideas.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;

namespace PuTTY_Storm
{
    class SessionsForm : FormHelper
    {
        List<ProcessInfo> my_ProcessInfo_List_TC_1;
        TabControl tabcontrol1;
        TabControl tabcontrol2;
        SplitContainer SessionsSplitContainer;
        List<GroupBox> containers_list;

        // GlobalHotKeys Configuration settings
        private TabPagesForwardGlobalHotKeySettings tabPagesForwardGlobalHotKeySettings;
        private TabPagesBackwardGlobalHotKeySettings tabPagesbackwardGlobalHotKeySettings;
        private SplitScreenGlobalHotKeySettings splitScreenGlobalHotKeySettings;
        private SFTPManagerGlobalHotKeySettings sftpManagerGlobalHotKeySettings;
        private KotarakGlobalHotKeySettings kotarakGlobalHotKeySettings;

        // GlobalHotKeys registration
        private GlobalHotKeysWorker TabPagesForwardGlobalHotKeyWorker;
        private GlobalHotKeysWorker TabPagesBackwardGlobalHotKeyWorker;
        private GlobalHotKeysWorker SplitScreenGlobalHotKeyWorker;
        private GlobalHotKeysWorker SFTPManagerGlobalHotKeyWorker;
        private GlobalHotKeysWorker KotarakGlobalHotKeyWorker;

        private GetSavedSessions saved_data;
        private PasswordLess IsPasswordLess;

        /// <summary>
        /// Initialize PuTTY STORM sessions form (Form2) and register hotkeys for
        /// easy and fast swithing between TabPages.
        /// </summary>
        public SessionsForm(List<ProcessInfo> _my_ProcessInfo_List_TC_1, TabControl _tabcontrol1, TabControl _tabcontrol2, 
            SplitContainer _SessionsSplitContainer, List<GroupBox> _containers_list, GlobalHotKeysWorker _TabPagesForwardGlobalHotKeyWorker,
            GlobalHotKeysWorker _TabPagesBackwardGlobalHotKeyWorker, GlobalHotKeysWorker _SplitScreenGlobalHotKeyWorker,
            GlobalHotKeysWorker _SFTPManagerGlobalHotKeyWorker, GlobalHotKeysWorker _KotarakGlobalHotKeyWorker)
        {
            this.tabcontrol1 = _tabcontrol1;
            this.tabcontrol2 = _tabcontrol2;
            this.my_ProcessInfo_List_TC_1 = _my_ProcessInfo_List_TC_1;
            this.SessionsSplitContainer = _SessionsSplitContainer;
            this.containers_list = _containers_list;

            // GlobalHotKeys Configuration settings
            tabPagesForwardGlobalHotKeySettings = new TabPagesForwardGlobalHotKeySettings();
            tabPagesbackwardGlobalHotKeySettings = new TabPagesBackwardGlobalHotKeySettings();
            splitScreenGlobalHotKeySettings = new SplitScreenGlobalHotKeySettings();
            sftpManagerGlobalHotKeySettings = new SFTPManagerGlobalHotKeySettings();
            kotarakGlobalHotKeySettings = new KotarakGlobalHotKeySettings();

            // GlobalHotKeys registration
            this.TabPagesForwardGlobalHotKeyWorker = _TabPagesForwardGlobalHotKeyWorker;
            this.TabPagesBackwardGlobalHotKeyWorker = _TabPagesBackwardGlobalHotKeyWorker;
            this.SplitScreenGlobalHotKeyWorker = _SplitScreenGlobalHotKeyWorker;
            this.SFTPManagerGlobalHotKeyWorker = _SFTPManagerGlobalHotKeyWorker;
            this.KotarakGlobalHotKeyWorker = _KotarakGlobalHotKeyWorker;

            saved_data = new GetSavedSessions();
            IsPasswordLess = new PasswordLess();

            // Initialize GlobalHotKeys to the default values if config is empty (application first run)!
            GlobalHotKeysFirstStart();

            // Register GlobalHotKeys
            this.TabPagesForwardGlobalHotKeyWorker.Handle = this.Handle;
            this.TabPagesForwardGlobalHotKeyWorker.RegisterGlobalHotKey((int)GlobalHotKeysManager.ConvertFromStringToKey(tabPagesForwardGlobalHotKeySettings.key), 
                tabPagesForwardGlobalHotKeySettings.modifier);

            this.TabPagesBackwardGlobalHotKeyWorker.Handle = this.Handle;
            this.TabPagesBackwardGlobalHotKeyWorker.RegisterGlobalHotKey((int)GlobalHotKeysManager.ConvertFromStringToKey(tabPagesbackwardGlobalHotKeySettings.key), 
                tabPagesbackwardGlobalHotKeySettings.modifier);

            this.SplitScreenGlobalHotKeyWorker.Handle = this.Handle;
            this.SplitScreenGlobalHotKeyWorker.RegisterGlobalHotKey((int)GlobalHotKeysManager.ConvertFromStringToKey(splitScreenGlobalHotKeySettings.key), 
                splitScreenGlobalHotKeySettings.modifier);

            this.SFTPManagerGlobalHotKeyWorker.Handle = this.Handle;
            this.SFTPManagerGlobalHotKeyWorker.RegisterGlobalHotKey((int)GlobalHotKeysManager.ConvertFromStringToKey(sftpManagerGlobalHotKeySettings.key), 
                sftpManagerGlobalHotKeySettings.modifier);

            this.KotarakGlobalHotKeyWorker.Handle = this.Handle;
            this.KotarakGlobalHotKeyWorker.RegisterGlobalHotKey((int)GlobalHotKeysManager.ConvertFromStringToKey(kotarakGlobalHotKeySettings.key), 
                kotarakGlobalHotKeySettings.modifier);
        

            WindowEvents = new GlobalWindowEvents();
            SessionsForm.WindowEvents.SystemSwitch += new EventHandler<GlobalWindowEventArgs>(OnSystemSwitch);
        }

        bool isSwitchingViaAltTab = false;

        /// <summary>
        /// OnSystemSwitch event handler. Detects when we switch between application windows
        /// using ALT+TAB.
        /// </summary>
        void OnSystemSwitch(object sender, GlobalWindowEventArgs e)
        {
            switch (e.eventType)
            {
                case (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHSTART:
                    this.isSwitchingViaAltTab = true;
                    Console.WriteLine("ATLTAB START");
                    Console.WriteLine(isSwitchingViaAltTab);
                    break;
                case (uint)NativeMethods.WinEvents.EVENT_SYSTEM_SWITCHEND:
                    this.isSwitchingViaAltTab = false;
                    Console.WriteLine("ATLTAB  FINISH");
                    Console.WriteLine(isSwitchingViaAltTab);
                    break;
            }
        }

        /// <summary>
        /// Override WndProc, so:
        /// - We can handle focus when switching between applications 
        ///   using ALT+TAB and using registered hotkeys.
        /// - We can handle switching between tabs - forward/backward
        /// - We can handle split screen activation/deactivation
        /// - We can handle SFTP manager activation
        /// - We can handle Kotarak plugin activation
        /// </summary>
        /// 
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (my_ProcessInfo_List_TC_1.Count > 0)
            {
                // Trap WM_ACTIVATE when we get active
                if (m.Msg == 6 && m.WParam.ToInt32() == 1)
                {
                    if (Control.FromHandle(m.LParam) == null)
                    {
                        if (isSwitchingViaAltTab == true)
                        {
                            NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                        }
                    }
                }

                // Handle switching between tabs CRTL+TAB => forward
                if (m.Msg == 0x0312 && (short)m.WParam == this.TabPagesForwardGlobalHotKeyWorker.HOTKEYID)
                {
                    Console.WriteLine("HOTKEY ctrl+tab pressed");

                    if (tabcontrol1.TabCount == tabcontrol1.SelectedIndex + 1)
                    {
                        tabcontrol1.SelectedIndex = 0;
                    }
                    else
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.SelectedIndex + 1;
                    }
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                }

                // Handle switching between tabs CTRL+SHIFT+TAB => backward
                if (m.Msg == 0x0312 && (short)m.WParam == this.TabPagesBackwardGlobalHotKeyWorker.HOTKEYID)
                {
                    Console.WriteLine("HOTKEY ctrl+shift+tab pressed");

                    if (tabcontrol1.SelectedIndex == 0)
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.TabCount - 1;
                    }
                    else
                    {
                        tabcontrol1.SelectedIndex = tabcontrol1.SelectedIndex - 1;
                    }
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                }
            }

            // Handle split screen activation CRTL+F1
            if (m.Msg == 0x0312 && (short)m.WParam == this.SplitScreenGlobalHotKeyWorker.HOTKEYID)
            {
                if (SessionsSplitContainer.Panel2Collapsed)
                {
                    Console.WriteLine("Split Screen Enabled");
                    SessionsSplitContainer.Panel2Collapsed = false;
                } else
                {
                    Console.WriteLine("Split Screen Disabled");
                    SessionsSplitContainer.Panel2Collapsed = true;
                }
            }

            // Handle SFTP manager activation
            if (m.Msg == 0x0312 && (short)m.WParam == this.SFTPManagerGlobalHotKeyWorker.HOTKEYID)
            {
                if (SessionsSplitContainer.Panel2Collapsed == true)
                {
                    if (tabcontrol1.TabCount > 0)
                    {
                        Console.WriteLine("SFTP Manager started without splitscreen enabled!");
                        var credentials = GetSFTPCredentials(containers_list, tabcontrol1, null);
                        StartSFTPManager(credentials);
                    }
                    else
                    {
                        MessageBox.Show("Unable to start SFTP Manager - No active sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("SFTP Manager started but split screen is enabled, we have to decide which server to use!");

                    // If tabcontrol1 doesn't contain any opened tabs, then we are going to use tabcontrol2 for SFTP manager
                    if (tabcontrol1.TabCount == 0)
                    {
                        if (tabcontrol2.TabCount > 0)
                        {
                            Console.WriteLine("Split screen enabled but tabcontrol1 is empty");
                            var credentials = GetSFTPCredentials(containers_list, tabcontrol2, null);
                            StartSFTPManager(credentials);
                        }
                        else
                        {
                            MessageBox.Show("Unable to start SFTP Manager - No active sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    // If tabcontrol2 doesn't contain any opened tabs, then we are going to use tabcontrol1 for SFTP manager
                    else if (tabcontrol2.TabCount == 0)
                    {
                        if (tabcontrol1.TabCount > 0)
                        {
                            Console.WriteLine("Split screen enabled but tabcontrol2 is empty");
                            var credentials = GetSFTPCredentials(containers_list, tabcontrol1, null);
                            StartSFTPManager(credentials);
                        }
                        else
                        {
                            MessageBox.Show("Unable to start SFTP Manager - No active sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    // Otherwise we have opened tabs in both tabcontrols, so we are going to select the session for SFTP manager connect.
                    else
                    {
                        Console.WriteLine("Split screen enabled and tabcontrol2 contains active sessions");

                        String selectedTab1 = tabcontrol1.SelectedTab.Text;
                        String selectedTab2 = tabcontrol2.SelectedTab.Text;

                        if (selectedTab1 == selectedTab2)
                        {
                            var credentials = GetSFTPCredentials(containers_list, null, selectedTab1);
                            StartSFTPManager(credentials);
                        }
                        else
                        {
                            SelectSFTPConnection(selectedTab1, selectedTab2);
                        }
                    }
                }
            }

            // Handle Kotarak plugin activation
            if (m.Msg == 0x0312 && (short)m.WParam == this.KotarakGlobalHotKeyWorker.HOTKEYID)
            {
                StartKotarakPlugin();
            }
        }

        /// <summary>
        /// Start SFTP Manager
        /// </summary>
        private void StartSFTPManager(Tuple<string, string, string> _credentials)
        {
            string PrivateKey = null;
            string hostname = _credentials.Item1;
            string login = _credentials.Item2;
            string password = _credentials.Item3;
            string pk_pwd = null;

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                SavedPrivatekeysInfo privatekeys = saved_data.get_PrivateKeys();
                string[] groups = IsPasswordLess.GetGroupsForPwdLessHostname(containers_list, _credentials.Item1);

                // Check if group or su-group is part of private keys configuration setup
                if ((IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, groups[0])) ||
                    (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, groups[1])))
                {
                    // Fetch private key and password for group 
                    PrivateKey = IsPasswordLess.GetOpenSSHPrivateKeyForGroup(privatekeys, groups[0]);
                    pk_pwd = IsPasswordLess.GetOpenSSHPrivateKeyPassPhrase(privatekeys, groups[0]);

                    // If private key and password is still null, then sub-group is part of its setup - fetch it!
                    if (PrivateKey == null && pk_pwd == null)
                    {
                        Console.WriteLine("## Sub-group is part of pwdess login!");
                        PrivateKey = IsPasswordLess.GetOpenSSHPrivateKeyForGroup(privatekeys, groups[1]);
                        pk_pwd = IsPasswordLess.GetOpenSSHPrivateKeyPassPhrase(privatekeys, groups[1]);
                    }

                    // If private key doesn't exists or is still null then something is wrong! Stop processing and return!
                    if (!File.Exists(PrivateKey))
                    {
                        if (PrivateKey == null || PrivateKey == "")
                        {
                            PrivateKey = "of OpenSSH type or its group";
                        }
                        MessageBox.Show("You are going to use SFTP Manager passwordless login, " + Environment.NewLine +
                            "however private key " + PrivateKey + " doesn't exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        return;
                    }

                    password = null;
                }
            }

            SFTPManager sftpManagerForm = new SFTPManager(hostname, login, password, PrivateKey, pk_pwd);
            sftpManagerForm.Name = "SFTPManager";
            sftpManagerForm.Text = GlobalVar.VERSION + " - SFTP Manager";
            sftpManagerForm.Show();
        }

        /// <summary>
        /// Get credentials for SFTP connection based on active tab in TabControl
        /// </summary>
        private Tuple<string, string, string> GetSFTPCredentials(List<GroupBox> containers_list, TabControl tabcontrol, String _selectedTab)
        {
            String selectedTab;

            if (_selectedTab == null)
            {
                selectedTab = tabcontrol.SelectedTab.Text;
            } else
            {
                selectedTab = _selectedTab;
            }

            string hostname = null;
            string username = null;
            string password = null;

            Regex regex = new Regex(@selectedTab);

            foreach (GroupBox container in containers_list)
            {
                Control[] hostname_textbox = container.Controls.Find("hostname_textbox", true);

                Match match = regex.Match(hostname_textbox[0].Text);
                if (match.Success)
                {
                    hostname = hostname_textbox[0].Text;

                    Control[] username_texbox = container.Controls.Find("username_textbox", true);
                    username = username_texbox[0].Text;

                    Control[] password_texbox = container.Controls.Find("password_textbox", true);
                    password = password_texbox[0].Text;

                    break;
                }
            }

            return new Tuple<string, string, string>(hostname, username, password);
        }

        /// <summary>
        /// When split screen is enabled we have to decide which server to use for SFTP connection
        /// </summary>
        FormHelper SelectConnectionForm;

        private void SelectSFTPConnection (String _selectedTab1, String _selectedTab2)
        {
            SelectConnectionForm = new FormHelper();
            SelectConnectionForm.Size = DPIAwareScaling.ScaleSize(400, 200);
            SelectConnectionForm.MinimumSize = DPIAwareScaling.ScaleSize(400, 200);
            SelectConnectionForm.MaximumSize = DPIAwareScaling.ScaleSize(400, 200);
            SelectConnectionForm.MaximizeBox = false;
            SelectConnectionForm.Text = GlobalVar.VERSION;
            SelectConnectionForm.StartPosition = FormStartPosition.CenterScreen;
            SelectConnectionForm.Name = "SelectSFTPConnectionForm";
            SelectConnectionForm.BackColor = Color.SlateGray;

            Label SelectConnectionLabel = new Label();
            SelectConnectionLabel.Text = "Select SFTP connection";
            SelectConnectionLabel.Size = DPIAwareScaling.ScaleSize(300, 30);
            SelectConnectionLabel.Location = DPIAwareScaling.ScalePoint(60, 15);
            SelectConnectionLabel.Font = new Font("Calibri", 20);
            SelectConnectionLabel.ForeColor = Color.White;

            Button connection1Button = new Button();
            connection1Button.Size = DPIAwareScaling.ScaleSize(250, 30);
            connection1Button.Location = DPIAwareScaling.ScalePoint(65, 70);
            connection1Button.Font = new Font("Calibri", 10);
            connection1Button.Text = _selectedTab1;
            connection1Button.UseVisualStyleBackColor = true;
            connection1Button.FlatStyle = FlatStyle.System;
            connection1Button.Click += new EventHandler(connection1Button_Click);

            Button connection2Button = new Button();
            connection2Button.Size = DPIAwareScaling.ScaleSize(250, 30);
            connection2Button.Location = DPIAwareScaling.ScalePoint(65, 110);
            connection2Button.Font = new Font("Calibri", 10);
            connection2Button.Text = _selectedTab2;
            connection2Button.UseVisualStyleBackColor = true;
            connection2Button.FlatStyle = FlatStyle.System;
            connection2Button.Click += new EventHandler(connection2Button_Click);


            SelectConnectionForm.Controls.Add(SelectConnectionLabel);
            SelectConnectionForm.Controls.Add(connection1Button);
            SelectConnectionForm.Controls.Add(connection2Button);
            SelectConnectionForm.Show();
        }

        private void connection1Button_Click(object sender, EventArgs e)
        {
            var selectedTab = sender as Button;

            var credentials = GetSFTPCredentials(containers_list, null, selectedTab.Text);
            StartSFTPManager(credentials);

            SelectConnectionForm.Dispose();
        }

        private void connection2Button_Click(object sender, EventArgs e)
        {
            var selectedTab = sender as Button;

            var credentials = GetSFTPCredentials(containers_list, null, selectedTab.Text);
            StartSFTPManager(credentials);

            SelectConnectionForm.Dispose();
        }

        /// <summary>
        /// Start Kotarak configuration management plugin Form
        /// </summary>
        private void StartKotarakPlugin ()
        {
            KotarakMainForm KotarakPlugin = new KotarakMainForm(containers_list);
            KotarakPlugin.Name = "kotarak";
            KotarakPlugin.Text = GlobalVar.VERSION + " - Kotarak - Configuration Management";
            KotarakPlugin.Show();
        }

        /// <summary>
        /// This method performs a first initialization of default settings for 
        /// GlobalHotKeysManager configuration. Called only once during first 
        /// application startup (Sessions Form opening).
        /// </summary>
        private void GlobalHotKeysFirstStart()
        {
            // Application first run - set the default GlobalHotKeys (this must be placed to to application startup!!!)
            if (tabPagesForwardGlobalHotKeySettings.key == null && tabPagesbackwardGlobalHotKeySettings.key == null &&
                splitScreenGlobalHotKeySettings.key == null && sftpManagerGlobalHotKeySettings.key == null && kotarakGlobalHotKeySettings.key == null)
            {
                // CTRL + TAB for tabPagesForwardGlobalHotKey
                tabPagesForwardGlobalHotKeySettings.modifier = 2;
                tabPagesForwardGlobalHotKeySettings.key = "Tab";

                // CTRL + Shift + Tab for tabPagesbackwardGlobalHotKey
                tabPagesbackwardGlobalHotKeySettings.modifier = 6;
                tabPagesbackwardGlobalHotKeySettings.key = "Tab";

                // CTRL + F1 for splitScreenGlobalHotKey
                splitScreenGlobalHotKeySettings.modifier = 2;
                splitScreenGlobalHotKeySettings.key = "F1";

                // CTRL + F2 for sftpManagerGlobalHotKey
                sftpManagerGlobalHotKeySettings.modifier = 2;
                sftpManagerGlobalHotKeySettings.key = "F2";

                // CTRL + F3 for kotarakGlobalHotKey
                kotarakGlobalHotKeySettings.modifier = 2;
                kotarakGlobalHotKeySettings.key = "F3";

                // Save it!
                tabPagesForwardGlobalHotKeySettings.Save();
                tabPagesbackwardGlobalHotKeySettings.Save();
                splitScreenGlobalHotKeySettings.Save();
                sftpManagerGlobalHotKeySettings.Save();
                kotarakGlobalHotKeySettings.Save();

            }
        }

        #region Properties
        public static GlobalWindowEvents WindowEvents { get; private set; }
        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionsForm));
            this.SuspendLayout();
            // 
            // SessionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionsForm";
            this.ResumeLayout(false);

        }
    }
}
