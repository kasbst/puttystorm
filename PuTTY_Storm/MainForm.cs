/*
 * Copyright (c) 2017 Karol Sebesta
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace PuTTY_Storm
{
    public partial class MainForm : Form
    {
        private SetControls custom_controls;
        private PasswordLess IsPasswordLess;
        private GetSavedSessions saved_data;
        private SaveSessions sessions;
        private CryptoHelper crypto;

        public MainForm()
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Text = GlobalVar.VERSION;
            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            IsPasswordLess = new PasswordLess();
            saved_data = new GetSavedSessions();
            custom_controls = new SetControls();
            sessions = new SaveSessions();
            crypto = new CryptoHelper();
           
            timer = new System.Windows.Forms.Timer();
            timer.Tick += timer_Tick;
            timer.Interval = 400;

            // Set some controls scaling based on display DPI
            DPIAwareScaling.SetControlsExtendedDPISettings();
        }

        private void StartScreen ()
        {
            Application.Run(new Splash_Screens.StartScreen());
        }

        private void StopScreen ()
        {
            Application.Run(new Splash_Screens.StopScreen());
        }

        /// <summary>
        /// Initialize controls on the main configuration Form (MainForm) of PuTTY STORM. Load controls to each session GroupBox container 
        /// and put this session GroupBox to the main configurion Form during the MainForm_Load function.
        /// </summary>
        /// <returns>GrouBox container.</returns>
        private GroupBox Add_Main_Component()
        {
            GroupBox container = new GroupBox();
            Label label_hostname = new Label();
            TextBox textbox_hostname = new TextBox();
            Label label_username = new Label();
            TextBox textbox_username = new TextBox();
            Label label_password = new Label();
            TextBox textbox_password = new TextBox();
            NumericUpDown numericupdown = new NumericUpDown();
            ComboBox groups = new ComboBox();
            ComboBox sub_groups = new ComboBox();

            custom_controls.initialize_container(container);
            custom_controls.initialize_hostname_label(label_hostname);
            custom_controls.initialize_hostname_textbox(textbox_hostname);
            custom_controls.initialize_username_label(label_username);
            custom_controls.initialize_username_textbox(textbox_username);
            custom_controls.initialize_password_label(label_password);
            custom_controls.initialize_password_textbox(textbox_password);
            custom_controls.initialize_numbericupdown(numericupdown, Numericupdown_ValueChanged);
            custom_controls.initialize_combobox(groups, Combobox_SelectedIndexChanged, combobox_PKGroupChanged);
            custom_controls.initialize_sub_groups_combobox(sub_groups, Combobox_SelectedIndexChanged, sub_groups_combobox_PKGroupChanged);

            container.Controls.Add(label_hostname);
            container.Controls.Add(textbox_hostname);
            container.Controls.Add(label_username);
            container.Controls.Add(textbox_username);
            container.Controls.Add(label_password);
            container.Controls.Add(textbox_password);
            container.Controls.Add(numericupdown);
            container.Controls.Add(groups);
            container.Controls.Add(sub_groups);
            
            return container;
        }

        /// <summary>
        /// Fill the panel with required controls. This is the panel contained within the top of PuTTY STORM sessions
        /// Form (SessionsForm). It contains controls to open new putty session, search for saved sessions and Manage already saved 
        /// sessions.
        /// </summary>
        private void Fill_New_Connect_Panel(Panel new_connect)
        {
            Label host_label = new Label();
            TextBox host_textbox = new TextBox();
            Label username_label = new Label();
            TextBox username_textbox = new TextBox();
            Label password_label = new Label();
            TextBox password_textbox = new TextBox();
            NumericUpDown numericupdown = new NumericUpDown();
            Label vertical_divide_label = new Label();
            Label search = new Label();
            TextBox search_textbox = new TextBox();
            Label vertical_divide_label_1 = new Label();

            custom_controls.initialize_new_connect_host_label(host_label);
            custom_controls.initialize_new_connect_host_textbox(host_textbox);
            custom_controls.initialize_new_connect_username_label(username_label);
            custom_controls.initialize_new_connect_username_textbox(username_textbox);
            custom_controls.initialize_new_connect_password_label(password_label);
            custom_controls.initialize_new_connect_password_textbox(password_textbox);
            custom_controls.initialize_new_connect_numbericupdown(numericupdown);
            custom_controls.initialize_new_connect_vertical_divide_label(vertical_divide_label);
            custom_controls.initialize_new_connect_search_label(search);
            custom_controls.initialize_new_connect_search_textbox(search_textbox, SearchTextbox_KeyDown);
            custom_controls.initialize_new_connect_vertical_divide_label_1(vertical_divide_label_1);

            new_connect.Controls.Add(host_label);
            new_connect.Controls.Add(host_textbox);
            new_connect.Controls.Add(username_label);
            new_connect.Controls.Add(username_textbox);
            new_connect.Controls.Add(password_label);
            new_connect.Controls.Add(password_textbox);
            new_connect.Controls.Add(numericupdown);
            new_connect.Controls.Add(vertical_divide_label);
            new_connect.Controls.Add(search);
            new_connect.Controls.Add(search_textbox);
            new_connect.Controls.Add(vertical_divide_label_1);
        }

        /// <summary>
        /// Initialize a GroupBox with putty.exe path info and possibility to 
        /// set the path to putty.exe. It's contained withing the main PuTTY STORM configuration
        /// Form (MainForm).
        /// </summary>
        /// <returns>GroupBox container.</returns>
        private GroupBox Add_PuTTY_Config_Container()
        {
            GroupBox putty_config_container = new GroupBox();
            Label putty_path_label = new Label();
            Button set_path_button = new Button();
            set_path_button.Click += new EventHandler(SetPath_Click);

            custom_controls.initialize_putty_config_container(putty_config_container);
            custom_controls.initialize_putty_path_label(putty_path_label);
            custom_controls.initialize_putty_config_button(set_path_button);

            putty_config_container.Controls.Add(putty_path_label);
            putty_config_container.Controls.Add(set_path_button);

            return putty_config_container;
        }

        /// <summary>
        /// 1) Load the main PuTTY STORM configuration Form (MainForm). Load/initialize it with GroupBox containers
        /// and fill controls in it with saved sessions data (from sessions.xml config). 
        /// 
        /// 2) Load/initialize Putty Configuration GroupBox container which contains path to putty.exe with 
        /// possibility to change its configured path (saved in UserSettings).
        /// 
        /// 3) Load/initialize "Add New Entry" button - This button adds new session configuration entry 
        /// GroupBox to the main Form (MainForm).
        /// 
        /// 4) Load/initialize "Advanced" button - This button opens a new Form (AdvancedForm) which provides an interface
        /// to change saved passwords for all configured sessions (per group). And also provides an interface to
        /// manage groups (add/remove).
        /// 
        /// 5) Load/initialize "Connect" button - This button opens a new Form (SessionsForm) - (Putty Sessions Form) which contains
        /// a TabControl with TabPages where putty sessions are started.
        /// 
        /// 6) Load/initialize "SearchSessionConfigTextBox" textbox - Allows easy and fast search based on hostname
        /// 
        /// NOTE: MainForm contains auto-save functionality. When the main Form (MainForm) is closed all sessions are saved to 
        /// the sessions.xml configuration file.
        /// </summary>
        int i = 0;
        List<GroupBox> containers_list;
        MyUserSettings mus;
        GroupBox PuTTY_Config;
        Panel SettingPanel;
        SavedPrivatekeysInfo privatekeys;

        TextBox SearchSessionConfigTextBox;

        private void MainForm_Load(object sender, EventArgs e)
        {   
            // Start Initialization splash screen       
            Splash_Screens.StartScreen.ShowStartScreen();
            Thread.Sleep(2000);

            this.Visible = false;

            // Encrypted private keys have encrypted passphrases, so get them only once during initialization
            // and refresh it once new private key is saved.
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                privatekeys = saved_data.get_PrivateKeys();
            }

            containers_list = new List<GroupBox>();
            mus = new MyUserSettings();

            PuTTY_Config = Add_PuTTY_Config_Container();

            foreach (Control control in PuTTY_Config.Controls)
            {
                if (control.Name == "putty_path_label")
                {
                    control.Text = "Path: " + mus.putty_path;
                }
            }

            SettingPanel = new Panel();
            SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 255);
            SettingPanel.Size = DPIAwareScaling.ScaleSize(250, 120);
            SettingPanel.BackColor = Color.SlateGray;
            SettingPanel.Name = "SettingPanel";

            SearchSessionConfigTextBox = new TextBox();
            SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 380);
            SearchSessionConfigTextBox.Size = DPIAwareScaling.ScaleSize(215, 60);
            SearchSessionConfigTextBox.Name = "SearchSessionConfigTextBox";
            SearchSessionConfigTextBox.Text = "<search hostname>";
            SearchSessionConfigTextBox.Font = new Font("Courier new", DPIAwareScaling.SearchSessionConfigTextBoxFont);
            SearchSessionConfigTextBox.TextAlign = HorizontalAlignment.Center;
            SearchSessionConfigTextBox.ForeColor = SystemColors.GrayText;
            SearchSessionConfigTextBox.KeyDown += new KeyEventHandler(SearchSessionConfigTextBox_KeyDown);
            SearchSessionConfigTextBox.Leave += new EventHandler(SearchSessionConfigTextBox_Leave);
            SearchSessionConfigTextBox.Enter += new EventHandler(SearchSessionConfigTextBox_Enter);

            Button AddEntry = new Button();
            AddEntry.Font = new Font("Calibri", 10);
            AddEntry.Location = DPIAwareScaling.ScalePoint(5, 10);
            AddEntry.Size = DPIAwareScaling.ScaleSize(103, 38);
            AddEntry.Text = "Add New Entry";
            AddEntry.Name = "Add_New_Entry";
            AddEntry.UseVisualStyleBackColor = true;
            AddEntry.Click += new EventHandler(AddEntry_Click);
            AddEntry.FlatStyle = FlatStyle.System;

            Button Connect = new Button();
            Connect.Font = new Font("Calibri", 10);
            Connect.Location = DPIAwareScaling.ScalePoint(120, 5);
            Connect.Size = DPIAwareScaling.ScaleSize(103, 103);
            Connect.Text = "Open";
            Connect.Name = "Connect";
            Connect.UseVisualStyleBackColor = true;
            Connect.Click += new EventHandler(Connect_Click);
            Connect.FlatStyle = FlatStyle.System;

            Button Save_Close = new Button();
            Save_Close.Font = new Font("Calibri", 10);
            Save_Close.Location = DPIAwareScaling.ScalePoint(120, 33);
            Save_Close.Size = DPIAwareScaling.ScaleSize(103, 38);
            Save_Close.Text = "Save and Close";
            Save_Close.Name = "Save_Close";
            Save_Close.UseVisualStyleBackColor = true;
            Save_Close.Click += new EventHandler(Save_Close_Click);
            Save_Close.FlatStyle = FlatStyle.System;
            Save_Close.Hide();

            Button Advanced_Options = new Button();
            Advanced_Options.Font = new Font("Calibri", 10);
            Advanced_Options.Location = DPIAwareScaling.ScalePoint(5, 64);
            Advanced_Options.Size = DPIAwareScaling.ScaleSize(103, 38);
            Advanced_Options.Text = "Advanced";
            Advanced_Options.Name = "Advanced";
            Advanced_Options.UseVisualStyleBackColor = true;
            Advanced_Options.Click += new EventHandler(Advanced_Click);
            Advanced_Options.FlatStyle = FlatStyle.System;

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "sessions.xml")))
            {
                SavedConnectionInfo sessions = saved_data.get_Sessions();

                // If config doesn't contain any saved sessions load just first empty GroupBox container.
                if (sessions.hostnames.Count == 0)
                {
                    containers_list.Add(Add_Main_Component());
                    Controls.Add(containers_list[i]);
                    i++;
                }
                //Otherwise load saved sessions from configuration file.
                else
                {
                    for (int host = 0; host < sessions.hostnames.Count; host++)
                    {
                        // Initialize first GroupBox
                        if (host == 0)
                        {
                            containers_list.Add(Add_Main_Component());
                            foreach (Control control in containers_list[i].Controls)
                            {
                                if (control.Name == "hostname_textbox")
                                {
                                    control.Text = sessions.hostnames[i];
                                }
                                if (control.Name == "username_textbox")
                                {
                                    control.Text = sessions.usernames[i];
                                }
                                if (control.Name == "password_textbox")
                                {
                                    control.Text = sessions.passwords[i];
                                }
                                if (control.Name == "combobox")
                                {
                                    control.Text = sessions.groups[i];
                                }
                                if (control.Name == "sub_groups_combobox")
                                {
                                    control.Text = sessions.sub_groups[i];
                                }

                                foreach (NumericUpDown ctlNumeric in containers_list[i].Controls.OfType<NumericUpDown>())
                                {
                                    ctlNumeric.Value = Convert.ToInt32(sessions.counts[i]);
                                }
                            }
                            Controls.Add(containers_list[i]);
                            i++;
                        }
                        // Initialize rest of GroupBoxes and always take the Y coordinates of the last GroupBox and put 
                        // the next one under last one.
                        else
                        {
                            // For now we allow only 120 sessions
                            if (i == 120)
                            {
                                break;
                            }

                            // Get the Y coordinate of last GroupBox container
                            Point locationOnForm = containers_list[i - 1].FindForm().PointToClient(containers_list[i - 1].
                                Parent.PointToScreen(containers_list[i - 1].Location));
                            int new_groupbox_location = locationOnForm.Y + DPIAwareScaling._ScaleY(240);

                            // Add new GroupBox, set new Y location coordinate, increment its counter
                            containers_list.Add(Add_Main_Component());
                            custom_controls.set_groupbox_location(containers_list[i], new_groupbox_location);
                            custom_controls.set_group_box_text(containers_list[i], (i + 1).ToString());


                            foreach (Control control in containers_list[i].Controls)
                            {
                                if (control.Name == "hostname_textbox")
                                {
                                    control.Text = sessions.hostnames[i];
                                }
                                if (control.Name == "username_textbox")
                                {
                                    control.Text = sessions.usernames[i];
                                }
                                if (control.Name == "password_textbox")
                                {
                                    control.Text = sessions.passwords[i];
                                }
                                if (control.Name == "combobox")
                                {
                                    control.Text = sessions.groups[i];
                                }
                                if (control.Name == "sub_groups_combobox")
                                {
                                    control.Text = sessions.sub_groups[i];
                                }

                                foreach (NumericUpDown ctlNumeric in containers_list[i].Controls.OfType<NumericUpDown>())
                                {
                                    ctlNumeric.Value = Convert.ToInt32(sessions.counts[i]);
                                }
                            }

                            // Initialize the Removal Button - for removing the GroupBox with session data info
                            Button Remove = new Button();
                            custom_controls.initialize_removal_button(Remove, ButtonClickOneEvent);
                            Remove.Tag = i;
                            Remove.Name = "Remove_button";
                            containers_list[i].Controls.Add(Remove);

                            // Add new GoupBox to the Form
                            Controls.Add(containers_list[i]);
                            i++;
                        }
                    }
                }
            }
            // If config does't exists load just an empty first GroupBox.
            else
            {
                containers_list.Add(Add_Main_Component());
                Controls.Add(containers_list[i]);
                i++;
            }

            SettingPanel.Controls.Add(AddEntry);
            SettingPanel.Controls.Add(Connect);
            SettingPanel.Controls.Add(Save_Close);
            SettingPanel.Controls.Add(Advanced_Options);
            Controls.Add(PuTTY_Config);
            Controls.Add(SettingPanel);
            Controls.Add(SearchSessionConfigTextBox);

            // Check which groups in created sessions containers (GroupBoxes) are part
            // of private keys setup. If group is part of PK setup change properties 
            // of password textboxes.
            IsPasswordLess.DetermineIfSessionGroupIsPasswordLess(containers_list);

            this.Visible = true;
            
            // Stop initialization splash screen
            Splash_Screens.StartScreen.CloseStartScreen();

            // Set focus to SettingPanel, so we prevent pogo and scroll issues when scolling
            // with mousewheel or touchpad on laptops when application starts.
            SettingPanel.Focus();

            // Throw a warning message if there is more than 120 session in config. I didn't put the checks
            // to the sessions save/load code as this restriction will be removed!
            if (i == 120)
            {
                MessageBox.Show("Only first 120 sessions allowed! More sessions will not be rendered/loaded! " + 
                    "Please change your config!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// MainForm_Scroll & MainForm_MouseWheel handles user's scrolling up and down via 
        /// sessions configurations, so setting buttons are placed on the same position.
        /// </summary>
        int LastScrollValue = 0;

        private void MainForm_Scroll(object sender, ScrollEventArgs e)
        {
            SettingPanel.Focus();
            
            // When scrollbox is being moved hide SettingPanel and SearchSessionConfigTextBox
            // to prevent flickering
            if (e.Type == ScrollEventType.ThumbTrack)
            {
                SettingPanel.Hide();
                SearchSessionConfigTextBox.Hide();
            }

            if (e.OldValue > e.NewValue)
            {
                if (VerticalScroll.Value < LastScrollValue)
                {                  
                    SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 270);
                    SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 230);
                }
            }
            else
            {
                if (VerticalScroll.Value > 0)
                {
                    LastScrollValue = VerticalScroll.Value;
                    SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 270);
                    SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 230);
                }
            }

            // Move SearchSessionConfigTextBox to its original location when scrolled to top
            if (VerticalScroll.Value == 0)
            {
                SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 255);
                SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 380);
            }

            // When scrollbox stopped to move, show SettingPanel and SearchSessionConfigTextBox again
            if (e.Type != ScrollEventType.ThumbTrack)
            {
                SettingPanel.Show();
                SearchSessionConfigTextBox.Show();
            }
        }

        /// <summary>
        /// Use Timer to detect when MouseWheel scroll event stopped!
        /// </summary>
        System.Windows.Forms.Timer timer;
        float someValue = 0;

        void timer_Tick(object sender, EventArgs e)
        {
            //Prevent timer from looping
            (sender as System.Windows.Forms.Timer).Stop();

            SettingPanel.Show();
            SearchSessionConfigTextBox.Show();

            someValue = 0;
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            SettingPanel.Focus();
            someValue += e.Delta;

            if (e.Delta > 0)
            {
                if (VerticalScroll.Value < LastScrollValue)
                {
                    Console.WriteLine(VerticalScroll.Value + "----" + LastScrollValue);
               
                    SettingPanel.Hide();
                    SearchSessionConfigTextBox.Hide();

                    LastScrollValue = VerticalScroll.Value;
                    SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 270);
                    SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 230);
                }
            } else
            {              
                if (VerticalScroll.Value > 0)
                {
                    // Prevent hiding of SettingPanel and SearchSessionConfigTextBox when
                    // we reach bottom of the Form!
                    if (VerticalScroll.Value != LastScrollValue)
                    {
                        SettingPanel.Hide();
                        SearchSessionConfigTextBox.Hide();
                    }

                    LastScrollValue = VerticalScroll.Value;
                    SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 270);
                    SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 230);
                }
            }

            // Move SearchSessionConfigTextBox to its original location when scrolle to top
            if (VerticalScroll.Value == 0)
            {
                SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 255);
                SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 380);
            }

            timer.Stop();
            timer.Start();

        }

        /// <summary>
        /// Search hostname for fast configuration searching within the main form
        /// </summary>
        private void SearchSessionConfigTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender is TextBox)
                {
                    TextBox txb = (TextBox)sender;

                    if (txb.Text == "")
                    {
                        return;
                    }

                    Regex regex = new Regex(Regex.Escape(txb.Text));

                    foreach (GroupBox container in containers_list)
                    {
                        Control[] hostname_textbox = container.Controls.Find("hostname_textbox", true);

                        Match match = regex.Match(hostname_textbox[0].Text);
                        if (match.Success)
                        {
                            container.Focus();
                            SettingPanel.Location = DPIAwareScaling.ScalePoint(500, 255);
                            SearchSessionConfigTextBox.Location = DPIAwareScaling.ScalePoint(507, 215);
                            txb.Text = "<search hostname>";
                            txb.ForeColor = SystemColors.GrayText;
                            break;
                        }
                    }
                }
                // Disable beep when enter is pressed
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Remove "watermark" text from textbox when user enters it
        /// </summary>
        private void SearchSessionConfigTextBox_Enter(object sender, EventArgs e)
        {
            if (SearchSessionConfigTextBox.Text == "<search hostname>")
            {
                SearchSessionConfigTextBox.Text = "";
                SearchSessionConfigTextBox.ForeColor = SystemColors.WindowText;
            }
        }

        /// <summary>
        /// Add "watermark" text to the textbox when user leaves it
        /// </summary>
        private void SearchSessionConfigTextBox_Leave(object sender, EventArgs e)
        {
            if (SearchSessionConfigTextBox.Text.Length == 0)
            {
                SearchSessionConfigTextBox.Text = "<search hostname>";
                SearchSessionConfigTextBox.ForeColor = SystemColors.GrayText;
            }
        }

        /// <summary>
        /// Force the focus change back to the main PuTTY STORM configuration Form when value in
        /// combobox or numericupdown changes. This prevents these two controls from keeping focus,
        /// so we can conveniently scroll through the saved sessions.
        /// </summary>
        private void Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void Numericupdown_ValueChanged(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        /// <summary>
        /// Auto-save functionality. When the main Form (MainForm) is closed all sessions are
        /// saved to the sessions.xml configuraiton file.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;

            // Start ending splash screen 
            Splash_Screens.StopScreen.ShowStopScreen();
            Thread.Sleep(2000);

            // Save sessions
            sessions.Save_sessions(containers_list);

            // Stop ending splash screen
            Splash_Screens.StopScreen.CloseStopScreen();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Load/Initialize Advanced Form settings (AdvancedForm). Contains possibility to manage groups,
        /// manage private keys setup, change saved session passwords per group and manage login secret.
        /// </summary>
        FormHelper AdvancedForm;
        SplitContainer splitcontainer1;
        List<Panel> Groups;
        int Rows = 0;

        List<Panel> PrivateKeys;
        int PK_Rows = 0;

        Panel section_panel;
        Panel section_panel_private_keys;

        private void Advanced_Click(object sender, EventArgs e)
        {

            Form check_forms = Application.OpenForms["Advanced"];

            if (check_forms != null)
            {
                MessageBox.Show("Advanced Settings Panel is already opened!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Groups = new List<Panel>();
            PrivateKeys = new List<Panel>();

            AdvancedForm = new PuTTY_Storm.FormHelper();
            AdvancedForm.ShowInTaskbar = true;
            AdvancedForm.Size = DPIAwareScaling.ScaleSize(800, 600);
            AdvancedForm.StartPosition = FormStartPosition.CenterScreen;
            AdvancedForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            AdvancedForm.Text = GlobalVar.VERSION + " - Advanced Settings";
            AdvancedForm.FormClosing += new FormClosingEventHandler(AdvancedForm_Closing);
            AdvancedForm.MaximizeBox = false;
            AdvancedForm.Name = "Advanced";

            splitcontainer1 = new SplitContainer();
            splitcontainer1.Dock = DockStyle.Fill;
            splitcontainer1.BackColor = Color.White;
            splitcontainer1.BorderStyle = BorderStyle.FixedSingle;
            splitcontainer1.SplitterDistance = 70;
            splitcontainer1.IsSplitterFixed = true;
            splitcontainer1.Panel1.BackColor = Color.SlateGray;
            splitcontainer1.Panel2.BackColor = Color.SlateGray;

            Label panel1_add_groups_header = new Label();
            custom_controls.initialize_advanced_add_group_label_header(panel1_add_groups_header);
            Label panel1_add_group = new Label();
            custom_controls.initialize_advanced_add_group_label(panel1_add_group);
            TextBox panel1_add_group_textbox = new TextBox();
            custom_controls.initialize_advanced_add_group_textbox(panel1_add_group_textbox);
            Label horizontal_divider = new Label();
            custom_controls.initialize_advanced_group_divider(horizontal_divider);

            Label panel1_private_keys_label = new Label();
            custom_controls.initialize_advanced_private_keys_label_header(panel1_private_keys_label);
            TextBox panel1_private_keys_filedialog_textbox = new TextBox();
            custom_controls.initialize_advanced_private_keys_textbox_filedialog(panel1_private_keys_filedialog_textbox);
            Label panel1_private_keys_keytype_label = new Label();
            custom_controls.initialize_advanced_private_keys_label_keytype(panel1_private_keys_keytype_label);
            ComboBox panel1_private_keys_keytype_combobox = new ComboBox();
            custom_controls.initialize_advanced_private_keys_keytype_combobox(panel1_private_keys_keytype_combobox, Combobox_SelectedIndexChanged);
            Label panel1_private_keys_group_label = new Label();
            custom_controls.initialize_advanced_private_keys_label_group(panel1_private_keys_group_label);
            ComboBox panel1_private_keys_group_combobox = new ComboBox();
            custom_controls.initialize_advanced_private_keys_group_combobox(panel1_private_keys_group_combobox, Combobox_SelectedIndexChanged);
            Label panel1_private_keys_divider = new Label();
            custom_controls.initialize_advanced_private_keys_divider(panel1_private_keys_divider);
            Label panel1_private_keys_passphrase_label = new Label();
            custom_controls.initialize_advanced_private_keys_passphrase_label(panel1_private_keys_passphrase_label);
            TextBox panel1_private_keys_passphrase_textbox = new TextBox();
            custom_controls.initialize_advanced_private_keys_passphrase_textbox(panel1_private_keys_passphrase_textbox);


            Label panel2_passwords_header = new Label();
            custom_controls.initialize_advanced_passwords_label_header(panel2_passwords_header);
            Label panel2_new_password_label = new Label();
            custom_controls.initialize_advanced_new_password_label(panel2_new_password_label);
            TextBox panel2_new_password_textbox = new TextBox();
            custom_controls.initialize_advanced_new_password_textbox(panel2_new_password_textbox);
            Label panel2_new_password_for_group = new Label();
            custom_controls.initialize_advanced_for_password_label(panel2_new_password_for_group);
            ComboBox panel2_passwords_groups = new ComboBox();
            custom_controls.initialize_passwords_combobox(panel2_passwords_groups, Combobox_SelectedIndexChanged);

            Label panel2_horizontal_divider = new Label();
            custom_controls.initialize_panel2_advanced_login_secret_divider(panel2_horizontal_divider);
            Label panel2_main_login_passwd_secret_label = new Label();
            custom_controls.initialize_panel2_advanced_login_main_label(panel2_main_login_passwd_secret_label);
            Label panel2_main_login_old_passwd_label = new Label();
            custom_controls.initialize_panel2_advanced_login_old_passwd_label(panel2_main_login_old_passwd_label);
            Label panel2_main_login_new_passwd_label = new Label();
            custom_controls.initialize_panel2_advanced_login_new_passwd_label(panel2_main_login_new_passwd_label);
            Label panel2_main_login_confirm_new_passwd_label = new Label();
            custom_controls.initialize_panel2_advanced_login_confirm_new_passwd_label(panel2_main_login_confirm_new_passwd_label);
            TextBox panel2_main_login_old_passwd_textbox = new TextBox();
            custom_controls.initialize_panel2_advanced_login_old_passwd_textbox(panel2_main_login_old_passwd_textbox);
            TextBox panel2_main_login_new_passwd_textbox = new TextBox();
            custom_controls.initialize_panel2_advanced_login_new_passwd_textbox(panel2_main_login_new_passwd_textbox);
            TextBox panel2_main_login_confirm_new_passwd_textbox = new TextBox();
            custom_controls.initialize_panel2_advanced_login_confirm_new_passwd_textbox(panel2_main_login_confirm_new_passwd_textbox);

            Button Save_groups_button = new Button();
            Save_groups_button.Font = new Font("Calibri", 10);
            Save_groups_button.Location = DPIAwareScaling.ScalePoint(280, 45);
            Save_groups_button.Size = DPIAwareScaling.ScaleSize(70, 25);
            Save_groups_button.Text = "Save";
            Save_groups_button.Name = "save_groups";
            Save_groups_button.UseVisualStyleBackColor = true;
            Save_groups_button.FlatStyle = FlatStyle.System;
            Save_groups_button.Click += new EventHandler(Save_Groups_Click);

            Button Add_Group_button = new Button();
            Add_Group_button.Font = new Font("Calibri", 10);
            Add_Group_button.Location = DPIAwareScaling.ScalePoint(220, 45);
            Add_Group_button.Size = DPIAwareScaling.ScaleSize(50, 25);
            Add_Group_button.Text = "Add";
            Add_Group_button.Name = "add_group";
            Add_Group_button.UseVisualStyleBackColor = true;
            Add_Group_button.FlatStyle = FlatStyle.System;
            Add_Group_button.Click += new EventHandler(Add_Group_Click);

            Button Set_passwords_button = new Button();
            Set_passwords_button.Font = new Font("Calibri", 10);
            Set_passwords_button.Location = DPIAwareScaling.ScalePoint(260, 150);
            Set_passwords_button.Size = DPIAwareScaling.ScaleSize(100, 35);
            Set_passwords_button.Text = "Set password";
            Set_passwords_button.Name = "set_password";
            Set_passwords_button.UseVisualStyleBackColor = true;
            Set_passwords_button.FlatStyle = FlatStyle.System;
            Set_passwords_button.Click += new EventHandler(Set_Passwords_Click);

            Button Set_new_secret_button = new Button();
            Set_new_secret_button.Font = new Font("Calibri", 10);
            Set_new_secret_button.Location = DPIAwareScaling.ScalePoint(260, 450);
            Set_new_secret_button.Size = DPIAwareScaling.ScaleSize(100, 35);
            Set_new_secret_button.Text = "Set new secret";
            Set_new_secret_button.Name = "set_new_secret";
            Set_new_secret_button.UseVisualStyleBackColor = true;
            Set_new_secret_button.FlatStyle = FlatStyle.System;
            Set_new_secret_button.Click += new EventHandler(Set_New_Secret_Click);

            Button Select_private_key_button = new Button();
            Select_private_key_button.Font = new Font("Calibri", 9);
            Select_private_key_button.Location = DPIAwareScaling.ScalePoint(5, 300);
            Select_private_key_button.Size = DPIAwareScaling.ScaleSize(60, 25);
            Select_private_key_button.Text = "Select";
            Select_private_key_button.Name = "select_private_key";
            Select_private_key_button.UseVisualStyleBackColor = true;
            Select_private_key_button.FlatStyle = FlatStyle.System;
            Select_private_key_button.Click += new EventHandler(Select_private_key_Click);

            Button Add_private_key_button = new Button();
            Add_private_key_button.Font = new Font("Calibri", 10);
            Add_private_key_button.Location = DPIAwareScaling.ScalePoint(220, 380);
            Add_private_key_button.Size = DPIAwareScaling.ScaleSize(60, 25);
            Add_private_key_button.Text = "Add";
            Add_private_key_button.Name = "add_private_key";
            Add_private_key_button.UseVisualStyleBackColor = true;
            Add_private_key_button.FlatStyle = FlatStyle.System;
            Add_private_key_button.Click += new EventHandler(Add_private_key_Click);

            Button Save_private_key_button = new Button();
            Save_private_key_button.Font = new Font("Calibri", 10);
            Save_private_key_button.Location = DPIAwareScaling.ScalePoint(290, 380);
            Save_private_key_button.Size = DPIAwareScaling.ScaleSize(60, 25);
            Save_private_key_button.Text = "Save";
            Save_private_key_button.Name = "save_private_key";
            Save_private_key_button.UseVisualStyleBackColor = true;
            Save_private_key_button.FlatStyle = FlatStyle.System;
            Save_private_key_button.Click += new EventHandler(Save_private_key_Click);

            section_panel = new Panel();
            section_panel.Size = DPIAwareScaling.ScaleSize(355, 140);
            section_panel.BackColor = Color.SlateGray;
            section_panel.Location = DPIAwareScaling.ScalePoint(3, 100);
            section_panel.AutoScroll = true;

            section_panel_private_keys = new Panel();
            section_panel_private_keys.Size = DPIAwareScaling.ScaleSize(355, 130);
            section_panel_private_keys.BackColor = Color.SlateGray;
            section_panel_private_keys.Location = DPIAwareScaling.ScalePoint(3, 430);
            section_panel_private_keys.AutoScroll = true;

            splitcontainer1.Panel1.Controls.Add(panel1_add_groups_header);
            splitcontainer1.Panel1.Controls.Add(panel1_add_group);
            splitcontainer1.Panel1.Controls.Add(panel1_add_group_textbox);
            splitcontainer1.Panel1.Controls.Add(Add_Group_button);
            splitcontainer1.Panel1.Controls.Add(horizontal_divider);
            splitcontainer1.Panel1.Controls.Add(Save_groups_button);
            splitcontainer1.Panel1.Controls.Add(section_panel);

            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_label);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_filedialog_textbox);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_keytype_label);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_keytype_combobox);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_group_label);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_group_combobox);
            splitcontainer1.Panel1.Controls.Add(Select_private_key_button);
            splitcontainer1.Panel1.Controls.Add(Add_private_key_button);
            splitcontainer1.Panel1.Controls.Add(Save_private_key_button);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_divider);
            splitcontainer1.Panel1.Controls.Add(section_panel_private_keys);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_passphrase_label);
            splitcontainer1.Panel1.Controls.Add(panel1_private_keys_passphrase_textbox);

            splitcontainer1.Panel2.Controls.Add(panel2_passwords_header);
            splitcontainer1.Panel2.Controls.Add(panel2_new_password_label);
            splitcontainer1.Panel2.Controls.Add(panel2_new_password_textbox);
            splitcontainer1.Panel2.Controls.Add(panel2_new_password_for_group);
            splitcontainer1.Panel2.Controls.Add(panel2_passwords_groups);
            splitcontainer1.Panel2.Controls.Add(Set_passwords_button);

            splitcontainer1.Panel2.Controls.Add(panel2_horizontal_divider);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_passwd_secret_label);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_old_passwd_label);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_new_passwd_label);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_confirm_new_passwd_label);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_old_passwd_textbox);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_new_passwd_textbox);
            splitcontainer1.Panel2.Controls.Add(panel2_main_login_confirm_new_passwd_textbox);
            splitcontainer1.Panel2.Controls.Add(Set_new_secret_button);

            AdvancedForm.Controls.Add(splitcontainer1);
            AdvancedForm.Show();

            // Load saved groups in groups.xml configuration file.
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_data.get_Groups();

                if (groups.names.Count != 0)
                {
                    foreach (string name in groups.names)
                    {
                        Create_New_Groups_Panel(name);
                    }
                }
            }

            // Load saved private keys in privatekeys.xml configuration file.
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                if (privatekeys.names.Count != 0)
                {
                    for (int name = 0; name < privatekeys.names.Count; name++)
                    {
                        Create_New_PrivateKey_Panel(privatekeys.names[name], privatekeys.types[name], privatekeys.groups[name], privatekeys.pwds[name]);
                    }
                }
            }
        }

        /// <summary>
        /// FileDialog button for private key selecting.
        /// </summary>
        private void Select_private_key_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            string filePath = openFileDialog1.FileName;

            Control[] panel1_private_keys_textbox_filedialog = splitcontainer1.Panel1.Controls.Find("private_keys_textbox_filedialog", true);
            panel1_private_keys_textbox_filedialog[0].Text = filePath;

        }

        /// <summary>
        /// Add selected private key configuration to the list of private keys
        /// - calls Create_New_PrivateKey_Panel().
        /// </summary>
        private void Add_private_key_Click(object sender, EventArgs e)
        {
            string private_key;
            Control[] panel1_private_keys_textbox_filedialog = splitcontainer1.Panel1.Controls.Find("private_keys_textbox_filedialog", true);
            private_key = panel1_private_keys_textbox_filedialog[0].Text;

            string pk_type;
            Control[] panel1_private_keys_keytype_combobox = splitcontainer1.Panel1.Controls.Find("private_keys_keytype_combobox", true);
            pk_type = panel1_private_keys_keytype_combobox[0].Text;

            string pk_group;
            Control[] panel1_private_keys_group_combobox = splitcontainer1.Panel1.Controls.Find("private_keys_group_combobox", true);
            pk_group = panel1_private_keys_group_combobox[0].Text;

            string pk_passphrase;
            Control[] panel1_private_keys_passphrase_textbox = splitcontainer1.Panel1.Controls.Find("private_keys_passphrase_textbox", true);
            pk_passphrase = panel1_private_keys_passphrase_textbox[0].Text;

            if (private_key == "")
            {
                MessageBox.Show("Select private key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (pk_type == "")
            {
                MessageBox.Show("Select private key type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (pk_group == "")
            {
                MessageBox.Show("Select private key group!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Check several states, stop processing and return if:
            // 1) PK Name && PK group && pk type already exists in Private Keys Panel
            // 2) Group is already used in PPK or openSSH PK (one private key [PPK and openSSH] to one group only!)
            foreach (Panel privatekeys_panel in PrivateKeys)
            {
                Control[] pkpanel_name_label = privatekeys_panel.Controls.Find("pk_name_label", true);
                Control[] pkpanel_group_label = privatekeys_panel.Controls.Find("pk_group_label", true);
                Control[] pkpanel_type_label = privatekeys_panel.Controls.Find("pk_type_label", true);

                string pkpanel_name = pkpanel_name_label[0].Text;
                string pkpanel_group = pkpanel_group_label[0].Text.Replace("Group: ", string.Empty);
                string pkpanel_type = pkpanel_type_label[0].Text.Replace("Type: ", string.Empty);

                // Check if PK Name && PK group && pk type already exists in Private Keys Panel
                if ((pkpanel_name == private_key) && (pkpanel_group == pk_group) &&
                    (pkpanel_type == pk_type))
                {
                    MessageBox.Show("Private key " + private_key + " of type " + pk_type +
                        " for group " + pk_group + " already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    panel1_private_keys_textbox_filedialog[0].Text = null;
                    panel1_private_keys_keytype_combobox[0].Text = null;
                    panel1_private_keys_group_combobox[0].Text = null;
                    panel1_private_keys_passphrase_textbox[0].Text = null;
                    return;
                }

                // Check if group is already used in PPK or openSSH PK (one private key [PPK and openSSH] to one group only!)
                if ((pkpanel_group == pk_group) && (pkpanel_type == pk_type))
                {
                    MessageBox.Show("Group " + pk_group + " is already used!" +
                        " Select different group for this private key!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    panel1_private_keys_textbox_filedialog[0].Text = null;
                    panel1_private_keys_keytype_combobox[0].Text = null;
                    panel1_private_keys_group_combobox[0].Text = null;
                    panel1_private_keys_passphrase_textbox[0].Text = null;
                    return;
                }

            }

            Create_New_PrivateKey_Panel(private_key, pk_type, pk_group, pk_passphrase);
            panel1_private_keys_textbox_filedialog[0].Text = null;
            panel1_private_keys_keytype_combobox[0].Text = null;
            panel1_private_keys_group_combobox[0].Text = null;
            panel1_private_keys_passphrase_textbox[0].Text = null;
        }

        /// <summary>
        /// Save private keys configuration to the privatekeys.xml configuration file.
        /// </summary>
        private void Save_private_key_Click(object sender, EventArgs e)
        {
            sessions.Save_PrivateKeys(PrivateKeys);
            MessageBox.Show("PrivateKeys saved to the config!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            IsPasswordLess.DetermineIfSessionGroupIsPasswordLess(containers_list);

            // Refresh private keys after saving
            privatekeys = saved_data.get_PrivateKeys();
        }

        /// <summary>
        /// Check which groups in created sessions containers (GroupBoxes) are part
        /// of private keys setup. If group is part of PK setup change properties 
        /// of password textboxes.
        /// </summary>
        private void combobox_PKGroupChanged(object sender, EventArgs e)
        {
            IsPasswordLess.DetermineIfSessionGroupIsPasswordLess(containers_list);
        }

        /// <summary>
        /// Check which sub-groups in created sessions containers (GroupBoxes) are part
        /// of private keys setup. If sub-group is part of PK setup change properties 
        /// of password textboxes.
        /// </summary>
        private void sub_groups_combobox_PKGroupChanged(object sender, EventArgs e)
        {
            IsPasswordLess.DetermineIfSessionGroupIsPasswordLess(containers_list);
        }

        /// <summary>
        /// Set the new login secret password. Saved it hashed back to the user config and re-save all
        /// sessions passwords encrypted with new login secret back to the sessions.xml file.
        /// </summary>
        private void Set_New_Secret_Click(object sender, EventArgs e)
        {
            Control[] panel2_main_login_old_passwd_textbox = splitcontainer1.Panel2.Controls.Find("panel2_advanced_login_old_passwd_textbox", true);
            string old_saved_secret = mus.password_secret;
            string old_compare_hash = crypto.ComputeHash(panel2_main_login_old_passwd_textbox[0].Text, new SHA256CryptoServiceProvider());
            
            if (old_saved_secret != old_compare_hash)
            {
                MessageBox.Show("Old Password Secret Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                panel2_main_login_old_passwd_textbox[0].Text = null;
                return;
            }

            Control[] panel2_main_login_new_passwd_textbox = splitcontainer1.Panel2.Controls.Find("panel2_advanced_login_new_passwd_textbox", true);
            Control[] panel2_main_login_confirm_new_passwd_textbox = splitcontainer1.Panel2.Controls.Find("panel2_advanced_login_confirm_new_passwd_textbox", true);

            if ((panel2_main_login_new_passwd_textbox[0].Text == panel2_main_login_confirm_new_passwd_textbox[0].Text))
            {
                mus.password_secret = crypto.ComputeHash(panel2_main_login_confirm_new_passwd_textbox[0].Text, new SHA256CryptoServiceProvider());
                GlobalVar.SECRET = panel2_main_login_confirm_new_passwd_textbox[0].Text;
                mus.Save();
                sessions.Save_sessions(containers_list);

                MessageBox.Show("Login Secret Changed Successfully! All Changes Saved Successfully!",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panel2_main_login_old_passwd_textbox[0].Text = null;
                panel2_main_login_new_passwd_textbox[0].Text = null;
                panel2_main_login_confirm_new_passwd_textbox[0].Text = null;
            }
            else
            {
                MessageBox.Show("New Password Secret is not the same!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                panel2_main_login_new_passwd_textbox[0].Text = null;
                panel2_main_login_confirm_new_passwd_textbox[0].Text = null;
                return;
            }
        }

        /// <summary>
        /// Set the new password per group or sub-group and save it back to the sessions.xml configruration file (AdvancedForm).
        /// </summary>
        private void Set_Passwords_Click(object sender, EventArgs e)
        {
            Control[] new_password_textbox = splitcontainer1.Panel2.Controls.Find("advanced_new_password_textbox", true);
            Control[] new_password_combobox_group = splitcontainer1.Panel2.Controls.Find("new_password_combobox", true);

            if (new_password_textbox[0].Text != "" && new_password_combobox_group[0].Text != "")
            {
                bool pwd_changed = false;

                foreach (GroupBox container in containers_list)
                {
                    Control[] password_textbox = container.Controls.Find("password_textbox", true);
                    Control[] combobox_group = container.Controls.Find("combobox", true);
                    Control[] sub_combobox_group = container.Controls.Find("sub_groups_combobox", true);

                    // Change password if primary group is supplied
                    if (combobox_group[0].Text == new_password_combobox_group[0].Text)
                    {
                        password_textbox[0].Text = new_password_textbox[0].Text;
                        pwd_changed = true;
                    }

                    // Change password if sub-group is supplied
                    if (sub_combobox_group[0].Text == new_password_combobox_group[0].Text)
                    {
                        password_textbox[0].Text = new_password_textbox[0].Text;
                        pwd_changed = true;
                    }                 
                }
                if (pwd_changed)
                {
                    sessions.Save_sessions(containers_list);
                    MessageBox.Show("New Password For Group " + new_password_combobox_group[0].Text + " Set And Saved!",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    MessageBox.Show("Group " + new_password_combobox_group[0].Text +
                            " is not a part of saved sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                new_password_textbox[0].Text = null;

            } else
            {
                MessageBox.Show("Enter The New Password With Group First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Save the groups to the groups.xml configuration file and reload the groups list 
        /// in combobox (withing the Form1 sessions and combobox itself in password management
        /// interface) - AdvancedForm.
        /// </summary>
        private void Save_Groups_Click(object sender, EventArgs e)
        {
            sessions.Save_groups(Groups);
            SavedGroupInfo groups = saved_data.get_Groups();
            custom_controls.set_combobox_groups(containers_list, groups);
            custom_controls.set_passwords_combobox_groups(splitcontainer1, groups);
            custom_controls.set_pk_combobox_groups(splitcontainer1, groups);
            MessageBox.Show("Groups Saved To The Config!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Add new group to the panel (Form3).
        /// </summary>
        private void Add_Group_Click(object sender, EventArgs e)
        {
            string group;
            Control[] panel1_add_textbox = splitcontainer1.Panel1.Controls.Find("advanced_add_group_textbox", true);
            group = panel1_add_textbox[0].Text;

            if (group == "")
            {
                MessageBox.Show("Enter The Group Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (group.Length > 12)
            {
                MessageBox.Show("Group name is too long! Name can contain only 12 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Check if the group already exists. If exists stop processing and return!
            foreach (Panel groups_panel in Groups)
            {
                Control[] group_name_label = groups_panel.Controls.Find("group_name_label", true);
                if (group_name_label[0].Text == group)
                {
                    MessageBox.Show("Group " + group + " already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    panel1_add_textbox[0].Text = null;
                    return;
                }
            }

            // Create a new groups panel if group doesn't exists
            Create_New_Groups_Panel(group);
            panel1_add_textbox[0].Text = null;
        }

        /// <summary>
        /// Load/Initialize new groups panel withing the AdvancedForm. When clicking on Add group button,
        /// add new group names one by one with Remove button next to each group.
        /// </summary>
        private void Create_New_Groups_Panel(string group)
        {
            Panel groups_panel = new Panel();
            groups_panel.Size = DPIAwareScaling.ScaleSize(330, 50);

            if (Groups.Count == 0)
            {
                groups_panel.Location = DPIAwareScaling.ScalePoint(0, 0);
            }
            else
            {
                // Get the Y coordinate of last GroupBox container
                Point locationOnForm = Groups[Rows - 1].FindForm().PointToClient(Groups[Rows - 1].
                    Parent.PointToScreen(Groups[Rows - 1].Location));
                int new_groups_panel_location = locationOnForm.Y - DPIAwareScaling._ScaleY(70);
               
                groups_panel.Location = new Point(0, new_groups_panel_location);
            }

            Button Remove_Group_Button = new Button();
            custom_controls.initialize_advanced_remove_group_button(Remove_Group_Button, ButtonClickGroupEvent);
            Remove_Group_Button.Tag = Rows;

            Label group_name = new Label();
            group_name.UseMnemonic = false;
            group_name.Text = group;
            group_name.Font = new Font("Calibri", 14);
            group_name.Location = DPIAwareScaling.ScalePoint(3, 20);
            group_name.Size = DPIAwareScaling.ScaleSize(240, 30);
            group_name.ForeColor = Color.White;
            group_name.Name = "group_name_label";


            groups_panel.Controls.Add(group_name);
            groups_panel.Controls.Add(Remove_Group_Button);

            section_panel.Controls.Add(groups_panel);

            Groups.Add(groups_panel);

            Rows++;
        }

        /// <summary>
        /// Load/Initialize new private keys panel withing the AdvancedForm. When clicking on Add private key button,
        /// add new private keys name, type and group one by one with Remove button next to each private key setup information.
        /// </summary>
        private void Create_New_PrivateKey_Panel(string pk, string type, string group, string pk_passphrase)
        {
            Panel pk_panel = new Panel();
            pk_panel.Size = DPIAwareScaling.ScaleSize(335, 120);

            if (PrivateKeys.Count == 0)
            {
                pk_panel.Location = DPIAwareScaling.ScalePoint(0, 0);
            } else
            {
                // Get the Y coordinate of last GroupBox container
                Point locationOnForm = PrivateKeys[PK_Rows - 1].FindForm().PointToClient(PrivateKeys[PK_Rows - 1].
                    Parent.PointToScreen(PrivateKeys[PK_Rows - 1].Location));
                int new_pks_panel_location = locationOnForm.Y - DPIAwareScaling._ScaleY(330);

                pk_panel.Location = new Point (0, new_pks_panel_location);
            }

            Button Remove_PK_Button = new Button();
            custom_controls.initialize_advanced_remove_pk_button(Remove_PK_Button, ButtonClickPKEvent);
            Remove_PK_Button.Tag = PK_Rows;

            Label pk_name = new Label();
            pk_name.UseMnemonic = false;
            pk_name.Text = pk;
            pk_name.Font = new Font("Calibri", 9);
            pk_name.Location = DPIAwareScaling.ScalePoint(3, 60);
            pk_name.Size = DPIAwareScaling.ScaleSize(250, 80);
            pk_name.ForeColor = Color.White;
            pk_name.Name = "pk_name_label";

            Label pk_group = new Label();
            pk_group.UseMnemonic = false;
            pk_group.Text = "Group: " + group;
            pk_group.Font = new Font("Calibri", 9);
            pk_group.Location = DPIAwareScaling.ScalePoint(3, 20);
            pk_group.Size = DPIAwareScaling.ScaleSize(125, 20);
            pk_group.ForeColor = Color.White;
            pk_group.Name = "pk_group_label";

            Label pk_type = new Label();
            pk_type.UseMnemonic = false;
            pk_type.Text = "Type: " + type;
            pk_type.Font = new Font("Calibri", 9);
            pk_type.Location = DPIAwareScaling.ScalePoint(128, 20);
            pk_type.Size = DPIAwareScaling.ScaleSize(125, 20);
            pk_type.ForeColor = Color.White;
            pk_type.Name = "pk_type_label";

            Label passphrase = new Label();
            passphrase.UseMnemonic = false;
            if (pk_passphrase == null || pk_passphrase == "")
            {
                passphrase.Text = "PWD: NO";
            } else
            {
                passphrase.Text = "PWD: YES";
            }
            passphrase.Font = new Font("Calibri", 9);
            passphrase.Location = DPIAwareScaling.ScalePoint(3, 40);
            passphrase.Size = DPIAwareScaling.ScaleSize(125, 20);
            passphrase.ForeColor = Color.White;
            passphrase.Name = "pk_passphrase_label";

            TextBox hidden_passphrase_textbox = new TextBox();
            hidden_passphrase_textbox.Name = "private_keys_hidden_passphrase_textbox";
            hidden_passphrase_textbox.UseSystemPasswordChar = true;
            hidden_passphrase_textbox.Text = pk_passphrase;
            hidden_passphrase_textbox.Visible = false;

            pk_panel.Controls.Add(pk_group);
            pk_panel.Controls.Add(pk_type);
            pk_panel.Controls.Add(passphrase);
            pk_panel.Controls.Add(hidden_passphrase_textbox);
            pk_panel.Controls.Add(pk_name);
            pk_panel.Controls.Add(Remove_PK_Button);

            section_panel_private_keys.Controls.Add(pk_panel);

            PrivateKeys.Add(pk_panel);

            PK_Rows++;
        }

        /// <summary>
        /// Handle removing of private keys (AdvancedForm).
        /// </summary>
        private void ButtonClickPKEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int tag = (int)button.Tag;

            if (button != null)
            {
                section_panel_private_keys.Controls.Remove(PrivateKeys[tag]);
                PrivateKeys.RemoveAt(tag);

                int r = 0;
                foreach (Panel panel in PrivateKeys)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control.Name == "Remove_PK_button")
                        {
                            control.Tag = r;
                            r++;
                        }
                    }
                }

                for (int g = tag; g < PrivateKeys.Count; g++)
                {
                    Point locationOnForm = PrivateKeys[g].FindForm().PointToClient(PrivateKeys[g].
                        Parent.PointToScreen(PrivateKeys[g].Location));
                    int new_pk_panel_location = locationOnForm.Y - DPIAwareScaling._ScaleY(530);

                    Panel temp = PrivateKeys[g];
                    temp.Location = new Point (0, new_pk_panel_location);
                }
                PK_Rows--;
            }
        }

        /// <summary>
        /// Handle removing of groups (AdvancedForm).
        /// </summary>
        private void ButtonClickGroupEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int tag = (int)button.Tag;

            if (button != null)
            {
                section_panel.Controls.Remove(Groups[tag]);
                Groups.RemoveAt(tag);

                int r = 0;
                foreach (Panel panel in Groups)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control.Name == "Remove_Group_button")
                        {
                            control.Tag = r;
                            r++;
                        }
                    }
                }

                for (int g = tag; g < Groups.Count; g++)
                {
                    Point locationOnForm = Groups[g].FindForm().PointToClient(Groups[g].
                        Parent.PointToScreen(Groups[g].Location));
                    int new_groups_panel_location = locationOnForm.Y - DPIAwareScaling._ScaleY(131);

                    Panel temp = Groups[g];
                    temp.Location = new Point(0, new_groups_panel_location);
                }
                Rows--;
            }
        }

        private void AdvancedForm_Closing(object sender, FormClosingEventArgs e)
        {
            AdvancedForm = null;
            Rows = 0;
            PK_Rows = 0;

            for (int i = 0; i < Groups.Count; i++)
            {
                Groups.RemoveAt(i);
            }

            for (int i = 0; i < PrivateKeys.Count; i++)
            {
                PrivateKeys.RemoveAt(i);
            }
        }

        /// <summary>
        /// Handle setting putty.exe path settings. And saving to configuration file (UserConfig).
        /// Contained within the MainForm.
        /// </summary>
        private void SetPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            string filePath = openFileDialog1.FileName;

            mus.putty_path = filePath;

            foreach (Control control in PuTTY_Config.Controls)
            {
                if (control.Name == "putty_path_label")
                {
                    if (mus.putty_path != "")
                    {
                        control.Text = "Path: " + mus.putty_path;
                    } else
                    {
                        mus.putty_path = "<N/A - Set PuTTY Path>";
                        control.Text = mus.putty_path;
                    }
                }
            }
            mus.Save();
        }

        private void main_header_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Add a new empty session GroupBox container to the MainForm.
        /// </summary>
        private void AddEntry_Click(object sender, EventArgs e)
        {
            if (i == 120)
            {
                MessageBox.Show("Maximum allowed sessions!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            custom_controls = new SetControls();

            // Get the Y coordinate of last GroupBox container
            Point locationOnForm = containers_list[i-1].FindForm().PointToClient(containers_list[i-1].
                Parent.PointToScreen(containers_list[i-1].Location));
            int new_groupbox_location = locationOnForm.Y + DPIAwareScaling._ScaleY(240);

            // Add new GroupBox, set new Y location coordinate, increment its counter
            containers_list.Add(Add_Main_Component());
            custom_controls.set_groupbox_location(containers_list[i], new_groupbox_location);
            custom_controls.set_group_box_text(containers_list[i], (i + 1).ToString());

            // Initialize Removal Button
            Button Remove = new Button();
            custom_controls.initialize_removal_button(Remove, ButtonClickOneEvent);
            Remove.Tag = i;
            Remove.Name = "Remove_button";
            containers_list[i].Controls.Add(Remove);
            
            // Add new GoupBox to the Form
            Controls.Add(containers_list[i]);
            i++;
        }

        /// <summary>
        /// Opens a new PuTTY STORM sessions Form (SessionsForm).
        /// This Form contains several parts:
        /// 
        /// 1) TOP - This is the panel contained within the top of PuTTY STORM sessions Form (SessionsForm). 
        /// It contains controls to open new putty session, search for saved sessions, manage already saved
        /// sessions and various settings, manager GlobalHotKeys configuration and Show/Hide side panel with
        /// servers.
        /// 
        /// 2) TabControls with split screen implemeneted where putty sessions are opened inside of TabPages.
        /// 
        /// 3) TreeView on right side with server names divided to groups and sub-groups.
        /// </summary>
        DraggableTabControl tabcontrol1;
        DraggableTabControl tabcontrol2;
        SessionsForm SessionsForm;
        List<Panel> panelsList;
        List<ProcessInfo> my_ProcessInfo_List_TC_1;
        List<ProcessInfo> my_ProcessInfo_List_TC_2;
        Panel new_connect_panel;
        OpenPuTTY now;
        SplitContainer SessionsSplitContainer;
        TreeView SimpleServerPane;
        Panel SimpleServerPanePanel;
        SplitContainer SessionSplitContainerAndServerPanel;

        // GlobalHotKeys handling (registration/unregistration).
        GlobalHotKeysWorker TabPagesForwardGlobalHotKeyWorker;
        GlobalHotKeysWorker TabPagesBackwardGlobalHotKeyWorker;
        GlobalHotKeysWorker SplitScreenGlobalHotKeyWorker;
        GlobalHotKeysWorker SFTPManagerGlobalHotKeyWorker;
        GlobalHotKeysWorker KotarakGlobalHotKeyWorker;

        private void Connect_Click(object sender, EventArgs e)
        {
            if (!File.Exists(mus.putty_path))
            {
                MessageBox.Show("Select Path To Putty.exe First!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            TabPagesForwardGlobalHotKeyWorker = new GlobalHotKeysWorker("TabPagesForward");
            TabPagesBackwardGlobalHotKeyWorker = new GlobalHotKeysWorker("TabPagesBackward");
            SplitScreenGlobalHotKeyWorker = new GlobalHotKeysWorker("SplitScreen");
            SFTPManagerGlobalHotKeyWorker = new GlobalHotKeysWorker("SFTPManager");
            KotarakGlobalHotKeyWorker = new GlobalHotKeysWorker("Kotarak");

            panelsList = new List<Panel>();
            my_ProcessInfo_List_TC_1 = new List<ProcessInfo>();
            my_ProcessInfo_List_TC_2 = new List<ProcessInfo>();
            SessionsSplitContainer = new SplitContainer();           

            tabcontrol1 = new DraggableTabControl(my_ProcessInfo_List_TC_1, my_ProcessInfo_List_TC_2, SessionsSplitContainer);
            tabcontrol2 = new DraggableTabControl(my_ProcessInfo_List_TC_1, my_ProcessInfo_List_TC_2, SessionsSplitContainer);

            SessionsSplitContainer.Dock = DockStyle.Fill;
            SessionsSplitContainer.BorderStyle = BorderStyle.Fixed3D;
            SessionsSplitContainer.SplitterDistance = DPIAwareScaling.TabControlSplitterDistance;
            SessionsSplitContainer.Panel1.AutoScroll = true;
            SessionsSplitContainer.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);

            SessionsForm = new SessionsForm(my_ProcessInfo_List_TC_1, tabcontrol1, tabcontrol2, SessionsSplitContainer, containers_list,
                TabPagesForwardGlobalHotKeyWorker, TabPagesBackwardGlobalHotKeyWorker, SplitScreenGlobalHotKeyWorker, SFTPManagerGlobalHotKeyWorker,
                KotarakGlobalHotKeyWorker);
            SessionsForm.FormClosed += new FormClosedEventHandler(SessionsForm_FormClosed);
            SessionsForm.StartPosition = FormStartPosition.CenterScreen;
            this.Hide();

            int i;
            now = new OpenPuTTY();

            for (int c = 0; c < containers_list.Count; c++)
            {
                string hostname = null;
                string username = null;
                string password = null;
                string group = null;
                string sub_group = null;
                string PrivateKey = null;
                int c_count = 0;

                foreach (Control control in containers_list[c].Controls)
                {
                    if (control.Name == "hostname_textbox")
                    {
                        hostname = control.Text;
                    }
                    if (control.Name == "username_textbox")
                    {
                        username = control.Text;
                    }
                    if (control.Name == "password_textbox")
                    {
                        password = control.Text;
                    }
                    if (control.Name == "combobox")
                    {
                        group = control.Text;
                    }
                    if (control.Name == "sub_groups_combobox")
                    {
                        sub_group = control.Text;
                    }
                }

                foreach (NumericUpDown ctlNumeric in containers_list[c].Controls.OfType<NumericUpDown>())
                {
                    c_count = (int)ctlNumeric.Value;
                }

                // In case we are going to use private key to login - check if it exists first!
                // And also check if group and sub-group in this session is part of private keys setup.
                // If yes, then use passwordless login.
                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "PuTTYStorm", "privatekeys.xml")))
                {
                    // Check if group or su-group is part of private keys configuration setup
                    if ((IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, group)) ||
                        (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, sub_group)))
                    {
                        Console.WriteLine("#### GROUP CHECK IN LOGIN: " + group + " and " + sub_group);

                        // Fetch private key for group 
                        PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, group);

                        // If private key is still null, then sub-group is part of its setup - fetch it!
                        if (PrivateKey == null)
                        {
                            Console.WriteLine("## Sub-group is part of pwdess login!");
                            PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, sub_group);
                        }

                        // If private key doesn't exists or is still null then something is wrong! Stop processing and return!
                        if (!File.Exists(PrivateKey))
                        {
                            if (PrivateKey == null || PrivateKey == "")
                            {
                                PrivateKey = "of type PPK or its group";
                            }
                            MessageBox.Show("You are going to use passwordless login, " + Environment.NewLine +
                                "however private key " + PrivateKey + " doesn't exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                            this.Show();
                            tabcontrol1.Dispose();
                            tabcontrol2.Dispose();
                            SessionsForm.Dispose();
                            panelsList = null;
                            my_ProcessInfo_List_TC_1 = null;
                            my_ProcessInfo_List_TC_2 = null;
                            now = null;
                            SessionsSplitContainer.Dispose();
                            TabPagesForwardGlobalHotKeyWorker = null;
                            TabPagesBackwardGlobalHotKeyWorker = null;
                            SplitScreenGlobalHotKeyWorker = null;
                            SFTPManagerGlobalHotKeyWorker = null;
                            KotarakGlobalHotKeyWorker = null;

                            return;
                        }

                        password = null;
                    }
                }

                // If the hostname string is a valid IP address then show
                // the IP address in a session's tabpage. Otherwise show only
                // hostname's shortname.
                string shortname = custom_controls.SetSessionTabShortname(hostname);
                
                for (i = 0; i < c_count; i++)
                {
                    Process process = new Process();
                    process.EnableRaisingEvents = true;
                    process.Exited += new EventHandler(Process_Exited);
                    Panel panels = new Panel();
                    TabPage tabpage = new TabPage(shortname);
                    tabpage.Font = new Font("Calibri", 10);
                    panels.AutoSize = true;
                    panels.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(panels);
                    my_ProcessInfo_List_TC_1.Add(now.start_putty(panels, i, process, hostname, username, password, mus.putty_path, SessionsSplitContainer, PrivateKey));
                    tabcontrol1.TabPages.Add(tabpage);
                }
            }

            tabcontrol1.MouseClick += new MouseEventHandler(Tabcontrol1_MouseClick);
            tabcontrol1.Appearance = TabAppearance.FlatButtons;
            tabcontrol1.Resize += new EventHandler(Tabcontrol1_Resize);
            tabcontrol1.HandleDestroyed += new EventHandler(Tabcontrol1_HandleDestroyed);
            tabcontrol1.Font = new Font("Calibri", DPIAwareScaling.TabcontrolFont);
            tabcontrol1.ItemSize = DPIAwareScaling.ScaleSize(50, 25);

            tabcontrol2.MouseClick += new MouseEventHandler(Tabcontrol2_MouseClick);
            tabcontrol2.Resize += new EventHandler(Tabcontrol2_Resize);
            tabcontrol2.Appearance = TabAppearance.FlatButtons;
            tabcontrol2.HandleDestroyed += new EventHandler(Tabcontrol2_HandleDestroyed);
            tabcontrol2.Font = new Font("Calibri", DPIAwareScaling.TabcontrolFont);
            tabcontrol2.ItemSize = DPIAwareScaling.ScaleSize(50, 25);

            Panel tabcontrol_panel = new Panel();
            tabcontrol_panel.Dock = DockStyle.Fill;

            Panel tabcontrol_panel_2 = new Panel();
            tabcontrol_panel_2.Dock = DockStyle.Fill;

            new_connect_panel = new Panel();
            new_connect_panel.Size = DPIAwareScaling.ScaleSize(0, 30);
            new_connect_panel.Dock = DockStyle.Top;
            new_connect_panel.BackColor = SystemColors.Control;
            new_connect_panel.BorderStyle = BorderStyle.FixedSingle;
            Fill_New_Connect_Panel(new_connect_panel);

            // TreeView docked within the SimpleServerPanePanel
            SimpleServerPane = new TreeView();
            SimpleServerPane.Dock = DockStyle.Fill;
            SimpleServerPane.Font = new Font("courier new", 9);
            SimpleServerPane.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(ServerPane_NodeMouseDoubleClick);
            custom_controls.LoadTreeViewPane(SimpleServerPane, containers_list, ServerPane_NodeMouseDoubleClick);

            // SimpleServerPanePanel docked within the SessionSplitContainerAndServerPanel.Panel2 (SplitContainer).
            SimpleServerPanePanel = new Panel();
            SimpleServerPanePanel.Dock = DockStyle.Fill;
            SimpleServerPanePanel.Size = DPIAwareScaling.ScaleSize(300, 0);

            // This SplitContainer contains above SimpleServerPanePanel in Panel2 and the most important
            // SessionsSplitContainer in Panel1 (which contains tabcontrols with sessions).
            SessionSplitContainerAndServerPanel = new SplitContainer();
            SessionSplitContainerAndServerPanel.Dock = DockStyle.Fill;
            SessionSplitContainerAndServerPanel.SplitterDistance = DPIAwareScaling._ScaleX(400);
            SessionSplitContainerAndServerPanel.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);

            Button new_connect_button = new Button();
            new_connect_button.Font = new Font("Calibri", 9);
            new_connect_button.Location = DPIAwareScaling.ScalePoint(630, 3);
            new_connect_button.Size = DPIAwareScaling.ScaleSize(70, 23);
            new_connect_button.Text = "Connect";
            new_connect_button.Click += new EventHandler(New_Connect_Click);
            new_connect_panel.Controls.Add(new_connect_button);

            Button manage_sessions_button = new Button();
            manage_sessions_button.Font = new Font("Calibri", 9);
            manage_sessions_button.Location = DPIAwareScaling.ScalePoint(930, 3);
            manage_sessions_button.Size = DPIAwareScaling.ScaleSize(80, 23);
            manage_sessions_button.Text = "Settings";
            manage_sessions_button.Click += new EventHandler(Manage_Sessions_Click);
            new_connect_panel.Controls.Add(manage_sessions_button);

            Button global_hot_keys_button = new Button();
            global_hot_keys_button.Font = new Font("Calibri", 9);
            global_hot_keys_button.Location = DPIAwareScaling.ScalePoint(1020, 3);
            global_hot_keys_button.Size = DPIAwareScaling.ScaleSize(110, 23);
            global_hot_keys_button.Text = "GlobalHotKeys";
            global_hot_keys_button.Click += new EventHandler(Global_Hot_Keys_Click);
            new_connect_panel.Controls.Add(global_hot_keys_button);

            Button show_server_pane_button = new Button();
            show_server_pane_button.Font = new Font("Calibri", 9);
            show_server_pane_button.Location = DPIAwareScaling.ScalePoint(1150, 3);
            show_server_pane_button.Size = DPIAwareScaling.ScaleSize(130, 23);
            show_server_pane_button.Text = "HideServersPanel";
            show_server_pane_button.Click += new EventHandler(Show_server_pane_Click);
            new_connect_panel.Controls.Add(show_server_pane_button);
           
            SessionsForm.ShowInTaskbar = true;
            SessionsForm.KeyPreview = true;
            SessionsForm.Size = DPIAwareScaling.ScaleSize(1200, 800);
            SessionsForm.StartPosition = FormStartPosition.CenterScreen;
            SessionsForm.AutoSize = true;
            SessionsForm.Text = GlobalVar.VERSION + " - Sessions";
            SessionsForm.Name = "Sessions";
            SessionsForm.ResizeEnd += new EventHandler(SessionsForm_ResizeEnd);
            SessionsForm.SizeChanged += new EventHandler(SessionsForm_SizeChanged);
            tabcontrol1.MouseEnter += new EventHandler(Tabcontrol1_MouseEnter);
            tabcontrol2.MouseEnter += new EventHandler(Tabcontrol2_MouseEnter);

            tabcontrol1.Dock = DockStyle.Fill;
            tabcontrol1.AutoSize = true;
            tabcontrol1.Name = "TABCONTROL_1";

            tabcontrol2.Dock = DockStyle.Fill;
            tabcontrol2.AutoSize = true;
            tabcontrol2.Name = "TABCONTROL_2";

            tabcontrol_panel.Controls.Add(tabcontrol1);
            tabcontrol_panel_2.Controls.Add(tabcontrol2);

            SessionsSplitContainer.Panel1.Controls.Add(tabcontrol_panel);
            SessionsSplitContainer.Panel2.Controls.Add(tabcontrol_panel_2);
            SessionsSplitContainer.Panel2Collapsed = true;

            SimpleServerPanePanel.Controls.Add(SimpleServerPane);

            SessionSplitContainerAndServerPanel.Panel2.Controls.Add(SimpleServerPanePanel);
            SessionSplitContainerAndServerPanel.Panel1.Controls.Add(SessionsSplitContainer);

            SessionsForm.Controls.Add(SessionSplitContainerAndServerPanel);
            SessionsForm.Controls.Add(new_connect_panel);

            SessionsForm.Show();
        }

        /// <summary>
        /// Handle focus between SessionsForm/TabControl1 and putty.exe window.
        /// </summary>
        private void Tabcontrol1_MouseEnter(object sender, EventArgs e)
        {
            if (!tabcontrol1.Focused)
            {
                Form SFTPManager = Application.OpenForms["SFTPManager"];
                Form SelectSFTPConnectionForm = Application.OpenForms["SelectSFTPConnectionForm"];
                Form Kotarak = Application.OpenForms["kotarak"];
                Form GlobalHotKeysManager = Application.OpenForms["GlobalHotKeysManager"];

                Control[] new_connect_panel_host_textbox = new_connect_panel.Controls.Find("new_connect_host_textbox", true);
                Control[] new_connect_panel_username_textbox = new_connect_panel.Controls.Find("new_connect_username_textbox", true);
                Control[] new_connect_panel_password_textbox = new_connect_panel.Controls.Find("new_connect_password_textbox", true);

                if (!((this.Visible) || (SFTPManager != null && SFTPManager.Visible) || (SelectSFTPConnectionForm != null && SelectSFTPConnectionForm.Visible) 
                    || (Kotarak != null && Kotarak.Visible) || (GlobalHotKeysManager != null && GlobalHotKeysManager.Visible)
                    || new_connect_panel_host_textbox[0].Focused || new_connect_panel_username_textbox[0].Focused
                    || new_connect_panel_password_textbox[0].Focused))
                {
                    if (my_ProcessInfo_List_TC_1.Count > 0) {
                        tabcontrol1.Focus();
                        NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                        FlashWindow(SessionsForm.Handle, FLASHW_STOP);
                        Console.WriteLine("Tabcontrol1 focused");
                    }
                }
            }
        }

        /// <summary>
        /// Handle focus between SessionsForm/TabControl2 and putty.exe window.
        /// </summary>
        private void Tabcontrol2_MouseEnter(object sender, EventArgs e)
        {
            if (!tabcontrol2.Focused)
            {
                Form SFTPManager = Application.OpenForms["SFTPManager"];
                Form SelectSFTPConnectionForm = Application.OpenForms["SelectSFTPConnectionForm"];
                Form Kotarak = Application.OpenForms["kotarak"];
                Form GlobalHotKeysManager = Application.OpenForms["GlobalHotKeysManager"];

                Control[] new_connect_panel_host_textbox = new_connect_panel.Controls.Find("new_connect_host_textbox", true);
                Control[] new_connect_panel_username_textbox = new_connect_panel.Controls.Find("new_connect_username_textbox", true);
                Control[] new_connect_panel_password_textbox = new_connect_panel.Controls.Find("new_connect_password_textbox", true);

                if (!((this.Visible) || (SFTPManager != null && SFTPManager.Visible) || (SelectSFTPConnectionForm != null && SelectSFTPConnectionForm.Visible) 
                    || (Kotarak != null && Kotarak.Visible) || (GlobalHotKeysManager != null && GlobalHotKeysManager.Visible)
                    || new_connect_panel_host_textbox[0].Focused || new_connect_panel_username_textbox[0].Focused
                    || new_connect_panel_password_textbox[0].Focused))
                {
                    if (my_ProcessInfo_List_TC_2.Count > 0)
                    {
                        tabcontrol2.Focus();
                        NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_2.ElementAt(tabcontrol2.SelectedIndex).mainhandle);
                        FlashWindow(SessionsForm.Handle, FLASHW_STOP);
                        Console.WriteLine("Tabcontrol2 focused");
                    }
                }
            }
        }

        private void SessionsForm_ResizeEnd(object sender, EventArgs e)
        {
            if (my_ProcessInfo_List_TC_1.Count > 0)
            {
                NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
            }
        }

        string Previous_WindowState = null;

        private void SessionsForm_SizeChanged(object sender, EventArgs e)
        {
            if (my_ProcessInfo_List_TC_1.Count > 0)
            {
                if (SessionsForm.WindowState == FormWindowState.Maximized)
                {
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                    Previous_WindowState = "Maximized";
                    Console.WriteLine("SessionsForm maximized");
                }

                if (SessionsForm.WindowState == FormWindowState.Minimized)
                {
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                    Previous_WindowState = "Minimized";
                    Console.WriteLine("SessionsForm minimized");
                }

                if (SessionsForm.WindowState == FormWindowState.Normal)
                {
                    if (Previous_WindowState == "Maximized" || Previous_WindowState == "Minimized")
                    {
                        NativeMethods.SetForegroundWindow(my_ProcessInfo_List_TC_1.ElementAt(tabcontrol1.SelectedIndex).mainhandle);
                        Previous_WindowState = "Normal";
                        Console.WriteLine("SessionsForm normal");
                    }
                }
            }
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// This is the search texbox contained within the panel placed on top of SessionsForm (PuTTY STORM sessions Form).
        /// It provides the functionality to find a saved session in sessions.xml configuration file.
        /// And on enter it fills the conenction info (host, username, password, number of sessions) controls in the
        /// top panel.
        /// </summary>
        public void SearchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender is TextBox)
                {
                    TextBox txb = (TextBox)sender;

                    if (txb.Text == "")
                    {
                        return;
                    }

                    string hostname = null;
                    string username = null;
                    string password = null;

                    Regex regex = new Regex(Regex.Escape(txb.Text));

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

                    foreach (Control control in new_connect_panel.Controls)
                    {
                        if (control.Name == "new_connect_host_textbox")
                        {
                            control.Text = hostname;
                        }
                        if (control.Name == "new_connect_username_textbox")
                        {
                            control.Text = username;
                        }
                        if (control.Name == "new_connect_password_textbox")
                        {
                            TextBox password_txtbox = (TextBox)control;

                            // In case session group or sub-group is part of private keys setup,
                            // change new_connect_password_textbox attributes.
                            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                "PuTTYStorm", "privatekeys.xml")))
                            {
                                string[] hostname_groups = IsPasswordLess.GetGroupsForPwdLessHostname(containers_list, hostname);

                                if ((IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, hostname_groups[0])) ||
                                    (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, hostname_groups[1])))
                                {
                                    password_txtbox.Text = "PWDLESS";
                                    password_txtbox.ReadOnly = true;
                                    password_txtbox.BackColor = Color.White;
                                    password_txtbox.ForeColor = Color.SlateGray;
                                    password_txtbox.UseSystemPasswordChar = false;
                                }
                                else
                                {
                                    password_txtbox.Text = password;
                                    password_txtbox.UseSystemPasswordChar = true;
                                    password_txtbox.ForeColor = Color.Black;
                                    password_txtbox.ReadOnly = false;
                                    password_txtbox.BackColor = Color.White;
                                }
                            } else
                            {
                                password_txtbox.Text = password;
                            }                                
                        }
                    }
                }
                // Disable beep when enter is pressed
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// This is the button placed within the panel on top of SessionsForm (PuTTY STORM sessions Form).
        /// It opens the main configuration Form (MainForm), however without "Connect" button.
        /// Instead of "Connect" button it shows "Save and Close" button which will save the new 
        /// sessions to the sessions.xml configuration file and again hide the main configuration
        /// Form (MainForm) window.
        /// </summary>
        private void Manage_Sessions_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ControlBox = false;
            Control[] Connect = this.Controls.Find("Connect", true);
            Connect[0].Hide();

            Control[] Save_Close = this.Controls.Find("Save_Close", true);
            Save_Close[0].Show();
        }

        /// <summary>
        /// "Save and Close" button related to "Manage_sessions_Click". It saves new sessions to the
        /// sessions.xml configuration file and hides MainForm.
        /// </summary>
        private void Save_Close_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form check_forms = Application.OpenForms["Sessions"];

            if (check_forms != null)
            {
                custom_controls.LoadTreeViewPane(SimpleServerPane, containers_list, ServerPane_NodeMouseDoubleClick);
            }

            Control[] Connect = this.Controls.Find("Connect", true);
            Connect[0].Show();

            Control[] Save_Close = this.Controls.Find("Save_Close", true);
            Save_Close[0].Hide();
            this.ControlBox = true;

            sessions.Save_sessions(containers_list);

            //Form check_forms = Application.OpenForms["Sessions"];

            if (check_forms == null)
            {
                this.Close();
            }
        }

        /// <summary>
        /// "Connect" button contained within the panel placed on the top of PuTTY STORM sessions Form (SessionsForm).
        /// It opens new putty.exe process docked within the TabControl's Tabpage.
        /// </summary>
        private void New_Connect_Click(object sender, EventArgs e)
        {
            string new_hostname = null;
            string new_username = null;
            string new_password = null;
            string[] new_hostname_groups = null;
            string PrivateKey = null;
            int new_c_count = 0;

            foreach (Control control in new_connect_panel.Controls)
            {
                if (control.Name == "new_connect_host_textbox")
                {
                    new_hostname = control.Text;
                }
                if (control.Name == "new_connect_username_textbox")
                {
                    new_username = control.Text;
                }
                if (control.Name == "new_connect_password_textbox")
                {
                    new_password = control.Text;
                }
            }

            foreach (NumericUpDown ctlNumeric in new_connect_panel.Controls.OfType<NumericUpDown>())
            {
                new_c_count = (int)ctlNumeric.Value;
            }

            // If the hostname string is a valid IP address then show
            // the IP address in a session's tabpage. Otherwise show only
            // hostname's shortname.
            string shortname = custom_controls.SetSessionTabShortname(new_hostname);

            if (new_hostname == "" || new_username == "")
            {
                MessageBox.Show("Host & Username Must Be Defined!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // In case we are going to use private key to login - check if it exists first!
            // And also check if group or sub-group in this session is part of private keys setup.
            // If yes, then use passwordless login.
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                new_hostname_groups = IsPasswordLess.GetGroupsForPwdLessHostname(containers_list, new_hostname);

                // Check if group or su-group is part of private keys configuration setup
                if ((IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, new_hostname_groups[0])) ||
                    (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, new_hostname_groups[1])))
                {
                    Console.WriteLine("#### GROUP CHECK IN NEW CONNECT LOGIN: " + new_hostname_groups[0] + " and " + new_hostname_groups[1]);

                    // Fetch private key for group 
                    PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, new_hostname_groups[0]);

                    // If private key is still null, then sub-group is part of its setup - fetch it!
                    if (PrivateKey == null)
                    {
                        Console.WriteLine("## Sub-group is part of pwdess login!");
                        PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, new_hostname_groups[1]);
                    }

                    // If private key doesn't exists or is still null then something is wrong! Stop processing and return!
                    if (!File.Exists(PrivateKey))
                    {
                        if (PrivateKey == null || PrivateKey == "")
                        {
                            PrivateKey = "of type PPK or its group";
                        }

                        MessageBox.Show("You are going to use passwordless login, " + Environment.NewLine +
                            "however private key " + PrivateKey + " doesn't exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        return;
                    }

                    new_password = null;
                }
            }

            for (int i = 0; i < new_c_count; i++)
            {
                Process process = new Process();
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(Process_Exited);
                Panel panels = new Panel();
                TabPage tabpage = new TabPage(shortname);
                tabpage.Font = new Font("Calibri", 10);
                panels.AutoSize = true;
                panels.Dock = DockStyle.Fill;
                tabpage.Controls.Add(panels);
                my_ProcessInfo_List_TC_1.Add(now.start_putty(panels, i, process, new_hostname, new_username, new_password, mus.putty_path, SessionsSplitContainer, PrivateKey));
                tabcontrol1.TabPages.Add(tabpage);
            }

            foreach (Control control in new_connect_panel.Controls)
            {
                if (control.Name == "new_connect_host_textbox")
                {
                    control.Text = null;
                }
                if (control.Name == "new_connect_username_textbox")
                {
                    control.Text = null;
                }
                if (control.Name == "new_connect_password_textbox")
                {
                    // After connecting to pwdless session, searched via SearchTextbox_KeyDown, revert password textbox settings
                    // back to the original (rw) state
                    if (((TextBox)control).ReadOnly)
                    {
                        ((TextBox)control).UseSystemPasswordChar = true;
                        ((TextBox)control).ForeColor = Color.Black;
                        ((TextBox)control).ReadOnly = false;
                        ((TextBox)control).BackColor = Color.White;
                        ((TextBox)control).Text = null;
                    } else
                    {
                        control.Text = null;
                    }
                }
            }

            foreach (NumericUpDown ctlNumeric in new_connect_panel.Controls.OfType<NumericUpDown>())
            {
                ctlNumeric.Value = 0;
            }
        }

        /// <summary>
        /// Open new session by clicking on hostname contained within the TreeView (SimpleServerPane).
        /// It opens new putty.exe process docked within the TabControl's Tabpage.
        /// </summary>
        private void ServerPane_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string _hostname = null;
            string _username = null;
            string _password = null;
            string _group = null;
            string _sub_group = null;
            string PrivateKey = null;

            if (SimpleServerPane.SelectedNode != null)
            {
                foreach (GroupBox container in containers_list)
                {
                    Control[] hostname_textbox = container.Controls.Find("hostname_textbox", true);
                    Control[] username_texbox = container.Controls.Find("username_textbox", true);
                    Control[] password_texbox = container.Controls.Find("password_textbox", true);
                    Control[] group_combobox = container.Controls.Find("combobox", true);
                    Control[] sub_group_combobox = container.Controls.Find("sub_groups_combobox", true);

                    if (hostname_textbox[0].Text == SimpleServerPane.SelectedNode.Text)
                    {
                        _hostname = hostname_textbox[0].Text;
                        _username = username_texbox[0].Text;
                        _password = password_texbox[0].Text;
                        _group = group_combobox[0].Text;
                        _sub_group = sub_group_combobox[0].Text;
                    }
                }

                if (SimpleServerPane.SelectedNode.Text != _group && SimpleServerPane.SelectedNode.Text == _hostname)
                {
                    Console.WriteLine("## All fine we are processing hostname and not a group or sub-group");
                    // If the hostname string is a valid IP address then show
                    // the IP address in a session's tabpage. Otherwise show only
                    // hostname's shortname.
                    string shortname = custom_controls.SetSessionTabShortname(_hostname);

                    // In case we are going to use private key to login - check if it exists first!
                    // And also check if group or sub-group in this session is part of private keys setup.
                    // If yes, then use passwordless login.
                    if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "PuTTYStorm", "privatekeys.xml")))
                    {
                        // Check if group or su-group is part of private keys configuration setup
                        if ((IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, _group)) ||
                            (IsPasswordLess.IsGroupBetweenPrivateKeys(privatekeys, _sub_group)))
                        {
                            Console.WriteLine("#### GROUP CHECK IN Simple pane (TreeView) LOGIN: " + _group + " and " + _sub_group);

                            // Fetch private key for group
                            PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, _group);

                            // If private key is still null, then sub-group is part of its setup - fetch it!
                            if (PrivateKey == null)
                            {
                                Console.WriteLine("## Sub-group is part of pwdess login!");
                                PrivateKey = IsPasswordLess.GetPPKPrivateKeyForGroup(privatekeys, _sub_group);
                            }

                            // If private key doesn't exists or is still null then something is wrong! Stop processing and return!
                            if (!File.Exists(PrivateKey))
                            {
                                if (PrivateKey == null || PrivateKey == "")
                                {
                                    PrivateKey = "of type PPK or its group";
                                }

                                MessageBox.Show("You are going to use passwordless login, " + Environment.NewLine +
                                    "however private key " + PrivateKey + " doesn't exists!", "Error", MessageBoxButtons.OK, 
                                    MessageBoxIcon.Stop);

                                return;
                            }

                            _password = null;
                        }
                    }

                    Process process = new Process();
                    process.EnableRaisingEvents = true;
                    process.Exited += new EventHandler(Process_Exited);
                    Panel panels = new Panel();
                    TabPage tabpage = new TabPage(shortname);
                    tabpage.Font = new Font("Calibri", 10);
                    panels.AutoSize = true;
                    panels.Dock = DockStyle.Fill;
                    tabpage.Controls.Add(panels);
                    my_ProcessInfo_List_TC_1.Add(now.start_putty(panels, i, process, _hostname, _username, _password, 
                        mus.putty_path, SessionsSplitContainer, PrivateKey));
                    tabcontrol1.TabPages.Add(tabpage);
                }
                SimpleServerPane.SelectedNode = null;
            }
        }

        /// <summary>
        /// Show and hide Panel2 of SessionSplitContainerAndServerPanel (SplitControl) 
        /// which contains SimpleServerPane (TreeView).
        /// </summary>
        private void Show_server_pane_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (SessionSplitContainerAndServerPanel.Panel2Collapsed)
                {
                    button.Text = "HideServersPanel";
                    SessionSplitContainerAndServerPanel.Panel2Collapsed = false;
                }
                else 
                {
                    button.Text = "ShowServersPanel";
                    SessionSplitContainerAndServerPanel.Panel2Collapsed = true;
                }
            }
        }

        /// <summary>
        /// Start GlobalHotKeysManager Form
        /// </summary>
        private void Global_Hot_Keys_Click(object sender, EventArgs e)
        {
            GlobalHotKeysManager globalHotKeysManager = new GlobalHotKeysManager(TabPagesForwardGlobalHotKeyWorker, 
                TabPagesBackwardGlobalHotKeyWorker, SplitScreenGlobalHotKeyWorker, SFTPManagerGlobalHotKeyWorker,
                KotarakGlobalHotKeyWorker);

            globalHotKeysManager.Name = "GlobalHotKeysManager";
            globalHotKeysManager.Text = GlobalVar.VERSION + " - GlobalHotKeysManager";
            globalHotKeysManager.Show();
        }


        /// <summary>
        /// Handle the removal of Tabpage and putty.exe processes on process exit!
        /// Triggered with "crtl+d" or typing "exit" in putty.exe window.
        /// </summary>
        bool Process_Disposed = false;

        private void Process_Exited(object sender, System.EventArgs e)
        {
            _Process_Exited(my_ProcessInfo_List_TC_1, tabcontrol1);
            _Process_Exited(my_ProcessInfo_List_TC_2, tabcontrol2);
            Process_Disposed = false;           
        }

        private void Removetabpage(int i, TabControl tabcontrol)
        {
            if (tabcontrol.InvokeRequired)
            {               
                tabcontrol.Invoke(new Action<int, TabControl>(Removetabpage), i, tabcontrol);
                return;
            }

            var tabpage_current = tabcontrol.TabPages[i];
            tabcontrol.TabPages.Remove(tabpage_current);
        }

        private void Selecttab (int i, TabControl tabcontrol)
        {
            if (tabcontrol.InvokeRequired)
            {
                tabcontrol.Invoke(new Action<int, TabControl>(Selecttab), i, tabcontrol);
                return;
            }
            tabcontrol.SelectTab(i);
        }

        private void RemoveSplitScreen (TabControl tabcontrol)
        {
            if (SessionsSplitContainer.InvokeRequired)
            {
                SessionsSplitContainer.Invoke(new Action<TabControl> (RemoveSplitScreen), tabcontrol);
                return;
            }
            if (my_ProcessInfo_List_TC_1.Count == 1 && my_ProcessInfo_List_TC_2.Count == 0)
            {
                SessionsSplitContainer.Panel2Collapsed = true;
            }

            if (my_ProcessInfo_List_TC_2.Count == 1 && my_ProcessInfo_List_TC_1.Count == 0)
            {
                SessionsSplitContainer.Panel2Collapsed = true;
            }

            if (my_ProcessInfo_List_TC_2.Count == 1 && tabcontrol.Name == "TABCONTROL_2")
            {
                SessionsSplitContainer.Panel2Collapsed = true;
            }
        }

        private void _Process_Exited(List<ProcessInfo> my_ProcessInfo_List, TabControl tabcontrol)
        {
            //Console.WriteLine("TabCount: " + tabcontrol1.TabCount);
            //Console.WriteLine(Process_Disposed);
            if (!Process_Disposed && my_ProcessInfo_List != null)
            {
                //Console.WriteLine("Process count: " + my_ProcessInfo_List.Count);
                //Console.WriteLine("TabCount: " + tabcontrol1.TabCount);
                for (int i = 0; i < my_ProcessInfo_List.Count; i++)
                {
                    if (my_ProcessInfo_List[i].process != null && my_ProcessInfo_List[i].process.HasExited)
                    {
                        RemoveSplitScreen(tabcontrol);
                        Removetabpage(i, tabcontrol);
                        my_ProcessInfo_List.RemoveAt(i);

                        for (int j = 0; j < tabcontrol.TabCount; j++)
                        {
                            ProcessInfo temp_count = my_ProcessInfo_List[j];
                            temp_count.count = j;
                            my_ProcessInfo_List[j] = temp_count;
                        }

                        if (tabcontrol.TabCount != 0)
                        {
                            if (i != 0)
                            {
                                Selecttab(i - 1, tabcontrol);
                                NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(i - 1).mainhandle);
                            }
                            else
                            {
                                Selecttab(i, tabcontrol);
                                NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(i).mainhandle);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle Tabpage removal and process disposal when right click on Tabpage
        /// cotnained within tabcontrol1.
        /// </summary>
        private void Tabcontrol1_MouseClick(object sender, MouseEventArgs e)
        {
            Console.Write("Tabconbtrol1 mouse click\n");
            var tabControl = sender as TabControl;

            _TabControlFastHandling(tabControl, my_ProcessInfo_List_TC_1, e);
        }

        /// <summary>
        /// Handle Tabpage removal and process disposal when right click on Tabpage
        /// cotnained within tabcontrol2.
        /// </summary>
        private void Tabcontrol2_MouseClick(object sender, MouseEventArgs e)
        {
            Console.Write("Tabconbtrol2 mouse click\n");
            var tabControl = sender as TabControl;

            _TabControlFastHandling(tabControl, my_ProcessInfo_List_TC_2, e);
        }

        /// <summary>
        /// Helper method for Tabcontrol1_MouseClick & Tabcontrol2_MouseClick
        /// </summary>
        private void _TabControlFastHandling(TabControl tabControl, List<ProcessInfo> my_ProcessInfo_List, MouseEventArgs e)
        {
            TabPage tabPageCurrent = null;
            int my_tabindex = 0;
            Process_Disposed = false;

            for (int i = 0; i < tabControl.TabCount; i++)
            {
                if (!tabControl.GetTabRect(i).Contains(e.Location))
                {
                    continue;
                }
                my_tabindex = i;
                tabPageCurrent = tabControl.TabPages[i];
                break;
            }

            if (tabPageCurrent != null)
            {
                // On left click just handle the focus change back to putty.exe window.
                if (e.Button == MouseButtons.Left)
                {
                    NativeMethods.SetForegroundWindow(my_ProcessInfo_List.ElementAt(my_tabindex).mainhandle);
                }

                // On right click introduce fast closing feature. Remove TabPage and dispose the 
                // putty.exe process.
                if (e.Button == MouseButtons.Right)
                {
                    // Remove split screen if we are going to close the last tabpage!!
                    if (my_ProcessInfo_List_TC_1.Count == 1 && my_ProcessInfo_List_TC_2.Count == 0)
                    {
                        SessionsSplitContainer.Panel2Collapsed = true;
                    }

                    if (my_ProcessInfo_List_TC_2.Count == 1 && my_ProcessInfo_List_TC_1.Count == 0)
                    {
                        SessionsSplitContainer.Panel2Collapsed = true;
                    }

                    if (my_ProcessInfo_List_TC_2.Count == 1 && tabControl.Name == "TABCONTROL_2")
                    {
                        SessionsSplitContainer.Panel2Collapsed = true;
                    }

                    tabControl.TabPages.Remove(tabPageCurrent);
                    if (my_ProcessInfo_List.ElementAt(my_tabindex).panel != null)
                    {
                        Panel panel_to_dispose = my_ProcessInfo_List.ElementAt(my_tabindex).panel;
                        panel_to_dispose.Dispose();
                        Process_Disposed = true;
                    }
                    my_ProcessInfo_List.RemoveAt(my_tabindex);
                }

                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    ProcessInfo temp_count = my_ProcessInfo_List[i];
                    temp_count.count = i;
                    my_ProcessInfo_List[i] = temp_count;
                }

            }
        }

        /// <summary>
        /// When closing the PuTTY STORM sessions form (Form2), show the main configuration 
        /// form (Form1) again.
        /// </summary>
        void SessionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TabPagesForwardGlobalHotKeyWorker.UnregisterGlobalHotKey();
            TabPagesBackwardGlobalHotKeyWorker.UnregisterGlobalHotKey();
            SplitScreenGlobalHotKeyWorker.UnregisterGlobalHotKey();
            SFTPManagerGlobalHotKeyWorker.UnregisterGlobalHotKey();
            KotarakGlobalHotKeyWorker.UnregisterGlobalHotKey();

            SessionsForm = null;
            this.Show();
            this.Refresh();           
        }

        /// <summary>
        /// On TabControl1 resize event, always move the putty.exe window so it is where it should be.
        /// </summary>
        void Tabcontrol1_Resize(object sender, EventArgs e)
        {
            foreach (ProcessInfo my_ProcessInfo in my_ProcessInfo_List_TC_1)
            {
                if (my_ProcessInfo.mainhandle != IntPtr.Zero)
                {
                    NativeMethods.MoveWindow(my_ProcessInfo.mainhandle, DPIAwareScaling.MoveWindowX, DPIAwareScaling.MoveWindowY, 
                        SessionsSplitContainer.Panel1.Width + DPIAwareScaling.MoveWindowNWidth, SessionsSplitContainer.Panel1.Height + DPIAwareScaling.MoveWindowNHeight, true);
                }
                base.OnResize(e);
            }
        }

        /// <summary>
        /// On TabControl2 resize event, always move the putty.exe window so it is where it should be.
        /// </summary>
        void Tabcontrol2_Resize(object sender, EventArgs e)
        {
            foreach (ProcessInfo my_ProcessInfo in my_ProcessInfo_List_TC_2)
            {
                if (my_ProcessInfo.mainhandle != IntPtr.Zero)
                {
                    NativeMethods.MoveWindow(my_ProcessInfo.mainhandle, DPIAwareScaling.MoveWindowX, DPIAwareScaling.MoveWindowY, 
                        SessionsSplitContainer.Panel2.Width + DPIAwareScaling.MoveWindowNWidth, SessionsSplitContainer.Panel2.Height + DPIAwareScaling.MoveWindowNHeight, true);
                }
                base.OnResize(e);
            }
        }

        /// <summary>
        /// When closing PuTTY STORM sessions form, which contains TabControls - dispose
        /// all putty.exe processes within tabcontrol1.
        /// </summary>
        void Tabcontrol1_HandleDestroyed(object sender, EventArgs e)
        {
            foreach (ProcessInfo my_ProcessInfo in my_ProcessInfo_List_TC_1) {
                // Stop the application
                if (my_ProcessInfo.mainhandle != IntPtr.Zero)
                {
                    // Post a colse message
                    //PostMessage(processes[i].MainWindowHandle, (uint)WindowLongFlags2.WM_CLOSE, 0, 0);

                    // Delay for it to get the message
                    //System.Threading.Thread.Sleep(1000);

                    // Clear internal handle
                    IntPtr HandleDestroy = my_ProcessInfo.mainhandle;
                    HandleDestroy = IntPtr.Zero;
                }
                base.OnHandleDestroyed(e);
            }
            my_ProcessInfo_List_TC_1 = null;
        }

        /// <summary>
        /// When closing PuTTY STORM sessions form, which contains TabControls - dispose
        /// all putty.exe processes within tabcontrol2.
        /// </summary>
        void Tabcontrol2_HandleDestroyed(object sender, EventArgs e)
        {
            foreach (ProcessInfo my_ProcessInfo in my_ProcessInfo_List_TC_2)
            {
                // Stop the application
                if (my_ProcessInfo.mainhandle != IntPtr.Zero)
                {
                    // Post a colse message
                    //PostMessage(processes[i].MainWindowHandle, (uint)WindowLongFlags2.WM_CLOSE, 0, 0);

                    // Delay for it to get the message
                    //System.Threading.Thread.Sleep(1000);

                    // Clear internal handle
                    IntPtr HandleDestroy = my_ProcessInfo.mainhandle;
                    HandleDestroy = IntPtr.Zero;
                }
                base.OnHandleDestroyed(e);
            }
            my_ProcessInfo_List_TC_2 = null;
        }

        #region Flashing windows

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        //Stop flashing. The system restores the window to its original state. 
        public const UInt32 FLASHW_STOP = 0;
        //Flash the window caption. 
        public const UInt32 FLASHW_CAPTION = 1;
        //Flash the taskbar button. 
        public const UInt32 FLASHW_TRAY = 2;
        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;
        //Flash continuously, until the FLASHW_STOP flag is set. 
        public const UInt32 FLASHW_TIMER = 4;
        //Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;

        public static bool FlashWindow(IntPtr hWnd, uint mode)
        {
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = (UInt32)mode;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }
        #endregion


        /// <summary>
        /// Handle the removal of session GroupBoxes contained within the main PuTTY STORM 
        /// configuration form (MainForm).
        /// </summary>
        int TempScrollValue = 0;
        void ButtonClickOneEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
               int x = Convert.ToInt32(containers_list[(int)button.Tag].Text);
               if ((int)button.Tag + 1 == x)
                {
                    int bnt_tag = (int)button.Tag;
                    Controls.Remove(containers_list[(int)button.Tag]);
                    containers_list.RemoveAt((int)button.Tag);

                    Control[] focus_SettingsPanel = this.Controls.Find("SettingPanel", true);
                    Control[] focus_SearchSessionConfigTextBox = this.Controls.Find("SearchSessionConfigTextBox", true);

                    focus_SettingsPanel[0].Focus();
                    TempScrollValue = LastScrollValue - DPIAwareScaling._ScaleY(240);
                    LastScrollValue = TempScrollValue;

                    if (LastScrollValue >= 0)
                    {                       
                        this.VerticalScroll.Value = LastScrollValue;
                    }

                    focus_SettingsPanel[0].Location = DPIAwareScaling.ScalePoint(500, 255);
                    focus_SearchSessionConfigTextBox[0].Location = DPIAwareScaling.ScalePoint(507, 215);
                    this.Refresh();

                    int y = 0;
                    int z = 1;
                    foreach (GroupBox container in containers_list)
                    {
                        y++;
                        container.Text = y.ToString();

                        foreach (Control control in container.Controls)
                        {
                            if (control.Name == "Remove_button")
                            {
                                control.Tag = z;
                                z++;
                            }
                        }
                    }

                    for (int g = bnt_tag; g < containers_list.Count; g++)
                    {
                        Point locationOnForm = containers_list[g].FindForm().PointToClient(containers_list[g].
                            Parent.PointToScreen(containers_list[g].Location));
                        int new_groupbox_location = locationOnForm.Y - DPIAwareScaling._ScaleY(240);

                        custom_controls.set_groupbox_location(containers_list[g], new_groupbox_location);
                    }
                    i--;
                }
            }
        }

    }
}
