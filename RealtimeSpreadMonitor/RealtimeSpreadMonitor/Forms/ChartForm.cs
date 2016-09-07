using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class ChartForm : Form
    {
        //bool maximizedChart = false;
        

        public ChartForm()
        {
            //this.IsMdiContainer = true;
            InitializeComponent();
        }

        //public void maximizeChart(OrderChart orderChart)
        //{
        //    //TSErrorCatch.debugWriteOut("test");

        //    //this.tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;

        //    if (maximizedChart)
        //    {
        //        float normalHeight = 100 / tableLayoutPanel1.RowStyles.Count;
        //        float normalWidth = 100 / tableLayoutPanel1.ColumnStyles.Count;

        //        for (int rowCnt = 0; rowCnt < tableLayoutPanel1.RowStyles.Count; rowCnt++)
        //        {
        //            tableLayoutPanel1.RowStyles[rowCnt].Height = normalHeight;
        //        }

        //        for (int colCnt = 0; colCnt < tableLayoutPanel1.ColumnStyles.Count; colCnt++)
        //        {
        //            tableLayoutPanel1.ColumnStyles[colCnt].Width = normalWidth;
        //        }

        //        maximizedChart = false;
        //    }
        //    else
        //    {
        //        //normalHeight = tableLayoutPanel1.RowStyles[orderChart.row].Height;
        //        //normalWidth = tableLayoutPanel1.ColumnStyles[orderChart.col].Width;

        //        tableLayoutPanel1.RowStyles[orderChart.row].Height = 100;
        //        tableLayoutPanel1.ColumnStyles[orderChart.col].Width = 100;

        //        maximizedChart = true;
        //    }
            
        //}

        private void ChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Visible = false;
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        public void initializeLayout()
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }
    }
}
