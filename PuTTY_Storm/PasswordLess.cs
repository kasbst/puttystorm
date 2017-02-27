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
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace PuTTY_Storm
{
    class PasswordLess
    {
        /// <summary>
        /// Check which groups in created sessions containers (GroupBoxes) are part
        /// of private keys setup. If group is part of PK setup change properties 
        /// of password textboxes.
        /// Calls IsGroupBetweenPrivateKeys().
        /// </summary>
        public void DetermineIfSessionGroupIsPasswordLess(List<GroupBox> containers_list)
        {
            GetSavedSessions saved_data = new GetSavedSessions();
            SavedPrivatekeysInfo privatekeys = null;

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                privatekeys = saved_data.get_PrivateKeys();
            } else
            {
                return;
            }

            foreach (GroupBox container in containers_list)
            {
                string group = null;
                TextBox password_textbox = null;

                foreach (Control control in container.Controls)
                {                   
                    if (control.Name == "password_textbox")
                    {
                        password_textbox = (TextBox)control;
                    }

                    if (control.Name == "combobox")
                    {
                        group = control.Text;
                    }                  
                }
                if (IsGroupBetweenPrivateKeys(privatekeys, group))
                {
                    password_textbox.ReadOnly = true;
                    password_textbox.BackColor = System.Drawing.Color.White;
                    password_textbox.ForeColor = Color.SlateGray;
                    password_textbox.UseSystemPasswordChar = false;
                    password_textbox.Text = "PASSWORDLESS";
                } else
                {
                    if (password_textbox.Text == "PASSWORDLESS")
                    {
                        password_textbox.ReadOnly = false;
                        password_textbox.BackColor = System.Drawing.Color.White;
                        password_textbox.ForeColor = Color.Black;
                        password_textbox.UseSystemPasswordChar = true;
                        password_textbox.Text = "";
                    }
                }
            }
        }

        /// <summary>
        /// Check if session group is between private keys and return true/false based on that.
        /// </summary>
        public bool IsGroupBetweenPrivateKeys(SavedPrivatekeysInfo privatekeys, string session_group)
        {
            bool IsPrivateKey = false;

            if (privatekeys.names.Count != 0)
            {
                for (int i = 0; i < privatekeys.names.Count; i++)
                {
                    if (privatekeys.groups[i] == session_group)
                    {
                        IsPrivateKey = true;
                    }
                    
                }
            }
            return IsPrivateKey;
        }

        /// <summary>
        /// Get path of PPK private key.
        /// </summary>
        public string GetPPKPrivateKeyForGroup(SavedPrivatekeysInfo privatekeys, string session_group)
        {
            string PPKPrivateKey = null;

            if (privatekeys.names.Count != 0)
            {
                for (int i = 0; i < privatekeys.names.Count; i++)
                {
                    if (privatekeys.groups[i] == session_group && privatekeys.types[i] == "PPK")
                    {
                        PPKPrivateKey = privatekeys.names[i];
                    }
                }
            }
            return PPKPrivateKey;
        }

        /// <summary>
        /// Get path of OpenSSH private key.
        /// </summary>
        public string GetOpenSSHPrivateKeyForGroup(SavedPrivatekeysInfo privatekeys, string session_group)
        {
            string OpenSSHPrivateKey = null;

            if (privatekeys.names.Count != 0)
            {
                for (int i = 0; i < privatekeys.names.Count; i++)
                {
                    if (privatekeys.groups[i] == session_group && privatekeys.types[i] == "OpenSSH")
                    {
                        OpenSSHPrivateKey = privatekeys.names[i];
                    }
                }
            }
            return OpenSSHPrivateKey;
        }

        /// <summary>
        /// Get passphrase for encrypted openSSH private key
        /// </summary>
        public string GetOpenSSHPrivateKeyPassPhrase(SavedPrivatekeysInfo privatekeys, string session_group)
        {
            string PassPhrase = null;

            if (privatekeys.names.Count != 0)
            {
                for (int i = 0; i < privatekeys.names.Count; i++)
                {
                    if (privatekeys.groups[i] == session_group && privatekeys.types[i] == "OpenSSH")
                    {
                        PassPhrase = privatekeys.pwds[i];
                    }
                }
            }
            return PassPhrase;
        }

        /// <summary>
        /// Helper function used when creating a new connection from already opened sessions form.
        /// This is needed because we don't have a direct access to the session group from there.
        /// Therefore we need to find a group for particular hostname from the main containers list.
        /// </summary>
        public string GetGroupForPwdLessHostname(List<GroupBox> containers_list, string new_hostname)
        {
            string return_group = null;

            foreach (GroupBox container in containers_list)
            {
                string group = null;
                string hostname = null;

                foreach (Control control in container.Controls)
                {
                    if (control.Name == "hostname_textbox")
                    {
                        
                         hostname = KotarakMainForm.ReadValueFromControl(control);                      
                    }

                    if (control.Name == "combobox")
                    {
                        if (control.InvokeRequired)
                        {
                            return (string)control.Invoke(new Func<String>(() => GetGroupForPwdLessHostname(containers_list, new_hostname)));
                        }
                        else
                        {
                            group = KotarakMainForm.ReadValueFromControl(control);
                        }
                    }
                }

                if (new_hostname == hostname)
                {
                    return_group = group;
                    break;
                }
            }
            Console.WriteLine("## GROUP RETURNED: " + return_group);
            return return_group;
        }        


    }
}
