﻿using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FakeCQG.Internal;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataCollectionForRealtime
{
    public partial class DCMainForm : Form
    {
        #region Private fields

        private const int AutoWorkTimerInterval = 30;      // ms
        private const int HandshakingInterval = 3000;
        private int NoSubscribersCount = 0;
        //if there will be no handshake consecutive 2 times dictionaries will be cleaned
        private int NoSubscriberCountForCleanDictionaries = 2;

        private bool enteringMongoDBURL = false;

        private System.Timers.Timer AutoWorkTimer;

        private CQGDataManagement CqgDataManagement;

        private QueryHandler QueryHandler;

        #endregion

        #region Constructors

        public DCMainForm()
        {
            InitializeComponent();
            CenterToScreen();
            MinimumSize = Size;

            CqgDataManagement = new CQGDataManagement(this, Program.MiniMonitor);
            
            QueryHandler = new QueryHandler(CqgDataManagement);

            Listener.StartListening(HandshakingInterval);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;

            EventHandler.EventAppsSubscribersNum = new Dictionary<string, int>();
        }

        #endregion

        #region CQG helper methods

        internal void UpdateConnectionStatus(string connectionStatusLabel, Color connColor)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate()
                {
                    ConnectionStatusUpdateEffects(connectionStatusLabel, connColor);
                });
            }
            else
            {
                ConnectionStatusUpdateEffects(connectionStatusLabel, connColor);
            }
        }

        internal void ConnectionStatusUpdateEffects(string connectionStatusLabel, Color connColor)
        {
            connectionStatus.Text = connectionStatusLabel;
            connectionStatus.ForeColor = connColor;
            Program.MiniMonitor.BackColor = connColor;
        }

        internal void UpdateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
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

        public void UpdateStatusSubscribeData(String subcriptionMessage)
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

        #endregion

        #region Helpers DC event handlers and methods

        private void Listener_SubscribersAdded(HandshakingEventArgs args)
        {
            var subscribersArray = new HandshakingInfo[ServerDictionaries.RealtimeIds.Count];
            ServerDictionaries.RealtimeIds.CopyTo(subscribersArray);
            ServerDictionaries.RealtimeIds.Clear();
            if (!args.NoSubscribers)
            {
                var subscribersList = new List<HandshakingInfo>(subscribersArray);
                foreach (HandshakingInfo subscriber in args.Subscribers)
                {
                    subscribersList.Remove(subscriber);
                    ServerDictionaries.RealtimeIds.Add(subscriber);
                }

                if (subscribersList.Count > 0)
                {
                    // Handle last queries for unsubscribe items
                    QueryHandler.ReadQueries();
                    QueryHandler.ProcessEntireQueryList();

                    foreach (var item in subscribersList)
                    {
                        UnsubscribeEvents(item);
                        ServerDictionaries.DeleteFromServerDictionaries(item);
                        Listener.DeleteUnsubscriber(item.ID);
                    }
                }
                NoSubscribersCount = 0;
            }
            else
            {
                NoSubscribersCount++;
                if(NoSubscribersCount >= 2)
                {
                    ServerDictionaries.ClearAllDictionaries();
                }
            }
        }

        private void UnsubscribeEvents(HandshakingInfo subscriber)
        {
            foreach (var eventInfor in subscriber.UnsubscribeEventList)
            {
                foreach (var dic in eventInfor.Value)
                {
                    if (dic.Value)
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(eventInfor.Key);
                        System.Reflection.EventInfo ei = qObj.GetType().GetEvent(dic.Key);

                        // Find corresponding CQG delegate
                        Type delType = QueryHandler.FindDelegateType(QueryHandler.CQGAssembly, dic.Key);

                        // Instantiate the delegate with our own handler
                        string handlerName = string.Format("_ICQGCELEvents_{0}EventHandlerImpl", dic.Key);

                        MethodInfo handlerInfo = typeof(CQGEventHandlers).GetMethod(handlerName);
                        Delegate d = Delegate.CreateDelegate(delType, handlerInfo);

                        // Unsubscribe our handler from CQG event
                        ei.RemoveEventHandler(qObj, d);
                    }
                }
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

        private void CQG_LogChange(string message)
        {
            AsyncTaskListener.LogMessage(message);
        }

        #endregion

        #region Timers elapsed handlers

        //Automatic processing of queries
        private void AutoWorkTimer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            QueryHandler.ReadQueries();
            QueryHandler.ProcessEntireQueryList();

            if (checkBoxAuto.Checked)
            {
                AutoWorkTimer.Start();
            }
        }

        #endregion

        #region Controls event handlers

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            Core.LogChange += CQG_LogChange;

            QueryHandler.HelpersInit();

            AutoWorkTimer = new System.Timers.Timer();
            AutoWorkTimer.Elapsed += AutoWorkTimer_Elapsed;
            AutoWorkTimer.Interval = AutoWorkTimerInterval;
            AutoWorkTimer.AutoReset = false;

            foreach (string name in Enum.GetNames(typeof(LogModeEnum)))
            {
                comboBoxLogMode.Items.Add(name);
            }
            comboBoxLogMode.SelectedIndexChanged += LogModeComboBox_SelectedIndexChanged;
            comboBoxLogMode.SelectedIndex = (int)LogModeEnum.Filtered;
        }

        private void LogModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Core.LogMode = (LogModeEnum)Enum.GetValues(typeof(LogModeEnum)).GetValue(comboBoxLogMode.SelectedIndex);
        }

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            QueryHandler.ReadQueries();
        }

        private void ButtonRespond_Click(object sender, EventArgs e)
        {
            QueryHandler.ProcessEntireQueryList();
        }

        private void CheckBoxAuto_CheckedChanged(object sender, EventArgs e)
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

        private void MinimizeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.MiniMonitor.Show();
        }

        private void ChangeURLOfMongoDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enteringMongoDBURL = enteringMongoDBURL ? false : true;
            textBoxMongoDbUrl.Visible = enteringMongoDBURL;
            labelMongoDbUrl.Visible = enteringMongoDBURL;
            buttonChangeMongoDbUrl.Visible = enteringMongoDBURL;
            textBoxMongoDbUrl.Text = ConnectionSettings.ConnectionString;
        }

        private void ChangeDBURLBtn_Click(object sender, EventArgs e)
        {
            if (ConnectionSettings.ConnectionString != textBoxMongoDbUrl.Text && textBoxMongoDbUrl.Text != "")
            {
                ConnectionSettings.ConnectionString = textBoxMongoDbUrl.Text;
                QueryHandler.HelpersInit();
            }

            textBoxMongoDbUrl.Visible = false;
            labelMongoDbUrl.Visible = false;
            buttonChangeMongoDbUrl.Visible = false;
            enteringMongoDBURL = false;
        }

        private void DCMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Program.MiniMonitor.Visible && ServerDictionaries.RealtimeIds.Count > 0)
            {
                string message = string.Format("Are you sure that you want to stop fake CQG server? \nCurrently {0} client(s) is/are connected to it.",
                ServerDictionaries.RealtimeIds.Count);
                string caption = "Data collector";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.WindowState = FormWindowState.Minimized;
                    CqgDataManagement.shutDownCQGConn();
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                CqgDataManagement.shutDownCQGConn();
            }
        }

        private void DCMainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (ServerDictionaries.RealtimeIds.Count > 0)
            {
                string eventKey = Core.CreateUniqueKey();

                EventHandler.EventAppsSubscribersNum.Add("DCClosed", Listener.SubscribersCount);

                var eventInfo = new FakeCQG.Internal.Models.EventInfo(eventKey, "DCClosed", numOfSubscribers: Listener.SubscribersCount);

                Task.Run(() => EventHandler.FireEvent(eventInfo));

                QueryHandler.ReadQueries(false);
                if (QueryHandler.QueryList.Count > 0)
                {
                    QueryHandler.ProcessEntireQueryList();
                }

                Environment.Exit(0);
            }
            else
            {
                CqgDataManagement.shutDownCQGConn();
            }
        }

        #endregion

    }
}
