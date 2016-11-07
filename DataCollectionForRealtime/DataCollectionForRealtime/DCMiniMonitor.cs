using FakeCQG.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataCollectionForRealtime
{
    public partial class DCMiniMonitor : Form
    {
        public static List<string> instrumentsList;
        public static List<string> symbolsList;

        public DCMiniMonitor()
        {
            InitializeComponent();
            instrumentsList = new List<string>();
            symbolsList = new List<string>();
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
            if (ServerDictionaries.RealtimeIds.Count > 0)
            {
                string message = string.Concat("Are you sure that you want to stop fake CQG server? \nCurrently ", 
                    ServerDictionaries.RealtimeIds.Count, " client(s) is/are connected to it.");
                string caption = "Data collector";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Program.MainForm.Close();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Program.MainForm.Close();
            }
            
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

        internal void SymbolsAndInstrumentsListsUpdate()
        {
            Action action = new Action(
                () =>
                {
                    instrumentsListTextBox.Clear();
                    symbolsListTextBox.Clear();

                    for (int i = 0; i < symbolsList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(symbolsList[i]))
                        {
                            symbolsListTextBox.Text += symbolsList[i] + "\n";
                            symbolsListTextBox.Select(symbolsListTextBox.Text.Length, symbolsListTextBox.Text.Length);
                        }
                    }

                    for (int i = 0; i < instrumentsList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(instrumentsList[i]))
                        {
                            instrumentsListTextBox.Text += instrumentsList[i] + "\n";
                            instrumentsListTextBox.Select(instrumentsListTextBox.Text.Length, instrumentsListTextBox.Text.Length);
                        }
                    }

                });
            
            try
            {
                if (Program.MainForm.IsHandleCreated)
                {
                    Invoke(action);
                }
            }
            catch (ObjectDisposedException)
            {
                // User closed the form
            }
        }
    }
}
