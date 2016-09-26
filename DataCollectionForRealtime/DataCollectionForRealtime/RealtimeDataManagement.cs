using System;
using System.Drawing;
using System.Windows.Forms;
using CQGLibrary.HandShaking;


using FakeCQG;
using FakeCQG.Helpers;
using FakeCQG.Models;
using CQGLibrary.Models;
using System.Collections.Generic;

namespace DataCollectionForRealtime
{
    public partial class RealtimeDataManagement : Form
    {
        System.Timers.Timer AutoWorkTimer;

        const int AutoWorkTimerInterval = 1000; // 1s

        CQGDataManagement cqgDataManagement;

        int timeForHandShaking = 5000;

        QueryHandler queryHandler;

        public RealtimeDataManagement()
        {
            InitializeComponent();

            cqgDataManagement = new CQGDataManagement(this);

            queryHandler = new QueryHandler(cqgDataManagement);

            Listener.StartListerning(timeForHandShaking);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;
        }

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            FakeCQG.CQG.ClearQueriesListAsync();
            FakeCQG.CQG.ClearAnswersListAsync();
            FakeCQG.CQG.LogChange += CQG_LogChange;
            FakeCQG.CQG.GetQueries += queryHandler.SetQueryList;

            AutoWorkTimer = new System.Timers.Timer();
            AutoWorkTimer.Elapsed += AutoWorkTimer_Elapsed;
            AutoWorkTimer.Interval = AutoWorkTimerInterval;
            AutoWorkTimer.AutoReset = false;
        }

        private void Listener_SubscribersAdded(HandShakingEventArgs args)
        {
            DataDictionaries.RealTimeIds = new HashSet<Guid>();
            if (!args.NoSubscribers)
            {
                foreach (var subscriber in args.Subscribers)
                {
                    DataDictionaries.RealTimeIds.Add(subscriber.ID);
                }
            }
            else
            {
                DataDictionaries.ClearAllDictionaris();
            }
        }

        internal void updateConnectionStatus(string connectionStatusLabel, Color connColor)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                { 
                    connectionStatus.Text = connectionStatusLabel;
                    connectionStatus.ForeColor = connColor;
                });
            }
            else
            {
                connectionStatus.Text = connectionStatusLabel;
                connectionStatus.ForeColor = connColor;
            }
        }

        internal void updateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
        {
#if DEBUG
            try
#endif
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate()
                    {
                        this.dataStatus.ForeColor = foreColor;
                        this.dataStatus.BackColor = backColor;
                        this.dataStatus.Text = dataStatus;
                    });
                }
                else
                {
                    this.dataStatus.ForeColor = foreColor;
                    this.dataStatus.BackColor = backColor;
                    this.dataStatus.Text = dataStatus;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void updateStatusSubscribeData(String subcriptionMessage)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    statusSubscribeData.Text = subcriptionMessage;
                });
            }
            else
            {
                statusSubscribeData.Text = subcriptionMessage;
            }
        }

        private void AsyncTaskListener_Updated(string message = null)
        {
            Action action = new Action(
                () =>
                {
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        richTextBoxLog.Text += message + "\n";
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

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            queryHandler.CheckRequestsQueue();
        }

        private void buttonRespond_Click(object sender, EventArgs e)
        {
            queryHandler.ProcessEntireQueryList();
        }     

        private void CQG_LogChange(string message)
        {
            AsyncTaskListener.LogMessage(message);
        }

        private void checkBoxAuto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Checked)
            {
                AutoWorkTimer.Start();
            }
            else
            {
                AutoWorkTimer.Stop();
            }
        }

        private void AutoWorkTimer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            queryHandler.CheckRequestsQueue();
            queryHandler.ProcessEntireQueryList();
            if (checkBoxAuto.Checked)
            {
                AutoWorkTimer.Start();
            }
        }
    }
}
