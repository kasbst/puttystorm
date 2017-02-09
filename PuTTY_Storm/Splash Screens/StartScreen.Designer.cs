namespace PuTTY_Storm.Splash_Screens
{
    partial class StartScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartScreen));
            this.puttystormLogo = new System.Windows.Forms.PictureBox();
            this.puttyStormLoadBar = new System.Windows.Forms.ProgressBar();
            this.PuttyStormLoadingHeader = new System.Windows.Forms.Label();
            this.PuttyStormLoadSessionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.puttystormLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // puttystormLogo
            // 
            this.puttystormLogo.Image = ((System.Drawing.Image)(resources.GetObject("puttystormLogo.Image")));
            this.puttystormLogo.Location = new System.Drawing.Point(39, 85);
            this.puttystormLogo.Name = "puttystormLogo";
            this.puttystormLogo.Size = new System.Drawing.Size(480, 188);
            this.puttystormLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.puttystormLogo.TabIndex = 0;
            this.puttystormLogo.TabStop = false;
            // 
            // puttyStormLoadBar
            // 
            this.puttyStormLoadBar.Location = new System.Drawing.Point(0, 331);
            this.puttyStormLoadBar.MarqueeAnimationSpeed = 50;
            this.puttyStormLoadBar.Name = "puttyStormLoadBar";
            this.puttyStormLoadBar.Size = new System.Drawing.Size(576, 29);
            this.puttyStormLoadBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.puttyStormLoadBar.TabIndex = 1;
            // 
            // PuttyStormLoadingHeader
            // 
            this.PuttyStormLoadingHeader.AutoSize = true;
            this.PuttyStormLoadingHeader.Font = new System.Drawing.Font("Calibri", 45F);
            this.PuttyStormLoadingHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PuttyStormLoadingHeader.Location = new System.Drawing.Point(113, 9);
            this.PuttyStormLoadingHeader.Name = "PuttyStormLoadingHeader";
            this.PuttyStormLoadingHeader.Size = new System.Drawing.Size(345, 73);
            this.PuttyStormLoadingHeader.TabIndex = 2;
            this.PuttyStormLoadingHeader.Text = "PuTTY Storm";
            // 
            // PuttyStormLoadSessionLabel
            // 
            this.PuttyStormLoadSessionLabel.AutoSize = true;
            this.PuttyStormLoadSessionLabel.Font = new System.Drawing.Font("Calibri", 18F);
            this.PuttyStormLoadSessionLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PuttyStormLoadSessionLabel.Location = new System.Drawing.Point(185, 297);
            this.PuttyStormLoadSessionLabel.Name = "PuttyStormLoadSessionLabel";
            this.PuttyStormLoadSessionLabel.Size = new System.Drawing.Size(194, 29);
            this.PuttyStormLoadSessionLabel.TabIndex = 3;
            this.PuttyStormLoadSessionLabel.Text = "Loading sessions...";
            // 
            // StartScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(576, 360);
            this.Controls.Add(this.PuttyStormLoadSessionLabel);
            this.Controls.Add(this.PuttyStormLoadingHeader);
            this.Controls.Add(this.puttyStormLoadBar);
            this.Controls.Add(this.puttystormLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(576, 360);
            this.MinimumSize = new System.Drawing.Size(576, 360);
            this.Name = "StartScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartScreen";
            this.Load += new System.EventHandler(this.StartScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.puttystormLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox puttystormLogo;
        private System.Windows.Forms.ProgressBar puttyStormLoadBar;
        private System.Windows.Forms.Label PuttyStormLoadingHeader;
        private System.Windows.Forms.Label PuttyStormLoadSessionLabel;
    }
}