namespace DataCollectionForRealtime
{
    partial class DCMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCMainForm));
            this.statusStripOptionMonitor = new System.Windows.Forms.StatusStrip();
            this.statusOfUpdatedInstruments = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusSubscribeData = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStripMainRealtime = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemListern = new System.Windows.Forms.ToolStripMenuItem();
            this.MiniMonitorCallTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeMongoDbUrlTSMI = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.tabPageMongoDbUrl = new System.Windows.Forms.TabPage();
            this.AutomaticProcCheckBox = new System.Windows.Forms.CheckBox();
            this.LogRTBox = new System.Windows.Forms.RichTextBox();
            this.ProcessAndSendAnswerBtn = new System.Windows.Forms.Button();
            this.CheckQueriesBtn = new System.Windows.Forms.Button();
            this.MongoDbUrlLbl = new System.Windows.Forms.Label();
            this.MongoDbUrlTBox = new System.Windows.Forms.TextBox();
            this.ChangeMongoDbUrlBtn = new System.Windows.Forms.Button();
            this.labelLogMode = new System.Windows.Forms.Label();
            this.comboBoxLogMode = new System.Windows.Forms.ComboBox();
            this.labelDayNight = new System.Windows.Forms.Label();
            this.statusStripOptionMonitor.SuspendLayout();
            this.menuStripMainRealtime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.tabPageMongoDbUrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripOptionMonitor
            // 
            this.statusStripOptionMonitor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripOptionMonitor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusOfUpdatedInstruments,
            this.connectionStatus,
            this.statusSubscribeData,
            this.dataStatus});
            this.statusStripOptionMonitor.Location = new System.Drawing.Point(0, 391);
            this.statusStripOptionMonitor.Name = "statusStripOptionMonitor";
            this.statusStripOptionMonitor.Size = new System.Drawing.Size(795, 22);
            this.statusStripOptionMonitor.TabIndex = 6;
            this.statusStripOptionMonitor.Text = "statusStrip1";
            // 
            // statusOfUpdatedInstruments
            // 
            this.statusOfUpdatedInstruments.Name = "statusOfUpdatedInstruments";
            this.statusOfUpdatedInstruments.Size = new System.Drawing.Size(148, 17);
            this.statusOfUpdatedInstruments.Text = "statusOfUpdatedInstruments";
            // 
            // connectionStatus
            // 
            this.connectionStatus.BackColor = System.Drawing.Color.Black;
            this.connectionStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectionStatus.ForeColor = System.Drawing.Color.White;
            this.connectionStatus.Image = ((System.Drawing.Image)(resources.GetObject("connectionStatus.Image")));
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.connectionStatus.Size = new System.Drawing.Size(114, 17);
            this.connectionStatus.Text = "CQG:WAITING";
            // 
            // statusSubscribeData
            // 
            this.statusSubscribeData.BackColor = System.Drawing.Color.White;
            this.statusSubscribeData.Name = "statusSubscribeData";
            this.statusSubscribeData.Size = new System.Drawing.Size(106, 17);
            this.statusSubscribeData.Text = "statusSubscribeData";
            // 
            // dataStatus
            // 
            this.dataStatus.BackColor = System.Drawing.Color.Black;
            this.dataStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataStatus.ForeColor = System.Drawing.Color.White;
            this.dataStatus.Name = "dataStatus";
            this.dataStatus.Size = new System.Drawing.Size(62, 17);
            this.dataStatus.Text = "dataStatus";
            // 
            // menuStripMainRealtime
            // 
            this.menuStripMainRealtime.BackColor = System.Drawing.SystemColors.Control;
            this.menuStripMainRealtime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemListern,
            this.MiniMonitorCallTSMI,
            this.ChangeMongoDbUrlTSMI});
            this.menuStripMainRealtime.Location = new System.Drawing.Point(0, 0);
            this.menuStripMainRealtime.Name = "menuStripMainRealtime";
            this.menuStripMainRealtime.Size = new System.Drawing.Size(795, 24);
            this.menuStripMainRealtime.TabIndex = 8;
            this.menuStripMainRealtime.Text = "menuStrip1";
            // 
            // toolStripMenuItemListern
            // 
            this.toolStripMenuItemListern.Name = "toolStripMenuItemListern";
            this.toolStripMenuItemListern.Size = new System.Drawing.Size(12, 20);
            // 
            // MiniMonitorCallTSMI
            // 
            this.MiniMonitorCallTSMI.Name = "MiniMonitorCallTSMI";
            this.MiniMonitorCallTSMI.Size = new System.Drawing.Size(89, 20);
            this.MiniMonitorCallTSMI.Text = "Mini Monitor";
            this.MiniMonitorCallTSMI.Click += new System.EventHandler(this.MiniMonitorCallTSMI_Click);
            // 
            // ChangeMongoDbUrlTSMI
            // 
            this.ChangeMongoDbUrlTSMI.Name = "ChangeMongoDbUrlTSMI";
            this.ChangeMongoDbUrlTSMI.Size = new System.Drawing.Size(155, 20);
            this.ChangeMongoDbUrlTSMI.Text = "Change URL of MongoDB";
            this.ChangeMongoDbUrlTSMI.Click += new System.EventHandler(this.ChangeMongoDbUrlTSMI_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Location = new System.Drawing.Point(12, 48);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.MainTabControl);
            this.splitContainer.Size = new System.Drawing.Size(771, 304);
            this.splitContainer.SplitterDistance = 208;
            this.splitContainer.TabIndex = 10;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.tabPageMongoDbUrl);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(559, 304);
            this.MainTabControl.TabIndex = 7;
            // 
            // tabPageMongoDbUrl
            // 
            this.tabPageMongoDbUrl.Controls.Add(this.AutomaticProcCheckBox);
            this.tabPageMongoDbUrl.Controls.Add(this.LogRTBox);
            this.tabPageMongoDbUrl.Controls.Add(this.ProcessAndSendAnswerBtn);
            this.tabPageMongoDbUrl.Controls.Add(this.CheckQueriesBtn);
            this.tabPageMongoDbUrl.Location = new System.Drawing.Point(4, 22);
            this.tabPageMongoDbUrl.Name = "tabPageMongoDbUrl";
            this.tabPageMongoDbUrl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMongoDbUrl.Size = new System.Drawing.Size(551, 278);
            this.tabPageMongoDbUrl.TabIndex = 2;
            this.tabPageMongoDbUrl.Text = "Mongo-CQG";
            this.tabPageMongoDbUrl.UseVisualStyleBackColor = true;
            // 
            // AutomaticProcCheckBox
            // 
            this.AutomaticProcCheckBox.AutoSize = true;
            this.AutomaticProcCheckBox.Checked = true;
            this.AutomaticProcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutomaticProcCheckBox.Location = new System.Drawing.Point(444, 12);
            this.AutomaticProcCheckBox.Name = "AutomaticProcCheckBox";
            this.AutomaticProcCheckBox.Size = new System.Drawing.Size(99, 17);
            this.AutomaticProcCheckBox.TabIndex = 3;
            this.AutomaticProcCheckBox.Text = "Autoprocessing";
            this.AutomaticProcCheckBox.UseVisualStyleBackColor = true;
            this.AutomaticProcCheckBox.CheckedChanged += new System.EventHandler(this.AutomaticProcCheckBox_CheckedChanged);
            // 
            // LogRTBox
            // 
            this.LogRTBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.LogRTBox.ForeColor = System.Drawing.SystemColors.Info;
            this.LogRTBox.Location = new System.Drawing.Point(7, 37);
            this.LogRTBox.Name = "LogRTBox";
            this.LogRTBox.Size = new System.Drawing.Size(538, 235);
            this.LogRTBox.TabIndex = 2;
            this.LogRTBox.Text = "";
            // 
            // ProcessAndSendAnswerBtn
            // 
            this.ProcessAndSendAnswerBtn.Location = new System.Drawing.Point(163, 7);
            this.ProcessAndSendAnswerBtn.Name = "ProcessAndSendAnswerBtn";
            this.ProcessAndSendAnswerBtn.Size = new System.Drawing.Size(271, 23);
            this.ProcessAndSendAnswerBtn.TabIndex = 1;
            this.ProcessAndSendAnswerBtn.Text = "Process the request and send a response to the DB";
            this.ProcessAndSendAnswerBtn.UseVisualStyleBackColor = true;
            this.ProcessAndSendAnswerBtn.Click += new System.EventHandler(this.ProcessAndSendAnswerBtn_Click);
            // 
            // CheckQueriesBtn
            // 
            this.CheckQueriesBtn.Location = new System.Drawing.Point(7, 7);
            this.CheckQueriesBtn.Name = "CheckQueriesBtn";
            this.CheckQueriesBtn.Size = new System.Drawing.Size(150, 23);
            this.CheckQueriesBtn.TabIndex = 0;
            this.CheckQueriesBtn.Text = "Check queries in MongoDB";
            this.CheckQueriesBtn.UseVisualStyleBackColor = true;
            this.CheckQueriesBtn.Click += new System.EventHandler(this.CheckQueriesBtn_Click);
            // 
            // MongoDbUrlLbl
            // 
            this.MongoDbUrlLbl.AutoSize = true;
            this.MongoDbUrlLbl.Location = new System.Drawing.Point(9, 24);
            this.MongoDbUrlLbl.Name = "MongoDbUrlLbl";
            this.MongoDbUrlLbl.Size = new System.Drawing.Size(83, 13);
            this.MongoDbUrlLbl.TabIndex = 11;
            this.MongoDbUrlLbl.Text = "MongoDB URL:";
            this.MongoDbUrlLbl.Visible = false;
            // 
            // MongoDbUrlTBox
            // 
            this.MongoDbUrlTBox.Location = new System.Drawing.Point(101, 21);
            this.MongoDbUrlTBox.Name = "MongoDbUrlTBox";
            this.MongoDbUrlTBox.Size = new System.Drawing.Size(274, 20);
            this.MongoDbUrlTBox.TabIndex = 12;
            this.MongoDbUrlTBox.Visible = false;
            // 
            // ChangeMongoDbUrlBtn
            // 
            this.ChangeMongoDbUrlBtn.Location = new System.Drawing.Point(381, 20);
            this.ChangeMongoDbUrlBtn.Name = "ChangeMongoDbUrlBtn";
            this.ChangeMongoDbUrlBtn.Size = new System.Drawing.Size(75, 21);
            this.ChangeMongoDbUrlBtn.TabIndex = 13;
            this.ChangeMongoDbUrlBtn.Text = "Apply";
            this.ChangeMongoDbUrlBtn.UseVisualStyleBackColor = true;
            this.ChangeMongoDbUrlBtn.Visible = false;
            this.ChangeMongoDbUrlBtn.Click += new System.EventHandler(this.ChangeDBURLBtn_Click);
            // 
            // labelLogMode
            // 
            this.labelLogMode.AutoSize = true;
            this.labelLogMode.Location = new System.Drawing.Point(228, 359);
            this.labelLogMode.Name = "labelLogMode";
            this.labelLogMode.Size = new System.Drawing.Size(55, 13);
            this.labelLogMode.TabIndex = 14;
            this.labelLogMode.Text = "Log Mode";
            // 
            // comboBoxLogMode
            // 
            this.comboBoxLogMode.FormattingEnabled = true;
            this.comboBoxLogMode.Location = new System.Drawing.Point(298, 356);
            this.comboBoxLogMode.Name = "comboBoxLogMode";
            this.comboBoxLogMode.Size = new System.Drawing.Size(87, 21);
            this.comboBoxLogMode.TabIndex = 15;
            // 
            // labelDayNight
            // 
            this.labelDayNight.AutoSize = true;
            this.labelDayNight.BackColor = System.Drawing.Color.White;
            this.labelDayNight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDayNight.Location = new System.Drawing.Point(391, 359);
            this.labelDayNight.Name = "labelDayNight";
            this.labelDayNight.Size = new System.Drawing.Size(29, 13);
            this.labelDayNight.TabIndex = 16;
            this.labelDayNight.Text = "Day";
            this.labelDayNight.Click += new System.EventHandler(this.labelDayNight_Click);
            // 
            // DCMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 413);
            this.Controls.Add(this.labelDayNight);
            this.Controls.Add(this.comboBoxLogMode);
            this.Controls.Add(this.labelLogMode);
            this.Controls.Add(this.ChangeMongoDbUrlBtn);
            this.Controls.Add(this.MongoDbUrlTBox);
            this.Controls.Add(this.MongoDbUrlLbl);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStripMainRealtime);
            this.Controls.Add(this.statusStripOptionMonitor);
            this.Name = "DCMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Collector for Realtime";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCMainForm_FormClosing);
            this.Load += new System.EventHandler(this.RealtimeDataManagement_Load);
            this.statusStripOptionMonitor.ResumeLayout(false);
            this.statusStripOptionMonitor.PerformLayout();
            this.menuStripMainRealtime.ResumeLayout(false);
            this.menuStripMainRealtime.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.tabPageMongoDbUrl.ResumeLayout(false);
            this.tabPageMongoDbUrl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.StatusStrip statusStripOptionMonitor;
        private System.Windows.Forms.ToolStripStatusLabel statusOfUpdatedInstruments;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusSubscribeData;
        private System.Windows.Forms.ToolStripStatusLabel dataStatus;
        private System.Windows.Forms.MenuStrip menuStripMainRealtime;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemListern;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage tabPageMongoDbUrl;
        private System.Windows.Forms.CheckBox AutomaticProcCheckBox;
        private System.Windows.Forms.RichTextBox LogRTBox;
        private System.Windows.Forms.Button ProcessAndSendAnswerBtn;
        private System.Windows.Forms.Button CheckQueriesBtn;
        private System.Windows.Forms.ToolStripMenuItem MiniMonitorCallTSMI;
        private System.Windows.Forms.ToolStripMenuItem ChangeMongoDbUrlTSMI;
        private System.Windows.Forms.Label MongoDbUrlLbl;
        private System.Windows.Forms.TextBox MongoDbUrlTBox;
        private System.Windows.Forms.Button ChangeMongoDbUrlBtn;
        private System.Windows.Forms.Label labelLogMode;
        private System.Windows.Forms.ComboBox comboBoxLogMode;
        private System.Windows.Forms.Label labelDayNight;
    }
}

