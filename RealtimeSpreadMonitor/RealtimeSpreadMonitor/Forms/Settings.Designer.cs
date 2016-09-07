namespace RealtimeSpreadMonitor.Forms
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtnBidPriceRules = new System.Windows.Forms.RadioButton();
            this.radioBtnAskPriceRules = new System.Windows.Forms.RadioButton();
            this.radioBtnTheorPriceRules = new System.Windows.Forms.RadioButton();
            this.radioBtnMidPriceRules = new System.Windows.Forms.RadioButton();
            this.radioBtnDefaultPriceRules = new System.Windows.Forms.RadioButton();
            this.chkBoxEodSettlements = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWriteRealtimeStateToDB = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRealtimeUpdatedTo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.instrumentAcctList = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnGetEOD = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.eodConflictList = new System.Windows.Forms.ListView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.additionalFixFieldsList = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBtnBidPriceRules);
            this.groupBox1.Controls.Add(this.radioBtnAskPriceRules);
            this.groupBox1.Controls.Add(this.radioBtnTheorPriceRules);
            this.groupBox1.Controls.Add(this.radioBtnMidPriceRules);
            this.groupBox1.Controls.Add(this.radioBtnDefaultPriceRules);
            this.groupBox1.Location = new System.Drawing.Point(12, 141);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 132);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pricing Algorithm";
            // 
            // radioBtnBidPriceRules
            // 
            this.radioBtnBidPriceRules.AutoSize = true;
            this.radioBtnBidPriceRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnBidPriceRules.Location = new System.Drawing.Point(6, 84);
            this.radioBtnBidPriceRules.Name = "radioBtnBidPriceRules";
            this.radioBtnBidPriceRules.Size = new System.Drawing.Size(39, 17);
            this.radioBtnBidPriceRules.TabIndex = 7;
            this.radioBtnBidPriceRules.Text = "Bid";
            this.radioBtnBidPriceRules.UseVisualStyleBackColor = true;
            this.radioBtnBidPriceRules.CheckedChanged += new System.EventHandler(this.radioBtnBidPriceRules_CheckedChanged);
            // 
            // radioBtnAskPriceRules
            // 
            this.radioBtnAskPriceRules.AutoSize = true;
            this.radioBtnAskPriceRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnAskPriceRules.Location = new System.Drawing.Point(6, 38);
            this.radioBtnAskPriceRules.Name = "radioBtnAskPriceRules";
            this.radioBtnAskPriceRules.Size = new System.Drawing.Size(42, 17);
            this.radioBtnAskPriceRules.TabIndex = 6;
            this.radioBtnAskPriceRules.Text = "Ask";
            this.radioBtnAskPriceRules.UseVisualStyleBackColor = true;
            this.radioBtnAskPriceRules.CheckedChanged += new System.EventHandler(this.radioBtnAskPriceRules_CheckedChanged);
            // 
            // radioBtnTheorPriceRules
            // 
            this.radioBtnTheorPriceRules.AutoSize = true;
            this.radioBtnTheorPriceRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnTheorPriceRules.Location = new System.Drawing.Point(6, 107);
            this.radioBtnTheorPriceRules.Name = "radioBtnTheorPriceRules";
            this.radioBtnTheorPriceRules.Size = new System.Drawing.Size(78, 17);
            this.radioBtnTheorPriceRules.TabIndex = 5;
            this.radioBtnTheorPriceRules.Text = "Theoretical";
            this.radioBtnTheorPriceRules.UseVisualStyleBackColor = true;
            this.radioBtnTheorPriceRules.CheckedChanged += new System.EventHandler(this.radioBtnTheorPriceRules_CheckedChanged);
            // 
            // radioBtnMidPriceRules
            // 
            this.radioBtnMidPriceRules.AutoSize = true;
            this.radioBtnMidPriceRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnMidPriceRules.Location = new System.Drawing.Point(6, 61);
            this.radioBtnMidPriceRules.Name = "radioBtnMidPriceRules";
            this.radioBtnMidPriceRules.Size = new System.Drawing.Size(80, 17);
            this.radioBtnMidPriceRules.TabIndex = 4;
            this.radioBtnMidPriceRules.Text = "Mid/Bid/Ask";
            this.radioBtnMidPriceRules.UseVisualStyleBackColor = true;
            this.radioBtnMidPriceRules.CheckedChanged += new System.EventHandler(this.radioBtnMidPriceRules_CheckedChanged);
            // 
            // radioBtnDefaultPriceRules
            // 
            this.radioBtnDefaultPriceRules.AutoSize = true;
            this.radioBtnDefaultPriceRules.Checked = true;
            this.radioBtnDefaultPriceRules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnDefaultPriceRules.Location = new System.Drawing.Point(6, 15);
            this.radioBtnDefaultPriceRules.Name = "radioBtnDefaultPriceRules";
            this.radioBtnDefaultPriceRules.Size = new System.Drawing.Size(60, 17);
            this.radioBtnDefaultPriceRules.TabIndex = 3;
            this.radioBtnDefaultPriceRules.TabStop = true;
            this.radioBtnDefaultPriceRules.Text = "Default";
            this.radioBtnDefaultPriceRules.UseVisualStyleBackColor = true;
            this.radioBtnDefaultPriceRules.CheckedChanged += new System.EventHandler(this.radioBtnDefaultPriceRules_CheckedChanged);
            // 
            // chkBoxEodSettlements
            // 
            this.chkBoxEodSettlements.AutoSize = true;
            this.chkBoxEodSettlements.Location = new System.Drawing.Point(6, 19);
            this.chkBoxEodSettlements.Name = "chkBoxEodSettlements";
            this.chkBoxEodSettlements.Size = new System.Drawing.Size(120, 17);
            this.chkBoxEodSettlements.TabIndex = 1;
            this.chkBoxEodSettlements.Text = "End of Day Analysis";
            this.chkBoxEodSettlements.UseVisualStyleBackColor = true;
            this.chkBoxEodSettlements.CheckedChanged += new System.EventHandler(this.chkBoxEodSettlements_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(12, 318);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnWriteRealtimeStateToDB
            // 
            this.btnWriteRealtimeStateToDB.Enabled = false;
            this.btnWriteRealtimeStateToDB.Location = new System.Drawing.Point(6, 77);
            this.btnWriteRealtimeStateToDB.Name = "btnWriteRealtimeStateToDB";
            this.btnWriteRealtimeStateToDB.Size = new System.Drawing.Size(88, 24);
            this.btnWriteRealtimeStateToDB.TabIndex = 3;
            this.btnWriteRealtimeStateToDB.Text = "Write";
            this.btnWriteRealtimeStateToDB.UseVisualStyleBackColor = true;
            this.btnWriteRealtimeStateToDB.Click += new System.EventHandler(this.btnWriteRealtimeStateToDB_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblRealtimeUpdatedTo);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkBoxEodSettlements);
            this.groupBox2.Controls.Add(this.btnWriteRealtimeStateToDB);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 132);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "End Of Day Control";
            // 
            // lblRealtimeUpdatedTo
            // 
            this.lblRealtimeUpdatedTo.AutoSize = true;
            this.lblRealtimeUpdatedTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRealtimeUpdatedTo.Location = new System.Drawing.Point(6, 104);
            this.lblRealtimeUpdatedTo.Name = "lblRealtimeUpdatedTo";
            this.lblRealtimeUpdatedTo.Size = new System.Drawing.Size(0, 12);
            this.lblRealtimeUpdatedTo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 27);
            this.label1.TabIndex = 4;
            this.label1.Text = "Write Realtime State to Database";
            // 
            // instrumentAcctList
            // 
            this.instrumentAcctList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instrumentAcctList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instrumentAcctList.FullRowSelect = true;
            this.instrumentAcctList.GridLines = true;
            this.instrumentAcctList.Location = new System.Drawing.Point(3, 3);
            this.instrumentAcctList.Name = "instrumentAcctList";
            this.instrumentAcctList.Size = new System.Drawing.Size(665, 313);
            this.instrumentAcctList.TabIndex = 10;
            this.instrumentAcctList.UseCompatibleStateImageBehavior = false;
            this.instrumentAcctList.View = System.Windows.Forms.View.Details;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnGetEOD);
            this.splitContainer1.Panel1.Controls.Add(this.btnClose);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(829, 345);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 11;
            // 
            // btnGetEOD
            // 
            this.btnGetEOD.Location = new System.Drawing.Point(12, 288);
            this.btnGetEOD.Name = "btnGetEOD";
            this.btnGetEOD.Size = new System.Drawing.Size(126, 24);
            this.btnGetEOD.TabIndex = 8;
            this.btnGetEOD.Text = "Refresh EOD Conflict";
            this.btnGetEOD.UseVisualStyleBackColor = true;
            this.btnGetEOD.Click += new System.EventHandler(this.btnGetEOD_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(679, 345);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.eodConflictList);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(671, 319);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "End Of Day / Realtime Conflicts";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // eodConflictList
            // 
            this.eodConflictList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eodConflictList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eodConflictList.FullRowSelect = true;
            this.eodConflictList.GridLines = true;
            this.eodConflictList.Location = new System.Drawing.Point(0, 0);
            this.eodConflictList.Name = "eodConflictList";
            this.eodConflictList.Size = new System.Drawing.Size(671, 319);
            this.eodConflictList.TabIndex = 11;
            this.eodConflictList.UseCompatibleStateImageBehavior = false;
            this.eodConflictList.View = System.Windows.Forms.View.Details;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.instrumentAcctList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(671, 319);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Account Allocation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.additionalFixFieldsList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(671, 319);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Additional FIX Fields";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // additionalFixFieldsList
            // 
            this.additionalFixFieldsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalFixFieldsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.additionalFixFieldsList.FullRowSelect = true;
            this.additionalFixFieldsList.GridLines = true;
            this.additionalFixFieldsList.Location = new System.Drawing.Point(3, 3);
            this.additionalFixFieldsList.Name = "additionalFixFieldsList";
            this.additionalFixFieldsList.Size = new System.Drawing.Size(665, 313);
            this.additionalFixFieldsList.TabIndex = 11;
            this.additionalFixFieldsList.UseCompatibleStateImageBehavior = false;
            this.additionalFixFieldsList.View = System.Windows.Forms.View.Details;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 345);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Price Settings and End Of Day";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBtnTheorPriceRules;
        private System.Windows.Forms.RadioButton radioBtnMidPriceRules;
        private System.Windows.Forms.RadioButton radioBtnDefaultPriceRules;
        private System.Windows.Forms.CheckBox chkBoxEodSettlements;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RadioButton radioBtnBidPriceRules;
        private System.Windows.Forms.RadioButton radioBtnAskPriceRules;
        private System.Windows.Forms.Button btnWriteRealtimeStateToDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRealtimeUpdatedTo;
        private System.Windows.Forms.ListView instrumentAcctList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView additionalFixFieldsList;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGetEOD;
        private System.Windows.Forms.ListView eodConflictList;
    }
}