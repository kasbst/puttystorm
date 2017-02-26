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
using System.Xml;
using System.IO;

namespace PuTTY_Storm
{
    class SaveSessions
    {
        /// <summary>
        /// Save sessions to the sessions.xml configuration file.
        /// </summary>
        public void Save_sessions(List<GroupBox> containers_list)
        {
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "sessions.xml");

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm"));
            }
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Sessions");

                foreach (GroupBox container in containers_list)
                {
                    string hostname = null;
                    string username = null;
                    string password = null;
                    string c_count = null;
                    string group = null;
                    string sub_group = null;

                    foreach (Control control in container.Controls)
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
                            // Handle passwordless login
                            if (control.Text == "")
                            {
                                password = " ";
                            } else
                            {
                                password = control.Text;
                            }
                        }
                        if (control.Name == "combobox")
                        {
                            if (control.Text == "")
                            {
                                group = " ";
                            } else
                            {
                                group = control.Text;
                            }
                        }

                        if (control.Name == "sub_groups_combobox")
                        {
                            if (control.Text == "")
                            {
                                sub_group = " ";
                            }
                            else
                            {
                                sub_group = control.Text;
                            }
                        }
                    }

                    foreach (NumericUpDown ctlNumeric in container.Controls.OfType<NumericUpDown>())
                    {
                        c_count = ctlNumeric.Value.ToString();                       
                    }

                    if (hostname != "" && username != "")
                    {
                        writer.WriteStartElement("Session");

                        writer.WriteElementString("hostname", hostname);
                        writer.WriteElementString("username", username);
                        writer.WriteElementString("password", AESEncryptDecrypt.Encrypt(password));
                        writer.WriteElementString("count", c_count);
                        writer.WriteElementString("group", group);
                        writer.WriteElementString("subgroup", sub_group);

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Backup_Sessions();
        }

        /// <summary>
        /// Save groups to the groups.xml configuration file.
        /// </summary>
        public void Save_groups(List<Panel> Groups)
        {
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "groups.xml");

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm"));
            }

            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Groups");

                foreach (Panel container in Groups)
                {
                    string name = null;

                    foreach (Control control in container.Controls)
                    {
                        if (control.Name == "group_name_label")
                        {
                            name = control.Text;
                        }
                    }

                    if (name != "")
                    {
                        writer.WriteStartElement("Group");

                        writer.WriteElementString("name", name);

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Backup_Groups();
        }

        /// <summary>
        /// Save private keys to the privatekeys.xml configuration file.
        /// </summary>
        public void Save_PrivateKeys(List<Panel> PrivateKeys)
        {
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml");

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm"));
            }

            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("PrivateKeys");

                foreach (Panel container in PrivateKeys)
                {
                    string pk_name = null;
                    string pk_type = null;
                    string pk_group = null;

                    foreach (Control control in container.Controls)
                    {
                        if (control.Name == "pk_name_label")
                        {
                            pk_name = control.Text;
                        }

                        if (control.Name == "pk_type_label")
                        {
                            pk_type = control.Text.Replace("Type: ", string.Empty);
                        }

                        if (control.Name == "pk_group_label")
                        {
                            pk_group = control.Text.Replace("Group: ", string.Empty);
                        }
                    }

                    if (pk_name != "")
                    {
                        writer.WriteStartElement("PrivateKey");

                        writer.WriteElementString("name", pk_name);
                        writer.WriteElementString("type", pk_type);
                        writer.WriteElementString("group", pk_group);

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            GetSavedSessions saved_data = new GetSavedSessions();
            SavedPrivatekeysInfo privatekeys = null;

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "PuTTYStorm", "privatekeys.xml")))
            {
                privatekeys = saved_data.get_PrivateKeys();
            }

            if (privatekeys.names.Count != 0)
            {
                Backup_PrivateKeys();
            }
        }

        /// <summary>
        /// Make a backup of privatekeys.xml file after each save file operation.
        /// </summary>
        private void Backup_PrivateKeys()
        {
            try
            {
                String FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PuTTYStorm", "privatekeys.xml");

                if (File.Exists(FilePath))
                {
                    String NewFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                        "PuTTYStorm", "privatekeys-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff") + ".xml");
                    File.Copy(FilePath, NewFilePath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            DeleteOldPrivateKeys();
        }

        /// <summary>
        /// Make a backup of groups.xml file after each save file operation.
        /// </summary>
        private void Backup_Groups()
        {
            try
            {
                String FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PuTTYStorm", "groups.xml");

                if (File.Exists(FilePath))
                {
                    String NewFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                        "PuTTYStorm", "groups-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff") + ".xml");
                    File.Copy(FilePath, NewFilePath);
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            DeleteOldGroups();
        }

        /// <summary>
        /// Make a backup of sesssions.xml file after each save file operation.
        /// </summary>
        private void Backup_Sessions()
        {
            try
            {
                String FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PuTTYStorm", "sessions.xml");

                if (File.Exists(FilePath))
                {
                    String NewFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                        "PuTTYStorm", "sessions-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff") + ".xml");
                    File.Copy(FilePath, NewFilePath);
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            DeleteOldSessions();
        }

        /// <summary>
        /// Delete old sessions-*.xml backup files. Order them youngest first, skips the first 20, and deletes the rest.
        /// Always keep the 20 most recent files.
        /// </summary>
        List<FileInfo> Sessionsfi;
        private void DeleteOldSessions()
        {
            try
            {
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
                {

                    String FolderPath = Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm");

                    String FilesToDelete = @"sessions-*";
                    var fileList = Directory.GetFiles(FolderPath, FilesToDelete);

                    Console.WriteLine("### Number of session-* files: " + fileList.Count());

                    Sessionsfi = new List<FileInfo>();

                    foreach (var file in fileList)
                    {
                        Sessionsfi.Add(new FileInfo(file));
                    }

                    foreach (var f in Sessionsfi.OrderByDescending(x => x.CreationTime).Skip(20))
                    {
                        f.Delete();
                    }
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Delete old groups-*.xml backup files. Order them youngest first, skips the first 20, and deletes the rest.
        /// Always keep the 20 most recent files.
        /// </summary>
        List<FileInfo> Groupsfi;
        private void DeleteOldGroups()
        {
            try
            {
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm");

                    String FilesToDelete = @"groups-*";
                    var fileList = Directory.GetFiles(FolderPath, FilesToDelete);

                    Console.WriteLine("### Number of groups-* files: " + fileList.Count());

                    Groupsfi = new List<FileInfo>();

                    foreach (var file in fileList)
                    {
                        Groupsfi.Add(new FileInfo(file));
                    }

                    foreach (var f in Groupsfi.OrderByDescending(x => x.CreationTime).Skip(20))
                    {
                        f.Delete();
                    }
                }
            } catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        /// <summary>
        /// Delete old privatekeys-*.xml backup files. Order them youngest first, skips the first 20, and deletes the rest.
        /// Always keep the 20 most recent files.
        /// </summary>
        List<FileInfo> PrivateKeysfi;
        private void DeleteOldPrivateKeys()
        {
            try
            {
                if (Directory.Exists(Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm")))
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath
                    (Environment.SpecialFolder.MyDocuments), "PuTTYStorm");

                    String FilesToDelete = @"privatekeys-*";
                    var fileList = Directory.GetFiles(FolderPath, FilesToDelete);

                    Console.WriteLine("### Number of privatekeys-* files: " + fileList.Count());

                    PrivateKeysfi = new List<FileInfo>();

                    foreach (var file in fileList)
                    {
                        PrivateKeysfi.Add(new FileInfo(file));
                    }

                    foreach (var f in PrivateKeysfi.OrderByDescending(x => x.CreationTime).Skip(20))
                    {
                        f.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


    }
}
