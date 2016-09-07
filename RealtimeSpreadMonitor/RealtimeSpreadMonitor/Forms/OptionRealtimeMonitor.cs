using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
//using CQG;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Windows.Threading;
using RealtimeSpreadMonitor.FormManipulation;
using System.Media;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class OptionRealtimeMonitor : Form
    {
        private bool continueUpdating = true;

        private DateTime previousTime = DateTime.Now;

        private BackgroundWorker backgroundWorkerOptionRealtimeMonitor;

        delegate void ThreadSafeMoveSplitterDistance(int splitterDistance);

        delegate void ThreadSafeFillLiveDataGridViewPageDelegate(DataGridView gridView, int row, int col, String displayValue,
            bool updateColor, double value);

        delegate void ThreadSafeFillLiveDataPageDelegate(int row, int col, String displayValue,
            bool updateColor, double value);

        delegate void ThreadSafeHideUnhideContractSummaryLiveDataDelegate(DataGridView gridView, int row, bool visible);

        //delegate void ThreadSafeMarkAsConnectedDelegate(DataGridView gridView, int row, int col, bool connected, Color backgroundColor);

        delegate void ThreadSafeFillGridViewValueAsStringAndColorDelegate(int rowToUpdate, int col, string displayValue, Color color);

        delegate void ThreadSafeUpdateStatusStripOptionMonitorDelegate();

        delegate void ThreadSafeUpdateStatusSubscribeData(String subcriptionMessage);

        

        delegate void ThreadSafeUpdateInstrumentSummaryDelegate(int row, int col, double val);

        //delegate void ThreadSafeUpdateListViewDelegate(ListView listView, int row, int col, String val);

        //delegate void ThreadSafeUpdateBackColorListViewDelegate(ListView listView, int row, Color backColor);

        //delegate void ThreadSafeAddItemToListView(ListView listView, ListViewItem listViewItem);

        //delegate void ThreadSafeRemoveItemFromListView(ListView listView, int itemRow);

        delegate void ThreadSafeUpdateButtonText(ToolStripButton toolStripBtn, string buttonText);

        //delegate void ThreadSafeFillGridModelADMComparison();

        //delegate void ThreadSafeBeginEndUpdateList(bool begin);

        delegate void ThreadSafeGenericDelegateWithoutParams();

        //variables passed from OptionSpreadManager
        private OptionSpreadManager optionSpreadManager;

        private OptionStrategy[] optionStrategies;
        private Instrument[] instruments;

        private DataTable portfolioSummaryDataTable = new DataTable();
        private DataTable portfolioSummarySettlementDataTable = new DataTable();

        //private DataTable contractSummaryLiveDataTable = new DataTable();

        private LiveSpreadTotals[] instrumentSpreadTotals;
        private LiveSpreadTotals portfolioSpreadTotals;

        private LiveSpreadTotals[,] instrumentADMSpreadTotals;
        private LiveSpreadTotals[] portfolioADMSpreadTotals;

        private OptionArrayTypes optionArrayTypes;

        private List<OptionSpreadExpression> optionSpreadExpressionList;

        public Settings settings;

        //private ChartForm orderChartForm = null;
        //private bool orderChartInstantiated = false;

        private FileLoadList fileLoadListForm = null;



        private List<int> contractSummaryExpressionListIdx;  // = new List<int>();



        private bool hideUnhideInstrumentsInSummaryPL = true;

        internal DataGridView getGridViewContractSummary
        {
            get { return gridViewContractSummary; }
        }

        private GridViewFCMPostionManipulation gridViewFCMPostionManipulation;

        //private ModelADMCompareCalculationAndDisplay modelADMCompareCalculationAndDisplay;

        internal DataGridView getGridLiveFCMData
        {
            get { return gridLiveFCMData; }
        }

        internal DataGridView getGridViewModelADMCompare
        {
            get { return gridViewModelADMCompare; }
        }

        public OptionRealtimeMonitor(
            OptionSpreadManager optionSpreadManager,
            OptionStrategy[] optionStrategies, Instrument[] instruments,
            LiveSpreadTotals[] instrumentSpreadTotals, LiveSpreadTotals portfolioSpreadTotals,
            LiveSpreadTotals[,] instrumentADMSpreadTotals, LiveSpreadTotals[] portfolioADMSpreadTotals,
            OptionArrayTypes optionArrayTypes, List<int> contractSummaryExpressionListIdx,
            List<ADMPositionImportWeb> admPositionImportWebListForCompare)
        {
            gridViewFCMPostionManipulation = new GridViewFCMPostionManipulation(optionSpreadManager);

            //modelADMCompareCalculationAndDisplay = new ModelADMCompareCalculationAndDisplay(optionSpreadManager);

            this.optionSpreadManager = optionSpreadManager;

            this.optionStrategies = optionStrategies;

            this.instruments = instruments;

            this.instrumentSpreadTotals = instrumentSpreadTotals;

            this.portfolioSpreadTotals = portfolioSpreadTotals;

            this.instrumentADMSpreadTotals = instrumentADMSpreadTotals;

            this.portfolioADMSpreadTotals = portfolioADMSpreadTotals;

            this.optionArrayTypes = optionArrayTypes;

            this.contractSummaryExpressionListIdx = contractSummaryExpressionListIdx;

            //this.optionSpreadManager.admPositionImportWebListForCompare = optionSpreadManager.admPositionImportWebListForCompare;

            InitializeComponent();


            this.Text =
                //"NEW VERSION PORTFOLIO: "                 
                optionSpreadManager.initializationParmsSaved.portfolioGroupName + "  Version:"
                + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            modelDateStatus.ToolTipText = "MODEL DATE: " + optionSpreadManager.initializationParmsSaved.modelDateTime
                .ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            modelDateStatus.Text = optionSpreadManager.initializationParmsSaved.modelDateTime
                .ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);



            initializeRealtimeMonitor();

            updateStatusStripOptionMonitor();

            Type orderPlacementTypes = typeof(StageOrdersToTTWPFLibrary.Enums.ORDER_PLACEMENT_TYPE);
            Array orderPlacementTypesArray = Enum.GetNames(orderPlacementTypes);

            for (int i = 0; i < orderPlacementTypesArray.Length; i++)
            {
                cmbxOrderPlacementType.Items.Add(orderPlacementTypesArray.GetValue(i).ToString());
            }

            cmbxOrderPlacementType.SelectedIndex = 0;



            updateCQGConnectionStatus(optionSpreadManager.ConnectionStatusString,
                optionSpreadManager.ConnectionStatusColor, optionSpreadManager.ConnectionStatusShortString);

            if (optionSpreadManager.supplementContractFilled)
            {
                toolStripStatusLabelUsingSupplementContract.Text = "SUP CON";
                toolStripStatusLabelUsingSupplementContract.ToolTipText = "SUPPLEMENT CONTRACTS FILE FILLED";
                toolStripStatusLabelUsingSupplementContract.BackColor = Color.Aqua;
            }
            else
            {
                toolStripStatusLabelUsingSupplementContract.Text = "";
            }
        }




        public void initializeRealtimeMonitor()
        {
            setupGridLiveData();

            setupInstrumentSummary();

            setupInstrumentSummaryStrategyRRisk();

            setupPortfolioSummary();

            setupPortfolioSummarySettlements();

            setupGridOrders();

            setupOrderSummaryList();

            setupStrategyStateFields();

            setupRollStrikes();

            gridViewFCMPostionManipulation.setupGridLiveADMData(this);

            optionSpreadManager.modelADMCompareCalculationAndDisplay.setupGridModelADMComparison(this);

            setupExpressionListGridView();

            setupPreviousPLAnalysis(dataGridPreviousModelPriceCompare, dataGridPreviousModelPL, true);

            setupPreviousPLAnalysis(dataGridPreviousFCMPriceCompare, dataGridPreviousFCMPL, false);


        }

        public void updateStatusSubscribeData(String subcriptionMessage)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeUpdateStatusSubscribeData d = new ThreadSafeUpdateStatusSubscribeData(updateStatusSubscribeDataThreadSafe);

                this.Invoke(d, subcriptionMessage);
            }
            else
            {
                updateStatusSubscribeDataThreadSafe(subcriptionMessage);
            }
        }

        private void updateStatusSubscribeDataThreadSafe(String subcriptionMessage)
        {
            statusSubscribeData.Text = subcriptionMessage;

            //statusSubscribeData.BackColor = Color.Yellow;
        }


        public void updateStatusStripOptionMonitor()
        {
            if (this.InvokeRequired)
            {
                ThreadSafeUpdateStatusStripOptionMonitorDelegate d = new ThreadSafeUpdateStatusStripOptionMonitorDelegate(updateStatusStripOptionMonitorThreadSafe);

                this.Invoke(d);
            }
            else
            {
                updateStatusStripOptionMonitorThreadSafe();
            }
        }

        private void updateStatusStripOptionMonitorThreadSafe()
        {

            if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
            {
                statusPriceType.ToolTipText = "OPTION THEORETICAL AND FUTURE SETTLEMENTS";

                statusPriceType.Text = "OPT THEO SETL";

                statusPriceType.BackColor = Color.Yellow;

                statusEODSettlement.ToolTipText = "END OF DAY ANALYSIS";

                statusEODSettlement.Text = "EOD";

                statusEODSettlement.BackColor = Color.Yellow;
            }
            else
            {
                statusPriceType.ToolTipText = Enum.GetName(typeof(REALTIME_PRICE_FILL_TYPE), optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType).Replace('_', ' ');

                switch (optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType)
                {
                    case REALTIME_PRICE_FILL_TYPE.PRICE_DEFAULT:
                        statusPriceType.Text = "PRC DEF";
                        break;

                    case REALTIME_PRICE_FILL_TYPE.PRICE_ASK:
                        statusPriceType.Text = "PRC ASK";
                        break;

                    case REALTIME_PRICE_FILL_TYPE.PRICE_MID_BID_ASK:
                        statusPriceType.Text = "PRC MID";
                        break;

                    case REALTIME_PRICE_FILL_TYPE.PRICE_BID:
                        statusPriceType.Text = "PRC BID";
                        break;

                    case REALTIME_PRICE_FILL_TYPE.PRICE_THEORETICAL:
                        statusPriceType.Text = "PRC THEOR";
                        break;
                }



                statusPriceType.BackColor = Color.LawnGreen;

                statusEODSettlement.Text = "";
            }
        }

        delegate void ThreadSafeUpdateStatusDataFillingDelegate(
            int expressionsUpdated, int totalExpressions,
            bool measuredVolume, bool volumeOk);

        public void updateStatusDataFilling()
        {
            int subscribedCount = 0;
            bool cumVolGreaterThan0 = true;
            bool measuredVolume = false;

            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                
                if (optionSpreadExpressionList[i].setSubscriptionLevel)
                {
                    subscribedCount++;
                }

                if (optionSpreadExpressionList[i].decisionBar != null)
                {
                    measuredVolume = true;

                    if(cumVolGreaterThan0
                        && optionSpreadExpressionList[i].decisionBar.cumulativeVolume == 0)
                    {
                        cumVolGreaterThan0 = false;
                    }
                }
                
            }

            if (this.InvokeRequired)
            {
                ThreadSafeUpdateStatusDataFillingDelegate d =
                    new ThreadSafeUpdateStatusDataFillingDelegate(updateStatusDataFillingThreadSafe);

                this.Invoke(d, subscribedCount, optionSpreadExpressionList.Count,
                    measuredVolume, cumVolGreaterThan0);
            }
            else
            {
                updateStatusDataFillingThreadSafe(subscribedCount, optionSpreadExpressionList.Count,
                    measuredVolume, cumVolGreaterThan0);
            }
        }

        private void updateStatusDataFillingThreadSafe(int expressionsUpdated, int totalExpressions,
            bool measuredVolume, bool volumeOk)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("UPDATE STATUS: ");
            sb.Append(expressionsUpdated);
            sb.Append(" OF ");
            sb.Append(totalExpressions);

            if (measuredVolume)
            {
                sb.Append(" VOLUME ");

                if (volumeOk)
                {
                    sb.Append(" GOOD");
                }
                else
                {
                    sb.Append(" ERROR");
                }
            }

            statusOfUpdatedInstruments.Text = sb.ToString();

            StringBuilder sbText = new StringBuilder();
            sbText.Append("UPDATE STATUS: ");
            sbText.Append(expressionsUpdated);
            sbText.Append(" OF ");
            sbText.Append(totalExpressions);
            sbText.Append(" UPDATED");

            statusOfUpdatedInstruments.ToolTipText = sbText.ToString();

            if (expressionsUpdated < totalExpressions || !volumeOk)
            {
                statusOfUpdatedInstruments.BackColor = Color.Red;
            }
            else
            {
                statusOfUpdatedInstruments.BackColor = Color.LawnGreen;
            }
        }



        public void passExpressionListToRealtimeMonitor(
            List<OptionSpreadExpression> optionSpreadExpressionList)
        {
            this.optionSpreadExpressionList = optionSpreadExpressionList;
        }

        public void realtimeMonitorStartupBackgroundUpdateLoop()
        {
            initializeOptionSummaryRealtimeBackgroundWorker();
        }

        //used for running the reconnect to CQG timers
        private System.Threading.Timer timer1SetupTimeForCQGData;
        private System.Threading.Timer timer2SetupTimeForCQGData;

        

        public void initializeOptionSummaryRealtimeBackgroundWorker()
        {
            
            
            setUpTimerForCQGDataReset(optionSpreadManager.dataResetTime1, 0);

            
            setUpTimerForCQGDataReset(optionSpreadManager.dataResetTime2, 1);

            //try
            //{
            backgroundWorkerOptionRealtimeMonitor = new BackgroundWorker();
            backgroundWorkerOptionRealtimeMonitor.WorkerReportsProgress = true;
            backgroundWorkerOptionRealtimeMonitor.WorkerSupportsCancellation = true;

            backgroundWorkerOptionRealtimeMonitor.DoWork +=
                new DoWorkEventHandler(setupBackgroundWorkerOptionSummaryRealtime);

            backgroundWorkerOptionRealtimeMonitor.ProgressChanged +=
                new ProgressChangedEventHandler(
                    backgroundWorkerOptionSummaryRealtime_ProgressChanged);

            backgroundWorkerOptionRealtimeMonitor.RunWorkerAsync();
            //}
            //catch (Exception ex)
            //{
            //    TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            //}
        }

        private void setUpTimerForCQGDataReset(TimeSpan alertTime, int timerToUse)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }


            System.Threading.TimerCallback timerDelegate = resetConnectionToCQGAtTimeInvoke;
            //new System.Threading.TimerCallback(resetConnectionToCQGAtTimeInvoke);

            System.Threading.Timer timer = new System.Threading.Timer(
                timerDelegate, null, timeToGo, Timeout.InfiniteTimeSpan);

            switch (timerToUse)
            {
                case 0:
                    timer1SetupTimeForCQGData = timer;
                    break;
                case 1:
                    timer2SetupTimeForCQGData = timer; ;
                    break;
            }


        }

        private void resetConnectionToCQGAtTimeInvoke(object StateObj)
        {
            //TSErrorCatch.debugWriteOut("test************************");

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(resetConnectionToCQGAtTime));
            }
            else
            {
                resetConnectionToCQGAtTime();
            }
        }

        private void resetConnectionToCQGAtTime()
        {
            optionSpreadManager.fullReConnectCQG();
            Thread.Sleep(2000);
            optionSpreadManager.callOptionRealTimeData(false);
        }

        

        private bool updateOptionRealtimeMonitorGUI = true;

        public void setupBackgroundWorkerOptionSummaryRealtime(object sender,
            DoWorkEventArgs e)
        {
            //this.Invoke(new EventHandler(optionSpreadManager.openThread));

            optionSpreadManager.openThread(null, null);

            //{
            while (continueUpdating)
            {
                System.Threading.Thread.Sleep(TradingSystemConstants.OPTIONREALTIMEREFRESH);

                if (updateOptionRealtimeMonitorGUI)
                {
                    updateOptionRealtimeMonitorGUI = false;

                    backgroundWorkerOptionRealtimeMonitor.ReportProgress(0);
                }





            }
            //}
            //catch (Exception ex)
            //{
            //    TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            //}

            optionSpreadManager.closeThread(null, null);

            //this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        }



        

        public void backgroundWorkerOptionSummaryRealtime_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {

            if (continueUpdating)
            {
                if (optionSpreadManager.fxceConnected)
                {
                    toolStripFixConnectionStatus.Text = "TTFIX UP";
                    toolStripFixConnectionStatus.BackColor = Color.LawnGreen;
                }
                else
                {
                    toolStripFixConnectionStatus.Text = "TTFIX DN";
                    toolStripFixConnectionStatus.BackColor = Color.Red;
                }

                //DateTime currentTime = DateTime.Now;

                ////int startRangeMinute1 = 0;
                //int compareHour1 = 7;
                //int compareTimeMinute1 = 5;
                //int stopRangeMinute1 = 10;                

                //int compareHour2 = 8;
                //int startRangeMinute2 = 25;                
                //int compareTimeMinute2 = 30;
                //int stopRangeMinute2 = 35;

                //if ((currentTime.Hour == compareHour1 && currentTime.Minute < stopRangeMinute1) ||
                //    (currentTime.Hour == 14 && currentTime.Minute > startRangeMinute2 
                //    && currentTime.Minute < stopRangeMinute2))
                //{
                //    DateTime compareToTime = new DateTime();

                //    if (currentTime.Hour == compareHour1 && currentTime.Minute < stopRangeMinute1)
                //    {
                //        compareToTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                //            compareHour1, compareTimeMinute1, 0);
                //    }
                //    else if (currentTime.Hour == compareHour2 && currentTime.Minute > startRangeMinute2 
                //        && currentTime.Minute < stopRangeMinute2)
                //    {
                //        compareToTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                //            compareHour2, compareTimeMinute2, 0);
                //    }

                //    //DateTime compareToTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                //    //    8, 7, 0);

                //    if (currentTime.CompareTo(compareToTime) >= 0
                //        &&
                //        previousTime.CompareTo(compareToTime) < 0)
                //    {
                //        optionSpreadManager.fullReConnectCQG();
                //        Thread.Sleep(2000);
                //        optionSpreadManager.callOptionRealTimeData(false);
                //    }

                //    previousTime = currentTime;
                //}

                //*******************
                //update without new thread datatable
                sendUpdateToPortfolioTotalGrid();

                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                {
                    sendUpdateToPortfolioTotalSettlementGrid();
                }

                fillOrderSummaryList();
                //*******************


                Thread callUpdateLivePage = new Thread(new ParameterizedThreadStart(updateLiveDataPage));
                callUpdateLivePage.IsBackground = true;
                callUpdateLivePage.Start();

                //updateLiveDataPage(null);

                //DateTime ct = DateTime.Now;
                //TSErrorCatch.debugWriteOut(ct.ToString("HH:mm:ss", DateTimeFormatInfo.InvariantInfo));

                updateOptionRealtimeMonitorGUI = true;


                //Thread.Sleep(7000);
                //updateLiveDataPage(null);

            }

        }

        public void updateLiveDataPage(Object obj)
        {
            this.Invoke(new EventHandler(optionSpreadManager.openThread));

            //if (setupLiveGrid)
            {
                DateTime currentTime = DateTime.Now;
                bool markedEODAnalysisAtInstrument = false;
                for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                {
                    if (!instruments[instrumentCnt].eodAnalysisAtInstrument
                        && currentTime.CompareTo(instruments[instrumentCnt].settlementDateTimeMarker) > 0)
                    {
                        instruments[instrumentCnt].eodAnalysisAtInstrument = true;
                        markedEODAnalysisAtInstrument = true;

                        optionSpreadManager.fillRollIntoLegExpressions(instrumentCnt);
                    }
                }

                if (markedEODAnalysisAtInstrument)
                {
                    //recall data
                    //optionCQGDataManagement.sendSubscribeRequest(sendOnlyUnsubscribed);

                    optionSpreadManager.callOptionRealTimeData(false);

                }

                sendSnapshotOfStrategyToDB();

                sendUpdateToLiveGrid();

                optionSpreadManager.gridViewContractSummaryManipulation.sendUpdateToContractSummaryLiveData(this);

                gridViewFCMPostionManipulation.sendUpdateToADMPositionsGrid(this);

                //sendUpdateToPortfolioTotalGrid();

                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                //{
                //    sendUpdateToPortfolioTotalSettlementGrid();
                //}

                sendUpdateToOrderGrid();

                //fillOrderSummaryList();

                //sendUpdateToRollStrikes();

                optionSpreadManager.modelADMCompareCalculationAndDisplay.fillGridModelADMComparison(this);

                updateStatusDataFilling();

                sendUpdateToExpressionListGrid();
            }


            this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        }

        public void cancelBackgroundWorker()
        {
            backgroundWorkerOptionRealtimeMonitor.CancelAsync();

            continueUpdating = false;

        }

        //         private String rowHeaderLabelCreate(OptionStrategy optionStrategy)
        //         {
        //             StringBuilder header = new StringBuilder();
        //             header.Append(optionStrategy.idStrategy);
        //             header.Append(" (");
        //             header.Append(optionStrategy.instrument.CQGsymbol);
        //             header.Append(") - ");
        // 
        //             header.Append(optionStrategy.instrument
        //                 .customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
        // 
        //             //header.Append(" ");
        //             //header.Append(optionBuilderSpreadStructure_local[
        //             //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
        //             //    .spreadInfo.strategyIdFromDB); //.optionSpreadConfigName);
        //             //header.Append(" - ");
        //             //header.Append(optionBuilderSpreadStructure_local[
        //             //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
        //             //    .instrument.customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
        // 
        //             return header.ToString().Replace('_', ' ');
        //         }

        //GridLiveData code
        public void setupGridLiveData()
        {
            //int[] liveLegRowIdexes = null;

            try
            {

                Type liveColTypes = typeof(OPTION_LIVE_DATA_COLUMNS);
                Array liveColTypesArray = Enum.GetNames(liveColTypes);


                gridLiveData.ColumnCount = liveColTypesArray.Length;

                gridLiveData.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridLiveData.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridLiveData.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                gridLiveData.Columns[0].Frozen = true;

                for (int i = 0; i < gridLiveData.ColumnCount; i++)
                {
                    gridLiveData.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < liveColTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(liveColTypesArray.GetValue(i).ToString());

                    gridLiveData
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    gridLiveData.Columns[i].Width = 50;
                }

                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.CONTRACT].Width = 115;

                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.LEG].Width = 30;
                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.LEG].DefaultCellStyle.Font = new Font("Tahoma", 7);

                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.TIME].Width = 70;
                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);
                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.EXPR].DefaultCellStyle.Font = new Font("Tahoma", 6);
                gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.EXPR].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                int rowIdx = 0;

                for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                {
                    int numberOfLegs = (int)optionStrategies[stratCounter]
                        .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                    optionStrategies[stratCounter].liveGridRowLoc = new int[numberOfLegs + 2];

                    for (int rowCount = 0; rowCount < numberOfLegs + 2; rowCount++)
                    {
                        optionStrategies[stratCounter].liveGridRowLoc[rowCount] = rowIdx;

                        rowIdx++;
                    }
                }

                gridLiveData.RowCount = rowIdx;

                //setup looks of live grid
                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.Black;

                rowIdx = 0;

                Color currentRowColor = rowColor1;

                for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                {
                    switch (stratCounter % 2)
                    {
                        case 0:
                            currentRowColor = rowColor1;
                            break;

                        case 1:
                            currentRowColor = rowColor2;
                            break;

                        //default:
                        //    currentRowColor = rowColor2;
                        //    break;
                    }

                    //optionStrategies[stratCounter].liveGridRowLoc = new int[
                    //    optionStrategies[stratCounter].liveGridRowLoc.Length];

                    for (int stratRowCount = 0; stratRowCount <
                        optionStrategies[stratCounter].liveGridRowLoc.Length; stratRowCount++)
                    {
                        //optionStrategies[stratCounter].liveGridRowLoc[stratRowCount] = rowIdx;

                        gridLiveData
                            .Rows[rowIdx]
                                .HeaderCell.Style.BackColor = currentRowColor;

                        if (stratRowCount == 0)
                        {
                            gridLiveData
                                .Rows[rowIdx]
                                .HeaderCell.Value =
                                optionSpreadManager.rowHeaderLabelCreate(optionStrategies[stratCounter]);

                            for (int j = 0; j < gridLiveData.ColumnCount; j++)
                            {
                                gridLiveData.Rows[rowIdx].Cells[j] = new CustomDataGridViewCell(true);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < gridLiveData.ColumnCount; j++)
                            {
                                gridLiveData.Rows[rowIdx].Cells[j] = new CustomDataGridViewCell(false);
                            }
                        }

                        rowIdx++;

                    }



                    DateTime currentDate = DateTime.Now;

                    int currentRow = 0;

                    for (int legCount = 0; legCount <
                        optionStrategies[stratCounter].optionStrategyParameters[
                            (int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue; legCount++)
                    {
                        currentRow = optionStrategies[stratCounter].liveGridRowLoc[legCount];

                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.LEG].Value = legCount + 1;

                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.SPREAD_QTY].Value =
                                optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition]
                                .stateValueParsed[legCount];

                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.CONTRACT].Value =
                                optionStrategies[stratCounter].legInfo[legCount].cqgSymbol;

                        //optionStrategies[stratCounter].legInfo[legCount].expirationDate

                        TimeSpan span = optionStrategies[stratCounter].legInfo[legCount].expirationDate.Date - currentDate.Date;

                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.CNTDN].Value =
                                span.Days;

                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.EXPR].Value =
                                new DateTime(
                                    optionStrategies[stratCounter].legInfo[legCount].expirationDate.Year,
                                    optionStrategies[stratCounter].legInfo[legCount].expirationDate.Month,
                                    optionStrategies[stratCounter].legInfo[legCount].expirationDate.Day,
                                    optionStrategies[stratCounter].legInfo[legCount].optionExpirationTime.Hour,
                                    optionStrategies[stratCounter].legInfo[legCount].optionExpirationTime.Minute,
                                    0
                                )
                                .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo);



                        gridLiveData
                                .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.STRIKE_PRICE].Value =
                                optionStrategies[stratCounter].legInfo[legCount].optionStrikePrice;

                    }



                    int totalLegs = (int)optionStrategies[stratCounter].optionStrategyParameters[
                                (int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                    currentRow = optionStrategies[stratCounter].liveGridRowLoc[totalLegs + (int)OPTION_LIVE_DATA_SUMMARY_ROWS.DELTA_HEDGING_ROW];

                    gridLiveData
                            .Rows[currentRow].
                                Cells[(int)OPTION_LIVE_DATA_COLUMNS.CONTRACT].Value
                                    = "HEDGE";

                    currentRow++;

                    gridLiveData
                               .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.MARGIN].Value =
                               optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.R_RISK].Value =
                            optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.R_RISK_R].Value =
                            optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk_R].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.ONE_R].Value =
                            optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.R_STATUS].Value =
                            optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.R_STATUS_R].Value =
                            optionStrategies[stratCounter].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus_R].stateValue;

                    gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.LOCKED_IN_R].Value =
                            optionStrategies[stratCounter].lockedIn_R;



                    for (int strategyRowCount = 0; strategyRowCount < optionStrategies[stratCounter].liveGridRowLoc.Length; strategyRowCount++)
                    {
                        currentRow = optionStrategies[stratCounter].liveGridRowLoc[strategyRowCount];

                        gridLiveData
                            .Rows[currentRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.SPREAD_ID].Value = stratCounter;
                    }


                }


            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            //return liveLegRowIdexes;
        }

        public void setupTreeViewInstruments()
        {
            for (int instrumentCnt = 0; instrumentCnt <= instruments.Length; instrumentCnt++)
            {
                //instrumentRollIntoSummary[i] = new InstrumentRollIntoSummary();

                if (instrumentCnt == instruments.Length)
                {


                    treeViewInstruments.Nodes.Add(instrumentCnt.ToString(), "ALL INSTRUMENTS");

                    treeViewInstruments.SelectedNode = treeViewInstruments.Nodes[instrumentCnt];

                    //treeViewInstruments.SelectedNode.BackColor = Color.Yellow;
                }
                else
                {
                    treeViewInstruments.Nodes.Add(instrumentCnt.ToString(), instruments[instrumentCnt].CQGsymbol);
                }
            }
        }

        public void setupTreeViewBrokerAcct()
        {
            optionSpreadManager.brokerAccountChosen = optionSpreadManager.portfolioGroupAllocation.Length;

            for (int groupAllocCnt = 0; groupAllocCnt <= optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
            {
                //instrumentRollIntoSummary[i] = new InstrumentRollIntoSummary();

                if (groupAllocCnt == optionSpreadManager.portfolioGroupAllocation.Length)
                {


                    treeViewBrokerAcct.Nodes.Add(groupAllocCnt.ToString(), "ALL ACCTS");

                    treeViewBrokerAcct.SelectedNode = treeViewBrokerAcct.Nodes[groupAllocCnt];

                    //treeViewInstruments.SelectedNode.BackColor = Color.Yellow;
                }
                else
                {
                    StringBuilder treeVal = new StringBuilder();

                    if (optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].useConfigFile)
                    {
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                        treeVal.Append(" | CFG:");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].cfgfile);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].FCM_POFFIC_PACCT);
                    }
                    else
                    {
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].account);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].FCM_POFFIC_PACCT);
                    }

                    treeViewBrokerAcct.Nodes.Add(groupAllocCnt.ToString(), treeVal.ToString());
                }
            }
        }




        public void setupExpressionListGridView()
        {
            try
            {

                Type liveColTypes = typeof(EXPRESSION_LIST_VIEW);
                Array liveColTypesArray = Enum.GetNames(liveColTypes);

                dataGridViewExpressionList.ColumnCount = liveColTypesArray.Length;

                dataGridViewExpressionList.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = dataGridViewExpressionList.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = dataGridViewExpressionList.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;

                dataGridViewExpressionList.Columns[0].Frozen = true;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < dataGridViewExpressionList.ColumnCount; i++)
                {
                    dataGridViewExpressionList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    if (i == (int)EXPRESSION_LIST_VIEW.MANUAL_STTLE)
                    {
                        DataGridViewCheckBoxColumn manualSettleCol = new DataGridViewCheckBoxColumn();
                        {
                            manualSettleCol.HeaderText = liveColTypesArray.GetValue(
                                (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE).ToString().Replace('_', ' ');
                            //column.Name = ColumnName.OutOfOffice.ToString();
                            manualSettleCol.AutoSizeMode =
                                DataGridViewAutoSizeColumnMode.DisplayedCells;
                            manualSettleCol.FlatStyle = FlatStyle.Standard;
                            //column.ThreeState = true;
                            manualSettleCol.CellTemplate = new DataGridViewCheckBoxCell();
                            manualSettleCol.CellTemplate.Style.BackColor = Color.LightBlue;
                            manualSettleCol.ReadOnly = false;
                            //column.e = false;
                        }

                        dataGridViewExpressionList.Columns.RemoveAt(i);
                        dataGridViewExpressionList.Columns.Insert(i, manualSettleCol);

                        //dataGridViewExpressionList.Columns.Insert(i, myChkBox);

                        //dataGridViewExpressionList.Columns[i].ReadOnly = false;
                    }
                    else
                    {
                        dataGridViewExpressionList.Columns[i].ReadOnly = true;
                    }



                    sb.Clear();

                    sb.Append(liveColTypesArray.GetValue(i).ToString());

                    dataGridViewExpressionList
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    dataGridViewExpressionList.Columns[i].Width = 50;
                }



                //for (int i = 0; i < liveColTypesArray.Length; i++)
                //{

                //}

                //************
                //dataGridViewExpressionList.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CONTRACT].Width = 115;

                //dataGridViewExpressionList.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LEG].Width = 30;
                //dataGridViewExpressionList.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LEG].DefaultCellStyle.Font = new Font("Tahoma", 7);

                dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.TIME].Width = 70;
                dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);
                dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


                dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.SETL_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);
                dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.SETL_TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.EXPR].DefaultCellStyle.Font = new Font("Tahoma", 6);
                //dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.EXPR].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //************

                //List<LiveADMStrategyInfo> liveADMStrategyInfoList = optionSpreadManager.liveADMStrategyInfoList;

                //updateSetupExpressionListGridView();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void updateSetupExpressionListGridView()
        {
            try
            {

                //dataGridViewExpressionList.RowCount = optionSpreadExpressionList.Count - 1;
                updateExpressionGridRowCount();

                int rowIdx = 0;


                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.DarkBlue;

                Color currentRowColor = rowColor1;

                int optionSpreadExpressionIdx = 0;

                for (int instrumentCnt = 0; instrumentCnt <= instruments.Length; instrumentCnt++)
                {
                    optionSpreadExpressionIdx = 0;

                    foreach (OptionSpreadExpression ose in optionSpreadExpressionList)
                    {
                        if (ose.optionExpressionType != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                            && ose.instrument.idxOfInstrumentInList == instrumentCnt)
                        {
                            ose.dataGridExpressionListRow = rowIdx;

                            switch (rowIdx % 2)
                            {
                                case 0:
                                    currentRowColor = rowColor1;
                                    break;

                                case 1:
                                    currentRowColor = rowColor2;
                                    break;

                            }

                            //FIX THIS THREAD SAFE
                            //dataGridViewExpressionList
                            //        .Rows[rowIdx]
                            //            .HeaderCell.Style.BackColor = currentRowColor;

                            //dataGridViewExpressionList
                            //        .Rows[rowIdx]
                            //        .HeaderCell.Value =
                            //            ose.cqgSymbol;

                            fillExpressionListGridHeaders(rowIdx, -1, ose.cqgSymbol, currentRowColor);

                            fillDataGridViewExpressionListPage(rowIdx,
                                    (int)EXPRESSION_LIST_VIEW.INSTRUMENT_ID,
                                   instrumentCnt.ToString(), true, instrumentCnt);

                            fillDataGridViewExpressionListPage(rowIdx,
                                    (int)EXPRESSION_LIST_VIEW.EXPRESSION_IDX,
                                   optionSpreadExpressionIdx.ToString(), true, optionSpreadExpressionIdx);

                            //dataGridViewExpressionList.Rows[rowIdx].Cells[(int)EXPRESSION_LIST_VIEW.INSTRUMENT_ID].Value = instrumentCnt;


                            rowIdx++;

                        }

                        optionSpreadExpressionIdx++;
                    }
                }

                //updateColorOfADMStrategyGrid();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            //return liveLegRowIdexes;
        }

        private void updateExpressionGridRowCount()
        {
            if (this.InvokeRequired)
            {
                ThreadSafeGenericDelegateWithoutParams d =
                    new ThreadSafeGenericDelegateWithoutParams(threadSafeUpdateExpressionGridRowCount);

                this.Invoke(d);
            }
            else
            {
                threadSafeUpdateExpressionGridRowCount();
            }
        }

        private void threadSafeUpdateExpressionGridRowCount()
        {
            dataGridViewExpressionList.RowCount = optionSpreadExpressionList.Count - 1;
        }

        public void fillExpressionListGridHeaders(int rowToUpdate, int col, string displayValue, Color color)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    //ThreadSafeFillGridViewValueAsStringAndColorDelegate(int rowToUpdate, int col, string displayValue, Color color);

                    ThreadSafeFillGridViewValueAsStringAndColorDelegate d =
                        new ThreadSafeFillGridViewValueAsStringAndColorDelegate(threadSafeFillExpressionListGridHeaders);

                    this.Invoke(d, rowToUpdate, col, displayValue, color);
                }
                else
                {
                    threadSafeFillExpressionListGridHeaders(rowToUpdate, col, displayValue, color);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillExpressionListGridHeaders(int rowToUpdate, int col, string displayValue, Color color)
        {
            try
            {
                //int rowToUpdate = row;

                //if (
                //    (
                //    dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Value == null
                //    ||
                //    dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                //    ))
                {
                    dataGridViewExpressionList
                                    .Rows[rowToUpdate]
                                        .HeaderCell.Style.BackColor = color;

                    dataGridViewExpressionList
                            .Rows[rowToUpdate]
                            .HeaderCell.Value = displayValue;
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }





        public void setupInstrumentSummary()
        {
#if DEBUG
            try
#endif
            {

                Type instrumentSummaryGridRowTypes = typeof(INSTRUMENT_SUMMARY_GRID_ROWS);
                Array instrumentSummaryGridRowTypesArray = Enum.GetNames(instrumentSummaryGridRowTypes);

                //ADD EXTRA COLUMN FOR TOTAL
                instrumentSummaryGrid.ColumnCount = instruments.Length + 1;
                instrumentSummaryGrid.RowCount = instrumentSummaryGridRowTypesArray.Length;



                for (int i = 0; i < instrumentSummaryGrid.ColumnCount; i++)
                {
                    instrumentSummaryGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    instrumentSummaryGrid.Columns[i].Width = 70;
                }

                instrumentSummaryGrid.Columns[instruments.Length].HeaderText = "TOTAL";


                for (int i = 0; i < instrumentSummaryGridRowTypesArray.Length; i++)
                {
                    instrumentSummaryGrid.Rows[i].HeaderCell.Value = instrumentSummaryGridRowTypesArray.GetValue(i).ToString();
                }

                double totalMarginSum = 0;
                double totalRRiskSum = 0;

                double marginSum = 0;
                double rRiskSum = 0;
                double oneR = 0;
                double rStatus = 0;
                double lockedInR = 0;

                double[] instrumentRRisk = new double[instruments.Length];

                for (int i = 0; i <= instruments.Length; i++)
                {
                    if (i < instruments.Length)
                    {
                        instrumentSummaryGrid.Columns[i].HeaderText = instruments[i].CQGsymbol
                            + " - " + instruments[i].exchangeSymbol;

                        marginSum = 0;
                        rRiskSum = 0;
                        oneR = 0;
                        rStatus = 0;
                        lockedInR = 0;

                        for (int stratCnt = 0; stratCnt < optionStrategies.Length; stratCnt++)
                        {
                            if (optionStrategies[stratCnt].indexOfInstrumentInInstrumentsArray == i)
                            {
                                double[] position =
                                    optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition]
                                    .stateValueParsed;

                                int posCnt = 0;
                                bool hasPosition = false;
                                while (posCnt < position.Length)
                                {
                                    if (position[posCnt] != 0)
                                    {
                                        hasPosition = true;
                                        break;
                                    }

                                    posCnt++;
                                }

                                if (hasPosition)
                                {
                                    marginSum +=
                                        optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].stateValue;

                                    if (optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue
                                        > 0)
                                    {
                                        rRiskSum +=
                                            optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                                    }

                                    oneR +=
                                        optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].stateValue;

                                    rStatus +=
                                        optionStrategies[stratCnt].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue;

                                    lockedInR +=
                                        optionStrategies[stratCnt].lockedIn_R;

                                }
                            }
                        }

                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.MARGIN_SUMMARY].Cells[i].Value = marginSum;
                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.R_RISK_SUMMARY].Cells[i].Value = rRiskSum;

                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.ONE_R].Cells[i].Value = marginSum;
                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.R_STATUS].Cells[i].Value = rRiskSum;

                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.LOCKED_IN_R].Cells[i].Value = lockedInR;

                        //instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN].Cells[i].Value = 
                        //    instruments[i].coreAPIinitialMargin;

                        //instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN].Cells[i].Value =
                        //    instruments[i].coreAPImaintenanceMargin;

                        totalMarginSum += marginSum;

                        totalRRiskSum += rRiskSum;

                        instrumentRRisk[i] = rRiskSum;
                    }
                    else
                    {
                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.MARGIN_SUMMARY].Cells[i].Value = totalMarginSum;
                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.R_RISK_SUMMARY].Cells[i].Value = totalRRiskSum;
                    }

                }

                if (totalRRiskSum != 0)
                {
                    for (int i = 0; i < instruments.Length; i++)
                    {
                        instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.R_RISK_PERCENT].Cells[i].Value =
                            Math.Round(instrumentRRisk[i] /
                            totalRRiskSum * 100, 2);

                    }
                }



            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void setupInstrumentSummaryStrategyRRisk()
        {
            //int[] liveLegRowIdexes = null;

#if DEBUG
            try
#endif
            {

                //contractSummaryInstrumentSelectedIdx = instruments.Length;

                Type instrumentRriskSummaryColTypes = typeof(INSTRUMENT_SUMMARY_STRATEGY_RRISK);
                Array instrumentRriskSummaryColTypesArray = Enum.GetNames(instrumentRriskSummaryColTypes);


                dataGridStrategyRRiskSummary.ColumnCount = instrumentRriskSummaryColTypesArray.Length;

                dataGridStrategyRRiskSummary.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = dataGridStrategyRRiskSummary.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = dataGridStrategyRRiskSummary.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                dataGridStrategyRRiskSummary.Columns[0].Frozen = true;

                for (int i = 0; i < dataGridStrategyRRiskSummary.ColumnCount; i++)
                {
                    dataGridStrategyRRiskSummary.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < instrumentRriskSummaryColTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(instrumentRriskSummaryColTypesArray.GetValue(i).ToString());

                    dataGridStrategyRRiskSummary
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    dataGridStrategyRRiskSummary.Columns[i].Width = 50;
                }


                dataGridStrategyRRiskSummary.RowCount = optionStrategies.Length;

                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.Black;

                Color currentRowColor = rowColor1;

                //int heldLotsExpressionCnt = 0;

                for (int instrumentCnt = 0; instrumentCnt <= instruments.Length; instrumentCnt++)
                {
                    //instrumentRollIntoSummary[i] = new InstrumentRollIntoSummary();

                    //if (instrumentCnt == instruments.Length)
                    //{
                    //    treeViewInstrumentsSummary.Nodes.Add(instrumentCnt.ToString(), "ALL INSTRUMENTS");

                    //    //treeViewInstrumentsContractSummary.Nodes[instrumentCnt].;
                    //}
                    //else
                    //{
                    //    treeViewInstrumentsSummary.Nodes.Add(instrumentCnt.ToString(), instruments[instrumentCnt].CQGsymbol);
                    //}

                    for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                    {
                        switch (stratCounter % 2)
                        {
                            case 0:
                                currentRowColor = rowColor1;
                                break;

                            case 1:
                                currentRowColor = rowColor2;
                                break;
                        }

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter]
                                        .HeaderCell.Style.BackColor = currentRowColor;

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter]
                                        .HeaderCell.Value =
                                            optionStrategies[stratCounter].idStrategy;
                        //+
                        //" " +
                        //optionStrategies[stratCounter].strategyName;

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.MARGIN].Value =
                                            optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.marginRequirement].stateValue;

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.R_RISK].Value =
                                            Math.Round(optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue);

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.R_RISK_R].Value =
                                            optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk_R].stateValue;

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.ONE_R].Value =
                                            Math.Round(optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].stateValue);

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.R_STATUS].Value =
                                            Math.Round(optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue);

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.R_STATUS_R].Value =
                                            optionStrategies[stratCounter]
                                                .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus_R].stateValue;

                        dataGridStrategyRRiskSummary
                                    .Rows[stratCounter].Cells[(int)INSTRUMENT_SUMMARY_STRATEGY_RRISK.LOCKED_IN_R].Value =
                                            Math.Round(optionStrategies[stratCounter].lockedIn_R);
                    }

                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void setupPortfolioSummary()
        {
#if DEBUG
            try
#endif
            {

                Type portfolioSummaryGridRowTypes = typeof(PORTFOLIO_SUMMARY_GRID_ROWS);
                Array portfolioSummaryGridRowTypesArray = Enum.GetNames(portfolioSummaryGridRowTypes);

                for (int i = 0; i < instruments.Length; i++)
                {
                    portfolioSummaryDataTable.Columns.Add(instruments[i].CQGsymbol
                        + " - " + instruments[i].exchangeSymbol);
                }

                //ADD EXTRA COLUMN FOR TOTAL
                portfolioSummaryDataTable.Columns.Add("TOTAL");

                for (int i = 0; i < portfolioSummaryGridRowTypesArray.Length; i++)
                {
                    portfolioSummaryDataTable.Rows.Add();
                }

                //this must be at the end after all the columns and rows are added
                portfolioSummaryGrid.DataSource = portfolioSummaryDataTable;

                //Dispatcher.BeginInvoke( new Action( () => { portfolioSummaryGrid.DataSource = portfolioSummaryDataTable; }))

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }


        private void portfolioSummaryGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //http://stackoverflow.com/questions/13231149/datagridview-not-updating-refreshing

            for (int i = 0; i < portfolioSummaryGrid.ColumnCount; i++)
            {
                portfolioSummaryGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                portfolioSummaryGrid.Columns[i].Width = 70;
            }

            Type portfolioSummaryGridRowTypes = typeof(PORTFOLIO_SUMMARY_GRID_ROWS);
            Array portfolioSummaryGridRowTypesArray = Enum.GetNames(portfolioSummaryGridRowTypes);

            for (int i = 0; i < portfolioSummaryGridRowTypesArray.Length; i++)
            {
                portfolioSummaryGrid.Rows[i].HeaderCell.Value = portfolioSummaryGridRowTypesArray.GetValue(i).ToString();
            }
        }



        public void setupPortfolioSummarySettlements()
        {
#if DEBUG
            try
#endif
            {
                Type portfolioSummaryGridRowTypes = typeof(PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS);
                Array portfolioSummaryGridRowTypesArray = Enum.GetNames(portfolioSummaryGridRowTypes);

                for (int i = 0; i < instruments.Length; i++)
                {
                    portfolioSummarySettlementDataTable.Columns.Add(instruments[i].CQGsymbol
                        + " - " + instruments[i].exchangeSymbol);

                    //portfolioSummaryGridSettlements.Columns[i].HeaderText = instruments[i].CQGsymbol
                    //    + " - " + instruments[i].exchangeSymbol;

                }

                portfolioSummarySettlementDataTable.Columns.Add("TOTAL");

                


                for (int i = 0; i < portfolioSummaryGridRowTypesArray.Length; i++)
                {
                    portfolioSummarySettlementDataTable.Rows.Add();
                }

                //this must be at the end after all the columns and rows are added
                portfolioSummaryGridSettlements.DataSource = portfolioSummarySettlementDataTable;

                //for (int i = 0; i < portfolioSummaryGridSettlements.ColumnCount; i++)
                //{
                //    portfolioSummaryGridSettlements.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                //    portfolioSummaryGridSettlements.Columns[i].Width = 70;
                //}

                //portfolioSummaryGridSettlements.Columns[instruments.Length].HeaderText = "TOTAL";


                //for (int i = 0; i < portfolioSummaryGridRowTypesArray.Length; i++)
                //{
                //    portfolioSummaryGridSettlements.Rows[i].HeaderCell.Value = portfolioSummaryGridRowTypesArray.GetValue(i).ToString();
                //}

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void portfolioSummaryGridSettlements_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < portfolioSummaryGridSettlements.ColumnCount; i++)
            {
                portfolioSummaryGridSettlements.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                portfolioSummaryGridSettlements.Columns[i].Width = 70;
            }

            Type portfolioSummaryGridRowTypes = typeof(PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS);
            Array portfolioSummaryGridRowTypesArray = Enum.GetNames(portfolioSummaryGridRowTypes);

            for (int i = 0; i < portfolioSummaryGridRowTypesArray.Length; i++)
            {
                portfolioSummaryGridSettlements.Rows[i].HeaderCell.Value = portfolioSummaryGridRowTypesArray.GetValue(i).ToString().Replace('_', ' ');
                //portfolioSummaryGridSettlements..HeaderCell..Width = 100;
            }
        }

        public void setupPreviousPLAnalysis(DataGridView dataGridViewPriceCompare, DataGridView dataGridViewPL,
             bool model)
        {
#if DEBUG
            try
#endif
            {
                Type previousPriceGridRowTypes = typeof(PREVIOUS_PRICE_COMPARE_ANALYSIS);
                Array previousPriceGridRowTypesArray = Enum.GetNames(previousPriceGridRowTypes);

                dataGridViewPriceCompare.ColumnCount = previousPriceGridRowTypesArray.Length;

                dataGridViewPriceCompare.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = dataGridViewPriceCompare.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = dataGridViewPriceCompare.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                dataGridViewPriceCompare.Columns[0].Frozen = true;

                for (int i = 0; i < dataGridViewPriceCompare.ColumnCount; i++)
                {
                    dataGridViewPriceCompare.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < previousPriceGridRowTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(previousPriceGridRowTypesArray.GetValue(i).ToString());

                    dataGridViewPriceCompare
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    dataGridViewPriceCompare.Columns[i].Width = 60;
                }

                if (model)
                {
                    dataGridViewPriceCompare.TopLeftHeaderCell.Value = "MODEL";
                }
                else
                {
                    dataGridViewPriceCompare.TopLeftHeaderCell.Value = "FCM";
                }

                dataGridViewPriceCompare.RowCount = 0;






                Type previousPLGridRowTypes = typeof(PREVIOUS_PL_COMPARE_ANALYSIS);
                Array previousPLGridRowTypesArray = Enum.GetNames(previousPLGridRowTypes);

                dataGridViewPL.ColumnCount = previousPLGridRowTypesArray.Length;

                dataGridViewPL.EnableHeadersVisualStyles = false;

                colTotalPortStyle = dataGridViewPL.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                rowTotalPortStyle = dataGridViewPL.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                dataGridViewPL.Columns[0].Frozen = true;

                for (int i = 0; i < dataGridViewPL.ColumnCount; i++)
                {
                    dataGridViewPL.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                //StringBuilder sb = new StringBuilder();

                for (int i = 0; i < previousPLGridRowTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(previousPLGridRowTypesArray.GetValue(i).ToString());

                    dataGridViewPL
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    dataGridViewPL.Columns[i].Width = 80;
                }

                if (model)
                {
                    dataGridViewPL.TopLeftHeaderCell.Value = "MODEL";
                }
                else
                {
                    dataGridViewPL.TopLeftHeaderCell.Value = "FCM";
                }

                dataGridViewPL.RowCount = instruments.Length + 1;

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }


        public void setupGridOrders()
        {
#if DEBUG
            try
#endif
            {

                Type liveColTypes = typeof(OPTION_ORDERS_COLUMNS);
                Array liveColTypesArray = Enum.GetNames(liveColTypes);


                gridOrders.ColumnCount = liveColTypesArray.Length;

                gridOrders.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridOrders.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridOrders.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                //gridLiveData.Columns[0].Frozen = true;

                for (int i = 0; i < gridOrders.ColumnCount; i++)
                {
                    gridOrders.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < liveColTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(liveColTypesArray.GetValue(i).ToString());

                    gridOrders
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    //gridOrders.Columns[i].Width = 50;
                    gridOrders.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.POTENTIAL_ACTION].Width = 70;

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.POTENTIAL_ACTION].DefaultCellStyle.Font = new Font("Tahoma", 6);

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.SIGNAL_TYPE].Width = 20;

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.ACTION_RULE].Width = 270;
                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE].Width = 120;
                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION].Width = 70;

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.CONTRACT].Width = 125;
                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.QTY].Width = 30;

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANS_PRICE].Width = 50;

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANS_TIME].Width = 70;
                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANS_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.SPREAD_ID].Width = 0;

                int rowIdx = 0;

                int rowsForStrategy = 0;

                for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                {
                    int numberOfLegs = (int)optionStrategies[stratCounter]
                        .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                    switch (optionStrategies[stratCounter].actionType)
                    {
                        case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.NO_ACTION:
                            rowsForStrategy = 1;
                            break;

                        case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY:
                            rowsForStrategy = numberOfLegs;
                            break;

                        case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY_WITH_ROLL:
                            rowsForStrategy = numberOfLegs;
                            break;

                        case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT:
                            rowsForStrategy = numberOfLegs;
                            break;

                        case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT_OR_ROLL_OVER:
                            rowsForStrategy = numberOfLegs * 2;
                            break;

                    }

                    optionStrategies[stratCounter].orderGridRowLoc = new int[rowsForStrategy];

                    for (int rowCount = 0; rowCount < rowsForStrategy; rowCount++)
                    {
                        optionStrategies[stratCounter].orderGridRowLoc[rowCount] = rowIdx;

                        rowIdx++;
                    }
                }

                gridOrders.RowCount = rowIdx;

                //setup looks of live grid
                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.Black;

                //rowIdx = 0;

                Color currentRowColor = rowColor1;

                for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                {
                    switch (stratCounter % 2)
                    {
                        case 0:
                            currentRowColor = rowColor1;
                            break;

                        case 1:
                            currentRowColor = rowColor2;
                            break;

                        //default:
                        //    currentRowColor = rowColor2;
                        //    break;
                    }



                    for (int strategyRowCount = 0; strategyRowCount < optionStrategies[stratCounter].orderGridRowLoc.Length; strategyRowCount++)
                    {
                        int currentRow = optionStrategies[stratCounter].orderGridRowLoc[strategyRowCount];

                        gridOrders
                            .Rows[currentRow]
                                .HeaderCell.Style.BackColor = currentRowColor;

                        if (strategyRowCount == 0)
                        {
                            gridOrders
                                .Rows[currentRow]
                                .HeaderCell.Value =
                                optionSpreadManager.rowHeaderLabelCreate(optionStrategies[stratCounter]);

                            for (int j = 0; j < gridOrders.ColumnCount; j++)
                            {
                                gridOrders.Rows[currentRow].Cells[j] = new CustomDataGridViewCell(true);
                            }

                            gridOrders
                                .Rows[currentRow].Cells[(int)OPTION_ORDERS_COLUMNS.POTENTIAL_ACTION].Value =
                                Enum.GetName(typeof(SPREAD_POTENTIAL_OPTION_ACTION_TYPES), optionStrategies[stratCounter].actionType).Replace('_', ' ');

                            gridOrders
                                .Rows[currentRow].Cells[(int)OPTION_ORDERS_COLUMNS.SIGNAL_TYPE].Value =
                                    optionStrategies[stratCounter].idRiskType;
                                
                        }
                        else
                        {
                            for (int j = 0; j < gridOrders.ColumnCount; j++)
                            {
                                gridOrders.Rows[currentRow].Cells[j] = new CustomDataGridViewCell(false);
                            }
                        }




                        gridOrders
                            .Rows[currentRow].Cells[(int)OPTION_ORDERS_COLUMNS.SPREAD_ID].Value = stratCounter;

                    }

                }


            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            //return liveLegRowIdexes;
        }

        public void setupOrderSummaryList()
        {
            orderSummaryGrid.DataSource = optionSpreadManager.orderSummaryDataTable;

            rollIntoOrderSummaryGrid.DataSource = optionSpreadManager.rollIntoOrderSummaryDataTable;

            orderSummaryGridInactive.DataSource = optionSpreadManager.orderSummaryNotActiveDataTable;

            rollIntoOrderSummaryGridInactive.DataSource = optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable;

            //orderSummaryInstrumentSelectedIdx = instruments.Length;

            optionSpreadManager.instrumentRollIntoSummary = new InstrumentTransactionSummary[instruments.Length + 1];

            for (int i = 0; i <= instruments.Length; i++)
            {
                optionSpreadManager.instrumentRollIntoSummary[i] = new InstrumentTransactionSummary();

                //if (i == instruments.Length)
                //{
                //    treeViewInstrumentsXXX.Nodes.Add(i.ToString(), "ALL INSTRUMENTS");
                //}
                //else
                //{
                //    treeViewInstrumentsXXX.Nodes.Add(i.ToString(), instruments[i].CQGsymbol);
                //}
            }


            optionSpreadManager.orderSummaryDataTable.Columns.Add("Inst");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Contract");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("#");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Decs T");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Tran T");            
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Decs P");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Tran P");
            optionSpreadManager.orderSummaryDataTable.Columns.Add("Decs Filled");

            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Inst");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Contract");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("#");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Decs T");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Tran T");            
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Decs P");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Tran P");
            optionSpreadManager.orderSummaryNotActiveDataTable.Columns.Add("Decs Filled");



            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Inst");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Contract");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("#");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Decs T");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Tran T");            
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Decs P");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Tran P");
            optionSpreadManager.rollIntoOrderSummaryDataTable.Columns.Add("Decs Filled");

            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Inst");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Contract");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("#");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Decs T");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Tran T");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Decs P");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Tran P");
            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Columns.Add("Decs Filled");
        }


        public void fillOrderSummaryList()
        {
            //orderListBeginEndUpdate(true);

            //TreeNode x = treeViewInstrumentsXXX.SelectedNode;

            //if (x == null)
            //{
            //    if (orderSummaryHashTable.Count > 0
            //        || orderSummaryNotActiveHashTable.Count > 0
            //        || orderRollIntoSummaryHashTable.Count > 0
            //        || orderRollIntoSummaryHashTableDisplayed.Count > 0
            //        || orderRollIntoSummaryHashTableNotActive.Count > 0
            //        || orderRollIntoSummaryHashTableDisplayedNotActive.Count > 0)
            //    {
            //        orderSummaryList.Items.Clear();
            //        orderSummaryListNotActive.Items.Clear();

            //        rollIntoOrderSummaryList.Items.Clear();
            //        rollIntoOrderSummaryListNotActive.Items.Clear();


            //        orderSummaryHashTable.Clear();
            //        orderSummaryNotActiveHashTable.Clear();


            //        orderRollIntoSummaryHashTable.Clear();
            //        orderRollIntoSummaryHashTableDisplayed.Clear();

            //        orderRollIntoSummaryHashTableNotActive.Clear();
            //        orderRollIntoSummaryHashTableDisplayedNotActive.Clear();
            //    }
            //}
            //else
            {
                //int idx = x.Index;

                for (int rollSummaryHashCount = 0; rollSummaryHashCount < optionSpreadManager.orderRollIntoSummaryHashTableDisplayed.Count; rollSummaryHashCount++)
                {
                    optionSpreadManager.orderRollIntoSummaryHashTableDisplayed[rollSummaryHashCount] = false;
                }

                //for (int rollSummaryHashCount = 0; rollSummaryHashCount < orderRollIntoSummaryHashTableDisplayedNotActive.Count; rollSummaryHashCount++)
                //{
                //    orderRollIntoSummaryHashTableDisplayedNotActive[rollSummaryHashCount] = false;
                //}

                List<string> symbolsToRemove = new List<string>();

                for (int i = 0; i < instruments.Length; i++)
                {
                    symbolsToRemove.Clear();

                    foreach (ContractList rollIntoContracts in optionSpreadManager.instrumentRollIntoSummary[i].contractHashTable.Values)
                    {
                        if (!rollIntoContracts.currentlyRollingContract)
                        {
                            symbolsToRemove.Add(rollIntoContracts.cqgSymbol);
                        }

                        if (rollIntoContracts.currentlyRollingContract)
                        {
                            rollIntoContracts.numberOfContracts = rollIntoContracts.tempNumberOfContracts;

                            rollIntoContracts.numberOfContractsNotActive = rollIntoContracts.tempNumberOfContractsNotActive;

                            if (i == optionSpreadManager.contractSummaryInstrumentSelectedIdx
                                || optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
                            {
                                //if (rollIntoContracts.currentlyRollingContract)
                                {

                                    if (rollIntoContracts.numberOfContracts != 0)
                                    {
                                        enterRollIntoOrderSummary(rollIntoContracts,
                                            optionSpreadManager.orderRollIntoSummaryHashTable, optionSpreadManager.orderRollIntoSummaryHashTableDisplayed,
                                            optionSpreadManager.rollIntoOrderSummaryDataTable,
                                            instruments[i], true, rollIntoContracts.numberOfContracts);
                                    }


                                    if (rollIntoContracts.numberOfContractsNotActive != 0)
                                    {
                                        enterRollIntoOrderSummary(rollIntoContracts,
                                            optionSpreadManager.orderRollIntoSummaryHashTableNotActive, optionSpreadManager.orderRollIntoSummaryHashTableDisplayedNotActive,
                                            optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable, instruments[i], false, rollIntoContracts.numberOfContractsNotActive);
                                    }

                                }

                            }

                        }
                    }


                    for (int symbolsToRemoveCnt = 0; symbolsToRemoveCnt < symbolsToRemove.Count; symbolsToRemoveCnt++)
                    {

                        //int removeAttempts = 0;

                        ContractList rollIntoContracts;

                        //while (!instrumentRollIntoSummary[i].contractHashTable
                        //    .TryRemove(symbolsToRemove[symbolsToRemoveCnt], out rollIntoContracts) && removeAttempts < 10)
                        //{
                        //    removeAttempts++;
                        //}

                        optionSpreadManager.instrumentRollIntoSummary[i].contractHashTable
                            .TryRemove(symbolsToRemove[symbolsToRemoveCnt], out rollIntoContracts);

                    }
                }

                removeRolls(optionSpreadManager.orderRollIntoSummaryHashTable, optionSpreadManager.orderRollIntoSummaryHashTableDisplayed);

                //removeRolls(orderRollIntoSummaryHashTableNotActive, orderRollIntoSummaryHashTableDisplayedNotActive,
                //    rollIntoOrderSummaryListNotActive);



                for (int i = 0; i < optionSpreadExpressionList.Count; i++)
                {
                    if (optionSpreadExpressionList[i].optionExpressionType
                        != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                    {
                        optionSpreadExpressionList[i].numberOfOrderContracts =
                           optionSpreadExpressionList[i].numberOfOrderContractsTempForCalc;

                        optionSpreadExpressionList[i].numberOfOrderContractsNotActive =
                           optionSpreadExpressionList[i].numberOfOrderContractsTempForCalcNotActive;

                        //this code removes the unselected instrument contracts from the following list
                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length
                            && optionSpreadExpressionList[i].instrument.idxOfInstrumentInList != optionSpreadManager.contractSummaryInstrumentSelectedIdx)
                        {
                            if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[i].cqgSymbol))
                            {

                                int rowIdx = optionSpreadManager.orderSummaryHashTable.FindIndex(new System.Predicate<string>(
                                    (value) => { return value == optionSpreadExpressionList[i].cqgSymbol; }));

                                optionSpreadManager.orderSummaryHashTable.RemoveAt(rowIdx);

                                optionSpreadManager.orderSummaryDataTable.Rows.RemoveAt(rowIdx);
                            }
                            else if (optionSpreadManager.orderSummaryNotActiveHashTable.Contains(optionSpreadExpressionList[i].cqgSymbol))
                            {

                                int rowIdx = optionSpreadManager.orderSummaryNotActiveHashTable.FindIndex(new System.Predicate<string>(
                                    (value) => { return value == optionSpreadExpressionList[i].cqgSymbol; }));

                                optionSpreadManager.orderSummaryNotActiveHashTable.RemoveAt(rowIdx);

                                optionSpreadManager.orderSummaryNotActiveDataTable.Rows.RemoveAt(rowIdx);
                            }
                        }



                        if (optionSpreadExpressionList[i].instrument.idxOfInstrumentInList == optionSpreadManager.contractSummaryInstrumentSelectedIdx
                                || optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
                        {
                            if (optionSpreadExpressionList[i].numberOfOrderContracts != 0)
                            {
                                enterExpressionsIntoOrderSummary(optionSpreadManager.orderSummaryHashTable,
                                    optionSpreadExpressionList[i], optionSpreadManager.orderSummaryDataTable,
                                    true, optionSpreadExpressionList[i].numberOfOrderContracts);


                            }
                            //number of contracts are == 0
                            else if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[i].cqgSymbol))
                            {

                                //remove contracts that no longer have order contracts

                                int rowIdx = optionSpreadManager.orderSummaryHashTable.FindIndex(new System.Predicate<string>(
                                    (value) => { return value == optionSpreadExpressionList[i].cqgSymbol; }));

                                if (rowIdx >= 0)
                                {
                                    optionSpreadManager.orderSummaryHashTable.RemoveAt(rowIdx);

                                    optionSpreadManager.orderSummaryDataTable.Rows.RemoveAt(rowIdx);
                                }
                            }


                            if (optionSpreadExpressionList[i].numberOfOrderContractsNotActive != 0)
                            {
                                enterExpressionsIntoOrderSummary(optionSpreadManager.orderSummaryNotActiveHashTable,
                                    optionSpreadExpressionList[i], optionSpreadManager.orderSummaryNotActiveDataTable,
                                    false, optionSpreadExpressionList[i].numberOfOrderContractsNotActive);
                            }
                            else if (optionSpreadManager.orderSummaryNotActiveHashTable.Contains(optionSpreadExpressionList[i].cqgSymbol))
                            {

                                //remove contracts that no longer have OrderContractsNotActive contracts

                                int rowIdx = optionSpreadManager.orderSummaryNotActiveHashTable.FindIndex(new System.Predicate<string>(
                                    (value) => { return value == optionSpreadExpressionList[i].cqgSymbol; }));

                                if (rowIdx >= 0)
                                {
                                    optionSpreadManager.orderSummaryNotActiveHashTable.RemoveAt(rowIdx);

                                    optionSpreadManager.orderSummaryNotActiveDataTable.Rows.RemoveAt(rowIdx);
                                }
                            }



                        }

                    }
                }
            }

            //orderSummaryList.EndUpdate();

            //orderListBeginEndUpdate(false);

        }

        private void removeRolls(List<string> rollHashTable, List<bool> rollHashDisplayed)
        {
            for (int rollSummaryHashCount = rollHashDisplayed.Count - 1;
                    rollSummaryHashCount >= 0; rollSummaryHashCount--)
            {
                if (!rollHashDisplayed[rollSummaryHashCount])
                {
                    rollHashDisplayed.RemoveAt(rollSummaryHashCount);

                    rollHashTable.RemoveAt(rollSummaryHashCount);

                    //removeItemFromListView(rollIntoList, rollSummaryHashCount);

                    optionSpreadManager.rollIntoOrderSummaryDataTable.Rows.RemoveAt(rollSummaryHashCount);

                    //rollIntoList.Items[rollSummaryHashCount].Remove();
                }
            }
        }

        private void enterRollIntoOrderSummary(ContractList rollIntoContracts,
            List<string> rollHashTable, List<bool> rollHashDisplayed, DataTable dataTable,
            Instrument instrument, bool active, int numberOfContracts)
        {
            numberOfContracts *= optionSpreadManager.portfolioGroupTotalMultiple;

            if (rollHashTable.Contains(rollIntoContracts.cqgSymbol))
            {

                int rowIdx = rollHashTable.FindIndex(new System.Predicate<string>(
                    (value) => { return value == rollIntoContracts.cqgSymbol; }));

                dataTable.Rows[rowIdx][2] = numberOfContracts.ToString();
                // dataTable.Rows[rowIdx][3] = active;


                //updateListViewsInRealtime(rollIntoList, rowIdx, 2, numberOfContracts.ToString());

                //updateListViewsInRealtime(rollIntoList, rowIdx, 3, active.ToString());


                if (rollIntoContracts.expression != null && rollIntoContracts.expression.cqgInstrument != null
                    //&& optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                    && rollIntoContracts.expression.instrument.eodAnalysisAtInstrument)
                {

                    dataTable.Rows[rowIdx][5] =
                        rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                                rollIntoContracts.expression.decisionPrice);

                    dataTable.Rows[rowIdx][6] =
                        rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                                rollIntoContracts.expression.transactionPrice);

                    //updateListViewsInRealtime(rollIntoList, rowIdx, 6,
                    //    rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                    //            rollIntoContracts.expression.decisionPrice));


                    //updateListViewsInRealtime(rollIntoList, rowIdx, 6,
                    //    rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                    //            rollIntoContracts.expression.transactionPrice));
                }
                else
                {
                    dataTable.Rows[rowIdx][5] = "";
                    dataTable.Rows[rowIdx][6] = "";

                    //updateListViewsInRealtime(rollIntoList, rowIdx, 6, " ");

                    //updateListViewsInRealtime(rollIntoList, rowIdx, 7, " ");
                }

                

                //dataTable.Rows[rowIdx][7] = rollIntoContracts.expression.decisionPriceFilled;

                dataTable.Rows[rowIdx][7] = rollIntoContracts.reachedBarAfterDecisionBar_ForRoll;


                //if (rollIntoContracts.expression != null)
                //{
                //    dataTable.Rows[rowIdx][7] = rollIntoContracts.expression.reachedBarAfterDecisionBar;
                //}
                //else
                //{
                //    dataTable.Rows[rowIdx][7] = false;
                //}

                //NOV 5 2014

                if (rowIdx < rollHashDisplayed.Count)
                {
                    rollHashDisplayed[rowIdx] = true;
                }
            }
            else
            {
                rollHashTable.Add(rollIntoContracts.cqgSymbol);

                rollHashDisplayed.Add(true);


                //************
                dataTable.Rows.Add();
                dataTable.Rows[dataTable.Rows.Count - 1][0] = instrument.CQGsymbol.ToString();
                dataTable.Rows[dataTable.Rows.Count - 1][1] = rollIntoContracts.cqgSymbol;
                dataTable.Rows[dataTable.Rows.Count - 1][2] = numberOfContracts;
                //dataTable.Rows[dataTable.Rows.Count - 1][3] = active;
                dataTable.Rows[dataTable.Rows.Count - 1][3] =
                    instrument.customDayBoundaryTime
                    .AddMinutes(-instrument.decisionOffsetMinutes)
                    .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
                dataTable.Rows[dataTable.Rows.Count - 1][4] =
                    instrument.customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

                //************

                if (rollIntoContracts.expression != null && rollIntoContracts.expression.cqgInstrument != null
                    && rollIntoContracts.expression.instrument.eodAnalysisAtInstrument)
                    //&& optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                {
                    dataTable.Rows[dataTable.Rows.Count - 1][5] =
                        rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                                rollIntoContracts.expression.decisionPrice);

                    dataTable.Rows[dataTable.Rows.Count - 1][6] =
                        rollIntoContracts.expression.cqgInstrument.ToDisplayPrice(
                                rollIntoContracts.expression.transactionPrice);
                }
                else
                {
                    dataTable.Rows[dataTable.Rows.Count - 1][5] = " ";

                    dataTable.Rows[dataTable.Rows.Count - 1][6] = " ";
                }

                dataTable.Rows[dataTable.Rows.Count - 1][7] = rollIntoContracts.reachedBarAfterDecisionBar_ForRoll;

                //if (rollIntoContracts.expression != null)
                //{
                //    dataTable.Rows[dataTable.Rows.Count - 1][7] = rollIntoContracts.expression.reachedBarAfterDecisionBar;
                //}
                //else
                //{
                //    dataTable.Rows[dataTable.Rows.Count - 1][7] = false;
                //}
            }

        }

        private void enterExpressionsIntoOrderSummary(List<string> orderHash,
            OptionSpreadExpression optionSpreadExpression, DataTable dataTable,
            bool orderActionTest, int numberOfContracts)
        {
            numberOfContracts *= optionSpreadManager.portfolioGroupTotalMultiple;

            

            if (orderHash.Contains(optionSpreadExpression.cqgSymbol))
            {

                int rowIdx = orderHash.FindIndex(new System.Predicate<string>(
                    (value) => { return value == optionSpreadExpression.cqgSymbol; }));

                dataTable.Rows[rowIdx][2] = numberOfContracts.ToString();
                //dataTable.Rows[rowIdx][3] = orderActionTest;


                if (optionSpreadExpression.cqgInstrument != null
                    && optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                    //&& optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                {
                    dataTable.Rows[rowIdx][5] = optionSpreadExpression.cqgInstrument.ToDisplayPrice(optionSpreadExpression.decisionPrice);
                    dataTable.Rows[rowIdx][6] = optionSpreadExpression.cqgInstrument.ToDisplayPrice(optionSpreadExpression.transactionPrice);
                }
                else
                {
                    dataTable.Rows[rowIdx][5] = "";
                    dataTable.Rows[rowIdx][6] = "";
                }

                dataTable.Rows[rowIdx][7] = optionSpreadExpression.reachedBarAfterDecisionBar;

            }
            else
            {
                orderHash.Add(optionSpreadExpression.cqgSymbol);

                dataTable.Rows.Add();
                dataTable.Rows[dataTable.Rows.Count - 1][0] = optionSpreadExpression.instrument.CQGsymbol.ToString();
                dataTable.Rows[dataTable.Rows.Count - 1][1] = optionSpreadExpression.cqgSymbol;
                dataTable.Rows[dataTable.Rows.Count - 1][2] = numberOfContracts;
                //dataTable.Rows[dataTable.Rows.Count - 1][3] = orderActionTest;
                dataTable.Rows[dataTable.Rows.Count - 1][3] =
                    optionSpreadExpression.instrument.customDayBoundaryTime
                    .AddMinutes(-optionSpreadExpression.instrument.decisionOffsetMinutes)
                    .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);
                dataTable.Rows[dataTable.Rows.Count - 1][4] =
                    optionSpreadExpression.instrument.customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

                if (optionSpreadExpression.cqgInstrument != null
                    && optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                    //&& optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                {
                    dataTable.Rows[dataTable.Rows.Count - 1][5] =
                        optionSpreadExpression.cqgInstrument.ToDisplayPrice(optionSpreadExpression.decisionPrice);
                    dataTable.Rows[dataTable.Rows.Count - 1][6] =
                        optionSpreadExpression.cqgInstrument.ToDisplayPrice(optionSpreadExpression.transactionPrice);
                }
                else
                {
                    dataTable.Rows[dataTable.Rows.Count - 1][5] = " ";
                    dataTable.Rows[dataTable.Rows.Count - 1][6] = " ";
                }

                dataTable.Rows[dataTable.Rows.Count - 1][7] = optionSpreadExpression.reachedBarAfterDecisionBar;

            }
        }



        //private void updateBackColorListView(ListView listView, int row, Color backColor)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeUpdateBackColorListViewDelegate d = new ThreadSafeUpdateBackColorListViewDelegate(threadSafeUpdateBackColorListView);

        //        this.Invoke(d, listView, row, backColor);
        //    }
        //    else
        //    {
        //        threadSafeUpdateBackColorListView(listView, row, backColor);
        //    }
        //}

        //private void threadSafeUpdateBackColorListView(ListView listView, int row, Color backColor)
        //{
        //    if (row < listView.Items.Count)
        //    {
        //        for (int itemsCount = 1; itemsCount < listView.Items[row].SubItems.Count; itemsCount++)
        //        {
        //            listView.Items[row].SubItems[itemsCount].BackColor = backColor;
        //        }
        //    }
        //}

        //private void addItemToListView(ListView listView, ListViewItem listViewItem)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeAddItemToListView d = new ThreadSafeAddItemToListView(threadSafeAddItemToListView);

        //        this.Invoke(d, listView, listViewItem);
        //    }
        //    else
        //    {
        //        threadSafeAddItemToListView(listView, listViewItem);
        //    }
        //}

        //private void threadSafeAddItemToListView(ListView listView, ListViewItem listViewItem)
        //{
        //    listView.Items.Add(listViewItem);
        //}

        //private void removeItemFromListView(ListView listView, int itemRow)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        ThreadSafeRemoveItemFromListView d = new ThreadSafeRemoveItemFromListView(threadSafeRemoveItemFromListView);

        //        this.Invoke(d, listView, itemRow);
        //    }
        //    else
        //    {
        //        threadSafeRemoveItemFromListView(listView, itemRow);
        //    }
        //}

        //private void threadSafeRemoveItemFromListView(ListView listView, int itemRow)
        //{
        //    listView.Items[itemRow].Remove();
        //}

        public void setupStrategyStateFields()
        {
            for (int strategyCount = 0; strategyCount < optionStrategies.Length; strategyCount++)
            {
                cmbBoxOptionSpreadList.Items.Add(optionSpreadManager.rowHeaderLabelCreate(optionStrategies[strategyCount]));
            }



            strategyStateFieldsList.Columns.Add("State Field", 150, HorizontalAlignment.Left);
            strategyStateFieldsList.Columns.Add("State Value", 300, HorizontalAlignment.Center);

            //optionConfigSetupGrid.ColumnCount = optionBuilderSpreadStructure[selectedSpreadIdx].optionBuildLegStructureArray.Length;

            Type optionConfigColTypes = typeof(TBL_STRATEGY_STATE_FIELDS);
            Array optionConfigColTypesArray = Enum.GetNames(optionConfigColTypes);


            strategyStateFieldsList.BeginUpdate();

            int strategyIdx = 0;

            for (int configCounter = 0; configCounter < optionConfigColTypesArray.Length; configCounter++)
            {
                //optionConfigSetupGrid.Rows[configCounter].HeaderCell.Value = optionConfigColTypesArray.GetValue(configCounter).ToString();
                ListViewItem item = new ListViewItem(optionConfigColTypesArray.GetValue(configCounter).ToString());
                item.BackColor = Color.Black;
                item.ForeColor = Color.White;
                item.UseItemStyleForSubItems = false;

                strategyStateFieldsList.Items.Add(item);


                if (optionStrategies[strategyIdx].optionStrategyParameters[configCounter].parseParameter == TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER)
                {
                    item.SubItems.Add(
                            optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValue.ToString(),
                            Color.Black, Color.LawnGreen, item.Font);
                }
                else
                {
                    if (optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValueStringNotParsed != null)
                    {
                        item.SubItems.Add(
                                optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValueStringNotParsed,
                                Color.Black, Color.LawnGreen, item.Font);
                    }
                    else
                    {
                        item.SubItems.Add(" ", Color.Black, Color.LawnGreen, item.Font);
                    }

                }

            }

            strategyStateFieldsList.EndUpdate();

            cmbBoxOptionSpreadList.SelectedIndex = 0;
        }

        public void setupRollStrikes()
        {
            int middleStrikeCount = (TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2);

            dataGridRollStrikeSelection.EnableHeadersVisualStyles = false;

            DataGridViewCellStyle colTotalPortStyle = dataGridRollStrikeSelection.ColumnHeadersDefaultCellStyle;
            colTotalPortStyle.BackColor = Color.Black;
            colTotalPortStyle.ForeColor = Color.White;

            DataGridViewCellStyle rowTotalPortStyle = dataGridRollStrikeSelection.RowHeadersDefaultCellStyle;
            rowTotalPortStyle.BackColor = Color.Black;
            rowTotalPortStyle.ForeColor = Color.White;


            int rowIdx = 0;

            for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
            {
                if (optionStrategies[stratCounter].actionType == SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY_WITH_ROLL
                    || optionStrategies[stratCounter].actionType == SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT_OR_ROLL_OVER)
                {
                    optionStrategies[stratCounter].rollStrikeGridRows = new List<LiveRowInfoIdx>();


                    int numberOfLegs = (int)optionStrategies[stratCounter]
                        .optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                    int legCounter = 0;

                    while (legCounter < numberOfLegs)
                    {
                        if (optionStrategies[stratCounter].rollIntoLegInfo[legCounter].legContractType ==
                            OPTION_SPREAD_CONTRACT_TYPE.CALL
                            ||
                            optionStrategies[stratCounter].rollIntoLegInfo[legCounter].legContractType ==
                            OPTION_SPREAD_CONTRACT_TYPE.PUT
                            ||
                            optionStrategies[stratCounter].rollIntoLegInfo[legCounter].legContractType ==
                            OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            LiveRowInfoIdx liveRowInfoIdx = new LiveRowInfoIdx();

                            liveRowInfoIdx.legIdx = legCounter;
                            //liveRowInfoIdx.rowIdx = rowIdx;

                            if (optionStrategies[stratCounter].rollIntoLegInfo[legCounter].legContractType ==
                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE
                                ||
                                optionStrategies[stratCounter].rollStrikeGridRows.Count == 0)
                            {
                                optionStrategies[stratCounter].rollStrikeGridRows.Add(liveRowInfoIdx);
                            }
                            else
                            {
                                optionStrategies[stratCounter].rollStrikeGridRows.Insert(
                                    optionStrategies[stratCounter].rollStrikeGridRows.Count - 1, liveRowInfoIdx);
                            }

                            //rowIdx++;
                        }

                        legCounter++;
                    }

                    LiveRowInfoIdx liveRowInfoIdxSummaryRow = new LiveRowInfoIdx();

                    //liveRowInfoIdxSummaryRow.rowIdx = rowIdx;

                    optionStrategies[stratCounter].rollStrikeGridRows.Add(liveRowInfoIdxSummaryRow);


                    for (int rollStrikeLoop = 0; rollStrikeLoop < optionStrategies[stratCounter].rollStrikeGridRows.Count; rollStrikeLoop++)
                    {
                        optionStrategies[stratCounter].rollStrikeGridRows[rollStrikeLoop].rowIdx = rowIdx;

                        rowIdx++;
                    }


                }
            }



            dataGridRollStrikeSelection.RowCount = rowIdx;



            Type optionWatchStrikeColumns = typeof(OPTION_WATCH_ROLL_STRIKES_COLUMNS);
            Array optionWatchStrikeColumnsArray = Enum.GetNames(optionWatchStrikeColumns);

            if (rowIdx > 0)
            {
                dataGridRollStrikeSelection.ColumnCount = TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT + optionWatchStrikeColumnsArray.Length;

                dataGridRollStrikeSelection.Columns[1].Frozen = true;
            }

            //x.Columns[(int)OPTION_LIVE_DATA_COLUMNS.L].Width = 30;
            //dataGridRollStrikeSelection.Columns[(int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.LEG].DefaultCellStyle.Font = new Font("Tahoma", 7);

            //x.Columns[(int)OPTION_LIVE_DATA_COLUMNS.TIME].Width = 70;

            Color rowColor1 = Color.DarkGray;
            Color rowColor2 = Color.Black;



            //int previousSpreadId = -1;
            Color currentRowColor = rowColor2;


            for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
            {
                if (optionStrategies[stratCounter].rollStrikeGridRows != null)
                {
                    if (currentRowColor == rowColor1)
                        currentRowColor = rowColor2;
                    else
                        currentRowColor = rowColor1;

                    int rowCounter = 0;

                    while (rowCounter < optionStrategies[stratCounter].rollStrikeGridRows.Count)
                    {
                        rowIdx = optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].rowIdx;

                        int legIdx = optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].legIdx;

                        if (rowCounter == 0)
                        {
                            dataGridRollStrikeSelection
                                .Rows[rowIdx]
                                .HeaderCell.Value =
                                optionSpreadManager.rowHeaderLabelCreate(optionStrategies[stratCounter]);

                            for (int j = 0; j < dataGridRollStrikeSelection.ColumnCount; j++)
                            {
                                dataGridRollStrikeSelection.Rows[rowIdx].Cells[j] = new CustomDataGridViewCell(true);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dataGridRollStrikeSelection.ColumnCount; j++)
                            {
                                dataGridRollStrikeSelection.Rows[rowIdx].Cells[j] = new CustomDataGridViewCell(false);
                            }
                        }

                        dataGridRollStrikeSelection
                                    .Rows[rowIdx]
                                        .HeaderCell.Style.BackColor = currentRowColor;

                        if (legIdx >= 0)
                        {
                            if (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                            OPTION_SPREAD_CONTRACT_TYPE.CALL
                            ||
                            optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                            OPTION_SPREAD_CONTRACT_TYPE.PUT)
                            {
                                dataGridRollStrikeSelection
                                                .Rows[rowIdx].Cells[(int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.CONTRACT].Value =
                                                optionStrategies[stratCounter].rollIntoLegInfo[legIdx]
                                                    .cqgSymbolWithoutStrike_ForRollover;

                                //                                                 optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionCallOrPut
                                //                                                     + "."
                                //                                                     + optionStrategies[stratCounter].instrument.CQGsymbol
                                // 
                                //                                                 + optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionMonth.ToString()
                                // 
                                //                                                 + (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionYear % 100).ToString();

                                StringBuilder legActionLabel = new StringBuilder();

                                int strikePriceInc = (int)optionStrategies[stratCounter].rollIntoLegInfo[legIdx].strikeLevelOffsetForRoll;

                                legActionLabel.Append(strikePriceInc);
                                legActionLabel.Append(" STRKS");

                                dataGridRollStrikeSelection
                                            .Rows[rowIdx].Cells[(int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.OFFSET].Value
                                            = legActionLabel.ToString();

                                legActionLabel.Clear();

                                legActionLabel.Append("LEG ");
                                legActionLabel.Append(legIdx + 1);

                                dataGridRollStrikeSelection
                                                .Rows[rowIdx].Cells[(int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.LEG].Value
                                                = legActionLabel.ToString();

                                for (int strikeCount = 0; strikeCount < TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT; strikeCount++)
                                {
                                    dataGridRollStrikeSelection
                                                .Rows[rowIdx].Cells[strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Value =

                                                    ConversionAndFormatting.convertToTickMovesString(
                                                        optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionStrikePriceReference[strikeCount],
                                                        optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                                        optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                                        );
                                }
                            }
                            else if (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                dataGridRollStrikeSelection
                                                .Rows[rowIdx].Cells[(int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.CONTRACT].Value =
                                                optionStrategies[stratCounter].rollIntoLegInfo[legIdx]
                                                    .cqgSymbol;

                                StringBuilder strikeRange = new StringBuilder();
                                for (int strikeCount = 0; strikeCount < TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT; strikeCount++)
                                {
                                    strikeRange.Clear();

                                    strikeRange.Append("[ ");
                                    strikeRange.Append(
                                        ConversionAndFormatting.convertToTickMovesString(
                                            optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceRule[0, strikeCount],
                                            optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                            optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                            ));

                                    strikeRange.Append(" ... ");

                                    strikeRange.Append(ConversionAndFormatting.convertToTickMovesString(
                                        optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceRule[1, strikeCount],
                                        optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                        optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                        ));

                                    strikeRange.Append(" )");

                                    dataGridRollStrikeSelection
                                        .Rows[rowIdx].Cells[strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Value = strikeRange.ToString();

                                    dataGridRollStrikeSelection
                                        .Rows[rowIdx].Cells[middleStrikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Style.BackColor = Color.Yellow;
                                }
                            }
                        }


                        rowCounter++;
                    }


                }
            }

        }

        public void updateStrategyStateFields(int strategyIdx)
        {
            //Type optionConfigColTypes = typeof(TBL_STRATEGY_STATE_FIELDS);
            //Array optionConfigColTypesArray = Enum.GetNames(optionConfigColTypes);


            strategyStateFieldsList.BeginUpdate();
            //optionConfigSetupGrid.RowCount = optionConfigColTypesArray.Length;

            for (int configCounter = 0; configCounter < optionStrategies[strategyIdx].optionStrategyParameters.Length; configCounter++)
            {

                //strategyStateFieldsList.Items[0].SubItems[1].Text = "test";


                if (optionStrategies[strategyIdx].optionStrategyParameters[configCounter].parseParameter == TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER)
                {
                    strategyStateFieldsList.Items[configCounter].SubItems[1].Text =
                            optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValue.ToString();
                }
                else
                {
                    if (optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValueStringNotParsed != null)
                    {
                        strategyStateFieldsList.Items[configCounter].SubItems[1].Text =
                                optionStrategies[strategyIdx].optionStrategyParameters[configCounter].stateValueStringNotParsed;
                    }
                    else
                    {
                        strategyStateFieldsList.Items[configCounter].SubItems[1].Text = " ";
                    }
                }

            }

            strategyStateFieldsList.EndUpdate();
        }



        public void sendSnapshotOfStrategyToDB()
        {
            int optionSpreadCounter = 0;

            while (optionSpreadCounter < optionStrategies.Length)
            {
                if (!optionStrategies[optionSpreadCounter].wroteIntradaySnapshotToDB)
                {

                    int legCounter = 0;

                    bool decisionPriceFilled = true;

                    while (legCounter < optionStrategies[optionSpreadCounter].legData.Length)
                    {
                        OptionSpreadExpression optionSpreadExpressionList = optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression;



                        if (!optionSpreadExpressionList.reached1MinAfterDecisionBarUsedForSnapshot)
                        {
                            decisionPriceFilled = false;

                            break;
                        }

                        legCounter++;
                    }

                    if (decisionPriceFilled)
                    {
                        optionStrategies[optionSpreadCounter].wroteIntradaySnapshotToDB = true;

                        optionSpreadManager.writeIntradayRealtimeStateToDatabase(
                            optionStrategies[optionSpreadCounter]);
                    }
                }

                optionSpreadCounter++;
            }
        }

        public void sendUpdateToLiveGrid()  //*eQuoteType cqgQuoteType,*/ int spreadExpressionIdx /*int colIdx*/)
        {
            //CQGQuote quote = optionSpreadExpressionList[spreadExpressionIdx].cqgInstrument.Quotes[cqgQuoteType];

            try
            {

                int optionSpreadCounter = 0;

                //while (optionSpreadCounter < optionStrategies.Length)
                
                if(false)
                {

                    int totalLegs = optionStrategies[optionSpreadCounter].legData.Length;

                    //if (optionSpreadExpressionList[spreadExpressionIdx].cqgInstrument != null)
                    for (int legCounter = 0; legCounter < optionStrategies[optionSpreadCounter].legData.Length; legCounter++)
                    {
                        CQGLibrary.CQGInstrument cqgInstrument = optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.cqgInstrument;

                        if (cqgInstrument != null)  // && CQG. cqgInstrument)
                        {
                            OptionSpreadExpression optionSpreadExpressionList = optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression;

                            optionSpreadManager.statusAndConnectedUpdates.checkUpdateStatus(
                                gridLiveData, optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                (int)OPTION_LIVE_DATA_COLUMNS.CONTRACT,
                                optionSpreadExpressionList);

                            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                            
                            
                            if (optionSpreadExpressionList.instrument.eodAnalysisAtInstrument)
                            {
                                //gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.TIME,
                                        optionSpreadExpressionList.lastTimeUpdated.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }
                            else
                            {
                                //gridLiveData.Columns[(int)OPTION_LIVE_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 8);

                                fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.TIME,
                                        optionSpreadExpressionList.lastTimeUpdated.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                    (int)OPTION_LIVE_DATA_COLUMNS.ASK,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.ask), false, optionSpreadExpressionList.ask);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                    (int)OPTION_LIVE_DATA_COLUMNS.BID,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.bid), false, optionSpreadExpressionList.bid);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                    (int)OPTION_LIVE_DATA_COLUMNS.LAST,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.trade), false, optionSpreadExpressionList.trade);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                    (int)OPTION_LIVE_DATA_COLUMNS.DFLT_PRICE,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.defaultPrice),
                                        false, optionSpreadExpressionList.defaultPrice);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                    (int)OPTION_LIVE_DATA_COLUMNS.STTLE,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.settlement),
                                        false, optionSpreadExpressionList.settlement);

                            if (optionSpreadExpressionList.settlementDateTime.Date.CompareTo(DateTime.Now.Date) >= 0)
                            {
                                fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadExpressionList.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, 1);
                            }
                            else
                            {
                                fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadExpressionList.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, -1);
                            }

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.YEST_STTLE,
                                        cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.yesterdaySettlement),
                                            false, optionSpreadExpressionList.yesterdaySettlement);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.impliedVol, 2).ToString(),
                                        false, optionSpreadExpressionList.impliedVol);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.THEOR_PRICE,
                                        cqgInstrument.ToDisplayPrice(
                                        optionSpreadExpressionList.theoreticalOptionPrice),
                                        false, optionSpreadExpressionList.theoreticalOptionPrice);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.SPAN_IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.impliedVolFromSpan, 2).ToString(),
                                        false, optionSpreadExpressionList.impliedVolFromSpan);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.SETL_IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.settlementImpliedVol, 2).ToString(),
                                        false, optionSpreadExpressionList.settlementImpliedVol);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.RFR,
                                        optionSpreadExpressionList.riskFreeRate.ToString(),
                                        false, optionSpreadExpressionList.riskFreeRate);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.DELTA,
                                            Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].delta, 2).ToString(),
                                            true, optionStrategies[optionSpreadCounter].legData[legCounter].delta);

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.PL_DAY_CHG,
                                            Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].pAndLDay, 2).ToString(),
                                            true, optionStrategies[optionSpreadCounter].legData[legCounter].pAndLDay);

                            int numberOfContracts = (int)optionStrategies[optionSpreadCounter].optionStrategyParameters[
                                                (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                            fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[legCounter],
                                        (int)OPTION_LIVE_DATA_COLUMNS.SPREAD_QTY,
                                            numberOfContracts.ToString(), true, numberOfContracts);
                        }
                    }

                    fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[
                            totalLegs + (int)OPTION_LIVE_DATA_SUMMARY_ROWS.TOTAL_ROW],
                                (int)OPTION_LIVE_DATA_COLUMNS.DELTA,
                                    Math.Round(optionStrategies[optionSpreadCounter].liveSpreadTotals.delta, 2).ToString(),
                                    true, optionStrategies[optionSpreadCounter].liveSpreadTotals.delta);

                    fillLiveDataPage(optionStrategies[optionSpreadCounter].liveGridRowLoc[
                            totalLegs + (int)OPTION_LIVE_DATA_SUMMARY_ROWS.TOTAL_ROW],
                                (int)OPTION_LIVE_DATA_COLUMNS.PL_DAY_CHG,
                                    Math.Round(optionStrategies[optionSpreadCounter].liveSpreadTotals.pAndLDay, 2).ToString(),
                                    true, optionStrategies[optionSpreadCounter].liveSpreadTotals.pAndLDay);



                    optionSpreadCounter++;
                }



            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }


        public void sendUpdateToPortfolioTotalGrid()
        {
            //portfolioSummaryDataTable.Rows.Add();

            //portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.PL_CHG][instrumentCnt]




            for (int instrumentCnt = 0; instrumentCnt < instrumentSpreadTotals.Length; instrumentCnt++)
            {
                portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.PL_CHG][instrumentCnt]
                    = Math.Round(instrumentSpreadTotals[instrumentCnt].pAndLDay, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

                portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.TOTAL_DELTA][instrumentCnt]
                    = Math.Round(instrumentSpreadTotals[instrumentCnt].delta, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

                portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_PL_CHG][instrumentCnt]
                    = Math.Round(instrumentADMSpreadTotals[instrumentCnt, optionSpreadManager.acctGroupSelected].pAndLDay, 2);

                portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_TOTAL_DELTA][instrumentCnt]
                    = Math.Round(instrumentADMSpreadTotals[instrumentCnt, optionSpreadManager.acctGroupSelected].delta, 2);


                //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.PL_CHG, instrumentCnt,
                //    Math.Round(instrumentSpreadTotals[instrumentCnt].pAndLDay, 2).ToString(),
                //        true, instrumentSpreadTotals[instrumentCnt].pAndLDay);

                //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.TOTAL_DELTA, instrumentCnt,
                //    Math.Round(instrumentSpreadTotals[instrumentCnt].delta, 2).ToString(),
                //        true, instrumentSpreadTotals[instrumentCnt].delta);

                //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_PL_CHG, instrumentCnt,
                //    Math.Round(instrumentADMSpreadTotals[instrumentCnt].pAndLDay, 2).ToString(),
                //        true, instrumentADMSpreadTotals[instrumentCnt].pAndLDay);

                //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_TOTAL_DELTA, instrumentCnt,
                //    Math.Round(instrumentADMSpreadTotals[instrumentCnt].delta, 2).ToString(),
                //        true, instrumentADMSpreadTotals[instrumentCnt].delta);
            }


            portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.PL_CHG][instrumentSpreadTotals.Length]
                    = Math.Round(portfolioSpreadTotals.pAndLDay, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

            portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.TOTAL_DELTA][instrumentSpreadTotals.Length]
                = Math.Round(portfolioSpreadTotals.delta, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

            portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_PL_CHG][instrumentSpreadTotals.Length]
                = Math.Round(portfolioADMSpreadTotals[optionSpreadManager.acctGroupSelected].pAndLDay, 2);

            portfolioSummaryDataTable.Rows[(int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_TOTAL_DELTA][instrumentSpreadTotals.Length]
                = Math.Round(portfolioADMSpreadTotals[optionSpreadManager.acctGroupSelected].delta, 2);


            //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.PL_CHG, instrumentSpreadTotals.Length,
            //        Math.Round(portfolioSpreadTotals.pAndLDay, 2).ToString(),
            //            true, portfolioSpreadTotals.pAndLDay);

            //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.TOTAL_DELTA, instrumentSpreadTotals.Length,
            //    Math.Round(portfolioSpreadTotals.delta, 2).ToString(),
            //        true, portfolioSpreadTotals.delta);

            //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_PL_CHG, instrumentSpreadTotals.Length,
            //        Math.Round(portfolioADMSpreadTotals.pAndLDay, 2).ToString(),
            //            true, portfolioADMSpreadTotals.pAndLDay);

            //fillPortfolioSummary(portfolioSummaryGrid, (int)PORTFOLIO_SUMMARY_GRID_ROWS.ADM_TOTAL_DELTA, instrumentSpreadTotals.Length,
            //    Math.Round(portfolioADMSpreadTotals.delta, 2).ToString(),
            //        true, portfolioADMSpreadTotals.delta);
        }

        

        public void sendUpdateToPortfolioTotalSettlementGrid()
        {
            for (int instrumentCnt = 0; instrumentCnt < instrumentSpreadTotals.Length; instrumentCnt++)
            {
                portfolioSummarySettlementDataTable.Rows[(int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_MODEL_SETTLEMENT_PL_CHG][instrumentCnt] =
                    Math.Round(instrumentSpreadTotals[instrumentCnt].pAndLDaySettlementToSettlement, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

                portfolioSummarySettlementDataTable.Rows[(int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_ADM_SETTLEMENT_PL_CHG][instrumentCnt] =
                    Math.Round(instrumentADMSpreadTotals[instrumentCnt, optionSpreadManager.acctGroupSelected].pAndLDaySettlementToSettlement, 2);

                //* optionSpreadManager.portfolioGroupTotalMultiple;

                //fillPortfolioSummary(portfolioSummaryGridSettlements, (int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_MODEL_SETTLEMENT_PL_CHG, instrumentCnt,
                //    Math.Round(instrumentSpreadTotals[instrumentCnt].pAndLDaySettlementToSettlement, 2).ToString(),
                //        true, instrumentSpreadTotals[instrumentCnt].pAndLDaySettlementToSettlement);


                //fillPortfolioSummary(portfolioSummaryGridSettlements, (int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_ADM_SETTLEMENT_PL_CHG, instrumentCnt,
                //    Math.Round(instrumentADMSpreadTotals[instrumentCnt].pAndLDaySettlementToSettlement, 2).ToString(),
                //        true, instrumentADMSpreadTotals[instrumentCnt].pAndLDaySettlementToSettlement);
            }

            portfolioSummarySettlementDataTable.Rows[(int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_MODEL_SETTLEMENT_PL_CHG][instrumentSpreadTotals.Length] =
                    Math.Round(portfolioSpreadTotals.pAndLDaySettlementToSettlement, 2)
                    * optionSpreadManager.portfolioGroupTotalMultiple;

            portfolioSummarySettlementDataTable.Rows[(int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_ADM_SETTLEMENT_PL_CHG][instrumentSpreadTotals.Length] =
                Math.Round(portfolioADMSpreadTotals[optionSpreadManager.acctGroupSelected].pAndLDaySettlementToSettlement, 2);
            //* optionSpreadManager.portfolioGroupTotalMultiple;

            //fillPortfolioSummary(portfolioSummaryGridSettlements, (int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_MODEL_SETTLEMENT_PL_CHG, instrumentSpreadTotals.Length,
            //        Math.Round(portfolioSpreadTotals.pAndLDaySettlementToSettlement, 2).ToString(),
            //            true, portfolioSpreadTotals.pAndLDaySettlementToSettlement);



            //fillPortfolioSummary(portfolioSummaryGridSettlements, (int)PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS.TOTAL_ADM_SETTLEMENT_PL_CHG, instrumentSpreadTotals.Length,
            //        Math.Round(portfolioADMSpreadTotals.pAndLDaySettlementToSettlement, 2).ToString(),
            //            true, portfolioADMSpreadTotals.pAndLDaySettlementToSettlement);

        }

        public void sendUpdateToExpressionListGrid()  //*eQuoteType cqgQuoteType,*/ int spreadExpressionIdx /*int colIdx*/)
        {
            //CQGQuote quote = optionSpreadExpressionList[spreadExpressionIdx].cqgInstrument.Quotes[cqgQuoteType];

            try
            {

                int optionSpreadCounter = 0;

                foreach (OptionSpreadExpression ose in optionSpreadExpressionList)
                {
                    if (ose.optionExpressionType != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                        &&
                        ose.dataGridExpressionListRow <= dataGridViewExpressionList.Rows.Count)
                    {
                        //ose.dataGridExpressionListRow = rowIdx;
                        CQGLibrary.CQGInstrument cqgInstrument = ose.cqgInstrument;

                        if (cqgInstrument != null)  // && CQG. cqgInstrument)
                        {
                            //OptionSpreadExpression optionSpreadExpressionList = optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression;

                            optionSpreadManager.statusAndConnectedUpdates.checkUpdateStatus(dataGridViewExpressionList, ose.dataGridExpressionListRow,
                                (int)EXPRESSION_LIST_VIEW.TIME, ose);

                            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                            if(ose.instrument.eodAnalysisAtInstrument)
                            {
                                //dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.TIME,
                                        ose.lastTimeUpdated.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }
                            else
                            {
                                //dataGridViewExpressionList.Columns[(int)EXPRESSION_LIST_VIEW.TIME].DefaultCellStyle.Font = new Font("Tahoma", 8);

                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.TIME,
                                        ose.lastTimeUpdated.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.DELTA,
                                    Math.Round(ose.delta, 2).ToString(), true, ose.delta);

                            //************************************************
                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.TRANS_PRICE,
                                    ose.transactionPrice.ToString(), false, ose.transactionPrice);

                            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                            if (ose.instrument.eodAnalysisAtInstrument)
                            {
                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.ORDER_PL,
                                        Math.Round(ose.plChgOrdersToSettlement, 2).ToString(), true, ose.plChgOrdersToSettlement);
                            }
                            else
                            {
                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.ORDER_PL,
                                        Math.Round(ose.plChgOrders, 2).ToString(), true, ose.plChgOrders);
                            }
                            //************************************************


                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.DFLT_PRICE,
                                    ose.defaultPrice.ToString(), false, ose.defaultPrice);

                            if (ose.decisionBar != null)
                            {
                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.CUM_VOL,
                                        ose.decisionBar.cumulativeVolume.ToString(), false,
                                        ose.decisionBar.cumulativeVolume);
                            }

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.THEOR_PRICE,
                                    ose.theoreticalOptionPrice.ToString(), false, ose.theoreticalOptionPrice);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.SPAN_IMPL_VOL,
                                    Math.Round(ose.impliedVolFromSpan, 2).ToString(), false, ose.impliedVolFromSpan);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.SETL_IMPL_VOL,
                                    Math.Round(ose.settlementImpliedVol, 2).ToString(), false, ose.settlementImpliedVol);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.IMPL_VOL,
                                    Math.Round(ose.impliedVol, 2).ToString(), false, ose.impliedVol);



                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.BID,
                                    ose.bid.ToString(), false, ose.bid);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.ASK,
                                    ose.ask.ToString(), false, ose.ask);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.LAST,
                                    ose.trade.ToString(), false, ose.trade);

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.STTLE,
                                    ose.settlement.ToString(), false, ose.settlement);

                            if (ose.settlementDateTime.Date.CompareTo(DateTime.Now.Date) >= 0)
                            {
                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.SETL_TIME,
                                        ose.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, 1);
                            }
                            else
                            {
                                fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                        (int)EXPRESSION_LIST_VIEW.SETL_TIME,
                                        ose.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, -1);
                            }

                            fillDataGridViewExpressionListPage(ose.dataGridExpressionListRow,
                                    (int)EXPRESSION_LIST_VIEW.YEST_STTLE,
                                    ose.yesterdaySettlement.ToString(), false, ose.yesterdaySettlement);


                        }
                    }

                    optionSpreadCounter++;
                }



            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        private void resetNumberOfContractsForEachExpression()
        {
            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                optionSpreadExpressionList[i].numberOfOrderContractsTempForCalc = 0;

                optionSpreadExpressionList[i].numberOfOrderContractsTempForCalcNotActive = 0;
            }

            for (int i = 0; i < instruments.Length; i++)
            {
                foreach (ContractList rollIntoContracts in optionSpreadManager.instrumentRollIntoSummary[i].contractHashTable.Values)
                {
                    rollIntoContracts.currentlyRollingContract = false;

                    rollIntoContracts.tempNumberOfContracts = 0;

                    rollIntoContracts.tempNumberOfContractsNotActive = 0;
                }
            }
        }

        //private void copyNumberOfContractsFromTemp()
        //{
        //    for (int i = 0; i < optionSpreadExpressionList.Count; i++)
        //    {
        //        optionSpreadExpressionList[i].numberOfContracts =
        //            optionSpreadExpressionList[i].numberOfContractsTempForCalc;

        //        //if (optionSpreadExpressionList[i].numberOfContracts != 0)
        //        //{
        //        //    TSErrorCatch.debugWriteOut("summary of contracts " +
        //        //        optionSpreadExpressionList[i].cqgSymbol + "  " +
        //        //        optionSpreadExpressionList[i].numberOfContracts);
        //        //}
        //    }
        //}

        private void updateNumberOfTempContracts(OptionSpreadExpression optionSpreadExpression,
            int contractsToTransact, bool actionTest)
        {
            if (actionTest)
            {
                optionSpreadExpression.numberOfOrderContractsTempForCalc += contractsToTransact;
            }
            else
            {
                optionSpreadExpression.numberOfOrderContractsTempForCalcNotActive += contractsToTransact;
            }

            optionSpreadExpression.contractHasOrder = true;

        }

        private void updateNumberOfRollIntoContracts(ContractList rollIntoContracts,
            int contractsToTransact, bool actionTest)
        {
            if (actionTest)
            {
                rollIntoContracts.tempNumberOfContracts += contractsToTransact;
            }
            else
            {
                rollIntoContracts.tempNumberOfContractsNotActive += contractsToTransact;
            }

            rollIntoContracts.currentlyRollingContract = true;

        }

        public void sendUpdateToOrderGrid()
        {

            resetNumberOfContractsForEachExpression();


            bool actionTest;

            StringBuilder actionRule = new StringBuilder();

            for (int i = 0; i < optionStrategies.Length; i++)
            {
                actionTest = false;

                actionRule.Clear();

                switch (optionStrategies[i].actionType)
                {
                    case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.NO_ACTION:
                        {
                            //                             bool actionTest = optionStrategies[i].entryRule.Evaluate();
                            // 
                            //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
                            //                                 optionStrategies[i].entryRule.parsedWithVariables(),  //OriginalExpression,
                            //                                 true, actionTest ? 1 : -1);
                            // 
                            //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_TEST,
                            //                                 actionTest.ToString(),
                            //                                 true, actionTest ? 1 : -1);

                            break;
                        }

                    case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY:
                        {
                            actionTest = optionStrategies[i].entryRule.Evaluate();

                            actionRule.Append(optionStrategies[i].entryRule.parsedWithVariables());
                            actionRule.Append(" : ");
                            //actionRule.Append(actionTest);

                            if (actionTest)
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ENTER));
                            }
                            else
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_ENTER));
                            }

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            actionRule.Clear();
                            actionRule.Append("FUT: ");
                            actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
                                .optionSpreadExpression.decisionPrice);

                            //actionRule.Append("\nINTRP: ");
                            //actionRule.Append(optionStrategies[i].entryRule.strategyStateComparisonValues.syntheticCloseFutureValue);

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_TEST,
                            //                                 actionTest.ToString(),
                            //                                 true, actionTest ? 1 : -1);

                            int rowIdx = 0;

                            for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
                            {
                                int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots]
                                        .stateValueParsed[legCounter];

                                LEG_ACTION_TYPES buyOrSell;

                                if (contractsToEnter > 0)
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
                                }
                                else
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
                                }


                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
                                    Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                    optionStrategies[i].legInfo[legCounter].cqgSymbol,
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
                                    contractsToEnter.ToString(),
                                    true, actionTest ? 1 : -1);

                                //if (actionTest)
                                {
                                    updateNumberOfTempContracts(
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression,
                                            contractsToEnter, actionTest);
                                }

                                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                                if (optionStrategies[i].legData[legCounter].optionSpreadExpression.instrument.eodAnalysisAtInstrument
                                    && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
                                {

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
                                            optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                    //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
                                            .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                }
                                else
                                {
                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        "", false, -1);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        "", false, -1);
                                }

                                rowIdx++;
                            }

                            break;
                        }

                    case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY_WITH_ROLL:
                        {
                            optionStrategies[i].rollStrikesUpdated = false;

                            sendUpdateToRollStrikes(i);

                            sendFutureValueUpdateToRollStrikes(i);

                            actionTest = optionStrategies[i].entryRule.Evaluate();

                            actionRule.Append(optionStrategies[i].entryRule.parsedWithVariables());
                            actionRule.Append(" : ");
                            //actionRule.Append(actionTest);

                            if (actionTest)
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ENTER_WITH_ROLL));
                            }
                            else
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_ENTER));
                            }

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            actionRule.Clear();
                            actionRule.Append("FUT: ");
                            actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
                                .optionSpreadExpression.decisionPrice);

                            //actionRule.Append("\nINTRP: ");
                            //actionRule.Append(optionStrategies[i].entryRule.strategyStateComparisonValues.syntheticCloseFutureValue);

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            int rowIdx = 0;

                            for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
                            {
                                int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots]
                                        .stateValueParsed[legCounter];

                                LEG_ACTION_TYPES buyOrSell;

                                if (contractsToEnter > 0)
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
                                }
                                else
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
                                }


                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
                                    Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
                                    true, actionTest ? 1 : -1);


                                StringBuilder cqgSymbol = new StringBuilder();
                                double strikePrice = 0;

                                if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                        cqgSymbol.ToString(),
                                        true, actionTest ? 1 : -1);
                                }
                                else
                                {

                                    cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover);
                                    cqgSymbol.Append(
                                        ConversionAndFormatting.convertToTickMovesString(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                                        [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange],
                                                        optionStrategies[i].instrument.optionStrikeIncrement,
                                                        optionStrategies[i].instrument.optionStrikeDisplay)
                                                        );

                                    strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                                        [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                        cqgSymbol.ToString(),
                                        true, actionTest ? 1 : -1);

                                }

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
                                    contractsToEnter.ToString(),
                                    true, actionTest ? 1 : -1);

                                ContractList rollIntoContracts;


                                if (optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                            .contractHashTable.Count > 0
                                    && optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                            .contractHashTable.ContainsKey(cqgSymbol.ToString()))
                                {
                                    rollIntoContracts = optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                        .contractHashTable[cqgSymbol.ToString()];
                                }
                                else
                                {
                                    rollIntoContracts = new ContractList();

                                    optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                        .contractHashTable
                                        .TryAdd(cqgSymbol.ToString(), rollIntoContracts);
                                }

                                rollIntoContracts.cqgSymbol = cqgSymbol.ToString();



                                rollIntoContracts.contractType = optionStrategies[i].rollIntoLegInfo[legCounter].legContractType;


                                if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    rollIntoContracts.contractMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].contractMonthInt;

                                    rollIntoContracts.contractYear = optionStrategies[i].rollIntoLegInfo[legCounter].contractYear;
                                }
                                else
                                {
                                    rollIntoContracts.optionMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt;

                                    rollIntoContracts.optionYear = optionStrategies[i].rollIntoLegInfo[legCounter].optionYear;

                                    rollIntoContracts.expirationDate = optionStrategies[i].rollIntoLegInfo[legCounter].expirationDate;

                                    //rollIntoContracts.yearFraction = 
                                    //    optionSpreadManager.calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
                                    //                        DateTime.Now.Date);

                                    //rollIntoContracts.idUnderlyingContract = optionStrategies[i].rollIntoLegInfo[legCounter].idUnderlyingContract;

                                    rollIntoContracts.strikePrice = strikePrice;
                                }





                                rollIntoContracts.indexOfInstrumentInInstrumentsArray = optionStrategies[i].indexOfInstrumentInInstrumentsArray;

                                rollIntoContracts.expression =
                                    optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression;

                                rollIntoContracts.futurePriceUsedToCalculateStrikes =
                                    optionStrategies[i].rollIntoLegInfo[legCounter].futurePriceUsedToCalculateStrikes;

                                rollIntoContracts.reachedBarAfterDecisionBar_ForRoll =
                                    optionStrategies[i].rollIntoLegInfo[legCounter].reachedBarAfterDecisionBar_ForRoll;

                                //rollIntoContracts.currentlyRollingContract = true;

                                //rollIntoContracts.tempNumberOfContracts += contractsToEnter;

                                //rollIntoContracts.actionTest = actionTest;

                                updateNumberOfRollIntoContracts(rollIntoContracts, contractsToEnter, actionTest);

                                //if (actionTest)
                                //{
                                //    if (optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null)
                                //    {
                                //        updateNumberOfTempContracts(
                                //            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression,
                                //                contractsToEnter);
                                //    }
                                //}

                                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                                if(optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null
                                    && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.instrument.eodAnalysisAtInstrument
                                    && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument != null)
                                {

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
                                            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPrice),
                                        true,
                                        actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                    //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPriceTime
                                            .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true,
                                        actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
                                }
                                else
                                {
                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        "", false, -1);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        "", false, -1);
                                }

                                rowIdx++;
                            }

                            break;
                        }

                    case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT:
                        {
                            actionTest = optionStrategies[i].exitRule.Evaluate();

                            actionRule.Append(optionStrategies[i].exitRule.parsedWithVariables());
                            actionRule.Append(" : ");
                            //actionRule.Append(actionTest);

                            if (actionTest)
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.EXIT_ONLY));
                            }
                            else
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_EXIT));
                            }

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            actionRule.Clear();
                            actionRule.Append("FUT: ");
                            actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
                                .optionSpreadExpression.decisionPrice);

                            //actionRule.Append("\nINTRP: ");
                            //actionRule.Append(optionStrategies[i].exitRule.strategyStateComparisonValues.syntheticCloseFutureValue);

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            int rowIdx = 0;

                            for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
                            {
                                int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
                                        .stateValueParsed[legCounter];

                                LEG_ACTION_TYPES buyOrSell;

                                if (contractsToEnter > 0)
                                {
                                    buyOrSell = LEG_ACTION_TYPES.CLOSE_BUY;
                                }
                                else
                                {
                                    buyOrSell = LEG_ACTION_TYPES.CLOSE_SELL;
                                }

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
                                    Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                    optionStrategies[i].legInfo[legCounter].cqgSymbol,
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
                                    optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
                                        .stateValueParsed[legCounter].ToString(),
                                    true, actionTest ? 1 : -1);

                                //if (actionTest)
                                {
                                    updateNumberOfTempContracts(
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression,
                                            contractsToEnter, actionTest);
                                }

                                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                                if (optionStrategies[i].legData[legCounter].optionSpreadExpression.instrument.eodAnalysisAtInstrument
                                    && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
                                {

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
                                            optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                    //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
                                            .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
                                }
                                else
                                {
                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        "", false, -1);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        "", false, -1);
                                }

                                rowIdx++;
                            }

                            break;
                        }

                    case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT_OR_ROLL_OVER:
                        {
                            optionStrategies[i].rollStrikesUpdated = false;

                            sendUpdateToRollStrikes(i);

                            sendFutureValueUpdateToRollStrikes(i);

                            bool exitTest = optionStrategies[i].exitRule.Evaluate();

                            //if (!exitTest)
                            //{

                            //}

                            //ALWAYS WILL EXIT B/C WILL BE ROLLING IF NOT EXITING
                            actionTest = true;

                            actionRule.Append(optionStrategies[i].exitRule.parsedWithVariables());
                            actionRule.Append(" : ");

                            if (exitTest)
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.EXIT_ONLY));
                            }
                            else
                            {
                                actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ROLL_OVER));
                            }

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            actionRule.Clear();
                            actionRule.Append("FUT: ");
                            actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
                                .optionSpreadExpression.decisionPrice);

                            //actionRule.Append("\nINTRP: ");
                            //actionRule.Append(optionStrategies[i].exitRule.strategyStateComparisonValues.syntheticCloseFutureValue);

                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
                                actionRule.ToString(),  //OriginalExpression,
                                true, actionTest ? 1 : -1);

                            int rowIdx = 0;

                            for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
                            {
                                int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
                                        .stateValueParsed[legCounter];

                                LEG_ACTION_TYPES buyOrSell;

                                if (contractsToEnter > 0)
                                {
                                    buyOrSell = LEG_ACTION_TYPES.CLOSE_BUY;
                                }
                                else
                                {
                                    buyOrSell = LEG_ACTION_TYPES.CLOSE_SELL;
                                }

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
                                    Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                    optionStrategies[i].legInfo[legCounter].cqgSymbol,
                                    true, actionTest ? 1 : -1);

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
                                    optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
                                        .stateValueParsed[legCounter].ToString(),
                                    true, actionTest ? 1 : -1);

                                //if (actionTest)
                                {
                                    updateNumberOfTempContracts(
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression,
                                            contractsToEnter, actionTest);
                                }

                                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                                if (optionStrategies[i].legData[legCounter].optionSpreadExpression.instrument.eodAnalysisAtInstrument
                                    && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
                                {

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
                                            optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                    //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
                                            .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true,
                                        actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
                                }
                                else
                                {
                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        "", false, -1);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        "", false, -1);
                                }

                                rowIdx++;
                            }


                            //************************



                            //int rowIdx = 0;

                            if (exitTest)
                            {
                                actionTest = false;  //don't roll into spread because exit rule true;
                            }

                            for (int legCounter = 0; legCounter < optionStrategies[i].rollIntoLegInfo.Length; legCounter++)
                            {
                                int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition]
                                        .stateValueParsed[legCounter];

                                LEG_ACTION_TYPES buyOrSell;

                                if (contractsToEnter > 0)
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
                                }
                                else
                                {
                                    buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
                                }


                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
                                    Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
                                    true, actionTest ? 1 : -1);


                                StringBuilder cqgSymbol = new StringBuilder();

                                //StringBuilder symbolComparison = new StringBuilder();

                                double strikePrice = 0;

                                if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                        cqgSymbol.ToString(),
                                        true, actionTest ? 1 : -1);
                                }
                                else
                                {
                                    //if (optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol == null)
                                    {
                                        //TSErrorCatch.debugWriteOut(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol + "  "
                                        //    + cqgSymbol.ToString());

                                        //cqgSymbol = new StringBuilder();
                                        cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover);
                                        //cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                        //                    [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange]);

                                        cqgSymbol.Append(
                                            ConversionAndFormatting.convertToTickMovesString(
                                                optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                                            [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange],
                                                            optionStrategies[i].instrument.optionStrikeIncrement,
                                                            optionStrategies[i].instrument.optionStrikeDisplay)
                                                            );

                                        strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                                        [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

                                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                            cqgSymbol.ToString(),
                                            true, actionTest ? 1 : -1);
                                    }
                                    //else
                                    //{
                                    //    cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

                                    //    strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
                                    //                    [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

                                    //    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
                                    //        cqgSymbol.ToString(),
                                    //        true, actionTest ? 1 : -1);
                                    //}



                                }

                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
                                    contractsToEnter.ToString(),
                                    true, actionTest ? 1 : -1);

                                //if (actionTest)
                                //{
                                //    if (optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null)
                                //    {
                                //        updateNumberOfTempContracts(
                                //            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression,
                                //                contractsToEnter);
                                //    }
                                //}

                                ContractList rollIntoContracts;

                                if (optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                            .contractHashTable.Count > 0
                                    && optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                            .contractHashTable.ContainsKey(cqgSymbol.ToString()))
                                {
                                    rollIntoContracts = optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                        .contractHashTable[cqgSymbol.ToString()];
                                }
                                else
                                {
                                    rollIntoContracts = new ContractList();

                                    //instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                    //    .contractHashTable.Add(cqgSymbol.ToString(), rollIntoContracts);

                                    //int removeAttempts = 0;

                                    //while (!instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                    //    .contractHashTable
                                    //    .TryAdd(cqgSymbol.ToString(), rollIntoContracts) && removeAttempts < 10)
                                    //{
                                    //    removeAttempts++;
                                    //}

                                    optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
                                        .contractHashTable
                                        .TryAdd(cqgSymbol.ToString(), rollIntoContracts);
                                }

                                rollIntoContracts.cqgSymbol = cqgSymbol.ToString();


                                rollIntoContracts.contractType = optionStrategies[i].rollIntoLegInfo[legCounter].legContractType;

                                if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    rollIntoContracts.contractMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].contractMonthInt;

                                    rollIntoContracts.contractYear = optionStrategies[i].rollIntoLegInfo[legCounter].contractYear;
                                }
                                else
                                {
                                    rollIntoContracts.optionMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt;

                                    rollIntoContracts.optionYear = optionStrategies[i].rollIntoLegInfo[legCounter].optionYear;


                                    rollIntoContracts.expirationDate = optionStrategies[i].rollIntoLegInfo[legCounter].expirationDate;

                                    //rollIntoContracts.yearFraction =
                                    //    optionSpreadManager.calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
                                    //                        DateTime.Now.Date);

                                    //rollIntoContracts.idUnderlyingContract = optionStrategies[i].rollIntoLegInfo[legCounter].idUnderlyingContract;

                                    rollIntoContracts.strikePrice = strikePrice;
                                }

                                rollIntoContracts.indexOfInstrumentInInstrumentsArray = optionStrategies[i].indexOfInstrumentInInstrumentsArray;

                                rollIntoContracts.expression =
                                    optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression;

                                rollIntoContracts.futurePriceUsedToCalculateStrikes =
                                    optionStrategies[i].rollIntoLegInfo[legCounter].futurePriceUsedToCalculateStrikes;

                                rollIntoContracts.reachedBarAfterDecisionBar_ForRoll =
                                    optionStrategies[i].rollIntoLegInfo[legCounter].reachedBarAfterDecisionBar_ForRoll;

                                //rollIntoContracts.currentlyRollingContract = true;

                                //rollIntoContracts.tempNumberOfContracts += contractsToEnter;

                                //rollIntoContracts.actionTest = actionTest;

                                updateNumberOfRollIntoContracts(rollIntoContracts, contractsToEnter, actionTest);

                                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                                if (optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null
                                    && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument != null
                                    && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                                {

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
                                            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPrice),
                                        true,
                                        actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



                                    //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPriceTime
                                            .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true,
                                        actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
                                        && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
                                }
                                else
                                {
                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
                                        "", false, -1);

                                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
                                        "", false, -1);
                                }

                                rowIdx++;
                            }

                            break;

                        }


                }
            }

            //copyNumberOfContractsFromTemp();

        }

        public void sendUpdateToRollStrikes(int stratCounter)
        {
            //for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
            {
                if (optionStrategies[stratCounter].rollStrikeGridRows != null)
                {
                    int rowCounter = 0;

                    while (rowCounter < optionStrategies[stratCounter].rollStrikeGridRows.Count)
                    {
                        int rowIdx = optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].rowIdx;

                        int legIdx = optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].legIdx;

                        double futureClose = 0;

                        if (legIdx >= 0)
                        {

                            StringBuilder strikeRange = new StringBuilder();

                            for (int strikeCount = 0; strikeCount < TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT; strikeCount++)
                            {
                                if (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                                    OPTION_SPREAD_CONTRACT_TYPE.CALL
                                    ||
                                    optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                                    OPTION_SPREAD_CONTRACT_TYPE.PUT)
                                {
                                    fillRollStrikePage(rowIdx, strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES,
                                        ConversionAndFormatting.convertToTickMovesString(
                                                        optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionStrikePriceReference[strikeCount],
                                                        optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                                        optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                                        ), false, 0);
                                }
                                else if (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType ==
                                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    strikeRange.Clear();

                                    strikeRange.Append("[ ");
                                    strikeRange.Append(
                                        ConversionAndFormatting.convertToTickMovesString(
                                            optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceRule[0, strikeCount],
                                            optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                            optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                            ));

                                    strikeRange.Append(" ... ");

                                    strikeRange.Append(ConversionAndFormatting.convertToTickMovesString(
                                        optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceRule[1, strikeCount],
                                        optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                        optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                        ));

                                    strikeRange.Append(" )");

                                    //dataGridRollStrikeSelection
                                    //    .Rows[rowIdx].Cells[strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Value = strikeRange.ToString();

                                    //dataGridRollStrikeSelection
                                    //   .Rows[rowIdx].Cells[middleStrikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Style.BackColor = Color.Yellow;


                                    fillRollStrikePage(rowIdx, strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES,
                                        strikeRange.ToString(), false, 0);

                                    futureClose = optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceUsedToCalculateStrikes;
                                }

                                //dataGridRollStrikeSelection
                                //            .Rows[rowIdx].Cells[strikeCount + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES].Value =

                                //                ConversionAndFormatting.convertToTickMovesString(
                                //                    optionStrategies[stratCounter].rollIntoLegInfo[legIdx].optionStrikePriceReference[strikeCount],
                                //                    optionStrategies[stratCounter].instrument.optionStrikeIncrement,
                                //                    optionStrategies[stratCounter].instrument.optionStrikeDisplay
                                //                    );
                            }


                        }
                        else
                        {
                            fillRollStrikePage(rowIdx, TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2
                                + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES,
                                        futureClose.ToString(), true, 1);
                        }

                        rowCounter++;

                    }  // end of while (rowCounter < optionStrategies[stratCounter].rollStrikeGridRows.Count)
                }
            }
        }

        public void sendFutureValueUpdateToRollStrikes(int stratCounter)
        {
            //for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
            {
                if (optionStrategies[stratCounter].rollIntoLegInfo != null)
                {
                    bool foundFuture = false;

                    int futureIdx = 0;

                    while (futureIdx < optionStrategies[stratCounter].rollIntoLegInfo.Length)
                    {
                        if (optionStrategies[stratCounter].rollIntoLegInfo[futureIdx].legContractType
                            == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            foundFuture = true;
                            break;
                        }

                        futureIdx++;
                    }

                    for (int legIdx = 0; legIdx < optionStrategies[stratCounter].rollIntoLegInfo.Length; legIdx++)
                    {
                        //if (optionStrategies[stratCounter].rollIntoLegInfo[legIdx].legContractType
                        //    != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            optionStrategies[stratCounter].rollIntoLegInfo[legIdx].futurePriceUsedToCalculateStrikes =
                                optionStrategies[stratCounter].rollIntoLegInfo[futureIdx].futurePriceUsedToCalculateStrikes;

                            optionStrategies[stratCounter].rollIntoLegInfo[legIdx].reachedBarAfterDecisionBar_ForRoll =
                                optionStrategies[stratCounter].rollIntoLegInfo[futureIdx].reachedBarAfterDecisionBar_ForRoll;
                        }
                    }

                    if (foundFuture)
                    {
                        int rowCounter = 0;

                        while (rowCounter < optionStrategies[stratCounter].rollStrikeGridRows.Count)
                        {
                            if (optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].legIdx < 0)
                            {
                                fillRollStrikePage(optionStrategies[stratCounter].rollStrikeGridRows[rowCounter].rowIdx,
                                    TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2
                                    + (int)OPTION_WATCH_ROLL_STRIKES_COLUMNS.STRIKES,
                                    ConversionAndFormatting.convertToTickMovesString(
                                    optionStrategies[stratCounter].rollIntoLegInfo[futureIdx].futurePriceUsedToCalculateStrikes,
                                    optionStrategies[stratCounter].instrument.tickSize,
                                    optionStrategies[stratCounter].instrument.tickDisplay), true, 1);

                                break;
                            }

                            rowCounter++;
                        }
                    }
                }
            }
        }

        public void fillLiveDataPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            //try
            //{
            //    if (this.InvokeRequired)
            //    {
            //        ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillLiveDataPage);

            //        this.Invoke(d, row, col, displayValue, updateColor, value);
            //    }
            //    else
            //    {
            //        threadSafeFillLiveDataPage(row, col, displayValue, updateColor, value);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            //}
        }

        public void threadSafeFillLiveDataPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                if (
                    //rowToUpdate < gridLiveData.RowCount
                    //&&
                    (
                    gridLiveData.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    gridLiveData.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    ))
                {
                    gridLiveData.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridLiveData.Rows[rowToUpdate].Cells[col].Style.BackColor = plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void fillDataGridViewExpressionListPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillDataGridViewExpressionListPage);

                    this.Invoke(d, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillDataGridViewExpressionListPage(row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillDataGridViewExpressionListPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                if (
                    (
                    dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    ))
                {
                    dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        dataGridViewExpressionList.Rows[rowToUpdate].Cells[col].Style.BackColor = plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void fillContractSummaryLiveData(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillContractSummaryLiveData);

                    this.Invoke(d, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillContractSummaryLiveData(row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillContractSummaryLiveData(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                if (gridViewContractSummary.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    gridViewContractSummary.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    )
                {
                    gridViewContractSummary.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridViewContractSummary.Rows[rowToUpdate].Cells[col].Style.BackColor = plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        //*********
        public void hideUnhideSummaryData(DataGridView gridView, int row, bool visible)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
                        new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideSummaryData);

                    this.Invoke(d, gridView, row, visible);
                }
                else
                {
                    threadSafeHideUnhideSummaryData(gridView, row, visible);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeHideUnhideSummaryData(DataGridView gridView, int row, bool visible)
        {
            try
            {
                int rowToUpdate = row;

                gridView.Rows[rowToUpdate].Visible = visible;
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }
        //*********

        //public void hideUnhideStrategyRRisk(int row, bool visible)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
        //                new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideStrategyRRisk);

        //            this.Invoke(d, row, visible);
        //        }
        //        else
        //        {
        //            threadSafeHideUnhideStrategyRRisk(row, visible);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeHideUnhideStrategyRRisk(int row, bool visible)
        //{
        //    try
        //    {
        //        int rowToUpdate = row;

        //        dataGridStrategyRRiskSummary.Rows[rowToUpdate].Visible = visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void hideUnhideOrders(int row, bool visible)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
        //                new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideOrders);

        //            this.Invoke(d, row, visible);
        //        }
        //        else
        //        {
        //            threadSafeHideUnhideOrders(row, visible);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeHideUnhideOrders(int row, bool visible)
        //{
        //    try
        //    {
        //        int rowToUpdate = row;

        //        gridOrders.Rows[rowToUpdate].Visible = visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void hideUnhideGridLive(int row, bool visible)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
        //                new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideGridLive);

        //            this.Invoke(d, row, visible);
        //        }
        //        else
        //        {
        //            threadSafeHideUnhideGridLive(row, visible);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeHideUnhideGridLive(int row, bool visible)
        //{
        //    try
        //    {
        //        int rowToUpdate = row;

        //        gridLiveData.Rows[rowToUpdate].Visible = visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        ////hide unhide gridLiveADMData
        //public void hideUnhideGridLiveADMData(int row, bool visible)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
        //                new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideGridLiveADMData);

        //            this.Invoke(d, row, visible);
        //        }
        //        else
        //        {
        //            threadSafeHideUnhideGridLiveADMData(row, visible);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeHideUnhideGridLiveADMData(int row, bool visible)
        //{
        //    try
        //    {
        //        int rowToUpdate = row;

        //        gridLiveADMData.Rows[rowToUpdate].Visible = visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        ////**************************

        //public void hideUnhideGridModelADMCompare(int row, bool visible)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeHideUnhideContractSummaryLiveDataDelegate d =
        //                new ThreadSafeHideUnhideContractSummaryLiveDataDelegate(threadSafeHideUnhideGridModelADMCompare);

        //            this.Invoke(d, row, visible);
        //        }
        //        else
        //        {
        //            threadSafeHideUnhideGridModelADMCompare(row, visible);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeHideUnhideGridModelADMCompare(int row, bool visible)
        //{
        //    try
        //    {
        //        int rowToUpdate = row;

        //        gridViewModelADMCompare.Rows[rowToUpdate].Visible = visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}



        public void fillLiveADMDataPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillLiveADMDataPage);

                    this.Invoke(d, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillLiveADMDataPage(row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillLiveADMDataPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                if (gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    )
                {
                    gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridLiveFCMData.Rows[rowToUpdate].Cells[col].Style.BackColor = plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        //public void fillPortfolioSummary(DataGridView gridView, int row, int col, String displayValue,
        //    bool updateColor, double value)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeFillLiveDataGridViewPageDelegate d = new ThreadSafeFillLiveDataGridViewPageDelegate(
        //                threadSafeFillPortfolioSummary);

        //            this.Invoke(d, gridView, row, col, displayValue, updateColor, value);
        //        }
        //        else
        //        {
        //            threadSafeFillPortfolioSummary(gridView, row, col, displayValue, updateColor, value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        public void threadSafeFillPortfolioSummary(DataGridView gridView, int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (gridView.Rows[row].Cells[col].Value == null
                    ||
                    gridView.Rows[row].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    )
                {
                    gridView.Rows[row].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridView.Rows[row].Cells[col].Style.BackColor = plUpDownColor(value);
                        //portfolioSummaryGrid.Rows[row].Cells[col].Style.ForeColor = Color.Black;
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void fillLiveOrderPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillOrderPage);

                    this.Invoke(d, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillOrderPage(row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillOrderPage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                Color updatedColor = orderEngagedColor(value);

                if (gridOrders.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    gridOrders.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    ||
                    !updatedColor.Equals(gridOrders.Rows[rowToUpdate].Cells[col].Style.BackColor)
                    )
                {
                    gridOrders.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridOrders.Rows[rowToUpdate].Cells[col].Style.BackColor = orderEngagedColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void fillRollStrikePage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeRollStrikePage);

                    this.Invoke(d, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeRollStrikePage(row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeRollStrikePage(int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                Color updatedColor = plUpDownColor(value);

                if (dataGridRollStrikeSelection.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    dataGridRollStrikeSelection.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    ||
                    !updatedColor.Equals(dataGridRollStrikeSelection.Rows[rowToUpdate].Cells[col].Style.BackColor)
                    )
                {
                    dataGridRollStrikeSelection.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        dataGridRollStrikeSelection.Rows[rowToUpdate].Cells[col].Style.BackColor = plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private Color plUpDownColor(double value)
        {
            if (value >= 0)
            {
                return Color.LawnGreen;
            }
            else
            {
                return Color.Red;
            }
        }

        private Color orderEngagedColor(double value)
        {
            if (value >= 0)
            {
                return Color.LawnGreen;
            }
            else
            {
                return Color.WhiteSmoke;
            }
        }

        internal void draggingFileCheck(object sender, DragEventArgs e)
        {


            Array files = (Array)(e.Data.GetData(DataFormats.FileDrop));

            if (fileLoadListForm == null)
            {
                fileLoadListForm = new FileLoadList(this);
            }

            fileLoadListForm.Show();
            fileLoadListForm.BringToFront();

            for (int i = 0; i < files.Length; i++)
            {
                fileLoadListForm.loadFiles((String)files.GetValue(i));
            }
        }

        public void closeFileLoadList()
        {
            fileLoadListForm.Close();

            fileLoadListForm = null;
        }

        public void loadFiles(String[] files)
        {


            if (files != null && files.Length > 0)
            {


                //String configFileName = files;  // (String)files.GetValue(0);

                //fileLoadListForm.loadFiles(configFileName);

                {
                    bool updatedSettings = false;

                    if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                    {
                        optionSpreadManager.realtimeMonitorSettings.eodAnalysis = false;

                        updatedSettings = true;
                    }

                    if (settings == null)
                    {
                        settings = new Settings(optionSpreadManager);
                    }
                    else
                    {
                        settings.updateSettings();
                    }

                    if (updatedSettings)
                    {
                        optionSpreadManager.updateEODMonitorDataSettings(false);
                    }

                    Thread loadingADMFileThread = new Thread(new ParameterizedThreadStart(loadingADMFile));
                    loadingADMFileThread.IsBackground = true;
                    loadingADMFileThread.Start(files);

                    //loadingADMFile(files);

                    //loadingADMFile(configFileName);
                    //optionSpreadManager.displayADMInputWithWebPositions();
                    //optionSpreadManager.displayADMInputWithWebPositions();
                }
            }
        }

        private void OptionRealtimeMonitor_DragDrop(object sender, DragEventArgs e)
        {

            draggingFileCheck(sender, e);

        }

        private void loadingADMFile(Object configFileNameObj)
        {
            this.Invoke(new EventHandler(optionSpreadManager.openThread));

            String[] configFileName = (String[])configFileNameObj;

            ImportFileCheck importFileCheck = new ImportFileCheck();

            importFileCheck.importingBackedUpSavedFile = false;

            //bool importFile = 

            optionSpreadManager.fillADMInputWithWebPositions(configFileName,
                importFileCheck);

            if (importFileCheck.importfile)
            {
                //optionSpreadManager.aDMDataCommonMethods.copyADMStoredDataFile(configFileName);


                //June 15 2015
                optionSpreadManager.aDMDataCommonMethods.copyADMDataToFile(
                    optionSpreadManager.admPositionImportWebForImportDisplay,
                    importFileCheck);
                //

                optionSpreadManager.modelADMCompareCalculationAndDisplay.fillGridModelADMComparison(this);

                gridViewFCMPostionManipulation.fillGridLiveADMData(this);



                optionSpreadManager.resetDataUpdatesWithLatestExpressions();
            }

            ThreadSafeGenericDelegateWithoutParams displayFCMDataDelegateRun =
                new ThreadSafeGenericDelegateWithoutParams(
                    optionSpreadManager.displayADMInputWithWebPositions);

            this.Invoke(displayFCMDataDelegateRun);

            //optionSpreadManager.displayADMInputWithWebPositions();

            this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        }


        private void admPositionsCheckAndFillGrid(ADMPositionImportWeb aDMPositionImportWeb,
            int rowCounter, bool isInADMList)
        {
            //DateTime currentDate = DateTime.Now;


            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CONTRACT].Value =
            //                    liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.cqgSymbol;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LEG].Value = legCounter + 1;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.NET].Value =
            //    liveADMStrategyInfo.admLegInfo[legCounter].numberOfContracts;



            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.MODEL].Value =
            //                liveADMStrategyInfo.admLegInfo[legCounter].numberOfModelContracts;


            //TimeSpan span = liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.expirationDate.Date - currentDate.Date;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CNTDN].Value =
            //                    span.Days;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.EXPR].Value =
            //    new DateTime(
            //                        liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.expirationDate.Year,
            //                        liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.expirationDate.Month,
            //                        liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.expirationDate.Day,
            //                        liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.optionExpirationTime.Hour,
            //                        liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.legInfo.optionExpirationTime.Minute,
            //                        0
            //                    )
            //                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo);

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.AVERAGE_PRC].Value =
            //    liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.AveragePrice;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.STRIKE].Value =
            //    liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.strike;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.DESCRIP].Value =
            //    liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.Description;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CUSIP].Value =
            //    liveADMStrategyInfo.admLegInfo[legCounter].aDMPositionImportWeb.PCUSIP;

            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.SPREAD_ID].Value = strategyCount;





        }

        private void checkFuturesModelLotsAgainstADMNetAndHighLight(int legIdx, int rowIdx)
        {
            //if (liveADMStrategyInfo.admLegInfo[legIdx].numberOfModelContracts !=
            //        liveADMStrategyInfo.admLegInfo[legIdx].numberOfContracts)
            //{
            //    gridLiveADMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.MODEL].Style.BackColor = Color.Aquamarine;
            //}
            //else
            //{
            //    gridLiveADMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.MODEL].Style.BackColor = Color.White;
            //}
        }

        private void OptionRealtimeMonitor_DragEnter(object sender, DragEventArgs e)
        {
#if DEBUG
            try
#endif
            {
                e.Effect = DragDropEffects.All;
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void hideMessageToReconnect()
        {
            if (this.InvokeRequired)
            {
                ThreadSafeMoveSplitterDistance d =
                    new ThreadSafeMoveSplitterDistance(threadSafeMoveSplitter);

                this.Invoke(d, 0);
            }
            else
            {
                threadSafeMoveSplitter(0);
            }
        }

        public void displayMessageToReConnect()
        {
            //splitContainer4.SplitterDistance = 50;

            if (this.InvokeRequired)
            {
                ThreadSafeMoveSplitterDistance d =
                    new ThreadSafeMoveSplitterDistance(threadSafeMoveSplitter);

                this.Invoke(d, 50);
            }
            else
            {
                threadSafeMoveSplitter(50);
            }
        }

        public void threadSafeMoveSplitter(int splitterDistance)
        {
            try
            {
                splitContainer4.SplitterDistance = splitterDistance;
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settings == null)
            {
                settings = new Settings(optionSpreadManager);
            }

            settings.Show();

            settings.BringToFront();
        }

        private void cmbBoxOptionSpreadList_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateStrategyStateFields(cmbBoxOptionSpreadList.SelectedIndex);
        }

        //private void checkUpdateStatus(DataGridView gridView,
        //    int row, int col, OptionSpreadExpression optionSpreadExpression)
        //{
        //    Color backColor;

        //    if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
        //    {
        //        backColor = Color.Plum;

        //    }
        //    else if (optionSpreadExpression.minutesSinceLastUpdate > TradingSystemConstants.MINUTES_STALE_LIVE_UPDATE)
        //    {
        //        backColor = Color.Yellow;
        //    }
        //    else
        //    {
        //        backColor = Color.LightSkyBlue;

        //    }

        //    markLiveAsConnected(gridView, row,
        //                            col,
        //                            true, backColor);

        //}



        //public void markLiveAsConnected(DataGridView gridView, int row, int col, bool connected, Color backgroundColor)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            ThreadSafeMarkAsConnectedDelegate d = new ThreadSafeMarkAsConnectedDelegate(threadSafeMarkAsConnected);

        //            this.Invoke(d, gridView, row, col, connected, backgroundColor);
        //        }
        //        else
        //        {
        //            threadSafeMarkAsConnected(gridView, row, col, connected, backgroundColor);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        //public void threadSafeMarkAsConnected(DataGridView gridView, int row, int col, bool connected, Color backgroundColor)
        //{
        //    try
        //    {
        //        if (gridView.Rows[row].Cells[col].Style.BackColor != backgroundColor)
        //        {
        //            gridView.Rows[row].Cells[col].Style.BackColor = backgroundColor;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
        //    }
        //}

        private void bringUpADMWebpageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //             ProcessStartInfo psi = new ProcessStartInfo(@"<Path for Internet explorer - iexplorer.exe file>");
            // 
            //             psi.Arguments = @"<path for your web application>";
            // 
            //             Process.Start(psi);

            System.Diagnostics.Process.Start("IEXPLORE.EXE", "https://members.admis.com/AccountLogin.aspx?ReturnUrl=%2fdefault.aspx");
        }

        private void showADMWebImportedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionSpreadManager.displayADMInputWithWebPositions();

            //updateColorOfADMStrategyGrid();
        }

        private void gridLiveADMData_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void gridLiveADMData_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

            if (e.Data.GetDataPresent(typeof(ADMDragOverData)))
            {


                Point clientPoint = gridLiveFCMData.PointToClient(new Point(e.X, e.Y));

                int rowIndexOfItemUnderMouseToDrop = gridLiveFCMData.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (rowIndexOfItemUnderMouseToDrop != -1)
                {
                    gridLiveFCMData.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                }
            }
        }

        private void gridLiveADMData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != (int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_EDITABLE)
            {
                e.Cancel = true;
            }

        }

        private void gridLiveADMData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_EDITABLE)
            {

                int admPosWebIdx = Convert.ToInt16(gridLiveFCMData.Rows[e.RowIndex].Cells[
                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.ADMPOSWEB_IDX].Value);

                int contracts = Convert.ToInt16(gridLiveFCMData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                if (contracts != Convert.ToInt16(gridLiveFCMData.Rows[e.RowIndex].Cells[
                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_AT_ADM].Value))
                {
                    gridLiveFCMData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                }
                else
                {
                    gridLiveFCMData.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }

                optionSpreadManager.admPositionImportWeb[admPosWebIdx].netContractsEditable = contracts;




            }

        }

        private void checkContractsAllocateAndNotAllocatedUsingCUSIP(String cusip)
        {



        }

        public void checkContractsAllocateAndNotAllocatedUsingCqgSymbol(String cqgSymbol)
        {



        }





        private void OptionRealtimeMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            Cursor.Current = Cursors.WaitCursor;

            //portfolioSummaryGrid.Rows[1].Cells[1].Value = "9999";

            //Thread threadTest = new Thread(new ParameterizedThreadStart(runSystem));
            //threadTest.IsBackground = true;
            //threadTest.Start();

            //Thread.Sleep(7000);

            optionSpreadManager.shutDownOptionSpreadRealtime();
        }

        //private void runSystem(Object x)
        //{
        //    optionSpreadManager.openThread(null, null);
        //    //this.Invoke(new EventHandler(optionSpreadManager.openThread));


        //    portfolioSummaryGrid.Rows[1].Cells[1].Value = "10000";

        //    Thread.Sleep(5000);

        //    portfolioSummaryGrid.Rows[1].Cells[1].Value = "50000";

        //    //this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        //    optionSpreadManager.closeThread(null, null);
        //}

        //private void treeViewInstruments_AfterSelect(object sender, TreeViewEventArgs e)
        private void updateOrderSummaryAfterInstrumentSelect()
        {
            //TreeNode x = treeViewInstrumentsXXX.SelectedNode;

            //if (x != null)
            {
                //orderSummaryInstrumentSelectedIdx = x.Index;



                //if (idx == instruments.Length)
                {
                    //orderSummaryList.Items.Clear();

                    //rollIntoOrderSummaryList.Items.Clear();

                    //orderSummaryHashTable.Clear();

                    //orderRollIntoSummaryHashTable.Clear();

                    //orderRollIntoSummaryHashTableDisplayed.Clear();

                    //orderSummaryList.Items.Clear();
                    //orderSummaryListNotActive.Items.Clear();

                    optionSpreadManager.orderSummaryDataTable.Clear();
                    optionSpreadManager.orderSummaryNotActiveDataTable.Clear();


                    optionSpreadManager.rollIntoOrderSummaryDataTable.Clear();
                    optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable.Clear();


                    optionSpreadManager.orderSummaryHashTable.Clear();
                    optionSpreadManager.orderSummaryNotActiveHashTable.Clear();


                    optionSpreadManager.orderRollIntoSummaryHashTable.Clear();
                    optionSpreadManager.orderRollIntoSummaryHashTableDisplayed.Clear();

                    optionSpreadManager.orderRollIntoSummaryHashTableNotActive.Clear();
                    optionSpreadManager.orderRollIntoSummaryHashTableDisplayedNotActive.Clear();

                }
            }

            fillOrderSummaryList();
        }

        private void copyListToClipboard(ListView listView1)
        {

            // Bail out early if no selected items 
            if (listView1.SelectedItems.Count == 0)
                return;

            StringBuilder buffer = new StringBuilder();
            // Loop over all the selected items 
            foreach (ListViewItem currentItem in listView1.SelectedItems)
            {
                // Don't need to look at currentItem, because it is in subitem[0] 
                // So just loop over all the subitems of this selected item 
                foreach (ListViewItem.ListViewSubItem sub in currentItem.SubItems)
                {
                    // Append the text and tab 
                    buffer.Append(sub.Text);
                    buffer.Append("\t");
                }
                // Annoyance: there is a trailing tab in the buffer, get rid of it 
                buffer.Remove(buffer.Length - 1, 1);
                // If you only use \n, not all programs (notepad!!!) will recognize the newline 
                buffer.Append("\r\n");
            }
            // Set output to clipboard. 
            Clipboard.SetDataObject(buffer.ToString(), true);

        }

        private void orderSummaryList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                copyListToClipboard((ListView)sender);
            }
        }




        private void makeCMEMarginRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CMEMarginCall cmeMarginCall = new CMEMarginCall(optionSpreadExpressionList, optionStrategies,
                instruments, this, portfolioSpreadTotals, optionSpreadManager.admPositionImportWeb, false);

            cmeMarginCall.generateMarginRequest();

            //Thread calcMarginCallThread = new Thread(new ParameterizedThreadStart(cmeMarginCall.generateMarginRequest));

            //calcMarginCallThread.Start();

            //cmeMarginCall.generateMarginRequest(optionSpreadExpressionList, optionStrategies,
            //    instruments, this, portfolioSpreadTotals);

            //for (int i = 0; i < instruments.Length; i++)
            //{
            //    instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_INIT_MARGIN].Cells[i].Value =
            //        instruments[i].coreAPIinitialMargin;

            //    instrumentSummaryGrid.Rows[(int)INSTRUMENT_SUMMARY_GRID_ROWS.SPAN_MAINT_MARGIN].Cells[i].Value =
            //        instruments[i].coreAPImaintenanceMargin;
            //}
        }




        //private void treeViewInstrumentsSummary_AfterSelect(object sender, TreeViewEventArgs e)
        private void updateInstrumentSummaryAfterInstrumentChg()
        {
            //TreeNode x = treeViewInstrumentsSummary.SelectedNode;

            //if (x != null)
            {
                //int instrumentSummarySelectedIdx = x.Index;

                if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
                {
                    for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                    {
                        hideUnhideSummaryData(dataGridStrategyRRiskSummary, stratCounter, true);
                    }
                }
                else
                {
                    for (int stratCounter = 0; stratCounter < optionStrategies.Length; stratCounter++)
                    {
                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionStrategies[stratCounter].indexOfInstrumentInInstrumentsArray)
                        {
                            hideUnhideSummaryData(dataGridStrategyRRiskSummary, stratCounter, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(dataGridStrategyRRiskSummary, stratCounter, false);
                        }
                    }
                }
            }
        }

        private void treeViewInstrumentsContractSummary_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            TreeNode x = treeViewInstruments.SelectedNode;

            if (x != null)
            {
                optionSpreadManager.contractSummaryInstrumentSelectedIdx = x.Index;

                updateSelectedInstrumentFromTree();


                optionSpreadManager.updateADMSummaryForm(optionSpreadManager.contractSummaryInstrumentSelectedIdx);

            }
        }

        private void updateSelectedInstrumentFromTree()
        {
            //TreeNode x = treeViewInstruments.SelectedNode;

            //if (x != null)
            {
                //contractSummaryInstrumentSelectedIdx = x.Index;

                if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
                {
                    for (int contractSummaryExpressionCnt = 0;
                        contractSummaryExpressionCnt < contractSummaryExpressionListIdx.Count;
                        contractSummaryExpressionCnt++)
                    {
                        hideUnhideSummaryData(gridViewContractSummary, contractSummaryExpressionCnt, true);
                    }

                    for (int orderCntRow = 0; orderCntRow < gridOrders.RowCount; orderCntRow++)
                    {
                        hideUnhideSummaryData(gridOrders, orderCntRow, true);
                    }

                    for (int liveDataCntRow = 0; liveDataCntRow < gridLiveData.RowCount; liveDataCntRow++)
                    {
                        hideUnhideSummaryData(gridLiveData, liveDataCntRow, true);
                    }

                    for (int liveADMDataCntRow = 0; liveADMDataCntRow < gridLiveFCMData.RowCount; liveADMDataCntRow++)
                    {
                        //hideUnhideSummaryData(gridLiveFCMData, liveADMDataCntRow, true);

                        StringBuilder keyAcct = new StringBuilder();

                        keyAcct.Append(
                            gridLiveFCMData.Rows[liveADMDataCntRow].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.POFFIC].Value.ToString());

                        keyAcct.Append(
                            gridLiveFCMData.Rows[liveADMDataCntRow].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.PACCT].Value.ToString());


                        if (optionSpreadManager.portfolioGroupFcmOfficeAcctChosenHashSet.ContainsKey(keyAcct.ToString()))
                        {
                            hideUnhideSummaryData(gridLiveFCMData, liveADMDataCntRow, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(gridLiveFCMData, liveADMDataCntRow, false);
                        }
                    }

                    for (int dataGridViewExpressionListCount = 0; dataGridViewExpressionListCount < dataGridViewExpressionList.RowCount;
                        dataGridViewExpressionListCount++)
                    {
                        hideUnhideSummaryData(dataGridViewExpressionList, dataGridViewExpressionListCount, true);
                    }
                }
                else
                {
                    for (int contractSummaryExpressionCnt = 0;
                        contractSummaryExpressionCnt < contractSummaryExpressionListIdx.Count;
                        contractSummaryExpressionCnt++)
                    {
                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionSpreadExpressionList[contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                    .instrument.idxOfInstrumentInList)
                        {
                            hideUnhideSummaryData(gridViewContractSummary, contractSummaryExpressionCnt, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(gridViewContractSummary, contractSummaryExpressionCnt, false);
                        }
                    }

                    for (int orderCntRow = 0; orderCntRow < gridOrders.RowCount; orderCntRow++)
                    {
                        int spreadId = (int)gridOrders.Rows[orderCntRow].Cells[(int)OPTION_ORDERS_COLUMNS.SPREAD_ID].Value;

                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx ==
                            optionStrategies[spreadId].indexOfInstrumentInInstrumentsArray)
                        {
                            hideUnhideSummaryData(gridOrders, orderCntRow, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(gridOrders, orderCntRow, false);
                        }

                    }

                    for (int liveDataCntRow = 0; liveDataCntRow < gridLiveData.RowCount; liveDataCntRow++)
                    {


                        int spreadId = (int)gridLiveData.Rows[liveDataCntRow].Cells[(int)OPTION_LIVE_DATA_COLUMNS.SPREAD_ID].Value;

                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx ==
                            optionStrategies[spreadId].indexOfInstrumentInInstrumentsArray)
                        {
                            hideUnhideSummaryData(gridLiveData, liveDataCntRow, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(gridLiveData, liveDataCntRow, false);
                        }
                    }


                    for (int aDMCompareCntRow = 0;
                        aDMCompareCntRow < gridLiveFCMData.RowCount; aDMCompareCntRow++)
                    {
                        int instrumentId = Convert.ToInt16(gridLiveFCMData.Rows[aDMCompareCntRow].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.INSTRUMENT_ID].Value);

                        StringBuilder keyAcct = new StringBuilder();

                        keyAcct.Append(
                            gridLiveFCMData.Rows[aDMCompareCntRow].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.POFFIC].Value.ToString());

                        keyAcct.Append(
                            gridLiveFCMData.Rows[aDMCompareCntRow].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.PACCT].Value.ToString());


                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instrumentId
                            && optionSpreadManager.portfolioGroupFcmOfficeAcctChosenHashSet.ContainsKey(keyAcct.ToString()))
                        {
                            hideUnhideSummaryData(gridLiveFCMData, aDMCompareCntRow, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(gridLiveFCMData, aDMCompareCntRow, false);
                        }
                    }

                    for (int dataGridViewExpressionListCount = 0;
                        dataGridViewExpressionListCount < dataGridViewExpressionList.RowCount; dataGridViewExpressionListCount++)
                    {
                        int instrumentId = Convert.ToInt16(dataGridViewExpressionList.Rows[dataGridViewExpressionListCount].Cells[(int)EXPRESSION_LIST_VIEW.INSTRUMENT_ID].Value);

                        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instrumentId)
                        {
                            hideUnhideSummaryData(dataGridViewExpressionList, dataGridViewExpressionListCount, true);
                        }
                        else
                        {
                            hideUnhideSummaryData(dataGridViewExpressionList, dataGridViewExpressionListCount, false);
                        }
                    }
                }

                updateInstrumentSummaryAfterInstrumentChg();

                updateOrderSummaryAfterInstrumentSelect();

                updateModelADMCompareViewable();

                //hideUnhideADMStratsWithPositions();

            }




        }

        internal void updateModelADMCompareViewable()
        {



            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
            {

                for (int modelADMCompareCntRow = 0;
                                modelADMCompareCntRow < gridViewModelADMCompare.RowCount; modelADMCompareCntRow++)
                {
                    bool accountGroupDisplay = true;

                    if (!(optionSpreadManager.acctGroupSelected == optionSpreadManager.portfolioGroupAllocation.Length))
                    {
                        int acctGrpIdx = Convert.ToInt16(gridViewModelADMCompare.Rows[modelADMCompareCntRow]
                            .Cells[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ACCT_GROUP_IDX].Value);

                        if (optionSpreadManager.acctGroupSelected == acctGrpIdx)
                        {
                            accountGroupDisplay = true;
                        }
                        else
                        {
                            accountGroupDisplay = false;
                        }
                    }

                    if (accountGroupDisplay)
                    {
                        hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, true);
                    }
                    else
                    {
                        hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, false);
                    }
                }
            }
            else
            {
                for (int modelADMCompareCntRow = 0;
                        modelADMCompareCntRow < gridViewModelADMCompare.RowCount; modelADMCompareCntRow++)
                {
                    int instrumentId = Convert.ToInt16(gridViewModelADMCompare.Rows[modelADMCompareCntRow].Cells[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.INSTRUMENT_ID].Value);

                    bool accountGroupDisplay = true;

                    if (!(optionSpreadManager.acctGroupSelected == optionSpreadManager.portfolioGroupAllocation.Length))
                    {
                        int acctGrpIdx = Convert.ToInt16(gridViewModelADMCompare.Rows[modelADMCompareCntRow]
                            .Cells[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ACCT_GROUP_IDX].Value);

                        if (optionSpreadManager.acctGroupSelected == acctGrpIdx)
                        {
                            accountGroupDisplay = true;
                        }
                        else
                        {
                            accountGroupDisplay = false;
                        }
                    }

                    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instrumentId
                        && accountGroupDisplay)
                    {
                        hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, true);
                    }
                    else
                    {
                        hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, false);
                    }

                }
            }
        }


        private void tsbtnPortfolioAndOrderPayoff_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {

                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

                List<ContractList> contractListOfRollovers = new List<ContractList>();


                foreach (var cl in
                    optionSpreadManager.instrumentRollIntoSummary[optionSpreadManager.contractSummaryInstrumentSelectedIdx]
                        .contractHashTable)
                {
                    // cl.Value.cqgSymbol\
                    //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
                    if (cl.Value.currentlyRollingContract
                        && cl.Value.numberOfContracts != 0)
                    {
                        contractListOfRollovers.Add(cl.Value);
                    }
                }

                //contractSummaryExpressionListIdx,
                //    optionSpreadExpressionList, instruments[contractSummaryInstrumentSelectedIdx],

                List<int> contractsInOrders = new List<int>();

                for (int contractSummaryCnt = 0; contractSummaryCnt < contractSummaryExpressionListIdx.Count; contractSummaryCnt++)
                {
                    contractsInOrders.Add(contractSummaryExpressionListIdx[contractSummaryCnt]);
                }

                for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
                {
                    if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol)
                        && !contractsInOrders.Contains(expressionCount))
                    {
                        contractsInOrders.Add(expressionCount);
                    }
                }



                scPrice.optionPLChartUserForm1.fillGrid(optionSpreadManager, contractsInOrders,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    0, OptionPLChartUserForm.PAYOFF_CHART_TYPE.CONTRACT_AND_ORDER_SUMMARY_PAYOFF,
                    contractListOfRollovers);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }
        }

        private void tsbtnOrderPayoffChart_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {


                List<ContractList> contractListOfRollovers = new List<ContractList>();


                foreach (var cl in
                    optionSpreadManager.instrumentRollIntoSummary[optionSpreadManager.contractSummaryInstrumentSelectedIdx]
                        .contractHashTable)
                {
                    // cl.Value.cqgSymbol\
                    //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
                    if (cl.Value.currentlyRollingContract
                        && cl.Value.numberOfContracts != 0)
                    {
                        contractListOfRollovers.Add(cl.Value);
                    }
                }


                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

                List<int> contractsInOrders = new List<int>();

                for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
                {
                    if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol))
                    {
                        contractsInOrders.Add(expressionCount);
                    }
                }

                scPrice.optionPLChartUserForm1.fillGrid(optionSpreadManager, contractsInOrders,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    0, OptionPLChartUserForm.PAYOFF_CHART_TYPE.ORDER_SUMMARY_PAYOFF, contractListOfRollovers);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }
        }

        private void tsbtnContractSummaryPayoffChart_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                double rRisk = 0;

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
                    {
                        rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                    }
                }

                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);

                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

                scPrice.optionPLChartUserForm1.fillGridFromContractSummary(
                    optionSpreadManager,
                    contractSummaryExpressionListIdx,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    rRisk, OptionPLChartUserForm.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }
        }

        public void fillInstrumentSummary(int row, int col, double value)
        {
            try
            {
                if (this.InvokeRequired)
                {

                    ThreadSafeUpdateInstrumentSummaryDelegate d = new ThreadSafeUpdateInstrumentSummaryDelegate(threadSafeUpdateInstrumentSummary);

                    this.Invoke(d, row, col, value);
                }
                else
                {
                    threadSafeUpdateInstrumentSummary(row, col, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void threadSafeUpdateInstrumentSummary(int row, int col, double val)
        {
            try
            {

                instrumentSummaryGrid.Rows[row].Cells[col].Value = val;

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void makePortfolioCSVFilesForCOREToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CMEMarginCall cmeMarginCall = new CMEMarginCall(optionSpreadExpressionList, optionStrategies,
                instruments, this, portfolioSpreadTotals, optionSpreadManager.admPositionImportWeb, true);

            cmeMarginCall.generateMarginRequest();
        }

        public void updateButtonText(ToolStripButton toolStripBtn, string buttonText)
        {

            if (this.InvokeRequired)
            {

                ThreadSafeUpdateButtonText d = new ThreadSafeUpdateButtonText(threadSafeUpdateHideUnhideButtonText);

                this.Invoke(d, buttonText);
            }
            else
            {
                threadSafeUpdateHideUnhideButtonText(toolStripBtn, buttonText);
            }

        }


        private void threadSafeUpdateHideUnhideButtonText(ToolStripButton toolStripBtn, string buttonText)
        {

            toolStripBtn.Text = buttonText;
        }

        private void toolStripBtnADMPayoff_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                double rRisk = 0;

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
                    {
                        rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                    }
                }

                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);


                scPrice.optionPLChartUserForm1.fillGridWithFCMData(
                    optionSpreadManager, null, null, optionSpreadManager.admPositionImportWeb,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    rRisk);
                //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }
        }



        private void btnIncludeExcludeOrders_Click(object sender, EventArgs e)
        {
            //exclude include orders in model adm compare

            optionSpreadManager.includeExcludeOrdersInModelADMCompare = !optionSpreadManager.includeExcludeOrdersInModelADMCompare;

            if (optionSpreadManager.includeExcludeOrdersInModelADMCompare)
            {
                updateButtonText(btnIncludeExcludeOrders, "EXCLUDE ORDERS");
            }
            else
            {
                updateButtonText(btnIncludeExcludeOrders, "INCLUDE ORDERS");
            }





            optionSpreadManager.modelADMCompareCalculationAndDisplay.fillGridModelADMComparison(this);
        }

        

        private void gridViewModelADMCompare_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE)
            {

                bool zeroPrice = Convert.ToBoolean(gridViewModelADMCompare.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                //optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].exclude = exclude;

                if (zeroPrice)
                {
                    optionSpreadManager.zeroPriceContractList.Add(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                    optionSpreadManager.exceptionContractList.Remove(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                }
                else
                {
                    optionSpreadManager.zeroPriceContractList.Remove(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                }

                optionSpreadManager.modelADMCompareCalculationAndDisplay.setBackgroundZeroPrice_ModelADMCompare(this, e.RowIndex, e.ColumnIndex);

                //optionSpreadManager.aDMDataCommonMethods.saveADMStrategyInfo(optionSpreadManager);
            }
            else if (e.ColumnIndex == (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS)
            {

                bool exception = Convert.ToBoolean(gridViewModelADMCompare.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                //optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].exclude = exclude;

                if (exception)
                {
                    optionSpreadManager.exceptionContractList.Add(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                    optionSpreadManager.zeroPriceContractList.Remove(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                }
                else
                {
                    optionSpreadManager.exceptionContractList.Remove(optionSpreadManager.admPositionImportWebListForCompare[e.RowIndex].cqgSymbol);
                }

                optionSpreadManager.modelADMCompareCalculationAndDisplay.setBackgroundZeroPrice_ModelADMCompare(this, e.RowIndex, e.ColumnIndex);


            }

            optionSpreadManager.aDMDataCommonMethods.saveADMStrategyInfo(optionSpreadManager);

            //TSErrorCatch.debugWriteOut(e.ColumnIndex + "  " + e.RowIndex + " " + sender.ToString());


        }

        private void gridViewModelADMCompare_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gridViewModelADMCompare.EndEdit();
        }

        private void dateTimePreviousPL_ValueChanged(object sender, EventArgs e)
        {
            if (optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                displayPreviousPLDataAzure();
            }
            else
            {
                displayPreviousPLData();
            }
        }

        private void displayPreviousPLDataAzure()
        {
            DateTime dateTimeQuery = dateTimePreviousPL.Value.Date;

            DataRow[][] systemOptionPlResults = optionSpreadManager.getOptionSettlePLPriceCompareAzure(dateTimeQuery, 0);

            DataRow[] systemFuturePlResults = optionSpreadManager.getFutureSettlePLPriceCompareAzure(dateTimeQuery, 0);

            DataRow[][] fcmOptionPlResults = optionSpreadManager.getOptionSettlePLPriceCompareAzure(dateTimeQuery, 1);

            DataRow[] fcmFuturePlResults = optionSpreadManager.getFutureSettlePLPriceCompareAzure(dateTimeQuery, 1);

            int plTableIdx = 0;
            int optionTableIdx = 1;

            double[] instrumentModelPL = new double[instruments.Length];
            double[] instrumentModelPLDiff = new double[instruments.Length];
            double instrumentModelTotalPL = 0;
            double instrumentModelTotalPLDiff = 0;
            double instrumentModelTotalPLNet = 0;

            double[] instrumentFCMPL = new double[instruments.Length];
            double[] instrumentFCMPLDiff = new double[instruments.Length];
            double instrumentFCMTotalPL = 0;
            double instrumentFCMTotalPLDiff = 0;
            double instrumentFCMTotalPLNet = 0;

            dataGridPreviousModelPriceCompare.RowCount =
                systemOptionPlResults[plTableIdx].Length + systemFuturePlResults.Length;

            dataGridPreviousFCMPriceCompare.RowCount =
                fcmOptionPlResults[plTableIdx].Length + fcmFuturePlResults.Length;

            Color rowColor1 = Color.DarkGray;
            Color rowColor2 = Color.DarkBlue;

            int rowCnt = 0;
            int rowCntFCM = 0;

            int instrumentCnt = 0;

            while (instrumentCnt < instruments.Length)
            {
                Color currentRowColor = rowColor1;
                switch (instrumentCnt % 2)
                {
                    case 0:
                        currentRowColor = rowColor1;
                        break;

                    case 1:
                        currentRowColor = rowColor2;
                        break;
                }

                Dictionary<int, DataRow> systemOptionPLResultsHashSet = new Dictionary<int, DataRow>();
                for (int i = 0; i < systemOptionPlResults[optionTableIdx].Length; i++)
                {
                    systemOptionPLResultsHashSet.Add(
                        Convert.ToInt32(systemOptionPlResults[optionTableIdx][i]["idoption"]),
                        systemOptionPlResults[optionTableIdx][i]);

                }

                for (int i = 0; i < systemOptionPlResults[plTableIdx].Length; i++)
                {
                    if ((int)systemOptionPlResults[plTableIdx][i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {

                        instrumentModelPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingAzure(dateTimeQuery, dataGridPreviousModelPriceCompare,
                            systemOptionPlResults[plTableIdx], systemOptionPLResultsHashSet, false,
                            rowCnt, Color.DarkGray, i, instruments[instrumentCnt]);


                        instrumentModelPL[instrumentCnt] +=
                            Convert.ToDouble(systemOptionPlResults[plTableIdx][i]["plSettleDayChg"])
                            + Convert.ToDouble(systemOptionPlResults[plTableIdx][i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(systemOptionPlResults[plTableIdx][i]["plShortSettleOrderChg"]);


                        rowCnt++;
                    }
                }

                for (int i = 0; i < systemFuturePlResults.Length; i++)
                {
                    if ((int)systemFuturePlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentModelPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingAzure(dateTimeQuery, dataGridPreviousModelPriceCompare,
                            systemFuturePlResults, null, true,
                            rowCnt, Color.DarkGray, i, instruments[instrumentCnt]);


                        instrumentModelPL[instrumentCnt] +=
                            Convert.ToDouble(systemFuturePlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(systemFuturePlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(systemFuturePlResults[i]["plShortSettleOrderChg"]);

                        rowCnt++;
                    }
                }

                //FCM

                Dictionary<int, DataRow> fcmOptionPlResultsHashSet = new Dictionary<int, DataRow>();
                for (int i = 0; i < fcmOptionPlResults[optionTableIdx].Length; i++)
                {
                    fcmOptionPlResultsHashSet.Add(
                        Convert.ToInt32(fcmOptionPlResults[optionTableIdx][i]["idoption"]),
                        fcmOptionPlResults[optionTableIdx][i]);

                }

                for (int i = 0; i < fcmOptionPlResults[plTableIdx].Length; i++)
                {
                    if ((int)fcmOptionPlResults[plTableIdx][i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentFCMPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingAzure(dateTimeQuery, dataGridPreviousFCMPriceCompare,
                            fcmOptionPlResults[plTableIdx], fcmOptionPlResultsHashSet, false,
                            rowCntFCM, Color.DarkGray, i, instruments[instrumentCnt]);


                        instrumentFCMPL[instrumentCnt] +=
                            Convert.ToDouble(fcmOptionPlResults[plTableIdx][i]["plSettleDayChg"])
                            + Convert.ToDouble(fcmOptionPlResults[plTableIdx][i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(fcmOptionPlResults[plTableIdx][i]["plShortSettleOrderChg"]);

                        rowCntFCM++;
                    }
                }

                for (int i = 0; i < fcmFuturePlResults.Length; i++)
                {
                    if ((int)fcmFuturePlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentFCMPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingAzure(dateTimeQuery, dataGridPreviousFCMPriceCompare,
                            fcmFuturePlResults, null, true,
                            rowCntFCM, Color.DarkGray, i, instruments[instrumentCnt]);


                        instrumentFCMPL[instrumentCnt] +=
                            Convert.ToDouble(fcmFuturePlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(fcmFuturePlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(fcmFuturePlResults[i]["plShortSettleOrderChg"]);

                        rowCntFCM++;
                    }
                }

                dataGridPreviousModelPL
                            .Rows[instrumentCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

                dataGridPreviousFCMPL
                            .Rows[instrumentCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

                dataGridPreviousModelPL.Rows[instrumentCnt].HeaderCell.Value =
                                instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol;

                dataGridPreviousFCMPL.Rows[instrumentCnt].HeaderCell.Value =
                                instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol;

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentModelPL[instrumentCnt], 2);

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentModelPLDiff[instrumentCnt], 2);

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentModelPL[instrumentCnt] +
                        instrumentModelPLDiff[instrumentCnt], 2);


                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentFCMPL[instrumentCnt], 2);

                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentFCMPLDiff[instrumentCnt], 2);

                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentFCMPL[instrumentCnt] +
                        instrumentFCMPLDiff[instrumentCnt], 2);

                instrumentModelTotalPL += instrumentModelPL[instrumentCnt];

                instrumentModelTotalPLDiff += instrumentModelPLDiff[instrumentCnt];

                instrumentModelTotalPLNet += instrumentModelPL[instrumentCnt] + instrumentModelPLDiff[instrumentCnt];

                instrumentFCMTotalPL += instrumentFCMPL[instrumentCnt];

                instrumentFCMTotalPLDiff += instrumentFCMPLDiff[instrumentCnt];

                instrumentFCMTotalPLNet += instrumentFCMPL[instrumentCnt] + instrumentFCMPLDiff[instrumentCnt];

                instrumentCnt++;

            }

            //dataGridPreviousModelPL
            //                .Rows[instrumentCnt]
            //                    .HeaderCell.Style.BackColor = Color.Black;

            dataGridPreviousModelPL.Rows[instrumentCnt].HeaderCell.Value = "TOTAL";

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentModelTotalPL, 2);

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentModelTotalPLDiff, 2);

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentModelTotalPLNet, 2);


            //dataGridPreviousFCMPL
            //                .Rows[instrumentCnt]
            //                    .HeaderCell.Style.BackColor = currentRowColor;

            dataGridPreviousFCMPL.Rows[instrumentCnt].HeaderCell.Value = "TOTAL";

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                Math.Round(instrumentFCMTotalPL, 2);

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                Math.Round(instrumentFCMTotalPLDiff, 2);

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                Math.Round(instrumentFCMTotalPLNet, 2);
        }

        private void displayPreviousPLData()
        {
            DateTime dateTimeQuery = dateTimePreviousPL.Value.Date;

            DataRow[] systemOptionPlResults = optionSpreadManager.getOptionSettlePLPriceCompare(dateTimeQuery, 0);

            DataRow[] systemFuturePlResults = optionSpreadManager.getFutureSettlePLPriceCompare(dateTimeQuery, 0);

            DataRow[] fcmOptionPlResults = optionSpreadManager.getOptionSettlePLPriceCompare(dateTimeQuery, 1);

            DataRow[] fcmFuturePlResults = optionSpreadManager.getFutureSettlePLPriceCompare(dateTimeQuery, 1);

            double[] instrumentModelPL = new double[instruments.Length];
            double[] instrumentModelPLDiff = new double[instruments.Length];
            double instrumentModelTotalPL = 0;
            double instrumentModelTotalPLDiff = 0;
            double instrumentModelTotalPLNet = 0;

            double[] instrumentFCMPL = new double[instruments.Length];
            double[] instrumentFCMPLDiff = new double[instruments.Length];
            double instrumentFCMTotalPL = 0;
            double instrumentFCMTotalPLDiff = 0;
            double instrumentFCMTotalPLNet = 0;

            dataGridPreviousModelPriceCompare.RowCount =
                systemOptionPlResults.Length + systemFuturePlResults.Length;

            dataGridPreviousFCMPriceCompare.RowCount =
                fcmOptionPlResults.Length + fcmFuturePlResults.Length;

            Color rowColor1 = Color.DarkGray;
            Color rowColor2 = Color.DarkBlue;

            int rowCnt = 0;
            int rowCntFCM = 0;

            int instrumentCnt = 0;

            while (instrumentCnt < instruments.Length)
            {
                Color currentRowColor = rowColor1;
                switch (instrumentCnt % 2)
                {
                    case 0:
                        currentRowColor = rowColor1;
                        break;

                    case 1:
                        currentRowColor = rowColor2;
                        break;
                }

                for (int i = 0; i < systemOptionPlResults.Length; i++)
                {
                    if ((int)systemOptionPlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {

                        instrumentModelPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingLocalDB(dateTimeQuery, dataGridPreviousModelPriceCompare, systemOptionPlResults, false,
                            rowCnt, currentRowColor, i, instruments[instrumentCnt]);


                        instrumentModelPL[instrumentCnt] +=
                            Convert.ToDouble(systemOptionPlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(systemOptionPlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(systemOptionPlResults[i]["plShortSettleOrderChg"]);


                        rowCnt++;
                    }
                }

                for (int i = 0; i < systemFuturePlResults.Length; i++)
                {
                    if ((int)systemFuturePlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentModelPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingLocalDB(dateTimeQuery, dataGridPreviousModelPriceCompare, systemFuturePlResults, true,
                            rowCnt, currentRowColor, i, instruments[instrumentCnt]);


                        instrumentModelPL[instrumentCnt] +=
                            Convert.ToDouble(systemFuturePlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(systemFuturePlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(systemFuturePlResults[i]["plShortSettleOrderChg"]);

                        rowCnt++;
                    }
                }

                //FCM

                for (int i = 0; i < fcmOptionPlResults.Length; i++)
                {
                    if ((int)fcmOptionPlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentFCMPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingLocalDB(dateTimeQuery, dataGridPreviousFCMPriceCompare, fcmOptionPlResults, false,
                            rowCntFCM, currentRowColor, i, instruments[instrumentCnt]);


                        instrumentFCMPL[instrumentCnt] +=
                            Convert.ToDouble(fcmOptionPlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(fcmOptionPlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(fcmOptionPlResults[i]["plShortSettleOrderChg"]);

                        rowCntFCM++;
                    }
                }

                for (int i = 0; i < fcmFuturePlResults.Length; i++)
                {
                    if ((int)fcmFuturePlResults[i]["idinstrument"] == instruments[instrumentCnt].idInstrument)
                    {
                        instrumentFCMPLDiff[instrumentCnt]
                            += fillDataGridPreviousPriceUsingLocalDB(dateTimeQuery, dataGridPreviousFCMPriceCompare, fcmFuturePlResults, true,
                            rowCntFCM, currentRowColor, i, instruments[instrumentCnt]);


                        instrumentFCMPL[instrumentCnt] +=
                            Convert.ToDouble(fcmFuturePlResults[i]["plSettleDayChg"])
                            + Convert.ToDouble(fcmFuturePlResults[i]["plLongSettleOrderChg"])
                            + Convert.ToDouble(fcmFuturePlResults[i]["plShortSettleOrderChg"]);

                        rowCntFCM++;
                    }
                }

                dataGridPreviousModelPL
                            .Rows[instrumentCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

                dataGridPreviousFCMPL
                            .Rows[instrumentCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

                dataGridPreviousModelPL.Rows[instrumentCnt].HeaderCell.Value =
                                instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol;

                dataGridPreviousFCMPL.Rows[instrumentCnt].HeaderCell.Value =
                                instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol;

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentModelPL[instrumentCnt], 2);

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentModelPLDiff[instrumentCnt], 2);

                dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentModelPL[instrumentCnt] +
                        instrumentModelPLDiff[instrumentCnt], 2);


                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentFCMPL[instrumentCnt], 2);

                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentFCMPLDiff[instrumentCnt], 2);

                dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentFCMPL[instrumentCnt] +
                        instrumentFCMPLDiff[instrumentCnt], 2);

                instrumentModelTotalPL += instrumentModelPL[instrumentCnt];

                instrumentModelTotalPLDiff += instrumentModelPLDiff[instrumentCnt];

                instrumentModelTotalPLNet += instrumentModelPL[instrumentCnt] + instrumentModelPLDiff[instrumentCnt];

                instrumentFCMTotalPL += instrumentFCMPL[instrumentCnt];

                instrumentFCMTotalPLDiff += instrumentFCMPLDiff[instrumentCnt];

                instrumentFCMTotalPLNet += instrumentFCMPL[instrumentCnt] + instrumentFCMPLDiff[instrumentCnt];

                instrumentCnt++;

            }

            //dataGridPreviousModelPL
            //                .Rows[instrumentCnt]
            //                    .HeaderCell.Style.BackColor = Color.Black;

            dataGridPreviousModelPL.Rows[instrumentCnt].HeaderCell.Value = "TOTAL";

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                    Math.Round(instrumentModelTotalPL, 2);

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                    Math.Round(instrumentModelTotalPLDiff, 2);

            dataGridPreviousModelPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                    Math.Round(instrumentModelTotalPLNet, 2);


            //dataGridPreviousFCMPL
            //                .Rows[instrumentCnt]
            //                    .HeaderCell.Style.BackColor = currentRowColor;

            dataGridPreviousFCMPL.Rows[instrumentCnt].HeaderCell.Value = "TOTAL";

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL].Value =
                Math.Round(instrumentFCMTotalPL, 2);

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_DIFF].Value =
                Math.Round(instrumentFCMTotalPLDiff, 2);

            dataGridPreviousFCMPL.Rows[instrumentCnt].Cells[(int)PREVIOUS_PL_COMPARE_ANALYSIS.PL_NET].Value =
                Math.Round(instrumentFCMTotalPLNet, 2);
        }

        private double fillDataGridPreviousPriceUsingAzure(DateTime modelDateTime,
            DataGridView dataGridView, DataRow[] dataRowResults,
            Dictionary<int, DataRow> systemOptionPLResultsHashSet, bool futureValue,
            int rowCnt, Color currentRowColor, int dataRowCnt, Instrument instrument)
        {
            int idoption = Convert.ToInt32(dataRowResults[dataRowCnt]["idoption"]);

            dataGridView.Rows[rowCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

            dataGridView.Rows[rowCnt].HeaderCell.Value =
                    dataRowResults[dataRowCnt]["contract"];

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.OFFICE_ACCT].Value =
                    dataRowResults[dataRowCnt]["officeAcct"];

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.EOD_SETTLE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["settle"]);

            double spanSettle = 0;

            if (futureValue)
            {
                spanSettle = Convert.ToDouble(dataRowResults[dataRowCnt]["settlement"]);
            }
            else
            {
                if (systemOptionPLResultsHashSet != null
                    && systemOptionPLResultsHashSet.ContainsKey(idoption))
                {
                    spanSettle = Convert.ToDouble(systemOptionPLResultsHashSet[idoption]["price"]);
                }
                //else
                //{
                //    TSErrorCatch.debugWriteOut(idoption + "");
                //}
            }

            double priceDiff = 0;

            if (modelDateTime.CompareTo(
                Convert.ToDateTime(dataRowResults[dataRowCnt]["expirationdate"])) <= 0)
            {

                priceDiff = spanSettle - Convert.ToDouble(dataRowResults[dataRowCnt]["settle"]);

                priceDiff = ConversionAndFormatting.roundToSmallestIncrement(
                                priceDiff,
                                instrument.tickSize,
                                instrument.optionTickSize,
                                instrument.secondaryOptionTickSize);
            }

            int totalQty = Convert.ToInt16(dataRowResults[dataRowCnt]["totalQty"]);

            int longOrders = Convert.ToInt16(dataRowResults[dataRowCnt]["longOrders"]);

            int shortOrders = Convert.ToInt16(dataRowResults[dataRowCnt]["shortOrders"]);



            double plDiff = (totalQty + longOrders - shortOrders) * priceDiff / instrument.tickSize * instrument.tickValue;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SPAN_SETTLE].Value =
                    spanSettle;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PRICE_DIFF].Value =
                    priceDiff;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PL_DIFF].Value =
                    plDiff;


            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.DATE].Value =
                    Convert.ToDateTime(dataRowResults[dataRowCnt]["date"]).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plSettleDayChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_ORDER_PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plLongSettleOrderChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_ORDER_PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plShortSettleOrderChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.TOTAL_QTY].Value = totalQty;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_ORDER_QTY].Value = longOrders;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_ORDER_QTY].Value = shortOrders;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_AVG_PRICE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["longTransAvgPrice"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_AVG_PRICE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["shortTransAvgPrice"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PREVIOUS_DAY_SETTLE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["yesterdaySettle"]);

            if (!futureValue)
            {
                dataGridView.Rows[rowCnt]
                    .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SETTLE_IMPLIED_VOL].Value =
                        Convert.ToDouble(dataRowResults[dataRowCnt]["settlementImpliedVol"]);

                if (systemOptionPLResultsHashSet != null
                    && systemOptionPLResultsHashSet.ContainsKey(idoption))
                {
                    dataGridView.Rows[rowCnt]
                        .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SPAN_IMPLIED_VOL].Value =
                            Convert.ToDouble(systemOptionPLResultsHashSet[idoption]["impliedVol"]);
                }
                //else
                //{
                //    TSErrorCatch.debugWriteOut(idoption + "");
                //}
            }

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.INSTRUMENT_ID].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["idinstrument"]);

            return plDiff;
        }


        private double fillDataGridPreviousPriceUsingLocalDB(DateTime modelDateTime,
            DataGridView dataGridView, DataRow[] dataRowResults, bool futureValue,
            int rowCnt, Color currentRowColor, int dataRowCnt, Instrument instrument)
        {
            dataGridView.Rows[rowCnt]
                                .HeaderCell.Style.BackColor = currentRowColor;

            dataGridView.Rows[rowCnt].HeaderCell.Value =
                    dataRowResults[dataRowCnt]["contract"];

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.EOD_SETTLE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["settle"]);

            double spanSettle;

            if (futureValue)
            {
                spanSettle = Convert.ToDouble(dataRowResults[dataRowCnt]["settlement"]);
            }
            else
            {
                spanSettle = Convert.ToDouble(dataRowResults[dataRowCnt]["price"]);
            }

            double priceDiff = 0;

            if (modelDateTime.CompareTo(
                Convert.ToDateTime(dataRowResults[dataRowCnt]["expirationdate"])) <= 0)
            {

                priceDiff = spanSettle - Convert.ToDouble(dataRowResults[dataRowCnt]["settle"]);

                priceDiff = ConversionAndFormatting.roundToSmallestIncrement(
                                priceDiff,
                                instrument.tickSize,
                                instrument.optionTickSize,
                                instrument.secondaryOptionTickSize);
            }

            int totalQty = Convert.ToInt16(dataRowResults[dataRowCnt]["totalQty"]);

            int longOrders = Convert.ToInt16(dataRowResults[dataRowCnt]["longOrders"]);

            int shortOrders = Convert.ToInt16(dataRowResults[dataRowCnt]["shortOrders"]);



            double plDiff = (totalQty + longOrders - shortOrders) * priceDiff / instrument.tickSize * instrument.tickValue;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SPAN_SETTLE].Value =
                    spanSettle;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PRICE_DIFF].Value =
                    priceDiff;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PL_DIFF].Value =
                    plDiff;


            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.DATE].Value =
                    Convert.ToDateTime(dataRowResults[dataRowCnt]["date"]).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plSettleDayChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_ORDER_PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plLongSettleOrderChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_ORDER_PL].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["plShortSettleOrderChg"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.TOTAL_QTY].Value = totalQty;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_ORDER_QTY].Value = longOrders;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_ORDER_QTY].Value = shortOrders;

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.LONG_AVG_PRICE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["longTransAvgPrice"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SHORT_AVG_PRICE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["shortTransAvgPrice"]);

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.PREVIOUS_DAY_SETTLE].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["yesterdaySettle"]);

            if (!futureValue)
            {
                dataGridView.Rows[rowCnt]
                    .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SETTLE_IMPLIED_VOL].Value =
                        Convert.ToDouble(dataRowResults[dataRowCnt]["settlementImpliedVol"]);

                dataGridView.Rows[rowCnt]
                    .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.SPAN_IMPLIED_VOL].Value =
                        Convert.ToDouble(dataRowResults[dataRowCnt]["impliedVol"]);
            }

            dataGridView.Rows[rowCnt]
                .Cells[(int)PREVIOUS_PRICE_COMPARE_ANALYSIS.INSTRUMENT_ID].Value =
                    Convert.ToDouble(dataRowResults[dataRowCnt]["idinstrument"]);

            return plDiff;
        }

        private void hideUnhideInstrumentsWithoutPositionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideUnhideInstrumentsInSummaryPL = !hideUnhideInstrumentsInSummaryPL;

            if (hideUnhideInstrumentsInSummaryPL)
            {
                for (int i = 0; i < instruments.Length; i++)
                {
                    portfolioSummaryGrid.Columns[i].Visible = true;
                }
            }
            else
            {
                for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                {

                    bool showInstrument = false;

                    int expressionCnt = 0;

                    while (expressionCnt < optionSpreadExpressionList.Count)
                    {
                        if (optionSpreadExpressionList[expressionCnt].optionExpressionType
                            != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                            &&
                            optionSpreadExpressionList[expressionCnt].instrument.idxOfInstrumentInList == instrumentCnt)
                        {

                            if (optionSpreadExpressionList[expressionCnt].numberOfLotsHeldForContractSummary != 0
                                || optionSpreadExpressionList[expressionCnt].numberOfOrderContracts != 0)
                            {
                                showInstrument = true;
                                break;
                            }
                        }

                        expressionCnt++;
                    }

                    if (!showInstrument)
                    {
                        int admWebPositionCounter = 0;

                        while (admWebPositionCounter < optionSpreadManager.admPositionImportWeb.Count)
                        {
                            if (optionSpreadManager.admPositionImportWeb[admWebPositionCounter]
                                .instrument.idxOfInstrumentInList == instrumentCnt)
                            {
                                showInstrument = true;
                                break;
                            }

                            admWebPositionCounter++;
                        }
                    }

                    if (showInstrument)
                    {
                        portfolioSummaryGrid.Columns[instrumentCnt].Visible = true;
                    }
                    else
                    {
                        portfolioSummaryGrid.Columns[instrumentCnt].Visible = false;
                    }
                }

            }

        }

        private void dataGridViewExpressionList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)EXPRESSION_LIST_VIEW.MANUAL_STTLE)
            {

                //bool zeroPrice = Convert.ToBoolean(gridViewModelADMCompare.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                bool x = Convert.ToBoolean(dataGridViewExpressionList
                                .Rows[e.RowIndex]
                                .Cells[e.ColumnIndex].Value);

                if (x)
                {
                    dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].ReadOnly = false;

                    if (dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].Value == null)
                    {
                        dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].Value = 0;
                    }

                }
                else
                {
                    dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].ReadOnly = true;

                    int idx = Convert.ToInt16(
                            dataGridViewExpressionList
                                .Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.EXPRESSION_IDX].Value);

                    optionSpreadExpressionList[idx].manuallyFilled = false;
                    optionSpreadExpressionList[idx].settlementFilled = false;
                    optionSpreadExpressionList[idx].settlementIsCurrentDay = false;

                    optionSpreadExpressionList[idx].setSubscriptionLevel = false;

                    optionSpreadManager.updateEODMonitorDataSettings(true);

                    //dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].Value = "###";
                }
            }
            else if (e.ColumnIndex == (int)EXPRESSION_LIST_VIEW.STTLE)
            {
                if (dataGridViewExpressionList.Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].Value != null)
                {
                    int idx = Convert.ToInt16(
                            dataGridViewExpressionList
                                .Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.EXPRESSION_IDX].Value);

                    optionSpreadExpressionList[idx].settlement =
                        Convert.ToDouble(
                            dataGridViewExpressionList
                                .Rows[e.RowIndex].Cells[(int)EXPRESSION_LIST_VIEW.STTLE].Value);

                    optionSpreadExpressionList[idx].manuallyFilled = true;
                    optionSpreadExpressionList[idx].settlementFilled = true;
                    optionSpreadExpressionList[idx].settlementIsCurrentDay = true;

                    optionSpreadExpressionList[idx].setSubscriptionLevel = false;

                    optionSpreadManager.updateEODMonitorDataSettings(true);
                }
            }
        }

        private void dataGridViewExpressionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == (int)EXPRESSION_LIST_VIEW.MANUAL_STTLE)
            {
                dataGridViewExpressionList.EndEdit();
            }
        }



        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.stageOrdersLibrary == null)
            {
                optionSpreadManager.stageOrdersLibrary = new StageOrdersToTTWPFLibrary.FixOrderStagingController();

                optionSpreadManager.fxce = optionSpreadManager.stageOrdersLibrary.initializeFixSetup(
                    optionSpreadManager.initializationParmsSaved.portfolioGroupName);

                optionSpreadManager.fxce.CallAlert += new StageOrdersToTTWPFLibrary.FixConnectionEvent.AlertEventHandler(trigger_CallAlert);

                //optionSpreadManager.stageOrdersLibrary.


            }
        }

        void trigger_CallAlert(object sender, StageOrdersToTTWPFLibrary.AlertEventArgs e)
        {
            //TSErrorCatch.debugWriteOut( e.fixConn + "");

            optionSpreadManager.fxceConnected = e.fixConn;
        }

        //***************
        delegate void ThreadSafeTTFIXConnDelegate(bool e);

        public void ttconn(bool e)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeTTFIXConnDelegate d = new ThreadSafeTTFIXConnDelegate(threadSafeTTFIXConn);

                this.Invoke(d, e);
            }
            else
            {
                //threadSafeTTFIXConn(e);
            }
        }

        private void threadSafeTTFIXConn(bool e)
        {
            //toolStripFixConnectionStatus.Text = e.uuiData;

            if (e)
            {
                toolStripFixConnectionStatus.BackColor = Color.LawnGreen;
            }
            else
            {
                toolStripFixConnectionStatus.BackColor = Color.Red;
            }
        }
        //***************

        private void tsbtnStageOrders_Click(object sender, EventArgs e)
        {

            //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length
            //    && optionSpreadManager.stageOrdersLibrary != null)
            if (optionSpreadManager.stageOrdersLibrary != null)
            {


                List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage = new List<StageOrdersToTTWPFLibrary.Model.OrderModel>();


                foreach (var cl in
                    optionSpreadManager.instrumentRollIntoSummary[optionSpreadManager.contractSummaryInstrumentSelectedIdx]
                        .contractHashTable)
                {
                    // cl.Value.cqgSymbol\
                    //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
                    if (cl.Value.currentlyRollingContract
                        && cl.Value.numberOfContracts != 0)
                    {

                        for (int acctCnt = 0; acctCnt < optionSpreadManager.portfolioGroupAllocationChosen.Count; acctCnt++)
                        {

                            StageOrdersToTTWPFLibrary.Model.OrderModel orderModel = null;

                            int cltsCount = 0;
                            bool contractAlreadyInList = false;

                            while (cltsCount < contractListToStage.Count)
                            {
                                if (contractListToStage[cltsCount].cqgSymbol.CompareTo(cl.Value.cqgSymbol) == 0
                                    && contractListToStage[cltsCount].broker_18220.CompareTo(
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].broker) == 0
                                    && contractListToStage[cltsCount].acct.CompareTo(
                                        optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]], false)) == 0
                                    )
                                {
                                    orderModel = contractListToStage[cltsCount];

                                    contractAlreadyInList = true;

                                    break;
                                }

                                cltsCount++;
                            }

                            if (!contractAlreadyInList)
                            {
                                orderModel =
                                    new StageOrdersToTTWPFLibrary.Model.OrderModel();


                                orderModel.cqgSymbol = cl.Value.cqgSymbol;
                                orderModel.optionMonthInt = cl.Value.optionMonthInt;
                                orderModel.optionYear = cl.Value.optionYear;
                                orderModel.optionStrikePrice =
                                    (decimal)ConversionAndFormatting.convertToStrikeForTT(cl.Value.strikePrice,
                                    instruments[cl.Value.indexOfInstrumentInInstrumentsArray].optionStrikeIncrement,
                                    instruments[cl.Value.indexOfInstrumentInInstrumentsArray].optionStrikeDisplayTT,
                                    instruments[cl.Value.indexOfInstrumentInInstrumentsArray].idInstrument);

                                orderModel.contractMonthint = cl.Value.contractMonthInt;
                                orderModel.contractYear = cl.Value.contractYear;

                                //orderModel.expirationDate = cl.Value.expirationDate;



                                orderModel.underlyingExchange =
                                    instruments[cl.Value.indexOfInstrumentInInstrumentsArray].tradingTechnologiesExchange;

                                orderModel.underlyingGateway =
                                    instruments[cl.Value.indexOfInstrumentInInstrumentsArray].tradingTechnologiesGateway;

                                orderModel.lotsTotal += cl.Value.numberOfContracts;

                                //orderModel.orderQty = Math.Abs(cl.Value.numberOfContracts);


                                //this is a rolled contract so don't send price with this

                                if (cl.Value.contractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.FUTURE;

                                    orderModel.underlyingExchangeSymbol =
                                        instruments[cl.Value.indexOfInstrumentInInstrumentsArray].exchangeSymbolTT;

                                    orderModel.maturityMonthYear =
                                            new DateTime(orderModel.contractYear, orderModel.contractMonthint, 1)
                                            .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);
                                }
                                else
                                {
                                    orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.OPTION;

                                    orderModel.underlyingExchangeSymbol =
                                        instruments[cl.Value.indexOfInstrumentInInstrumentsArray].optionExchangeSymbolTT;

                                    orderModel.maturityMonthYear =
                                            new DateTime(orderModel.optionYear, orderModel.optionMonthInt, 1)
                                                .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                                    if (cl.Value.contractType == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                                    {
                                        orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.CALL;
                                    }
                                    else
                                    {
                                        orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.PUT;
                                    }


                                }

                                orderModel.broker_18220 =
                                    optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].broker;

                                orderModel.acct =
                                    optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]], false);
                            }
                            else //contractAlreadyInList
                            {
                                orderModel.lotsTotal += cl.Value.numberOfContracts;
                            }

                            orderModel.orderQty = Math.Abs(orderModel.lotsTotal)
                                * optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].multiple;

                            if (orderModel.lotsTotal > 0)
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Buy;
                            }
                            else
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Sell;
                            }

                            orderModel.stagedOrderMessage = "ROLL TRIGGER ORDER";


                            string instrumentSpecificFieldKey = optionSpreadManager.getInstrumentSpecificFieldKey(orderModel);


                            if (optionSpreadManager.instrumentSpecificFIXFieldHashSet.ContainsKey(instrumentSpecificFieldKey))
                            {
                                InstrumentSpecificFIXFields instrumentSpecificFIXFields
                                    = optionSpreadManager.instrumentSpecificFIXFieldHashSet[instrumentSpecificFieldKey];

                                orderModel.TAG47_Rule80A.useTag = true;
                                orderModel.TAG47_Rule80A.tagCharValue = instrumentSpecificFIXFields.TAG_47_Rule80A;

                                orderModel.TAG204_CustomerOrFirm.useTag = true;
                                orderModel.TAG204_CustomerOrFirm.tagIntValue = instrumentSpecificFIXFields.TAG_204_CustomerOrFirm;

                                orderModel.TAG18205_TTAccountType.useTag = true;
                                orderModel.TAG18205_TTAccountType.tagStringValue = instrumentSpecificFIXFields.TAG_18205_TTAccountType;

                                orderModel.TAG440_ClearingAccount.useTag = true;
                                orderModel.TAG440_ClearingAccount.tagStringValue = instrumentSpecificFIXFields.TAG_440_ClearingAccount;

                                orderModel.TAG16102_FFT2.useTag = true;
                                orderModel.TAG16102_FFT2.tagStringValue = orderModel.acct;
                            }

                            //*****************************
                            orderModel.orderPrice = 0;


                            orderModel.orderPlacementType = cmbxOrderPlacementType.SelectedIndex;
                            //*****************************

                            contractListToStage.Add(orderModel);
                        }

                    }
                }

                for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
                {
                    //TSErrorCatch.debugWriteOut(
                    //    optionSpreadExpressionList[expressionCount].cqgSymbol + " "
                    //    +
                    //     orderSummarySelectedItems[0].SubItems[1].ToString());

                    if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol))
                    {

                        for (int acctCnt = 0; acctCnt < optionSpreadManager.portfolioGroupAllocationChosen.Count; acctCnt++)
                        {

                            StageOrdersToTTWPFLibrary.Model.OrderModel orderModel =
                                new StageOrdersToTTWPFLibrary.Model.OrderModel();

                            int cltsCount = 0;
                            bool contractAlreadyInList = false;

                            while (cltsCount < contractListToStage.Count)
                            {
                                if (contractListToStage[cltsCount].cqgSymbol.CompareTo(
                                        optionSpreadExpressionList[expressionCount].cqgSymbol) == 0
                                    && contractListToStage[cltsCount].broker_18220.CompareTo(
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].broker) == 0
                                    && contractListToStage[cltsCount].acct.CompareTo(
                                        optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]], false)) == 0
                                    )
                                {
                                    orderModel = contractListToStage[cltsCount];

                                    contractAlreadyInList = true;

                                    break;
                                }

                                cltsCount++;
                            }

                            if (!contractAlreadyInList)
                            {

                                orderModel.cqgSymbol = optionSpreadExpressionList[expressionCount].cqgSymbol;
                                orderModel.optionMonthInt = optionSpreadExpressionList[expressionCount].optionMonthInt;

                                orderModel.optionYear = optionSpreadExpressionList[expressionCount].optionYear;
                                orderModel.optionStrikePrice =
                                    (decimal)ConversionAndFormatting.convertToStrikeForTT(
                                    optionSpreadExpressionList[expressionCount].strikePrice,
                                    optionSpreadExpressionList[expressionCount].instrument.optionStrikeIncrement,
                                    optionSpreadExpressionList[expressionCount].instrument.optionStrikeDisplayTT,
                                    optionSpreadExpressionList[expressionCount].instrument.idInstrument);

                                orderModel.contractMonthint = optionSpreadExpressionList[expressionCount].futureContractMonthInt;
                                orderModel.contractYear = optionSpreadExpressionList[expressionCount].futureContractYear;

                                //orderModel.expirationDate = optionSpreadExpressionList[expressionCount].con;



                                orderModel.underlyingExchange =
                                    optionSpreadExpressionList[expressionCount].instrument.tradingTechnologiesExchange;

                                orderModel.underlyingGateway =
                                    optionSpreadExpressionList[expressionCount].instrument.tradingTechnologiesGateway;

                                orderModel.lotsTotal += optionSpreadExpressionList[expressionCount].numberOfOrderContracts;

                                //orderModel.orderQty = Math.Abs(optionSpreadExpressionList[expressionCount].numberOfOrderContracts);

                                //orderModel.orderPrice = cl.Value.;

                                if (optionSpreadExpressionList[expressionCount].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.FUTURE;

                                    orderModel.underlyingExchangeSymbol =
                                        optionSpreadExpressionList[expressionCount].instrument.exchangeSymbolTT;

                                    orderModel.maturityMonthYear =
                                        new DateTime(orderModel.contractYear, orderModel.contractMonthint, 1)
                                            .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                                }
                                else
                                {
                                    orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.OPTION;

                                    orderModel.underlyingExchangeSymbol =
                                        optionSpreadExpressionList[expressionCount].instrument.optionExchangeSymbolTT;

                                    orderModel.maturityMonthYear =
                                        new DateTime(orderModel.optionYear, orderModel.optionMonthInt, 1)
                                            .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                                    if (optionSpreadExpressionList[expressionCount].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                                    {
                                        orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.CALL;
                                    }
                                    else
                                    {
                                        orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.PUT;
                                    }

                                }

                                orderModel.broker_18220 =
                                    optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].broker;

                                orderModel.acct =
                                    optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]], false);

                            }
                            else
                            {
                                orderModel.lotsTotal += optionSpreadExpressionList[expressionCount].numberOfOrderContracts;
                            }

                            orderModel.orderQty = Math.Abs(orderModel.lotsTotal)
                                * optionSpreadManager.portfolioGroupAllocation[
                                        optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].multiple;


                            if (orderModel.lotsTotal > 0)
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Buy;
                            }
                            else
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Sell;
                            }

                            orderModel.orderPrice = findLimitPrice(
                                        orderModel.side,
                                        optionSpreadExpressionList[expressionCount]);

                            orderModel.stagedOrderMessage = "SIGNAL TRIGGER ORDER";



                            string instrumentSpecificFieldKey = optionSpreadManager.getInstrumentSpecificFieldKey(orderModel);


                            if (optionSpreadManager.instrumentSpecificFIXFieldHashSet.ContainsKey(instrumentSpecificFieldKey))
                            {
                                InstrumentSpecificFIXFields instrumentSpecificFIXFields
                                    = optionSpreadManager.instrumentSpecificFIXFieldHashSet[instrumentSpecificFieldKey];

                                orderModel.TAG47_Rule80A.useTag = true;
                                orderModel.TAG47_Rule80A.tagCharValue = instrumentSpecificFIXFields.TAG_47_Rule80A;

                                orderModel.TAG204_CustomerOrFirm.useTag = true;
                                orderModel.TAG204_CustomerOrFirm.tagIntValue = instrumentSpecificFIXFields.TAG_204_CustomerOrFirm;

                                orderModel.TAG18205_TTAccountType.useTag = true;
                                orderModel.TAG18205_TTAccountType.tagStringValue = instrumentSpecificFIXFields.TAG_18205_TTAccountType;

                                orderModel.TAG440_ClearingAccount.useTag = true;
                                orderModel.TAG440_ClearingAccount.tagStringValue = instrumentSpecificFIXFields.TAG_440_ClearingAccount;

                                orderModel.TAG16102_FFT2.useTag = true;
                                orderModel.TAG16102_FFT2.tagStringValue = orderModel.acct;
                            }


                            orderModel.orderPlacementType = cmbxOrderPlacementType.SelectedIndex;

                            contractListToStage.Add(orderModel);
                        }
                    }
                }


                if (contractListToStage.Count > 0)
                {
                    optionSpreadManager.stageOrdersLibrary.stageOrders(contractListToStage);
                }

            }
        }

        private decimal findLimitPrice(StageOrdersToTTWPFLibrary.Enums.Side side,
            OptionSpreadExpression optionSpreadExpression)
        //double price, double tickSize, double tickDisplay)
        {
            decimal priceToReturn = 0;
            double offsetPrice = 0;
            int tickOffset = Convert.ToInt16(tsbtnOrderTickOffset.Text);

            double tickSize = 0;
            double tickDisplay = 0;

            bool isOptionContract = true;
            //double tickDisplayTT_multiplier = 1;

            if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                tickSize = optionSpreadExpression.instrument.tickSize;
                tickDisplay = optionSpreadExpression.instrument.tickDisplay;

                isOptionContract = false;
                //tickDisplayTT_multiplier = optionSpreadExpression.instrument.tickDisplayTT;
            }
            //else
            //{
            //    tickSize = optionSpreadExpression.instrument.optionTickSize;
            //    tickDisplay = optionSpreadExpression.instrument.optionTickDisplay;

            //    //tickDisplayTT_multiplier = optionSpreadExpression.instrument.optionTickDisplayTT;
            //}

            if (side == StageOrdersToTTWPFLibrary.Enums.Side.Buy)
            {
                if (optionSpreadExpression.bidFilled)
                {
                    offsetPrice = optionSpreadExpression.bid;
                }
                else
                {
                    offsetPrice = optionSpreadExpression.defaultPrice;
                }

                if (isOptionContract)
                {
                    tickSize = OptionSpreadManager.chooseOptionTickSize(
                                    offsetPrice,
                                    optionSpreadExpression.instrument.optionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                    tickDisplay = OptionSpreadManager.chooseOptionTickSize(
                                offsetPrice,
                                optionSpreadExpression.instrument.optionTickDisplay,
                                optionSpreadExpression.instrument.secondaryoptiontickdisplay,
                                optionSpreadExpression.instrument.secondaryOptionTickSizeRule);
                }

                offsetPrice = offsetPrice - tickOffset * tickSize;
            }
            else
            {
                if (optionSpreadExpression.askFilled)
                {
                    offsetPrice = optionSpreadExpression.ask;
                }
                else
                {
                    offsetPrice = optionSpreadExpression.defaultPrice;
                }

                if (isOptionContract)
                {
                    tickSize = OptionSpreadManager.chooseOptionTickSize(
                                    offsetPrice,
                                    optionSpreadExpression.instrument.optionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                    tickDisplay = OptionSpreadManager.chooseOptionTickSize(
                                offsetPrice,
                                optionSpreadExpression.instrument.optionTickDisplay,
                                optionSpreadExpression.instrument.secondaryoptiontickdisplay,
                                optionSpreadExpression.instrument.secondaryOptionTickSizeRule);
                }

                offsetPrice = offsetPrice + tickOffset * tickSize;
            }

            priceToReturn =
                    Convert.ToDecimal(ConversionAndFormatting.convertToOrderPriceForTT(
                        offsetPrice,
                        tickSize,
                        tickDisplay, optionSpreadExpression.instrument.idInstrument, isOptionContract));

            if (priceToReturn < 0)
            {
                priceToReturn = 0;
            }

            return priceToReturn;

        }

        private void stageOrdersFromExpressionList_Click(object sender, EventArgs e)
        {
            sendOrderOrSecDefRequest(true);
        }

        private void sendOrderOrSecDefRequest(bool sendOrder)
        {

            if (optionSpreadManager.stageOrdersLibrary != null)
            {
                List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage = new List<StageOrdersToTTWPFLibrary.Model.OrderModel>();


                for (int i = 0; i < dataGridViewExpressionList.RowCount; i++)
                {
                    if (dataGridViewExpressionList.Rows[i].Selected)
                    {
                        int expressionCount = Convert.ToInt16(dataGridViewExpressionList.Rows[i].Cells[(int)EXPRESSION_LIST_VIEW.EXPRESSION_IDX].Value);
                        //optionSpreadExpressionList[i]

                        //if (orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol))

                        for (int acctCnt = 0; acctCnt < optionSpreadManager.portfolioGroupAllocationChosen.Count; acctCnt++)
                        {
                            //ContractList cl = new ContractList();
                            //cl.

                            //contractListOfRollovers.Add()

                            StageOrdersToTTWPFLibrary.Model.OrderModel orderModel =
                                new StageOrdersToTTWPFLibrary.Model.OrderModel();

                            orderModel.cqgSymbol = optionSpreadExpressionList[expressionCount].cqgSymbol;
                            orderModel.optionMonthInt = optionSpreadExpressionList[expressionCount].optionMonthInt;

                            double sd = Convert.ToDouble(toolStripTextBoxStrikeDisplay.Text);
                            if (sd == 0)
                            {
                                sd = optionSpreadExpressionList[expressionCount].instrument.optionStrikeDisplayTT;
                            }

                            orderModel.optionYear = optionSpreadExpressionList[expressionCount].optionYear;
                            orderModel.optionStrikePrice =
                                (decimal)ConversionAndFormatting.convertToStrikeForTT(
                                optionSpreadExpressionList[expressionCount].strikePrice,
                                optionSpreadExpressionList[expressionCount].instrument.optionStrikeIncrement,
                                sd,
                                optionSpreadExpressionList[expressionCount].instrument.idInstrument);

                            orderModel.contractMonthint = optionSpreadExpressionList[expressionCount].futureContractMonthInt;
                            orderModel.contractYear = optionSpreadExpressionList[expressionCount].futureContractYear;

                            //orderModel.expirationDate = optionSpreadExpressionList[expressionCount].con;

                            //orderModel.orderPrice = 
                            //    Convert.ToDecimal(ConversionAndFormatting.convertToTickMovesDouble(
                            //    optionSpreadExpressionList[expressionCount].defaultPrice,
                            //    optionSpreadExpressionList[expressionCount].instrument.tickSize,
                            //    optionSpreadExpressionList[expressionCount].instrument.tickDisplay));

                            orderModel.underlyingExchange =
                                optionSpreadExpressionList[expressionCount].instrument.tradingTechnologiesExchange;

                            orderModel.underlyingGateway =
                                optionSpreadExpressionList[expressionCount].instrument.tradingTechnologiesGateway;

                            int stageOrderLots = Convert.ToInt16(toolStripStageOrderContracts.Text);

                            orderModel.orderQty = Math.Abs(stageOrderLots)
                                * optionSpreadManager.portfolioGroupAllocation[
                                    optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].multiple;

                            //orderModel.orderPrice = cl.Value.;

                            if (optionSpreadExpressionList[expressionCount].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.FUTURE;

                                orderModel.underlyingExchangeSymbol =
                                    optionSpreadExpressionList[expressionCount].instrument.exchangeSymbolTT;

                                orderModel.maturityMonthYear =
                                    new DateTime(orderModel.contractYear, orderModel.contractMonthint, 1)
                                        .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                                //orderModel.orderPrice = 0;
                                //Convert.ToDecimal(ConversionAndFormatting.convertToTickMovesDouble(
                                //optionSpreadExpressionList[expressionCount].defaultPrice,
                                //optionSpreadExpressionList[expressionCount].instrument.tickSize,
                                //optionSpreadExpressionList[expressionCount].instrument.tickDisplay));
                            }
                            else
                            {
                                orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.OPTION;

                                orderModel.underlyingExchangeSymbol =
                                    optionSpreadExpressionList[expressionCount].instrument.optionExchangeSymbolTT;

                                orderModel.maturityMonthYear =
                                    new DateTime(orderModel.optionYear, orderModel.optionMonthInt, 1)
                                        .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                                if (optionSpreadExpressionList[expressionCount].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                                {
                                    orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.CALL;
                                }
                                else
                                {
                                    orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.PUT;
                                }

                                //orderModel.orderPrice = 0;
                                //Convert.ToDecimal(ConversionAndFormatting.convertToTickMovesDouble(
                                //optionSpreadExpressionList[expressionCount].defaultPrice,
                                //optionSpreadExpressionList[expressionCount].instrument.optionTickSize,
                                //optionSpreadExpressionList[expressionCount].instrument.optionTickDisplay));

                            }

                            if (stageOrderLots > 0)
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Buy;
                            }
                            else
                            {
                                orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Sell;
                            }

                            //orderModel.orderPrice = Convert.ToDecimal(toolStripTextBoxTickDisplay.Text);

                            orderModel.orderPrice = findLimitPrice(
                                    orderModel.side,
                                    optionSpreadExpressionList[expressionCount]);
                            
                            //        Convert.ToDecimal(toolStripTextBoxTickDisplay.Text);

                            orderModel.stagedOrderMessage = "SIGNAL TRIGGER ORDER";

                            //TODO: AUG 10 2015 - PortfolioGroupAllocation
                            orderModel.broker_18220 = //(String)(cmbxBroker.SelectedItem);

                                optionSpreadManager.portfolioGroupAllocation[
                                    optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].broker;

                            orderModel.acct =
                                //optionSpreadManager.portfolioGroupAllocation[
                                //    optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]].account;
                                optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol,
                                    optionSpreadManager.portfolioGroupAllocation[
                                    optionSpreadManager.portfolioGroupAllocationChosen[acctCnt]], false);
                            //(String)(cmbxAcct.SelectedItem);

                            orderModel.orderPlacementType = cmbxOrderPlacementType.SelectedIndex;


                            string instrumentSpecificFieldKey = optionSpreadManager.getInstrumentSpecificFieldKey(orderModel);


                            if (optionSpreadManager.instrumentSpecificFIXFieldHashSet.ContainsKey(instrumentSpecificFieldKey))
                            {
                                InstrumentSpecificFIXFields instrumentSpecificFIXFields
                                    = optionSpreadManager.instrumentSpecificFIXFieldHashSet[instrumentSpecificFieldKey];

                                orderModel.TAG47_Rule80A.useTag = true;
                                orderModel.TAG47_Rule80A.tagCharValue = instrumentSpecificFIXFields.TAG_47_Rule80A;

                                orderModel.TAG204_CustomerOrFirm.useTag = true;
                                orderModel.TAG204_CustomerOrFirm.tagIntValue = instrumentSpecificFIXFields.TAG_204_CustomerOrFirm;

                                orderModel.TAG18205_TTAccountType.useTag = true;
                                orderModel.TAG18205_TTAccountType.tagStringValue = instrumentSpecificFIXFields.TAG_18205_TTAccountType;

                                orderModel.TAG440_ClearingAccount.useTag = true;
                                orderModel.TAG440_ClearingAccount.tagStringValue = instrumentSpecificFIXFields.TAG_440_ClearingAccount;

                                orderModel.TAG16102_FFT2.useTag = true;
                                orderModel.TAG16102_FFT2.tagStringValue = orderModel.acct;
                            }


                            contractListToStage.Add(orderModel);
                        }
                    }
                }

                
                if (contractListToStage.Count > 0)
                {
                    if (sendOrder)
                    {
                        optionSpreadManager.stageOrdersLibrary.stageOrders(contractListToStage);
                    }
                    else
                    {
                        optionSpreadManager.stageOrdersLibrary.securityDefRequest(contractListToStage);
                    }
                }

                //DataGridViewSelectedRowCollection selectedRows = dataGridViewExpressionList.SelectedRows;


                //foreach (DataGridViewRow dataGridViewRow in selectedRows) 
                //{
                //    TSErrorCatch.debugWriteOut(dataGridViewRow.Cells[-1].Value.ToString());
                //}

                //TSErrorCatch.debugWriteOut("test");
            }
        }


        private void tsbtnFcmAndOrderPayoff_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                double rRisk = 0;

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
                    {
                        rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                    }
                }


                List<ContractList> contractListOfRollovers = new List<ContractList>();


                foreach (var cl in
                    optionSpreadManager.instrumentRollIntoSummary[optionSpreadManager.contractSummaryInstrumentSelectedIdx]
                        .contractHashTable)
                {
                    // cl.Value.cqgSymbol\
                    //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
                    if (cl.Value.currentlyRollingContract
                        && cl.Value.numberOfContracts != 0)
                    {
                        contractListOfRollovers.Add(cl.Value);
                    }
                }


                List<int> contractsInOrders = new List<int>();

                for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
                {
                    if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol))
                    {
                        contractsInOrders.Add(expressionCount);
                    }
                }

                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);


                scPrice.optionPLChartUserForm1.fillGridWithFCMData(
                    optionSpreadManager,
                    contractsInOrders, contractListOfRollovers,
                    optionSpreadManager.admPositionImportWeb,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    rRisk);
                //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }


        }

        private void tsbtnModelFCMDiff_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);


                scPrice.optionPLChartUserForm1.fillGridFromFCMModelDiff(optionSpreadManager,
                    optionSpreadManager.admPositionImportWebListForCompare,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    false);
                //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }



        }

        private void tsbtnModelFCMOrder_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                OptionPayoffChart scPrice = new OptionPayoffChart();
                scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

                scPrice.optionPLChartUserForm1.fillGridFromFCMModelDiff(
                    optionSpreadManager,
                    optionSpreadManager.admPositionImportWebListForCompare,
                    optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
                    true);
                //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                scPrice.optionPLChartUserForm1.fillChart();

                scPrice.Show();
            }
        }

        private void btnCallAllInstruments_Click(object sender, EventArgs e)
        {
            optionSpreadManager.callOptionRealTimeData(false);

        }

        private void btnCallUnsubscribed_Click(object sender, EventArgs e)
        {
            optionSpreadManager.callOptionRealTimeData(true);
        }

        private void btnResetAllInstruments_Click(object sender, EventArgs e)
        {
            optionSpreadManager.fullReConnectCQG();
        }

        private void btnCQGRecon_Click(object sender, EventArgs e)
        {
            optionSpreadManager.reInitializeCQG();
        }

        delegate void ThreadSafeUpdateCQGReconnectBtn(bool enabled);

        public void updateCQGReConnectBtn(bool enable)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeUpdateCQGReconnectBtn d = new ThreadSafeUpdateCQGReconnectBtn(threadSafeUpdateCQGReConnectBtn);

                this.Invoke(d, enable);
            }
            else
            {
                threadSafeUpdateCQGReConnectBtn(enable);
            }
        }

        private void threadSafeUpdateCQGReConnectBtn(bool enable)
        {
            btnCQGRecon.Enabled = enable;
        }

        delegate void ThreadSafeUpdateCQGConnectionStatusDelegate(String connectStatus, Color connColor,
            String connectShortStringStatus);

        public void updateCQGConnectionStatus(String connectStatus, Color connColor,
            String connectShortStringStatus)
        {
#if DEBUG
            try
#endif
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeUpdateCQGConnectionStatusDelegate d = new ThreadSafeUpdateCQGConnectionStatusDelegate(threadSafeUpdateCQGConnectionStatus);

                    this.Invoke(d, connectStatus, connColor, connectShortStringStatus);
                }
                else
                {
                    threadSafeUpdateCQGConnectionStatus(connectStatus, connColor, connectShortStringStatus);
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void threadSafeUpdateCQGConnectionStatus(String connectStatus, Color connColor,
            String connectShortStringStatus)
        {
            this.connectionStatus.Text = connectShortStringStatus;
            this.connectionStatus.ToolTipText = connectStatus;
            this.connectionStatus.ForeColor = connColor;
        }

        delegate void ThreadSafeUpdateCQGDataStatusDelegate(String dataStatus, Color backColor, Color foreColor);


        public void updateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
        {
#if DEBUG
            try
#endif
            {
                if (this.InvokeRequired)
                {
                    ThreadSafeUpdateCQGDataStatusDelegate d = new ThreadSafeUpdateCQGDataStatusDelegate(threadSafeUpdateCQGDataStatus);

                    this.Invoke(d, dataStatus, backColor, foreColor);
                }
                else
                {
                    threadSafeUpdateCQGDataStatus(dataStatus, backColor, foreColor);
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void threadSafeUpdateCQGDataStatus(String dataStatus, Color backColor, Color foreColor)
        {
            this.dataStatus.ForeColor = foreColor;
            this.dataStatus.BackColor = backColor;
            this.dataStatus.Text = dataStatus;
        }

        private void cmbxOrderPlacementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            optionSpreadManager.initializationParmsSaved.FIX_OrderPlacementType =
                cmbxOrderPlacementType.SelectedIndex;

        }

        private void treeViewBrokerAcct_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode x = treeViewBrokerAcct.SelectedNode;

            if (x != null)
            {
                optionSpreadManager.portfolioGroupAllocationChosen.Clear();

                optionSpreadManager.acctGroupSelected = x.Index;

                if (x.Index == optionSpreadManager.portfolioGroupAllocation.Length)
                {
                    //selected all portfoliogroups



                    optionSpreadManager.portfolioGroupTotalMultiple = 0;

                    for (int i = 0; i < optionSpreadManager.portfolioGroupAllocation.Length; i++)
                    {
                        optionSpreadManager.portfolioGroupAllocationChosen.Add(i);

                        optionSpreadManager.portfolioGroupTotalMultiple += optionSpreadManager.portfolioGroupAllocation[i].multiple;
                    }
                }
                else
                {
                    optionSpreadManager.portfolioGroupAllocationChosen.Add(x.Index);

                    optionSpreadManager.portfolioGroupTotalMultiple = optionSpreadManager.portfolioGroupAllocation[x.Index].multiple;
                }


                updateOrderSummaryAfterInstrumentSelect();

                sendUpdateToPortfolioTotalGrid();

                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                {
                    sendUpdateToPortfolioTotalSettlementGrid();
                }

                optionSpreadManager.gridViewContractSummaryManipulation.sendUpdateToContractSummaryLiveData(this);

                optionSpreadManager.resetPortfolioGroupFcmOfficeAcctChosenHashSet();

                updateSelectedInstrumentFromTree();
            }
        }



        private void orderSummaryGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //orderSummaryGrid_CellFormattingDelegate

            orderSummaryGrid_CellFormattingDelegate.Invoke(e, 
                orderSummaryGrid, optionSpreadManager.orderSummaryDataTable, Color.LightBlue);
            

        }

        private void rollIntoOrderSummaryGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            rollIntoOrderSummaryGrid_CellFormattingDelegate.Invoke(e, 
                rollIntoOrderSummaryGrid, optionSpreadManager.rollIntoOrderSummaryDataTable, Color.LightBlue);            
        }

        private void orderSummaryGridInactive_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            orderSummaryGridInactive_CellFormattingDelegate.Invoke(e, 
                orderSummaryGridInactive, optionSpreadManager.orderSummaryNotActiveDataTable, Color.Gray);            
        }

        private void rollIntoOrderSummaryGridInactive_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            rollIntoOrderSummaryGridInactive_CellFormattingDelegate.Invoke(e, 
                rollIntoOrderSummaryGridInactive, optionSpreadManager.rollIntoOrderSummaryNotActiveDataTable, Color.Gray);
        }

        private delegate void OrderSummaryGridFormattingDelegate(DataGridViewCellFormattingEventArgs e, DataGridView dataGrid,
            DataTable dataTable, Color backColor);

        OrderSummaryGridFormattingDelegate orderSummaryGrid_CellFormattingDelegate 
            = new OrderSummaryGridFormattingDelegate(orderSummaryGridFormatting);

        OrderSummaryGridFormattingDelegate rollIntoOrderSummaryGrid_CellFormattingDelegate
            = new OrderSummaryGridFormattingDelegate(orderSummaryGridFormatting);

        OrderSummaryGridFormattingDelegate orderSummaryGridInactive_CellFormattingDelegate
            = new OrderSummaryGridFormattingDelegate(orderSummaryGridFormatting);

        OrderSummaryGridFormattingDelegate rollIntoOrderSummaryGridInactive_CellFormattingDelegate
            = new OrderSummaryGridFormattingDelegate(orderSummaryGridFormatting);

        private static void orderSummaryGridFormatting(DataGridViewCellFormattingEventArgs e, DataGridView dataGrid,
            DataTable dataTable, Color backColor)
        {
            e.CellStyle.BackColor = backColor;

            if (dataGrid.Columns[e.ColumnIndex].Name == "Contract")
            {
                dataGrid.Columns[e.ColumnIndex].MinimumWidth = 110;
            }

            if (dataGrid.Columns[e.ColumnIndex].Name == "#")
            {
                int lots = Convert.ToInt16(dataTable.Rows[e.RowIndex][e.ColumnIndex]);

                if (lots >= 0)
                {
                    e.CellStyle.BackColor = Color.LawnGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }


            if (dataGrid.Columns[e.ColumnIndex].Name == "Decs Filled")
            {
                if (dataTable.Rows[e.RowIndex][e.ColumnIndex].ToString().Length > 0)
                {
                    bool descisionFilled = Convert.ToBoolean(dataTable.Rows[e.RowIndex][e.ColumnIndex]);

                    if (descisionFilled)
                    {
                        e.CellStyle.BackColor = Color.LawnGreen;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.Red;
                    }
                }
            }

        }

        private void fTPGMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileLoadListForm == null)
            {
                fileLoadListForm = new FileLoadList(this);
            }

            fileLoadListForm.Show();
            fileLoadListForm.BringToFront();

            Thread callFtpGmiThread = new Thread(new ParameterizedThreadStart(callFtpGmi));
            callFtpGmiThread.IsBackground = true;
            callFtpGmiThread.Start();
        }

        private void callFtpGmi(object objectin)
        {
            this.Invoke(new EventHandler(optionSpreadManager.openThread));

            FTPGetGMI ftpGetGMI = new FTPGetGMI();


            fileLoadListForm.updateLoadingFTPfilesProgressBar(40);


            List<string> downloadedFiles = ftpGetGMI.getFTPFiles();

            if (fileLoadListForm != null)
            {

                fileLoadListForm.updateLoadingFTPfilesProgressBar(50);

                for (int i = 0; i < downloadedFiles.Count; i++)
                {
                    fileLoadListForm.loadFiles(downloadedFiles[i]);
                }

                fileLoadListForm.updateLoadingFTPfilesProgressBar(100);
            }

            this.Invoke(new EventHandler(optionSpreadManager.closeThread));
        }

        private void sendSecurityDefFromExpressionList_Click(object sender, EventArgs e)
        {
            sendOrderOrSecDefRequest(false);
        }


        private void btnSOD_Click(object sender, EventArgs e)
        {
            btnSOD.Enabled = false;

            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                SystemSounds.Asterisk.Play();
            }
            else
            //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
            {
                OptionPayoffComparison sodPayoffs = new OptionPayoffComparison();

                sodPayoffs.Text = "Start Of Day Comparison";

                for (int instrumentCnt = 0; instrumentCnt < instruments.Count(); instrumentCnt++)
                {
                    OptionPayoffComparisonUserControl optionPayoffComparisonUserControl =
                        sodPayoffs.addTabForInstrument(instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol);

                    double rRisk = 0;

                    for (int i = 0; i < optionStrategies.Length; i++)
                    {
                        if (instrumentCnt == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
                        {
                            rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                        }
                    }


                    //M + O
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormModel;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);
                        //scPrice.adjustChartSplitter();

                        List<ContractList> contractListOfRollovers = new List<ContractList>();




                        //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);

                        //OptionPayoffChart scPrice = new OptionPayoffChart();
                        //scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

                        scPrice.fillGridFromContractSummary(
                            optionSpreadManager,
                            contractSummaryExpressionListIdx,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            rRisk, OptionPLChartUserForm.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                        scPrice.fillChart();

                        scPrice.Show();
                    }

                    //FCM
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {

                        //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormFCM;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);

                        //scPrice.adjustChartSplitter();

                        scPrice.fillGridWithFCMData(
                            optionSpreadManager, null, null, optionSpreadManager.admPositionImportWeb,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            rRisk);
                        //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                        scPrice.fillChart();


                    }

                    //Model + Orders vs. FCM Diff
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {
                        //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                        //scPrice.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                        //scPrice.adjustChartSplitter();

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormDifference;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);

                        //scPrice.adjustChartSplitter();

                        scPrice.fillGridFromFCMModelNetDifference(
                            optionSpreadManager,
                            optionSpreadManager.admPositionImportWebListForCompare,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            false, "Difference");
                        //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);


                        scPrice.fillChart();

                        scPrice.highlightModelFCMDifferences();


                    }

                }

                //LayoutMdi(MdiLayout.TileHorizontal);



                sodPayoffs.Show();

                //eodPayoffs.LayoutMdi(MdiLayout.TileHorizontal);

                //Thread.Sleep(2000);


                //LayoutMdi(MdiLayout.TileHorizontal);
            }

            btnSOD.Enabled = true;
        }


        private void btnEOD_Click(object sender, EventArgs e)
        {
            //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)

            btnEOD.Enabled = false;

            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
            {
                SystemSounds.Asterisk.Play();
            }
            else
            //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
            {
                OptionPayoffComparison eodPayoffs = new OptionPayoffComparison();

                eodPayoffs.Text = "End Of Day Comparison";


                for (int instrumentCnt = 0; instrumentCnt < instruments.Count(); instrumentCnt++)
                {
                    OptionPayoffComparisonUserControl optionPayoffComparisonUserControl =
                        eodPayoffs.addTabForInstrument(instruments[instrumentCnt].CQGsymbol + "-" + instruments[instrumentCnt].exchangeSymbol);


                    //M + O
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormModel;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);
                        //scPrice.adjustChartSplitter();

                        List<ContractList> contractListOfRollovers = new List<ContractList>();


                        foreach (var cl in
                            optionSpreadManager.instrumentRollIntoSummary[instrumentCnt]
                                .contractHashTable)
                        {
                            // cl.Value.cqgSymbol\
                            //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
                            if (cl.Value.currentlyRollingContract
                                && cl.Value.numberOfContracts != 0)
                            {
                                contractListOfRollovers.Add(cl.Value);
                            }
                        }

                        //contractSummaryExpressionListIdx,
                        //    optionSpreadExpressionList, instruments[contractSummaryInstrumentSelectedIdx],

                        List<int> contractsInOrders = new List<int>();

                        for (int contractSummaryCnt = 0; contractSummaryCnt < contractSummaryExpressionListIdx.Count; contractSummaryCnt++)
                        {
                            contractsInOrders.Add(contractSummaryExpressionListIdx[contractSummaryCnt]);
                        }

                        for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
                        {
                            if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol)
                                && !contractsInOrders.Contains(expressionCount))
                            {
                                contractsInOrders.Add(expressionCount);
                            }
                        }



                        scPrice.fillGrid(optionSpreadManager, contractsInOrders,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            0, OptionPLChartUserForm.PAYOFF_CHART_TYPE.CONTRACT_AND_ORDER_SUMMARY_PAYOFF,
                            contractListOfRollovers);

                        scPrice.fillChart();
                    }

                    //FCM
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {
                        double rRisk = 0;

                        for (int i = 0; i < optionStrategies.Length; i++)
                        {
                            if (instrumentCnt == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
                            {
                                rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                            }
                        }

                        //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormFCM;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);

                        //scPrice.adjustChartSplitter();

                        scPrice.fillGridWithFCMData(
                            optionSpreadManager, null, null, optionSpreadManager.admPositionImportWeb,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            rRisk);
                        //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

                        scPrice.fillChart();
                    }

                    //Model + Orders vs. FCM Diff
                    //if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
                    {
                        //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
                        //scPrice.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                        //scPrice.adjustChartSplitter();

                        OptionPLChartUserForm scPrice = optionPayoffComparisonUserControl.optionPLChartUserFormDifference;
                        scPrice.startupChart(optionArrayTypes, optionSpreadManager);

                        //scPrice.adjustChartSplitter();

                        scPrice.fillGridFromFCMModelNetDifference(
                            optionSpreadManager,
                            optionSpreadManager.admPositionImportWebListForCompare,
                            optionSpreadExpressionList, instruments[instrumentCnt],
                            true, "Difference");
                        //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);


                        scPrice.fillChart();

                        scPrice.highlightModelFCMDifferences();
                       
                    }

                }

                eodPayoffs.Show();

            }

            btnEOD.Enabled = true;
        }


        //private void btnEOD_Clickxxx(object sender, EventArgs e)
        //{
        //    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
        //    {
        //        EODPayoffs eodPayoffs = new EODPayoffs();

        //        //M + O
        //        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
        //        {

        //            //OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
        //            OptionPayoffChart scPrice = new OptionPayoffChart();
        //            scPrice.optionPLChartUserForm1.startupChart(optionArrayTypes, optionSpreadManager);

        //            scPrice.optionPLChartUserForm1.adjustChartSplitter();

        //            List<ContractList> contractListOfRollovers = new List<ContractList>();


        //            foreach (var cl in
        //                optionSpreadManager.instrumentRollIntoSummary[optionSpreadManager.contractSummaryInstrumentSelectedIdx]
        //                    .contractHashTable)
        //            {
        //                // cl.Value.cqgSymbol\
        //                //TSErrorCatch.debugWriteOut(cl.Value.cqgSymbol);
        //                if (cl.Value.currentlyRollingContract
        //                    && cl.Value.numberOfContracts != 0)
        //                {
        //                    contractListOfRollovers.Add(cl.Value);
        //                }
        //            }

        //            //contractSummaryExpressionListIdx,
        //            //    optionSpreadExpressionList, instruments[contractSummaryInstrumentSelectedIdx],

        //            List<int> contractsInOrders = new List<int>();

        //            for (int contractSummaryCnt = 0; contractSummaryCnt < contractSummaryExpressionListIdx.Count; contractSummaryCnt++)
        //            {
        //                contractsInOrders.Add(contractSummaryExpressionListIdx[contractSummaryCnt]);
        //            }

        //            for (int expressionCount = 0; expressionCount < optionSpreadExpressionList.Count; expressionCount++)
        //            {
        //                if (optionSpreadManager.orderSummaryHashTable.Contains(optionSpreadExpressionList[expressionCount].cqgSymbol)
        //                    && !contractsInOrders.Contains(expressionCount))
        //                {
        //                    contractsInOrders.Add(expressionCount);
        //                }
        //            }



        //            scPrice.fillGrid(optionSpreadManager, contractsInOrders,
        //                optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
        //                0, OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_AND_ORDER_SUMMARY_PAYOFF,
        //                contractListOfRollovers);

        //            scPrice.fillChart();

        //            scPrice.AutoScroll = true;

        //            scPrice.HorizontalScroll.Value = scPrice.HorizontalScroll.Maximum;

        //            scPrice.MdiParent = eodPayoffs;

        //            scPrice.Size = new System.Drawing.Size(eodPayoffs.Width, eodPayoffs.Height / 3);

        //            scPrice.Show();

        //            scPrice.Location = new Point(20, 10);
        //        }

        //        //FCM
        //        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
        //        {
        //            double rRisk = 0;

        //            for (int i = 0; i < optionStrategies.Length; i++)
        //            {
        //                if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == optionStrategies[i].indexOfInstrumentInInstrumentsArray)
        //                {
        //                    rRisk += optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
        //                }
        //            }

        //            OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);

        //            scPrice.adjustChartSplitter();

        //            scPrice.fillGridWithFCMData(
        //                optionSpreadManager, null, null, optionSpreadManager.admPositionImportWeb,
        //                optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
        //                rRisk);
        //            //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

        //            scPrice.fillChart();

        //            scPrice.AutoScroll = true;

        //            scPrice.HorizontalScroll.Value = scPrice.HorizontalScroll.Maximum;

        //            scPrice.MdiParent = eodPayoffs;

        //            scPrice.Size = new System.Drawing.Size(eodPayoffs.Width, eodPayoffs.Height / 3);

        //            scPrice.Show();

        //            scPrice.Location = new Point(20, 10);
        //        }

        //        //Model + Orders vs. FCM Diff
        //        if (optionSpreadManager.contractSummaryInstrumentSelectedIdx != instruments.Length)
        //        {
        //            OptionPLChart scPrice = new OptionPLChart(optionArrayTypes, optionSpreadManager);
        //            //scPrice.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        //            scPrice.adjustChartSplitter();

        //            scPrice.fillGridFromFCMModelNetDifference(
        //                optionSpreadManager,
        //                optionSpreadManager.admPositionImportWebListForCompare,
        //                optionSpreadExpressionList, instruments[optionSpreadManager.contractSummaryInstrumentSelectedIdx],
        //                true, "Difference");
        //            //OptionPLChart.PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF);

        //            scPrice.fillChart();
        //            //scPrice.AutoScaleMode = AutoScaleMode.Font;
        //            //scPrice.AutoSize = false;
        //            scPrice.AutoScroll = true;

        //            scPrice.HorizontalScroll.Value = scPrice.HorizontalScroll.Maximum;

        //            scPrice.MdiParent = eodPayoffs;

        //            //float scaleX = 100;// ((float)Screen.PrimaryScreen.WorkingArea.Width / 1024);
        //            //float scaleY = 100; // ((float)Screen.PrimaryScreen.WorkingArea.Height / 768);
        //            //SizeF aSf = new SizeF(scaleX, scaleY);

        //            //scPrice.Scale(aSf);
        //            scPrice.Size = new System.Drawing.Size(eodPayoffs.Width, eodPayoffs.Height / 3);



        //            scPrice.Show();

        //            //scPrice.Location = new Point(20, 10);
        //        }

        //        //LayoutMdi(MdiLayout.TileHorizontal);



        //        eodPayoffs.Show();

        //        eodPayoffs.LayoutMdi(MdiLayout.TileHorizontal);

        //        //Thread.Sleep(2000);


        //        //LayoutMdi(MdiLayout.TileHorizontal);
        //    }
        //}


        private void portfolioSummaryGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            portfolioSummaryGrid_CellFormattingDelegate.Invoke(e, portfolioSummaryDataTable);
        }

        private void portfolioSummaryGridSettlements_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            portfolioSummaryGridSettlements_CellFormattingDelegate.Invoke(e, portfolioSummarySettlementDataTable);
        }

        private delegate void PlTotalGridFormattingDelegate(DataGridViewCellFormattingEventArgs e,
            DataTable dataTable);

        PlTotalGridFormattingDelegate portfolioSummaryGrid_CellFormattingDelegate
            = new PlTotalGridFormattingDelegate(plTotalGridFormatting);

        PlTotalGridFormattingDelegate portfolioSummaryGridSettlements_CellFormattingDelegate
            = new PlTotalGridFormattingDelegate(plTotalGridSettlementFormatting);

        private static void plTotalGridFormatting(DataGridViewCellFormattingEventArgs e,
            DataTable dataTable)
        {

            if (dataTable.Rows.Count > 0
                && dataTable.Rows[e.RowIndex][e.ColumnIndex] != null)
            {


                string objString = (dataTable.Rows[e.RowIndex][e.ColumnIndex] as string);

                double cellValue = Convert.ToDouble(string.IsNullOrEmpty(objString) ? "0.0" : objString);

                if (cellValue >= 0)
                {
                    e.CellStyle.BackColor = Color.LawnGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }

        }


        private static void plTotalGridSettlementFormatting(DataGridViewCellFormattingEventArgs e,
            DataTable dataTable)
        {

            if (dataTable.Rows.Count > 0
                && dataTable.Rows[e.RowIndex][e.ColumnIndex] != null
                && (e.RowIndex == 0 || e.RowIndex == 2))
            {


                string objString = (dataTable.Rows[e.RowIndex][e.ColumnIndex] as string);

                double cellValue = Convert.ToDouble(string.IsNullOrEmpty(objString) ? "0.0" : objString);

                if (cellValue >= 0)
                {
                    e.CellStyle.BackColor = Color.LawnGreen;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }

        }

        private void OptionRealtimeMonitor_Load(object sender, EventArgs e)
        {

        }
    }
}
