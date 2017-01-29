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

        private GetSavedSessions saved_data;
        private PasswordLess IsPasswordLess;

        const int MYACTION_HOTKEY_ID1 = 1;
        const int MYACTION_HOTKEY_ID2 = 2;
        const int MYACTION_HOTKEY_ID3 = 3;
        const int MYACTION_HOTKEY_ID4 = 4;
        const int MYACTION_HOTKEY_ID5 = 5;

        /// <summary>
        /// Initialize PuTTY STORM sessions form (Form2) and register hotkeys for
        /// easy and fast swithing between TabPages.
        /// </summary>
        public SessionsForm(List<ProcessInfo> _my_ProcessInfo_List_TC_1, TabControl _tabcontrol1, TabControl _tabcontrol2, 
            SplitContainer _SessionsSplitContainer, List<GroupBox> _containers_list)
        {
            this.tabcontrol1 = _tabcontrol1;
            this.tabcontrol2 = _tabcontrol2;
            this.my_ProcessInfo_List_TC_1 = _my_ProcessInfo_List_TC_1;
            this.SessionsSplitContainer = _SessionsSplitContainer;
            this.containers_list = _containers_list;

            saved_data = new GetSavedSessions();
            IsPasswordLess = new PasswordLess();

            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID1, 2, (int)Keys.Tab);
            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID2, 6, (int)Keys.Tab);

            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID3, 2, (int)Keys.F1);
            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID4, 2, (int)Keys.F2);

            NativeMethods.RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID5, 2, (int)Keys.F3);

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
                if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID1)
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
                if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID2)
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
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID3)
            {
                Console.WriteLine("Split Screen Enabled");
                SessionsSplitContainer.Panel2Collapsed = false;
            }

            // Handle split screen deactivation CTRL+F2
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID4)
            {
                Console.WriteLine("Split Screen Disabled");
                SessionsSplitContainer.Panel2Collapsed = true;
            }

            // Handle SFTP manager activation
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID5)
            {
                if (SessionsSplitContainer.Panel2Collapsed == true)
                {
                    if (tabcontrol1.TabCount > 0)
                    {
                        Console.WriteLine("SFTP Manager started without splitscreen enabled!");
                        var credentials = GetSFTPCredentials(containers_list, tabcontrol1, null);
                        StartSFTPManager(credentials);
                    } else
                    {
                        MessageBox.Show("Unable to start SFTP Manager - No active sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                } else
                {
                    Console.WriteLine("SFTP Manager started but split screen is enabled, we have to decide which server to use!");

                    if (tabcontrol2.TabCount == 0)
                    {
                        if (tabcontrol1.TabCount > 0)
                        {
                            Console.WriteLine("Split screen enabled but tabcontrol2 is empty");
                            var credentials = GetSFTPCredentials(containers_list, tabcontrol1, null);
                            StartSFTPManager(credentials);
                        } else
                        {
                            MessageBox.Show("Unable to start SFTP Manager - No active sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    } else
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
        }

        /// <summary>
        /// Start SFTP Manager
        /// </summary>
        private void StartSFTPManager (Tuple<string, string, string> _credentials)
        {
            string PrivateKey = null;
            string hostname = _credentials.Item1;
            string login = _credentials.Item2;
            string password = _credentials.Item3;

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                SavedPrivatekeysInfo privatekeys = saved_data.get_PrivateKeys();
                string group = IsPasswordLess.GetGroupForPwdLessHostname(containers_list, _credentials.Item1);

                if (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, group))
                {
                    PrivateKey = IsPasswordLess.GetOpenSSHPrivateKeyForGroup(privatekeys, group);

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

            SFTPManager sftpManagerForm = new SFTPManager(hostname, login, password, PrivateKey);
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
            SelectConnectionForm.Size = new Size(400, 200);
            SelectConnectionForm.MinimumSize = new Size(400, 200);
            SelectConnectionForm.MaximumSize = new Size(400, 200);
            SelectConnectionForm.MaximizeBox = false;
            SelectConnectionForm.Text = GlobalVar.VERSION;
            SelectConnectionForm.StartPosition = FormStartPosition.CenterScreen;
            SelectConnectionForm.Name = "SelectSFTPConnectionForm";
            SelectConnectionForm.BackColor = Color.SlateGray;

            Label SelectConnectionLabel = new Label();
            SelectConnectionLabel.Text = "Select SFTP connection";
            SelectConnectionLabel.Size = new Size(300, 30);
            SelectConnectionLabel.Location = new Point(60, 15);
            SelectConnectionLabel.Font = new Font("Calibri", 20);
            SelectConnectionLabel.ForeColor = Color.White;

            Button connection1Button = new Button();
            connection1Button.Size = new Size(250, 30);
            connection1Button.Location = new Point(65, 70);
            connection1Button.Font = new Font("Calibri", 10);
            connection1Button.Text = _selectedTab1;
            connection1Button.UseVisualStyleBackColor = true;
            connection1Button.Click += new EventHandler(connection1Button_Click);

            Button connection2Button = new Button();
            connection2Button.Size = new Size(250, 30);
            connection2Button.Location = new Point(65, 110);
            connection2Button.Font = new Font("Calibri", 10);
            connection2Button.Text = _selectedTab2;
            connection2Button.UseVisualStyleBackColor = true;
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

        #region Global HotKeys
        public class KeyHandler
        {
            private int key;
            private IntPtr hWnd;
            private int id;

            public KeyHandler(Keys key, Form form)
            {
                this.key = (int)key;
                this.hWnd = form.Handle;
                id = this.GetHashCode();
            }

            public override int GetHashCode()
            {
                return key ^ hWnd.ToInt32();
            }

            public bool Register()
            {
                return NativeMethods.RegisterHotKey(hWnd, id, 0, key);
            }

            public bool Unregiser()
            {
                return NativeMethods.UnregisterHotKey(hWnd, id);
            }
        }
        #endregion

        #region Properties
        public static GlobalWindowEvents WindowEvents { get; private set; }
        #endregion
    }
}
