﻿/*
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
using System.Text;
using System.Collections.Generic;

namespace PuTTY_Storm
{
    class Run
    {
        string password = null;

        // Dictionary (key: hostname => value: output) which contains the full output for DataGridView output column.
        Dictionary<string, string> DataGridViewGetFullOutputDictionary = new Dictionary<string, string>();

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
                    int exitCode = 999;

                    if (command != null)
                    {
                        terminal = client.RunCommand(command);
                        Console.WriteLine("Command: " + command + " executed");
                        Console.WriteLine(terminal.Result);
                        output = ReplaceTabsWithSpaces(terminal.Result, 4).TrimEnd('\r', '\n');
                        exitCode = terminal.ExitStatus;

                        if (exitCode != 0)
                        {
                            output = terminal.Error.TrimEnd('\r', '\n');
                        }
                    }

                    WriteDataGridViewOutput(dataGridView1, _hostname, exitCode, output);

                    client.Disconnect();
                }
            } catch (Exception ex)
            {
                WriteDataGridViewOutput(dataGridView1, _hostname, 999, ex.Message);
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
                    int exitCode = 999;
                    string command = "echo $'" + text.Replace("'", "\\'").Replace("\r", string.Empty) +
                        "'" + " > PuTTY_STORM_TMP.sh; chmod +x PuTTY_STORM_TMP.sh; OUTPUT=\"$(./PuTTY_STORM_TMP.sh 2>&1)\"; echo $OUTPUT; rm PuTTY_STORM_TMP.sh";

                    if (command != null)
                    {
                        terminal = client.RunCommand(command);
                        Console.WriteLine("Command: " + command + " executed");
                        output = terminal.Result.TrimEnd('\r', '\n');
                        exitCode = terminal.ExitStatus;
                    }

                    WriteDataGridViewOutput(dataGridView1, _hostname, exitCode, output);

                    client.Disconnect();
                }
            } catch (Exception ex)
            {
                WriteDataGridViewOutput(dataGridView1, _hostname, 999, ex.Message);
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
                        if (returnCode != null && returnCode != 0)
                        {
                            dataGridView1.Rows[i].Cells["ReturnCodeColumn"].Style.BackColor = System.Drawing.Color.OrangeRed;
                        }
                        else if (returnCode != null && returnCode == 0)
                        {
                            dataGridView1.Rows[i].Cells["ReturnCodeColumn"].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                        dataGridView1.Rows[i].Cells["ReturnCodeColumn"].Value = returnCode;

                        // Truncate the output if it is longer than 200 characters
                        if (output.Length > 200)
                        {
                            string truncate = output.Substring(0, 200) + "\r\n\r\nOutput truncated... Double click to get the full output...";
                            dataGridView1.Rows[i].Cells["OutputColumn"].Value = truncate;

                            if (DataGridViewGetFullOutputDictionary.ContainsKey(hostname))
                            {
                                DataGridViewGetFullOutputDictionary[hostname] = output;
                            }
                            else
                            {
                                DataGridViewGetFullOutputDictionary.Add(hostname, output);
                            }
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells["OutputColumn"].Value = output;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tabs are represented as non-printable characters in DataGridView, therefore we need
        /// to replace tabs with required number of spaces.
        /// 
        /// The solution below replaces tab with up to 4 or 8 spaces.
        /// The logic iterates through the input string, one character at a time 
        /// and keeps track of the current position (column #) in output string.
        /// 
        /// - If it encounters \t (tab char) - Finds the next tab stop, calculates how many spaces 
        ///   it needs to get to the next tab stop, replaces \t with those number of spaces.
        /// - If \n (new line) - Appends it to the output string and Resets the position pointer to 1 
        ///   on new line.
        /// - Any other characters - Appends it to the output string and increments the position.
        /// </summary>
        private int GetNearestTabStop(int currentPosition, int tabLength)
        {
            // if already at the tab stop, jump to the next tab stop.
            if ((currentPosition % tabLength) == 1)
                currentPosition += tabLength;
            else
            {
                // if in the middle of two tab stops, move forward to the nearest.
                for (int i = 0; i < tabLength; i++, currentPosition++)
                    if ((currentPosition % tabLength) == 1)
                        break;
            }

            return currentPosition;
        }

        private string ReplaceTabsWithSpaces(string input, int tabLength)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder output = new StringBuilder();

            int positionInOutput = 1;
            foreach (var c in input)
            {
                switch (c)
                {
                    case '\t':
                        int spacesToAdd = GetNearestTabStop(positionInOutput, tabLength) - positionInOutput;
                        output.Append(new string(' ', spacesToAdd));
                        positionInOutput += spacesToAdd;
                        break;

                    case '\n':
                        output.Append(c);
                        positionInOutput = 1;
                        break;

                    default:
                        output.Append(c);
                        positionInOutput++;
                        break;
                }
            }
            return output.ToString();
        }

        public Dictionary<string, string> GetDataGridViewGetFullOutput()
        {
            return this.DataGridViewGetFullOutputDictionary;
        }

        public void ResetDataGridViewGetFullOutput()
        {
            this.DataGridViewGetFullOutputDictionary.Clear();
        }

    }
}
