using System;
using System.Windows.Forms;
using CQG = FakeCQG;

namespace TimedBars
{
    public class frmTimedBars : System.Windows.Forms.Form
    {
        
        [STAThread]
        static void Main()
        {
            Application.Run(new frmTimedBars());
        }
        
        // The CQGCEL object, which encapsulates the main functionality of CQG API
        public CQG.CQGCEL CEL;
        
        // The currently selected TimedBars object in the combo box
        private CQG.CQGTimedBars m_CurTimedBars;
        
        // Specifies the not available string
        private const string N_A = "N/A";
        
        // Specifies the max records count
        private const int MAX_NUMBER_OF_RECORDS = 5;
        private System.Windows.Forms.ColumnHeader columnHeader29;
        private System.Windows.Forms.ColumnHeader columnHeader30;
        private System.Windows.Forms.ColumnHeader columnHeader31;
        private System.Windows.Forms.ColumnHeader columnHeader32;
        private System.Windows.Forms.ColumnHeader columnHeader33;
        internal System.Windows.Forms.ColumnHeader columnHeader35;
        internal System.Windows.Forms.ColumnHeader columnHeader36;
        internal System.Windows.Forms.ColumnHeader columnHeader37;
        internal System.Windows.Forms.ColumnHeader columnHeader38;
        internal System.Windows.Forms.ColumnHeader columnHeader34;
        internal System.Windows.Forms.Label lblRPTickFlter;
        internal System.Windows.Forms.Label label6;
        
        // Specifies the max index interval
        private const int MAX_INDEX_INTERVAL = 500;
        
        #region " Windows Form Designer generated code "
        
        public frmTimedBars()
        {
            
            //This call is required by the Windows Form Designer.
            InitializeComponent();
            
            //Add any initialization after the InitializeComponent() call
            
        }
        
        //Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!(components == null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        //Required by the Windows Form Designer
        private System.ComponentModel.Container components = null;
        
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.
        //Do not modify it using the code editor.
        internal System.Windows.Forms.Label lblDataConnection;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.LinkLabel llWeb;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.PictureBox PictureBox3;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Label Label14;
        internal System.Windows.Forms.Label Label18;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.Label Label16;
        internal System.Windows.Forms.Label Label20;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.ComboBox cmbTimedBars;
        internal System.Windows.Forms.Label lblRPSymbol;
        internal System.Windows.Forms.Label lblRPRangeEnd;
        internal System.Windows.Forms.Label lblRPRangeStart;
        internal System.Windows.Forms.Label lblOEnd;
        internal System.Windows.Forms.Label lblOStart;
        internal System.Windows.Forms.Label lblStatus;
        internal System.Windows.Forms.TextBox txtTagName;
        internal System.Windows.Forms.TextBox txtTagValue;
        internal System.Windows.Forms.Button btnRequest;
        internal System.Windows.Forms.Button btnRemove;
        internal System.Windows.Forms.Button btnRemoveAll;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox gbIndexInterval;
        internal System.Windows.Forms.GroupBox gbTimeInterval;
        internal System.Windows.Forms.DateTimePicker dtpStartRange;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Button btnShow;
        internal System.Windows.Forms.RadioButton optTimeInterval;
        internal System.Windows.Forms.RadioButton optIndexInterval;
        internal System.Windows.Forms.NumericUpDown nudEnd;
        internal System.Windows.Forms.NumericUpDown nudStart;
        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.DateTimePicker dtpEndRange;
        internal System.Windows.Forms.Label lblError;
        internal System.Windows.Forms.ListView lvShownTimedBars;
        internal System.Windows.Forms.ColumnHeader ColumnHeader13;
        internal System.Windows.Forms.ColumnHeader ColumnHeader14;
        internal System.Windows.Forms.ColumnHeader ColumnHeader15;
        internal System.Windows.Forms.ColumnHeader ColumnHeader16;
        internal System.Windows.Forms.ColumnHeader ColumnHeader17;
        internal System.Windows.Forms.ColumnHeader ColumnHeader18;
        internal System.Windows.Forms.ColumnHeader ColumnHeader19;
        internal System.Windows.Forms.ColumnHeader ColumnHeader20;
        internal System.Windows.Forms.ColumnHeader ColumnHeader21;
        internal System.Windows.Forms.ColumnHeader ColumnHeader22;
        internal System.Windows.Forms.ColumnHeader ColumnHeader23;
        internal System.Windows.Forms.ColumnHeader ColumnHeader24;
        internal System.Windows.Forms.ColumnHeader ColumnHeader25;
        internal System.Windows.Forms.ColumnHeader ColumnHeader26;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader2;
        internal System.Windows.Forms.ColumnHeader ColumnHeader3;
        internal System.Windows.Forms.ColumnHeader ColumnHeader4;
        internal System.Windows.Forms.ColumnHeader ColumnHeader5;
        internal System.Windows.Forms.ColumnHeader ColumnHeader6;
        internal System.Windows.Forms.ColumnHeader ColumnHeader7;
        internal System.Windows.Forms.ColumnHeader ColumnHeader8;
        internal System.Windows.Forms.ColumnHeader ColumnHeader9;
        internal System.Windows.Forms.ColumnHeader ColumnHeader10;
        internal System.Windows.Forms.ColumnHeader ColumnHeader11;
        internal System.Windows.Forms.ColumnHeader ColumnHeader12;
        internal System.Windows.Forms.ColumnHeader ColumnHeader27;
        internal System.Windows.Forms.ColumnHeader ColumnHeader28;
        internal System.Windows.Forms.ListView lvTimedBars;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.Label Label21;
        internal System.Windows.Forms.Label Label23;
        internal System.Windows.Forms.Label Label25;
        internal System.Windows.Forms.Label Label27;
        internal System.Windows.Forms.Label Label29;
        internal System.Windows.Forms.Label lblRPSessionFlags;
        internal System.Windows.Forms.Label lblRPHistoricalPeriod;
        internal System.Windows.Forms.Label lblRPIntradayPeriod;
        internal System.Windows.Forms.Label lblRPIncludeEnd;
        internal System.Windows.Forms.Label lblRPUpdatesEnabled;
        internal System.Windows.Forms.Label lblRPContinuation;
        internal System.Windows.Forms.Label lblRPSessionsFilter;
        internal System.Windows.Forms.Label lblRPDaysBeforeExpiration;
        internal System.Windows.Forms.Label lblRPEqualizeCloses;
        [System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
        {
            this.lblDataConnection = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.llWeb = new System.Windows.Forms.LinkLabel();
            this.cmbTimedBars = new System.Windows.Forms.ComboBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRPDaysBeforeExpiration = new System.Windows.Forms.Label();
            this.Label27 = new System.Windows.Forms.Label();
            this.lblRPEqualizeCloses = new System.Windows.Forms.Label();
            this.Label29 = new System.Windows.Forms.Label();
            this.lblRPContinuation = new System.Windows.Forms.Label();
            this.Label23 = new System.Windows.Forms.Label();
            this.lblRPSessionsFilter = new System.Windows.Forms.Label();
            this.Label25 = new System.Windows.Forms.Label();
            this.lblRPIncludeEnd = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.lblRPUpdatesEnabled = new System.Windows.Forms.Label();
            this.Label21 = new System.Windows.Forms.Label();
            this.lblRPHistoricalPeriod = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.lblRPIntradayPeriod = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.lblRPRangeEnd = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.lblRPSessionFlags = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.lblRPRangeStart = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.lblRPSymbol = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.PictureBox3 = new System.Windows.Forms.PictureBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTagValue = new System.Windows.Forms.TextBox();
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label18 = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.lblOEnd = new System.Windows.Forms.Label();
            this.Label16 = new System.Windows.Forms.Label();
            this.lblOStart = new System.Windows.Forms.Label();
            this.Label20 = new System.Windows.Forms.Label();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.lvTimedBars = new System.Windows.Forms.ListView();
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
            this.ColumnHeader27 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader28 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader33 = new System.Windows.Forms.ColumnHeader();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.lvShownTimedBars = new System.Windows.Forms.ListView();
            this.ColumnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader16 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader17 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader18 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader19 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader20 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader21 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader22 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader23 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader24 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader25 = new System.Windows.Forms.ColumnHeader();
            this.ColumnHeader26 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader36 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader37 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader38 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
            this.btnShow = new System.Windows.Forms.Button();
            this.gbIndexInterval = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.nudEnd = new System.Windows.Forms.NumericUpDown();
            this.Label2 = new System.Windows.Forms.Label();
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.gbTimeInterval = new System.Windows.Forms.GroupBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.dtpEndRange = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.dtpStartRange = new System.Windows.Forms.DateTimePicker();
            this.optTimeInterval = new System.Windows.Forms.RadioButton();
            this.optIndexInterval = new System.Windows.Forms.RadioButton();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.lblError = new System.Windows.Forms.Label();
            this.lblRPTickFlter = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.GroupBox5.SuspendLayout();
            this.gbIndexInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            this.gbTimeInterval.SuspendLayout();
            this.GroupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDataConnection
            // 
            this.lblDataConnection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDataConnection.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblDataConnection.Location = new System.Drawing.Point(8, 8);
            this.lblDataConnection.Name = "lblDataConnection";
            this.lblDataConnection.Size = new System.Drawing.Size(216, 16);
            this.lblDataConnection.TabIndex = 24;
            this.lblDataConnection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Black;
            this.PictureBox1.Location = new System.Drawing.Point(0, 40);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(1272, 3);
            this.PictureBox1.TabIndex = 23;
            this.PictureBox1.TabStop = false;
            // 
            // llWeb
            // 
            this.llWeb.BackColor = System.Drawing.SystemColors.Control;
            this.llWeb.LinkArea = new System.Windows.Forms.LinkArea(18, 58);
            this.llWeb.Location = new System.Drawing.Point(632, 8);
            this.llWeb.Name = "llWeb";
            this.llWeb.Size = new System.Drawing.Size(408, 16);
            this.llWeb.TabIndex = 25;
            this.llWeb.TabStop = true;
            this.llWeb.Text = "CQG API web page: http://www.cqg.com/Products/CQG-API.aspx";
            this.llWeb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWeb_LinkClicked);
            // 
            // cmbTimedBars
            // 
            this.cmbTimedBars.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimedBars.Location = new System.Drawing.Point(24, 56);
            this.cmbTimedBars.Name = "cmbTimedBars";
            this.cmbTimedBars.Size = new System.Drawing.Size(448, 24);
            this.cmbTimedBars.TabIndex = 0;
            this.cmbTimedBars.SelectedIndexChanged += new System.EventHandler(this.cmbTimedBars_SelectedIndexChanged);
            // 
            // btnRequest
            // 
            this.btnRequest.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.btnRequest.Location = new System.Drawing.Point(24, 88);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(96, 40);
            this.btnRequest.TabIndex = 1;
            this.btnRequest.Text = "Request";
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.btnRemove.Location = new System.Drawing.Point(128, 88);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(96, 40);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Enabled = false;
            this.btnRemoveAll.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.btnRemoveAll.Location = new System.Drawing.Point(232, 88);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(96, 40);
            this.btnRemoveAll.TabIndex = 3;
            this.btnRemoveAll.Text = "Remove All";
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
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
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.lblRPDaysBeforeExpiration);
            this.GroupBox1.Controls.Add(this.Label27);
            this.GroupBox1.Controls.Add(this.lblRPEqualizeCloses);
            this.GroupBox1.Controls.Add(this.Label29);
            this.GroupBox1.Controls.Add(this.lblRPContinuation);
            this.GroupBox1.Controls.Add(this.Label23);
            this.GroupBox1.Controls.Add(this.lblRPSessionsFilter);
            this.GroupBox1.Controls.Add(this.Label25);
            this.GroupBox1.Controls.Add(this.lblRPIncludeEnd);
            this.GroupBox1.Controls.Add(this.Label17);
            this.GroupBox1.Controls.Add(this.lblRPUpdatesEnabled);
            this.GroupBox1.Controls.Add(this.Label21);
            this.GroupBox1.Controls.Add(this.lblRPHistoricalPeriod);
            this.GroupBox1.Controls.Add(this.Label9);
            this.GroupBox1.Controls.Add(this.lblRPIntradayPeriod);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.lblRPRangeEnd);
            this.GroupBox1.Controls.Add(this.Label12);
            this.GroupBox1.Controls.Add(this.lblRPSessionFlags);
            this.GroupBox1.Controls.Add(this.Label10);
            this.GroupBox1.Controls.Add(this.lblRPRangeStart);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.lblRPSymbol);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.lblRPTickFlter);
            this.GroupBox1.Controls.Add(this.label6);
            this.GroupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.GroupBox1.Location = new System.Drawing.Point(488, 48);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(552, 248);
            this.GroupBox1.TabIndex = 32;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Request Parameters";
            // 
            // lblRPDaysBeforeExpiration
            // 
            this.lblRPDaysBeforeExpiration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPDaysBeforeExpiration.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPDaysBeforeExpiration.Location = new System.Drawing.Point(400, 184);
            this.lblRPDaysBeforeExpiration.Name = "lblRPDaysBeforeExpiration";
            this.lblRPDaysBeforeExpiration.Size = new System.Drawing.Size(136, 24);
            this.lblRPDaysBeforeExpiration.TabIndex = 57;
            this.lblRPDaysBeforeExpiration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label27
            // 
            this.Label27.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label27.Location = new System.Drawing.Point(264, 184);
            this.Label27.Name = "Label27";
            this.Label27.Size = new System.Drawing.Size(128, 24);
            this.Label27.TabIndex = 56;
            this.Label27.Text = "Days Before Exp. :";
            this.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPEqualizeCloses
            // 
            this.lblRPEqualizeCloses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPEqualizeCloses.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPEqualizeCloses.Location = new System.Drawing.Point(136, 184);
            this.lblRPEqualizeCloses.Name = "lblRPEqualizeCloses";
            this.lblRPEqualizeCloses.Size = new System.Drawing.Size(128, 24);
            this.lblRPEqualizeCloses.TabIndex = 55;
            this.lblRPEqualizeCloses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label29
            // 
            this.Label29.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label29.Location = new System.Drawing.Point(8, 184);
            this.Label29.Name = "Label29";
            this.Label29.Size = new System.Drawing.Size(120, 24);
            this.Label29.TabIndex = 54;
            this.Label29.Text = "Equalize Closes :";
            this.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPContinuation
            // 
            this.lblRPContinuation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPContinuation.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPContinuation.Location = new System.Drawing.Point(400, 152);
            this.lblRPContinuation.Name = "lblRPContinuation";
            this.lblRPContinuation.Size = new System.Drawing.Size(136, 24);
            this.lblRPContinuation.TabIndex = 53;
            this.lblRPContinuation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label23
            // 
            this.Label23.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label23.Location = new System.Drawing.Point(296, 152);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(96, 24);
            this.Label23.TabIndex = 52;
            this.Label23.Text = "Continuation :";
            this.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPSessionsFilter
            // 
            this.lblRPSessionsFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPSessionsFilter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPSessionsFilter.Location = new System.Drawing.Point(136, 216);
            this.lblRPSessionsFilter.Name = "lblRPSessionsFilter";
            this.lblRPSessionsFilter.Size = new System.Drawing.Size(400, 24);
            this.lblRPSessionsFilter.TabIndex = 51;
            this.lblRPSessionsFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label25
            // 
            this.Label25.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label25.Location = new System.Drawing.Point(16, 216);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(112, 24);
            this.Label25.TabIndex = 50;
            this.Label25.Text = "Sessions Filter :";
            this.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPIncludeEnd
            // 
            this.lblRPIncludeEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPIncludeEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPIncludeEnd.Location = new System.Drawing.Point(400, 120);
            this.lblRPIncludeEnd.Name = "lblRPIncludeEnd";
            this.lblRPIncludeEnd.Size = new System.Drawing.Size(136, 24);
            this.lblRPIncludeEnd.TabIndex = 49;
            this.lblRPIncludeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label17
            // 
            this.Label17.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label17.Location = new System.Drawing.Point(296, 120);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(96, 24);
            this.Label17.TabIndex = 48;
            this.Label17.Text = "Include End :";
            this.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPUpdatesEnabled
            // 
            this.lblRPUpdatesEnabled.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPUpdatesEnabled.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPUpdatesEnabled.Location = new System.Drawing.Point(136, 120);
            this.lblRPUpdatesEnabled.Name = "lblRPUpdatesEnabled";
            this.lblRPUpdatesEnabled.Size = new System.Drawing.Size(128, 24);
            this.lblRPUpdatesEnabled.TabIndex = 47;
            this.lblRPUpdatesEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label21
            // 
            this.Label21.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label21.Location = new System.Drawing.Point(4, 120);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(124, 24);
            this.Label21.TabIndex = 46;
            this.Label21.Text = "Updates Enabled :";
            this.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPHistoricalPeriod
            // 
            this.lblRPHistoricalPeriod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPHistoricalPeriod.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPHistoricalPeriod.Location = new System.Drawing.Point(400, 88);
            this.lblRPHistoricalPeriod.Name = "lblRPHistoricalPeriod";
            this.lblRPHistoricalPeriod.Size = new System.Drawing.Size(136, 24);
            this.lblRPHistoricalPeriod.TabIndex = 45;
            this.lblRPHistoricalPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label9
            // 
            this.Label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label9.Location = new System.Drawing.Point(272, 88);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(120, 24);
            this.Label9.TabIndex = 44;
            this.Label9.Text = "Historical Period :";
            this.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPIntradayPeriod
            // 
            this.lblRPIntradayPeriod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPIntradayPeriod.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPIntradayPeriod.Location = new System.Drawing.Point(136, 88);
            this.lblRPIntradayPeriod.Name = "lblRPIntradayPeriod";
            this.lblRPIntradayPeriod.Size = new System.Drawing.Size(128, 24);
            this.lblRPIntradayPeriod.TabIndex = 43;
            this.lblRPIntradayPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label13
            // 
            this.Label13.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label13.Location = new System.Drawing.Point(8, 88);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(120, 24);
            this.Label13.TabIndex = 42;
            this.Label13.Text = "Intraday Period :";
            this.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPRangeEnd
            // 
            this.lblRPRangeEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPRangeEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPRangeEnd.Location = new System.Drawing.Point(400, 56);
            this.lblRPRangeEnd.Name = "lblRPRangeEnd";
            this.lblRPRangeEnd.Size = new System.Drawing.Size(136, 24);
            this.lblRPRangeEnd.TabIndex = 41;
            this.lblRPRangeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label12
            // 
            this.Label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label12.Location = new System.Drawing.Point(304, 56);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(88, 24);
            this.Label12.TabIndex = 40;
            this.Label12.Text = "Range End :";
            this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPSessionFlags
            // 
            this.lblRPSessionFlags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPSessionFlags.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPSessionFlags.Location = new System.Drawing.Point(136, 152);
            this.lblRPSessionFlags.Name = "lblRPSessionFlags";
            this.lblRPSessionFlags.Size = new System.Drawing.Size(128, 24);
            this.lblRPSessionFlags.TabIndex = 39;
            this.lblRPSessionFlags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label10
            // 
            this.Label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label10.Location = new System.Drawing.Point(16, 152);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(112, 24);
            this.Label10.TabIndex = 38;
            this.Label10.Text = "Session Flags :";
            this.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPRangeStart
            // 
            this.lblRPRangeStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPRangeStart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPRangeStart.Location = new System.Drawing.Point(136, 56);
            this.lblRPRangeStart.Name = "lblRPRangeStart";
            this.lblRPRangeStart.Size = new System.Drawing.Size(128, 24);
            this.lblRPRangeStart.TabIndex = 37;
            this.lblRPRangeStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label8
            // 
            this.Label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label8.Location = new System.Drawing.Point(8, 56);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(120, 24);
            this.Label8.TabIndex = 36;
            this.Label8.Text = "Range Start :";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRPSymbol
            // 
            this.lblRPSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPSymbol.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPSymbol.Location = new System.Drawing.Point(136, 24);
            this.lblRPSymbol.Name = "lblRPSymbol";
            this.lblRPSymbol.Size = new System.Drawing.Size(128, 24);
            this.lblRPSymbol.TabIndex = 33;
            this.lblRPSymbol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label4.Location = new System.Drawing.Point(48, 24);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(80, 24);
            this.Label4.TabIndex = 32;
            this.Label4.Text = "Symbol :";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackColor = System.Drawing.Color.Black;
            this.PictureBox2.Location = new System.Drawing.Point(-96, 304);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(1272, 1);
            this.PictureBox2.TabIndex = 33;
            this.PictureBox2.TabStop = false;
            // 
            // PictureBox3
            // 
            this.PictureBox3.BackColor = System.Drawing.Color.Black;
            this.PictureBox3.Location = new System.Drawing.Point(-96, 384);
            this.PictureBox3.Name = "PictureBox3";
            this.PictureBox3.Size = new System.Drawing.Size(1272, 3);
            this.PictureBox3.TabIndex = 34;
            this.PictureBox3.TabStop = false;
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtTagValue);
            this.GroupBox2.Controls.Add(this.txtTagName);
            this.GroupBox2.Controls.Add(this.Label14);
            this.GroupBox2.Controls.Add(this.Label18);
            this.GroupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.GroupBox2.Location = new System.Drawing.Point(8, 312);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(472, 59);
            this.GroupBox2.TabIndex = 4;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Tags";
            // 
            // txtTagValue
            // 
            this.txtTagValue.Location = new System.Drawing.Point(312, 24);
            this.txtTagValue.Name = "txtTagValue";
            this.txtTagValue.Size = new System.Drawing.Size(144, 25);
            this.txtTagValue.TabIndex = 1;
            this.txtTagValue.Text = "";
            this.txtTagValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTagValue_KeyDown);
            // 
            // txtTagName
            // 
            this.txtTagName.Location = new System.Drawing.Point(72, 24);
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.Size = new System.Drawing.Size(144, 25);
            this.txtTagName.TabIndex = 0;
            this.txtTagName.Text = "";
            this.txtTagName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTagName_KeyDown);
            // 
            // Label14
            // 
            this.Label14.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label14.Location = new System.Drawing.Point(248, 24);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(56, 24);
            this.Label14.TabIndex = 40;
            this.Label14.Text = "Value :";
            this.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label18
            // 
            this.Label18.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label18.Location = new System.Drawing.Point(8, 24);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(56, 24);
            this.Label18.TabIndex = 36;
            this.Label18.Text = "Name :";
            this.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.lblOEnd);
            this.GroupBox3.Controls.Add(this.Label16);
            this.GroupBox3.Controls.Add(this.lblOStart);
            this.GroupBox3.Controls.Add(this.Label20);
            this.GroupBox3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.GroupBox3.Location = new System.Drawing.Point(488, 312);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(552, 59);
            this.GroupBox3.TabIndex = 42;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Output Parameters";
            // 
            // lblOEnd
            // 
            this.lblOEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblOEnd.Location = new System.Drawing.Point(400, 24);
            this.lblOEnd.Name = "lblOEnd";
            this.lblOEnd.Size = new System.Drawing.Size(136, 24);
            this.lblOEnd.TabIndex = 41;
            this.lblOEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label16
            // 
            this.Label16.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label16.Location = new System.Drawing.Point(320, 24);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(72, 24);
            this.Label16.TabIndex = 40;
            this.Label16.Text = "End :";
            this.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOStart
            // 
            this.lblOStart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOStart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblOStart.Location = new System.Drawing.Point(136, 24);
            this.lblOStart.Name = "lblOStart";
            this.lblOStart.Size = new System.Drawing.Size(128, 24);
            this.lblOStart.TabIndex = 37;
            this.lblOStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Label20
            // 
            this.Label20.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label20.Location = new System.Drawing.Point(56, 24);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(72, 24);
            this.Label20.TabIndex = 36;
            this.Label20.Text = "Start :";
            this.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.lvTimedBars);
            this.GroupBox4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.GroupBox4.Location = new System.Drawing.Point(8, 392);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(1032, 136);
            this.GroupBox4.TabIndex = 5;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Last 5 Resolved TimedBars";
            // 
            // lvTimedBars
            // 
            this.lvTimedBars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
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
                                                                                          this.ColumnHeader27,
                                                                                          this.ColumnHeader28,
                                                                                          this.columnHeader29,
                                                                                          this.columnHeader30,
                                                                                          this.columnHeader31,
                                                                                          this.columnHeader32,
                                                                                          this.columnHeader33});
            this.lvTimedBars.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lvTimedBars.FullRowSelect = true;
            this.lvTimedBars.GridLines = true;
            this.lvTimedBars.Location = new System.Drawing.Point(8, 20);
            this.lvTimedBars.MultiSelect = false;
            this.lvTimedBars.Name = "lvTimedBars";
            this.lvTimedBars.Size = new System.Drawing.Size(1016, 108);
            this.lvTimedBars.TabIndex = 7;
            this.lvTimedBars.View = System.Windows.Forms.View.Details;
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
            this.ColumnHeader2.Width = 49;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Timestamp";
            this.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader3.Width = 112;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Open";
            this.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader4.Width = 75;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "High";
            this.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader5.Width = 75;
            // 
            // ColumnHeader6
            // 
            this.ColumnHeader6.Text = "Low";
            this.ColumnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader6.Width = 75;
            // 
            // ColumnHeader7
            // 
            this.ColumnHeader7.Text = "Close";
            this.ColumnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader7.Width = 75;
            // 
            // ColumnHeader8
            // 
            this.ColumnHeader8.Text = "Mid";
            this.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader8.Width = 75;
            // 
            // ColumnHeader9
            // 
            this.ColumnHeader9.Text = "HLC3";
            this.ColumnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader9.Width = 75;
            // 
            // ColumnHeader10
            // 
            this.ColumnHeader10.Text = "Avg";
            this.ColumnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader10.Width = 75;
            // 
            // ColumnHeader11
            // 
            this.ColumnHeader11.Text = "TrueHigh";
            this.ColumnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader11.Width = 75;
            // 
            // ColumnHeader12
            // 
            this.ColumnHeader12.Text = "TrueLow";
            this.ColumnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader12.Width = 75;
            // 
            // ColumnHeader27
            // 
            this.ColumnHeader27.Text = "Range";
            this.ColumnHeader27.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader27.Width = 75;
            // 
            // ColumnHeader28
            // 
            this.ColumnHeader28.Text = "TrueRange";
            this.ColumnHeader28.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader28.Width = 75;
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "OpenInterest";
            this.columnHeader29.Width = 75;
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "ActualVolume";
            this.columnHeader30.Width = 75;
            // 
            // columnHeader31
            // 
            this.columnHeader31.Text = "TickVolume";
            this.columnHeader31.Width = 75;
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "AskVolume";
            this.columnHeader32.Width = 75;
            // 
            // columnHeader33
            // 
            this.columnHeader33.Text = "BidVolume";
            this.columnHeader33.Width = 75;
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.lvShownTimedBars);
            this.GroupBox5.Controls.Add(this.btnShow);
            this.GroupBox5.Controls.Add(this.gbIndexInterval);
            this.GroupBox5.Controls.Add(this.gbTimeInterval);
            this.GroupBox5.Controls.Add(this.optTimeInterval);
            this.GroupBox5.Controls.Add(this.optIndexInterval);
            this.GroupBox5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.GroupBox5.Location = new System.Drawing.Point(8, 528);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(1032, 216);
            this.GroupBox5.TabIndex = 6;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "TimedBars Filtered by Index or Time Interval";
            // 
            // lvShownTimedBars
            // 
            this.lvShownTimedBars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                               this.ColumnHeader13,
                                                                                               this.ColumnHeader14,
                                                                                               this.ColumnHeader15,
                                                                                               this.ColumnHeader16,
                                                                                               this.ColumnHeader17,
                                                                                               this.ColumnHeader18,
                                                                                               this.ColumnHeader19,
                                                                                               this.ColumnHeader20,
                                                                                               this.ColumnHeader21,
                                                                                               this.ColumnHeader22,
                                                                                               this.ColumnHeader23,
                                                                                               this.ColumnHeader24,
                                                                                               this.ColumnHeader25,
                                                                                               this.ColumnHeader26,
                                                                                               this.columnHeader35,
                                                                                               this.columnHeader36,
                                                                                               this.columnHeader37,
                                                                                               this.columnHeader38,
                                                                                               this.columnHeader34});
            this.lvShownTimedBars.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lvShownTimedBars.FullRowSelect = true;
            this.lvShownTimedBars.GridLines = true;
            this.lvShownTimedBars.Location = new System.Drawing.Point(8, 80);
            this.lvShownTimedBars.MultiSelect = false;
            this.lvShownTimedBars.Name = "lvShownTimedBars";
            this.lvShownTimedBars.Size = new System.Drawing.Size(1016, 128);
            this.lvShownTimedBars.TabIndex = 6;
            this.lvShownTimedBars.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader13
            // 
            this.ColumnHeader13.Text = "";
            this.ColumnHeader13.Width = 0;
            // 
            // ColumnHeader14
            // 
            this.ColumnHeader14.Text = "Index";
            this.ColumnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader14.Width = 49;
            // 
            // ColumnHeader15
            // 
            this.ColumnHeader15.Text = "Timestamp";
            this.ColumnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader15.Width = 112;
            // 
            // ColumnHeader16
            // 
            this.ColumnHeader16.Text = "Open";
            this.ColumnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader16.Width = 75;
            // 
            // ColumnHeader17
            // 
            this.ColumnHeader17.Text = "High";
            this.ColumnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader17.Width = 75;
            // 
            // ColumnHeader18
            // 
            this.ColumnHeader18.Text = "Low";
            this.ColumnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader18.Width = 75;
            // 
            // ColumnHeader19
            // 
            this.ColumnHeader19.Text = "Close";
            this.ColumnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader19.Width = 75;
            // 
            // ColumnHeader20
            // 
            this.ColumnHeader20.Text = "Mid";
            this.ColumnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader20.Width = 75;
            // 
            // ColumnHeader21
            // 
            this.ColumnHeader21.Text = "HLC3";
            this.ColumnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader21.Width = 75;
            // 
            // ColumnHeader22
            // 
            this.ColumnHeader22.Text = "Avg";
            this.ColumnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader22.Width = 75;
            // 
            // ColumnHeader23
            // 
            this.ColumnHeader23.Text = "TrueHigh";
            this.ColumnHeader23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader23.Width = 75;
            // 
            // ColumnHeader24
            // 
            this.ColumnHeader24.Text = "TrueLow";
            this.ColumnHeader24.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader24.Width = 75;
            // 
            // ColumnHeader25
            // 
            this.ColumnHeader25.Text = "Range";
            this.ColumnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader25.Width = 75;
            // 
            // ColumnHeader26
            // 
            this.ColumnHeader26.Text = "TrueRange";
            this.ColumnHeader26.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader26.Width = 75;
            // 
            // columnHeader35
            // 
            this.columnHeader35.Text = "OpenInterest";
            this.columnHeader35.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader35.Width = 75;
            // 
            // columnHeader36
            // 
            this.columnHeader36.Text = "ActualVolume";
            this.columnHeader36.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader36.Width = 75;
            // 
            // columnHeader37
            // 
            this.columnHeader37.Text = "TickVolume";
            this.columnHeader37.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader37.Width = 75;
            // 
            // columnHeader38
            // 
            this.columnHeader38.Text = "AskVolume";
            this.columnHeader38.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader38.Width = 75;
            // 
            // columnHeader34
            // 
            this.columnHeader34.Text = "BidVolume";
            this.columnHeader34.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader34.Width = 75;
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.btnShow.Location = new System.Drawing.Point(760, 48);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(96, 24);
            this.btnShow.TabIndex = 4;
            this.btnShow.Text = "Show";
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // gbIndexInterval
            // 
            this.gbIndexInterval.Controls.Add(this.Label3);
            this.gbIndexInterval.Controls.Add(this.nudEnd);
            this.gbIndexInterval.Controls.Add(this.Label2);
            this.gbIndexInterval.Controls.Add(this.nudStart);
            this.gbIndexInterval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.gbIndexInterval.Location = new System.Drawing.Point(352, 16);
            this.gbIndexInterval.Name = "gbIndexInterval";
            this.gbIndexInterval.Size = new System.Drawing.Size(400, 56);
            this.gbIndexInterval.TabIndex = 3;
            this.gbIndexInterval.TabStop = false;
            this.gbIndexInterval.Text = "Index Interval";
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label3.Location = new System.Drawing.Point(200, 24);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(48, 24);
            this.Label3.TabIndex = 74;
            this.Label3.Text = "End: ";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudEnd
            // 
            this.nudEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.nudEnd.Location = new System.Drawing.Point(256, 24);
            this.nudEnd.Maximum = new System.Decimal(new int[] {
                                                                   10000000,
                                                                   0,
                                                                   0,
                                                                   0});
            this.nudEnd.Minimum = new System.Decimal(new int[] {
                                                                   1,
                                                                   0,
                                                                   0,
                                                                   0});
            this.nudEnd.Name = "nudEnd";
            this.nudEnd.Size = new System.Drawing.Size(128, 22);
            this.nudEnd.TabIndex = 1;
            this.nudEnd.Value = new System.Decimal(new int[] {
                                                                 1,
                                                                 0,
                                                                 0,
                                                                 0});
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label2.Location = new System.Drawing.Point(8, 24);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(48, 24);
            this.Label2.TabIndex = 70;
            this.Label2.Text = "Start: ";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudStart
            // 
            this.nudStart.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.nudStart.Location = new System.Drawing.Point(64, 24);
            this.nudStart.Maximum = new System.Decimal(new int[] {
                                                                     10000000,
                                                                     0,
                                                                     0,
                                                                     0});
            this.nudStart.Minimum = new System.Decimal(new int[] {
                                                                     1,
                                                                     0,
                                                                     0,
                                                                     0});
            this.nudStart.Name = "nudStart";
            this.nudStart.Size = new System.Drawing.Size(128, 22);
            this.nudStart.TabIndex = 0;
            this.nudStart.Value = new System.Decimal(new int[] {
                                                                   1,
                                                                   0,
                                                                   0,
                                                                   0});
            // 
            // gbTimeInterval
            // 
            this.gbTimeInterval.Controls.Add(this.Label7);
            this.gbTimeInterval.Controls.Add(this.dtpEndRange);
            this.gbTimeInterval.Controls.Add(this.Label5);
            this.gbTimeInterval.Controls.Add(this.dtpStartRange);
            this.gbTimeInterval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.gbTimeInterval.Location = new System.Drawing.Point(352, 16);
            this.gbTimeInterval.Name = "gbTimeInterval";
            this.gbTimeInterval.Size = new System.Drawing.Size(400, 56);
            this.gbTimeInterval.TabIndex = 2;
            this.gbTimeInterval.TabStop = false;
            this.gbTimeInterval.Text = "Time Interval";
            // 
            // Label7
            // 
            this.Label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label7.Location = new System.Drawing.Point(200, 24);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(48, 24);
            this.Label7.TabIndex = 73;
            this.Label7.Text = "End: ";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndRange
            // 
            this.dtpEndRange.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpEndRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.dtpEndRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndRange.Location = new System.Drawing.Point(256, 24);
            this.dtpEndRange.Name = "dtpEndRange";
            this.dtpEndRange.Size = new System.Drawing.Size(128, 22);
            this.dtpEndRange.TabIndex = 1;
            // 
            // Label5
            // 
            this.Label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.Label5.Location = new System.Drawing.Point(8, 24);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(48, 24);
            this.Label5.TabIndex = 71;
            this.Label5.Text = "Start: ";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartRange
            // 
            this.dtpStartRange.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpStartRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.dtpStartRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartRange.Location = new System.Drawing.Point(64, 24);
            this.dtpStartRange.Name = "dtpStartRange";
            this.dtpStartRange.Size = new System.Drawing.Size(128, 22);
            this.dtpStartRange.TabIndex = 0;
            // 
            // optTimeInterval
            // 
            this.optTimeInterval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.optTimeInterval.Location = new System.Drawing.Point(200, 48);
            this.optTimeInterval.Name = "optTimeInterval";
            this.optTimeInterval.Size = new System.Drawing.Size(168, 24);
            this.optTimeInterval.TabIndex = 1;
            this.optTimeInterval.TabStop = true;
            this.optTimeInterval.Text = "Show by Time Interval";
            this.optTimeInterval.CheckedChanged += new System.EventHandler(this.optIndexOrTimeInterval_CheckedChanged);
            // 
            // optIndexInterval
            // 
            this.optIndexInterval.Checked = true;
            this.optIndexInterval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.optIndexInterval.Location = new System.Drawing.Point(200, 24);
            this.optIndexInterval.Name = "optIndexInterval";
            this.optIndexInterval.Size = new System.Drawing.Size(168, 24);
            this.optIndexInterval.TabIndex = 0;
            this.optIndexInterval.TabStop = true;
            this.optIndexInterval.Text = "Show by Index Interval";
            this.optIndexInterval.CheckedChanged += new System.EventHandler(this.optIndexOrTimeInterval_CheckedChanged);
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.lblStatus);
            this.GroupBox6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.GroupBox6.Location = new System.Drawing.Point(336, 80);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(136, 48);
            this.GroupBox6.TabIndex = 45;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "Status";
            // 
            // lblError
            // 
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(24, 136);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(448, 160);
            this.lblError.TabIndex = 46;
            // 
            // lblRPTickFlter
            // 
            this.lblRPTickFlter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRPTickFlter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.lblRPTickFlter.Location = new System.Drawing.Point(400, 24);
            this.lblRPTickFlter.Name = "lblRPTickFlter";
            this.lblRPTickFlter.Size = new System.Drawing.Size(136, 24);
            this.lblRPTickFlter.TabIndex = 33;
            this.lblRPTickFlter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.label6.Location = new System.Drawing.Point(312, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 24);
            this.label6.TabIndex = 32;
            this.label6.Text = "Tick Flter :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmTimedBars
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1048, 748);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.GroupBox6);
            this.Controls.Add(this.GroupBox5);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.PictureBox3);
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.cmbTimedBars);
            this.Controls.Add(this.lblDataConnection);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.llWeb);
            this.Controls.Add(this.GroupBox3);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "frmTimedBars";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TimedBars";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmTimedBars_Closing);
            this.Load += new System.EventHandler(this.frmTimedBars_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox5.ResumeLayout(false);
            this.gbIndexInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            this.gbTimeInterval.ResumeLayout(false);
            this.GroupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion
        
        /// <summary>
        /// Creates a CEL object, changes its configurations, and starts up the created CEL object.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void frmTimedBars_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                // Creates the CQGCEL object
                CEL = new CQG.CQGCELClass();
                
                CEL.DataError += new CQG._ICQGCELEvents_DataErrorEventHandler(CEL_DataError);
                CEL.DataConnectionStatusChanged += new CQG._ICQGCELEvents_DataConnectionStatusChangedEventHandler(CEL_DataConnectionStatusChanged);
                CEL.TimedBarsResolved += new CQG._ICQGCELEvents_TimedBarsResolvedEventHandler(CEL_TimedBarsResolved);
                CEL.TimedBarsAdded += new CQG._ICQGCELEvents_TimedBarsAddedEventHandler(CEL_TimedBarsAdded);
                CEL.TimedBarsUpdated += new CQG._ICQGCELEvents_TimedBarsUpdatedEventHandler(CEL_TimedBarsUpdated);
                CEL.TimedBarsInserted += new CQG._ICQGCELEvents_TimedBarsInsertedEventHandler(CEL_TimedBarsInserted);
                CEL.TimedBarsRemoved += new CQG._ICQGCELEvents_TimedBarsRemovedEventHandler(CEL_TimedBarsRemoved);
                // !!
                //CEL.APIConfiguration.ReadyStatusCheck = CQG.eReadyStatusCheck.rscOff;
                //CEL.APIConfiguration.CollectionsThrowException = false;
                //CEL.APIConfiguration.TimeZoneCode = CQG.eTimeZone.tzCentral;

                // Disables the controls
                CEL_DataConnectionStatusChanged(CQG.eConnectionStatus.csConnectionDown);
                // Starts up the CQGCEL
                CEL.Startup();
                
                cmbTimedBars.Items.Clear();
                cmbTimedBars.SelectedIndex = - 1;
                
                ClearAllData();
                
                m_CurTimedBars = null;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "frmTimedBars_Load", ex);
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
        private void frmTimedBars_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CEL != null)
            {
                CEL.Shutdown();
            }
        }
        
        /// <summary>
        /// Opens frmTimedBarsRequestSettings and with passed values requests TimedBars.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void btnRequest_Click(System.Object sender, System.EventArgs e)
        {
            frmTimedBarsRequestSettings frmTimedBarsReq;
            CQG.CQGTimedBars TimedBars;
            CQG.CQGTimedBarsRequest req;
            
            try
            {
                // Opens the form to fill up request data
                frmTimedBarsReq = new frmTimedBarsRequestSettings();
                req = CEL.CreateTimedBarsRequest();
                
                frmTimedBarsReq.TimedBarsRequest = req;
                frmTimedBarsReq.LineTime = CEL.Environment.LineTime;
                
                // If the user confirms, request is sent to CQGCEL and added to the requests list
                if (frmTimedBarsReq.ShowDialog(this) == DialogResult.OK)
                {
                    TimedBars = CEL.RequestTimedBars(req);
                    
                    cmbTimedBars.Items.Add(TimedBars.Id);
                    cmbTimedBars.SelectedIndex = cmbTimedBars.Items.Count - 1;
                    
                    ChangeControlsStatuses();
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "btnRequest_Click", ex);
            }
        }
        
        /// <summary>
        /// Changes to show the selected TimedBars info.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data
        /// </param>
        private void cmbTimedBars_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (cmbTimedBars.SelectedIndex > - 1)
                {
                    // Gets the selected TimedBar object from CQGCEL by it's ID
                    m_CurTimedBars = CEL.AllTimedBars.get_ItemById(cmbTimedBars.Text);
                    
                    ClearAllData();
                    DumpAllData();
                    
                    DumpFilterInfo();
                }
                
                ChangeControlsStatuses();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "cmbTimedBars_SelectedIndexChanged", ex);
            }
        }
        
        /// <summary>
        /// Changes the visibility of groupboxes.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void optIndexOrTimeInterval_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                gbIndexInterval.Visible = optIndexInterval.Checked;
                gbTimeInterval.Visible = optTimeInterval.Checked;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "optIndexOrTimeInterval_CheckedChanged", ex);
            }
        }
        
        /// <summary>
        /// Shows few data from the selected TimedBars collection.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void btnShow_Click(System.Object sender, System.EventArgs e)
        {
            int iStart;
            int iEnd;
            DateTime dStart;
            DateTime dEnd;
            
            try
            {
                lvShownTimedBars.Items.Clear();
                
                if (optIndexInterval.Checked)
                {
                    iStart = System.Convert.ToInt32(nudStart.Value) - 1;
                    iEnd = System.Convert.ToInt32(nudEnd.Value) - 1;
                    if (iStart > iEnd || iEnd > m_CurTimedBars.Count - 1 || iEnd - iStart >= MAX_INDEX_INTERVAL)
                    {
                        MessageBox.Show("The start index must be smaller than the end index," + Environment.NewLine + "their range must indicate to no more than " + MAX_INDEX_INTERVAL + " records," + Environment.NewLine + "and no more than the returned records count.", "Incorrect range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else
                {
                    // To get time with 00 seconds
                    dStart = dtpStartRange.Value.AddSeconds(- dtpStartRange.Value.Second);
                    dEnd = dtpEndRange.Value.AddSeconds(- dtpEndRange.Value.Second);
                    iStart = m_CurTimedBars.IndexForDate(dStart);
                    iEnd = m_CurTimedBars.IndexForDate(dEnd);
                    if (dStart > dEnd || !(CEL.IsValid(iStart) && CEL.IsValid(iEnd)) || iEnd - iStart >= MAX_INDEX_INTERVAL)
                    {
                        MessageBox.Show("The start time must be earlier than the end time and their" + Environment.NewLine + "range must indicate to no more than " + MAX_INDEX_INTERVAL + " records.", "Incorrect range", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                
                lvShownTimedBars.SuspendLayout();
                for (int i = iEnd; i >= iStart; i--)
                {
                    DumpRecord(lvShownTimedBars, m_CurTimedBars[i], i + 1);
                }
                lvShownTimedBars.ResumeLayout(false);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "btnShow_Click", ex);
            }
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
        private void llWeb_LinkClicked(System.Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("iexplore.exe", "http://www.cqg.com/Products/CQG-API.aspx");
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "llWeb_LinkClicked", ex);
            }
        }
        
        /// <summary>
        /// Removes the selected TimedBar from the list and the CQGCEL.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void btnRemove_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                // Removes TimedBars from the collection
                CEL.RemoveTimedBars(m_CurTimedBars);
                
                // Removes TimedBars from the combo box
                cmbTimedBars.Items.RemoveAt(cmbTimedBars.SelectedIndex);
                
                cmbTimedBars.SelectedIndex = cmbTimedBars.Items.Count - 1;
                
                if (cmbTimedBars.Items.Count == 0)
                {
                    cmbTimedBars.SelectedIndex = - 1;
                    ClearAllData();
                    m_CurTimedBars = null;
                }
                
                ChangeControlsStatuses();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "btnRemove_Click", ex);
            }
        }
        
        /// <summary>
        /// Removes all TimedBars from the list and the CQGCEL.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void btnRemoveAll_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                // Clears the combo box
                cmbTimedBars.Items.Clear();
                cmbTimedBars.SelectedIndex = - 1;
                
                // Removes all TimedBars from the collection
                CEL.RemoveAllTimedBars();
                
                m_CurTimedBars = null;
                
                ClearAllData();
                
                ChangeControlsStatuses();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "btnRemoveAll_Click", ex);
            }
        }
        
        /// <summary>
        /// Shows tag value of the current TimedBars corresponding to the entered name.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// A KeyEventArgs that contains the event data.
        /// </param>
        private void txtTagName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (txtTagName.Text.Trim().Length == 0)
                {
                    return;
                }
                
                if (e.KeyCode == Keys.Return)
                {
                    object tagValue = m_CurTimedBars.get_Tag(txtTagName.Text);
                    if (tagValue != null)
                    {
                        txtTagValue.Text = tagValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "txtTagName_KeyDown", ex);
            }
        }
        
        /// <summary>
        /// Sets the tag value of the current TimedBars corresponding to the entered name.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// A KeyEventArgs that contains the event data.
        /// </param>
        private void txtTagValue_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (txtTagName.Text.Trim().Length == 0)
                {
                    return;
                }
                
                if (e.KeyCode == Keys.Return)
                {
                    if (txtTagValue.Text.Trim().Length == 0)
                    {
                        m_CurTimedBars.set_Tag(txtTagName.Text, null);
                    }
                    else
                    {
                        m_CurTimedBars.set_Tag(txtTagName.Text, txtTagValue.Text);
                    }
                    txtTagName.Text = "";
                    txtTagValue.Text = "";
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "txtTagValue_KeyDown", ex);
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
                if (cqg_error is CQG.CQGError)
                {
                    CQG.CQGError cqgErr = (CQG.CQGError) cqg_error;
                    
                    if (cqgErr.Code == 102)
                    {
                        error_description += " Restart the application.";
                    }
                    else if (cqgErr.Code == 125)
                    {
                        error_description += " Turn on CQG Client and restart the application.";
                    }
                }
                
                MessageBox.Show(error_description, "TimedBars", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_DataError", ex);
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
            string sInfo;
            System.Drawing.Color BackCol;
            
            try
            {
                if (new_status == CQG.eConnectionStatus.csConnectionUp)
                {
                    BackCol = System.Drawing.Color.FromArgb(192, 209, 205);
                    sInfo = "DATA Connection is UP";
                }
                else if (new_status == CQG.eConnectionStatus.csConnectionDelayed)
                {
                    BackCol = System.Drawing.Color.FromArgb(255, 114, 0);
                    sInfo = "DATA Connection is Delayed";
                }
                else
                {
                    BackCol = System.Drawing.Color.FromArgb(255, 114, 0);
                    sInfo = "DATA Connection is Down";
                }

                RefreshFormItems(BackCol, sInfo);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_DataConnectionStatusChanged", ex);
            }
        }
        
        /// <summary>
        /// Fired when the collection of timed bars (CQGTimedBars) is resolved or
        /// when some error occured during timed bars request processing.
        /// </summary>
        /// <param name="cqg_timed_bars">
        /// Reference to resolved CQGTimedBars
        /// </param>
        /// <param name="cqg_error">
        /// The CQGError object that describes the last error occurred
        /// while processing the TimedBars request or
        /// Nothing/Invalid_Error in case of no error.
        /// </param>
        private void CEL_TimedBarsResolved(CQG.CQGTimedBars cqg_timed_bars, CQG.CQGError cqg_error)
        {
            try
            {
                if (cqg_timed_bars.Id == m_CurTimedBars.Id)
                {
                    ClearAllData();
                    DumpAllData();
                    DumpFilterInfo();
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_TimedBarsResolved", ex);
            }
        }
        
        /// <summary>
        /// Fired when CQGTimedBar item is added to the end of CQGTimedBars.
        /// </summary>
        /// <param name="cqg_timed_bars">
        /// Reference to changed CQGTimedBars.
        /// </param>
        private void CEL_TimedBarsAdded(CQG.CQGTimedBars cqg_timed_bars)
        {
            try
            {
                if (cqg_timed_bars.Id == m_CurTimedBars.Id)
                {
                    ShowChangedData(cqg_timed_bars.Count);
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_TimedBarsAdded", ex);
            }
        }

        /// <summary>
        /// Fired when CQGTimedBar item is inserted into CQGTimedBars.
        /// </summary>
        /// <param name="cqg_timed_bars">
        /// Reference to changed CQGTimedBars.
        /// </param>
        /// <param name="index_">
        /// Specifies the changed CQGTimedBar index
        /// </param>
        private void CEL_TimedBarsInserted(CQG.CQGTimedBars cqg_timed_bars, int index_)
        {
            try
            {
                if (cqg_timed_bars.Id == m_CurTimedBars.Id)
                {
                    ShowChangedData(cqg_timed_bars.Count);
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_TimedBarsInserted", ex);
            }
        }

        /// <summary>
        /// Fired when CQGTimedBar item is removed from CQGTimedBars.
        /// </summary>
        /// <param name="cqg_timed_bars">
        /// Reference to changed CQGTimedBars.
        /// </param>
        /// <param name="index_">
        /// Specifies the changed CQGTimedBar index
        /// </param>
        private void CEL_TimedBarsRemoved(CQG.CQGTimedBars cqg_timed_bars, int index_)
        {
            try
            {
                if (cqg_timed_bars.Id == m_CurTimedBars.Id)
                {
                    ShowChangedData(cqg_timed_bars.Count);
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_TimedBarsRemoved", ex);
            }
        }
        
        /// <summary>
        /// Fired when CQGTimedBar item is updated.
        /// </summary>
        /// <param name="cqg_timed_bars">
        /// Reference to changed CQGTimedBars
        /// </param>
        /// <param name="index_">
        /// Specifies the updated CQGTimedBar index.
        /// </param>
        private void CEL_TimedBarsUpdated(CQG.CQGTimedBars cqg_timed_bars, int index_)
        {
            try
            {
                if (cqg_timed_bars.Id == m_CurTimedBars.Id)
                {
                    if (index_ > m_CurTimedBars.Count - MAX_NUMBER_OF_RECORDS)
                    {
                        UpdateRecord(lvTimedBars, m_CurTimedBars[index_], index_ + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "CEL_TimedBarsUpdated", ex);
            }
        }
        
        /// <summary>
        /// Shows the selected request parameters on the listview.
        /// </summary>
        private void DumpRequestParams()
        {
            try
            {
                CQG.CQGTimedBarsRequest req = m_CurTimedBars.Request;
                
                lblRPSymbol.Text = req.Symbol;
                lblRPRangeStart.Text = GetValueAsString(req.RangeStart);
                lblRPRangeEnd.Text = GetValueAsString(req.RangeEnd);
                lblRPIntradayPeriod.Text = req.IntradayPeriod.ToString();
                lblRPHistoricalPeriod.Text = req.HistoricalPeriod.ToString();
                lblRPTickFlter.Text = req.TickFilter.ToString();
                lblRPUpdatesEnabled.Text = req.UpdatesEnabled.ToString() + (req.IgnoreEventsOnHistoricalBars ? "/NoHist" : "");
                lblRPIncludeEnd.Text = req.IncludeEnd.ToString();
                lblRPSessionsFilter.Text = req.SessionsFilter.ToString();
                lblRPContinuation.Text = req.Continuation.ToString();
                lblRPEqualizeCloses.Text = req.EqualizeCloses.ToString();
                lblRPDaysBeforeExpiration.Text = req.DaysBeforeExpiration.ToString();
                lblRPSessionFlags.Text = req.SessionFlags.ToString();
                
                if (m_CurTimedBars.LastError != null)
                {
                    lblError.Text = m_CurTimedBars.LastError.Description;
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpRequestParams", ex);
            }
        }
        
        /// <summary>
        /// Shows the selected request outputs on the listview.
        /// </summary>
        private void DumpOutputParams()
        {
            try
            {
                lblOStart.Text = GetValueAsString(m_CurTimedBars.StartTimestamp);
                lblOEnd.Text = GetValueAsString(m_CurTimedBars.EndTimestamp);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpOutputParams", ex);
            }
        }

        /// <summary>
        /// Shows the selected request outputs on the listview after changes.
        /// </summary>
        /// <param name="count">
        /// Count of TimedBars that will be shown.
        /// </param>
        private void ShowChangedData(int count)
        {
            ClearOutputParams();
            DumpOutputParams();
                    
            DumpRecords(count);
        }
        
        /// <summary>
        /// Shows added TimedBars for the selected request records on the listview.
        /// </summary>
        /// <param name="addedTimedBarsCount">
        /// Count of TimedBars that will be shown.
        /// </param>
        private void DumpRecords(int addedTimedBarsCount)
        {
            try
            {
                // Clears all records
                ClearRecords();
                
                if (m_CurTimedBars.Count == 0)
                {
                    return;
                }
                
                if (addedTimedBarsCount == m_CurTimedBars.Count)
                {
                    addedTimedBarsCount = System.Convert.ToInt32(addedTimedBarsCount >= MAX_NUMBER_OF_RECORDS ? MAX_NUMBER_OF_RECORDS : m_CurTimedBars.Count);
                }
                for (int i = m_CurTimedBars.Count - 1; i >= m_CurTimedBars.Count - addedTimedBarsCount; i--)
                {
                    DumpRecord(lvTimedBars, m_CurTimedBars[i], i + 1);
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpRecords", ex);
            }
        }
        
        /// <summary>
        /// Dumps one record's values to the listview.
        /// </summary>
        /// <param name="lv">
        /// Listview, in which data must be shown.
        /// </param>
        /// <param name="timedBar">
        /// Record, which values must be shown.
        /// </param>
        /// <param name="recordIndex">
        /// Index of record, which values must be shown.
        /// </param>
        private void DumpRecord(ListView lv, CQG.CQGTimedBar timedBar, long recordIndex)
        {
            try
            {
                ListViewItem item = lv.Items.Add(new ListViewItem());
                item.SubItems.Add("Index").Text = recordIndex.ToString();
                item.SubItems.Add("Timestamp").Text = GetValueAsString(timedBar.Timestamp);
                item.SubItems.Add("Open").Text = GetValueAsString(timedBar.Open);
                item.SubItems.Add("High").Text = GetValueAsString(timedBar.High);
                item.SubItems.Add("Low").Text = GetValueAsString(timedBar.Low);
                item.SubItems.Add("Close").Text = GetValueAsString(timedBar.Close);
                item.SubItems.Add("Mid").Text = GetValueAsString(timedBar.Mid);
                item.SubItems.Add("HLC3").Text = GetValueAsString(timedBar.HLC3);
                item.SubItems.Add("Avg").Text = GetValueAsString(timedBar.Avg);
                item.SubItems.Add("TrueHigh").Text = GetValueAsString(timedBar.TrueHigh);
                item.SubItems.Add("TrueLow").Text = GetValueAsString(timedBar.TrueLow);
                item.SubItems.Add("Range").Text = GetValueAsString(timedBar.Range);
                item.SubItems.Add("TrueRange").Text = GetValueAsString(timedBar.TrueRange);
                item.SubItems.Add("OpenInterest").Text = GetValueAsString(timedBar.OpenInterest);
                item.SubItems.Add("ActualVolume").Text = GetValueAsString(timedBar.ActualVolume);
                item.SubItems.Add("TickVolume").Text = GetValueAsString(timedBar.TickVolume);
                item.SubItems.Add("AskVolume").Text = GetValueAsString(timedBar.AskVolume);
                item.SubItems.Add("BidVolume ").Text = GetValueAsString(timedBar.BidVolume);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpRecord", ex);
            }
        }
        
        /// <summary>
        /// Updates one record's values to the listview.
        /// </summary>
        /// <param name="lv">
        /// Listview, in which data must be shown.
        /// </param>
        /// <param name="timedBar">
        /// Record, which values must be shown.
        /// </param>
        /// <param name="recordIndex">
        /// Index of record, which values must be updated.
        /// </param>
        private void UpdateRecord(ListView lv, CQG.CQGTimedBar timedBar, int recordIndex)
        {
            try
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add("Index").Text = recordIndex.ToString();
                item.SubItems.Add("Timestamp").Text = GetValueAsString(timedBar.Timestamp);
                item.SubItems.Add("Open").Text = GetValueAsString(timedBar.Open);
                item.SubItems.Add("High").Text = GetValueAsString(timedBar.High);
                item.SubItems.Add("Low").Text = GetValueAsString(timedBar.Low);
                item.SubItems.Add("Close").Text = GetValueAsString(timedBar.Close);
                item.SubItems.Add("Mid").Text = GetValueAsString(timedBar.Mid);
                item.SubItems.Add("HLC3").Text = GetValueAsString(timedBar.HLC3);
                item.SubItems.Add("Avg").Text = GetValueAsString(timedBar.Avg);
                item.SubItems.Add("TrueHigh").Text = GetValueAsString(timedBar.TrueHigh);
                item.SubItems.Add("TrueLow").Text = GetValueAsString(timedBar.TrueLow);
                item.SubItems.Add("Range").Text = GetValueAsString(timedBar.Range);
                item.SubItems.Add("TrueRange").Text = GetValueAsString(timedBar.TrueRange);
                item.SubItems.Add("OpenInterest").Text = GetValueAsString(timedBar.OpenInterest);
                item.SubItems.Add("ActualVolume").Text = GetValueAsString(timedBar.ActualVolume);
                item.SubItems.Add("TickVolume").Text = GetValueAsString(timedBar.TickVolume);
                item.SubItems.Add("AskVolume").Text = GetValueAsString(timedBar.AskVolume);
                item.SubItems.Add("BidVolume ").Text = GetValueAsString(timedBar.BidVolume);
                
                lv.Items[m_CurTimedBars.Count - recordIndex] = item;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "UpdateRecord", ex);
            }
        }
        
        /// <summary>
        /// Dumps all request data (outputs\parameters\records)
        /// </summary>
        private void DumpAllData()
        {
            try
            {
                DumpRequestParams();
                
                lblStatus.Text = m_CurTimedBars.Status.ToString();
                
                if (m_CurTimedBars.Status == CQG.eRequestStatus.rsSuccess)
                {
                    DumpOutputParams();
                    DumpRecords(m_CurTimedBars.Count);
                }
                
                ChangeControlsStatuses();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpAllData", ex);
            }
        }
        
        /// <summary>
        /// Initializes the filter-related controls.
        /// </summary>
        private void DumpFilterInfo()
        {
            try
            {
                if (m_CurTimedBars != null&& m_CurTimedBars.Status == CQG.eRequestStatus.rsSuccess)
                {
                    dtpStartRange.Value = m_CurTimedBars.StartTimestamp;
                    dtpEndRange.Value = m_CurTimedBars.EndTimestamp;
                }
                else
                {
                    dtpStartRange.Value = DateTime.Now;
                    dtpEndRange.Value = DateTime.Now;
                }
                nudStart.Value = 1;
                nudEnd.Value = 1;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "DumpFilterInfo", ex);
            }
        }
        
        /// <summary>
        /// Clears all request data.
        /// </summary>
        private void ClearAllData()
        {
            try
            {
                lblStatus.Text = "";
                
                ClearRequestParams();
                ClearOutputParams();
                ClearTags();
                ClearRecords();
                
                lvShownTimedBars.Items.Clear();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ClearAllData", ex);
            }
        }
        
        /// <summary>
        /// Clears the request parameters from the listview.
        /// </summary>
        private void ClearRequestParams()
        {
            try
            {
                lblRPSymbol.Text = "";
                lblRPRangeStart.Text = "";
                lblRPRangeEnd.Text = "";
                lblRPIntradayPeriod.Text = "";
                lblRPHistoricalPeriod.Text = "";
                lblRPTickFlter.Text = "";
                lblRPUpdatesEnabled.Text = "";
                lblRPIncludeEnd.Text = "";
                lblRPSessionsFilter.Text = "";
                lblRPContinuation.Text = "";
                lblRPEqualizeCloses.Text = "";
                lblRPDaysBeforeExpiration.Text = "";
                lblRPSessionFlags.Text = "";
                
                lblError.Text = "";
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ClearRequestParams", ex);
            }
        }
        
        /// <summary>
        /// Clears request outputs from the listview.
        /// </summary>
        private void ClearOutputParams()
        {
            try
            {
                lblOStart.Text = "";
                lblOEnd.Text = "";
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ClearOutputParams", ex);
            }
        }
        
        /// <summary>
        /// Clears request tags.
        /// </summary>
        private void ClearTags()
        {
            try
            {
                txtTagName.Text = "";
                txtTagValue.Text = "";
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ClearTags", ex);
            }
        }
        
        /// <summary>
        /// Clears records from the listview.
        /// </summary>
        private void ClearRecords()
        {
            try
            {
                lvTimedBars.Items.Clear();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ClearRecords", ex);
            }
        }
        
        /// <summary>
        /// Enables\disables the controls, based on the connection status.
        /// </summary>
        private void ChangeControlsStatuses()
        {
            bool bIsDataConnectionUp = false;
            bool bIsTimedBarsSelected;
            bool bIsTimedBarsSucceded = false;
            
            try
            {
                if (CEL.IsStarted)
                {
                    bIsDataConnectionUp = CEL.Environment.DataConnectionStatus == CQG.eConnectionStatus.csConnectionUp;
                }
                
                btnRequest.Enabled = bIsDataConnectionUp;
                
                bIsTimedBarsSelected = m_CurTimedBars != null;
                
                btnRemove.Enabled = bIsTimedBarsSelected && bIsDataConnectionUp;
                btnRemoveAll.Enabled = bIsTimedBarsSelected && bIsDataConnectionUp;
                
                txtTagName.Enabled = bIsTimedBarsSelected;
                txtTagValue.Enabled = bIsTimedBarsSelected;
                
                if (bIsTimedBarsSelected)
                {
                    bIsTimedBarsSucceded = m_CurTimedBars.Status == CQG.eRequestStatus.rsSuccess;
                }
                
                dtpEndRange.Enabled = bIsTimedBarsSucceded;
                dtpStartRange.Enabled = bIsTimedBarsSucceded;
                
                nudEnd.Enabled = bIsTimedBarsSucceded;
                nudStart.Enabled = bIsTimedBarsSucceded;
                
                optIndexInterval.Enabled = bIsTimedBarsSucceded;
                optTimeInterval.Enabled = bIsTimedBarsSucceded;
                
                btnShow.Enabled = bIsTimedBarsSucceded;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBars", "ChangeControlsStatuses", ex);
            }
        }
        
        /// <summary>
        /// Validates the specified value and converts it to string.
        /// </summary>
        /// <param name="val">
        /// The value that will be validated and converted
        /// </param>
        /// <returns>
        /// Value converted to string or "N/A" if the value is invalid.
        /// </returns>
        private string GetValueAsString(object val)
        {
            string sResult = "";
            
            try
            {
                if (CEL.IsValid(val))
                {
                    if (val.GetType().FullName == "System.DateTime")
                    {
                        sResult = System.Convert.ToDateTime(val).ToString("dd/MM/yyyy HH:mm");
                    }
                    else
                    {
                        sResult = val.ToString();
                    }
                }
                else
                {
                    sResult = N_A;
                }
            }
            catch
            {
                sResult = N_A;
            }
            
            return sResult;
        }
        
    }

}
