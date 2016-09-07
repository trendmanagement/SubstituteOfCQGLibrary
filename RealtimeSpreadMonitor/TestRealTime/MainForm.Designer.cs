namespace TestRealTime
{
    partial class MainForm
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
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonRead = new System.Windows.Forms.Button();
            this.checkBoxAuto = new System.Windows.Forms.CheckBox();
            this.buttonClean = new System.Windows.Forms.Button();
            this.buttonGenQuery = new System.Windows.Forms.Button();
            this.buttonCall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBoxLog.Location = new System.Drawing.Point(3, 70);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(555, 244);
            this.richTextBoxLog.TabIndex = 3;
            this.richTextBoxLog.Text = "";
            // 
            // buttonRead
            // 
            this.buttonRead.BackColor = System.Drawing.Color.DimGray;
            this.buttonRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRead.ForeColor = System.Drawing.Color.White;
            this.buttonRead.Location = new System.Drawing.Point(3, 41);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(170, 23);
            this.buttonRead.TabIndex = 6;
            this.buttonRead.Text = "Read data from MongoDB";
            this.buttonRead.UseVisualStyleBackColor = false;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // checkBoxAuto
            // 
            this.checkBoxAuto.AutoSize = true;
            this.checkBoxAuto.Enabled = false;
            this.checkBoxAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxAuto.ForeColor = System.Drawing.Color.White;
            this.checkBoxAuto.Location = new System.Drawing.Point(299, 47);
            this.checkBoxAuto.Name = "checkBoxAuto";
            this.checkBoxAuto.Size = new System.Drawing.Size(113, 17);
            this.checkBoxAuto.TabIndex = 7;
            this.checkBoxAuto.Text = "Automatic work";
            this.checkBoxAuto.UseVisualStyleBackColor = true;
            this.checkBoxAuto.CheckedChanged += new System.EventHandler(this.checkBoxAuto_CheckedChanged);
            // 
            // buttonClean
            // 
            this.buttonClean.BackColor = System.Drawing.Color.DimGray;
            this.buttonClean.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClean.ForeColor = System.Drawing.Color.White;
            this.buttonClean.Location = new System.Drawing.Point(179, 41);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(114, 23);
            this.buttonClean.TabIndex = 8;
            this.buttonClean.Text = "Clean MongoDB";
            this.buttonClean.UseVisualStyleBackColor = false;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // buttonGenQuery
            // 
            this.buttonGenQuery.BackColor = System.Drawing.Color.DimGray;
            this.buttonGenQuery.Enabled = false;
            this.buttonGenQuery.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.buttonGenQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGenQuery.ForeColor = System.Drawing.Color.White;
            this.buttonGenQuery.Location = new System.Drawing.Point(3, 12);
            this.buttonGenQuery.Name = "buttonGenQuery";
            this.buttonGenQuery.Size = new System.Drawing.Size(170, 23);
            this.buttonGenQuery.TabIndex = 9;
            this.buttonGenQuery.Text = "Add entry in MongoDB";
            this.buttonGenQuery.UseVisualStyleBackColor = false;
            // 
            // buttonCall
            // 
            this.buttonCall.BackColor = System.Drawing.Color.ForestGreen;
            this.buttonCall.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.ForeColor = System.Drawing.Color.White;
            this.buttonCall.Location = new System.Drawing.Point(179, 12);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(114, 23);
            this.buttonCall.TabIndex = 9;
            this.buttonCall.Text = "Call test queries";
            this.buttonCall.UseVisualStyleBackColor = false;
            this.buttonCall.Click += new System.EventHandler(this.buttonCall_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(564, 320);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonGenQuery);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.checkBoxAuto);
            this.Controls.Add(this.buttonRead);
            this.Controls.Add(this.richTextBoxLog);
            this.Name = "MainForm";
            this.Text = "Test RealTime";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.CheckBox checkBoxAuto;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button buttonGenQuery;
        private System.Windows.Forms.Button buttonCall;
    }
}

