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

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
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
        }
    }
}
