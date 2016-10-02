using System;
using System.Drawing;
using System.Windows.Forms;
using FakeCQG.Handshaking;
using FakeCQG.Models;

namespace DataCollectionForRealtime
{
    public partial class RealtimeDataManagement : Form
    {
        const int AutoWorkTimerInterval = 30;      // ms
        const int HandshakingTimerInterval = 15000;
        const int DictionaryClearingInterval = 30000;

        System.Timers.Timer AutoWorkTimer;
        Timer HandshakingTimer = new Timer();

        CQGDataManagement CqgDataManagement;

        QueryHandler QueryHandler;

        public RealtimeDataManagement()
        {
            InitializeComponent();

            CqgDataManagement = new CQGDataManagement(this);

            QueryHandler = new QueryHandler(CqgDataManagement);

            Listener.StartListening(HandshakingTimerInterval);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;

            HandshakingTimer.Disposed += HandshakingTimer_Disposed;
            HandshakingTimer.Interval = DictionaryClearingInterval;
        }

        private void HandshakingTimer_Disposed(object sender, EventArgs e)
        {
            FakeCQG.ServerDictionaries.ClearAllDictionaries();
        }

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            FakeCQG.CQG.LogChange += CQG_LogChange;

            FakeCQG.CQG.QueryHelper = new FakeCQG.Helpers.QueryHelper();
            FakeCQG.CQG.QueryHelper.ClearQueriesListAsync();
            FakeCQG.CQG.QueryHelper.NewQueriesReady += QueryHandler.SetQueryList;

            FakeCQG.CQG.AnswerHelper = new FakeCQG.Helpers.AnswerHelper();
            FakeCQG.CQG.AnswerHelper.ClearAnswersListAsync();

            FakeCQG.CQG.EventHelper = new FakeCQG.Helpers.EventHelper();
            FakeCQG.CQG.EventHelper.ClearEventsListAsync();

            AutoWorkTimer = new System.Timers.Timer();
            AutoWorkTimer.Elapsed += AutoWorkTimer_Elapsed;
            AutoWorkTimer.Interval = AutoWorkTimerInterval;
            AutoWorkTimer.AutoReset = false;
        }

        private void Listener_SubscribersAdded(HandshakingEventArgs args)
        {
            if (!args.NoSubscribers)
            {
                foreach (HandshakingModel subscriber in args.Subscribers)
                {
                    FakeCQG.ServerDictionaries.RealtimeIds.Add(subscriber.ID);
                }
                HandshakingTimer.Stop();
            }
            else
            {
                HandshakingTimer.Start();
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
            QueryHandler.CheckRequestsQueue();
        }

        private void buttonRespond_Click(object sender, EventArgs e)
        {
            QueryHandler.ProcessEntireQueryList();
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
            QueryHandler.CheckRequestsQueue();
            QueryHandler.ProcessEntireQueryList();

            if (checkBoxAuto.Checked)
            {
                AutoWorkTimer.Start();
            }
        }
    }
}
