using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FakeCQG.Internal;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;

namespace DataCollectionForRealtime
{
    public partial class DCMainForm : Form
    {
        const int AutoWorkTimerInterval = 30;      // ms
        const int HandshakingTimerInterval = 10000;
        const int DictionaryClearingInterval = 30000;
		
		bool enteringMongoDBURL = false;

        System.Timers.Timer AutoWorkTimer;
        Timer HandshakingTimer = new Timer();

        CQGDataManagement CqgDataManagement;

        QueryHandler QueryHandler;

        public DCMainForm()
        {
            InitializeComponent();
            CenterToScreen();

            CqgDataManagement = new CQGDataManagement(this, Program.MiniMonitor);
            
            QueryHandler = new QueryHandler(CqgDataManagement);

            Listener.StartListening(HandshakingTimerInterval);

            Listener.SubscribersAdded += Listener_SubscribersAdded;

            AsyncTaskListener.Updated += AsyncTaskListener_Updated;

            HandshakingTimer.Disposed += HandshakingTimer_Disposed;
            HandshakingTimer.Interval = DictionaryClearingInterval;
        }

        private void HandshakingTimer_Disposed(object sender, EventArgs e)
        {
            ServerDictionaries.ClearAllDictionaries();
        }

        private void RealtimeDataManagement_Load(object sender, EventArgs e)
        {
            Core.LogChange += CQG_LogChange;

            QueryHandler.HelpersInit();

            AutoWorkTimer = new System.Timers.Timer();
            AutoWorkTimer.Elapsed += AutoWorkTimer_Elapsed;
            AutoWorkTimer.Interval = AutoWorkTimerInterval;
            AutoWorkTimer.AutoReset = false;

            logSettingsComboBox.Items.Add("Off  log");
            logSettingsComboBox.Items.Add("Separate log");
            logSettingsComboBox.Items.Add("All log");
            logSettingsComboBox.SelectedIndex = 1;
            logSettingsComboBox.SelectedIndexChanged += LogSettingsComboBox_SelectedIndexChanged;
        }

        private void LogSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           Core.LogSettings = logSettingsComboBox.SelectedIndex;
        }

        private void Listener_SubscribersAdded(HandshakingEventArgs args)
        {
            if (!args.NoSubscribers)
            {
                foreach (HandshakingModel subscriber in args.Subscribers)
                {
                    if (subscriber.Unsubscribe)
                    {
                        UnsubscribeEvents(subscriber);
                        ServerDictionaries.DeleteFromServerDictionaries(subscriber);
                        Listener.DeleteUnsubscriber(subscriber.ID);
                    }
                    else
                    {
                        ServerDictionaries.RealtimeIds.Add(subscriber.ID);
                    }
                }
                HandshakingTimer.Stop();
            }
            else
            {
                HandshakingTimer.Start();
            }
        }

        private void UnsubscribeEvents(HandshakingModel subscriber)
        {
            foreach(var eventInfor in subscriber.UnsubscribeEventList)
            {
                object qObj = ServerDictionaries.GetObjectFromTheDictionary(eventInfor.Key);
                System.Reflection.EventInfo ei = qObj.GetType().GetEvent(eventInfor.Value);

                // Find corresponding CQG delegate
                Type delType = QueryHandler.FindDelegateType(QueryHandler.CQGAssembly, eventInfor.Value);

                // Instantiate the delegate with our own handler
                string handlerName = string.Format("_ICQGCELEvents_{0}EventHandlerImpl", eventInfor.Value);

                MethodInfo handlerInfo = typeof(CQGEventHandlers).GetMethod(handlerName);
                Delegate d = Delegate.CreateDelegate(delType, handlerInfo);

                // Unsubscribe our handler from CQG event
                ei.RemoveEventHandler(qObj, d);
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

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            QueryHandler.CheckRequestsQueue();
        }

        private void ButtonRespond_Click(object sender, EventArgs e)
        {
            QueryHandler.ProcessEntireQueryList();
        }

        private void CQG_LogChange(string message)
        {
            AsyncTaskListener.LogMessage(message);
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

        private void AutoWorkTimer_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            QueryHandler.CheckRequestsQueue();
            QueryHandler.ProcessEntireQueryList();

            if (checkBoxAuto.Checked)
            {
                AutoWorkTimer.Start();
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
            MongoDBURL.Visible = enteringMongoDBURL;
            MongoDBURLLabel.Visible = enteringMongoDBURL;
            ChangeDBURLBtn.Visible = enteringMongoDBURL;
            MongoDBURL.Text = FakeCQG.Internal.Helpers.ConnectionSettings.ConnectionString;
        }

        private void ChangeDBURLBtn_Click(object sender, EventArgs e)
        {
            if (ConnectionSettings.ConnectionString != MongoDBURL.Text && MongoDBURL.Text != "")
            {
                ConnectionSettings.ConnectionString = MongoDBURL.Text;
                QueryHandler.HelpersInit();
            }

            MongoDBURL.Visible = false;
            MongoDBURLLabel.Visible = false;
            ChangeDBURLBtn.Visible = false;
            enteringMongoDBURL = false;
        }

        private void DCMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CqgDataManagement.shutDownCQGConn();
        }
    }
}
