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

using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public partial class KotarakMainForm : Form
    {
        private Run run = new Run();
        private GetSavedSessions saved_data = new GetSavedSessions();
        private PasswordLess IsPasswordLess = new PasswordLess();
        private SetControls custom_controls = new SetControls();

        static string mainKeywords = "if then else elif fi case esac for select while until do done in function time coproc";
        static string secondaryKeywords = "echo exit exec let";
        private BashLexer BashLexer = new BashLexer(mainKeywords, secondaryKeywords);
        private EditorInit EditorInit = new EditorInit();

        List<GroupBox> containers_list;
        public delegate void ExecuteFunction(DataGridView dataGridView1, string text, string hostname, string login, string password, string PrivateKey);

        public KotarakMainForm(List<GroupBox> _containers_list)
        {
            InitializeComponent();
            this.containers_list = _containers_list;

            custom_controls.initialize_kotarak_group_subgroup_combobox(SelectGroupSubGroupCombobox);

            // By default we are using login credentials from saved sessions.
            // Login and password textbox is disabled.
            LoginTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            LoginLabel.ForeColor = Color.LightGray;
            PasswordLabel.ForeColor = Color.LightGray;

            ForceAccountCheckBox.CheckStateChanged += new EventHandler(ForceAccountCheckBox_CheckStateChanged);

            BashScriptRadioButton.Checked = true;

            // Initialize datagridview with servers
            InitializeDataGridView();
            
            // Initialize scintillaNet editor with Bash lexer
            EditorInit.BashInit(BashScriptRadioButton, scintilla1);
        }

        private void scintilla_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            var startPos = scintilla1.GetEndStyled();
            var endPos = e.Position;

            BashLexer.Style(scintilla1, startPos, endPos);
        }

        /// <summary>
        /// Enable login and password textbox if we want to use different login credentials
        /// </summary>
        private void ForceAccountCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (ForceAccountCheckBox.Checked)
            {
                LoginTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;
                LoginLabel.ForeColor = Color.White;
                PasswordLabel.ForeColor = Color.White;
            } else
            {
                LoginTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;
                LoginLabel.ForeColor = Color.LightGray;
                PasswordLabel.ForeColor = Color.LightGray;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AutoSizeRowsMode(object sender, DataGridViewAutoSizeModeEventArgs e)
        {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        /// <summary>
        /// Handling of Bash or Shell commands execution
        /// See also: public void ExecuteDelegate(Tuple<string, string, string> _credentials, ExecuteFunction f)
        /// </summary>
        private void RunCodeButton_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ReturnCodeColumn"].Value = null;
                row.Cells["OutputColumn"].Value = null;
            }

            if (CommandRadioButton.Checked)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["SelectColumn"].Value))
                    {
                        var _credentials = GetKotarakCredentials(dataGridView1.Rows[i].Cells["HostnameColumn"].Value.ToString());

                        Task newtask = Task.Factory.StartNew(() =>
                            {
                                ExecuteDelegate(_credentials, run.ExecuteCommand);
                            }, TaskCreationOptions.LongRunning
                        );
                    }
                }
            }

            if (BashScriptRadioButton.Checked)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["SelectColumn"].Value))
                    {
                        var _credentials = GetKotarakCredentials(dataGridView1.Rows[i].Cells["HostnameColumn"].Value.ToString());

                        Task newtask = Task.Factory.StartNew(() =>
                        {
                            ExecuteDelegate(_credentials, run.ExecuteBashScript);
                        }, TaskCreationOptions.LongRunning
                        );
                    }
                }
            }
        }

        /// <summary>
        /// See: private void RunCodeButton_Click(object sender, EventArgs e)
        /// </summary>
        public void ExecuteDelegate(Tuple<string, string, string> _credentials, ExecuteFunction f)
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
                        MessageBox.Show("You are going to use Kotarak passwordless login, " + Environment.NewLine +
                            "however private key " + PrivateKey + " doesn't exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                        return;
                    }

                    password = null;
                }
            }

            // Priority Login
            if (ForceAccountCheckBox.Checked)
            {
                string force_username;
                string force_password;
                Control[] force_username_texbox = this.Controls.Find("LoginTextBox", true);
                Control[] force_password_texbox = this.Controls.Find("PasswordTextBox", true);

                force_username = force_username_texbox[0].Text;
                force_password = force_password_texbox[0].Text;

                login = force_username;
                password = force_password;

                PrivateKey = null;
            }

            f(dataGridView1, ReadValueFromControl(scintilla1), hostname, login, password, PrivateKey);
        }

        /// <summary>
        /// Select all devices in DataGridView
        /// </summary>
        private void SelectAllDevicesButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["SelectColumn"].Value = true;
            }
        }

        /// <summary>
        /// Unselect all devices in DataGridView
        /// </summary>
        private void UnselectAllDevicesButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["SelectColumn"].Value = false;
            }
        }

        /// <summary>
        /// If BashScriptRadioButton is checked - initialize scintillaNet editor
        /// with bash lexer for bash processing.
        /// </summary>
        private void BashScriptRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            EditorInit.BashInit(BashScriptRadioButton, scintilla1);
        }

        /// <summary>
        /// If CommandRadioButton is checked - initialize scintillaNet editor for
        /// raw commands execution processing.
        /// </summary>
        private void CommandRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            EditorInit.CommandInit(CommandRadioButton, scintilla1);
        }

        /// <summary>
        /// Save bash script from scintillaNet editor to disk
        /// </summary>
        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (!BashScriptRadioButton.Checked)
            {
                MessageBox.Show("Only Shell Script can be exported!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Shell Script (Bash) (*.sh;*.bash;*.zsh;*..bashrc;*..bash_profile;*..bash_login;*..profile;*..bash_logout) | *.sh;*.bash;*.zsh;*..bashrc;*..bash_profile;*..bash_login;*..profile;*..bash_logout|All files (*.*)|*.*";
            savefile.DefaultExt = "sh";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))
                {
                    if (savefile.FileName == null)
                    {
                        MessageBox.Show("File Name is missing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    } else
                    {
                        sw.WriteLine(scintilla1.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Load bash script from disk and import it to scintillaNet editor
        /// </summary>
        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (!BashScriptRadioButton.Checked)
            {
                MessageBox.Show("Only Shell Script can be imported!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Shell Script (Bash) (*.sh;*.bash;*.zsh;*..bashrc;*..bash_profile;*..bash_login;*..profile;*..bash_logout) | *.sh;*.bash;*.zsh;*..bashrc;*..bash_profile;*..bash_login;*..profile;*..bash_logout|All files (*.*)|*.*";
            openfiledialog.RestoreDirectory = true;

            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                if (openfiledialog.OpenFile() != null)
                {
                    using (StreamReader sr = new StreamReader(openfiledialog.FileName))
                    {
                        scintilla1.Text = sr.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Initialize DataGridView with devices from sessions configuration.
        /// </summary>
        private void InitializeDataGridView ()
        {
            this.dataGridView1.DefaultCellStyle.Font = new Font("Courier new", 10);
            this.dataGridView1.Size = new Size(1200, 500);
            this.dataGridView1.AutoScrollOffset = new Point(200, 300);

            for (int i = 0; i < containers_list.Count; i++)
            {
                Control[] hostname_textbox = containers_list[i].Controls.Find("hostname_textbox", true);

                if (!(hostname_textbox[0].Text == null || hostname_textbox[0].Text == ""))
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = i + 1;
                    row.Cells[2].Value = hostname_textbox[0].Text;
                    dataGridView1.Rows.Add(row);
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
            }

            this.dataGridView1.AllowUserToAddRows = false;
        }

        /// <summary>
        /// Get login credentials from sessions configuration
        /// </summary>
        private Tuple<string, string, string> GetKotarakCredentials(string gridviewhostname)
        {       
            string hostname = null;
            string username = null;
            string password = null;

            Regex regex = new Regex(gridviewhostname);

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
        /// Generic method to access and read values from controls which are running 
        /// on UI thread. Kotarak processing is executed via Long running tasks
        /// </summary>
        public static string ReadValueFromControl(Control control)
        {
            if (control.InvokeRequired)
            {
                return (string)control.Invoke(new Func<string>(() => ReadValueFromControl(control)));
            }
            else
            {
                string return_value = control.Text;
                return return_value;
            }
        }

        /// <summary>
        /// Select devices in DataGridView based on groups or sub-groups which they belong to
        /// </summary>
        private void SelectGroupSubGroupButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["SelectColumn"].Value = false;
            }

            for (int i = 0; i < containers_list.Count; i++)
            {
                Control[] session_hostname = containers_list[i].Controls.Find("hostname_textbox", true);
                Control[] session_group = containers_list[i].Controls.Find("combobox", true);
                Control[] session_subgroup = containers_list[i].Controls.Find("sub_groups_combobox", true);

                if (!(session_hostname[0].Text == null || session_hostname[0].Text != ""))
                {
                    continue;
                }

                if (session_hostname[0].Text == dataGridView1.Rows[i].Cells["HostnameColumn"].Value.ToString())
                {
                    if (session_group[0].Text == SelectGroupSubGroupCombobox.Text || session_subgroup[0].Text == SelectGroupSubGroupCombobox.Text)
                    {
                        dataGridView1.Rows[i].Cells["SelectColumn"].Value = true;
                    }
                }
                
            }
        }



    }
}
