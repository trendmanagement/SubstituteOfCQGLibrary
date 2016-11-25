using System;
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
        private const int HandshakingTimerInterval = 3000;
        private const int DictionaryClearingInterval = 30000;

        private bool enteringMongoDBURL = false;
        private bool initActionsDone = false;
        private bool loadActionsDone = false;

        private System.Timers.Timer AutoWorkTimer;
        private System.Timers.Timer HandshakingTimer = new System.Timers.Timer();

        private CQGDataManagement CqgDataManagement;

        private QueryHandler QueryHandler;
        #endregion

        #region Constructors

        public DCMainForm()
        {
            if (!initActionsDone)
            {
                InitActions();
                initActionsDone = true;
            }
        }

        #endregion

        #region CQG helper methods

        private void InitActions()
        {
            InitializeComponent();
            CenterToScreen();
            MinimumSize = Size;

            CqgDataManagement = new CQGDataManagement(this, Program.MiniMonitor);

            QueryHandler = new QueryHandler(CqgDataManagement);

            QueryHandler.InitHMethodDict();

            Listener.StartListening(HandshakingTimerInterval);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;

            HandshakingTimer.Disposed += HandshakingTimer_Disposed;
            HandshakingTimer.Interval = DictionaryClearingInterval;
            HandshakingTimer.AutoReset = false;

            EventHandler.EventAppsSubscribersNum = new Dictionary<string, int>();
        }

        private void LoadingActions()
        {
            Core.LogChange += CQG_LogChange;

            QueryHandler.HelpersInit();

            AutoWorkTimer = new System.Timers.Timer();
            AutoWorkTimer.Elapsed += AutoWorkTimer_Elapsed;
            AutoWorkTimer.Interval = AutoWorkTimerInterval;
            AutoWorkTimer.AutoReset = false;
            AutoWorkTimer.Start();

            var names = Enum.GetNames(typeof(LogModeEnum));

            for (int i = 0; i < names.Length; i++)
            {
                comboBoxLogMode.Items.Add(names[i]);
            }
            comboBoxLogMode.SelectedIndexChanged += LogModeComboBox_SelectedIndexChanged;
            comboBoxLogMode.SelectedIndex = (int)LogModeEnum.Filtered;
        }

        public void MainFormInitAndLoadActions()
        {
            if (!initActionsDone)
            {
                InitActions();
                initActionsDone = true;
            }

            if (!loadActionsDone)
            {
                LoadingActions();
                loadActionsDone = true;
            }
        }

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
                TSErrorCatch.errorCatchOut(string.Concat(this), ex);
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
                for (int i = 0; i < args.Subscribers.Count; i++)
                {
                    subscribersList.Remove(args.Subscribers[i]);
                    ServerDictionaries.RealtimeIds.Add(args.Subscribers[i]);
                }

                if (subscribersList.Count > 0)
                {
                    // Handle last queries for unsubscribe items
                    QueryHandler.ReadQueries();
                    QueryHandler.ProcessEntireQueryList();

                    for (int i = 0; i < subscribersList.Count; i++)
                    {
                        UnsubscribeEvents(subscribersList[i]);
                        ServerDictionaries.DeleteFromServerDictionaries(subscribersList[i]);
                        Listener.DeleteUnsubscriber(subscribersList[i].ID);
                    }
                }
                HandshakingTimer.Stop();
            }
            else
            {
                HandshakingTimer.Start();
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
                        string handlerName = string.Concat("_ICQGCELEvents_", dic.Key, "EventHandlerImpl");

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
                        LogRTBox.Text += message + "\n";
                        LogRTBox.Select(LogRTBox.Text.Length, LogRTBox.Text.Length);
                        LogRTBox.ScrollToCaret();
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
            try
            {
                QueryHandler.ReadQueries();
                QueryHandler.ProcessEntireQueryList();

            }
            finally
            {
                if (AutomaticProcCheckBox.Checked)
                {
                    AutoWorkTimer.Start();
                }

            }
        }

        private void HandshakingTimer_Disposed(object sender, EventArgs e)
        {
            try
            {
                ServerDictionaries.ClearAllDictionaries();
            }
            finally
            {
                HandshakingTimer.Start();
            }
        }

        #endregion

        #region Controls event handlers

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            if (!loadActionsDone)
            {
                LoadingActions();
                loadActionsDone = true;
            }
        }

        private void LogModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Core.LogMode = (LogModeEnum)Enum.GetValues(typeof(LogModeEnum)).GetValue(comboBoxLogMode.SelectedIndex);
        }

        private void CheckQueriesBtn_Click(object sender, EventArgs e)
        {
            QueryHandler.ReadQueries();
        }

        private void ProcessAndSendAnswerBtn_Click(object sender, EventArgs e)
        {
            QueryHandler.ProcessEntireQueryList();
        }

        private void AutomaticProcCheckBox_CheckedChanged(object sender, EventArgs e)
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

        private void MiniMonitorCallTSMI_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.MiniMonitor.Show();
        }

        private void ChangeMongoDbUrlTSMI_Click(object sender, EventArgs e)
        {
            enteringMongoDBURL = enteringMongoDBURL ? false : true;
            MongoDbUrlTBox.Visible = enteringMongoDBURL;
            MongoDbUrlLbl.Visible = enteringMongoDBURL;
            ChangeMongoDbUrlBtn.Visible = enteringMongoDBURL;
            MongoDbUrlTBox.Text = ConnectionSettings.ConnectionString;
        }

        private void ChangeDBURLBtn_Click(object sender, EventArgs e)
        {
            if (ConnectionSettings.ConnectionString != MongoDbUrlTBox.Text && MongoDbUrlTBox.Text != "")
            {
                ConnectionSettings.ConnectionString = MongoDbUrlTBox.Text;
                QueryHandler.HelpersInit();
            }

            MongoDbUrlTBox.Visible = false;
            MongoDbUrlLbl.Visible = false;
            ChangeMongoDbUrlBtn.Visible = false;
            enteringMongoDBURL = false;
        }

        private void DCMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerDictionaries.RealtimeIds.Count > 0)
            {
                string message = string.Concat("Are you sure that you want to stop fake CQG server? \nCurrently ",
                    ServerDictionaries.RealtimeIds.Count, " client(s) is/are connected to it.");
                string caption = "Data collector";
                if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    CqgDataManagement.shutDownCQGConn();
                    Program.MiniMonitor.Close();
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
                Program.MiniMonitor.Close();
            }
        }

        private void DCMainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            if (ServerDictionaries.RealtimeIds.Count > 0)
            {
                Program.MainForm.Visible = false;

                string eventKey = Core.CreateUniqueKey();

                EventHandler.EventAppsSubscribersNum.Add("DCClosed", Listener.SubscribersCount);

                var eventInfo = new FakeCQG.Internal.Models.EventInfo(eventKey, "DCClosed", numOfSubscribers: Listener.SubscribersCount);

                EventHandler.FireEvent(eventInfo);

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
