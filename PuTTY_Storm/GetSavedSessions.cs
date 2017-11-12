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
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public class SavedConnectionInfo
    {
        public List<string> hostnames = new List<string>();
        public List<string> usernames = new List<string>();
        public List<string> passwords = new List<string>();
        public List<string> counts = new List<string>();
        public List<string> groups = new List<string>();
        public List<string> sub_groups = new List<string>();
    }

    public class SavedGroupInfo
    {
        public List<string> names = new List<string>();
    }

    public class SavedPrivatekeysInfo
    {
        public List<string> names = new List<string>();
        public List<string> types = new List<string>();
        public List<string> groups = new List<string>();
        public List<string> pwds = new List<string>();
    }

    class GetSavedSessions
    {
        /// <summary>
        /// Get saved sessions from sessions.xml configuration file.
        /// </summary>
        /// <returns>Class with Lists of sessions conenction info</returns>
        public SavedConnectionInfo get_Sessions()
        {
            SavedConnectionInfo xml_connection_info = new SavedConnectionInfo();

            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "sessions.xml");

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {

                                case "hostname":
                                    if (reader.Read())
                                    {
                                        xml_connection_info.hostnames.Add(reader.Value);
                                    }
                                    break;
                                case "username":
                                    if (reader.Read())
                                    {
                                        xml_connection_info.usernames.Add(reader.Value);
                                    }
                                    break;
                                case "password":
                                    if (reader.Read())
                                    {
                                        // Handle passwordless login
                                        if (reader.Value == " ")
                                        {
                                            xml_connection_info.passwords.Add(null);
                                        }
                                        else
                                        {
                                            xml_connection_info.passwords.Add(AESEncryptDecrypt.Decrypt(reader.Value));
                                        }
                                    }
                                    break;
                                case "count":
                                    if (reader.Read())
                                    {
                                        xml_connection_info.counts.Add(reader.Value);
                                    }
                                    break;
                                case "group":
                                    if (reader.Read())
                                    {
                                        if (reader.Value == " ")
                                        {
                                            xml_connection_info.groups.Add(null);
                                        }
                                        else
                                        {
                                            xml_connection_info.groups.Add(reader.Value);
                                        }
                                    }
                                    break;
                                case "subgroup":
                                    if (reader.Read())
                                    {
                                        if (reader.Value == " ")
                                        {
                                            xml_connection_info.sub_groups.Add(null);
                                        }
                                        else
                                        {
                                            xml_connection_info.sub_groups.Add(reader.Value);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return xml_connection_info;
        }

        /// <summary>
        /// Get saved groups from groups.xml configuration file. 
        /// </summary>
        /// <returns>Class with List of groups</returns>
        public SavedGroupInfo get_Groups()
        {
            SavedGroupInfo xml_group_info = new SavedGroupInfo();

            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml");

            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {

                                case "name":
                                    if (reader.Read())
                                    {
                                        xml_group_info.names.Add(reader.Value);
                                    }
                                    break;
                            }
                        }
                    }
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return xml_group_info;
        }

        /// <summary>
        /// Get saved private keys from privatekeys.xml configuration file. 
        /// </summary>
        /// <returns>Class with List of private keys</returns>
        public SavedPrivatekeysInfo get_PrivateKeys()
        {
            SavedPrivatekeysInfo xml_privatekeys_info = new SavedPrivatekeysInfo();

            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml");
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {

                                case "name":
                                    if (reader.Read())
                                    {
                                        xml_privatekeys_info.names.Add(reader.Value);
                                    }
                                    break;
                                case "type":
                                    if (reader.Read())
                                    {
                                        xml_privatekeys_info.types.Add(reader.Value);
                                    }
                                    break;
                                case "group":
                                    if (reader.Read())
                                    {
                                        xml_privatekeys_info.groups.Add(reader.Value);
                                    }
                                    break;
                                case "pwd":
                                    if (reader.Read())
                                    {
                                        if (reader.Value == " ")
                                        {
                                            xml_privatekeys_info.pwds.Add(null);
                                        } else
                                        {
                                            xml_privatekeys_info.pwds.Add(AESEncryptDecrypt.Decrypt(reader.Value));
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            return xml_privatekeys_info;
        }

        /// <summary>
        /// Extract saved sessions from SavedConnectionInfo data structure to the List of Dictionaries (hashes).
        /// This data structure is used to fill GlobalVar.ConfigSessionsData at the program startup. Later,
        /// it is used to check whether there were any configuration changes.
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ExtractConfigSessionsData(SavedConnectionInfo sessions)
        {
            List<Dictionary<string, object>> ConfigSessionsData = new List<Dictionary<string, object>>();

            for (int i = 0; i < sessions.hostnames.Count; i++)
            {
                string password = null;
                if (sessions.passwords[i] == null)
                {
                    password = " ";
                } else
                {
                    password = sessions.passwords[i];
                }

                ConfigSessionsData.Add(new Dictionary<string, object>()
                {
                    {"hostname", sessions.hostnames[i] },
                    {"username", sessions.usernames[i] },
                    {"password", password },
                    {"c_count", sessions.counts[i] },
                    {"group", (sessions.groups[i] == null ? " " : sessions.groups[i]) },
                    {"sub_group", (sessions.sub_groups[i] == null ? " " :  sessions.sub_groups[i]) }
                });
            }
            return ConfigSessionsData;
        }

    }
}
