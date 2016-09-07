using System;
using System.Windows.Forms;
using FakeCQG;
using FakeCQG.Models;

namespace TestRealTime
{
    public partial class MainForm : Form
    {
        private FakeCQG.CQG cqg;
        private FakeCQG.CQGCELClass cqgcel;

        public MainForm()
        {
            InitializeComponent();
            AsyncTaskListener.Updated += AsyncTaskListener_Updated;

        }

        private void AsyncTaskListener_Updated(string msg)
        {
            Action action = new Action(() =>
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        richTextBoxLog.Text += msg + "\n";
                        richTextBoxLog.Select(richTextBoxLog.Text.Length, richTextBoxLog.Text.Length);
                        richTextBoxLog.ScrollToCaret();
                    }
                });
            try
            {
                Invoke(action);
            }
            catch (ObjectDisposedException)
            {
                // User closed the form
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //cqg = new FakeCQG.CQG();
            cqgcel = new FakeCQG.CQGCELClass();
            FakeCQG.CQG.LogChange += CQG_LogChange;
        }

        private void CQG_LogChange(string message)
        {
            AsyncTaskListener.LogMessage(message);
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            FakeCQG.CQG.ReadQueriesAsync();
        }

        private void checkBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            buttonRead.Enabled = !checkBoxAuto.Checked;
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            FakeCQG.CQG.ClearQueriesListAsync();
        }

        private async void buttonCall_Click(object sender, EventArgs e)
        {
            //bool isStarted = cqgcel.IsStarted;
            await FakeCQG.CQG.LoadInQueryAsync(new QueryInfo(QueryInfo.QueryType.Property, "key", string.Empty, "name", null, null));
        }
    }
}
