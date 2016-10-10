﻿namespace DataCollectionForRealtime
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
            this.mainRealtimeMenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemListern = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeURLOfMongoDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMongo = new System.Windows.Forms.TabPage();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonRespond = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.MongoDBURLLabel = new System.Windows.Forms.Label();
            this.MongoDBURL = new System.Windows.Forms.TextBox();
            this.ChangeDBURLBtn = new System.Windows.Forms.Button();
            this.logSettingsLabel = new System.Windows.Forms.Label();
            this.logSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.statusStripOptionMonitor.SuspendLayout();
            this.mainRealtimeMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMongo.SuspendLayout();
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
            this.mainRealtimeMenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.mainRealtimeMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemListern,
            this.minimizeWindowToolStripMenuItem,
            this.changeURLOfMongoDBToolStripMenuItem});
            this.mainRealtimeMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainRealtimeMenuStrip.Name = "mainRealtimeMenuStrip";
            this.mainRealtimeMenuStrip.Size = new System.Drawing.Size(795, 24);
            this.mainRealtimeMenuStrip.TabIndex = 8;
            this.mainRealtimeMenuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItemListern
            // 
            this.toolStripMenuItemListern.Name = "toolStripMenuItemListern";
            this.toolStripMenuItemListern.Size = new System.Drawing.Size(12, 20);
            // 
            // minimizeWindowToolStripMenuItem
            // 
            this.minimizeWindowToolStripMenuItem.Name = "minimizeWindowToolStripMenuItem";
            this.minimizeWindowToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.minimizeWindowToolStripMenuItem.Text = "Mini Monitor";
            this.minimizeWindowToolStripMenuItem.Click += new System.EventHandler(this.MinimizeWindowToolStripMenuItem_Click);
            // 
            // changeURLOfMongoDBToolStripMenuItem
            // 
            this.changeURLOfMongoDBToolStripMenuItem.Name = "changeURLOfMongoDBToolStripMenuItem";
            this.changeURLOfMongoDBToolStripMenuItem.Size = new System.Drawing.Size(155, 20);
            this.changeURLOfMongoDBToolStripMenuItem.Text = "Change URL of MongoDB";
            this.changeURLOfMongoDBToolStripMenuItem.Click += new System.EventHandler(this.ChangeURLOfMongoDBToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 48);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlMain);
            this.splitContainer1.Size = new System.Drawing.Size(771, 304);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.TabIndex = 10;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageMongo);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(559, 304);
            this.tabControlMain.TabIndex = 7;
            // 
            // tabPageMongo
            // 
            this.tabPageMongo.Controls.Add(this.checkBoxAuto);
            this.tabPageMongo.Controls.Add(this.richTextBoxLog);
            this.tabPageMongo.Controls.Add(this.buttonRespond);
            this.tabPageMongo.Controls.Add(this.buttonCheck);
            this.tabPageMongo.Location = new System.Drawing.Point(4, 22);
            this.tabPageMongo.Name = "tabPageMongo";
            this.tabPageMongo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMongo.Size = new System.Drawing.Size(551, 278);
            this.tabPageMongo.TabIndex = 2;
            this.tabPageMongo.Text = "Mongo-CQG";
            this.tabPageMongo.UseVisualStyleBackColor = true;
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
            // MongoDBURLLabel
            // 
            this.MongoDBURLLabel.AutoSize = true;
            this.MongoDBURLLabel.Location = new System.Drawing.Point(9, 24);
            this.MongoDBURLLabel.Name = "MongoDBURLLabel";
            this.MongoDBURLLabel.Size = new System.Drawing.Size(83, 13);
            this.MongoDBURLLabel.TabIndex = 11;
            this.MongoDBURLLabel.Text = "MongoDB URL:";
            this.MongoDBURLLabel.Visible = false;
            // 
            // MongoDBURL
            // 
            this.MongoDBURL.Location = new System.Drawing.Point(101, 21);
            this.MongoDBURL.Name = "MongoDBURL";
            this.MongoDBURL.Size = new System.Drawing.Size(274, 20);
            this.MongoDBURL.TabIndex = 12;
            this.MongoDBURL.Visible = false;
            // 
            // ChangeDBURLBtn
            // 
            this.ChangeDBURLBtn.Location = new System.Drawing.Point(381, 20);
            this.ChangeDBURLBtn.Name = "ChangeDBURLBtn";
            this.ChangeDBURLBtn.Size = new System.Drawing.Size(75, 21);
            this.ChangeDBURLBtn.TabIndex = 13;
            this.ChangeDBURLBtn.Text = "Apply";
            this.ChangeDBURLBtn.UseVisualStyleBackColor = true;
            this.ChangeDBURLBtn.Visible = false;
            this.ChangeDBURLBtn.Click += new System.EventHandler(this.ChangeDBURLBtn_Click);
            // 
            // logSettingsLabel
            // 
            this.logSettingsLabel.AutoSize = true;
            this.logSettingsLabel.Location = new System.Drawing.Point(228, 359);
            this.logSettingsLabel.Name = "logSettingsLabel";
            this.logSettingsLabel.Size = new System.Drawing.Size(64, 13);
            this.logSettingsLabel.TabIndex = 14;
            this.logSettingsLabel.Text = "Log settings";
            // 
            // logSettingsComboBox
            // 
            this.logSettingsComboBox.FormattingEnabled = true;
            this.logSettingsComboBox.Location = new System.Drawing.Point(298, 356);
            this.logSettingsComboBox.Name = "logSettingsComboBox";
            this.logSettingsComboBox.Size = new System.Drawing.Size(87, 21);
            this.logSettingsComboBox.TabIndex = 15;
            // 
            // DCMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 413);
            this.Controls.Add(this.logSettingsComboBox);
            this.Controls.Add(this.logSettingsLabel);
            this.Controls.Add(this.ChangeDBURLBtn);
            this.Controls.Add(this.MongoDBURL);
            this.Controls.Add(this.MongoDBURLLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainRealtimeMenuStrip);
            this.Controls.Add(this.statusStripOptionMonitor);
            this.Name = "DCMainForm";
            this.Text = "Data Collector for Realtime";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCMainForm_FormClosing);
            this.Load += new System.EventHandler(this.RealtimeDataManagement_Load);
            this.statusStripOptionMonitor.ResumeLayout(false);
            this.statusStripOptionMonitor.PerformLayout();
            this.mainRealtimeMenuStrip.ResumeLayout(false);
            this.mainRealtimeMenuStrip.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMongo.ResumeLayout(false);
            this.tabPageMongo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripOptionMonitor;
        private System.Windows.Forms.ToolStripStatusLabel statusOfUpdatedInstruments;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusSubscribeData;
        private System.Windows.Forms.ToolStripStatusLabel dataStatus;
        private System.Windows.Forms.MenuStrip mainRealtimeMenuStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemListern;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMongo;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonRespond;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.ToolStripMenuItem minimizeWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeURLOfMongoDBToolStripMenuItem;
        private System.Windows.Forms.Label MongoDBURLLabel;
        private System.Windows.Forms.TextBox MongoDBURL;
        private System.Windows.Forms.Button ChangeDBURLBtn;
        private System.Windows.Forms.Label logSettingsLabel;
        private System.Windows.Forms.ComboBox logSettingsComboBox;
    }
}
