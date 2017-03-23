namespace PuTTY_Storm
{
    partial class KotarakMainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KotarakMainForm));
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.RunCodeButton = new System.Windows.Forms.Button();
            this.SelectAllDevicesButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.UnselectAllDevicesButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.SelectGroupSubGroupButton = new System.Windows.Forms.Button();
            this.SelectGroupSubGroupCombobox = new System.Windows.Forms.ComboBox();
            this.SelectGroupSubGroupLabel = new System.Windows.Forms.Label();
            this.LoginLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.ForceAccountCheckBox = new System.Windows.Forms.CheckBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.KotarakAltLabel = new System.Windows.Forms.Label();
            this.KotarakHeadLabel = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.CommandRadioButton = new System.Windows.Forms.RadioButton();
            this.BashScriptRadioButton = new System.Windows.Forms.RadioButton();
            this.IndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.HostnameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReturnCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(981, 452);
            this.scintilla1.TabIndex = 1;
            this.scintilla1.StyleNeeded += new System.EventHandler<ScintillaNET.StyleNeededEventArgs>(this.scintilla_StyleNeeded);
            // 
            // ExportButton
            // 
            this.ExportButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportButton.Location = new System.Drawing.Point(95, 179);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(75, 23);
            this.ExportButton.TabIndex = 7;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportButton.Location = new System.Drawing.Point(95, 150);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(75, 23);
            this.ImportButton.TabIndex = 6;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // RunCodeButton
            // 
            this.RunCodeButton.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunCodeButton.Location = new System.Drawing.Point(95, 86);
            this.RunCodeButton.Name = "RunCodeButton";
            this.RunCodeButton.Size = new System.Drawing.Size(75, 41);
            this.RunCodeButton.TabIndex = 5;
            this.RunCodeButton.Text = "Run";
            this.RunCodeButton.UseVisualStyleBackColor = true;
            this.RunCodeButton.Click += new System.EventHandler(this.RunCodeButton_Click);
            // 
            // SelectAllDevicesButton
            // 
            this.SelectAllDevicesButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectAllDevicesButton.Location = new System.Drawing.Point(23, 86);
            this.SelectAllDevicesButton.Name = "SelectAllDevicesButton";
            this.SelectAllDevicesButton.Size = new System.Drawing.Size(82, 23);
            this.SelectAllDevicesButton.TabIndex = 4;
            this.SelectAllDevicesButton.Text = "Select all";
            this.SelectAllDevicesButton.UseVisualStyleBackColor = true;
            this.SelectAllDevicesButton.Click += new System.EventHandler(this.SelectAllDevicesButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.SlateGray;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexColumn,
            this.SelectColumn,
            this.HostnameColumn,
            this.ReturnCodeColumn,
            this.OutputColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.DividerHeight = 20;
            this.dataGridView1.RowTemplate.Height = 50000;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.Size = new System.Drawing.Size(980, 406);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.AutoSizeRowsModeChanged += new System.Windows.Forms.DataGridViewAutoSizeModeEventHandler(this.AutoSizeRowsMode);
            // 
            // UnselectAllDevicesButton
            // 
            this.UnselectAllDevicesButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnselectAllDevicesButton.Location = new System.Drawing.Point(23, 115);
            this.UnselectAllDevicesButton.Name = "UnselectAllDevicesButton";
            this.UnselectAllDevicesButton.Size = new System.Drawing.Size(82, 23);
            this.UnselectAllDevicesButton.TabIndex = 5;
            this.UnselectAllDevicesButton.Text = "Unselect all";
            this.UnselectAllDevicesButton.UseVisualStyleBackColor = true;
            this.UnselectAllDevicesButton.Click += new System.EventHandler(this.UnselectAllDevicesButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1264, 862);
            this.splitContainer1.SplitterDistance = 406;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.SelectGroupSubGroupButton);
            this.splitContainer3.Panel2.Controls.Add(this.SelectGroupSubGroupCombobox);
            this.splitContainer3.Panel2.Controls.Add(this.SelectGroupSubGroupLabel);
            this.splitContainer3.Panel2.Controls.Add(this.LoginLabel);
            this.splitContainer3.Panel2.Controls.Add(this.PasswordLabel);
            this.splitContainer3.Panel2.Controls.Add(this.ForceAccountCheckBox);
            this.splitContainer3.Panel2.Controls.Add(this.PasswordTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.LoginTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.KotarakAltLabel);
            this.splitContainer3.Panel2.Controls.Add(this.KotarakHeadLabel);
            this.splitContainer3.Panel2.Controls.Add(this.UnselectAllDevicesButton);
            this.splitContainer3.Panel2.Controls.Add(this.SelectAllDevicesButton);
            this.splitContainer3.Size = new System.Drawing.Size(1264, 406);
            this.splitContainer3.SplitterDistance = 980;
            this.splitContainer3.TabIndex = 0;
            // 
            // SelectGroupSubGroupButton
            // 
            this.SelectGroupSubGroupButton.Location = new System.Drawing.Point(163, 191);
            this.SelectGroupSubGroupButton.Name = "SelectGroupSubGroupButton";
            this.SelectGroupSubGroupButton.Size = new System.Drawing.Size(75, 23);
            this.SelectGroupSubGroupButton.TabIndex = 16;
            this.SelectGroupSubGroupButton.Text = "Select";
            this.SelectGroupSubGroupButton.UseVisualStyleBackColor = true;
            this.SelectGroupSubGroupButton.Click += new System.EventHandler(this.SelectGroupSubGroupButton_Click);
            // 
            // SelectGroupSubGroupCombobox
            // 
            this.SelectGroupSubGroupCombobox.FormattingEnabled = true;
            this.SelectGroupSubGroupCombobox.Location = new System.Drawing.Point(22, 192);
            this.SelectGroupSubGroupCombobox.Name = "SelectGroupSubGroupCombobox";
            this.SelectGroupSubGroupCombobox.Size = new System.Drawing.Size(121, 21);
            this.SelectGroupSubGroupCombobox.TabIndex = 15;
            // 
            // SelectGroupSubGroupLabel
            // 
            this.SelectGroupSubGroupLabel.AutoSize = true;
            this.SelectGroupSubGroupLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectGroupSubGroupLabel.ForeColor = System.Drawing.Color.White;
            this.SelectGroupSubGroupLabel.Location = new System.Drawing.Point(19, 171);
            this.SelectGroupSubGroupLabel.Name = "SelectGroupSubGroupLabel";
            this.SelectGroupSubGroupLabel.Size = new System.Drawing.Size(148, 15);
            this.SelectGroupSubGroupLabel.TabIndex = 14;
            this.SelectGroupSubGroupLabel.Text = "Select group or sub-group";
            // 
            // LoginLabel
            // 
            this.LoginLabel.AutoSize = true;
            this.LoginLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginLabel.ForeColor = System.Drawing.Color.White;
            this.LoginLabel.Location = new System.Drawing.Point(21, 289);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(36, 15);
            this.LoginLabel.TabIndex = 8;
            this.LoginLabel.Text = "Login";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.ForeColor = System.Drawing.Color.White;
            this.PasswordLabel.Location = new System.Drawing.Point(21, 313);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(61, 15);
            this.PasswordLabel.TabIndex = 9;
            this.PasswordLabel.Text = "Password";
            // 
            // ForceAccountCheckBox
            // 
            this.ForceAccountCheckBox.AutoSize = true;
            this.ForceAccountCheckBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForceAccountCheckBox.ForeColor = System.Drawing.Color.White;
            this.ForceAccountCheckBox.Location = new System.Drawing.Point(24, 258);
            this.ForceAccountCheckBox.Name = "ForceAccountCheckBox";
            this.ForceAccountCheckBox.Size = new System.Drawing.Size(143, 19);
            this.ForceAccountCheckBox.TabIndex = 7;
            this.ForceAccountCheckBox.Text = "Use different account";
            this.ForceAccountCheckBox.UseVisualStyleBackColor = true;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(86, 311);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(119, 20);
            this.PasswordTextBox.TabIndex = 11;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Location = new System.Drawing.Point(86, 285);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(119, 20);
            this.LoginTextBox.TabIndex = 10;
            // 
            // KotarakAltLabel
            // 
            this.KotarakAltLabel.AutoSize = true;
            this.KotarakAltLabel.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KotarakAltLabel.ForeColor = System.Drawing.Color.White;
            this.KotarakAltLabel.Location = new System.Drawing.Point(23, 48);
            this.KotarakAltLabel.Name = "KotarakAltLabel";
            this.KotarakAltLabel.Size = new System.Drawing.Size(100, 15);
            this.KotarakAltLabel.TabIndex = 13;
            this.KotarakAltLabel.Text = "Confmgmt plugin";
            // 
            // KotarakHeadLabel
            // 
            this.KotarakHeadLabel.AutoSize = true;
            this.KotarakHeadLabel.Font = new System.Drawing.Font("Calibri", 25F);
            this.KotarakHeadLabel.ForeColor = System.Drawing.Color.White;
            this.KotarakHeadLabel.Location = new System.Drawing.Point(17, 9);
            this.KotarakHeadLabel.Name = "KotarakHeadLabel";
            this.KotarakHeadLabel.Size = new System.Drawing.Size(122, 41);
            this.KotarakHeadLabel.TabIndex = 12;
            this.KotarakHeadLabel.Text = "Kotarak";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.scintilla1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.CommandRadioButton);
            this.splitContainer2.Panel2.Controls.Add(this.BashScriptRadioButton);
            this.splitContainer2.Panel2.Controls.Add(this.RunCodeButton);
            this.splitContainer2.Panel2.Controls.Add(this.ImportButton);
            this.splitContainer2.Panel2.Controls.Add(this.ExportButton);
            this.splitContainer2.Size = new System.Drawing.Size(1264, 452);
            this.splitContainer2.SplitterDistance = 981;
            this.splitContainer2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Code and Commands";
            // 
            // CommandRadioButton
            // 
            this.CommandRadioButton.AutoSize = true;
            this.CommandRadioButton.Font = new System.Drawing.Font("Calibri", 13F);
            this.CommandRadioButton.ForeColor = System.Drawing.Color.White;
            this.CommandRadioButton.Location = new System.Drawing.Point(19, 266);
            this.CommandRadioButton.Name = "CommandRadioButton";
            this.CommandRadioButton.Size = new System.Drawing.Size(179, 26);
            this.CommandRadioButton.TabIndex = 9;
            this.CommandRadioButton.TabStop = true;
            this.CommandRadioButton.Text = "Command execution";
            this.CommandRadioButton.UseVisualStyleBackColor = true;
            this.CommandRadioButton.CheckedChanged += new System.EventHandler(this.CommandRadioButton_CheckedChanged);
            // 
            // BashScriptRadioButton
            // 
            this.BashScriptRadioButton.AutoSize = true;
            this.BashScriptRadioButton.Font = new System.Drawing.Font("Calibri", 13F);
            this.BashScriptRadioButton.ForeColor = System.Drawing.Color.White;
            this.BashScriptRadioButton.Location = new System.Drawing.Point(19, 237);
            this.BashScriptRadioButton.Name = "BashScriptRadioButton";
            this.BashScriptRadioButton.Size = new System.Drawing.Size(107, 26);
            this.BashScriptRadioButton.TabIndex = 8;
            this.BashScriptRadioButton.TabStop = true;
            this.BashScriptRadioButton.Text = "Bash script";
            this.BashScriptRadioButton.UseVisualStyleBackColor = true;
            this.BashScriptRadioButton.CheckedChanged += new System.EventHandler(this.BashScriptRadioButton_CheckedChanged);
            // 
            // IndexColumn
            // 
            this.IndexColumn.HeaderText = "Index";
            this.IndexColumn.Name = "IndexColumn";
            this.IndexColumn.Width = 58;
            // 
            // SelectColumn
            // 
            this.SelectColumn.HeaderText = "Select";
            this.SelectColumn.Name = "SelectColumn";
            this.SelectColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SelectColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.SelectColumn.Width = 62;
            // 
            // HostnameColumn
            // 
            this.HostnameColumn.HeaderText = "Hostname";
            this.HostnameColumn.Name = "HostnameColumn";
            this.HostnameColumn.Width = 80;
            // 
            // ReturnCodeColumn
            // 
            this.ReturnCodeColumn.HeaderText = "Return Code";
            this.ReturnCodeColumn.Name = "ReturnCodeColumn";
            this.ReturnCodeColumn.Width = 92;
            // 
            // OutputColumn
            // 
            this.OutputColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OutputColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.OutputColumn.FillWeight = 60000F;
            this.OutputColumn.HeaderText = "Output";
            this.OutputColumn.Name = "OutputColumn";
            // 
            // KotarakMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(1264, 862);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "KotarakMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Kotarak_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button RunCodeButton;
        private System.Windows.Forms.Button SelectAllDevicesButton;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button UnselectAllDevicesButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label LoginLabel;
        private System.Windows.Forms.CheckBox ForceAccountCheckBox;
        private System.Windows.Forms.RadioButton CommandRadioButton;
        private System.Windows.Forms.RadioButton BashScriptRadioButton;
        private System.Windows.Forms.Label KotarakAltLabel;
        private System.Windows.Forms.Label KotarakHeadLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectGroupSubGroupButton;
        private System.Windows.Forms.ComboBox SelectGroupSubGroupCombobox;
        private System.Windows.Forms.Label SelectGroupSubGroupLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostnameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReturnCodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputColumn;
    }
}

