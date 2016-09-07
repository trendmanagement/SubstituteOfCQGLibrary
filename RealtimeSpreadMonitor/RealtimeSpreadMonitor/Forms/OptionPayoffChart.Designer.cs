namespace RealtimeSpreadMonitor.Forms
{
    partial class OptionPayoffChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionPayoffChart));
            this.optionPLChartUserForm1 = new RealtimeSpreadMonitor.Forms.OptionPLChartUserForm();
            this.SuspendLayout();
            // 
            // optionPLChartUserForm1
            // 
            this.optionPLChartUserForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPLChartUserForm1.Location = new System.Drawing.Point(0, 0);
            this.optionPLChartUserForm1.Name = "optionPLChartUserForm1";
            this.optionPLChartUserForm1.Size = new System.Drawing.Size(1008, 690);
            this.optionPLChartUserForm1.TabIndex = 0;
            // 
            // OptionPayoffChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 690);
            this.Controls.Add(this.optionPLChartUserForm1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionPayoffChart";
            this.Text = "OptionPayoffChart";
            this.ResumeLayout(false);

        }

        #endregion

        internal OptionPLChartUserForm optionPLChartUserForm1;
    }
}