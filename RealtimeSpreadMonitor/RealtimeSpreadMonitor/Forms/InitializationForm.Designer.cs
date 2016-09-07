namespace RealtimeSpreadMonitor.Forms
{
    partial class InitializationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializationForm));
            this.cmbxDatabase = new System.Windows.Forms.ComboBox();
            this.cmbxPortfolio = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRunSystem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExitSystem = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modelDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxUseHalfDay = new System.Windows.Forms.CheckBox();
            this.transactionTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.decisionMinuteOffset = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.timeGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxCloudDB = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decisionMinuteOffset)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.timeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbxDatabase
            // 
            this.cmbxDatabase.FormattingEnabled = true;
            this.cmbxDatabase.Location = new System.Drawing.Point(101, 3);
            this.cmbxDatabase.Name = "cmbxDatabase";
            this.cmbxDatabase.Size = new System.Drawing.Size(191, 21);
            this.cmbxDatabase.TabIndex = 8;
            // 
            // cmbxPortfolio
            // 
            this.cmbxPortfolio.FormattingEnabled = true;
            this.cmbxPortfolio.Location = new System.Drawing.Point(101, 30);
            this.cmbxPortfolio.Name = "cmbxPortfolio";
            this.cmbxPortfolio.Size = new System.Drawing.Size(191, 21);
            this.cmbxPortfolio.TabIndex = 15;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRunSystem,
            this.toolStripSeparator1,
            this.btnExitSystem});
            this.toolStrip1.Location = new System.Drawing.Point(62, 315);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(156, 25);
            this.toolStrip1.TabIndex = 22;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRunSystem
            // 
            this.btnRunSystem.BackColor = System.Drawing.Color.GreenYellow;
            this.btnRunSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRunSystem.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunSystem.Image = ((System.Drawing.Image)(resources.GetObject("btnRunSystem.Image")));
            this.btnRunSystem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRunSystem.Name = "btnRunSystem";
            this.btnRunSystem.Size = new System.Drawing.Size(75, 22);
            this.btnRunSystem.Text = "RUN SYSTEM";
            this.btnRunSystem.Click += new System.EventHandler(this.btnRunSystem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExitSystem
            // 
            this.btnExitSystem.BackColor = System.Drawing.Color.Red;
            this.btnExitSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExitSystem.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExitSystem.Image = ((System.Drawing.Image)(resources.GetObject("btnExitSystem.Image")));
            this.btnExitSystem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExitSystem.Name = "btnExitSystem";
            this.btnExitSystem.Size = new System.Drawing.Size(72, 22);
            this.btnExitSystem.Text = "EXIT SYSTEM";
            this.btnExitSystem.Click += new System.EventHandler(this.btnExitSystem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(2, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(2, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Strategy Group";
            // 
            // modelDateTimePicker
            // 
            this.modelDateTimePicker.Location = new System.Drawing.Point(101, 69);
            this.modelDateTimePicker.Name = "modelDateTimePicker";
            this.modelDateTimePicker.Size = new System.Drawing.Size(191, 20);
            this.modelDateTimePicker.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(2, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "MODEL DATE";
            // 
            // checkBoxUseHalfDay
            // 
            this.checkBoxUseHalfDay.AutoSize = true;
            this.checkBoxUseHalfDay.Location = new System.Drawing.Point(13, 19);
            this.checkBoxUseHalfDay.Name = "checkBoxUseHalfDay";
            this.checkBoxUseHalfDay.Size = new System.Drawing.Size(140, 17);
            this.checkBoxUseHalfDay.TabIndex = 28;
            this.checkBoxUseHalfDay.Text = "Set System to Half Day";
            this.checkBoxUseHalfDay.UseVisualStyleBackColor = true;
            this.checkBoxUseHalfDay.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // transactionTime
            // 
            this.transactionTime.CustomFormat = "HH:mm";
            this.transactionTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.transactionTime.Location = new System.Drawing.Point(6, 17);
            this.transactionTime.Name = "transactionTime";
            this.transactionTime.ShowUpDown = true;
            this.transactionTime.Size = new System.Drawing.Size(58, 22);
            this.transactionTime.TabIndex = 29;
            this.transactionTime.Value = new System.DateTime(2014, 12, 15, 9, 30, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(70, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Transaction Time";
            // 
            // decisionMinuteOffset
            // 
            this.decisionMinuteOffset.Location = new System.Drawing.Point(6, 47);
            this.decisionMinuteOffset.Name = "decisionMinuteOffset";
            this.decisionMinuteOffset.Size = new System.Drawing.Size(58, 22);
            this.decisionMinuteOffset.TabIndex = 31;
            this.decisionMinuteOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.timeGroupBox);
            this.groupBox1.Controls.Add(this.checkBoxUseHalfDay);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 127);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 144);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // timeGroupBox
            // 
            this.timeGroupBox.Controls.Add(this.label5);
            this.timeGroupBox.Controls.Add(this.transactionTime);
            this.timeGroupBox.Controls.Add(this.label4);
            this.timeGroupBox.Controls.Add(this.decisionMinuteOffset);
            this.timeGroupBox.Enabled = false;
            this.timeGroupBox.Location = new System.Drawing.Point(7, 42);
            this.timeGroupBox.Name = "timeGroupBox";
            this.timeGroupBox.Size = new System.Drawing.Size(274, 79);
            this.timeGroupBox.TabIndex = 33;
            this.timeGroupBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(70, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Decision Offset Minutes Back";
            // 
            // checkBoxCloudDB
            // 
            this.checkBoxCloudDB.AutoSize = true;
            this.checkBoxCloudDB.Location = new System.Drawing.Point(221, 104);
            this.checkBoxCloudDB.Name = "checkBoxCloudDB";
            this.checkBoxCloudDB.Size = new System.Drawing.Size(71, 17);
            this.checkBoxCloudDB.TabIndex = 33;
            this.checkBoxCloudDB.Text = "Cloud DB";
            this.checkBoxCloudDB.UseVisualStyleBackColor = true;
            // 
            // InitializationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(298, 343);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxCloudDB);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.modelDateTimePicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbxPortfolio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbxDatabase);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InitializationForm";
            this.Text = "Initialization Form";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decisionMinuteOffset)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.timeGroupBox.ResumeLayout(false);
            this.timeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbxDatabase;
        private System.Windows.Forms.ComboBox cmbxPortfolio;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRunSystem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExitSystem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker modelDateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxUseHalfDay;
        private System.Windows.Forms.DateTimePicker transactionTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown decisionMinuteOffset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox timeGroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxCloudDB;
    }
}