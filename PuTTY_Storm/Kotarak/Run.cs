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
using System.Windows.Forms;
using Renci.SshNet;
using System.Text.RegularExpressions;
using Renci.SshNet.Common;

namespace PuTTY_Storm
{
    class Run
    {
        string password = null;

        /// <summary>
        /// Event handler for KeyboardInteractiveAuthenticationMethod which passes the password
        /// </summary>
        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = this.password;
                }
            }
        }

        /// <summary>
        /// Handling of Shell command execution.
        /// </summary>
        public void ExecuteCommand(DataGridView dataGridView1, string text, string _hostname, string _username, 
            string _password, string _PrivateKey, string _pk_pwd)
        {
            WriteDataGridViewOutput(dataGridView1, _hostname, null, "Running...");

            ConnectionInfo con = null;

            // Use KeyboardInteractiveAuthentication or PasswordAuthenticationMethod
            if (_password != null && _password != "" && !Regex.IsMatch(_password, @"\s+") && _PrivateKey == null)
            {
                Console.WriteLine("## Kotarak using password for login");

                this.password = _password;

                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(_username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                con = new ConnectionInfo(_hostname, 22, _username, new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod(_username, _password),
                    keybAuth
                });
            }
            // Otherwise we have setup PrivateKeyAuthenticationMethod
            else if (_password == null && _PrivateKey != null)
            {
                Console.WriteLine("## Kotarak using OpenSSH private key for login");

                PrivateKeyFile keyFile;
                if (_pk_pwd == null)
                {
                    Console.WriteLine("## OpenSSH private key is not encrypted");
                    keyFile = new PrivateKeyFile(_PrivateKey);
                } else
                {
                    Console.WriteLine("## OpenSSH private key IS encrypted!");
                    keyFile = new PrivateKeyFile(_PrivateKey, _pk_pwd);
                }

                var keyFiles = new[] { keyFile };
                con = new ConnectionInfo(_hostname, 22, _username, new PrivateKeyAuthenticationMethod(_username, keyFiles));
            }

            string[] lines = null;
            MatchCollection commands = null;
            string command = null;

            try
            {
                // Parse command from Editor textbox
                if (text.Length > 0)
                {
                    lines = text.Split('\n');

                    foreach (var line in lines)
                    {
                        string match_command = "^\".+?\"$";
                        commands = Regex.Matches(line, match_command);
                    }

                    foreach (Match m in commands)
                    {
                        command = m.Value.Substring(1, m.Value.Length - 2);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                // Connect and execute command
                using (var client = new SshClient(con))
                {
                    client.Connect();

                    // Default params if no command is provided
                    SshCommand terminal = null;
                    string output = "Nothing to be executed!";
                    int exitCode = 9999;

                    if (command != null)
                    {
                        terminal = client.RunCommand(command);
                        Console.WriteLine("Command: " + command + " executed");
                        output = terminal.Result;
                        exitCode = terminal.ExitStatus;

                        if (exitCode != 0)
                        {
                            output = terminal.Error;
                        }
                    }

                    WriteDataGridViewOutput(dataGridView1, _hostname, exitCode, output);

                    client.Disconnect();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handling of Bash script execution
        /// </summary>
        public void ExecuteBashScript(DataGridView dataGridView1, string text, string _hostname, string _username, 
            string _password, string _PrivateKey, string _pk_pwd)
        {
            WriteDataGridViewOutput(dataGridView1, _hostname, null, "Running...");

            ConnectionInfo con = null;

            // Use KeyboardInteractiveAuthentication or PasswordAuthenticationMethod
            if (_password != null && _password != "" && !Regex.IsMatch(_password, @"\s+") && _PrivateKey == null)
            {
                Console.WriteLine("## Kotarak using password for login");

                this.password = _password;

                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(_username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                con = new ConnectionInfo(_hostname, 22, _username, new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod(_username, _password),
                    keybAuth
                });
            }
            // Otherwise we have setup PrivateKeyAuthenticationMethod
            else if (_password == null && _PrivateKey != null)
            {
                Console.WriteLine("## Kotarak using private key for login");

                PrivateKeyFile keyFile;
                if (_pk_pwd == null)
                {
                    Console.WriteLine("## OpenSSH private key is not encrypted");
                    keyFile = new PrivateKeyFile(_PrivateKey);
                }
                else
                {
                    Console.WriteLine("## OpenSSH private key IS encrypted!");
                    keyFile = new PrivateKeyFile(_PrivateKey, _pk_pwd);
                }

                var keyFiles = new[] { keyFile };
                con = new ConnectionInfo(_hostname, 22, _username, new PrivateKeyAuthenticationMethod(_username, keyFiles));
            }

            try
            {
                // Connect and execute command
                using (var client = new SshClient(con))
                {
                    client.Connect();

                    // Default params if no command is provided
                    SshCommand terminal = null;
                    string output = "Nothing to be executed!";
                    int exitCode = 9999;
                    string command = "echo $'" + text.Replace("'", "\\'").Replace("\r", string.Empty) +
                        "'" + " > PuTTY_STORM_TMP.sh; chmod +x PuTTY_STORM_TMP.sh; OUTPUT=\"$(./PuTTY_STORM_TMP.sh 2>&1)\"; echo $OUTPUT; rm PuTTY_STORM_TMP.sh";

                    if (command != null)
                    {
                        terminal = client.RunCommand(command);
                        Console.WriteLine("Command: " + command + " executed");
                        output = terminal.Result;
                        exitCode = terminal.ExitStatus;
                    }

                    WriteDataGridViewOutput(dataGridView1, _hostname, exitCode, output);

                    client.Disconnect();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Display results of Shell command or Bash script execution in DataGridView.
        /// </summary>
        private void WriteDataGridViewOutput(DataGridView dataGridView1, string hostname, int? returnCode, string output)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action<DataGridView, string, int?, string>(WriteDataGridViewOutput), dataGridView1, hostname, returnCode, output);
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells["HostnameColumn"].Value.ToString() == hostname)
                    {
                        dataGridView1.Rows[i].Cells["ReturnCodeColumn"].Value = returnCode;
                        dataGridView1.Rows[i].Cells["OutputColumn"].Value = output;
                    }
                }
            }
        }


    }
}
