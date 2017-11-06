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
using System.Drawing;
using System.IO;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;

namespace PuTTY_Storm
{
    class SetControls
    {
        GetSavedSessions saved_groups = new GetSavedSessions();

        /// <summary>
        /// Initialize controls in Form1 (PuTTY STORM main configuration form).
        /// </summary>
        public void initialize_container (GroupBox container)
        {
            container.BackColor = Color.SlateGray;
            container.ForeColor = Color.White;
            container.Font = new Font("Calibri", 25);
            container.Text = "1";
            container.Location = DPIAwareScaling.ScalePoint(23, 91);
            container.Size = DPIAwareScaling.ScaleSize(380, 225);
        }

        public void initialize_putty_config_container(GroupBox container)
        {
            container.BackColor = Color.SlateGray;
            container.ForeColor = Color.White;
            container.Font = new Font("Calibri", 14, FontStyle.Bold);
            container.Text = "PuTTY.exe";
            container.Location = DPIAwareScaling.ScalePoint(430, 100);
            container.Size = DPIAwareScaling.ScaleSize(370, 80);
            container.AutoSize = true;
        }

        public void initialize_putty_path_label(Label label)
        {
            label.Font = new Font("Calibri", 15);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Location = DPIAwareScaling.ScalePoint(7, 30);
            label.Size = DPIAwareScaling.ScaleSize(278, 33);
            label.AutoSize = true;
            label.Name = "putty_path_label";
        }

        public void initialize_putty_config_button(Button button)
        {
            button.Font = new Font("Calibri", 10);
            button.ForeColor = Color.Black;
            button.Location = DPIAwareScaling.ScalePoint(7, 65);
            button.Size = DPIAwareScaling.ScaleSize(90, 28);
            button.Text = "Set path";
            button.FlatStyle = FlatStyle.System;
            button.UseVisualStyleBackColor = true;
        }

        public void initialize_hostname_label (Label label)
        {
            label.Font = new Font("Calibri", 20);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Hostname or IP Address";
            label.Location = DPIAwareScaling.ScalePoint(63, 24);
            label.Size = DPIAwareScaling.ScaleSize(278, 33);
        }

        public void initialize_hostname_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = DPIAwareScaling.ScalePoint(70, 60);
            textbox.Size = DPIAwareScaling.ScaleSize(260, 27);
            textbox.Name = "hostname_textbox";
        }

        public void initialize_username_label (Label label)
        {
            label.Font = new Font("Calibri", 18);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Username";
            label.Location = DPIAwareScaling.ScalePoint(9, 95);
            label.Size = DPIAwareScaling.ScaleSize(113, 29);
        }

        public void initialize_username_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = DPIAwareScaling.ScalePoint(124, 98);
            textbox.Size = DPIAwareScaling.ScaleSize(140, 27);
            textbox.Name = "username_textbox";
        }

        public void initialize_password_label (Label label)
        {
            label.Font = new Font("Calibri", 18);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Password";
            label.Location = DPIAwareScaling.ScalePoint(11, 134);
            label.Size = DPIAwareScaling.ScaleSize(105, 29);
        }

        public void initialize_password_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = DPIAwareScaling.ScalePoint(124, 137);
            textbox.Size = DPIAwareScaling.ScaleSize(140, 27);
            textbox.Name = "password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_numbericupdown (NumericUpDown numericupdown, EventHandler numericupdown_ValueChanged)
        {
            numericupdown.Font = new Font("Calibri", 15);
            numericupdown.Location = DPIAwareScaling.ScalePoint(284, 118);
            numericupdown.Size = DPIAwareScaling.ScaleSize(43, 32);
            numericupdown.Value = 0;
            numericupdown.Name = "numericupdown";
            numericupdown.ValueChanged += new EventHandler(numericupdown_ValueChanged);
        }

        public void initialize_combobox (ComboBox combobox, EventHandler combobox_SelectedIndexChanged, 
            EventHandler combobox_PKGroupChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }

                combobox.DropDownWidth = DropDownWidth(combobox);
            }
            combobox.Font = new Font("Calibri", 10);
            combobox.Location = DPIAwareScaling.ScalePoint(15, 182);
            combobox.Size = DPIAwareScaling.ScaleSize(120, 32);
            combobox.Name = "combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            combobox.SelectedIndexChanged += new EventHandler(combobox_PKGroupChanged);

        }

        public void initialize_sub_groups_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged,
            EventHandler sub_groups_combobox_PKGroupChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }

                combobox.DropDownWidth = DropDownWidth(combobox);
            }
            combobox.Font = new Font("Calibri", 10);
            combobox.Location = DPIAwareScaling.ScalePoint(147, 182);
            combobox.Size = DPIAwareScaling.ScaleSize(120, 32);
            combobox.Name = "sub_groups_combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            combobox.SelectedIndexChanged += new EventHandler(sub_groups_combobox_PKGroupChanged);
        }

        public void initialize_removal_button(Button button, EventHandler ButtonClickOneEvent)
        {
            button.Name = "Remove_button";
            button.Font = new Font("Calibri", 10);
            button.Location = DPIAwareScaling.ScalePoint(290, 180);
            button.ForeColor = Color.Black;
            button.Size = DPIAwareScaling.ScaleSize(80, 28);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.FlatStyle = FlatStyle.System;
            button.Click += new EventHandler(ButtonClickOneEvent);
        }

        /// <summary>
        /// Initialize controls in NEW_CONNECT PANEL. This is the panel contained on the top
        /// of PuTTY STORM sessions form (Form2).
        /// </summary>
        public void initialize_new_connect_host_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "Host";
            label.Location = DPIAwareScaling.ScalePoint(0, 3);
            label.Size = DPIAwareScaling.ScaleSize(40, 33);
        }

        public void initialize_new_connect_host_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", DPIAwareScaling.NewConnectHostTextboxFont);
            textbox.Location = DPIAwareScaling.ScalePoint(43, 2);
            textbox.Size = DPIAwareScaling.ScaleSize(200, 27);
            textbox.Name = "new_connect_host_textbox";
        }

        public void initialize_new_connect_username_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "User";
            label.Location = DPIAwareScaling.ScalePoint(260, 3);
            label.Size = DPIAwareScaling.ScaleSize(40, 33);
        }

        public void initialize_new_connect_username_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", DPIAwareScaling.NewConnectUsernameTextboxFont);
            textbox.Location = DPIAwareScaling.ScalePoint(300, 2);
            textbox.Size = DPIAwareScaling.ScaleSize(60, 27);
            textbox.Name = "new_connect_username_textbox";
        }

        public void initialize_new_connect_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "Password";
            label.Location = DPIAwareScaling.ScalePoint(380, 3);
            label.Size = DPIAwareScaling.ScaleSize(75, 33);
        }

        public void initialize_new_connect_password_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", DPIAwareScaling.NewConnectPasswordTextboxFont);
            textbox.Location = DPIAwareScaling.ScalePoint(455, 2);
            textbox.Size = DPIAwareScaling.ScaleSize(60, 27);
            textbox.Name = "new_connect_password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_new_connect_numbericupdown(NumericUpDown numericupdown)
        {
            numericupdown.Font = new Font("Calibri", DPIAwareScaling.NewConnectNumericUpDownFont);
            numericupdown.Location = DPIAwareScaling.ScalePoint(550, 2);
            numericupdown.Size = DPIAwareScaling.ScaleSize(43, 30);
            numericupdown.Value = 0;
            numericupdown.Name = "new_connect_numericupdown";
        }

        public void initialize_new_connect_vertical_divide_label(Label label)
        {
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "";
            label.BorderStyle = BorderStyle.Fixed3D;
            label.Location = DPIAwareScaling.ScalePoint(730, 2);
            label.Size = DPIAwareScaling.ScaleSize(2, 25);
        }

        public void initialize_new_connect_search_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "Search";
            label.Location = DPIAwareScaling.ScalePoint(740, 3);
            label.Size = DPIAwareScaling.ScaleSize(60, 33);
        }

        public void initialize_new_connect_search_textbox(TextBox textbox, KeyEventHandler SearchTextbox_KeyDown)
        {
            textbox.Font = new Font("Calibri", DPIAwareScaling.NewConnectSearchTextboxFont);
            textbox.Location = DPIAwareScaling.ScalePoint(800, 2);
            textbox.Size = DPIAwareScaling.ScaleSize(100, 27);
            textbox.Name = "new_connect_search_textbox";
            textbox.AcceptsReturn = true;
            textbox.KeyDown += new KeyEventHandler(SearchTextbox_KeyDown);
        }

        public void initialize_new_connect_vertical_divide_label_1(Label label)
        {
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "";
            label.BorderStyle = BorderStyle.Fixed3D;
            label.Location = DPIAwareScaling.ScalePoint(920, 2);
            label.Size = DPIAwareScaling.ScaleSize(2, 25);
        }

        /// <summary>
        /// Initialize controls in ADVANCED OPTIONS PANEL1 (GROUPS). This is the left Panel of 
        /// splitcontainer1 contained within the Form3.
        /// </summary>
        public void initialize_advanced_add_group_label_header(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Groups";
            label.Location = DPIAwareScaling.ScalePoint(3, 3);
            label.Size = DPIAwareScaling.ScaleSize(90, 33);
        }

        public void initialize_advanced_add_group_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Add Group:";
            label.Location = DPIAwareScaling.ScalePoint(3, 46);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_advanced_add_group_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = DPIAwareScaling.ScalePoint(100, 45);
            textbox.Size = DPIAwareScaling.ScaleSize(100, 27);
            textbox.Name = "advanced_add_group_textbox";
        }

        public void initialize_advanced_remove_group_button(Button button, EventHandler ButtonClickGroupEvent)
        {
            button.Name = "Remove_Group_button";
            button.Font = new Font("Calibri", 10);
            button.Location = DPIAwareScaling.ScalePoint(250, 20);
            button.ForeColor = Color.Black;
            button.Size = DPIAwareScaling.ScaleSize(65, 25);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.FlatStyle = FlatStyle.System;
            button.Click += new EventHandler(ButtonClickGroupEvent);
        }

        public void initialize_advanced_group_divider(Label label)
        {
            label.BackColor = Color.White;
            label.ForeColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = DPIAwareScaling.ScalePoint(6, 85);
            label.Text = "";
            label.Size = DPIAwareScaling.ScaleSize(350, 4);
        }

        /// <summary>
        /// Initialize controls in ADVANCED OPTIONS PANEL2 (Passwords). This is the right panel of
        /// splitcontainer1 contained within the Form3. These are controls related to password change per 
        /// group.
        /// </summary>
        public void initialize_advanced_passwords_label_header(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Passwords";
            label.Location = DPIAwareScaling.ScalePoint(3, 3);
            label.Size = DPIAwareScaling.ScaleSize(100, 33);
        }

        public void initialize_advanced_new_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "New password:";
            label.Location = DPIAwareScaling.ScalePoint(3, 46);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_advanced_new_password_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = DPIAwareScaling.ScalePoint(130, 45);
            textbox.Size = DPIAwareScaling.ScaleSize(130, 27);
            textbox.Name = "advanced_new_password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_advanced_for_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "For Group:";
            label.Location = DPIAwareScaling.ScalePoint(3, 86);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_passwords_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }

                combobox.DropDownWidth = DropDownWidth(combobox);
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = DPIAwareScaling.ScalePoint(130, 86);
            combobox.Size = DPIAwareScaling.ScaleSize(130, 32);
            combobox.Name = "new_password_combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);          
        }

        /// <summary>
        /// Initialize controls in ADVANCED OPTIONS PANEL2 (Passwords). This is the right panel of
        /// splitcontainer1 contained within the Form3. These are controls related to Login password
        /// secret change.
        /// </summary>
        public void initialize_panel2_advanced_login_secret_divider(Label label)
        {
            label.BackColor = Color.White;
            label.ForeColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = DPIAwareScaling.ScalePoint(5, 250);
            label.Text = "";
            label.Size = DPIAwareScaling.ScaleSize(400, 4);
        }

        public void initialize_panel2_advanced_login_main_label(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Change Login Password Secret";
            label.Location = DPIAwareScaling.ScalePoint(3, 270);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_old_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Old password";
            label.Location = DPIAwareScaling.ScalePoint(3, 320);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_new_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "New password";
            label.Location = DPIAwareScaling.ScalePoint(3, 350);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_confirm_new_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Confirm new password";
            label.Location = DPIAwareScaling.ScalePoint(3, 380);
            label.Size = DPIAwareScaling.ScaleSize(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_old_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = DPIAwareScaling.ScalePoint(200, 320);
            textbox.Size = DPIAwareScaling.ScaleSize(150, 25);
            textbox.Name = "panel2_advanced_login_old_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_panel2_advanced_login_new_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = DPIAwareScaling.ScalePoint(200, 350);
            textbox.Size = DPIAwareScaling.ScaleSize(150, 25);
            textbox.Name = "panel2_advanced_login_new_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_panel2_advanced_login_confirm_new_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = DPIAwareScaling.ScalePoint(200, 380);
            textbox.Size = DPIAwareScaling.ScaleSize(150, 25);
            textbox.Name = "panel2_advanced_login_confirm_new_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Initialize controls in ADVANCED OPTIONS PANEL1 (Private Keys). This is the left Panel of 
        /// splitcontainer1 contained within the Form3 - Controls for Private keys settings.
        /// </summary>
        public void initialize_advanced_private_keys_label_header(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Private Keys";
            label.Location = DPIAwareScaling.ScalePoint(3, 260);
            label.Size = DPIAwareScaling.ScaleSize(130, 30);
        }

        public void initialize_advanced_private_keys_textbox_filedialog(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10, FontStyle.Bold);
            //textbox.BackColor = Color.SlateGray;
            textbox.ForeColor = Color.White;
            textbox.Text = "";
            textbox.Name = "private_keys_textbox_filedialog";
            textbox.Location = DPIAwareScaling.ScalePoint(80, 300);
            textbox.Size = DPIAwareScaling.ScaleSize(270, 20);
            textbox.ReadOnly = true;
        }

        public void initialize_advanced_private_keys_label_keytype(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Type";
            label.Location = DPIAwareScaling.ScalePoint(5, 340);
            label.Size = DPIAwareScaling.ScaleSize(50, 30);
        }

        public void initialize_advanced_private_keys_keytype_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            combobox.Items.Add("PPK");
            combobox.Items.Add("OpenSSH");
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = DPIAwareScaling.ScalePoint(65, 340);
            combobox.Size = DPIAwareScaling.ScaleSize(80, 32);
            combobox.Name = "private_keys_keytype_combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
        }

        public void initialize_advanced_private_keys_label_group(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Group";
            label.Location = DPIAwareScaling.ScalePoint(160, 340);
            label.Size = DPIAwareScaling.ScaleSize(60, 30);
        }

        public void initialize_advanced_private_keys_group_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }

                combobox.DropDownWidth = DropDownWidth(combobox);
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = DPIAwareScaling.ScalePoint(230, 340);
            combobox.Size = DPIAwareScaling.ScaleSize(120, 32);
            combobox.Name = "private_keys_group_combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
        }

        public void initialize_advanced_private_keys_divider(Label label)
        {
            label.BackColor = Color.White;
            label.ForeColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = DPIAwareScaling.ScalePoint(6, 420);
            label.Text = "";
            label.Size = DPIAwareScaling.ScaleSize(350, 4);
        }

        public void initialize_advanced_remove_pk_button(Button button, EventHandler ButtonClickPKEvent)
        {
            button.Name = "Remove_PK_button";
            button.Font = new Font("Calibri", 10);
            button.Location = DPIAwareScaling.ScalePoint(270, 30);
            button.ForeColor = Color.Black;
            button.Size = DPIAwareScaling.ScaleSize(65, 25);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.FlatStyle = FlatStyle.System;
            button.Click += new EventHandler(ButtonClickPKEvent);
        }

        public void initialize_advanced_private_keys_passphrase_label(Label label)
        {
            label.Font = new Font("Calibri", 10, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "PWD";
            label.Location = DPIAwareScaling.ScalePoint(6, 385);
            label.Size = DPIAwareScaling.ScaleSize(40, 30);
        }

        public void initialize_advanced_private_keys_passphrase_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            //textbox.ForeColor = Color.White;
            textbox.Name = "private_keys_passphrase_textbox";
            textbox.Location = DPIAwareScaling.ScalePoint(50, 380);
            textbox.Size = DPIAwareScaling.ScaleSize(150, 20);
            textbox.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Initialize controls in Kotarak Plugin
        /// </summary>
        public void initialize_kotarak_group_subgroup_combobox(ComboBox combobox)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }

                combobox.DropDownWidth = DropDownWidth(combobox);
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Custom methods for handling controls.
        /// </summary>
        public void set_groupbox_location (GroupBox groupbox, int height)
        {
            groupbox.Location = new Point(DPIAwareScaling._ScaleX(23), height);
        }

        public void set_group_box_text (GroupBox groupbox, string text)
        {
            groupbox.Text = text;
        }

        public void set_button_location (Button button, int height)
        {
            button.Location = DPIAwareScaling.ScalePoint(292, height);
        }

        /// <summary>
        /// Refresh all sessions dropdown menus when adding new groups
        /// </summary>
        public void set_combobox_groups (List<GroupBox> containers_list, SavedGroupInfo groups)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                foreach (GroupBox groupbox in containers_list)
                {
                    foreach (ComboBox combobox in groupbox.Controls.OfType<ComboBox>())
                    {                      
                        UpdateComboBox(combobox, groups);
                        combobox.DropDownWidth = DropDownWidth(combobox);                        
                    }
                }
            }
        }

        /// <summary>
        /// Refresh dropdown menus in advanced form Panel2 when adding new groups
        /// </summary>
        public void set_passwords_combobox_groups(SplitContainer splitcontainer, SavedGroupInfo groups)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                foreach (ComboBox combobox in splitcontainer.Panel2.Controls.OfType<ComboBox>())
                {                  
                    UpdateComboBox(combobox, groups);
                    combobox.DropDownWidth = DropDownWidth(combobox);                   
                }
            }
        }

        /// <summary>
        /// Refresh dropdown menus in advanced Panel1 - Private Keys section, when adding new groups
        /// </summary>
        public void set_pk_combobox_groups(SplitContainer splitcontainer, SavedGroupInfo groups)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                foreach (ComboBox combobox in splitcontainer.Panel1.Controls.OfType<ComboBox>())
                {
                    if (combobox.Name == "private_keys_group_combobox")
                    {                       
                        UpdateComboBox(combobox, groups);
                        combobox.DropDownWidth = DropDownWidth(combobox);                        
                    }
                }
            }
        }

        private void UpdateComboBox(ComboBox combobox, SavedGroupInfo groups)
        {
            List<string> ItemsToDelete = new List<string>();
            ItemsToDelete.AddRange(combobox.Items.Cast<String>().ToList());

            if (!combobox.Items.Contains(""))
            {
                combobox.Items.Add("");
            }

            foreach (string name in groups.names)
            {
                if (combobox.Items.Contains(name))
                {
                    ItemsToDelete.Remove(name);
                    continue;
                }
                else
                {
                    combobox.Items.Add(name);
                }
            }

            foreach (string name in ItemsToDelete)
            {
                if (name == "")
                    continue;

                combobox.Items.Remove(name);
            }
        }

        /// <summary>
        /// Load TreeView Panel with grouped servers placed on the right side of Sessions Form.
        /// </summary>
        public void LoadTreeViewPane(TreeView ServerPane, List<GroupBox> containers_list, 
            TreeNodeMouseClickEventHandler ServerPane_NodeMouseDoubleClick)
        {
            try
            {
                // Image list for ServerPane TreeView
                ImageList ServerPaneImageList = new ImageList();
                ServerPaneImageList.ImageSize = DPIAwareScaling.ScaleSize(16, 16);
                ServerPaneImageList.Images.Add(PuTTY_Storm.Properties.Resources.VSO_Folder_hoverblue_32x);
                ServerPaneImageList.Images.Add(PuTTY_Storm.Properties.Resources.ComputerSystem_32x);
                ServerPane.ImageList = ServerPaneImageList;

                // Disable redrawing of ServerPane to prevent flickering while changes are made.
                ServerPane.BeginUpdate();

                foreach (GroupBox container in containers_list)
                {                    
                    Control[] group = container.Controls.Find("combobox", true);
                    Control[] sub_group = container.Controls.Find("sub_groups_combobox", true);
                    Control[] hostname_textbox = container.Controls.Find("hostname_textbox", true);

                    Console.WriteLine(group[0].Text);
                    Console.WriteLine(hostname_textbox[0].Text);

                    if ((!(group[0].Text == null || group[0].Text == "")))
                    {
                        // If group doesn't exists - add it
                        if (!ServerPane.Nodes.ContainsKey(group[0].Text))
                        { 
                             ServerPane.Nodes.Add(new TreeNode(group[0].Text) { Name = group[0].Text,
                                 ImageIndex = 0, SelectedImageIndex = 0 });
                             Console.WriteLine("Group " + group[0].Text + " doesnt exists adding");
                            
                        }

                        // If sub-group doesn't exists - add it
                        if (!ServerPane.Nodes[group[0].Text].Nodes.ContainsKey(sub_group[0].Text))
                        {
                            if (!(sub_group[0].Text == null || sub_group[0].Text == ""))
                            {
                                ServerPane.Nodes[group[0].Text].Nodes.Add(new TreeNode(sub_group[0].Text) { Name = sub_group[0].Text,
                                    ImageIndex = 0, SelectedImageIndex = 0 });
                                Console.WriteLine("##### SUBGROUP " + sub_group[0].Text + " doesnt exists adding");
                            }
                        }

                        // If server has a group only defined - add it under that group
                        if ((!(group[0].Text == null || group[0].Text == "") && (sub_group[0].Text == null || sub_group[0].Text == "")))
                        {
                            // If server doesn't exists in current group first check if it's part of another group.
                            // If that's the case remove it from that group! Server can be part only of one group!
                            // And then add it to the current group.
                            if (!ServerPane.Nodes[group[0].Text].Nodes.ContainsKey(hostname_textbox[0].Text))
                            {
                                TreeNode[] node = ServerPane.Nodes.Find(hostname_textbox[0].Text, true);
                                if (node.Length > 0)
                                {
                                    ServerPane.Nodes.Remove(node[0]);
                                }
                                ServerPane.Nodes[group[0].Text].Nodes.Add(new TreeNode(hostname_textbox[0].Text)
                                    { Name = hostname_textbox[0].Text, ImageIndex = 1, SelectedImageIndex = 1 });
                            }
                        }

                        // If server has group and also sub-group defined - add it under that group -> sub-group
                        if ((!(group[0].Text == null || group[0].Text == "") && !(sub_group[0].Text == null || sub_group[0].Text == "")))
                        {
                            // If server doesn't exists in current sub-group first check if it's part of another sub-group.
                            // If that's the case remove it from that sub-group! Server can be part only of one sub-group!
                            // And then add it to the current sub-group.
                            if (!ServerPane.Nodes[group[0].Text].Nodes[sub_group[0].Text].Nodes.ContainsKey(hostname_textbox[0].Text))
                            {
                                TreeNode[] node = ServerPane.Nodes.Find(hostname_textbox[0].Text, true);
                                if (node.Length > 0)
                                {
                                    ServerPane.Nodes.Remove(node[0]);
                                }
                                ServerPane.Nodes[group[0].Text].Nodes[sub_group[0].Text].Nodes.Add(new TreeNode(hostname_textbox[0].Text)
                                    { Name = hostname_textbox[0].Text, ImageIndex = 1, SelectedImageIndex = 1 });
                            }
                        }
                    }                  

                }

                // Remove empty group and sub-group nodes
                SavedGroupInfo groups = null;

                if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "PuTTYStorm", "groups.xml")))
                {
                    groups = saved_groups.get_Groups();
                }
                // Check both layers in case we are removing group which have also empty sub-group
                // inside. Can be done also using recursion.              
                for (int i = 0; i < 2; i++)
                {
                    if (groups != null)
                        RemoveEmptryTreeViewGroups(ServerPane, groups, containers_list);
                }

                // Sort Nodes in TreeView
                ServerPane.TreeViewNodeSorter = new NodeSorter();
                // Enable redrawing of ServerPane.
                ServerPane.EndUpdate();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Remove empty group and sub-group nodes
        /// </summary>
        private void RemoveEmptryTreeViewGroups (TreeView ServerPane, SavedGroupInfo groups, List<GroupBox> containers_list)
        {
            foreach (string name in groups.names)
            {
                TreeNode[] remove_empty_group_nodes = ServerPane.Nodes.Find(name, true);

                if (remove_empty_group_nodes.Length > 0)
                {
                    foreach (TreeNode remove_group in remove_empty_group_nodes)
                    {
                        // If group or sub-group doesn't contain any other nodes remove it
                        if (remove_group.Nodes.Count == 0)
                        {
                            ServerPane.Nodes.Remove(remove_group);
                        }
                        else
                        {
                            foreach (TreeNode node in remove_group.Nodes)
                            {
                                // If group or sub-group is not empty then:
                                // 1) Skip sub-group if we are checking a group.
                                //    - sub-group will be removed in the next iteration
                                if (node != null && FindNodeGroupBetweenGroups(groups, node.Text))
                                {
                                    continue;
                                }

                                // 2) If the node is hostname and it is not between sessions anymore,
                                //    remove it.
                                if (node != null && !FindHostnameInContainers(containers_list, node.Text))
                                {
                                    ServerPane.Nodes.Remove(node);
                                }
                            }

                            // Check again if now group or sub-group is empty.
                            // If yes remove it, if not go to the next group or sub-group or
                            // to the next iteration.
                            if (remove_group.Nodes.Count == 0)
                            {
                                ServerPane.Nodes.Remove(remove_group);
                            }
                        }                        
                    }
                }
            }
        }

        public class NodeSorter : IComparer
        {
            // Compare the length of the strings, or the strings
            // themselves, if they are the same length.
            public int Compare(object x, object y)
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                // Compare the length of the strings, returning the difference.
                if (tx.Text.Length != ty.Text.Length)
                    return tx.Text.Length - ty.Text.Length;

                // If they are the same length, call Compare.
                return string.Compare(tx.Text, ty.Text);
            }
        }

        public int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            if (myCombo.Items.Count > 0)
            {
                foreach (var obj in myCombo.Items)
                {
                    temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                    if (temp > maxWidth)
                    {
                        maxWidth = temp;
                    }
                }
            }
            return maxWidth;
        }

        private bool FindHostnameInContainers (List<GroupBox> containers_list, string hostname)
        {
            bool find = false;

            foreach (GroupBox container in containers_list)
            {
                Control[] hostname_textbox = container.Controls.Find("hostname_textbox", true);
                if (hostname_textbox[0].Text == hostname)
                {
                    find = true;
                }
            }

            return find;
        }

        private bool FindNodeGroupBetweenGroups (SavedGroupInfo groups, string node_group)
        {
            bool find = false;

            foreach (string group in groups.names)
            {
                if (group == node_group)
                {
                    find = true;
                }
            }

            return find;
        }

        /// <summary>
        /// If the hostname string is a valid IP address then show
        /// the IP address in a session's tabpage. Otherwise show only
        /// hostname's shortname.
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns>valid shortname</returns>
        public string SetSessionTabShortname (string hostname)
        {
            string shortname;
            IPAddress ipAddress = null;

            if (IPAddress.TryParse(hostname, out ipAddress))
            {
                shortname = hostname;
            }
            else
            {
                Regex pattern = new Regex(@"^.*?(?=\.)");
                Match match = pattern.Match(hostname);
                shortname = match.Groups[0].Value;
            }

            return shortname;
        }


    }
}
