namespace TMLCommonMethodsAndTypes.Option_Section
{
    partial class OptionPLChart
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionPLChart));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridViewSpreadGrid = new System.Windows.Forms.DataGridView();
            this.btnChart = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.daysTextBox = new System.Windows.Forms.TextBox();
            this.volTextBox = new System.Windows.Forms.TextBox();
            this.riskFreeTextBox = new System.Windows.Forms.TextBox();
            this.btnVolFill = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDaysFill = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.optionPLChartDataMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSpreadGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.optionPLChartDataMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.DarkGray;
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.HorizontalCenter;
            this.chart1.BackSecondaryColor = System.Drawing.Color.White;
            this.chart1.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart1.BorderlineWidth = 2;
            this.chart1.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.IsStartedFromZero = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.ScaleView.SmallScrollMinSize = 0.1D;
            chartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Silver;
            chartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black;
            chartArea1.AxisX2.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX2.IsStartedFromZero = false;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.ScaleBreakStyle.Spacing = 0.1D;
            chartArea1.AxisY.ScaleView.SmallScrollMinSize = 0.1D;
            chartArea1.AxisY.ScrollBar.ButtonColor = System.Drawing.Color.Silver;
            chartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black;
            chartArea1.AxisY2.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY2.IsStartedFromZero = false;
            chartArea1.AxisY2.ScaleView.SmallScrollMinSize = 0.1D;
            chartArea1.BackColor = System.Drawing.Color.Gainsboro;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorX.SelectionColor = System.Drawing.Color.Gray;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.CursorY.SelectionColor = System.Drawing.Color.Gray;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 85F;
            chartArea1.InnerPlotPosition.Width = 90F;
            chartArea1.InnerPlotPosition.X = 8F;
            chartArea1.InnerPlotPosition.Y = 6F;
            chartArea1.Name = "Default";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 80F;
            chartArea1.Position.Width = 88F;
            chartArea1.Position.X = 5F;
            chartArea1.Position.Y = 5F;
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend2";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            this.chart1.Size = new System.Drawing.Size(569, 356);
            this.chart1.TabIndex = 2;
            this.chart1.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridViewSpreadGrid);
            this.splitContainer1.Size = new System.Drawing.Size(569, 546);
            this.splitContainer1.SplitterDistance = 356;
            this.splitContainer1.TabIndex = 3;
            // 
            // gridViewSpreadGrid
            // 
            this.gridViewSpreadGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewSpreadGrid.ContextMenuStrip = this.optionPLChartDataMenuStrip;
            this.gridViewSpreadGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewSpreadGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridViewSpreadGrid.Location = new System.Drawing.Point(0, 0);
            this.gridViewSpreadGrid.MultiSelect = false;
            this.gridViewSpreadGrid.Name = "gridViewSpreadGrid";
            this.gridViewSpreadGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridViewSpreadGrid.Size = new System.Drawing.Size(569, 186);
            this.gridViewSpreadGrid.TabIndex = 0;
            // 
            // btnChart
            // 
            this.btnChart.Location = new System.Drawing.Point(28, 360);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(62, 20);
            this.btnChart.TabIndex = 4;
            this.btnChart.Text = "Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.daysTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.volTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.riskFreeTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.btnVolFill);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.btnDaysFill);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.btnChart);
            this.splitContainer2.Size = new System.Drawing.Size(718, 546);
            this.splitContainer2.SplitterDistance = 569;
            this.splitContainer2.TabIndex = 5;
            // 
            // daysTextBox
            // 
            this.daysTextBox.Location = new System.Drawing.Point(53, 453);
            this.daysTextBox.Name = "daysTextBox";
            this.daysTextBox.Size = new System.Drawing.Size(37, 20);
            this.daysTextBox.TabIndex = 15;
            this.daysTextBox.Text = "1.0";
            // 
            // volTextBox
            // 
            this.volTextBox.Location = new System.Drawing.Point(53, 428);
            this.volTextBox.Name = "volTextBox";
            this.volTextBox.Size = new System.Drawing.Size(37, 20);
            this.volTextBox.TabIndex = 14;
            this.volTextBox.Text = "1.0";
            // 
            // riskFreeTextBox
            // 
            this.riskFreeTextBox.Location = new System.Drawing.Point(53, 402);
            this.riskFreeTextBox.Name = "riskFreeTextBox";
            this.riskFreeTextBox.Size = new System.Drawing.Size(37, 20);
            this.riskFreeTextBox.TabIndex = 13;
            this.riskFreeTextBox.Text = "1.0";
            // 
            // btnVolFill
            // 
            this.btnVolFill.Location = new System.Drawing.Point(96, 427);
            this.btnVolFill.Name = "btnVolFill";
            this.btnVolFill.Size = new System.Drawing.Size(37, 20);
            this.btnVolFill.TabIndex = 12;
            this.btnVolFill.Text = "Fill";
            this.btnVolFill.UseVisualStyleBackColor = true;
            this.btnVolFill.Click += new System.EventHandler(this.btnVolFill_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 431);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "VOL";
            // 
            // btnDaysFill
            // 
            this.btnDaysFill.Location = new System.Drawing.Point(96, 453);
            this.btnDaysFill.Name = "btnDaysFill";
            this.btnDaysFill.Size = new System.Drawing.Size(37, 20);
            this.btnDaysFill.TabIndex = 9;
            this.btnDaysFill.Text = "Fill";
            this.btnDaysFill.UseVisualStyleBackColor = true;
            this.btnDaysFill.Click += new System.EventHandler(this.btnDaysFill_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "DAYS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 405);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "RFR";
            // 
            // optionPLChartDataMenuStrip
            // 
            this.optionPLChartDataMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowMenuItem,
            this.deleteRowMenuItem});
            this.optionPLChartDataMenuStrip.Name = "contextMenuStrip1";
            this.optionPLChartDataMenuStrip.Size = new System.Drawing.Size(134, 48);
            // 
            // addRowMenuItem
            // 
            this.addRowMenuItem.Name = "addRowMenuItem";
            this.addRowMenuItem.Size = new System.Drawing.Size(133, 22);
            this.addRowMenuItem.Text = "Add Row";
            this.addRowMenuItem.Click += new System.EventHandler(this.addRowMenuItem_Click);
            // 
            // deleteRowMenuItem
            // 
            this.deleteRowMenuItem.Name = "deleteRowMenuItem";
            this.deleteRowMenuItem.Size = new System.Drawing.Size(133, 22);
            this.deleteRowMenuItem.Text = "Delete Row";
            this.deleteRowMenuItem.Click += new System.EventHandler(this.deleteRowMenuItem_Click);
            // 
            // OptionPLChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 546);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OptionPLChart";
            this.Text = "Strategy Chart";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSpreadGrid)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.optionPLChartDataMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridViewSpreadGrid;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDaysFill;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnVolFill;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox riskFreeTextBox;
        private System.Windows.Forms.TextBox daysTextBox;
        private System.Windows.Forms.TextBox volTextBox;
        private System.Windows.Forms.ContextMenuStrip optionPLChartDataMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowMenuItem;
    }
}