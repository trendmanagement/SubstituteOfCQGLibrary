namespace RealtimeSpreadMonitor.Forms
{
    partial class OptionPayoffComparisonUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.optionPLChartUserFormDifference = new RealtimeSpreadMonitor.Forms.OptionPLChartUserForm();
            this.optionPLChartUserFormModel = new RealtimeSpreadMonitor.Forms.OptionPLChartUserForm();
            this.optionPLChartUserFormFCM = new RealtimeSpreadMonitor.Forms.OptionPLChartUserForm();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.optionPLChartUserFormFCM);
            this.splitContainer1.Size = new System.Drawing.Size(1035, 710);
            this.splitContainer1.SplitterDistance = 470;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.optionPLChartUserFormDifference);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.optionPLChartUserFormModel);
            this.splitContainer2.Size = new System.Drawing.Size(1035, 470);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 1;
            // 
            // optionPLChartUserFormDifference
            // 
            this.optionPLChartUserFormDifference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPLChartUserFormDifference.Location = new System.Drawing.Point(0, 0);
            this.optionPLChartUserFormDifference.Name = "optionPLChartUserFormDifference";
            this.optionPLChartUserFormDifference.Size = new System.Drawing.Size(1035, 235);
            this.optionPLChartUserFormDifference.TabIndex = 0;
            // 
            // optionPLChartUserFormModel
            // 
            this.optionPLChartUserFormModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPLChartUserFormModel.Location = new System.Drawing.Point(0, 0);
            this.optionPLChartUserFormModel.Name = "optionPLChartUserFormModel";
            this.optionPLChartUserFormModel.Size = new System.Drawing.Size(1035, 231);
            this.optionPLChartUserFormModel.TabIndex = 0;
            // 
            // optionPLChartUserFormFCM
            // 
            this.optionPLChartUserFormFCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionPLChartUserFormFCM.Location = new System.Drawing.Point(0, 0);
            this.optionPLChartUserFormFCM.Name = "optionPLChartUserFormFCM";
            this.optionPLChartUserFormFCM.Size = new System.Drawing.Size(1035, 236);
            this.optionPLChartUserFormFCM.TabIndex = 1;
            // 
            // OptionPayoffComparisonUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "OptionPayoffComparisonUserControl";
            this.Size = new System.Drawing.Size(1035, 710);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        internal OptionPLChartUserForm optionPLChartUserFormDifference;
        internal OptionPLChartUserForm optionPLChartUserFormModel;
        internal OptionPLChartUserForm optionPLChartUserFormFCM;
    }
}
