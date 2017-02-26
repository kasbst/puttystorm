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

namespace PuTTY_Storm
{
    class Run
    {
        /// <summary>
        /// Handling of Shell command execution.
        /// </summary>
        public void ExecuteCommand(DataGridView dataGridView1, string text, string hostname, string username, string password, string PrivateKey)
        {
            WriteDataGridViewOutput(dataGridView1, hostname, null, "Running...");

            ConnectionInfo con = null;

            if (password != null && password != "" && !Regex.IsMatch(password, @"\s+") && PrivateKey == null)
            {
                Console.WriteLine("## Kotarak using password for login");

                con = new ConnectionInfo(hostname, 22, username, new PasswordAuthenticationMethod(username, password));
            }
            else if (password == null && PrivateKey != null)
            {
                Console.WriteLine("## Kotarak using private key for login");

                var keyFile = new PrivateKeyFile(PrivateKey);
                var keyFiles = new[] { keyFile };
                con = new ConnectionInfo(hostname, 22, username, new PrivateKeyAuthenticationMethod(username, keyFiles));
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

                    WriteDataGridViewOutput(dataGridView1, hostname, exitCode, output);

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
        public void ExecuteBashScript(DataGridView dataGridView1, string text, string hostname, string username, string password, string PrivateKey)
        {
            WriteDataGridViewOutput(dataGridView1, hostname, null, "Running...");

            ConnectionInfo con = null;

            if (password != null && password != "" && !Regex.IsMatch(password, @"\s+") && PrivateKey == null)
            {
                Console.WriteLine("## Kotarak using password for login");

                con = new ConnectionInfo(hostname, 22, username, new PasswordAuthenticationMethod(username, password));
            }
            else if (password == null && PrivateKey != null)
            {
                Console.WriteLine("## Kotarak using private key for login");

                var keyFile = new PrivateKeyFile(PrivateKey);
                var keyFiles = new[] { keyFile };
                con = new ConnectionInfo(hostname, 22, username, new PrivateKeyAuthenticationMethod(username, keyFiles));
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

                    WriteDataGridViewOutput(dataGridView1, hostname, exitCode, output);

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
