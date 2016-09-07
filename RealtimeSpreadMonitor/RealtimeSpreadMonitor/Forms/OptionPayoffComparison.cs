using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class OptionPayoffComparison : Form
    {
        public OptionPayoffComparison()
        {
            InitializeComponent();
        }

        internal OptionPayoffComparisonUserControl addTabForInstrument(string instrumentName)
        {
            //string title = "TabPage " + (tabControlInstruments.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(instrumentName);
            tabControlInstruments.TabPages.Add(myTabPage);

            OptionPayoffComparisonUserControl optionPayoffComparisonUserControl = 
                new OptionPayoffComparisonUserControl();

            myTabPage.Controls.Add(optionPayoffComparisonUserControl);

            optionPayoffComparisonUserControl.Dock = DockStyle.Fill;

            return optionPayoffComparisonUserControl;
        }
    }
}
