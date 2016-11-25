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
            this.MainFormCallBtn = new System.Windows.Forms.Button();
            this.SymbolsListTBox = new System.Windows.Forms.RichTextBox();
            this.ClearSymbolsListBtn = new System.Windows.Forms.Button();
            this.TimedBarsRequestSymbolsListLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MainFormCallBtn
            // 
            this.MainFormCallBtn.Location = new System.Drawing.Point(13, 13);
            this.MainFormCallBtn.Name = "MainFormCallBtn";
            this.MainFormCallBtn.Size = new System.Drawing.Size(134, 23);
            this.MainFormCallBtn.TabIndex = 0;
            this.MainFormCallBtn.Text = "Main Monitor";
            this.MainFormCallBtn.UseVisualStyleBackColor = true;
            this.MainFormCallBtn.Click += new System.EventHandler(this.MainFormCallBtn_Click);
            // 
            // SymbolsListTBox
            // 
            this.SymbolsListTBox.Location = new System.Drawing.Point(13, 95);
            this.SymbolsListTBox.Name = "SymbolsListTBox";
            this.SymbolsListTBox.Size = new System.Drawing.Size(134, 110);
            this.SymbolsListTBox.TabIndex = 5;
            this.SymbolsListTBox.Text = "";
            // 
            // ClearSymbolsListBtn
            // 
            this.ClearSymbolsListBtn.Location = new System.Drawing.Point(13, 42);
            this.ClearSymbolsListBtn.Name = "ClearSymbolsListBtn";
            this.ClearSymbolsListBtn.Size = new System.Drawing.Size(134, 24);
            this.ClearSymbolsListBtn.TabIndex = 6;
            this.ClearSymbolsListBtn.Text = "Clear symbols list";
            this.ClearSymbolsListBtn.UseVisualStyleBackColor = true;
            this.ClearSymbolsListBtn.Click += new System.EventHandler(this.ClearSymbolsListBtn_Click);
            // 
            // TimedBarsRequestSymbolsListLbl
            // 
            this.TimedBarsRequestSymbolsListLbl.AutoSize = true;
            this.TimedBarsRequestSymbolsListLbl.Location = new System.Drawing.Point(10, 81);
            this.TimedBarsRequestSymbolsListLbl.Name = "TimedBarsRequestSymbolsListLbl";
            this.TimedBarsRequestSymbolsListLbl.Size = new System.Drawing.Size(138, 13);
            this.TimedBarsRequestSymbolsListLbl.TabIndex = 7;
            this.TimedBarsRequestSymbolsListLbl.Text = "TimedBars request symbols:";
            // 
            // DCMiniMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(160, 216);
            this.Controls.Add(this.TimedBarsRequestSymbolsListLbl);
            this.Controls.Add(this.ClearSymbolsListBtn);
            this.Controls.Add(this.SymbolsListTBox);
            this.Controls.Add(this.MainFormCallBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DCMiniMonitor";
            this.Text = "Mini Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DCMiniMonitor_FormClosing);
            this.Load += new System.EventHandler(this.DCMiniMonitor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MainFormCallBtn;
        private System.Windows.Forms.RichTextBox SymbolsListTBox;
        private System.Windows.Forms.Button ClearSymbolsListBtn;
        private System.Windows.Forms.Label TimedBarsRequestSymbolsListLbl;
    }
}