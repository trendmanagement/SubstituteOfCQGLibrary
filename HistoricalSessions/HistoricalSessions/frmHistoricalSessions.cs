using System;
using Microsoft.VisualBasic;
using System.Drawing;
using System.Windows.Forms;
using CQG = FakeCQG;

namespace HistoricalSessions
{
	/// <summary>
	/// Summary description for frmHistoricalSessions.
	/// </summary>
	public class frmHistoricalSessions : System.Windows.Forms.Form
	{
      internal System.Windows.Forms.Label lblRPRangeEnd;
      internal System.Windows.Forms.Label Label12;
      internal System.Windows.Forms.Label lblRPType;
      internal System.Windows.Forms.Label Label6;
      internal System.Windows.Forms.Label lblRPSymbol;
      internal System.Windows.Forms.Label Label4;
      internal System.Windows.Forms.Button btnRequest;
      internal System.Windows.Forms.Label lblDataConnection;
      internal System.Windows.Forms.PictureBox PictureBox3;
      internal System.Windows.Forms.PictureBox PictureBox1;
      internal System.Windows.Forms.LinkLabel llWeb;
      internal System.Windows.Forms.Label Label8;
      internal System.Windows.Forms.Label lblRPRangeStart;
      internal System.Windows.Forms.GroupBox gbRequestParams;
      internal System.Windows.Forms.Label lblError;
      internal System.Windows.Forms.GroupBox gbSessions;
      internal System.Windows.Forms.GroupBox GroupBox6;
      internal System.Windows.Forms.Label lblStatus;
      internal System.Windows.Forms.GroupBox gbHolidays;
      internal System.Windows.Forms.ListView lvHolidays;
      internal System.Windows.Forms.ColumnHeader EmptyColumn;
      internal System.Windows.Forms.ColumnHeader Index;
      internal System.Windows.Forms.ColumnHeader HolidayDate;
      internal System.Windows.Forms.ColumnHeader SessionMask;
      internal System.Windows.Forms.ColumnHeader IsDaily;
      internal System.Windows.Forms.ColumnHeader SessDescNumber;
      internal System.Windows.Forms.ColumnHeader SessDescStart;
      internal System.Windows.Forms.ColumnHeader SessDescEnd;
      internal System.Windows.Forms.ListView lvSessions;
      internal System.Windows.Forms.ColumnHeader ColumnHeader1;
      internal System.Windows.Forms.ColumnHeader ColumnHeader10;
      internal System.Windows.Forms.ColumnHeader ColumnHeader3;
      internal System.Windows.Forms.ColumnHeader ColumnHeader2;
      internal System.Windows.Forms.ColumnHeader ColumnHeader4;
      internal System.Windows.Forms.ColumnHeader ColumnHeader5;
      internal System.Windows.Forms.ColumnHeader ColumnHeader6;
      internal System.Windows.Forms.ColumnHeader ColumnHeader7;
      internal System.Windows.Forms.ColumnHeader ColumnHeader8;
      internal System.Windows.Forms.ColumnHeader ColumnHeader9;
      internal System.Windows.Forms.ColumnHeader ColumnHeader11;
      internal System.Windows.Forms.ColumnHeader ColumnHeader12;
      internal System.Windows.Forms.ColumnHeader columnHeader13;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmHistoricalSessions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
         this.gbRequestParams = new System.Windows.Forms.GroupBox();
         this.lblRPRangeStart = new System.Windows.Forms.Label();
         this.Label8 = new System.Windows.Forms.Label();
         this.lblRPRangeEnd = new System.Windows.Forms.Label();
         this.Label12 = new System.Windows.Forms.Label();
         this.lblRPType = new System.Windows.Forms.Label();
         this.Label6 = new System.Windows.Forms.Label();
         this.lblRPSymbol = new System.Windows.Forms.Label();
         this.Label4 = new System.Windows.Forms.Label();
         this.btnRequest = new System.Windows.Forms.Button();
         this.lblDataConnection = new System.Windows.Forms.Label();
         this.gbSessions = new System.Windows.Forms.GroupBox();
         this.lvSessions = new System.Windows.Forms.ListView();
         this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader3 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader4 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader5 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader6 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader7 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader8 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader9 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader10 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader11 = new System.Windows.Forms.ColumnHeader();
         this.ColumnHeader12 = new System.Windows.Forms.ColumnHeader();
         this.PictureBox3 = new System.Windows.Forms.PictureBox();
         this.PictureBox1 = new System.Windows.Forms.PictureBox();
         this.llWeb = new System.Windows.Forms.LinkLabel();
         this.lblError = new System.Windows.Forms.Label();
         this.GroupBox6 = new System.Windows.Forms.GroupBox();
         this.lblStatus = new System.Windows.Forms.Label();
         this.gbHolidays = new System.Windows.Forms.GroupBox();
         this.lvHolidays = new System.Windows.Forms.ListView();
         this.EmptyColumn = new System.Windows.Forms.ColumnHeader();
         this.Index = new System.Windows.Forms.ColumnHeader();
         this.HolidayDate = new System.Windows.Forms.ColumnHeader();
         this.SessionMask = new System.Windows.Forms.ColumnHeader();
         this.IsDaily = new System.Windows.Forms.ColumnHeader();
         this.SessDescNumber = new System.Windows.Forms.ColumnHeader();
         this.SessDescStart = new System.Windows.Forms.ColumnHeader();
         this.SessDescEnd = new System.Windows.Forms.ColumnHeader();
         this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
         this.gbRequestParams.SuspendLayout();
         this.gbSessions.SuspendLayout();
         this.GroupBox6.SuspendLayout();
         this.gbHolidays.SuspendLayout();
         this.SuspendLayout();
         // 
         // gbRequestParams
         // 
         this.gbRequestParams.Controls.Add(this.lblRPRangeStart);
         this.gbRequestParams.Controls.Add(this.Label8);
         this.gbRequestParams.Controls.Add(this.lblRPRangeEnd);
         this.gbRequestParams.Controls.Add(this.Label12);
         this.gbRequestParams.Controls.Add(this.lblRPType);
         this.gbRequestParams.Controls.Add(this.Label6);
         this.gbRequestParams.Controls.Add(this.lblRPSymbol);
         this.gbRequestParams.Controls.Add(this.Label4);
         this.gbRequestParams.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.gbRequestParams.Location = new System.Drawing.Point(528, 48);
         this.gbRequestParams.Name = "gbRequestParams";
         this.gbRequestParams.Size = new System.Drawing.Size(512, 120);
         this.gbRequestParams.TabIndex = 57;
         this.gbRequestParams.TabStop = false;
         this.gbRequestParams.Text = "Request Parameters";
         // 
         // lblRPRangeStart
         // 
         this.lblRPRangeStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRPRangeStart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblRPRangeStart.Location = new System.Drawing.Point(352, 32);
         this.lblRPRangeStart.Name = "lblRPRangeStart";
         this.lblRPRangeStart.Size = new System.Drawing.Size(136, 24);
         this.lblRPRangeStart.TabIndex = 43;
         this.lblRPRangeStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // Label8
         // 
         this.Label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label8.Location = new System.Drawing.Point(240, 32);
         this.Label8.Name = "Label8";
         this.Label8.Size = new System.Drawing.Size(104, 24);
         this.Label8.TabIndex = 42;
         this.Label8.Text = "Range Start :";
         this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblRPRangeEnd
         // 
         this.lblRPRangeEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRPRangeEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblRPRangeEnd.Location = new System.Drawing.Point(352, 72);
         this.lblRPRangeEnd.Name = "lblRPRangeEnd";
         this.lblRPRangeEnd.Size = new System.Drawing.Size(136, 24);
         this.lblRPRangeEnd.TabIndex = 41;
         this.lblRPRangeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // Label12
         // 
         this.Label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label12.Location = new System.Drawing.Point(256, 72);
         this.Label12.Name = "Label12";
         this.Label12.Size = new System.Drawing.Size(88, 24);
         this.Label12.TabIndex = 40;
         this.Label12.Text = "Range End :";
         this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblRPType
         // 
         this.lblRPType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRPType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblRPType.Location = new System.Drawing.Point(112, 72);
         this.lblRPType.Name = "lblRPType";
         this.lblRPType.Size = new System.Drawing.Size(120, 24);
         this.lblRPType.TabIndex = 35;
         this.lblRPType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // Label6
         // 
         this.Label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label6.Location = new System.Drawing.Point(40, 72);
         this.Label6.Name = "Label6";
         this.Label6.Size = new System.Drawing.Size(56, 24);
         this.Label6.TabIndex = 34;
         this.Label6.Text = "Type :";
         this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // lblRPSymbol
         // 
         this.lblRPSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblRPSymbol.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblRPSymbol.Location = new System.Drawing.Point(112, 32);
         this.lblRPSymbol.Name = "lblRPSymbol";
         this.lblRPSymbol.Size = new System.Drawing.Size(120, 24);
         this.lblRPSymbol.TabIndex = 33;
         this.lblRPSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // Label4
         // 
         this.Label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label4.Location = new System.Drawing.Point(16, 32);
         this.Label4.Name = "Label4";
         this.Label4.Size = new System.Drawing.Size(80, 24);
         this.Label4.TabIndex = 32;
         this.Label4.Text = "Symbol :";
         this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // btnRequest
         // 
         this.btnRequest.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.btnRequest.Location = new System.Drawing.Point(16, 56);
         this.btnRequest.Name = "btnRequest";
         this.btnRequest.Size = new System.Drawing.Size(232, 40);
         this.btnRequest.TabIndex = 48;
         this.btnRequest.Text = "Request Historical Sessions";
         this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
         // 
         // lblDataConnection
         // 
         this.lblDataConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblDataConnection.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblDataConnection.Location = new System.Drawing.Point(8, 8);
         this.lblDataConnection.Name = "lblDataConnection";
         this.lblDataConnection.Size = new System.Drawing.Size(216, 16);
         this.lblDataConnection.TabIndex = 55;
         this.lblDataConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // gbSessions
         // 
         this.gbSessions.Controls.Add(this.lvSessions);
         this.gbSessions.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.gbSessions.Location = new System.Drawing.Point(8, 200);
         this.gbSessions.Name = "gbSessions";
         this.gbSessions.Size = new System.Drawing.Size(1032, 288);
         this.gbSessions.TabIndex = 53;
         this.gbSessions.TabStop = false;
         this.gbSessions.Text = "Sessions";
         // 
         // lvSessions
         // 
         this.lvSessions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                     this.ColumnHeader1,
                                                                                     this.ColumnHeader2,
                                                                                     this.ColumnHeader3,
                                                                                     this.ColumnHeader4,
                                                                                     this.ColumnHeader5,
                                                                                     this.ColumnHeader6,
                                                                                     this.ColumnHeader7,
                                                                                     this.ColumnHeader8,
                                                                                     this.ColumnHeader9,
                                                                                     this.ColumnHeader10,
                                                                                     this.ColumnHeader11,
                                                                                     this.ColumnHeader12,
                                                                                     this.columnHeader13});
         this.lvSessions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lvSessions.FullRowSelect = true;
         this.lvSessions.GridLines = true;
         this.lvSessions.Location = new System.Drawing.Point(8, 24);
         this.lvSessions.MultiSelect = false;
         this.lvSessions.Name = "lvSessions";
         this.lvSessions.Size = new System.Drawing.Size(1016, 256);
         this.lvSessions.TabIndex = 5;
         this.lvSessions.View = System.Windows.Forms.View.Details;
         // 
         // ColumnHeader1
         // 
         this.ColumnHeader1.Text = "";
         this.ColumnHeader1.Width = 0;
         // 
         // ColumnHeader2
         // 
         this.ColumnHeader2.Text = "Index";
         this.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader2.Width = 48;
         // 
         // ColumnHeader3
         // 
         this.ColumnHeader3.Text = "Name";
         this.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader3.Width = 83;
         // 
         // ColumnHeader4
         // 
         this.ColumnHeader4.Text = "Number";
         this.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader4.Width = 59;
         // 
         // ColumnHeader5
         // 
         this.ColumnHeader5.Text = "ActivationDate";
         this.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader5.Width = 95;
         // 
         // ColumnHeader6
         // 
         this.ColumnHeader6.Text = "Start Time";
         this.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader6.Width = 83;
         // 
         // ColumnHeader7
         // 
         this.ColumnHeader7.Text = "End Time";
         this.ColumnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader7.Width = 94;
         // 
         // ColumnHeader8
         // 
         this.ColumnHeader8.Text = "Is Primary";
         this.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader8.Width = 73;
         // 
         // ColumnHeader9
         // 
         this.ColumnHeader9.Text = "Type";
         this.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader9.Width = 53;
         // 
         // ColumnHeader10
         // 
         this.ColumnHeader10.Text = "Working Week Days";
         this.ColumnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader10.Width = 132;
         // 
         // ColumnHeader11
         // 
         this.ColumnHeader11.Text = "DescriptionNumber";
         this.ColumnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader11.Width = 122;
         // 
         // ColumnHeader12
         // 
         this.ColumnHeader12.Text = "DescritpionStart";
         this.ColumnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.ColumnHeader12.Width = 128;
         // 
         // PictureBox3
         // 
         this.PictureBox3.BackColor = System.Drawing.Color.Black;
         this.PictureBox3.Location = new System.Drawing.Point(-96, 184);
         this.PictureBox3.Name = "PictureBox3";
         this.PictureBox3.Size = new System.Drawing.Size(1272, 3);
         this.PictureBox3.TabIndex = 59;
         this.PictureBox3.TabStop = false;
         // 
         // PictureBox1
         // 
         this.PictureBox1.BackColor = System.Drawing.Color.Black;
         this.PictureBox1.Location = new System.Drawing.Point(0, 40);
         this.PictureBox1.Name = "PictureBox1";
         this.PictureBox1.Size = new System.Drawing.Size(1272, 3);
         this.PictureBox1.TabIndex = 54;
         this.PictureBox1.TabStop = false;
         // 
         // llWeb
         // 
         this.llWeb.BackColor = System.Drawing.SystemColors.Control;
         this.llWeb.LinkArea = new System.Windows.Forms.LinkArea(18, 58);
         this.llWeb.Location = new System.Drawing.Point(632, 8);
         this.llWeb.Name = "llWeb";
         this.llWeb.Size = new System.Drawing.Size(408, 16);
         this.llWeb.TabIndex = 56;
         this.llWeb.TabStop = true;
         this.llWeb.Text = "CQG API web page: http://www.cqg.com/Products/CQG-API.aspx";
         this.llWeb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.llWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWeb_LinkClicked);
         // 
         // lblError
         // 
         this.lblError.ForeColor = System.Drawing.Color.Red;
         this.lblError.Location = new System.Drawing.Point(16, 120);
         this.lblError.Name = "lblError";
         this.lblError.Size = new System.Drawing.Size(488, 48);
         this.lblError.TabIndex = 60;
         // 
         // GroupBox6
         // 
         this.GroupBox6.Controls.Add(this.lblStatus);
         this.GroupBox6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
         this.GroupBox6.Location = new System.Drawing.Point(264, 48);
         this.GroupBox6.Name = "GroupBox6";
         this.GroupBox6.Size = new System.Drawing.Size(136, 48);
         this.GroupBox6.TabIndex = 61;
         this.GroupBox6.TabStop = false;
         this.GroupBox6.Text = "Status";
         // 
         // lblStatus
         // 
         this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.lblStatus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lblStatus.Location = new System.Drawing.Point(8, 16);
         this.lblStatus.Name = "lblStatus";
         this.lblStatus.Size = new System.Drawing.Size(120, 24);
         this.lblStatus.TabIndex = 31;
         this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // gbHolidays
         // 
         this.gbHolidays.Controls.Add(this.lvHolidays);
         this.gbHolidays.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.gbHolidays.Location = new System.Drawing.Point(8, 504);
         this.gbHolidays.Name = "gbHolidays";
         this.gbHolidays.Size = new System.Drawing.Size(1032, 272);
         this.gbHolidays.TabIndex = 62;
         this.gbHolidays.TabStop = false;
         this.gbHolidays.Text = "Holidays";
         // 
         // lvHolidays
         // 
         this.lvHolidays.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                     this.EmptyColumn,
                                                                                     this.Index,
                                                                                     this.HolidayDate,
                                                                                     this.SessionMask,
                                                                                     this.IsDaily,
                                                                                     this.SessDescNumber,
                                                                                     this.SessDescStart,
                                                                                     this.SessDescEnd});
         this.lvHolidays.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.lvHolidays.FullRowSelect = true;
         this.lvHolidays.GridLines = true;
         this.lvHolidays.Location = new System.Drawing.Point(8, 24);
         this.lvHolidays.MultiSelect = false;
         this.lvHolidays.Name = "lvHolidays";
         this.lvHolidays.Size = new System.Drawing.Size(1016, 240);
         this.lvHolidays.TabIndex = 0;
         this.lvHolidays.View = System.Windows.Forms.View.Details;
         // 
         // EmptyColumn
         // 
         this.EmptyColumn.Text = "";
         this.EmptyColumn.Width = 0;
         // 
         // Index
         // 
         this.Index.Text = "Index";
         this.Index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.Index.Width = 49;
         // 
         // HolidayDate
         // 
         this.HolidayDate.Text = "HolidayDate";
         this.HolidayDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.HolidayDate.Width = 138;
         // 
         // SessionMask
         // 
         this.SessionMask.Text = "SessionMask";
         this.SessionMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.SessionMask.Width = 145;
         // 
         // IsDaily
         // 
         this.IsDaily.Text = "IsDaily";
         this.IsDaily.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.IsDaily.Width = 78;
         // 
         // SessDescNumber
         // 
         this.SessDescNumber.Text = "SessDescNumber";
         this.SessDescNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.SessDescNumber.Width = 170;
         // 
         // SessDescStart
         // 
         this.SessDescStart.Text = "SessDescStart";
         this.SessDescStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.SessDescStart.Width = 212;
         // 
         // SessDescEnd
         // 
         this.SessDescEnd.Text = "SessDescEnd";
         this.SessDescEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.SessDescEnd.Width = 195;
         // 
         // columnHeader13
         // 
         this.columnHeader13.Text = "DescritpionEnd";
         this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.columnHeader13.Width = 128;
         // 
         // frmHistoricalSessions
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
         this.ClientSize = new System.Drawing.Size(1048, 787);
         this.Controls.Add(this.gbHolidays);
         this.Controls.Add(this.GroupBox6);
         this.Controls.Add(this.lblError);
         this.Controls.Add(this.gbRequestParams);
         this.Controls.Add(this.btnRequest);
         this.Controls.Add(this.lblDataConnection);
         this.Controls.Add(this.gbSessions);
         this.Controls.Add(this.PictureBox3);
         this.Controls.Add(this.PictureBox1);
         this.Controls.Add(this.llWeb);
         this.Font = new System.Drawing.Font("Arial", 9.75F);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
         this.MaximizeBox = false;
         this.Name = "frmHistoricalSessions";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "HistoricalSessions";
         this.Closing += new System.ComponentModel.CancelEventHandler(this.frmHistoricalSessions_Closing);
         this.Load += new System.EventHandler(this.frmHistoricalSessions_Load);
         this.gbRequestParams.ResumeLayout(false);
         this.gbSessions.ResumeLayout(false);
         this.GroupBox6.ResumeLayout(false);
         this.gbHolidays.ResumeLayout(false);
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmHistoricalSessions());
		}

      // The CQGCEL object, which encapsulates the main functionality of CQG API
      [field:System.CLSCompliant(false)]		
      public FakeCQG.CQGCELClass m_CEL;

      // Specifies the not available string
      private const string N_A = "N/A";

      // Specifies the dash string
      private const string sDash = "-";

      /// <summary>
      /// Creates a CEL object, changes its configurations, and starts up the created CEL object.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void frmHistoricalSessions_Load(System.Object sender, System.EventArgs e)
      {
         try
         {
            // Creates the CQGCEL object
            m_CEL = new FakeCQG.CQGCELClass();
            m_CEL.DataError += new CQG._ICQGCELEvents_DataErrorEventHandler(CEL_DataError);
            m_CEL.DataConnectionStatusChanged += new CQG._ICQGCELEvents_DataConnectionStatusChangedEventHandler(CEL_DataConnectionStatusChanged);
            m_CEL.HistoricalSessionsResolved += new CQG._ICQGCELEvents_HistoricalSessionsResolvedEventHandler(CEL_HistoricalSessionsResolved);
            //m_CEL.APIConfiguration.ReadyStatusCheck = FakeCQG.eReadyStatusCheck.rscOff;
            //m_CEL.APIConfiguration.CollectionsThrowException = false;
            //m_CEL.APIConfiguration.TimeZoneCode = FakeCQG.eTimeZone.tzCentral;
            // Disables the controls
            CEL_DataConnectionStatusChanged(FakeCQG.eConnectionStatus.csConnectionDown);
            // Starts up the CQGCEL
            m_CEL.Startup();
				
            ClearAllData();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "frmHistoricalSessions_Load", ex);
            this.Close();
         }
      }

      /// <summary>
      /// Shuts down CEL.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// A CancelEventArgs that contains the event data.
      /// </param>
      private void frmHistoricalSessions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         try
         {
            if (m_CEL != null)
            {
               m_CEL.Shutdown();
            }
         }
         catch (System.Exception ex)
         {
         	modErrorHandler.ShowError("frmHistoricalSessions", "frmHistoricalSessions_Closing", ex);
         }
      }

      /// <summary>
      /// Enables\disables the controls, based on the connection status.
      /// </summary>
      private void ChangeControlsStatuses()
      {
         try
         {
            btnRequest.Enabled = false;
            if (m_CEL.IsStarted)
            {
               btnRequest.Enabled = m_CEL.Environment.DataConnectionStatus == FakeCQG.eConnectionStatus.csConnectionUp;
            }
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "ChangeControlsStatuses", ex);
         }
      }

      /// <summary>
      /// Opens frmHistSessionsRequestSettings and with passed values requests HistoricalSessions.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// An EventArgs that contains the event data.
      /// </param>
      private void btnRequest_Click(object sender, System.EventArgs e)
      {
         try
         {
            frmHistSessionsRequestSettings frmHistSessionsReq;
            FakeCQG.CQGHistoricalSessionsRequest req;

            // Opens the form to fill up request data
            frmHistSessionsReq = new frmHistSessionsRequestSettings();
            req = m_CEL.CreateHistoricalSessionsRequest();

            frmHistSessionsReq.HistoricalSessionsRequest = req;
            frmHistSessionsReq.LineTime = m_CEL.Environment.LineTime;

            // If the user confirms, request is sent to CQGCEL and added to the requests list
            if (frmHistSessionsReq.ShowDialog(this) == DialogResult.OK)
            {
               m_CEL.RequestHistoricalSessions(req);

               lblStatus.Text = "InProgress";

               ClearAllData();
               ChangeControlsStatuses();
               DumpRequestParams(req);
            }
         }
         catch (System.Exception ex)
         {
         	modErrorHandler.ShowError("frmHistoricalSessions", "btnRequest_Click", ex);
         }
      }

      /// <summary>
      /// This event is fired, when CQGCEL detects some abnormal discrepancy between data expected and data received.
      /// </summary>
      /// <param name="cqg_error">
      /// The object, in which the error has occurred.
      /// </param>
      /// <param name="error_description">
      /// The string, describing the error.
      /// </param>
      private void CEL_DataError(object cqg_error, string error_description)
      {
         try
         {
            if (cqg_error is FakeCQG.CQGError)
            {
               FakeCQG.CQGError cqgErr = (FakeCQG.CQGError) cqg_error;
					
               if (cqgErr.Code == 102)
               {
                  error_description += " Restart the application.";
               }
               else if (cqgErr.Code == 125)
               {
                  error_description += " Turn on CQG Client and restart the application.";
               }
            }
				
            MessageBox.Show(error_description, "HistoricalSessions", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "CEL_DataError", ex);
         }
      }

        /// <summary>
        /// This event is fired, when some changes occur in the connection with CQG data server.
        /// </summary>
        /// <param name="newStatus">
        /// The current status of the connection with the data server.
        /// </param>
        /// 

        delegate void RefreshFormItemsCallback(System.Drawing.Color BackCol, string sInfo);
        private void RefreshFormItems(System.Drawing.Color BackCol, string sInfo)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lblDataConnection.InvokeRequired)
            {
                RefreshFormItemsCallback d = new RefreshFormItemsCallback(RefreshFormItems);
                Invoke(d, new object[] { BackCol, sInfo });
            }
            else
            {
                lblDataConnection.BackColor = BackCol;
                lblDataConnection.Text = sInfo;

                ChangeControlsStatuses();
            }
        }

        private void CEL_DataConnectionStatusChanged(CQG.eConnectionStatus new_status)
      {
         try
         {
            string sInfo;
            Color backCol;
				
            if (new_status == FakeCQG.eConnectionStatus.csConnectionUp)
            {
               backCol = Color.FromArgb(192, 209, 205);
               sInfo = "DATA Connection is UP";
            }
            else if (new_status == FakeCQG.eConnectionStatus.csConnectionDelayed)
            {
               backCol = Color.FromArgb(255, 114, 0);
               sInfo = "DATA Connection is Delayed";
            }
            else
            {
               backCol = Color.FromArgb(255, 114, 0);
               sInfo = "DATA Connection is Down";
            }

            RefreshFormItems(backCol, sInfo);

            }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "CEL_DataConnectionStatusChanged", ex);
         }
      }

      /// <summary>
      /// This event is fired when the historical sessions are resolved or
      /// when some error has occurred during the historical sessions request processing.
      /// </summary>
      /// <param name="sessColl">
      /// Reference to resolved CQGSessionsCollection
      /// </param>
      /// <param name="request">
      /// Reference to CQGHistoricalSessionsRequest
      /// </param>
      /// <param name="cqg_error">
      /// CQGError object describing the last error occurred during the historical sessions
      /// request processing or Nothing/Invalid_Error in case of no error.
      /// CQGCEL.IsValid(Invalid_Error) returns False.
      /// </param>
      private void CEL_HistoricalSessionsResolved(FakeCQG.CQGSessionsCollection sessColl,
                                                  FakeCQG.CQGHistoricalSessionsRequest request,
                                                  FakeCQG.CQGError error)
      {
         string status = (error != null) ? "Failed" : "Succeeded";

         // clear all records
         ClearRecords();

         if (error != null)
         {
            lblError.Text = error.Description;
         }

         lblStatus.Text = status;   
      
         // Dump all data
         DumpAllData(sessColl);
      }

      /// <summary>
      /// Dumps all request data (parameters\records)
      /// </summary>
      /// <param name="historicalSessions">
      /// historical sessions collection
      /// </param>
      private void DumpAllData(FakeCQG.CQGSessionsCollection historicalSessions)
      {
         try
         {
            long sessIndex = 1;
            long holidayIndex = 1;

	         // Dump all data
            foreach (FakeCQG.CQGSessions sessions in historicalSessions)
            {
               DumpSessions(sessions, sessIndex);
               DumpHolidays(sessions.Holidays, holidayIndex);

               // Count current indices for sessions and holidays
               sessIndex += sessions.Count;
               holidayIndex += sessions.Holidays.Count;
            }

            ChangeControlsStatuses();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "DumpAllData", ex);
         }
      }

      /// <summary>
      /// Shows the selected request parameters on the list view.
      /// </summary>
      /// <param name="histSessionsReq">
      /// Historical sessions request 
      /// </param>
      private void DumpRequestParams(FakeCQG.CQGHistoricalSessionsRequest histSessionsReq)
      {
         lblRPSymbol.Text = histSessionsReq.Symbol.ToString();
         lblRPType.Text = histSessionsReq.Type.ToString();
         lblRPRangeStart.Text = GetValueAsString(histSessionsReq.RangeStart, true);
         lblRPRangeEnd.Text = GetValueAsString(histSessionsReq.RangeEnd, true);
      }

      /// <summary>
      /// Shows request sessions on the list view.
      /// </summary>
      /// <param name="sessions">
      /// Historical sessions
      /// </param>
      /// <param name="index">
      /// Current index
      /// </param>
      private void DumpSessions(FakeCQG.CQGSessions sessions, long index)
      {
         foreach (FakeCQG.CQGSession session in sessions)
         {
            ListViewItem sessionItem = lvSessions.Items.Add(new ListViewItem());
            sessionItem.SubItems.Add("Index").Text = (index).ToString();
            sessionItem.SubItems.Add("Name").Text = session.Name;
            sessionItem.SubItems.Add("Number").Text = session.Number.ToString();
            sessionItem.SubItems.Add("Activation Date").Text = GetValueAsString(session.ActivationDate, true);
            sessionItem.SubItems.Add("Start Time").Text = session.StartTime.ToShortTimeString();
            sessionItem.SubItems.Add("End Time").Text = session.EndTime.ToShortTimeString();
            sessionItem.SubItems.Add("Is Primary").Text = session.PrimaryFlag ? "Yes" : "No";
            sessionItem.SubItems.Add("Type").Text = session.Type.ToString();
            sessionItem.SubItems.Add("Working Week Days").Text = GetSessionWorkingDays(session.WorkingWeekDays);
            sessionItem.SubItems.Add("Description Number").Text = sessions.DescriptionNumber.ToString();
            sessionItem.SubItems.Add("Description Start").Text = GetValueAsString(sessions.DescriptionStart, true);
            sessionItem.SubItems.Add("Description End").Text = GetValueAsString(sessions.DescriptionEnd, true);

            ++index;
         }
      }

      /// <summary>
      /// Shows request sessions holidays on the list view.
      /// </summary>
      /// <param name="holidays">
      /// Holidays
      /// </param>
      /// <param name="index">
      /// Current index
      /// </param>
      private void DumpHolidays(FakeCQG.CQGHolidays holidays, long index)
      {
         foreach (FakeCQG.CQGHoliday holiday in holidays)
         {
            ListViewItem item = lvHolidays.Items.Add(new ListViewItem());
            item.SubItems.Add("Index").Text = (index).ToString();
            item.SubItems.Add("HolidayDate").Text = GetValueAsString(holiday.HolidayDate, false);
            item.SubItems.Add("SessionMask").Text = "0x" + Conversion.Hex(holiday.SessionMask);
            item.SubItems.Add("IsDaily").Text = holiday.IsDaily ? "TRUE" : "FALSE";
            item.SubItems.Add("SessDescNumber").Text = holidays.Sessions.DescriptionNumber.ToString();
            item.SubItems.Add("SessDescStart").Text = GetValueAsString(holidays.Sessions.DescriptionStart, true);
            item.SubItems.Add("SessDescEnd").Text = GetValueAsString(holidays.Sessions.DescriptionEnd, true);
               
            ++index;
         }
      }

      /// <summary>
      /// Clears all request data.
      /// </summary>
      private void ClearAllData()
      {
         try
         {		
            ClearRequestParams();
            ClearRecords();
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "ClearAllData", ex);
         }
      }

      /// <summary>
      /// Clears the request parameters from the list view.
      /// </summary>
      private void ClearRequestParams()
      {
         try
         {
            lblRPSymbol.Text = string.Empty;
            lblRPType.Text = string.Empty;
            lblRPRangeStart.Text = string.Empty;
            lblRPRangeEnd.Text = string.Empty;
            
            lblError.Text = string.Empty;
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "ClearRequestParams", ex);
         }
      }

      /// <summary>
      /// Clears records from the list view.
      /// </summary>
      private void ClearRecords()
      {
         try
         {
            lvSessions.Items.Clear();
            lvHolidays.Items.Clear();
            lblError.Text = string.Empty;
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmHistoricalSessions", "ClearRecords", ex);
         }
      }

      /// <summary>
      /// Validates the specified value and converts it to string.
      /// </summary>
      /// <param name="val">
      /// The value that will be validated and converted
      /// </param>
      /// <param name="withTime">
      /// The time part presence in the string presentation of the date value
      /// </param>
      /// <returns>
      /// Value converted to string or "N/A" if the value is invalid.
      /// </returns>
      private string GetValueAsString(object val, bool withTime)
      {
         string sResult = N_A;
			
         try
         {
            if (m_CEL.IsValid(val))
            {
               if (val.GetType().FullName == "System.DateTime")
               {
                  if (withTime)
                  {
                     sResult = System.Convert.ToDateTime(val).ToString("g");
                  }
                  else
                  {
                     sResult = System.Convert.ToDateTime(val).ToString("d");
                  }
               }
               else
               {
                  sResult = val.ToString();
               }
            }
         }
         catch
         {
            sResult = N_A;
            return sResult;
         }
			
         return sResult;
      }

      /// <summary>
      /// Return the session's week working days in this format: "SMTWTFS".
      /// </summary>
      /// <param name="weekDay">
      /// eSessionWeekDays bitmask, which contains information about working days.
      /// </param>
      /// <returns>
      /// String, which contains working days.
      /// </returns>
      private static string GetSessionWorkingDays(FakeCQG.eSessionWeekDays weekDay)
      {
         string sResult;
         
         sResult = (((weekDay & FakeCQG.eSessionWeekDays.swdSunday) == FakeCQG.eSessionWeekDays.swdSunday) ? "S" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdMonday) == FakeCQG.eSessionWeekDays.swdMonday) ? "M" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdTuesday) == FakeCQG.eSessionWeekDays.swdTuesday) ? "T" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdWednesday) == FakeCQG.eSessionWeekDays.swdWednesday) ? "W" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdThursday) == FakeCQG.eSessionWeekDays.swdThursday) ? "T" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdFriday) == FakeCQG.eSessionWeekDays.swdFriday) ? "F" : sDash).ToString();
         sResult += (((weekDay & FakeCQG.eSessionWeekDays.swdSaturday) == FakeCQG.eSessionWeekDays.swdSaturday) ? "S" : sDash).ToString();
         
         return sResult;
      }

      /// <summary>
      /// Open CQG API web page.
      /// </summary>
      /// <param name="sender">
      /// The source of the event.
      /// </param>
      /// <param name="e">
      /// A LinkLabelLinkClickedEventArgs that contains the event data.
      /// </param>
      private void llWeb_LinkClicked(System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.cqg.com/Products/CQG-API.aspx");
         }
         catch (Exception ex)
         {
            modErrorHandler.ShowError("frmInstrumentProperties", "llWeb_LinkClicked", ex);
         }
      }
	}
}
