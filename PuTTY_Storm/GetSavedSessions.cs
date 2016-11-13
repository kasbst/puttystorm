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

namespace PuTTY_Storm
{
    public class SavedConnectionInfo
    {
        public List<string> hostnames = new List<string>();
        public List<string> usernames = new List<string>();
        public List<string> passwords = new List<string>();
        public List<string> counts = new List<string>();
        public List<string> groups = new List<string>();
    }

    public class SavedGroupInfo
    {
        public List<string> names = new List<string>();
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
                                    } else
                                    {
                                        //Console.WriteLine(AESEncryptDecrypt.Decrypt(reader.Value));
                                        xml_connection_info.passwords.Add(AESEncryptDecrypt.Decrypt(reader.Value));
                                        //xml_connection_info.passwords.Add(reader.Value);
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
                                    } else
                                    {
                                        xml_connection_info.groups.Add(reader.Value);
                                    }                                   
                                }
                                break;
                        }
                    }
                }
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
            return xml_group_info;
        }

    }
}
