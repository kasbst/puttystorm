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

using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.Generic;
using Renci.SshNet.Common;

namespace PuTTY_Storm
{
    public partial class SFTPManager : FormHelper
    {
        String hostname = null;
        String username = null;
        String password = null;
        String PrivateKey = null;
        String KeyPassphrase = null;
        ConnectionInfo con = null;

        public SFTPManager(String _hostname, String _username, String _password, String _privatekey, String _pk_pwd)
        {
            InitializeComponent();

            if (DPIAwareScaling.UsingWindows7ClassicTheme())
            {
                progressBar1.BackColor = SystemColors.Control;
                progressBar1.ForeColor = Color.Green;

                progressBar2.BackColor = SystemColors.Control;
                progressBar2.ForeColor = Color.Green;
            }

            this.hostname = _hostname;
            this.username = _username;
            this.password = _password;
            this.PrivateKey = _privatekey;
            this.KeyPassphrase = _pk_pwd;

            // Use KeyboardInteractiveAuthentication or PasswordAuthenticationMethod
            if (this.password != null && this.password != "" && !Regex.IsMatch(this.password, @"\s+") && this.PrivateKey == null)
            {
                Console.WriteLine("## SFTP Manager using password for login");

                KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(this.username);
                keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                con = new ConnectionInfo(this.hostname, 22, this.username, new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod(this.username, this.password),
                    keybAuth
                });
            }
            // Otherwise we have setup PrivateKeyAuthenticationMethod
            else if (this.password == null && this.PrivateKey != null)
            {
                Console.WriteLine("## SFTP Manager using OpenSSH private key for login");

                PrivateKeyFile keyFile;
                if (this.KeyPassphrase == null)
                {
                    Console.WriteLine("## OpenSSH private key is not encrypted");
                    keyFile = new PrivateKeyFile(this.PrivateKey);
                } else
                {
                    Console.WriteLine("## OpenSSH private key IS encrypted!");
                    keyFile = new PrivateKeyFile(this.PrivateKey, this.KeyPassphrase);
                }

                var keyFiles = new[] { keyFile };
                con = new ConnectionInfo(this.hostname, 22, this.username, new PrivateKeyAuthenticationMethod(this.username, keyFiles));
            }
        }

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

            downloadBtn.Enabled = false;
        }

        private void __DOWNLOAD_THREAD()
        {
            Modify_progressBar1(progressBar1, 0);
            Modify_DownloadStatusTextBox(DownloadStatusTextBox, "");

            Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Initializing..." + System.Environment.NewLine);

            try
            {
                using (SftpClient client = new SftpClient(con))
                {
                    client.Connect();
                    Console.WriteLine("Connected to the server!");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Connected to the server " + this.hostname + "." + System.Environment.NewLine);

                    foreach (string remoteFileName in RemoteFileTextbox.Lines)
                    {
                        DOWNLOAD(client, remoteFileName);
                    }

                    if (sftpAsyncr.IsDownloadCanceled)
                    {
                        Modify_progressBar1(progressBar1, 0);
                        Modify_DownloadLabel(DownloadLabel, "Status: Canceled");
                        Modify_downloadBtnStatus(downloadBtn);                                              
                        sftpAsyncr = null;
                    } else
                    {
                        Modify_progressBar1(progressBar1, 0);
                        Modify_DownloadLabel(DownloadLabel, "Status: Completed");
                        Modify_downloadBtnStatus(downloadBtn);
                        sftpAsyncr = null;
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

        private void Modify_downloadBtnStatus(Button downloadBtn)
        {
            if (downloadBtn.InvokeRequired)
            {
                downloadBtn.Invoke(new Action<Button>(Modify_downloadBtnStatus), downloadBtn);
                return;
            }
            downloadBtn.Enabled = true;
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

                string name = remoteFileName;
                int pos = name.LastIndexOf("/") + 1;
                String localFileName = DownloadPathTextbox.Text + "/" + name.Substring(pos, name.Length - pos);

                using (FileStream fs = new FileStream(localFileName, FileMode.Create, FileAccess.Write))
                {
                    Console.WriteLine("Begin Async Download!");
                    Modify_DownloadStatusTextBox(DownloadStatusTextBox, "Downloading file " + remoteFileName + " ..." + System.Environment.NewLine);

                    IAsyncResult asyncr = client.BeginDownloadFile(remoteFileName, fs);
                    sftpAsyncr = (SftpDownloadAsyncResult)asyncr;


                    while (!sftpAsyncr.IsCompleted && !sftpAsyncr.IsDownloadCanceled)
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

                    fs.Close();
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

            UploadBtn.Enabled = false;
        }

        private void ___UPLOAD_THREAD ()
        {
            Modify_progressBar2(progressBar2, 0);
            Modify_UploadStatusTextBox(UploadStatusTextBox, "");
            Modify_UploadStatusTextBox(UploadStatusTextBox, "Initializing..." + System.Environment.NewLine);

            try
            {
                using (SftpClient client = new SftpClient(con))
                {
                    client.Connect();
                    Console.WriteLine("Connected to the server!");
                    Modify_UploadStatusTextBox(UploadStatusTextBox, "Connected to the server " + this.hostname + "." + System.Environment.NewLine);

                    foreach (string localFileName in SelectedFileTextbox.Lines)
                    {
                        UPLOAD(client, localFileName);                       
                    }

                    if (sftpAsyncrUpload.IsUploadCanceled)
                    {                       
                        Modify_progressBar2(progressBar2, 0);
                        Modify_UploadLabel(UploadLabel, "Status: Canceled");
                        Modify_UploadBtnStatus(UploadBtn);                        
                        sftpAsyncrUpload = null;
                    } else
                    {
                        Modify_progressBar2(progressBar2, 0);
                        Modify_UploadLabel(UploadLabel, "Status: Completed");
                        Modify_UploadBtnStatus(UploadBtn);
                        sftpAsyncrUpload = null;
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

        private void Modify_UploadBtnStatus(Button UploadBtn)
        {
            if (UploadBtn.InvokeRequired)
            {
                UploadBtn.Invoke(new Action<Button>(Modify_UploadBtnStatus), UploadBtn);
                return;
            }
            UploadBtn.Enabled = true;
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

                    while (!sftpAsyncrUpload.IsCompleted && !sftpAsyncrUpload.IsUploadCanceled)
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
            RemoteFilesForm.Size = DPIAwareScaling.ScaleSize(800, 600);
            RemoteFilesForm.MinimumSize = DPIAwareScaling.ScaleSize(800, 600);
            RemoteFilesForm.MaximumSize = DPIAwareScaling.ScaleSize(800, 600);
            RemoteFilesForm.MaximizeBox = false;
            RemoteFilesForm.Text = GlobalVar.VERSION + " - RemoteFiles " + hostname;
            RemoteFilesForm.StartPosition = FormStartPosition.CenterScreen;

            RemoteFilesListView = new ListView();
            RemoteFilesListView.Size = DPIAwareScaling.ScaleSize(785, 500);
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
            SelectMultipleFilesButton.Size = DPIAwareScaling.ScaleSize(70, 30);
            SelectMultipleFilesButton.Location = DPIAwareScaling.ScalePoint(690, 515);
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
            try
            {
                using (SftpClient client = new SftpClient(con))
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

        private void STFPManagerFormCLosing(object sender, FormClosingEventArgs e)
        {
            // When closing SFTP Manager form cancel the download and upload if running in parallel!!!
            if (!downloadBtn.Enabled && !UploadBtn.Enabled)
            {
                MessageBox.Show("Unable to close SFTP Manager - Download and Upload in progress! Cancel both operations first!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                e.Cancel = true;
                return;
            }

            // When closing SFTP Manager form cancel the download if running!!!
            if (!downloadBtn.Enabled)
            {
                MessageBox.Show("Unable to close SFTP Manager - Download in progress! Cancel the download first!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                e.Cancel = true;
                return;
            }

            // When closing SFTP Manager form cancel the upload if running!!!
            if (!UploadBtn.Enabled)
            {
                MessageBox.Show("Unable to close SFTP Manager - Upload in progress! Cancel the upload first!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                e.Cancel = true;
                return;
            }
        }


    }
}
