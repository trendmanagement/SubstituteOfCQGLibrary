using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using CQGLibrary.HandShaking;


using FakeCQG;
using FakeCQG.Helpers;
using FakeCQG.Models;
using CQGLibrary.Models;

namespace DataCollectionForRealtime
{
    public partial class RealtimeDataManagement : Form
    {

        private CQGDataManagement cqgDataManagement;

        private QueryHandler queryHandler;

        int timeForHandShaking = 5000;

        public RealtimeDataManagement()
        {
            InitializeComponent();

            cqgDataManagement = new CQGDataManagement(this);

            queryHandler = new QueryHandler(cqgDataManagement);

            Listener.StartListerning(timeForHandShaking);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;
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

        internal void updateConnectionStatus(string connectionStatusLabel,
            Color connColor)
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

        private void buttonFill_Click(object sender, EventArgs e)
        {
            //TODO: Before filling we must:
            // - collect queries                           TODO: Create collector for combine query
            // - get data from CQG by combined qeury
            // - divide answer for smoler answer by Key    TODO: Create answer divider 
            // - fill osems smoler answer's by Key 
            queryHandler.ProcessEntireQueryList();
        }     

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            FakeCQG.CQG.ClearQueriesListAsync();
            FakeCQG.CQG.LogChange += CQG_LogChange;
            FakeCQG.CQG.GetQueries += queryHandler.SetQueryList;
        }

        private void CQG_LogChange(string message)
        {
            AsyncTaskListener.LogMessage(message);
        }

    }
}
