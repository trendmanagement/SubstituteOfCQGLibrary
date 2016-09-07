namespace RealtimeSpreadMonitor.Forms
{
    partial class OptionStartupProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionStartupProgress));
            this.label1 = new System.Windows.Forms.Label();
            this.pgbOptionStartup = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Option Spread Intialization....";
            // 
            // pgbOptionStartup
            // 
            this.pgbOptionStartup.Location = new System.Drawing.Point(12, 37);
            this.pgbOptionStartup.Name = "pgbOptionStartup";
            this.pgbOptionStartup.Size = new System.Drawing.Size(164, 20);
            this.pgbOptionStartup.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pgbOptionStartup.TabIndex = 1;
            // 
            // OptionStartupProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(188, 70);
            this.ControlBox = false;
            this.Controls.Add(this.pgbOptionStartup);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionStartupProgress";
            this.Text = "Starting Up...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.OptionStartupProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pgbOptionStartup;

    }
}