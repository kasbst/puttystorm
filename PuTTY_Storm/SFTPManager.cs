﻿/*
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

using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuTTY_Storm
{
    public partial class SFTPManager : FormHelper
    {
        String hostname = null;
        String username = null;
        String password = null;

        public SFTPManager(String _hostname, String _username, String _password)
        {
            InitializeComponent();
            this.hostname = _hostname;
            this.username = _username;
            this.password = _password;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GettingTheListLabel.Hide();
            DownloadBytesLabel.Hide();
            UploadBytesLabel.Hide();
        }

        long fileSize;

        /// <summary>
        /// Download button click event - Handle Files downloading.
        /// </summary>
        SftpDownloadAsyncResult sftpAsyncr;

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            if (RemoteFileTextbox.Lines.Length == 0)
            {
                MessageBox.Show("Select files to be downloaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DownloadPathTextbox.Text == "")
            {
                MessageBox.Show("Download destination not set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new Thread(delegate ()
            {
                __DOWNLOAD_THREAD();
            }).Start();
        }

        private void __DOWNLOAD_THREAD()
        {
            var port = 22;

            Modify_progressBar1(progressBar1, 0);
            Modify_DownloadStatusTextBox(DownloadStatusTextBox, "");

            Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Initializing..." + System.Environment.NewLine);

            try
            {
                using (SftpClient client = new SftpClient(this.hostname, port, this.username, this.password))
                {
                    client.Connect();
                    Console.WriteLine("Connected to the server!");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Connected to the server " + this.hostname + "." + System.Environment.NewLine);

                    foreach (string remoteFileName in RemoteFileTextbox.Lines)
                    {
                        DOWNLOAD(client, remoteFileName);
                    }

                    Modify_progressBar1(progressBar1, 0);

                    if (sftpAsyncr.IsDownloadCanceled == true)
                    {
                        Modify_DownloadLabel(DownloadLabel, "Status: Canceled");
                    } else
                    {
                        Modify_DownloadLabel(DownloadLabel, "Status: Completed");
                    }

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Modify_DownloadStatusTextBox(TextBox DownloadStatusTextBox, String text)
        {
            if (DownloadStatusTextBox.InvokeRequired)
            {
                DownloadStatusTextBox.Invoke(new Action<TextBox, String>(Modify_DownloadStatusTextBox), DownloadStatusTextBox, text);
                return;
            }

            DownloadStatusTextBox.AppendText(text);
        }

        private void Modify_progressBar1(ProgressBar progressBar1, int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<ProgressBar, int>(Modify_progressBar1), progressBar1, value);
                return;
            }

            progressBar1.Value = value;
        }

        private void Modify_DownloadLabel(Label DownloadLabel, String text)
        {
            if (DownloadLabel.InvokeRequired)
            {
                DownloadLabel.Invoke(new Action<Label, String>(Modify_DownloadLabel), DownloadLabel, text);
                return;
            }

            DownloadLabel.Text = text;
        }

        private void Modify_DownloadBytesLabel(Label DownloadBytesLabel, ulong value)
        {
            if (DownloadBytesLabel.InvokeRequired)
            {
                DownloadBytesLabel.Invoke(new Action<Label, ulong>(Modify_DownloadBytesLabel), DownloadBytesLabel, value);
                return;
            }

            DownloadBytesLabel.Show();
            DownloadBytesLabel.Text = "Bytes: " + value;
        }

        /// <summary>
        /// Download method using SSH.NET SFTP implementation for async files downloading.
        /// </summary>
        private void DOWNLOAD (SftpClient client, String remoteFileName)
        {
            bool IsExists = client.Exists(remoteFileName);
            Modify_DownloadStatusTextBox(DownloadStatusTextBox, System.Environment.NewLine + "Checking if remote file " + remoteFileName + " exists..." + System.Environment.NewLine);

            if (IsExists)
            {
                Console.WriteLine("File exists... continue!");
                Modify_DownloadStatusTextBox(DownloadStatusTextBox, "File exists..." + System.Environment.NewLine);

                SftpFileAttributes att = client.GetAttributes(remoteFileName);
                fileSize = att.Size;
                Modify_DownloadStatusTextBox(DownloadStatusTextBox, "File size is: " + fileSize + "." + System.Environment.NewLine);

                Console.WriteLine("File size is: " + fileSize);

                using (MemoryStream ms = new MemoryStream())
                {
                    Console.WriteLine("Begin Async Download!");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Downloading file " + remoteFileName + " ..." + System.Environment.NewLine);

                    IAsyncResult asyncr = client.BeginDownloadFile(remoteFileName, ms);
                    sftpAsyncr = (SftpDownloadAsyncResult)asyncr;


                    while (!sftpAsyncr.IsCompleted)
                    {
                        int pct = Convert.ToInt32(((double)sftpAsyncr.DownloadedBytes / (double)fileSize) * 100);

                        double temp = (double)sftpAsyncr.DownloadedBytes / (double)fileSize;
                        Console.WriteLine("Downloaded Bytes: " + sftpAsyncr.DownloadedBytes);
                        Console.WriteLine("Downloaded: " + temp);
                        Console.WriteLine("File size is: " + fileSize);
                        Console.WriteLine(pct);
                        Modify_progressBar1(progressBar1, pct);
                        Modify_DownloadLabel(DownloadLabel, "Status: " + pct + " %");
                        Modify_DownloadBytesLabel(DownloadBytesLabel, sftpAsyncr.DownloadedBytes);
                    }
                    client.EndDownloadFile(asyncr);

                    string name = remoteFileName;
                    int pos = name.LastIndexOf("/") + 1;
                    String localFileName = DownloadPathTextbox.Text + "/" + name.Substring(pos, name.Length - pos);

                    FileStream fs = new FileStream(localFileName, FileMode.Create, FileAccess.Write);

                    ms.WriteTo(fs);
                    fs.Close();
                    ms.Close();
                }

                if (sftpAsyncr.IsDownloadCanceled)
                {
                    Console.WriteLine("File Download has been canceled!");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "File Download has been canceled!" + System.Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("The file " + remoteFileName + " has been successfully downloaded from the server");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "The file " + remoteFileName + " has been downloaded successfully!" + System.Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("The file " + remoteFileName + " does not exists on the server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cancel Download Button - Handles cancellation of async download operation.
        /// </summary>
        private void CancelDownloadBtn_Click(object sender, EventArgs e)
        {
            if (sftpAsyncr != null)
            {
                sftpAsyncr.IsDownloadCanceled = true;
            }
        }


        /// <summary>
        /// Upload button click event - Handles files uploading.
        /// </summary>
        long localFileSize;
        SftpUploadAsyncResult sftpAsyncrUpload;

        private void UploadBtn_Click(object sender, EventArgs e)
        {
            if (SelectedFileTextbox.Lines.Length == 0)
            {
                MessageBox.Show("Select files to be uploaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            new Thread(delegate ()
            {
                ___UPLOAD_THREAD();
            }).Start();
        }

        private void ___UPLOAD_THREAD ()
        {
            var port = 22;

            Modify_progressBar2(progressBar2, 0);
            Modify_UploadStatusTextBox(UploadStatusTextBox, "");
            Modify_UploadStatusTextBox(UploadStatusTextBox, "Initializing..." + System.Environment.NewLine);

            try
            {
                using (SftpClient client = new SftpClient(this.hostname, port, this.username, this.password))
                {
                    client.Connect();
                    Console.WriteLine("Connected to the server!");
                    Modify_UploadStatusTextBox(UploadStatusTextBox, "Connected to the server " + this.hostname + "." + System.Environment.NewLine);

                    foreach (string localFileName in SelectedFileTextbox.Lines)
                    {
                        UPLOAD(client, localFileName);                       
                    }

                    Modify_progressBar2(progressBar2, 0);

                    if (sftpAsyncrUpload.IsUploadCanceled == true)
                    {
                        Modify_UploadLabel(UploadLabel, "Status: Canceled");
                    } else
                    {
                        Modify_UploadLabel(UploadLabel, "Status: Completed");
                    }
                    
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Modify_UploadStatusTextBox(TextBox UploadStatusTextBox, String text)
        {
            if (UploadStatusTextBox.InvokeRequired)
            {
                UploadStatusTextBox.Invoke(new Action<TextBox, String>(Modify_UploadStatusTextBox), UploadStatusTextBox, text);
                return;
            }

            UploadStatusTextBox.AppendText(text);
        }

        private void Modify_progressBar2(ProgressBar progressBar2, int value)
        {
            if (progressBar2.InvokeRequired)
            {
                progressBar2.Invoke(new Action<ProgressBar, int>(Modify_progressBar2), progressBar2, value);
                return;
            }

            progressBar2.Value = value;
        }

        private void Modify_UploadLabel(Label UploadLabel, String text)
        {
            if (UploadLabel.InvokeRequired)
            {
                UploadLabel.Invoke(new Action<Label, String>(Modify_UploadLabel), UploadLabel, text);
                return;
            }

            UploadLabel.Text = text;
        }

        private void Modify_UploadBytesLabel(Label UploadBytesLabel, ulong value)
        {
            if (UploadBytesLabel.InvokeRequired)
            {
                UploadBytesLabel.Invoke(new Action<Label, ulong>(Modify_UploadBytesLabel), UploadBytesLabel, value);
                return;
            }

            UploadBytesLabel.Show();
            UploadBytesLabel.Text = "Bytes: " + value;
        }

        /// <summary>
        /// Upload method using SSH.NET SFTP implementation for async files uploading.
        /// </summary>
        private void UPLOAD(SftpClient client, String localFileName)
        {
            string name = localFileName;
            int pos = name.LastIndexOf(@"\") + 1;
            String remoteFileName = name.Substring(pos, name.Length - pos);

            Modify_UploadStatusTextBox(UploadStatusTextBox, System.Environment.NewLine + "Checking if local file " + localFileName + " exists..." + System.Environment.NewLine);

            if (File.Exists(localFileName))
            {
                Console.WriteLine("Local file exists, continue...");
                Modify_UploadStatusTextBox(UploadStatusTextBox, "File exists..." + System.Environment.NewLine);

                localFileSize = new FileInfo(localFileName).Length;
                Console.WriteLine("File size is: " + localFileSize);
                Modify_UploadStatusTextBox(UploadStatusTextBox, "File size is: " + localFileSize + "." + System.Environment.NewLine);

                using (Stream file = File.OpenRead(localFileName))
                {
                    Modify_UploadStatusTextBox(UploadStatusTextBox, "Uploading file " + localFileName + " ..." + System.Environment.NewLine);

                    Console.WriteLine("### CLIENT: " + client);
                    Console.WriteLine("## FILE: " + file);
                    IAsyncResult asyncr = client.BeginUploadFile(file, remoteFileName);
                    sftpAsyncrUpload = (SftpUploadAsyncResult)asyncr;

                    while (!sftpAsyncrUpload.IsCompleted)
                    {
                        int pct = Convert.ToInt32(((double)sftpAsyncrUpload.UploadedBytes / (double)localFileSize) * 100);

                        double temp = (double)sftpAsyncrUpload.UploadedBytes / (double)localFileSize;
                        Console.WriteLine("Uploaded Bytes: " + sftpAsyncrUpload.UploadedBytes);
                        Console.WriteLine("Uploaded: " + temp);
                        Console.WriteLine("File size is: " + localFileSize);
                        Console.WriteLine(pct);
                        Modify_progressBar2(progressBar2, pct);
                        Modify_UploadLabel(UploadLabel, "Status: " + pct + " %");
                        Modify_UploadBytesLabel(UploadBytesLabel, sftpAsyncrUpload.UploadedBytes);
                    }
                    client.EndUploadFile(asyncr);

                    file.Close();
                }

                if (sftpAsyncrUpload.IsUploadCanceled)
                {
                    Console.WriteLine("File Upload has been canceled!");
                    Modify_UploadStatusTextBox(UploadStatusTextBox, "File Upload has been canceled!" + System.Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("The file " + localFileName + " has been successfully uploaded successfully to the server!");
                    Modify_UploadStatusTextBox(UploadStatusTextBox, "The file " + localFileName + " has been uploaded successfully!" + System.Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("The file " + localFileName + " does not exists on this computer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cancel Upload Button - Handles cancellation of async upload operation.
        /// </summary>
        private void CancelUploadBtn_Click(object sender, EventArgs e)
        {
            if (sftpAsyncrUpload != null)
            {
                sftpAsyncrUpload.IsUploadCanceled = true;
            }
        }

        /// <summary>
        /// Select File button - Contains OpenFileDialog for selecting local files to be uploaded.
        /// </summary>
        private void SelectFileButton_Click(object sender, EventArgs e)
        {
            SelectedFileTextbox.Text = null;

            using (OpenFileDialog openfiledialog = new OpenFileDialog())
            {
                openfiledialog.Multiselect = true;

                if (openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = openfiledialog.FileNames;
                    foreach (string fileName in fileNames)
                    {
                        SelectedFileTextbox.AppendText(fileName + System.Environment.NewLine);
                    }
                    SelectedFileTextbox.Text = SelectedFileTextbox.Text.TrimEnd('\r', '\n');
                }
            }
        }

        /// <summary>
        /// Select folder button - Contains FolderBrowserDialog for selecting a folder where remote files
        /// should be downloaded from remote location.
        /// </summary>
        private void SelectFolderButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = fbd.SelectedPath;
                    DownloadPathTextbox.Text = selectedPath;
                }
            }
        }

        /// <summary>
        /// Opens a dedicated form with ListView which contains a list of files from
        /// remote home directory.
        /// </summary>
        ListView RemoteFilesListView;
        FormHelper RemoteFilesForm;

        private void SelectFilesToDownload_Click(object sender, EventArgs e)
        {
            GettingTheListLabel.Show();
            RemoteFileTextbox.Text = null;

            RemoteFilesForm = new FormHelper();
            RemoteFilesForm.Size = new Size(800, 600);
            RemoteFilesForm.MinimumSize = new Size(800, 600);
            RemoteFilesForm.MaximumSize = new Size(800, 600);
            RemoteFilesForm.MaximizeBox = false;
            RemoteFilesForm.Text = GlobalVar.VERSION + " - RemoteFiles " + hostname;
            RemoteFilesForm.StartPosition = FormStartPosition.CenterScreen;

            RemoteFilesListView = new ListView();
            RemoteFilesListView.Size = new Size(785, 500);
            RemoteFilesListView.View = View.Details;
            RemoteFilesListView.AllowColumnReorder = true;
            RemoteFilesListView.Sorting = SortOrder.Ascending;
            RemoteFilesListView.Font = new Font("courier new", 10);
            RemoteFilesListView.MouseDoubleClick += new MouseEventHandler(RemoteFilesListView_DoubleCLick);
            RemoteFilesListView.Columns.Add("Name", -2, HorizontalAlignment.Left);
            RemoteFilesListView.Columns.Add("Size", -2, HorizontalAlignment.Left);
            RemoteFilesListView.Columns.Add("Modified", -2, HorizontalAlignment.Left);

            Button SelectMultipleFilesButton = new Button();
            SelectMultipleFilesButton.Text = "Select Files";
            SelectMultipleFilesButton.Size = new Size(70, 30);
            SelectMultipleFilesButton.Location = new Point(690, 515);
            SelectMultipleFilesButton.Click += new EventHandler(SelectMultipleFilesButton_Click);
             
            ListRemoteFiles(RemoteFilesListView);
    
            RemoteFilesForm.Controls.Add(RemoteFilesListView);
            RemoteFilesForm.Controls.Add(SelectMultipleFilesButton);
            RemoteFilesForm.Show();
            GettingTheListLabel.Hide();
        }

        /// <summary>
        /// List files in remote home directory using SSH.NET
        /// </summary>
        private void ListRemoteFiles(ListView RemoteFilesListView)
        {
            var port = 22;

            try
            {
                using (SftpClient client = new SftpClient(this.hostname, port, this.username, this.password))
                {
                    client.Connect();
                    var files = client.ListDirectory("");
                    foreach (var file in files)
                    {
                        if (file.IsRegularFile)
                        {
                            var item1 = new ListViewItem(new[] { file.Name, file.Length.ToString(), file.LastWriteTime.ToString() });
                            RemoteFilesListView.Items.Add(item1);
                        }
                    }

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// When double-click on ListView item - put this item to the RemoteFileTextbox (this textbox
        /// contains list of remote files which should be downloaded).
        /// </summary>
        private void RemoteFilesListView_DoubleCLick(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem selectedItem in RemoteFilesListView.SelectedItems)
            {
                Console.WriteLine(selectedItem.Text);
                RemoteFileTextbox.AppendText(selectedItem.Text);
                RemoteFilesForm.Dispose();

            }
        }

        /// <summary>
        /// When selected multiple items in ListView item - put these items to the RemoteFileTextbox (this textbox
        /// contains list of remote files which should be downloaded).
        /// </summary>
        private void SelectMultipleFilesButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in RemoteFilesListView.SelectedItems)
            {
                Console.WriteLine(selectedItem.Text);
                RemoteFileTextbox.AppendText(selectedItem.Text + System.Environment.NewLine);                           
            }
            RemoteFileTextbox.Text = RemoteFileTextbox.Text.TrimEnd('\r', '\n');
            RemoteFilesForm.Dispose();
        }

    }
}