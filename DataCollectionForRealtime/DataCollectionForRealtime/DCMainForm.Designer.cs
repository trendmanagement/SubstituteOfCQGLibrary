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
            this.toolStripMenuItemMinimizeWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemChangeMongoDbUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMongoDbUrl = new System.Windows.Forms.TabPage();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonRespond = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.labelMongoDbUrl = new System.Windows.Forms.Label();
            this.textBoxMongoDbUrl = new System.Windows.Forms.TextBox();
            this.buttonChangeMongoDbUrl = new System.Windows.Forms.Button();
            this.labelLogMode = new System.Windows.Forms.Label();
            this.comboBoxLogMode = new System.Windows.Forms.ComboBox();
            this.statusStripOptionMonitor.SuspendLayout();
            this.menuStripMainRealtime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControlMain.SuspendLayout();
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
            // mainRealtimeMenuStrip
            // 
            this.menuStripMainRealtime.BackColor = System.Drawing.SystemColors.Control;
            this.menuStripMainRealtime.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemListern,
            this.toolStripMenuItemMinimizeWindow,
            this.toolStripMenuItemChangeMongoDbUrl});
            this.menuStripMainRealtime.Location = new System.Drawing.Point(0, 0);
            this.menuStripMainRealtime.Name = "mainRealtimeMenuStrip";
            this.menuStripMainRealtime.Size = new System.Drawing.Size(795, 24);
            this.menuStripMainRealtime.TabIndex = 8;
            this.menuStripMainRealtime.Text = "menuStrip1";
            // 
            // toolStripMenuItemListern
            // 
            this.toolStripMenuItemListern.Name = "toolStripMenuItemListern";
            this.toolStripMenuItemListern.Size = new System.Drawing.Size(12, 20);
            // 
            // toolStripMenuItemMinimizeWindow
            // 
            this.toolStripMenuItemMinimizeWindow.Name = "minimizeWindowToolStripMenuItem";
            this.toolStripMenuItemMinimizeWindow.Size = new System.Drawing.Size(89, 20);
            this.toolStripMenuItemMinimizeWindow.Text = "Mini Monitor";
            this.toolStripMenuItemMinimizeWindow.Click += new System.EventHandler(this.MinimizeWindowToolStripMenuItem_Click);
            // 
            // toolStripMenuItemChangeMongoDbUrl
            // 
            this.toolStripMenuItemChangeMongoDbUrl.Name = "changeURLOfMongoDBToolStripMenuItem";
            this.toolStripMenuItemChangeMongoDbUrl.Size = new System.Drawing.Size(155, 20);
            this.toolStripMenuItemChangeMongoDbUrl.Text = "Change URL of MongoDB";
            this.toolStripMenuItemChangeMongoDbUrl.Click += new System.EventHandler(this.ChangeURLOfMongoDBToolStripMenuItem_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Location = new System.Drawing.Point(12, 48);
            this.splitContainer.Name = "splitContainer1";
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlMain);
            this.splitContainer.Size = new System.Drawing.Size(771, 304);
            this.splitContainer.SplitterDistance = 208;
            this.splitContainer.TabIndex = 10;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMongoDbUrl);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(559, 304);
            this.tabControlMain.TabIndex = 7;
            // 
            // tabPageMongoDbUrl
            // 
            this.tabPageMongoDbUrl.Controls.Add(this.checkBoxAuto);
            this.tabPageMongoDbUrl.Controls.Add(this.richTextBoxLog);
            this.tabPageMongoDbUrl.Controls.Add(this.buttonRespond);
            this.tabPageMongoDbUrl.Controls.Add(this.buttonCheck);
            this.tabPageMongoDbUrl.Location = new System.Drawing.Point(4, 22);
            this.tabPageMongoDbUrl.Name = "tabPageMongo";
            this.tabPageMongoDbUrl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMongoDbUrl.Size = new System.Drawing.Size(551, 278);
            this.tabPageMongoDbUrl.TabIndex = 2;
            this.tabPageMongoDbUrl.Text = "Mongo-CQG";
            this.tabPageMongoDbUrl.UseVisualStyleBackColor = true;
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Location = new System.Drawing.Point(444, 12);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(99, 17);
            this.checkBoxAuto.TabIndex = 3;
            this.checkBoxAuto.Text = "Automatic work";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            this.checkBoxAuto.CheckedChanged += new System.EventHandler(this.CheckBoxAuto_CheckedChanged);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(7, 37);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(538, 235);
            this.richTextBoxLog.TabIndex = 2;
            this.richTextBoxLog.Text = "";
            // 
            // buttonRespond
            // 
            this.buttonRespond.Location = new System.Drawing.Point(163, 7);
            this.buttonRespond.Name = "buttonRespond";
            this.buttonRespond.Size = new System.Drawing.Size(271, 23);
            this.buttonRespond.TabIndex = 1;
            this.buttonRespond.Text = "Process the request and send a response to the DB";
            this.buttonRespond.UseVisualStyleBackColor = true;
            this.buttonRespond.Click += new System.EventHandler(this.ButtonRespond_Click);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(7, 7);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(150, 23);
            this.buttonCheck.TabIndex = 0;
            this.buttonCheck.Text = "Check queries in MongoDB";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.ButtonCheck_Click);
            // 
            // labelMongoDbUrl
            // 
            this.labelMongoDbUrl.AutoSize = true;
            this.labelMongoDbUrl.Location = new System.Drawing.Point(9, 24);
            this.labelMongoDbUrl.Name = "MongoDBURLLabel";
            this.labelMongoDbUrl.Size = new System.Drawing.Size(83, 13);
            this.labelMongoDbUrl.TabIndex = 11;
            this.labelMongoDbUrl.Text = "MongoDB URL:";
            this.labelMongoDbUrl.Visible = false;
            // 
            // textBoxMongoDbUrl
            // 
            this.textBoxMongoDbUrl.Location = new System.Drawing.Point(101, 21);
            this.textBoxMongoDbUrl.Name = "MongoDBURL";
            this.textBoxMongoDbUrl.Size = new System.Drawing.Size(274, 20);
            this.textBoxMongoDbUrl.TabIndex = 12;
            this.textBoxMongoDbUrl.Visible = false;
            // 
            // buttonChangeMongoDbUrl
            // 
            this.buttonChangeMongoDbUrl.Location = new System.Drawing.Point(381, 20);
            this.buttonChangeMongoDbUrl.Name = "ChangeDBURLBtn";
            this.buttonChangeMongoDbUrl.Size = new System.Drawing.Size(75, 21);
            this.buttonChangeMongoDbUrl.TabIndex = 13;
            this.buttonChangeMongoDbUrl.Text = "Apply";
            this.buttonChangeMongoDbUrl.UseVisualStyleBackColor = true;
            this.buttonChangeMongoDbUrl.Visible = false;
            this.buttonChangeMongoDbUrl.Click += new System.EventHandler(this.ChangeDBURLBtn_Click);
            // 
            // labelLogMode
            // 
            this.labelLogMode.AutoSize = true;
            this.labelLogMode.Location = new System.Drawing.Point(228, 359);
            this.labelLogMode.Name = "logSettingsLabel";
            this.labelLogMode.Size = new System.Drawing.Size(64, 13);
            this.labelLogMode.TabIndex = 14;
            this.labelLogMode.Text = "Log Mode";
            // 
            // comboBoxLogMode
            // 
            this.comboBoxLogMode.FormattingEnabled = true;
            this.comboBoxLogMode.Location = new System.Drawing.Point(298, 356);
            this.comboBoxLogMode.Name = "logSettingsComboBox";
            this.comboBoxLogMode.Size = new System.Drawing.Size(87, 21);
            this.comboBoxLogMode.TabIndex = 15;
            // 
            // DCMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 413);
            this.Controls.Add(this.comboBoxLogMode);
            this.Controls.Add(this.labelLogMode);
            this.Controls.Add(this.buttonChangeMongoDbUrl);
            this.Controls.Add(this.textBoxMongoDbUrl);
            this.Controls.Add(this.labelMongoDbUrl);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStripMainRealtime);
            this.Controls.Add(this.statusStripOptionMonitor);
            this.Name = "DCMainForm";
            this.Text = "Data Collector for Realtime";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCMainForm_FormClosing);
            this.FormClosed += DCMainForm_FormClosed;
            this.Load += new System.EventHandler(this.RealtimeDataManagement_Load);
            this.statusStripOptionMonitor.ResumeLayout(false);
            this.statusStripOptionMonitor.PerformLayout();
            this.menuStripMainRealtime.ResumeLayout(false);
            this.menuStripMainRealtime.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMongoDbUrl;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonRespond;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMinimizeWindow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemChangeMongoDbUrl;
        private System.Windows.Forms.Label labelMongoDbUrl;
        private System.Windows.Forms.TextBox textBoxMongoDbUrl;
        private System.Windows.Forms.Button buttonChangeMongoDbUrl;
        private System.Windows.Forms.Label labelLogMode;
        private System.Windows.Forms.ComboBox comboBoxLogMode;
    }
}

