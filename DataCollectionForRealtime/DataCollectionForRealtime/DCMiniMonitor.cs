﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataCollectionForRealtime
{
    public partial class DCMiniMonitor : Form
    {
        public DCMiniMonitor()
        {
            InitializeComponent();  
        }

        internal void updateConnectionStatus(string connectionStatusLabel, Color connColor)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    this.BackColor = connColor;
                });
            }
            else
            {
                this.BackColor = connColor;
            }
        }

        private void DCMiniMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.MainForm.Close();
        }

        private void MainFormCall_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.MainForm.Show();
        }

        public void SetNumberOfQueriesInLine(int num)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        queriesNumDsplLbl.Text = num.ToString();
                    });
                }
                else
                {
                    queriesNumDsplLbl.Text = num.ToString();
                }
            }
            catch (Exception)
            {
                SetNumberOfQueriesInLine(num);
            }
            queriesNumDsplLbl.Text = num.ToString();
        }

    }
}