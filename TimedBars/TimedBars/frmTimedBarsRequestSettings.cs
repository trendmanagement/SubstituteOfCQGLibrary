using System;
using System.Windows.Forms;
using CQG = FakeCQG;

namespace TimedBars
{
    public class frmTimedBarsRequestSettings : System.Windows.Forms.Form
    {
        
        
        #region " Windows Form Designer generated code "
        
        public frmTimedBarsRequestSettings()
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
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.GroupBox GroupBox3;
        internal System.Windows.Forms.DateTimePicker dtpEndRange;
        internal System.Windows.Forms.DateTimePicker dtpStartRange;
        internal System.Windows.Forms.TextBox txtSymbol;
        internal System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.GroupBox GroupBox4;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.GroupBox GroupBox6;
        internal System.Windows.Forms.NumericUpDown nudSessionsFilter;
        internal System.Windows.Forms.RadioButton optDateDate;
        internal System.Windows.Forms.RadioButton optDateInt;
        internal System.Windows.Forms.CheckBox chkIncludeEnd;
        internal System.Windows.Forms.CheckBox chkEqualizeCloses;
        internal System.Windows.Forms.NumericUpDown nudDaysBeforeExpiration;
        internal System.Windows.Forms.ComboBox cmbContinuationType;
        internal System.Windows.Forms.ComboBox cmbHistoricalPeriod;
        internal System.Windows.Forms.NumericUpDown nudIntradayPeriod;
        internal System.Windows.Forms.CheckBox chkUpdatesEnabled;
        internal System.Windows.Forms.CheckBox chkSFDailyFromIntraday;
        internal System.Windows.Forms.CheckBox chkSessionsCustomFilter;
        internal System.Windows.Forms.RadioButton optIntInt;
        internal System.Windows.Forms.NumericUpDown nudStartRange;
        internal System.Windows.Forms.NumericUpDown nudEndRange;
        internal System.Windows.Forms.TextBox txtSessionsFilter;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.ComboBox cmbTickFilter;
        internal System.Windows.Forms.GroupBox GroupBox7;
        internal System.Windows.Forms.CheckedListBox chlstOutputs;
        internal System.Windows.Forms.CheckBox chkNoHistorical;
        internal System.Windows.Forms.Label Label8;
        [System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
        {
         this.Label1 = new System.Windows.Forms.Label();
         this.GroupBox1 = new System.Windows.Forms.GroupBox();
         this.nudEndRange = new System.Windows.Forms.NumericUpDown();
         this.nudStartRange = new System.Windows.Forms.NumericUpDown();
         this.chkIncludeEnd = new System.Windows.Forms.CheckBox();
         this.optDateDate = new System.Windows.Forms.RadioButton();
         this.optDateInt = new System.Windows.Forms.RadioButton();
         this.optIntInt = new System.Windows.Forms.RadioButton();
         this.dtpEndRange = new System.Windows.Forms.DateTimePicker();
         this.dtpStartRange = new System.Windows.Forms.DateTimePicker();
         this.Label3 = new System.Windows.Forms.Label();
         this.Label2 = new System.Windows.Forms.Label();
         this.GroupBox3 = new System.Windows.Forms.GroupBox();
         this.cmbHistoricalPeriod = new System.Windows.Forms.ComboBox();
         this.nudIntradayPeriod = new System.Windows.Forms.NumericUpDown();
         this.Label6 = new System.Windows.Forms.Label();
         this.Label7 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.cmbTickFilter = new System.Windows.Forms.ComboBox();
         this.GroupBox2 = new System.Windows.Forms.GroupBox();
         this.txtSessionsFilter = new System.Windows.Forms.TextBox();
         this.chkSessionsCustomFilter = new System.Windows.Forms.CheckBox();
         this.nudSessionsFilter = new System.Windows.Forms.NumericUpDown();
         this.txtSymbol = new System.Windows.Forms.TextBox();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.GroupBox4 = new System.Windows.Forms.GroupBox();
         this.Label8 = new System.Windows.Forms.Label();
         this.cmbContinuationType = new System.Windows.Forms.ComboBox();
         this.nudDaysBeforeExpiration = new System.Windows.Forms.NumericUpDown();
         this.chkEqualizeCloses = new System.Windows.Forms.CheckBox();
         this.Label4 = new System.Windows.Forms.Label();
         this.Label5 = new System.Windows.Forms.Label();
         this.GroupBox5 = new System.Windows.Forms.GroupBox();
         this.chkUpdatesEnabled = new System.Windows.Forms.CheckBox();
         this.GroupBox6 = new System.Windows.Forms.GroupBox();
         this.chkSFDailyFromIntraday = new System.Windows.Forms.CheckBox();
         this.GroupBox7 = new System.Windows.Forms.GroupBox();
         this.chlstOutputs = new System.Windows.Forms.CheckedListBox();
         this.chkNoHistorical = new System.Windows.Forms.CheckBox();
         this.GroupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudEndRange)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStartRange)).BeginInit();
         this.GroupBox3.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudIntradayPeriod)).BeginInit();
         this.GroupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudSessionsFilter)).BeginInit();
         this.GroupBox4.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudDaysBeforeExpiration)).BeginInit();
         this.GroupBox5.SuspendLayout();
         this.GroupBox6.SuspendLayout();
         this.GroupBox7.SuspendLayout();
         this.SuspendLayout();
         // 
         // Label1
         // 
         this.Label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label1.Location = new System.Drawing.Point(16, 8);
         this.Label1.Name = "Label1";
         this.Label1.Size = new System.Drawing.Size(64, 24);
         this.Label1.TabIndex = 0;
         this.Label1.Text = "Symbol :";
         // 
         // GroupBox1
         // 
         this.GroupBox1.Controls.Add(this.nudEndRange);
         this.GroupBox1.Controls.Add(this.nudStartRange);
         this.GroupBox1.Controls.Add(this.chkIncludeEnd);
         this.GroupBox1.Controls.Add(this.optDateDate);
         this.GroupBox1.Controls.Add(this.optDateInt);
         this.GroupBox1.Controls.Add(this.optIntInt);
         this.GroupBox1.Controls.Add(this.dtpEndRange);
         this.GroupBox1.Controls.Add(this.dtpStartRange);
         this.GroupBox1.Controls.Add(this.Label3);
         this.GroupBox1.Controls.Add(this.Label2);
         this.GroupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox1.Location = new System.Drawing.Point(8, 40);
         this.GroupBox1.Name = "GroupBox1";
         this.GroupBox1.Size = new System.Drawing.Size(320, 120);
         this.GroupBox1.TabIndex = 2;
         this.GroupBox1.TabStop = false;
         this.GroupBox1.Text = "TimedBars Request Range";
         // 
         // nudEndRange
         // 
         this.nudEndRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.nudEndRange.Location = new System.Drawing.Point(48, 64);
         this.nudEndRange.Maximum = new System.Decimal(new int[] {
                                                                    10000000,
                                                                    0,
                                                                    0,
                                                                    0});
         this.nudEndRange.Minimum = new System.Decimal(new int[] {
                                                                    10000000,
                                                                    0,
                                                                    0,
                                                                    -2147483648});
         this.nudEndRange.Name = "nudEndRange";
         this.nudEndRange.Size = new System.Drawing.Size(128, 22);
         this.nudEndRange.TabIndex = 1;
         // 
         // nudStartRange
         // 
         this.nudStartRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.nudStartRange.Location = new System.Drawing.Point(48, 32);
         this.nudStartRange.Maximum = new System.Decimal(new int[] {
                                                                      10000000,
                                                                      0,
                                                                      0,
                                                                      0});
         this.nudStartRange.Minimum = new System.Decimal(new int[] {
                                                                      10000000,
                                                                      0,
                                                                      0,
                                                                      -2147483648});
         this.nudStartRange.Name = "nudStartRange";
         this.nudStartRange.Size = new System.Drawing.Size(128, 22);
         this.nudStartRange.TabIndex = 0;
         // 
         // chkIncludeEnd
         // 
         this.chkIncludeEnd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.chkIncludeEnd.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkIncludeEnd.Location = new System.Drawing.Point(72, 96);
         this.chkIncludeEnd.Name = "chkIncludeEnd";
         this.chkIncludeEnd.Size = new System.Drawing.Size(104, 16);
         this.chkIncludeEnd.TabIndex = 2;
         this.chkIncludeEnd.Text = "Include End";
         // 
         // optDateDate
         // 
         this.optDateDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optDateDate.Location = new System.Drawing.Point(184, 68);
         this.optDateDate.Name = "optDateDate";
         this.optDateDate.Size = new System.Drawing.Size(128, 24);
         this.optDateDate.TabIndex = 5;
         this.optDateDate.Text = "Date/Date";
         this.optDateDate.CheckedChanged += new System.EventHandler(this.RequestType_CheckedChanged);
         // 
         // optDateInt
         // 
         this.optDateInt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optDateInt.Location = new System.Drawing.Point(184, 48);
         this.optDateInt.Name = "optDateInt";
         this.optDateInt.Size = new System.Drawing.Size(128, 24);
         this.optDateInt.TabIndex = 4;
         this.optDateInt.Text = "Date/Int";
         this.optDateInt.CheckedChanged += new System.EventHandler(this.RequestType_CheckedChanged);
         // 
         // optIntInt
         // 
         this.optIntInt.Checked = true;
         this.optIntInt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.optIntInt.Location = new System.Drawing.Point(184, 28);
         this.optIntInt.Name = "optIntInt";
         this.optIntInt.Size = new System.Drawing.Size(128, 24);
         this.optIntInt.TabIndex = 3;
         this.optIntInt.TabStop = true;
         this.optIntInt.Text = "Int/Int";
         this.optIntInt.CheckedChanged += new System.EventHandler(this.RequestType_CheckedChanged);
         // 
         // dtpEndRange
         // 
         this.dtpEndRange.CustomFormat = "dd/MM/yyyy HH:mm";
         this.dtpEndRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.dtpEndRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
         this.dtpEndRange.Location = new System.Drawing.Point(48, 64);
         this.dtpEndRange.Name = "dtpEndRange";
         this.dtpEndRange.Size = new System.Drawing.Size(128, 22);
         this.dtpEndRange.TabIndex = 1;
         // 
         // dtpStartRange
         // 
         this.dtpStartRange.CustomFormat = "dd/MM/yyyy HH:mm";
         this.dtpStartRange.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.dtpStartRange.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
         this.dtpStartRange.Location = new System.Drawing.Point(48, 32);
         this.dtpStartRange.Name = "dtpStartRange";
         this.dtpStartRange.Size = new System.Drawing.Size(128, 22);
         this.dtpStartRange.TabIndex = 0;
         // 
         // Label3
         // 
         this.Label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label3.Location = new System.Drawing.Point(8, 64);
         this.Label3.Name = "Label3";
         this.Label3.Size = new System.Drawing.Size(40, 24);
         this.Label3.TabIndex = 1;
         this.Label3.Text = "End";
         this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Label2
         // 
         this.Label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label2.Location = new System.Drawing.Point(8, 32);
         this.Label2.Name = "Label2";
         this.Label2.Size = new System.Drawing.Size(40, 24);
         this.Label2.TabIndex = 0;
         this.Label2.Text = "Start";
         this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // GroupBox3
         // 
         this.GroupBox3.Controls.Add(this.cmbHistoricalPeriod);
         this.GroupBox3.Controls.Add(this.nudIntradayPeriod);
         this.GroupBox3.Controls.Add(this.Label6);
         this.GroupBox3.Controls.Add(this.Label7);
         this.GroupBox3.Controls.Add(this.label9);
         this.GroupBox3.Controls.Add(this.cmbTickFilter);
         this.GroupBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox3.Location = new System.Drawing.Point(336, 40);
         this.GroupBox3.Name = "GroupBox3";
         this.GroupBox3.Size = new System.Drawing.Size(220, 120);
         this.GroupBox3.TabIndex = 3;
         this.GroupBox3.TabStop = false;
         this.GroupBox3.Text = "TimedBar Parameters";
         // 
         // cmbHistoricalPeriod
         // 
         this.cmbHistoricalPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbHistoricalPeriod.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.cmbHistoricalPeriod.Location = new System.Drawing.Point(80, 24);
         this.cmbHistoricalPeriod.Name = "cmbHistoricalPeriod";
         this.cmbHistoricalPeriod.Size = new System.Drawing.Size(128, 24);
         this.cmbHistoricalPeriod.TabIndex = 1;
         this.cmbHistoricalPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbHistoricalPeriod_SelectedIndexChanged);
         // 
         // nudIntradayPeriod
         // 
         this.nudIntradayPeriod.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.nudIntradayPeriod.Location = new System.Drawing.Point(80, 56);
         this.nudIntradayPeriod.Maximum = new System.Decimal(new int[] {
                                                                          10000000,
                                                                          0,
                                                                          0,
                                                                          0});
         this.nudIntradayPeriod.Name = "nudIntradayPeriod";
         this.nudIntradayPeriod.Size = new System.Drawing.Size(128, 22);
         this.nudIntradayPeriod.TabIndex = 3;
         this.nudIntradayPeriod.Value = new System.Decimal(new int[] {
                                                                        1,
                                                                        0,
                                                                        0,
                                                                        0});
         // 
         // Label6
         // 
         this.Label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label6.Location = new System.Drawing.Point(8, 56);
         this.Label6.Name = "Label6";
         this.Label6.Size = new System.Drawing.Size(64, 24);
         this.Label6.TabIndex = 2;
         this.Label6.Text = "Intraday";
         this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // Label7
         // 
         this.Label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label7.Location = new System.Drawing.Point(8, 24);
         this.Label7.Name = "Label7";
         this.Label7.Size = new System.Drawing.Size(64, 24);
         this.Label7.TabIndex = 0;
         this.Label7.Text = "Historical";
         this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.Label7.Click += new System.EventHandler(this.Label7_Click);
         // 
         // label9
         // 
         this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.label9.Location = new System.Drawing.Point(8, 88);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(64, 24);
         this.label9.TabIndex = 4;
         this.label9.Text = "Tick Filter";
         this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // cmbTickFilter
         // 
         this.cmbTickFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbTickFilter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.cmbTickFilter.Location = new System.Drawing.Point(80, 88);
         this.cmbTickFilter.Name = "cmbTickFilter";
         this.cmbTickFilter.Size = new System.Drawing.Size(128, 24);
         this.cmbTickFilter.TabIndex = 5;
         // 
         // GroupBox2
         // 
         this.GroupBox2.Controls.Add(this.txtSessionsFilter);
         this.GroupBox2.Controls.Add(this.chkSessionsCustomFilter);
         this.GroupBox2.Controls.Add(this.nudSessionsFilter);
         this.GroupBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox2.Location = new System.Drawing.Point(8, 356);
         this.GroupBox2.Name = "GroupBox2";
         this.GroupBox2.Size = new System.Drawing.Size(548, 56);
         this.GroupBox2.TabIndex = 7;
         this.GroupBox2.TabStop = false;
         this.GroupBox2.Text = "Sessions Filter";
         // 
         // txtSessionsFilter
         // 
         this.txtSessionsFilter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.txtSessionsFilter.Location = new System.Drawing.Point(8, 24);
         this.txtSessionsFilter.Name = "txtSessionsFilter";
         this.txtSessionsFilter.Size = new System.Drawing.Size(428, 22);
         this.txtSessionsFilter.TabIndex = 0;
         this.txtSessionsFilter.Text = "";
         this.txtSessionsFilter.Visible = false;
         // 
         // chkSessionsCustomFilter
         // 
         this.chkSessionsCustomFilter.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
         this.chkSessionsCustomFilter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkSessionsCustomFilter.Location = new System.Drawing.Point(452, 28);
         this.chkSessionsCustomFilter.Name = "chkSessionsCustomFilter";
         this.chkSessionsCustomFilter.Size = new System.Drawing.Size(84, 16);
         this.chkSessionsCustomFilter.TabIndex = 1;
         this.chkSessionsCustomFilter.Text = "Custom";
         this.chkSessionsCustomFilter.CheckedChanged += new System.EventHandler(this.chkSessionsCustomFilter_CheckedChanged);
         // 
         // nudSessionsFilter
         // 
         this.nudSessionsFilter.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.nudSessionsFilter.Location = new System.Drawing.Point(8, 24);
         this.nudSessionsFilter.Name = "nudSessionsFilter";
         this.nudSessionsFilter.Size = new System.Drawing.Size(428, 22);
         this.nudSessionsFilter.TabIndex = 0;
         // 
         // txtSymbol
         // 
         this.txtSymbol.AutoSize = false;
         this.txtSymbol.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.txtSymbol.Location = new System.Drawing.Point(80, 8);
         this.txtSymbol.Name = "txtSymbol";
         this.txtSymbol.Size = new System.Drawing.Size(472, 20);
         this.txtSymbol.TabIndex = 1;
         this.txtSymbol.Text = "";
         // 
         // btnOK
         // 
         this.btnOK.Location = new System.Drawing.Point(404, 416);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(72, 24);
         this.btnOK.TabIndex = 8;
         this.btnOK.Text = "OK";
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(484, 416);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(72, 24);
         this.btnCancel.TabIndex = 9;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // GroupBox4
         // 
         this.GroupBox4.Controls.Add(this.Label8);
         this.GroupBox4.Controls.Add(this.cmbContinuationType);
         this.GroupBox4.Controls.Add(this.nudDaysBeforeExpiration);
         this.GroupBox4.Controls.Add(this.chkEqualizeCloses);
         this.GroupBox4.Controls.Add(this.Label4);
         this.GroupBox4.Controls.Add(this.Label5);
         this.GroupBox4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox4.Location = new System.Drawing.Point(8, 160);
         this.GroupBox4.Name = "GroupBox4";
         this.GroupBox4.Size = new System.Drawing.Size(320, 128);
         this.GroupBox4.TabIndex = 4;
         this.GroupBox4.TabStop = false;
         this.GroupBox4.Text = "Continuation";
         // 
         // Label8
         // 
         this.Label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label8.Location = new System.Drawing.Point(8, 56);
         this.Label8.Name = "Label8";
         this.Label8.Size = new System.Drawing.Size(144, 24);
         this.Label8.TabIndex = 10;
         this.Label8.Text = "Equalize Closes";
         this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // cmbContinuationType
         // 
         this.cmbContinuationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbContinuationType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.cmbContinuationType.Location = new System.Drawing.Point(160, 24);
         this.cmbContinuationType.Name = "cmbContinuationType";
         this.cmbContinuationType.Size = new System.Drawing.Size(152, 24);
         this.cmbContinuationType.TabIndex = 0;
         this.cmbContinuationType.SelectedIndexChanged += new System.EventHandler(this.cmbContinuationType_SelectedIndexChanged);
         // 
         // nudDaysBeforeExpiration
         // 
         this.nudDaysBeforeExpiration.Enabled = false;
         this.nudDaysBeforeExpiration.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.nudDaysBeforeExpiration.Location = new System.Drawing.Point(160, 88);
         this.nudDaysBeforeExpiration.Maximum = new System.Decimal(new int[] {
                                                                                10000000,
                                                                                0,
                                                                                0,
                                                                                0});
         this.nudDaysBeforeExpiration.Name = "nudDaysBeforeExpiration";
         this.nudDaysBeforeExpiration.Size = new System.Drawing.Size(152, 22);
         this.nudDaysBeforeExpiration.TabIndex = 2;
         // 
         // chkEqualizeCloses
         // 
         this.chkEqualizeCloses.Enabled = false;
         this.chkEqualizeCloses.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkEqualizeCloses.Location = new System.Drawing.Point(160, 60);
         this.chkEqualizeCloses.Name = "chkEqualizeCloses";
         this.chkEqualizeCloses.Size = new System.Drawing.Size(16, 16);
         this.chkEqualizeCloses.TabIndex = 1;
         // 
         // Label4
         // 
         this.Label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label4.Location = new System.Drawing.Point(8, 88);
         this.Label4.Name = "Label4";
         this.Label4.Size = new System.Drawing.Size(144, 24);
         this.Label4.TabIndex = 3;
         this.Label4.Text = "Days Before Expiration";
         this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Label5
         // 
         this.Label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.Label5.Location = new System.Drawing.Point(8, 24);
         this.Label5.Name = "Label5";
         this.Label5.Size = new System.Drawing.Size(144, 24);
         this.Label5.TabIndex = 2;
         this.Label5.Text = "Continuation Type";
         this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // GroupBox5
         // 
         this.GroupBox5.Controls.Add(this.chkUpdatesEnabled);
         this.GroupBox5.Controls.Add(this.chkNoHistorical);
         this.GroupBox5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox5.Location = new System.Drawing.Point(8, 292);
         this.GroupBox5.Name = "GroupBox5";
         this.GroupBox5.Size = new System.Drawing.Size(320, 60);
         this.GroupBox5.TabIndex = 5;
         this.GroupBox5.TabStop = false;
         this.GroupBox5.Text = "Updates";
         // 
         // chkUpdatesEnabled
         // 
         this.chkUpdatesEnabled.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
         this.chkUpdatesEnabled.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkUpdatesEnabled.Location = new System.Drawing.Point(24, 26);
         this.chkUpdatesEnabled.Name = "chkUpdatesEnabled";
         this.chkUpdatesEnabled.Size = new System.Drawing.Size(120, 18);
         this.chkUpdatesEnabled.TabIndex = 0;
         this.chkUpdatesEnabled.Text = "Updates Enabled";
         this.chkUpdatesEnabled.CheckedChanged += new System.EventHandler(this.chkUpdatesEnabled_CheckedChanged);
         // 
         // GroupBox6
         // 
         this.GroupBox6.Controls.Add(this.chkSFDailyFromIntraday);
         this.GroupBox6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.GroupBox6.Location = new System.Drawing.Point(336, 292);
         this.GroupBox6.Name = "GroupBox6";
         this.GroupBox6.Size = new System.Drawing.Size(220, 60);
         this.GroupBox6.TabIndex = 6;
         this.GroupBox6.TabStop = false;
         this.GroupBox6.Text = "Session Flags";
         // 
         // chkSFDailyFromIntraday
         // 
         this.chkSFDailyFromIntraday.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
         this.chkSFDailyFromIntraday.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkSFDailyFromIntraday.Location = new System.Drawing.Point(24, 26);
         this.chkSFDailyFromIntraday.Name = "chkSFDailyFromIntraday";
         this.chkSFDailyFromIntraday.Size = new System.Drawing.Size(184, 20);
         this.chkSFDailyFromIntraday.TabIndex = 0;
         this.chkSFDailyFromIntraday.Text = "Daily From Intraday";
         // 
         // GroupBox7
         // 
         this.GroupBox7.Controls.Add(this.chlstOutputs);
         this.GroupBox7.Location = new System.Drawing.Point(336, 160);
         this.GroupBox7.Name = "GroupBox7";
         this.GroupBox7.Size = new System.Drawing.Size(220, 128);
         this.GroupBox7.TabIndex = 10;
         this.GroupBox7.TabStop = false;
         this.GroupBox7.Text = "Outputs";
         // 
         // chlstOutputs
         // 
         this.chlstOutputs.Location = new System.Drawing.Point(8, 18);
         this.chlstOutputs.Name = "chlstOutputs";
         this.chlstOutputs.Size = new System.Drawing.Size(204, 106);
         this.chlstOutputs.TabIndex = 0;
         // 
         // chkNoHistorical
         // 
         this.chkNoHistorical.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
         this.chkNoHistorical.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.chkNoHistorical.Location = new System.Drawing.Point(160, 24);
         this.chkNoHistorical.Name = "chkNoHistorical";
         this.chkNoHistorical.Size = new System.Drawing.Size(128, 18);
         this.chkNoHistorical.TabIndex = 0;
         this.chkNoHistorical.Text = "No Historical";
         // 
         // frmTimedBarsRequestSettings
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
         this.ClientSize = new System.Drawing.Size(566, 447);
         this.Controls.Add(this.GroupBox7);
         this.Controls.Add(this.GroupBox6);
         this.Controls.Add(this.GroupBox5);
         this.Controls.Add(this.GroupBox4);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.txtSymbol);
         this.Controls.Add(this.GroupBox2);
         this.Controls.Add(this.GroupBox1);
         this.Controls.Add(this.Label1);
         this.Controls.Add(this.GroupBox3);
         this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmTimedBarsRequestSettings";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "TimedBars Request Settings";
         this.Load += new System.EventHandler(this.frmRequest_Load);
         this.GroupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.nudEndRange)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudStartRange)).EndInit();
         this.GroupBox3.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.nudIntradayPeriod)).EndInit();
         this.GroupBox2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.nudSessionsFilter)).EndInit();
         this.GroupBox4.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.nudDaysBeforeExpiration)).EndInit();
         this.GroupBox5.ResumeLayout(false);
         this.GroupBox6.ResumeLayout(false);
         this.GroupBox7.ResumeLayout(false);
         this.ResumeLayout(false);

      }
        
        #endregion
        
        // Default range start, range end and intraday period
        private const int DEF_RANGE_START = 0;
        private const int DEF_RANGE_END = - 10;
        
        // Current request
        private CQG.CQGTimedBarsRequest m_TimedBarsRequest;
        
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
        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            CQG.eHistoricalPeriod historicalPeriod;
            
            try
            {
                m_TimedBarsRequest.Symbol = txtSymbol.Text;
                
                if (optIntInt.Checked)
                {
                    m_TimedBarsRequest.RangeStart = System.Convert.ToInt32(nudStartRange.Value);
                    m_TimedBarsRequest.RangeEnd = System.Convert.ToInt32(nudEndRange.Value);
                }
                else if (optDateInt.Checked)
                {
                    m_TimedBarsRequest.RangeStart = dtpStartRange.Value;
                    m_TimedBarsRequest.RangeEnd = System.Convert.ToInt32(nudEndRange.Value);
                }
                else if (optDateDate.Checked)
                {
                    m_TimedBarsRequest.RangeStart = dtpStartRange.Value;
                    m_TimedBarsRequest.RangeEnd = dtpEndRange.Value;
                }
                m_TimedBarsRequest.IncludeEnd = chkIncludeEnd.Checked;
                
                historicalPeriod = (CQG.eHistoricalPeriod) cmbHistoricalPeriod.SelectedItem;
                if (historicalPeriod == CQG.eHistoricalPeriod.hpUndefined)
                {
                    m_TimedBarsRequest.IntradayPeriod = System.Convert.ToInt32(nudIntradayPeriod.Value);
                }
                else
                {
                    m_TimedBarsRequest.HistoricalPeriod = historicalPeriod;
                }

                m_TimedBarsRequest.TickFilter = (CQG.eTickFilter) cmbTickFilter.SelectedItem;
                
                m_TimedBarsRequest.Continuation = (CQG.eTimeSeriesContinuationType) cmbContinuationType.SelectedItem;
                m_TimedBarsRequest.EqualizeCloses = chkEqualizeCloses.Checked;
                m_TimedBarsRequest.DaysBeforeExpiration = System.Convert.ToInt32(nudDaysBeforeExpiration.Value);
                
                m_TimedBarsRequest.UpdatesEnabled = chkUpdatesEnabled.Checked;
                m_TimedBarsRequest.IgnoreEventsOnHistoricalBars = chkNoHistorical.Checked;
                
                if (chkSessionsCustomFilter.Checked)
                {
                    m_TimedBarsRequest.SessionsFilter = txtSessionsFilter.Text;
                }
                else
                {
                    m_TimedBarsRequest.SessionsFilter = System.Convert.ToInt32(nudSessionsFilter.Value);
                }
                
                if (chkSFDailyFromIntraday.Checked)
                {
                    m_TimedBarsRequest.SessionFlags = CQG.eSessionFlag.sfDailyFromIntraday;
                }
                else
                {
                    m_TimedBarsRequest.SessionFlags = CQG.eSessionFlag.sfUndefined;
                }
                
                m_TimedBarsRequest.ExcludeAllOutputs();
                foreach (CQG.eTimedBarsRequestOutputs selectedOutput in chlstOutputs.CheckedItems)
                {
                    m_TimedBarsRequest.IncludeOutput(selectedOutput, true);
                }
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "btnOK_Click", ex);
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
        private void btnCancel_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
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
                InitContinuationType();
                InitHistoricalPeriod();
                InitTickFilter();
            InitTBRequestOutputs();
                
                // Sets default values in the form
                nudIntradayPeriod.Value = m_TimedBarsRequest.IntradayPeriod;
                cmbHistoricalPeriod.SelectedItem = m_TimedBarsRequest.HistoricalPeriod;
                cmbTickFilter.SelectedItem = m_TimedBarsRequest.TickFilter;
                chkUpdatesEnabled.Checked = m_TimedBarsRequest.UpdatesEnabled;
                chkNoHistorical.Checked = m_TimedBarsRequest.IgnoreEventsOnHistoricalBars;
            chkNoHistorical.Enabled = chkUpdatesEnabled.Checked;
                chkIncludeEnd.Checked = m_TimedBarsRequest.IncludeEnd;
                nudSessionsFilter.Text = m_TimedBarsRequest.SessionsFilter.ToString();
                cmbContinuationType.SelectedItem = m_TimedBarsRequest.Continuation;
                chkEqualizeCloses.Checked = m_TimedBarsRequest.EqualizeCloses;
                nudDaysBeforeExpiration.Value = System.Convert.ToInt32(m_TimedBarsRequest.DaysBeforeExpiration);
                chkSFDailyFromIntraday.Checked = (m_TimedBarsRequest.SessionFlags & CQG.eSessionFlag.sfDailyFromIntraday) == CQG.eSessionFlag.sfDailyFromIntraday;
                
                RequestTypeChanged();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "frmRequest_Load", ex);
            }
        }
        
        /// <summary>
        /// Sets the new request reference
        /// </summary>
        /// <returns>
        /// New reference of CQGTimedBarsRequest
        /// </returns>
        public CQG.CQGTimedBarsRequest TimedBarsRequest
        {
            set
            {
                m_TimedBarsRequest = value;
            }
        }
        
        /// <summary>
        /// Sets the new time
        /// </summary>
        /// <returns>
        /// New time value
        /// </returns>
        public DateTime LineTime
        {
            set
            {
                m_LineTime = value;
            }
        }
        
        /// <summary>
        /// Changes NUD to textbox if CustomSession is checked.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void chkSessionsCustomFilter_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                txtSessionsFilter.Visible = chkSessionsCustomFilter.Checked;
                nudSessionsFilter.Visible = ! chkSessionsCustomFilter.Checked;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "chkSessionsCustomFilter_CheckedChanged", ex);
            }
        }
        
        /// <summary>
        /// Calls RequestTypeChanged sub to change the request type and control enablements
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void RequestType_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                RequestTypeChanged();
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "RequestType_CheckedChanged", ex);
            }
        }
        
        /// <summary>
        /// Initializes combobox of Continuation Types.
        /// </summary>
        private void InitContinuationType()
        {
            try
            {
                cmbContinuationType.Items.Clear();
                
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctNoContinuation);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctStandard);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctStandardByMonth);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctActive);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctActiveByMonth);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctAdjusted);
                cmbContinuationType.Items.Add(CQG.eTimeSeriesContinuationType.tsctAdjustedByMonth);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "InitContinuationType", ex);
            }
        }
        
        /// <summary>
        /// Initializes combobox of Historical Periods.
        /// </summary>
        private void InitHistoricalPeriod()
        {
            try
            {
                cmbHistoricalPeriod.Items.Clear();
                
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpUndefined);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpDaily);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpWeekly);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpMonthly);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpQuarterly);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpSemiannual);
                cmbHistoricalPeriod.Items.Add(CQG.eHistoricalPeriod.hpYearly);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "InitHistoricalPeriod", ex);
            }
        }

        /// <summary>
        /// Initializes combobox of Tick Filter.
        /// </summary>
        private void InitTickFilter()
        {
            try
            {
                cmbTickFilter.Items.Clear();

                cmbTickFilter.Items.Add(CQG.eTickFilter.tfAll);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfAsk);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfAskLow);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfBid);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfBidHigh);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfDefault);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfSettlement);
                cmbTickFilter.Items.Add(CQG.eTickFilter.tfTrade);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "InitTickFilter", ex);
            }
        }

        /// <summary>
        /// Initializes checked list box of timed bars request outputs
        /// </summary>
        private void InitTBRequestOutputs()
        {
            try
            {
                chlstOutputs.Items.Clear();

                chlstOutputs.Items.Add(CQG.eTimedBarsRequestOutputs.tbrActualVolume, true);
                chlstOutputs.Items.Add(CQG.eTimedBarsRequestOutputs.tbrTickVolume, true);
                chlstOutputs.Items.Add(CQG.eTimedBarsRequestOutputs.tbrAskVolume, true);
                chlstOutputs.Items.Add(CQG.eTimedBarsRequestOutputs.tbrBidVolume, true);
                chlstOutputs.Items.Add(CQG.eTimedBarsRequestOutputs.tbrOpenInterest, true);
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "InitTBRequestOutputs", ex);
            }
        }
        
        /// <summary>
        /// Enables NUD for Intraday Period, if hpUndefined is selected.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void cmbHistoricalPeriod_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                nudIntradayPeriod.Enabled = ((CQG.eHistoricalPeriod) cmbHistoricalPeriod.SelectedItem) == CQG.eHistoricalPeriod.hpUndefined;
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "cmbHistoricalPeriod_SelectedIndexChanged", ex);
            }
        }
        
        /// <summary>
        /// Changes Continuation Type related contols' enablements, depending from selected Continuation Type.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        private void cmbContinuationType_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                switch ((CQG.eTimeSeriesContinuationType) cmbContinuationType.SelectedItem)
                {
                    case CQG.eTimeSeriesContinuationType.tsctNoContinuation:
                        chkEqualizeCloses.Enabled = false;
                        nudDaysBeforeExpiration.Enabled = false;
                        break;
                        
                    case CQG.eTimeSeriesContinuationType.tsctStandard:
                        chkEqualizeCloses.Enabled = false;
                        nudDaysBeforeExpiration.Enabled = false;
                        break;
                        
                    case CQG.eTimeSeriesContinuationType.tsctStandardByMonth:
                        
                        chkEqualizeCloses.Enabled = false;
                        nudDaysBeforeExpiration.Enabled = false;
                        break;
                    case CQG.eTimeSeriesContinuationType.tsctActive:
                        chkEqualizeCloses.Enabled = true;
                        nudDaysBeforeExpiration.Enabled = false;
                        break;
                        
                    case CQG.eTimeSeriesContinuationType.tsctActiveByMonth:
                        
                        chkEqualizeCloses.Enabled = true;
                        nudDaysBeforeExpiration.Enabled = false;
                        break;
                    default:
                        
                        chkEqualizeCloses.Enabled = true;
                        nudDaysBeforeExpiration.Enabled = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "cmbContinuationType_SelectedIndexChanged", ex);
            }
        }
        
        /// <summary>
        /// Changes the request type and control enablements
        /// </summary>
        private void RequestTypeChanged()
        {
            try
            {
                if (optIntInt.Checked)
                {
                    dtpStartRange.Visible = false;
                    dtpEndRange.Visible = false;
                    
                    nudStartRange.Visible = true;
                    nudEndRange.Visible = true;
                    
                    nudStartRange.Value = DEF_RANGE_START;
                    nudEndRange.Value = DEF_RANGE_END;
                    
                    nudStartRange.Enabled = false;
                    chkIncludeEnd.Enabled = false;
                }
                else if (optDateInt.Checked)
                {
                    dtpStartRange.Visible = true;
                    dtpEndRange.Visible = false;
                    
                    nudStartRange.Visible = false;
                    nudEndRange.Visible = true;
                    
                    dtpStartRange.Value = m_LineTime.AddDays(-1);
                    nudEndRange.Value = DEF_RANGE_END;
                    
                    dtpStartRange.Enabled = true;
                    chkIncludeEnd.Enabled = false;
                }
                else if (optDateDate.Checked)
                {
                    dtpStartRange.Visible = true;
                    dtpEndRange.Visible = true;
                    
                    nudStartRange.Visible = false;
                    nudEndRange.Visible = false;
                    
                    dtpStartRange.Value = m_LineTime.AddDays(-1);
                    dtpEndRange.Value = m_LineTime;
                    
                    dtpStartRange.Enabled = true;
                    chkIncludeEnd.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                modErrorHandler.ShowError("frmTimedBarsRequestSettings", "RequestTypeChanged", ex);
            }
        }

        private void Label7_Click(object sender, System.EventArgs e)
        {
        
        }

      private void chkUpdatesEnabled_CheckedChanged(object sender, System.EventArgs e)
      {
         chkNoHistorical.Enabled = chkUpdatesEnabled.Checked;
      }
        
    }
    
}
