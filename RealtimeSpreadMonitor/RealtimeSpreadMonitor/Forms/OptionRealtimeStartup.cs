using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class OptionRealtimeStartup : Form
    {
        delegate void ThreadSafeUpdateCQGConnectionStatusDelegate(String connectStatus, Color connColor);
        delegate void ThreadSafeUpdateCQGDataStatusDelegate(String dataStatus, Color backColor, Color foreColor);

        delegate void ThreadSafeUpdateCQGReconnectBtn(bool enabled);

        delegate void ThreadSafeShutdownForm();

        //private TML_TradingSystemAboutBox tml_TradingSystemAboutBox;
        private InitializationParms initializationParms;
        private OptionSpreadManager optionSpreadManager;

        private OptionStartupProgress optionStartupProgress = new OptionStartupProgress();

        private BackgroundWorker backgroundWorkerProgressBar;
        
        private bool initialized = false;

        public OptionRealtimeStartup(InitializationParms initializationParms)
        {
            this.initializationParms = initializationParms;

            InitializeComponent();

            this.Text = "CONTROL PORTFOLIO: "
                + initializationParms.portfolioGroupName + "  Version:"
                + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //modelDateStatus.Text = "MODEL DATE: " + initializationParms.modelDateTime
            //    .ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);

            //disableButtons();

            optionSpreadManager = new OptionSpreadManager(initializationParms, this,
                optionStartupProgress);

            optionStartupProgress.Show();

            //optionSpreadManager.passVariablesToOptionSpreadManager(initializationParms, this, optionStartupProgress);

            ThreadPool.QueueUserWorkItem(new WaitCallback(optionSpreadManager.initializeOptionSystem));

            initializeBackgroundWorkerProgressBar();

        }

        

        public void finishedInitializing(bool finished)
        {
            this.initialized = finished;
        }

        private void initializeBackgroundWorkerProgressBar()
        {
            try
            {
                backgroundWorkerProgressBar = new BackgroundWorker();
                backgroundWorkerProgressBar.WorkerReportsProgress = true;
                backgroundWorkerProgressBar.WorkerSupportsCancellation = true;

                backgroundWorkerProgressBar.DoWork +=
                    new DoWorkEventHandler(setupBackgroundWorkerProgressBar);

                backgroundWorkerProgressBar.RunWorkerCompleted +=
                    new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                backgroundWorkerProgressBar.RunWorkerAsync();

                
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void setupBackgroundWorkerProgressBar(object sender,
            DoWorkEventArgs e)
        {
            //this.Invoke(new EventHandler(optionSpreadManager.openThread));

            optionSpreadManager.openThread(null, null);

            try
            {
                while (!initialized)
                {
                    //backgroundWorkerProgressBar.ReportProgress(0);
                    System.Threading.Thread.Sleep(1000);
                }


            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            optionSpreadManager.closeThread(null, null);

            //this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            optionStartupProgress.Close();

            optionSpreadManager.startOptionSpreadGUIs();

            //this.Close();
            this.Visible = false;
            //enableButtons();
        }

        //private void disableButtons()
        //{
        //    btnCallAllInstruments.Enabled = false;
        //    btnCallUnsubscribed.Enabled = false;
        //    //btnRefreshData.Enabled = false;
        //    btnResetAllInstruments.Enabled = false;
        //}

        //private void enableButtons()
        //{
        //    btnCallAllInstruments.Enabled = true;
        //    btnCallUnsubscribed.Enabled = true;
        //    //btnRefreshData.Enabled = true;
        //    btnResetAllInstruments.Enabled = true;

        //    this.Cursor = Cursors.Default;
        //}

        //private void btnAbout_Click(object sender, EventArgs e)
        //{
        //    if (tml_TradingSystemAboutBox != null)
        //    {
        //        tml_TradingSystemAboutBox.Show();
        //    }
        //    else
        //    {
        //        tml_TradingSystemAboutBox = new TML_TradingSystemAboutBox();
        //        tml_TradingSystemAboutBox.Show();
        //    }
        //}

        //private void btnInitialize_Click(object sender, EventArgs e)
        //{
        //    btnCallAllInstruments.Enabled = false;


        //    optionSpreadManager.callOptionRealTimeData(false);


        //    btnCallAllInstruments.Enabled = true;
        //}

        //public void updateCQGReConnectBtn(bool enable)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeUpdateCQGReconnectBtn d = new ThreadSafeUpdateCQGReconnectBtn(threadSafeUpdateCQGReConnectBtn);

        //        this.Invoke(d, enable);
        //    }
        //    else
        //    {
        //        threadSafeUpdateCQGReConnectBtn(enable);
        //    }
        //}

//        private void threadSafeUpdateCQGReConnectBtn(bool enable)
//        {
//            btnCQGRecon.Enabled = enable;
//        }        

//        public void updateCQGConnectionStatus(String connectStatus, Color connColor)
//        {
//#if DEBUG
//            try
//#endif
//            {
//                if (this.InvokeRequired)
//                {
//                    ThreadSafeUpdateCQGConnectionStatusDelegate d = new ThreadSafeUpdateCQGConnectionStatusDelegate(threadSafeUpdateCQGConnectionStatus);

//                    this.Invoke(d, connectStatus, connColor);
//                }
//                else
//                {
//                    threadSafeUpdateCQGConnectionStatus(connectStatus, connColor);
//                }
//            }
//#if DEBUG
//            catch (Exception ex)
//            {
//                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
//            }
//#endif
//        }

//        public void threadSafeUpdateCQGConnectionStatus(String connectStatus, Color connColor)
//        {
//            this.connectionStatus.Text = connectStatus;
//            this.connectionStatus.ForeColor = connColor;
//        }

//        public void updateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
//        {
//#if DEBUG
//            try
//#endif
//            {
//                if (this.InvokeRequired)
//                {
//                    ThreadSafeUpdateCQGDataStatusDelegate d = new ThreadSafeUpdateCQGDataStatusDelegate(threadSafeUpdateCQGDataStatus);

//                    this.Invoke(d, dataStatus, backColor, foreColor);
//                }
//                else
//                {
//                    threadSafeUpdateCQGDataStatus(dataStatus, backColor, foreColor);
//                }
//            }
//#if DEBUG
//            catch (Exception ex)
//            {
//                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
//            }
//#endif
//        }

//        public void threadSafeUpdateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
//        {
//            this.dataStatus.ForeColor = foreColor;
//            this.dataStatus.BackColor = backColor;
//            this.dataStatus.Text = dataStatus;
//        }

        private void OptionRealtimeStartup_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;

            //Cursor.Current = Cursors.WaitCursor;
            //ParameterizedThreadStart shutdownForm();
            
            //Thread shutDownFormThread
            // = new Thread(new ParameterizedThreadStart(shutdownForm));
            //shutDownFormThread.IsBackground = true;
            //shutDownFormThread.Start();
            
            //optionSpreadManager.shutDownOptionSpreadRealtime();
        }

        private void OptionRealtimeStartup_Load(object sender, EventArgs e)
        {

        }



        //private void btnCallUnsubscribedInstruments_Click(object sender, EventArgs e)
        //{
        //    btnCallAllInstruments.Enabled = false;

        //    //optionSpreadManager.callOptionRealTimeData(true);

        //    btnCallAllInstruments.Enabled = true;
        //}

        //private void btnReconnectCQG_Click(object sender, EventArgs e)
        //{
        //    //optionSpreadManager.reConnectCQG();
        //}

        //private void toolStripButtonAbout_Click(object sender, EventArgs e)
        //{
        //    if (tml_TradingSystemAboutBox != null)
        //    {
        //        tml_TradingSystemAboutBox.Show();
        //    }
        //    else
        //    {
        //        tml_TradingSystemAboutBox = new TML_TradingSystemAboutBox();
        //        tml_TradingSystemAboutBox.Show();
        //    }
        //}

        //private void btnCallAllInstruments_Click(object sender, EventArgs e)
        //{
        //    //btnCallAllInstruments.Enabled = false;
        //    disableButtons();

        //    //optionSpreadManager.partialReConnectCQG();

        //    optionSpreadManager.callOptionRealTimeData(false);

        //    enableButtons();
        //    //btnCallAllInstruments.Enabled = true;
        //}

        //private void btnCallUnsubscribed_Click(object sender, EventArgs e)
        //{
        //    disableButtons();

        //    optionSpreadManager.callOptionRealTimeData(true);

        //    enableButtons();
        //}

        //private void btnResetAllInstruments_Click(object sender, EventArgs e)
        //{
        //    optionSpreadManager.fullReConnectCQG();
        //}

        //private void btnCQGRecon_Click(object sender, EventArgs e)
        //{
        //    optionSpreadManager.reInitializeCQG();
        //}

        //private void toolStripButton1_Click(object sender, EventArgs e)
        //{

        //}
    }
}
