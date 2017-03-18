namespace PuTTY_Storm.Splash_Screens
{
    partial class StopScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StopScreen));
            this.puttystormLogo = new System.Windows.Forms.PictureBox();
            this.PuttyStormStopHeader = new System.Windows.Forms.Label();
            this.PuttyStormStopSessionLabel = new System.Windows.Forms.Label();
            this.puttyStormStopBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.puttystormLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // puttystormLogo
            // 
            this.puttystormLogo.Image = ((System.Drawing.Image)(resources.GetObject("puttystormLogo.Image")));
            this.puttystormLogo.Location = new System.Drawing.Point(40, 87);
            this.puttystormLogo.Name = "puttystormLogo";
            this.puttystormLogo.Size = new System.Drawing.Size(480, 188);
            this.puttystormLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.puttystormLogo.TabIndex = 1;
            this.puttystormLogo.TabStop = false;
            // 
            // PuttyStormStopHeader
            // 
            this.PuttyStormStopHeader.AutoSize = true;
            this.PuttyStormStopHeader.Font = new System.Drawing.Font("Calibri", 45F);
            this.PuttyStormStopHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PuttyStormStopHeader.Location = new System.Drawing.Point(173, 11);
            this.PuttyStormStopHeader.Name = "PuttyStormStopHeader";
            this.PuttyStormStopHeader.Size = new System.Drawing.Size(222, 73);
            this.PuttyStormStopHeader.TabIndex = 3;
            this.PuttyStormStopHeader.Text = "Bye Bye";
            // 
            // PuttyStormStopSessionLabel
            // 
            this.PuttyStormStopSessionLabel.AutoSize = true;
            this.PuttyStormStopSessionLabel.Font = new System.Drawing.Font("Calibri", 18F);
            this.PuttyStormStopSessionLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PuttyStormStopSessionLabel.Location = new System.Drawing.Point(194, 296);
            this.PuttyStormStopSessionLabel.Name = "PuttyStormStopSessionLabel";
            this.PuttyStormStopSessionLabel.Size = new System.Drawing.Size(180, 29);
            this.PuttyStormStopSessionLabel.TabIndex = 4;
            this.PuttyStormStopSessionLabel.Text = "Saving sessions...";
            // 
            // puttyStormStopBar
            // 
            this.puttyStormStopBar.Location = new System.Drawing.Point(0, 331);
            this.puttyStormStopBar.MarqueeAnimationSpeed = 50;
            this.puttyStormStopBar.Name = "puttyStormStopBar";
            this.puttyStormStopBar.Size = new System.Drawing.Size(576, 29);
            this.puttyStormStopBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.puttyStormStopBar.TabIndex = 5;
            // 
            // StopScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(576, 360);
            this.Controls.Add(this.puttyStormStopBar);
            this.Controls.Add(this.PuttyStormStopSessionLabel);
            this.Controls.Add(this.PuttyStormStopHeader);
            this.Controls.Add(this.puttystormLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(576, 360);
            this.MinimumSize = new System.Drawing.Size(576, 360);
            this.Name = "StopScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StopScreen";
            ((System.ComponentModel.ISupportInitialize)(this.puttystormLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox puttystormLogo;
        private System.Windows.Forms.Label PuttyStormStopHeader;
        private System.Windows.Forms.Label PuttyStormStopSessionLabel;
        private System.Windows.Forms.ProgressBar puttyStormStopBar;
    }
}