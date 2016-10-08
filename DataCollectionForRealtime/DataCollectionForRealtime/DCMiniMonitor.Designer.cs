namespace DataCollectionForRealtime
{
    partial class DCMiniMonitor
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
            this.MainFormCall = new System.Windows.Forms.Button();
            this.queriesNumLbl = new System.Windows.Forms.Label();
            this.queriesNumDsplLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MainFormCall
            // 
            this.MainFormCall.Location = new System.Drawing.Point(12, 11);
            this.MainFormCall.Name = "MainFormCall";
            this.MainFormCall.Size = new System.Drawing.Size(183, 23);
            this.MainFormCall.TabIndex = 0;
            this.MainFormCall.Text = "Main monitor";
            this.MainFormCall.UseVisualStyleBackColor = true;
            this.MainFormCall.Click += new System.EventHandler(this.MainFormCall_Click);
            // 
            // queriesNumLbl
            // 
            this.queriesNumLbl.AutoSize = true;
            this.queriesNumLbl.Location = new System.Drawing.Point(9, 130);
            this.queriesNumLbl.Name = "queriesNumLbl";
            this.queriesNumLbl.Size = new System.Drawing.Size(84, 13);
            this.queriesNumLbl.TabIndex = 1;
            this.queriesNumLbl.Text = "Queries amount:";
            // 
            // queriesNumDsplLbl
            // 
            this.queriesNumDsplLbl.AutoSize = true;
            this.queriesNumDsplLbl.Location = new System.Drawing.Point(105, 128);
            this.queriesNumDsplLbl.Name = "queriesNumDsplLbl";
            this.queriesNumDsplLbl.Size = new System.Drawing.Size(0, 13);
            this.queriesNumDsplLbl.TabIndex = 2;
            // 
            // DCMiniMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(207, 170);
            this.Controls.Add(this.queriesNumDsplLbl);
            this.Controls.Add(this.queriesNumLbl);
            this.Controls.Add(this.MainFormCall);
            this.Name = "DCMiniMonitor";
            this.Text = "DCMiniMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCMiniMonitor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MainFormCall;
        private System.Windows.Forms.Label queriesNumLbl;
        private System.Windows.Forms.Label queriesNumDsplLbl;
    }
}