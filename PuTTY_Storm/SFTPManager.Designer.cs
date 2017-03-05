namespace PuTTY_Storm
{
    partial class SFTPManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFTPManager));
            this.downloadBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.DownloadLabel = new System.Windows.Forms.Label();
            this.CancelDownloadBtn = new System.Windows.Forms.Button();
            this.UploadBtn = new System.Windows.Forms.Button();
            this.CancelUploadBtn = new System.Windows.Forms.Button();
            this.UploadLabel = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.DownloadStatusTextBox = new System.Windows.Forms.TextBox();
            this.UploadStatusTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.UploadBytesLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectedFileTextbox = new System.Windows.Forms.TextBox();
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.UploadFileLabel = new System.Windows.Forms.Label();
            this.DownloadBytesLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GettingTheListLabel = new System.Windows.Forms.Label();
            this.SelectFilesToDownload = new System.Windows.Forms.Button();
            this.DownloadPathTextbox = new System.Windows.Forms.TextBox();
            this.SelectFolderButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.RemoteFileTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // downloadBtn
            // 
            this.downloadBtn.Location = new System.Drawing.Point(24, 261);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(75, 23);
            this.downloadBtn.TabIndex = 0;
            this.downloadBtn.Text = "Download";
            this.downloadBtn.UseVisualStyleBackColor = true;
            this.downloadBtn.Click += new System.EventHandler(this.downloadBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(26, 324);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(443, 19);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // DownloadLabel
            // 
            this.DownloadLabel.AutoSize = true;
            this.DownloadLabel.Font = new System.Drawing.Font("Calibri", 13F);
            this.DownloadLabel.ForeColor = System.Drawing.Color.White;
            this.DownloadLabel.Location = new System.Drawing.Point(22, 295);
            this.DownloadLabel.Name = "DownloadLabel";
            this.DownloadLabel.Size = new System.Drawing.Size(60, 22);
            this.DownloadLabel.TabIndex = 2;
            this.DownloadLabel.Text = "Status:";
            // 
            // CancelDownloadBtn
            // 
            this.CancelDownloadBtn.Location = new System.Drawing.Point(117, 260);
            this.CancelDownloadBtn.Name = "CancelDownloadBtn";
            this.CancelDownloadBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelDownloadBtn.TabIndex = 4;
            this.CancelDownloadBtn.Text = "Cancel";
            this.CancelDownloadBtn.UseVisualStyleBackColor = true;
            this.CancelDownloadBtn.Click += new System.EventHandler(this.CancelDownloadBtn_Click);
            // 
            // UploadBtn
            // 
            this.UploadBtn.Location = new System.Drawing.Point(14, 178);
            this.UploadBtn.Name = "UploadBtn";
            this.UploadBtn.Size = new System.Drawing.Size(75, 23);
            this.UploadBtn.TabIndex = 5;
            this.UploadBtn.Text = "Upload";
            this.UploadBtn.UseVisualStyleBackColor = true;
            this.UploadBtn.Click += new System.EventHandler(this.UploadBtn_Click);
            // 
            // CancelUploadBtn
            // 
            this.CancelUploadBtn.Location = new System.Drawing.Point(107, 178);
            this.CancelUploadBtn.Name = "CancelUploadBtn";
            this.CancelUploadBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelUploadBtn.TabIndex = 6;
            this.CancelUploadBtn.Text = "Cancel";
            this.CancelUploadBtn.UseVisualStyleBackColor = true;
            this.CancelUploadBtn.Click += new System.EventHandler(this.CancelUploadBtn_Click);
            // 
            // UploadLabel
            // 
            this.UploadLabel.AutoSize = true;
            this.UploadLabel.Font = new System.Drawing.Font("Calibri", 13F);
            this.UploadLabel.ForeColor = System.Drawing.Color.White;
            this.UploadLabel.Location = new System.Drawing.Point(11, 211);
            this.UploadLabel.Name = "UploadLabel";
            this.UploadLabel.Size = new System.Drawing.Size(60, 22);
            this.UploadLabel.TabIndex = 7;
            this.UploadLabel.Text = "Status:";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(14, 239);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(443, 19);
            this.progressBar2.Step = 1;
            this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar2.TabIndex = 8;
            // 
            // DownloadStatusTextBox
            // 
            this.DownloadStatusTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.DownloadStatusTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadStatusTextBox.Location = new System.Drawing.Point(26, 349);
            this.DownloadStatusTextBox.Multiline = true;
            this.DownloadStatusTextBox.Name = "DownloadStatusTextBox";
            this.DownloadStatusTextBox.ReadOnly = true;
            this.DownloadStatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DownloadStatusTextBox.Size = new System.Drawing.Size(440, 187);
            this.DownloadStatusTextBox.TabIndex = 10;
            // 
            // UploadStatusTextBox
            // 
            this.UploadStatusTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.UploadStatusTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UploadStatusTextBox.Location = new System.Drawing.Point(14, 264);
            this.UploadStatusTextBox.Multiline = true;
            this.UploadStatusTextBox.Name = "UploadStatusTextBox";
            this.UploadStatusTextBox.ReadOnly = true;
            this.UploadStatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UploadStatusTextBox.Size = new System.Drawing.Size(440, 187);
            this.UploadStatusTextBox.TabIndex = 12;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.SlateGray;
            this.splitContainer1.Panel1.Controls.Add(this.UploadBytesLabel);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.SelectedFileTextbox);
            this.splitContainer1.Panel1.Controls.Add(this.SelectFileButton);
            this.splitContainer1.Panel1.Controls.Add(this.UploadFileLabel);
            this.splitContainer1.Panel1.Controls.Add(this.UploadBtn);
            this.splitContainer1.Panel1.Controls.Add(this.UploadStatusTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.CancelUploadBtn);
            this.splitContainer1.Panel1.Controls.Add(this.UploadLabel);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.SlateGray;
            this.splitContainer1.Panel2.Controls.Add(this.DownloadBytesLabel);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.GettingTheListLabel);
            this.splitContainer1.Panel2.Controls.Add(this.SelectFilesToDownload);
            this.splitContainer1.Panel2.Controls.Add(this.DownloadPathTextbox);
            this.splitContainer1.Panel2.Controls.Add(this.SelectFolderButton);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.RemoteFileTextbox);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.downloadBtn);
            this.splitContainer1.Panel2.Controls.Add(this.DownloadStatusTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.CancelDownloadBtn);
            this.splitContainer1.Panel2.Controls.Add(this.DownloadLabel);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 649);
            this.splitContainer1.SplitterDistance = 525;
            this.splitContainer1.TabIndex = 13;
            // 
            // UploadBytesLabel
            // 
            this.UploadBytesLabel.Font = new System.Drawing.Font("Courier New", 11F);
            this.UploadBytesLabel.ForeColor = System.Drawing.Color.White;
            this.UploadBytesLabel.Location = new System.Drawing.Point(179, 213);
            this.UploadBytesLabel.Name = "UploadBytesLabel";
            this.UploadBytesLabel.Size = new System.Drawing.Size(284, 23);
            this.UploadBytesLabel.TabIndex = 17;
            this.UploadBytesLabel.Text = "Bytes:";
            this.UploadBytesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 41);
            this.label3.TabIndex = 16;
            this.label3.Text = "Upload";
            // 
            // SelectedFileTextbox
            // 
            this.SelectedFileTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.SelectedFileTextbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedFileTextbox.Location = new System.Drawing.Point(14, 99);
            this.SelectedFileTextbox.Multiline = true;
            this.SelectedFileTextbox.Name = "SelectedFileTextbox";
            this.SelectedFileTextbox.ReadOnly = true;
            this.SelectedFileTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SelectedFileTextbox.Size = new System.Drawing.Size(443, 59);
            this.SelectedFileTextbox.TabIndex = 15;
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(191, 69);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(75, 23);
            this.SelectFileButton.TabIndex = 14;
            this.SelectFileButton.Text = "Select";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // UploadFileLabel
            // 
            this.UploadFileLabel.AutoSize = true;
            this.UploadFileLabel.Font = new System.Drawing.Font("Calibri", 11F);
            this.UploadFileLabel.ForeColor = System.Drawing.Color.White;
            this.UploadFileLabel.Location = new System.Drawing.Point(11, 71);
            this.UploadFileLabel.Name = "UploadFileLabel";
            this.UploadFileLabel.Size = new System.Drawing.Size(179, 18);
            this.UploadFileLabel.TabIndex = 13;
            this.UploadFileLabel.Text = "Select Files to be uploaded:";
            // 
            // DownloadBytesLabel
            // 
            this.DownloadBytesLabel.Font = new System.Drawing.Font("Courier New", 11F);
            this.DownloadBytesLabel.ForeColor = System.Drawing.Color.White;
            this.DownloadBytesLabel.Location = new System.Drawing.Point(189, 294);
            this.DownloadBytesLabel.Name = "DownloadBytesLabel";
            this.DownloadBytesLabel.Size = new System.Drawing.Size(284, 23);
            this.DownloadBytesLabel.TabIndex = 19;
            this.DownloadBytesLabel.Text = "Bytes:";
            this.DownloadBytesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(15, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 41);
            this.label4.TabIndex = 18;
            this.label4.Text = "Download";
            // 
            // GettingTheListLabel
            // 
            this.GettingTheListLabel.AutoSize = true;
            this.GettingTheListLabel.Font = new System.Drawing.Font("Calibri", 9F);
            this.GettingTheListLabel.ForeColor = System.Drawing.Color.White;
            this.GettingTheListLabel.Location = new System.Drawing.Point(312, 74);
            this.GettingTheListLabel.Name = "GettingTheListLabel";
            this.GettingTheListLabel.Size = new System.Drawing.Size(167, 20);
            this.GettingTheListLabel.TabIndex = 17;
            this.GettingTheListLabel.Text = "Getting the list of remote files ...";
            this.GettingTheListLabel.UseCompatibleTextRendering = true;
            // 
            // SelectFilesToDownload
            // 
            this.SelectFilesToDownload.Location = new System.Drawing.Point(229, 69);
            this.SelectFilesToDownload.Name = "SelectFilesToDownload";
            this.SelectFilesToDownload.Size = new System.Drawing.Size(75, 23);
            this.SelectFilesToDownload.TabIndex = 16;
            this.SelectFilesToDownload.Text = "List Files";
            this.SelectFilesToDownload.UseVisualStyleBackColor = true;
            this.SelectFilesToDownload.Click += new System.EventHandler(this.SelectFilesToDownload_Click);
            // 
            // DownloadPathTextbox
            // 
            this.DownloadPathTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.DownloadPathTextbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadPathTextbox.Location = new System.Drawing.Point(23, 206);
            this.DownloadPathTextbox.Name = "DownloadPathTextbox";
            this.DownloadPathTextbox.ReadOnly = true;
            this.DownloadPathTextbox.Size = new System.Drawing.Size(443, 20);
            this.DownloadPathTextbox.TabIndex = 15;
            // 
            // SelectFolderButton
            // 
            this.SelectFolderButton.Location = new System.Drawing.Point(167, 174);
            this.SelectFolderButton.Name = "SelectFolderButton";
            this.SelectFolderButton.Size = new System.Drawing.Size(75, 23);
            this.SelectFolderButton.TabIndex = 14;
            this.SelectFolderButton.Text = "Select";
            this.SelectFolderButton.UseVisualStyleBackColor = true;
            this.SelectFolderButton.Click += new System.EventHandler(this.SelectFolderButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(21, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 18);
            this.label2.TabIndex = 13;
            this.label2.Text = "Download destination:";
            // 
            // RemoteFileTextbox
            // 
            this.RemoteFileTextbox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoteFileTextbox.Location = new System.Drawing.Point(22, 99);
            this.RemoteFileTextbox.Multiline = true;
            this.RemoteFileTextbox.Name = "RemoteFileTextbox";
            this.RemoteFileTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RemoteFileTextbox.Size = new System.Drawing.Size(443, 59);
            this.RemoteFileTextbox.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 11F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(19, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name of files to be downloaded:";
            // 
            // SFTPManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 649);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1100, 688);
            this.MinimumSize = new System.Drawing.Size(1100, 688);
            this.Name = "SFTPManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Putty Strom - SFTP Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.STFPManagerFormCLosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button downloadBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label DownloadLabel;
        private System.Windows.Forms.Button CancelDownloadBtn;
        private System.Windows.Forms.Button UploadBtn;
        private System.Windows.Forms.Button CancelUploadBtn;
        private System.Windows.Forms.Label UploadLabel;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.TextBox DownloadStatusTextBox;
        private System.Windows.Forms.TextBox UploadStatusTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox SelectedFileTextbox;
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.Label UploadFileLabel;
        private System.Windows.Forms.TextBox DownloadPathTextbox;
        private System.Windows.Forms.Button SelectFolderButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox RemoteFileTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectFilesToDownload;
        private System.Windows.Forms.Label GettingTheListLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label UploadBytesLabel;
        private System.Windows.Forms.Label DownloadBytesLabel;
    }
}

