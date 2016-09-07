namespace RealtimeSpreadMonitor.Forms
{
    partial class OptionPayoffComparison
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionPayoffComparison));
            this.tabControlInstruments = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlInstruments
            // 
            this.tabControlInstruments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlInstruments.Location = new System.Drawing.Point(0, 0);
            this.tabControlInstruments.Name = "tabControlInstruments";
            this.tabControlInstruments.SelectedIndex = 0;
            this.tabControlInstruments.Size = new System.Drawing.Size(1019, 671);
            this.tabControlInstruments.TabIndex = 0;
            // 
            // OptionPayoffComparison
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 671);
            this.Controls.Add(this.tabControlInstruments);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionPayoffComparison";
            this.Text = "Option Payoff Comparison";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlInstruments;

    }
}