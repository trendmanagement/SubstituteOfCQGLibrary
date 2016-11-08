using System.Diagnostics;
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using CQG = FakeCQG;

namespace HistoricalSessions
{
    /// <summary>
    /// Summary description for frmHistSessionsRequestSettings.
    /// </summary>
    public class frmHistSessionsRequestSettings : System.Windows.Forms.Form
    {
      internal System.Windows.Forms.Label Label1;
      internal System.Windows.Forms.TextBox txtSymbol;
      internal System.Windows.Forms.Button btnOK;
      internal System.Windows.Forms.Button btnCancel;
      internal System.Windows.Forms.GroupBox GroupBox1;
      internal System.Windows.Forms.GroupBox GroupBox3;
      internal System.Windows.Forms.RadioButton optSinceTime;
      internal System.Windows.Forms.DateTimePicker dtpEndRange;
      internal System.Windows.Forms.DateTimePicker dtpStartRange;
      internal System.Windows.Forms.Label Label3;
      internal System.Windows.Forms.Label Label2;
      internal System.Windows.Forms.RadioButton optTimeRange;
      internal System.Windows.Forms.RadioButton optAll;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmHistSessionsRequestSettings()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Add any constructor code after InitializeComponent call
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.Label1 = new System.Windows.Forms.Label();
         this.txtSymbol = new System.Windows.Forms.TextBox();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.GroupBox1 = new System.Windows.Forms.GroupBox();
         this.GroupBox3 = new System.Windows.Forms.GroupBox();
         this.optAll = new System.Windows.Forms.RadioButton();
         this.optSinceTime = new System.Windows.Forms.RadioButton();
         this.optTimeRange = new System.Windows.Forms.RadioButton();
         this.dtpEndRange = new System.Windows.Forms.DateTimePicker();
         this.dtpStartRange = new System.Windows.Forms.DateTimePicker();
         this.Label3 = new System.Windows.Forms.Label();
         this.Label2 = new System.Windows.Forms.Label();
         this.GroupBox1.SuspendLayout();
         this.GroupBox3.SuspendLayout();
         this.SuspendLayout();
         // 
         // Label1
         // 
         this.Label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label1.Location = new System.Drawing.Point(15, 8);
         this.Label1.Name = "Label1";
         this.Label1.Size = new System.Drawing.Size(64, 24);
         this.Label1.TabIndex = 6;
         this.Label1.Text = "Symbol :";
         // 
         // txtSymbol
         // 
         this.txtSymbol.AutoSize = false;
         this.txtSymbol.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.txtSymbol.Location = new System.Drawing.Point(79, 8);
         this.txtSymbol.Name = "txtSymbol";
         this.txtSymbol.Size = new System.Drawing.Size(296, 20);
         this.txtSymbol.TabIndex = 5;
         this.txtSymbol.Text = "";
         // 
         // btnOK
         // 
         this.btnOK.Location = new System.Drawing.Point(223, 196);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(72, 24);
         this.btnOK.TabIndex = 8;
         this.btnOK.Text = "OK";
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(303, 196);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(72, 24);
         this.btnCancel.TabIndex = 9;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // GroupBox1
         // 
         this.GroupBox1.Controls.Add(this.GroupBox3);
         this.GroupBox1.Controls.Add(this.dtpEndRange);
         this.GroupBox1.Controls.Add(this.dtpStartRange);
         this.GroupBox1.Controls.Add(this.Label3);
         this.GroupBox1.Controls.Add(this.Label2);
         this.GroupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox1.Location = new System.Drawing.Point(7, 40);
         this.GroupBox1.Name = "GroupBox1";
         this.GroupBox1.Size = new System.Drawing.Size(368, 144);
         this.GroupBox1.TabIndex = 7;
         this.GroupBox1.TabStop = false;
         this.GroupBox1.Text = " Request Range";
         // 
         // GroupBox3
         // 
         this.GroupBox3.Controls.Add(this.optAll);
         this.GroupBox3.Controls.Add(this.optSinceTime);
         this.GroupBox3.Controls.Add(this.optTimeRange);
         this.GroupBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox3.Location = new System.Drawing.Point(200, 8);
         this.GroupBox3.Name = "GroupBox3";
         this.GroupBox3.Size = new System.Drawing.Size(160, 128);
         this.GroupBox3.TabIndex = 2;
         this.GroupBox3.TabStop = false;
         this.GroupBox3.Text = "Request Type";
         // 
         // optAll
         // 
         this.optAll.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optAll.Location = new System.Drawing.Point(8, 72);
         this.optAll.Name = "optAll";
         this.optAll.Size = new System.Drawing.Size(128, 24);
         this.optAll.TabIndex = 2;
         this.optAll.Text = "All";
         this.optAll.CheckedChanged += new System.EventHandler(this.RequestTypeCheckedChanged);
         // 
         // optSinceTime
         // 
         this.optSinceTime.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optSinceTime.Location = new System.Drawing.Point(8, 48);
         this.optSinceTime.Name = "optSinceTime";
         this.optSinceTime.Size = new System.Drawing.Size(128, 24);
         this.optSinceTime.TabIndex = 1;
         this.optSinceTime.Text = "Since Time";
         // 
         // optTimeRange
         // 
         this.optTimeRange.Checked = true;
         this.optTimeRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optTimeRange.Location = new System.Drawing.Point(8, 24);
         this.optTimeRange.Name = "optTimeRange";
         this.optTimeRange.Size = new System.Drawing.Size(128, 24);
         this.optTimeRange.TabIndex = 0;
         this.optTimeRange.TabStop = true;
         this.optTimeRange.Text = "Time Range";
         this.optTimeRange.CheckedChanged += new System.EventHandler(this.RequestTypeCheckedChanged);
         // 
         // dtpEndRange
         // 
         this.dtpEndRange.CustomFormat = "dd/MM/yyyy HH:mm";
         this.dtpEndRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.dtpEndRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
         this.dtpEndRange.Location = new System.Drawing.Point(48, 96);
         this.dtpEndRange.Name = "dtpEndRange";
         this.dtpEndRange.Size = new System.Drawing.Size(128, 22);
         this.dtpEndRange.TabIndex = 1;
         // 
         // dtpStartRange
         // 
         this.dtpStartRange.CustomFormat = "dd/MM/yyyy HH:mm";
         this.dtpStartRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.dtpStartRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
         this.dtpStartRange.Location = new System.Drawing.Point(48, 40);
         this.dtpStartRange.Name = "dtpStartRange";
         this.dtpStartRange.Size = new System.Drawing.Size(128, 22);
         this.dtpStartRange.TabIndex = 0;
         // 
         // Label3
         // 
         this.Label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label3.Location = new System.Drawing.Point(8, 96);
         this.Label3.Name = "Label3";
         this.Label3.Size = new System.Drawing.Size(40, 24);
         this.Label3.TabIndex = 1;
         this.Label3.Text = "End";
         this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Label2
         // 
         this.Label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label2.Location = new System.Drawing.Point(8, 40);
         this.Label2.Name = "Label2";
         this.Label2.Size = new System.Drawing.Size(40, 24);
         this.Label2.TabIndex = 0;
         this.Label2.Text = "Start";
         this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // frmHistSessionsRequestSettings
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(382, 229);
         this.Controls.Add(this.Label1);
         this.Controls.Add(this.txtSymbol);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.GroupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
         this.MaximizeBox = false;
         this.Name = "frmHistSessionsRequestSettings";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Historical Sessions Request Settings";
         this.GroupBox1.ResumeLayout(false);
         this.GroupBox3.ResumeLayout(false);
         this.ResumeLayout(false);

      }
        #endregion

      // Current request
      private FakeCQG.CQGHistoricalSessionsRequest m_HistSessionsRequest;

      // CQGCEL line time
      private DateTime m_LineTime;

      /// <summary>
      /// Occurs when the user clicks on the OK button.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void btnOK_Click(object sender, System.EventArgs e)
      {    
         try
         {
            FakeCQG.eHistoricalSessionsRequestType histSessionsReqType;

            if (txtSymbol.Text.Trim() == string.Empty)
            {
               MessageBox.Show("Symbol can not be empty.", "btnOK_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            if (optTimeRange.Checked)
            {
               histSessionsReqType = FakeCQG.eHistoricalSessionsRequestType.hsrtTimeRange;
            }
            else if (optSinceTime.Checked)
            {
               histSessionsReqType = FakeCQG.eHistoricalSessionsRequestType.hsrtSinceTime;
            }
            else
            {
               histSessionsReqType = FakeCQG.eHistoricalSessionsRequestType.hsrtAll;
            }

            m_HistSessionsRequest.Type = histSessionsReqType;
            m_HistSessionsRequest.Symbol = txtSymbol.Text;
                
            if (histSessionsReqType != FakeCQG.eHistoricalSessionsRequestType.hsrtAll)
            {
               m_HistSessionsRequest.RangeStart = dtpStartRange.Value;
            }

            if (histSessionsReqType == FakeCQG.eHistoricalSessionsRequestType.hsrtTimeRange)
            {
               m_HistSessionsRequest.RangeEnd = dtpEndRange.Value;

               if (m_HistSessionsRequest.RangeStart >= m_HistSessionsRequest.RangeEnd)
               {
                  MessageBox.Show("Range start must be earlier than range end.", "btnOK_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }
            }
                
            this.DialogResult = DialogResult.OK;
            this.Close();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistSesssionsRequestSettings", "btnOK_Click", ex);
         }
      }

      /// <summary>
      /// Occurs when the cancel button is clicked. Closes the request form.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void btnCancel_Click(object sender, System.EventArgs e)
      {
         try
         {
            this.Close();
         }
         catch (System.Exception ex)
         {
             modErrorHandler.ShowError("frmHistSesssionsRequestSettings", "btnCancel_Click", ex);
         }
      }

      /// <summary>
      /// Initializes the form.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void frmRequest_Load(System.Object sender, System.EventArgs e)
      {
         try
         {    
            RequestTypeChanged();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistSesssionsRequestSettings", "frmRequest_Load", ex);
         }
      }

      /// <summary>
      /// Calls the RequestTypeChanged sub to change the request type and controls the enablements
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void RequestTypeCheckedChanged(System.Object sender, System.EventArgs e)
      {
         try
         {
            RequestTypeChanged();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistSesssionsRequestSettings", "RequestTypeCheckedChanged", ex);
         }
      }

      /// <summary>
      /// Changes the request type and control enablements
      /// </summary>
      private void RequestTypeChanged()
      {
         try
         {
            const string ZERO_DATE = "01/01/1900 00:00";

            if (m_HistSessionsRequest != null)
            {
               dtpStartRange.Enabled = ! optAll.Checked;
               dtpEndRange.Enabled = optTimeRange.Checked;
                    
               if (dtpStartRange.Enabled)
               {
                  dtpStartRange.Value = m_LineTime.AddDays(-1);
               }
               else
               {
                  dtpStartRange.Text = ZERO_DATE;
               }
               if (dtpEndRange.Enabled)
               {
                  dtpEndRange.Value = m_LineTime;
               }
               else
               {
                  dtpEndRange.Text = ZERO_DATE;
               }
            }
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistSesssionsRequestSettings", "RequestTypeChanged", ex);
         }
      }

      /// <summary>
      /// Sets the new request reference
      /// </summary>
      /// <returns>
      /// New reference of CQGHistoricalSessionsRequest
      /// </returns>
      [property:System.CLSCompliant(false)]        
      public FakeCQG.CQGHistoricalSessionsRequest HistoricalSessionsRequest
      {
         set
         {
            m_HistSessionsRequest = value;
         }
      }
        
      /// <summary>
      /// Sets the new time
      /// </summary>
      /// <returns>
      /// New time value
      /// </returns>
      [property:System.CLSCompliant(false)]
      public DateTime LineTime
      {
         set
         {
            m_LineTime = value;
         }
      }
    }
}
