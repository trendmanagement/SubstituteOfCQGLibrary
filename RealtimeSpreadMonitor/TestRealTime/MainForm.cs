﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using FakeCQG.Models;
using CQGLibrary.HandShaking;

namespace TestRealTime
{
    public partial class MainForm : Form
    {
        private FakeCQG.CQG cqg;
        private FakeCQG.CQGCELClass cqgcel;

        public MainForm()
        {
            InitializeComponent();
            Subscriber.ListenForHanshaking();
            FakeCQG.CQG.LogChange += CQG_LogChange;
            FakeCQG.CQG.GetQueries += CQG_GetQueries;
            AsyncTaskListener.Updated += AsyncTaskListener_Updated;
        }

        private void CQG_GetQueries(System.Collections.Generic.List<QueryInfo> queries)
        {
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
            Task.Run(() => { cqgcel = new FakeCQG.CQGCELClass(); });
        }

        private void CQG_LogChange(string message)
        {
            Task.Run(() => { AsyncTaskListener.LogMessage(message); });
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            bool test = FakeCQG.CQG.CheckAnswerAsync("key").GetAwaiter().GetResult();
            AsyncTaskListener.LogMessage(test.ToString());
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
            bool isStarted = cqgcel.IsStarted;
            cqgcel.Shutdown();
            await FakeCQG.CQG.LoadInQueryAsync(new QueryInfo(QueryInfo.QueryType.Property, "key", string.Empty, "name", null, null));
        }
    }
}
