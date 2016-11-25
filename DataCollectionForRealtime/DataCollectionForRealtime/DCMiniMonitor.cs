using FakeCQG.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DataCollectionForRealtime
{
    public partial class DCMiniMonitor : Form
    {
        public static List<string> symbolsList;

        public DCMiniMonitor()
        {
            InitializeComponent();
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

        internal void SymbolsListsUpdate()
        {
            Action action = new Action(
                () =>
                {
                    SymbolsListTBox.Clear();

                    for (int i = 0; i < symbolsList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(symbolsList[i]))
                        {
                            SymbolsListTBox.Text += symbolsList[i] + "\n";
                            SymbolsListTBox.Select(SymbolsListTBox.Text.Length, SymbolsListTBox.Text.Length);
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

        private void DCMiniMonitor_Load(object sender, EventArgs e)
        {
            Program.MainForm.MainFormInitAndLoadActions();
        }

        private void MainFormCallBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.MainForm.Show();
        }

        private void ClearSymbolsListBtn_Click(object sender, EventArgs e)
        {
            symbolsList = new List<string>();
            SymbolsListTBox.Clear();
        }

        private void DCMiniMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.MainForm.Visible && ServerDictionaries.RealtimeIds.Count > 0)
            {
                string message = string.Concat("Are you sure that you want to stop fake CQG server? \nCurrently ",
                    ServerDictionaries.RealtimeIds.Count, " client(s) is/are connected to it.");
                string caption = "Data collector";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.WindowState = FormWindowState.Minimized;
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
