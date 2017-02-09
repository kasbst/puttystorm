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

namespace PuTTY_Storm
{
    class SetControls
    {
        /// <summary>
        /// Initialize controls in Form1 (PuTTY STORM main configuration form).
        /// </summary>
        public void initialize_container (GroupBox container)
        {
            container.BackColor = Color.SlateGray;
            container.ForeColor = Color.White;
            container.Font = new Font("Calibri", 25);
            container.Text = "1";
            container.Location = new Point(23, 91);
            container.Size = new Size(380, 225);
        }

        public void initialize_putty_config_container(GroupBox container)
        {
            container.BackColor = Color.SlateGray;
            container.ForeColor = Color.White;
            container.Font = new Font("Calibri", 14, FontStyle.Bold);
            container.Text = "PuTTY.exe";
            container.Location = new Point(430, 100);
            container.Size = new Size(370, 80);
            container.AutoSize = true;
        }

        public void initialize_putty_path_label(Label label)
        {
            label.Font = new Font("Calibri", 15);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Location = new Point(7, 30);
            label.Size = new Size(278, 33);
            label.AutoSize = true;
            label.Name = "putty_path_label";
        }

        public void initialize_putty_config_button(Button button)
        {
            button.Font = new Font("Calibri", 10);
            button.ForeColor = Color.Black;
            button.Location = new Point(7, 65);
            button.Size = new Size(90, 28);
            button.Text = "Set path";
            button.UseVisualStyleBackColor = true;
        }

        public void initialize_hostname_label (Label label)
        {
            label.Font = new Font("Calibri", 20);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Hostname or IP Address";
            label.Location = new Point(63, 24);
            label.Size = new Size(278, 33);
        }

        public void initialize_hostname_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = new Point(70, 60);
            textbox.Size = new Size(260, 27);
            textbox.Name = "hostname_textbox";
        }

        public void initialize_username_label (Label label)
        {
            label.Font = new Font("Calibri", 18);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Username";
            label.Location = new Point(9, 95);
            label.Size = new Size(113, 29);
        }

        public void initialize_username_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = new Point(124, 98);
            textbox.Size = new Size(140, 27);
            textbox.Name = "username_textbox";
        }

        public void initialize_password_label (Label label)
        {
            label.Font = new Font("Calibri", 18);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Password";
            label.Location = new Point(11, 134);
            label.Size = new Size(105, 29);
        }

        public void initialize_password_textbox (TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 12);
            textbox.Location = new Point(124, 137);
            textbox.Size = new Size(140, 27);
            textbox.Name = "password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_numbericupdown (NumericUpDown numericupdown, EventHandler numericupdown_ValueChanged)
        {
            numericupdown.Font = new Font("Calibri", 15);
            numericupdown.Location = new Point(284, 118);
            numericupdown.Size = new Size(43, 32);
            numericupdown.Value = 0;
            numericupdown.Name = "numericupdown";
            numericupdown.ValueChanged += new EventHandler(numericupdown_ValueChanged);
        }

        public void initialize_combobox (ComboBox combobox, EventHandler combobox_SelectedIndexChanged, EventHandler combobox_PKGroupChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = new Point(18, 180);
            combobox.Size = new Size(90, 32);
            combobox.Name = "combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
            combobox.SelectedIndexChanged += new EventHandler(combobox_PKGroupChanged);

        }

        public void initialize_removal_button(Button button, EventHandler ButtonClickOneEvent)
        {
            button.Name = "Remove_button";
            button.Font = new Font("Calibri", 10);
            button.Location = new Point(270, 180);
            button.ForeColor = Color.Black;
            button.Size = new Size(80, 28);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
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
            label.Location = new Point(0, 3);
            label.Size = new Size(40, 33);
        }

        public void initialize_new_connect_host_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(43, 2);
            textbox.Size = new Size(200, 27);
            textbox.Name = "new_connect_host_textbox";
        }

        public void initialize_new_connect_username_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "User";
            label.Location = new Point(260, 3);
            label.Size = new Size(40, 33);
        }

        public void initialize_new_connect_username_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(300, 2);
            textbox.Size = new Size(60, 27);
            textbox.Name = "new_connect_username_textbox";
        }

        public void initialize_new_connect_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "Password";
            label.Location = new Point(380, 3);
            label.Size = new Size(75, 33);
        }

        public void initialize_new_connect_password_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(455, 2);
            textbox.Size = new Size(60, 27);
            textbox.Name = "new_connect_password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_new_connect_numbericupdown(NumericUpDown numericupdown)
        {
            numericupdown.Font = new Font("Calibri", 10);
            numericupdown.Location = new Point(550, 2);
            numericupdown.Size = new Size(43, 30);
            numericupdown.Value = 0;
            numericupdown.Name = "new_connect_numericupdown";
        }

        public void initialize_new_connect_vertical_divide_label(Label label)
        {
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "";
            label.BorderStyle = BorderStyle.Fixed3D;
            label.Location = new Point(730, 2);
            label.Size = new Size(2, 25);
        }

        public void initialize_new_connect_search_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = SystemColors.Control;
            label.ForeColor = Color.Black;
            label.Text = "Search";
            label.Location = new Point(740, 3);
            label.Size = new Size(60, 33);
        }

        public void initialize_new_connect_search_textbox(TextBox textbox, KeyEventHandler SearchTextbox_KeyDown)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(800, 2);
            textbox.Size = new Size(100, 27);
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
            label.Location = new Point(920, 2);
            label.Size = new Size(2, 25);
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
            label.Location = new Point(3, 3);
            label.Size = new Size(90, 33);
        }

        public void initialize_advanced_add_group_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Add Group:";
            label.Location = new Point(3, 46);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_advanced_add_group_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(100, 45);
            textbox.Size = new Size(100, 27);
            textbox.Name = "advanced_add_group_textbox";
        }

        public void initialize_advanced_remove_group_button(Button button, EventHandler ButtonClickGroupEvent)
        {
            button.Name = "Remove_Group_button";
            button.Font = new Font("Calibri", 10);
            button.Location = new Point(120, 20);
            button.ForeColor = Color.Black;
            button.Size = new Size(65, 25);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.Click += new EventHandler(ButtonClickGroupEvent);
        }

        public void initialize_advanced_group_divider(Label label)
        {
            label.BackColor = Color.White;
            label.ForeColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(6, 85);
            label.Text = "";
            label.Size = new Size(350, 4);
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
            label.Location = new Point(3, 3);
            label.Size = new Size(100, 33);
        }

        public void initialize_advanced_new_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "New password:";
            label.Location = new Point(3, 46);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_advanced_new_password_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(130, 45);
            textbox.Size = new Size(130, 27);
            textbox.Name = "advanced_new_password_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_advanced_for_password_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "For Group:";
            label.Location = new Point(3, 86);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_passwords_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = new Point(130, 86);
            combobox.Size = new Size(90, 32);
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
            label.Location = new Point(5, 250);
            label.Text = "";
            label.Size = new Size(400, 4);
        }

        public void initialize_panel2_advanced_login_main_label(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Change Login Password Secret";
            label.Location = new Point(3, 270);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_old_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Old password";
            label.Location = new Point(3, 320);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_new_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "New password";
            label.Location = new Point(3, 350);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_confirm_new_passwd_label(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Confirm new password";
            label.Location = new Point(3, 380);
            label.Size = new Size(90, 35);
            label.AutoSize = true;
        }

        public void initialize_panel2_advanced_login_old_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(200, 320);
            textbox.Size = new Size(150, 25);
            textbox.Name = "panel2_advanced_login_old_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_panel2_advanced_login_new_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(200, 350);
            textbox.Size = new Size(150, 25);
            textbox.Name = "panel2_advanced_login_new_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        public void initialize_panel2_advanced_login_confirm_new_passwd_textbox(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10);
            textbox.Location = new Point(200, 380);
            textbox.Size = new Size(150, 25);
            textbox.Name = "panel2_advanced_login_confirm_new_passwd_textbox";
            textbox.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Initialize controls in ADVANCED OPTIONS PANEL1 (GROUPS). This is the left Panel of 
        /// splitcontainer1 contained within the Form3 - Controls for Private keys settings.
        /// </summary>
        public void initialize_advanced_private_keys_label_header(Label label)
        {
            label.Font = new Font("Calibri", 15, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Private Keys";
            label.Location = new Point(3, 260);
            label.Size = new Size(130, 30);
        }

        public void initialize_advanced_private_keys_textbox_filedialog(TextBox textbox)
        {
            textbox.Font = new Font("Calibri", 10, FontStyle.Bold);
            //textbox.BackColor = Color.SlateGray;
            textbox.ForeColor = Color.White;
            textbox.Text = "";
            textbox.Name = "private_keys_textbox_filedialog";
            textbox.Location = new Point(80, 300);
            textbox.Size = new Size(240, 20);
            textbox.ReadOnly = true;
        }

        public void initialize_advanced_private_keys_label_keytype(Label label)
        {
            label.Font = new Font("Calibri", 12, FontStyle.Bold);
            label.BackColor = Color.SlateGray;
            label.ForeColor = Color.White;
            label.Text = "Type";
            label.Location = new Point(5, 340);
            label.Size = new Size(50, 30);
        }

        public void initialize_advanced_private_keys_keytype_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            combobox.Items.Add("PPK");
            combobox.Items.Add("OpenSSH");
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = new Point(65, 340);
            combobox.Size = new Size(80, 32);
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
            label.Location = new Point(160, 340);
            label.Size = new Size(60, 30);
        }

        public void initialize_advanced_private_keys_group_combobox(ComboBox combobox, EventHandler combobox_SelectedIndexChanged)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                combobox.Items.Add("");

                foreach (string name in groups.names)
                {
                    combobox.Items.Add(name);
                }
            }
            combobox.Font = new Font("Calibri", 11);
            combobox.Location = new Point(230, 340);
            combobox.Size = new Size(90, 32);
            combobox.Name = "private_keys_group_combobox";
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.SelectedIndexChanged += new EventHandler(combobox_SelectedIndexChanged);
        }

        public void initialize_advanced_private_keys_divider(Label label)
        {
            label.BackColor = Color.White;
            label.ForeColor = Color.White;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(6, 420);
            label.Text = "";
            label.Size = new Size(350, 4);
        }

        public void initialize_advanced_remove_pk_button(Button button, EventHandler ButtonClickPKEvent)
        {
            button.Name = "Remove_PK_button";
            button.Font = new Font("Calibri", 10);
            button.Location = new Point(270, 30);
            button.ForeColor = Color.Black;
            button.Size = new Size(65, 25);
            button.Text = "Remove";
            button.UseVisualStyleBackColor = true;
            button.Click += new EventHandler(ButtonClickPKEvent);
        }


        /// <summary>
        /// Custom methods for handling controls.
        /// </summary>
        public void set_groupbox_location (GroupBox groupbox, int height)
        {
            groupbox.Location = new Point(23, height);
        }

        public void set_group_box_text (GroupBox groupbox, string text)
        {
            groupbox.Text = text;
        }

        public void set_button_location (Button button, int height)
        {
            button.Location = new Point(292, height);
        }

        public void set_combobox_groups (List<GroupBox> containers_list)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                foreach (GroupBox groupbox in containers_list)
                {
                    foreach (ComboBox combobox in groupbox.Controls.OfType<ComboBox>())
                    {
                        string last_value = combobox.Text;
                        combobox.Items.Clear();

                        combobox.Items.Add("");
                        foreach (string name in groups.names)
                        {
                            combobox.Items.Add(name);
                        }

                        combobox.Text = last_value;
                    }
                }
            }
        }

        public void set_passwords_combobox_groups(SplitContainer splitcontainer)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                foreach (ComboBox combobox in splitcontainer.Panel2.Controls.OfType<ComboBox>())
                {
                    combobox.Items.Clear();

                    combobox.Items.Add("");
                    foreach (string name in groups.names)
                    {
                        combobox.Items.Add(name);
                    }
                }
            }
        }

        public void set_pk_combobox_groups(SplitContainer splitcontainer)
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml")))
            {
                GetSavedSessions saved_groups = new GetSavedSessions();
                SavedGroupInfo groups = saved_groups.get_Groups();

                foreach (ComboBox combobox in splitcontainer.Panel1.Controls.OfType<ComboBox>())
                {
                    if (combobox.Name == "private_keys_group_combobox")
                    {
                        combobox.Items.Clear();

                        combobox.Items.Add("");
                        foreach (string name in groups.names)
                        {
                            combobox.Items.Add(name);
                        }
                    }
                }
            }
        }

    }
}
