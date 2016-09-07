using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RealtimeSpreadMonitor.Forms;
using System.Threading;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;
using RealtimeSpreadMonitor.FormManipulation;
using System.Collections.Concurrent;
using System.IO;
using System.Windows.Forms;
using RealtimeSpreadMonitor.Mongo;

namespace RealtimeSpreadMonitor
{
    /*
     *This is the main class that get data ready for display
     */

    public delegate string QueryFutureIdContractDelegate(object queryObject, int idInstrument,
        int contractYear, int contractMonthInt);    

    public class OptionSpreadManager
    {
        private System.Threading.Timer timer3SetupTimeRealtimeStateWriteToDatabase;
        private TimeSpan timeToWriteStateToDatabase = new TimeSpan(15, 00, 00);

        private TimeSpan _dataResetTime1 = new TimeSpan(7,10,0);
        public TimeSpan dataResetTime1
        {
            get { return _dataResetTime1; }
        }

        private TimeSpan _dataResetTime2 = new TimeSpan(8, 30, 0);
        public TimeSpan dataResetTime2
        {
            get { return _dataResetTime2; }
        }

        internal int threadCount;
        internal void closeThread(object o, EventArgs e)
        {
            // Decrease number of threads
            threadCount--;
        }

        internal void openThread(object o, EventArgs e)
        {
            // Decrease number of threads
            threadCount++;
        }

        private OptionArrayTypes optionArrayTypes = new OptionArrayTypes();

        private InitializationParms initializationParms;
        public InitializationParms initializationParmsSaved
        {
            get { return initializationParms; }
        }

        private OptionRealtimeStartup optionRealtimeStartup;
        private OptionStartupProgress optionStartupProgress;



        private OptionCQGDataManagement optionCQGDataManagement;

        internal int brokerAccountChosen;
        
        //Added Aug 14 2015 to deal with portfolio consolidation
        //^^^^^^^^
        /// <summary>
        /// Gets or sets the portfolio group allocation.
        /// </summary>
        /// <value>
        /// The portfolio group allocation.
        /// </value>
        internal PortfolioGroupAllocation[] portfolioGroupAllocation { get; set; }

        internal ConcurrentDictionary<string, int> portfolioGroupIdxAcctStringHashSet
            = new ConcurrentDictionary<string, int>();

        internal int acctGroupSelected;

        /// <summary>
        /// The portfolio group total multiple
        /// used for pl and order display
        /// </summary>
        internal int portfolioGroupTotalMultiple = 1;

        /// <summary>
        /// The portfolio group allocation chosen
        /// this is a list of the portfoliogroupallocation chosen from the tree
        /// </summary>
        internal List<int> portfolioGroupAllocationChosen = new List<int>();



        internal ConcurrentDictionary<string, FCM_POFFIC_PACCT> portfolioGroupFcmOfficeAcctChosenHashSet
            = new ConcurrentDictionary<string, FCM_POFFIC_PACCT>();

        internal ConcurrentDictionary<string, InstrumentSpecificFIXFields> instrumentSpecificFIXFieldHashSet
            = new ConcurrentDictionary<string, InstrumentSpecificFIXFields>();

        //^^^^^^^^


        private OptionStrategy[] optionStrategies;
        public OptionStrategy[] getOptionStrategies
        {
            get { return optionStrategies; }
        }

        internal Instrument[] instruments { get; set; }
        private Instrument_DefaultFutures[] instrument_DefaultFuturesArray;

        internal Dictionary<int, Instrument> substituteInstrumentHash = new Dictionary<int, Instrument>();


        /// <summary> The instrument spread calculate totals </summary>
        private LiveSpreadTotals[] instrumentModelCalcTotals;
        private LiveSpreadTotals[] instrumentSpreadTotals;
        private LiveSpreadTotals portfolioSpreadCalcTotals = new LiveSpreadTotals();
        private LiveSpreadTotals portfolioSpreadTotals = new LiveSpreadTotals();

        private LiveSpreadTotals[,] instrumentADMCalcTotals;
        private LiveSpreadTotals[,] instrumentADMSpreadTotals;
        private LiveSpreadTotals[] portfolioADMSpreadCalcTotals;// = new LiveSpreadTotals();
        private LiveSpreadTotals[] portfolioADMSpreadTotals;// = new LiveSpreadTotals();

        internal List<OptionSpreadExpression> optionSpreadExpressionList = new List<OptionSpreadExpression>();


        private OptionRealtimeMonitor sOptionRealtimeMonitor;  //the main realtime monitor
        public OptionRealtimeMonitor optionRealtimeMonitor
        {
            get { return sOptionRealtimeMonitor; }
        }

        private List<int> sContractSummaryExpressionListIdx = new List<int>();
        internal List<int> contractSummaryExpressionListIdx
        {
            get { return sContractSummaryExpressionListIdx; }
        }

        //private List<LiveADMStrategyInfo> sLiveADMStrategyInfoList;
        //public List<LiveADMStrategyInfo> liveADMStrategyInfoList
        //{
        //    get { return sLiveADMStrategyInfoList; }
        //}

        private List<ADMPositionImportWeb> sAdmPositionImportWebForImportDisplay;
        public List<ADMPositionImportWeb> admPositionImportWebForImportDisplay
        {
            get { return sAdmPositionImportWebForImportDisplay; }
        }

        private List<ADMPositionImportWeb> sAdmPositionImportWeb;
        public List<ADMPositionImportWeb> admPositionImportWeb
        {
            get { return sAdmPositionImportWeb; }
        }

        private DateTime dateOfADMPositionsFile;

        private ADMDataCommonMethods sADMDataCommonMethods = new ADMDataCommonMethods();
        public ADMDataCommonMethods aDMDataCommonMethods
        {
            get { return sADMDataCommonMethods; }
        }

        private AdmReportSummaryForm sAdmReportWebPositionsForm;
        public AdmReportSummaryForm admReportWebPositionsForm
        {
            get { return sAdmReportWebPositionsForm; }
        }

        //private bool usingEODSettlements;

        private RealtimeMonitorSettings sRealtimeMonitorSettings;
        public RealtimeMonitorSettings realtimeMonitorSettings
        {
            get { return sRealtimeMonitorSettings; }
        }

        private List<ADMPositionImportWeb> sAdmPositionImportWebListForCompare = new List<ADMPositionImportWeb>();
        public List<ADMPositionImportWeb> admPositionImportWebListForCompare
        {
            get { return sAdmPositionImportWebListForCompare; }
        }

        private HashSet<string> sZeroPriceContractList = new HashSet<string>();
        public HashSet<string> zeroPriceContractList
        {
            get { return sZeroPriceContractList; }
        }

        private HashSet<string> sExceptionContractList = new HashSet<string>();
        public HashSet<string> exceptionContractList
        {
            get { return sExceptionContractList; }
        }


        private StageOrdersToTTWPFLibrary.FixOrderStagingController _stageOrdersLibrary = null;
        public StageOrdersToTTWPFLibrary.FixOrderStagingController stageOrdersLibrary
        {
            get { return _stageOrdersLibrary; }
            set { _stageOrdersLibrary = value; }
        }

        private StageOrdersToTTWPFLibrary.FixConnectionEvent _fxce = null;
        public StageOrdersToTTWPFLibrary.FixConnectionEvent fxce
        {
            get { return _fxce; }
            set { _fxce = value; }
        }

        private bool _fxceConnected = false;
        public bool fxceConnected
        {
            get { return _fxceConnected; }
            set { _fxceConnected = value; }
        }

        //private Dictionary<string, string>[] sInstrumentAcctHashSet;
        //public Dictionary<string, string>[] instrumentAcctHashSet
        //{
        //    get { return sInstrumentAcctHashSet; }
        //}

        private StatusAndConnectedUpdates sStatusAndConnectedUpdates;
        internal StatusAndConnectedUpdates statusAndConnectedUpdates
        {
            get { return sStatusAndConnectedUpdates; }
        }

        private GridViewContractSummaryManipulation sGridViewContractSummaryManipulation;
        internal GridViewContractSummaryManipulation gridViewContractSummaryManipulation
        {
            get { return sGridViewContractSummaryManipulation; }
        }

        private ModelADMCompareCalculationAndDisplay sModelADMCompareCalculationAndDisplay;
        internal ModelADMCompareCalculationAndDisplay modelADMCompareCalculationAndDisplay
        {
            get { return sModelADMCompareCalculationAndDisplay; }
        }

        public OptionSpreadManager(InitializationParms initializationParms,
            OptionRealtimeStartup optionRealtimeStartup,
            OptionStartupProgress optionStartupProgress)
        {
            this.initializationParms = initializationParms;
            this.optionRealtimeStartup = optionRealtimeStartup;
            this.optionStartupProgress = optionStartupProgress;

            sRealtimeMonitorSettings = new RealtimeMonitorSettings();
        }


        //private Thread calcFutEquivOfSyntheticCloseThread;
        //private bool calcFutEquivOfSyntheticCloseThreadShouldStop = false;
        //private const int FUT_EQUIV_SYN_CLOSE_LOOP_DELAY = 30000;  //60000;


        private string sConnectionStatusString;
        internal string ConnectionStatusString
        {
            get { return sConnectionStatusString; }
            set { sConnectionStatusString = value; }
        }

        private string sConnectionStatusShortString;
        internal string ConnectionStatusShortString
        {
            get { return sConnectionStatusShortString; }
            set { sConnectionStatusShortString = value; }
        }

        private Color sConnectionStatusColor;
        internal Color ConnectionStatusColor
        {
            get { return sConnectionStatusColor; }
            set { sConnectionStatusColor = value; }
        }

        internal int contractSummaryInstrumentSelectedIdx {get; set;}


        //***********************************

        internal InstrumentTransactionSummary[] instrumentRollIntoSummary;

        public List<string> orderSummaryHashTable = new List<string>();
        internal DataTable orderSummaryDataTable = new DataTable();
        public List<string> orderSummaryNotActiveHashTable = new List<string>();
        internal DataTable orderSummaryNotActiveDataTable = new DataTable();


        public List<string> orderRollIntoSummaryHashTable = new List<string>();
        public List<bool> orderRollIntoSummaryHashTableDisplayed = new List<bool>();
        internal DataTable rollIntoOrderSummaryDataTable = new DataTable();

        public List<string> orderRollIntoSummaryHashTableNotActive = new List<string>();
        public List<bool> orderRollIntoSummaryHashTableDisplayedNotActive = new List<bool>();
        internal DataTable rollIntoOrderSummaryNotActiveDataTable = new DataTable();

        //***********************************

        internal bool includeExcludeOrdersInModelADMCompare = true;


        /// <summary>
        /// The option data set hash set
        /// used when calling data from database for the options data
        /// </summary>
        private Dictionary<int, DataSet> optionDataSetHashSet = new Dictionary<int, DataSet>();

        private Dictionary<string, int> optionIdFromInfo = new Dictionary<string, int>();

        /// <summary>
        /// The future data set hash set
        /// used when calling data from database for the options data
        /// </summary>
        private Dictionary<int, DataSet> futureDataSetHashSet = new Dictionary<int, DataSet>();

        private Dictionary<string, int> futureIdFromInfo = new Dictionary<string, int>();


        private bool _supplementContractFilled = false;
        internal bool supplementContractFilled
        {
            get { return _supplementContractFilled; }
        }

        public void initializeOptionSystem(Object obj)
        {
            optionStartupProgress.updateProgressBar(0.75);
            optionStartupProgress.updateProgressBar(1);


            //look up strategies and instruments associated with strategies
            //
            if (initializationParms.useCloudDb)
            {
                fillStrategiesAndInstrumentsFromAzure();
            }
            else
            {
                fillStrategiesAndInstruments();
            }

            readADMExludeContractFile();

            fillADMStrategyObjectsFromSavedFiles();


            //fillInstrumentToAccount();


            createCQGconnection();

            optionRealtimeStartup.finishedInitializing(true);

            //createCalcFutEquivOfSyntheticClose();

            //startOptionSpreadGUIs();

            
        }

        
        private void fillMongoObjects()
        {
            List<MongoDB_OptionSpreadExpression> MongoDB_OptionSpreadExpressionList =
                new List<MongoDB_OptionSpreadExpression>();

            for(int i = 0; i < optionSpreadExpressionList.Count; i++)
            {

            }
        }
        

        private void createCQGconnection()
        {
#if DEBUG
            try
#endif
            {
                optionCQGDataManagement = new OptionCQGDataManagement(this,
                    optionRealtimeStartup, optionSpreadExpressionList);
                //optionRealtimeStartup, dataErrorCheck, optionSpreadExpressionList,
                //currentDateContractListMainIdx, optionBuildCommonMethods);
                optionCQGDataManagement.initializeCQGAndCallbacks();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        //******
        /// <summary>
        /// Sets up timer for writing realtime state data to database.
        /// </summary>
        /// <param name="alertTime">The alert time.</param>
        /// <param name="timerToUse">The timer to use.</param>
        private void setUpTimerForWritingRealtimeStateDataToDatabase(TimeSpan alertTime)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }


            System.Threading.TimerCallback timerDelegate = writeRealtimeStateToDatabaseInvoke;
            //new System.Threading.TimerCallback(resetConnectionToCQGAtTimeInvoke);

            System.Threading.Timer timer = new System.Threading.Timer(
                timerDelegate, null, timeToGo, Timeout.InfiniteTimeSpan);

            timer3SetupTimeRealtimeStateWriteToDatabase = timer;


        }

        /// <summary>
        /// Writes the realtime state to database invoke.
        /// </summary>
        /// <param name="StateObj">The state object.</param>
        private void writeRealtimeStateToDatabaseInvoke(object StateObj)
        {
            //TSErrorCatch.debugWriteOut("test************************");

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new MethodInvoker(resetConnectionToCQGAtTime));
            //}
            //else
            //{
            //    resetConnectionToCQGAtTime();
            //}

            DateTime returnDateTime;

            if (initializationParmsSaved.useCloudDb)
            {
                returnDateTime = writeRealtimeStateToDatabaseAzure();
            }
            else
            {
                returnDateTime = writeRealtimeStateToDatabase();
            }

            if (optionRealtimeMonitor.settings == null)
            {
                optionRealtimeMonitor.settings = new Settings(this);
            }

            optionRealtimeMonitor.settings.updateWriteDate(returnDateTime);
        }

        ///// <summary>
        ///// Writes the realtime state to database.
        ///// </summary>
        //private void writeRealtimeStateToDatabase()
        //{
        //    DateTime returnDateTime;

        //    if (initializationParmsSaved.useCloudDb)
        //    {
        //        returnDateTime = writeRealtimeStateToDatabaseAzure();
        //    }
        //    else
        //    {
        //        returnDateTime = writeRealtimeStateToDatabase();
        //    }
        //}

        //*****************

        /// <summary>
        /// Starts the option spread gu is.
        /// </summary>
        public void startOptionSpreadGUIs()
        {
            sModelADMCompareCalculationAndDisplay
                = new ModelADMCompareCalculationAndDisplay(this);

            //need to run initializing of OptionRealtimeMonitor
            sOptionRealtimeMonitor = new OptionRealtimeMonitor(this, optionStrategies, instruments,
                instrumentSpreadTotals, portfolioSpreadTotals,
                instrumentADMSpreadTotals, portfolioADMSpreadTotals,
                optionArrayTypes, contractSummaryExpressionListIdx,
                admPositionImportWebListForCompare);


            sStatusAndConnectedUpdates 
                = new StatusAndConnectedUpdates(sOptionRealtimeMonitor, this);

            sGridViewContractSummaryManipulation
                = new GridViewContractSummaryManipulation(this, statusAndConnectedUpdates);

            

            //set the initially selected instrument to all instruments
            contractSummaryInstrumentSelectedIdx = instruments.Length;


            //should be done after initializing OptionRealtimeMonitor b/c will know rows
            //fillExpressionOptionList();

            //fillADMExpressionOptionList();

            fillAllExpressionsForCQGData();



            sOptionRealtimeMonitor.realtimeMonitorStartupBackgroundUpdateLoop();



            sOptionRealtimeMonitor.setupTreeViewInstruments();

            sOptionRealtimeMonitor.setupTreeViewBrokerAcct();

            gridViewContractSummaryManipulation.setupContractSummaryLiveData(sOptionRealtimeMonitor);
            //sOptionRealtimeMonitor.setupContractSummaryLiveData();

            sModelADMCompareCalculationAndDisplay.fillGridModelADMComparison(sOptionRealtimeMonitor);



            sOptionRealtimeMonitor.Show();

            optionRealtimeStartup.BringToFront();


            setUpTimerForWritingRealtimeStateDataToDatabase(timeToWriteStateToDatabase);
        }

        private void portfolioGroupAllocationAndInitialize()//PortfolioGroupAllocation portfolioGroupAllocation)
        {

            acctGroupSelected = portfolioGroupAllocation.Length;

            //initialize portfolio group total multiple
            portfolioGroupTotalMultiple = 0;

            for (int i = 0; i < portfolioGroupAllocation.Length; i++)
            {
                 portfolioGroupTotalMultiple += portfolioGroupAllocation[i].multiple;
            }

            for (int portfolioGroupAllocationCount = 0;
                    portfolioGroupAllocationCount < portfolioGroupAllocation.Length;
                    portfolioGroupAllocationCount++)
            {
                //fillInstrumentToAccount(portfolioGroupAllocation[portfolioGroupAllocationCount]);

                if (portfolioGroupAllocation[portfolioGroupAllocationCount].cfgfile != null &&
                    portfolioGroupAllocation[portfolioGroupAllocationCount].cfgfile.CompareTo("") != 0)
                {
                    SaveOutputFile sof = new SaveOutputFile();
                    portfolioGroupAllocation[portfolioGroupAllocationCount].instrumentAcctHashSet =
                        sof.readInstrumentAccount(portfolioGroupAllocation[portfolioGroupAllocationCount].cfgfile);

                    portfolioGroupAllocation[portfolioGroupAllocationCount].useConfigFile = true;
                }


                //for (int fcmOfficAcctCnt = 0; fcmOfficAcctCnt <
                //    portfolioGroupAllocation[portfolioGroupAllocationCount].FCM_POFFIC_PACCT_List.Count;
                //    fcmOfficAcctCnt++)

                foreach (var fcmOfficAcctKeyValPair in portfolioGroupAllocation[portfolioGroupAllocationCount].FCM_POFFIC_PACCT_hashset)
                {
                    FCM_POFFIC_PACCT fcmOfficAcct = fcmOfficAcctKeyValPair.Value;

                    StringBuilder key = new StringBuilder();
                    key.Append(fcmOfficAcct.FCM_POFFIC);
                    key.Append(fcmOfficAcct.FCM_PACCT);

                    portfolioGroupFcmOfficeAcctChosenHashSet.TryAdd(key.ToString(),
                        fcmOfficAcct);


                    portfolioGroupIdxAcctStringHashSet.TryAdd(key.ToString(), portfolioGroupAllocationCount);
                }



            }

            
        }

        private void setupInstrumentSpecificFIXFields()
        {
            SaveOutputFile sof = new SaveOutputFile();

            String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                    TradingSystemConstants.REALTIME_CONFIGURATION);

            //Dictionary<string, InstrumentSpecificFIXFields> instrumentSpecificFIXFieldHashSet =
            sof.readInstrumentSpecificFIXField(fullFile, instrumentSpecificFIXFieldHashSet);
        }

        private List<OptionStrategy> getSupplementalContracts(OptionArrayTypes optionArrayTypes,
            QueryFutureIdContractDelegate queryFutureIdContractDelegate, object queryObject)
        {
            SaveOutputFile sof = new SaveOutputFile();

            String fullFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                    TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY,
                    TradingSystemConstants.SUPPLEMENT_CONTRACTS);

            //Dictionary<string, InstrumentSpecificFIXFields> instrumentSpecificFIXFieldHashSet =
            return sof.readSupplementContracts(fullFile, optionArrayTypes, initializationParms.idPortfolioGroup,
                queryFutureIdContractDelegate, queryObject);

        }

        internal string getInstrumentSpecificFieldKey(StageOrdersToTTWPFLibrary.Model.OrderModel orderModel)
        {
            StringBuilder key = new StringBuilder();
            key.Append(orderModel.broker_18220);
            key.Append(orderModel.underlyingGateway);
            key.Append(orderModel.underlyingExchange);
            key.Append(orderModel.underlyingExchangeSymbol);

            return key.ToString();
        }

        internal void resetPortfolioGroupFcmOfficeAcctChosenHashSet()
        {
            portfolioGroupFcmOfficeAcctChosenHashSet.Clear();

            for (int portfolioGroupAllocationChosenCount = 0;
                    portfolioGroupAllocationChosenCount < portfolioGroupAllocationChosen.Count;
                    portfolioGroupAllocationChosenCount++)
            {

                int idxOfPortfolioGroup = portfolioGroupAllocationChosen[portfolioGroupAllocationChosenCount];

                foreach (var fcmOfficAcctKeyValPair in portfolioGroupAllocation[idxOfPortfolioGroup].FCM_POFFIC_PACCT_hashset)
                {
                    FCM_POFFIC_PACCT fcmOfficAcct = fcmOfficAcctKeyValPair.Value;

                    StringBuilder key = new StringBuilder();
                    key.Append(fcmOfficAcct.FCM_POFFIC);
                    key.Append(fcmOfficAcct.FCM_PACCT);

                    portfolioGroupFcmOfficeAcctChosenHashSet.TryAdd(key.ToString(),
                        fcmOfficAcct);
                }
            }
        }

            

        public void fillStrategiesAndInstruments()
        {

#if DEBUG
            try
#endif
            {
                TMLModelDBQueries btdb = new TMLModelDBQueries();

                btdb.connectDB(initializationParms.dbServerName);

                OptionStrategy[] optionStrategiesTemp = btdb.queryStrategies(initializationParms.idPortfolioGroup,// DateTime.Now);
                    initializationParms.modelDateTime.Date);

                QueryFutureIdContractDelegate queryFutureIdContractDelegate =
                    new QueryFutureIdContractDelegate(callLocalDBForFutureIdContract);

                List<OptionStrategy> optionStrategyList = getSupplementalContracts(optionArrayTypes,
                    queryFutureIdContractDelegate, btdb);

                if (optionStrategiesTemp != null && optionStrategyList.Count > 0)
                {
                    optionStrategies = new OptionStrategy[optionStrategiesTemp.Length + optionStrategyList.Count];

                    int strategyCnt = 0;
                    for (int i = 0; i < optionStrategiesTemp.Length; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategiesTemp[i];

                        strategyCnt++;
                    }

                    for (int i = 0; i < optionStrategyList.Count; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategyList[i];

                        strategyCnt++;
                    }

                    _supplementContractFilled = true;
                }
                else if (optionStrategiesTemp != null)
                {
                    optionStrategies = optionStrategiesTemp;
                }
                else if (optionStrategyList.Count > 0)
                {
                    optionStrategies = new OptionStrategy[optionStrategyList.Count];

                    int strategyCnt = 0;

                    for (int i = 0; i < optionStrategyList.Count; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategyList[i];

                        strategyCnt++;
                    }

                    _supplementContractFilled = true;
                }
                else
                {
                    optionStrategies = null;
                }

                


                portfolioGroupAllocation =
                        btdb.queryPortfolioGroupAllocation(initializationParms.idPortfolioGroup);

                

                //fcmOfficeAcctHashSet
                //    = new ConcurrentDictionary<string, FCM_POFFIC_PACCT>();

                //TODO AUG 26 2015
                //fill CONFIG 
                portfolioGroupAllocationAndInitialize();

                resetPortfolioGroupFcmOfficeAcctChosenHashSet();

                //call INSTRUMENT_SPECIFIC_FIX_FIELDS.cfg
                setupInstrumentSpecificFIXFields();
                


                if (optionStrategies == null)
                {
                    optionStrategies = new OptionStrategy[1];
                    optionStrategies[0] = new OptionStrategy();
                    optionStrategies[0].instrument = new Instrument();

                }

                instruments = btdb.queryInstruments(initializationParms.idPortfolioGroup);

                substituteInstrumentHash = btdb.fillSubstituteInstrumentHash(instruments);

                for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                {
                    instruments[instrumentCnt].settlementDateTimeMarker
                                        = initializationParmsSaved.modelDateTime.Date
                                        .AddHours(
                                            instruments[instrumentCnt].settlementTime.Hour)
                                        .AddMinutes(
                                            instruments[instrumentCnt].settlementTime.Minute
                                            + 15);
                }

                if (initializationParms.useHalfday)
                {

                    for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                    {
                        instruments[instrumentCnt].customDayBoundaryTime = initializationParms.halfDayTransactionTime;
                        instruments[instrumentCnt].decisionOffsetMinutes = initializationParms.halfDayDecisionOffsetMinutes;
                    }

                }


                instrument_DefaultFuturesArray = new Instrument_DefaultFutures[instruments.Length];

                for (int i = 0; i < instrument_DefaultFuturesArray.Length; i++)
                {
                    instrument_DefaultFuturesArray[i] = new Instrument_DefaultFutures();

                    instrument_DefaultFuturesArray[i].idInstrument = instruments[i].idInstrument;

                    instrument_DefaultFuturesArray[i].instrument = instruments[i];

                    instrument_DefaultFuturesArray[i].defaultFutures =
                        btdb.queryInstrumentDefaultFutureInfo(
                            initializationParms.modelDateTime.Date,
                                instruments[i].idInstrument,
                                    optionArrayTypes);

                    instrument_DefaultFuturesArray[i].defaultFuturesData =
                        new LegData[instrument_DefaultFuturesArray[i].defaultFutures.Length];

                    for (int futureCnt = 0; futureCnt < instrument_DefaultFuturesArray[i].defaultFutures.Length; futureCnt++)
                    {
                        instrument_DefaultFuturesArray[i].defaultFuturesData[futureCnt] = new LegData();

                        btdb.queryFuturesData(initializationParms.modelDateTime.Date,
                           instrument_DefaultFuturesArray[i].defaultFutures[futureCnt].idContract, optionArrayTypes,
                               instrument_DefaultFuturesArray[i].defaultFuturesData[futureCnt]);

                    }

                }



                //instruments = btdb.queryInstrumentsThatPossiblyHaveData();

                instrumentModelCalcTotals = new LiveSpreadTotals[instruments.Length];
                instrumentSpreadTotals = new LiveSpreadTotals[instruments.Length];

                //the total calcs includes an extra increment in array size to include sum of all portfoliogroups
                instrumentADMCalcTotals = new LiveSpreadTotals[instruments.Length, portfolioGroupAllocation.Length + 1];
                instrumentADMSpreadTotals = new LiveSpreadTotals[instruments.Length, portfolioGroupAllocation.Length + 1];

                

                for (int i = 0; i < instruments.Length; i++)
                {
                    instrumentModelCalcTotals[i] = new LiveSpreadTotals();
                    instrumentSpreadTotals[i] = new LiveSpreadTotals();

                    instruments[i].isSpread = true;

                    for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
                    {
                        instrumentADMCalcTotals[i, portfolioGroupCnt] = new LiveSpreadTotals();

                        instrumentADMSpreadTotals[i, portfolioGroupCnt] = new LiveSpreadTotals();
                    }
                }


                //the total calcs includes an extra increment in array size to include sum of all portfoliogroups
                portfolioADMSpreadCalcTotals = new LiveSpreadTotals[portfolioGroupAllocation.Length + 1];
                portfolioADMSpreadTotals = new LiveSpreadTotals[portfolioGroupAllocation.Length + 1];

                for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {
                    portfolioADMSpreadCalcTotals[portfolioGroupCnt] = new LiveSpreadTotals();
                    portfolioADMSpreadTotals[portfolioGroupCnt] = new LiveSpreadTotals();
                }

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    int instrumentCount = 0;

                    while (instrumentCount < instruments.Length)
                    {
                        if (instruments[instrumentCount].idInstrument == optionStrategies[i].idInstrument)
                        {
                            optionStrategies[i].indexOfInstrumentInInstrumentsArray = instrumentCount;

                            optionStrategies[i].instrument = instruments[instrumentCount];
                            break;
                        }

                        instrumentCount++;
                    }

                    if (optionStrategies[i].instrument == null)
                    {
                        String caption = "INSTRUMENT LIST ERROR";
                        String message = "Can't associate strategy to an instrument idStrategy:" +
                            optionStrategies[i].idStrategy +
                            ", idInstrument:" + optionStrategies[i].idInstrument;
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        System.Windows.Forms.DialogResult result;

                        // Displays the MessageBox.
                        result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                    }

                }

                //btdb.queryDataForCalendar(instruments);

                //int idxOfStrategiesThatHoldPositions = 0;

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    optionStrategies[i].dateTime = initializationParms.modelDateTime.Date;  //DateTime.Now.Date;

                    

                    if (!optionStrategies[i].supplementContract)
                    {
                        optionStrategies[i].optionStrategyParameters =
                            new OptionStrategyParameter[optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0)];

                        btdb.queryStrategyState(optionStrategies[i], optionArrayTypes);
                    }

                    parseStrategyState(optionStrategies[i]);

                    checkIfStrategyHasPosition(optionStrategies[i]);

                    //if(optionStrategies[i].holdsCurrentPosition)
                    //{
                    //    optionStrategies[i].idxOfStratsHoldingPositions = idxOfStrategiesThatHoldPositions;

                    //    idxOfStrategiesThatHoldPositions++;
                    //}

                    checkStrategyAction_FillSignalType_FillRiskType(optionStrategies[i]);

                    if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null)
                    {
                        int numberOfLegs = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                        optionStrategies[i].legInfo = new LegInfo[numberOfLegs];

                        optionStrategies[i].legData = new LegData[numberOfLegs];



                        for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {
                            optionStrategies[i].legInfo[legCounter] = new LegInfo();

                            optionStrategies[i].legData[legCounter] = new LegData();


                            switch ((int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter])
                            {
                                case (int)OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;
                                    break;
                            }



                            if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter] != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                btdb.queryOptionInfo(optionStrategies[i], legCounter, optionArrayTypes);



                                btdb.queryOptionExpirations(optionStrategies[i], optionStrategies[i].legInfo[legCounter]);

                                btdb.queryOptionData(optionStrategies[i], legCounter, optionArrayTypes);
                            }
                            else
                            {
                                optionStrategies[i].idxOfFutureLeg = legCounter;

                                btdb.queryFutureInfo(//optionStrategies[i],
                                    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                        .stateValueParsed[legCounter],
                                        optionArrayTypes,
                                        optionStrategies[i].legInfo[legCounter]);

                                optionStrategies[i].legInfo[legCounter].optionExpirationTime = new DateTime(
                                    1900, 1, 1, 0, 0, 0);

                                btdb.queryFuturesData(optionStrategies[i].dateTime,
                                    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                        .stateValueParsed[legCounter],
                                    optionArrayTypes,
                                    optionStrategies[i].legData[legCounter]);
                            }

                            //if (optionStrategies[i].instrument == null)
                            //{
                            //    TSErrorCatch.debugWriteOut(i);
                            //}

                            if (optionStrategies[i].instrument.substituteSymbolEOD)
                            {
                                //sets up data for realtime EOD CQG symbol

                                optionStrategies[i].legInfo[legCounter].useSubstitueSymbolEOD = true;

                                optionStrategies[i].legInfo[legCounter].instrumentSymbolPreEOD =
                                    optionStrategies[i].instrument.instrumentSymbolPreEOD;

                                optionStrategies[i].legInfo[legCounter].instrumentSymbolEODSubstitute =
                                    optionStrategies[i].instrument.instrumentSymbolEOD;

                                generateCQGSymbolForEODSubstitution(
                                    optionStrategies[i].legInfo[legCounter], optionStrategies[i].instrument);

                                //optionStrategies[i].legInfo[legCounter].cqgSubstituteSymbol =
                                //    optionStrategies[i].legInfo[legCounter].cqgSymbol.Replace(
                                //        optionStrategies[i].legInfo[legCounter].instrumentSymbolPreEOD,
                                //        optionStrategies[i].legInfo[legCounter].instrumentSymbolEODSubstitute);
                            }
                        }
                    }

                    //NEED TO GET OPTION DATA FOR ROLL OVER AND SET UP STRIKE SELECTION
                    if (optionStrategies[i].rollIntoLegInfo != null)
                    {
                        Type tblOptionMonthTypes = typeof(OPTION_MONTHS);
                        Array tblOptionMonthTypesArray = Enum.GetNames(tblOptionMonthTypes);

                        optionStrategies[i].rollIntoLegData = new LegData[optionStrategies[i].rollIntoLegInfo.Length];

                        for (int legCounter = 0; legCounter < optionStrategies[i].rollIntoLegInfo.Length; legCounter++)
                        {
                            //optionStrategies[i].legInfo[legCounter] = new LegInfo();

                            optionStrategies[i].rollIntoLegData[legCounter] = new LegData();

                            if (optionStrategies[i].instrument.substituteSymbolEOD)
                            {
                                //sets up data for realtime EOD SPAN symbol

                                optionStrategies[i].rollIntoLegInfo[legCounter].useSubstitueSymbolEOD = true;

                                optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD =
                                    optionStrategies[i].instrument.instrumentSymbolPreEOD;

                                optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute =
                                    optionStrategies[i].instrument.instrumentSymbolEOD;
                            }

                            switch ((int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter])
                            {
                                case (int)OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;
                                    break;
                            }

                            if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter] != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                //btdb.queryOptionInfo(optionStrategies[i], legCounter, optionArrayTypes);

                                optionStrategies[i].rollIntoLegInfo[legCounter].optionCallOrPut =
                                    optionStrategies[i].legInfo[legCounter].optionCallOrPut;

                                //optionStrategies[i].rollIntoLegInfo[legCounter].optionMonth =
                                //    Convert.ToChar(
                                //    tblOptionMonthTypesArray.GetValue(optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt - 1));

                                btdb.queryOptionInfoForRollingOptionPreSymbol(
                                    optionStrategies[i], legCounter, optionArrayTypes);

                                String strikeString =
                                    ConversionAndFormatting.convertToTickMovesString(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePrice,
                                                        optionStrategies[i].instrument.optionStrikeIncrement,
                                                        optionStrategies[i].instrument.optionStrikeDisplay);

                                int w = optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol
                                    .IndexOf(strikeString);

                                //String x = optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Substring(
                                //   0, w);

                                //TSErrorCatch.debugWriteOut(x);

                                optionStrategies[i].rollIntoLegInfo[legCounter].
                                    cqgSymbolWithoutStrike_ForRollover =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Substring(
                                            0, w);


                                if (optionStrategies[i].instrument.substituteSymbolEOD)
                                {
                                    //sets up data for realtime EOD SPAN symbol

                                    optionStrategies[i].rollIntoLegInfo[legCounter].cqgSubstituteSymbolWithoutStrike_ForRollover =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover.Replace(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD,
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);

                                    //generateOptionCQGSymbolForEODSubstitution(
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].optionCallOrPut,
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute,
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].contractMonth,
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].optionYear,
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePrice,
                                    //    optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);
                                }

                            }
                            else
                            {
                                //this is the future leg

                                optionStrategies[i].idxOfFutureLeg = legCounter;

                                btdb.queryFutureInfo(//optionStrategies[i],
                                    optionStrategies[i].rollIntoLegInfo[legCounter].idContract,
                                    optionArrayTypes,
                                    optionStrategies[i].rollIntoLegInfo[legCounter]);

                                optionStrategies[i].rollIntoLegInfo[legCounter].optionExpirationTime = new DateTime(
                                    1900, 1, 1, 0, 0, 0);

                                btdb.queryFuturesData(optionStrategies[i].dateTime,
                                    optionStrategies[i].rollIntoLegInfo[legCounter].idContract,
                                    optionArrayTypes,
                                    optionStrategies[i].rollIntoLegData[legCounter]);


                                if (optionStrategies[i].instrument.substituteSymbolEOD)
                                {
                                    //sets up data for realtime EOD SPAN symbol
                                    //
                                    optionStrategies[i].rollIntoLegInfo[legCounter].cqgSubstituteSymbol =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Replace(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD,
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);


                                    
                                }
                            }



                        }


                    }
                }

                btdb.closeDB();



            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        internal string callLocalDBForFutureIdContract(object queryObject, int idInstrument,
            int contractYear, int contractMonthInt)
        {
            TMLModelDBQueries btdb = (TMLModelDBQueries)queryObject;

            int idContract = btdb.queryFutureContractId(idInstrument,
                contractYear, contractMonthInt);

            StringBuilder stringContract = new StringBuilder();
            stringContract.Append(idContract);

            return stringContract.ToString();
        }

        internal string callCloudDBForFutureIdContract(object queryObject, int idInstrument,
            int contractYear, int contractMonthInt)
        {
            TMLAzureModelDBQueries btdb = (TMLAzureModelDBQueries)queryObject;

            int idContract = btdb.queryFutureContractId(idInstrument,
                contractYear, contractMonthInt);

            StringBuilder stringContract = new StringBuilder();
            stringContract.Append(idContract);

            return stringContract.ToString();
        }

        public void fillStrategiesAndInstrumentsFromAzure()
        {

#if DEBUG
            try
#endif
            {
                TMLAzureModelDBQueries btdb = new TMLAzureModelDBQueries();


                OptionStrategy[] optionStrategiesTemp = btdb.queryStrategies(initializationParms.idPortfolioGroup,// DateTime.Now);
                    initializationParms.modelDateTime.Date);

                QueryFutureIdContractDelegate queryFutureIdContractDelegate =
                    new QueryFutureIdContractDelegate(callCloudDBForFutureIdContract);

                List<OptionStrategy> optionStrategyList = getSupplementalContracts(optionArrayTypes,
                    queryFutureIdContractDelegate, btdb);

                if (optionStrategiesTemp != null && optionStrategyList.Count > 0)
                {
                    optionStrategies = new OptionStrategy[optionStrategiesTemp.Length + optionStrategyList.Count];

                    int strategyCnt = 0;
                    for (int i = 0; i < optionStrategiesTemp.Length; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategiesTemp[i];

                        strategyCnt++;
                    }

                    for (int i = 0; i < optionStrategyList.Count; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategyList[i];

                        strategyCnt++;
                    }

                    _supplementContractFilled = true;
                }
                else if (optionStrategiesTemp != null)
                {
                    optionStrategies = optionStrategiesTemp;
                }
                else if (optionStrategyList.Count > 0)
                {
                    optionStrategies = new OptionStrategy[optionStrategyList.Count];

                    int strategyCnt = 0;

                    for (int i = 0; i < optionStrategyList.Count; i++)
                    {
                        optionStrategies[strategyCnt] = optionStrategyList[i];

                        strategyCnt++;
                    }

                    _supplementContractFilled = true;
                }
                else
                {
                    optionStrategies = null;
                }




                portfolioGroupAllocation =
                    btdb.queryPortfolioGroupAllocation(initializationParms.idPortfolioGroup);



                //fcmOfficeAcctHashSet
                //    = new ConcurrentDictionary<string, FCM_POFFIC_PACCT>();

                //TODO AUG 26 2015
                //fill CONFIG 
                portfolioGroupAllocationAndInitialize();

                resetPortfolioGroupFcmOfficeAcctChosenHashSet();

                //call INSTRUMENT_SPECIFIC_FIX_FIELDS.cfg
                setupInstrumentSpecificFIXFields();


                if (optionStrategies == null)
                {
                    optionStrategies = new OptionStrategy[1];
                    optionStrategies[0] = new OptionStrategy();
                    optionStrategies[0].instrument = new Instrument();

                }

                instruments = btdb.queryInstruments(initializationParms.idPortfolioGroup);

                substituteInstrumentHash = btdb.fillSubstituteInstrumentHash(instruments);

                if (initializationParms.useHalfday)
                {

                    for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                    {
                        instruments[instrumentCnt].customDayBoundaryTime = initializationParms.halfDayTransactionTime;
                        instruments[instrumentCnt].decisionOffsetMinutes = initializationParms.halfDayDecisionOffsetMinutes;
                    }

                }


                instrument_DefaultFuturesArray = new Instrument_DefaultFutures[instruments.Length];

                for (int i = 0; i < instrument_DefaultFuturesArray.Length; i++)
                {
                    instrument_DefaultFuturesArray[i] = new Instrument_DefaultFutures();

                    instrument_DefaultFuturesArray[i].idInstrument = instruments[i].idInstrument;

                    instrument_DefaultFuturesArray[i].instrument = instruments[i];

                    instrument_DefaultFuturesArray[i].defaultFutures =
                        btdb.queryInstrumentDefaultFutureInfo(
                            initializationParms.modelDateTime.Date,
                                instruments[i].idInstrument,
                                    optionArrayTypes);

                    instrument_DefaultFuturesArray[i].defaultFuturesData =
                        new LegData[instrument_DefaultFuturesArray[i].defaultFutures.Length];

                    for (int futureCnt = 0; futureCnt < instrument_DefaultFuturesArray[i].defaultFutures.Length; futureCnt++)
                    {
                        instrument_DefaultFuturesArray[i].defaultFuturesData[futureCnt] = new LegData();

                        //btdb.queryFuturesData(initializationParms.modelDateTime.Date,
                        //   instrument_DefaultFuturesArray[i].defaultFutures[futureCnt].idContract, optionArrayTypes,
                        //       instrument_DefaultFuturesArray[i].defaultFuturesData[futureCnt]);

                        btdb.queryFutureInfoAndDataFromCloud(
                                    instrument_DefaultFuturesArray[i].defaultFutures[futureCnt].idContract,
                                    null,
                                    instrument_DefaultFuturesArray[i].defaultFuturesData[futureCnt],
                                    initializationParms.modelDateTime.Date,
                                    optionArrayTypes,
                                    futureDataSetHashSet, false, this, futureIdFromInfo);

                    }

                }



                //instruments = btdb.queryInstrumentsThatPossiblyHaveData();

                instrumentModelCalcTotals = new LiveSpreadTotals[instruments.Length];
                instrumentSpreadTotals = new LiveSpreadTotals[instruments.Length];

                //the total calcs includes an extra increment in array size to include sum of all portfoliogroups
                instrumentADMCalcTotals = new LiveSpreadTotals[instruments.Length, portfolioGroupAllocation.Length + 1];
                instrumentADMSpreadTotals = new LiveSpreadTotals[instruments.Length, portfolioGroupAllocation.Length + 1];



                for (int i = 0; i < instruments.Length; i++)
                {
                    instrumentModelCalcTotals[i] = new LiveSpreadTotals();
                    instrumentSpreadTotals[i] = new LiveSpreadTotals();

                    instruments[i].isSpread = true;

                    for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
                    {
                        instrumentADMCalcTotals[i, portfolioGroupCnt] = new LiveSpreadTotals();

                        instrumentADMSpreadTotals[i, portfolioGroupCnt] = new LiveSpreadTotals();
                    }
                }


                //the total calcs includes an extra increment in array size to include sum of all portfoliogroups
                portfolioADMSpreadCalcTotals = new LiveSpreadTotals[portfolioGroupAllocation.Length + 1];
                portfolioADMSpreadTotals = new LiveSpreadTotals[portfolioGroupAllocation.Length + 1];

                for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {
                    portfolioADMSpreadCalcTotals[portfolioGroupCnt] = new LiveSpreadTotals();
                    portfolioADMSpreadTotals[portfolioGroupCnt] = new LiveSpreadTotals();
                }

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    int instrumentCount = 0;

                    while (instrumentCount < instruments.Length)
                    {
                        if (instruments[instrumentCount].idInstrument == optionStrategies[i].idInstrument)
                        {
                            optionStrategies[i].indexOfInstrumentInInstrumentsArray = instrumentCount;

                            optionStrategies[i].instrument = instruments[instrumentCount];
                            break;
                        }

                        instrumentCount++;
                    }

                }
                

                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    optionStrategies[i].dateTime = initializationParms.modelDateTime.Date;  //DateTime.Now.Date;


                    if (!optionStrategies[i].supplementContract)
                    {
                        optionStrategies[i].optionStrategyParameters =
                            new OptionStrategyParameter[optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0)];

                        btdb.queryStrategyState(optionStrategies[i], optionArrayTypes);
                    }

                    parseStrategyState(optionStrategies[i]);

                    checkIfStrategyHasPosition(optionStrategies[i]);

                    //if(optionStrategies[i].holdsCurrentPosition)
                    //{
                    //    optionStrategies[i].idxOfStratsHoldingPositions = idxOfStrategiesThatHoldPositions;

                    //    idxOfStrategiesThatHoldPositions++;
                    //}

                    checkStrategyAction_FillSignalType_FillRiskType(optionStrategies[i]);

                    if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null)
                    {
                        int numberOfLegs = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                        optionStrategies[i].legInfo = new LegInfo[numberOfLegs];

                        optionStrategies[i].legData = new LegData[numberOfLegs];



                        for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {
                            optionStrategies[i].legInfo[legCounter] = new LegInfo();

                            optionStrategies[i].legData[legCounter] = new LegData();


                            switch ((int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter])
                            {
                                case (int)OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                    optionStrategies[i].legInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;
                                    break;
                            }



                            if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter] != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                //btdb.queryOptionInfo(optionStrategies[i], legCounter, optionArrayTypes);



                                //btdb.queryOptionExpirations(optionStrategies[i], optionStrategies[i].legInfo[legCounter]);

                                //btdb.queryOptionData(optionStrategies[i], legCounter, optionArrayTypes);

                                

                                btdb.queryOptionInfoAndDataFromCloud(
                                    //optionStrategies[i],
                                    //legCounter,
                                    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                        .stateValueParsed[legCounter],
                                    optionStrategies[i].instrument.idInstrument,
                                    optionStrategies[i].legInfo[legCounter],
                                    optionStrategies[i].legData[legCounter],
                                    optionStrategies[i].dateTime,
                                    optionArrayTypes,
                                    optionDataSetHashSet);


                                

                                String keyString = getOptionIdHashSetKeyString(optionStrategies[i].legInfo[legCounter].optionMonthInt,
                                    optionStrategies[i].legInfo[legCounter].optionYear,
                                    optionStrategies[i].instrument.idInstrument,
                                    optionStrategies[i].legInfo[legCounter].optionCallOrPut.ToString(),
                                    optionStrategies[i].legInfo[legCounter].optionStrikePrice);
                                    //optionStrategies[i].legInfo[legCounter].idOption);

                                if (!optionIdFromInfo.ContainsKey(keyString.ToString()))
                                {
                                    optionIdFromInfo.Add(keyString.ToString(), optionStrategies[i].legInfo[legCounter].idOption);
                                }
                            }
                            else
                            {
                                optionStrategies[i].idxOfFutureLeg = legCounter;

                                optionStrategies[i].legInfo[legCounter].optionExpirationTime = new DateTime(
                                    1900, 1, 1, 0, 0, 0);

                                //btdb.queryFutureInfo(//optionStrategies[i],
                                //    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                //        .stateValueParsed[legCounter],
                                //        optionArrayTypes,
                                //        optionStrategies[i].legInfo[legCounter]);

                                

                                //btdb.queryFuturesData(optionStrategies[i].dateTime,
                                //    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                //        .stateValueParsed[legCounter],
                                //    optionArrayTypes,
                                //    optionStrategies[i].legData[legCounter]);


                                btdb.queryFutureInfoAndDataFromCloud(
                                    (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                                            .stateValueParsed[legCounter],
                                    optionStrategies[i].legInfo[legCounter],
                                    optionStrategies[i].legData[legCounter],
                                    optionStrategies[i].dateTime,
                                    optionArrayTypes,
                                    futureDataSetHashSet, false, this, futureIdFromInfo);
                            }

                            if (optionStrategies[i].instrument.substituteSymbolEOD)
                            {
                                //sets up data for realtime EOD SPAN symbol

                                optionStrategies[i].legInfo[legCounter].useSubstitueSymbolEOD = true;

                                optionStrategies[i].legInfo[legCounter].instrumentSymbolPreEOD =
                                    optionStrategies[i].instrument.instrumentSymbolPreEOD;

                                optionStrategies[i].legInfo[legCounter].instrumentSymbolEODSubstitute =
                                    optionStrategies[i].instrument.instrumentSymbolEOD;

                                //optionStrategies[i].legInfo[legCounter].cqgSubstituteSymbol =
                                //    optionStrategies[i].legInfo[legCounter].cqgSymbol.Replace(
                                //        optionStrategies[i].legInfo[legCounter].instrumentSymbolPreEOD,
                                //        optionStrategies[i].legInfo[legCounter].instrumentSymbolEODSubstitute);

                                generateCQGSymbolForEODSubstitution(
                                    optionStrategies[i].legInfo[legCounter], optionStrategies[i].instrument);
                            }
                        }
                    }

                    //NEED TO GET OPTION DATA FOR ROLL OVER AND SET UP STRIKE SELECTION
                    if (optionStrategies[i].rollIntoLegInfo != null)
                    {
                        Type tblOptionMonthTypes = typeof(OPTION_MONTHS);
                        Array tblOptionMonthTypesArray = Enum.GetNames(tblOptionMonthTypes);

                        optionStrategies[i].rollIntoLegData = new LegData[optionStrategies[i].rollIntoLegInfo.Length];

                        for (int legCounter = 0; legCounter < optionStrategies[i].rollIntoLegInfo.Length; legCounter++)
                        {
                            //optionStrategies[i].legInfo[legCounter] = new LegInfo();

                            optionStrategies[i].rollIntoLegData[legCounter] = new LegData();

                            if (optionStrategies[i].instrument.substituteSymbolEOD)
                            {
                                //sets up data for realtime EOD SPAN symbol

                                optionStrategies[i].rollIntoLegInfo[legCounter].useSubstitueSymbolEOD = true;

                                optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD =
                                    optionStrategies[i].instrument.instrumentSymbolPreEOD;

                                optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute =
                                    optionStrategies[i].instrument.instrumentSymbolEOD;
                            }

                            switch ((int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter])
                            {
                                case (int)OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                                    break;

                                case (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                    optionStrategies[i].rollIntoLegInfo[legCounter].legContractType = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;
                                    break;
                            }

                            if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                .stateValueParsed[legCounter] != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                //btdb.queryOptionInfo(optionStrategies[i], legCounter, optionArrayTypes);

                                optionStrategies[i].rollIntoLegInfo[legCounter].optionCallOrPut =
                                    optionStrategies[i].legInfo[legCounter].optionCallOrPut;

                                //optionStrategies[i].rollIntoLegInfo[legCounter].optionMonth =
                                //    Convert.ToChar(
                                //    tblOptionMonthTypesArray.GetValue(optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt - 1));

                                btdb.queryOptionInfoForRollingOptionPreSymbol(
                                    optionStrategies[i], legCounter, optionArrayTypes);

                                String strikeString = ConversionAndFormatting.convertToTickMovesString(
                                                optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePrice,
                                                            optionStrategies[i].instrument.optionStrikeIncrement,
                                                            optionStrategies[i].instrument.optionStrikeDisplay);


                                int w = optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.IndexOf(strikeString);

                                //String x = optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Substring(
                                //   0, w);

                                //TSErrorCatch.debugWriteOut(x);

                                optionStrategies[i].rollIntoLegInfo[legCounter].
                                    cqgSymbolWithoutStrike_ForRollover =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Substring(
                                            0, w);


                                if (optionStrategies[i].instrument.substituteSymbolEOD)
                                {
                                    //sets up data for realtime EOD SPAN symbol
                                    //don't use generateCQGSymbolForEODSubstitution because a rolling contract without strikes

                                    optionStrategies[i].rollIntoLegInfo[legCounter].cqgSubstituteSymbolWithoutStrike_ForRollover =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover.Replace(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD,
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);
                                }

                            }
                            else
                            {
                                optionStrategies[i].idxOfFutureLeg = legCounter;

                                //btdb.queryFutureInfo(//optionStrategies[i],
                                //    optionStrategies[i].rollIntoLegInfo[legCounter].idContract,
                                //    optionArrayTypes,
                                //    optionStrategies[i].rollIntoLegInfo[legCounter]);

                                optionStrategies[i].rollIntoLegInfo[legCounter].optionExpirationTime = new DateTime(
                                    1900, 1, 1, 0, 0, 0);

                                //btdb.queryFuturesData(optionStrategies[i].dateTime,
                                //    optionStrategies[i].rollIntoLegInfo[legCounter].idContract,
                                //    optionArrayTypes,
                                //    optionStrategies[i].rollIntoLegData[legCounter]);



                                btdb.queryFutureInfoAndDataFromCloud(
                                    optionStrategies[i].rollIntoLegInfo[legCounter].idContract,
                                    optionStrategies[i].rollIntoLegInfo[legCounter],
                                    optionStrategies[i].rollIntoLegData[legCounter],
                                    optionStrategies[i].dateTime,
                                    optionArrayTypes,
                                    futureDataSetHashSet, false, this, futureIdFromInfo);

                                if (optionStrategies[i].instrument.substituteSymbolEOD)
                                {
                                    //sets up data for realtime EOD SPAN symbol
                                    //don't use generateCQGSymbolForEODSubstitution because a rolling contract and this is a future
                                    optionStrategies[i].rollIntoLegInfo[legCounter].cqgSubstituteSymbol =
                                        optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol.Replace(
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolPreEOD,
                                            optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);
                                }
                            }



                        }


                    }
                }

                //btdb.closeDB();



            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }


        private string getOptionIdHashSetKeyString(int monthInt,
            int optionYear,
            int idInstrument,
            string callOrPut,
            double strikeInDecimal)
        {
            StringBuilder keyString = new StringBuilder();

            keyString.Append(monthInt);
            keyString.Append(".");
            keyString.Append(optionYear);
            keyString.Append(".");
            keyString.Append(idInstrument);
            keyString.Append(".");
            keyString.Append(callOrPut);
            keyString.Append(".");
            keyString.Append(strikeInDecimal);

            //if (!optionIdFromInfo.ContainsKey(keyString.ToString()))
            //{
            //    optionIdFromInfo.Add(keyString.ToString(), optionId);
            //}

            return keyString.ToString();
        }


        internal string getFutureContractIdHashSetKeyString(int monthInt,
            int futureContractYear,
            int idInstrument)
        {
            StringBuilder keyString = new StringBuilder();

            keyString.Append(monthInt);
            keyString.Append(".");
            keyString.Append(futureContractYear);
            keyString.Append(".");
            keyString.Append(idInstrument);

            return keyString.ToString();
        }


        private void parseStrategyState(OptionStrategy optionStrategy)
        {
            if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null)
            {
                int numberOfLegs =
                    (int)optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                for (int i = 0; i < optionStrategy.optionStrategyParameters.Length; i++)
                {
                    if (optionStrategy.optionStrategyParameters[i].parseParameter == TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER)
                    {
                        optionStrategy.optionStrategyParameters[i].stateValueParsed =
                            parseStrategyStateConfig(optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed, numberOfLegs);
                    }
                    else if (optionStrategy.optionStrategyParameters[i].parseParameter == TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER)
                    {
                        optionStrategy.optionStrategyParameters[i].stateValueParsed = new double[numberOfLegs];

                        for (int legCnt = 0; legCnt < numberOfLegs; legCnt++)
                        {
                            optionStrategy.optionStrategyParameters[i].stateValueParsed[legCnt] =
                                optionStrategy.optionStrategyParameters[i].stateValue;
                        }
                    }
                    else if (optionStrategy.optionStrategyParameters[i].parseParameter == TBL_DB_PARSE_PARAMETER.PARSE_ROLL_PARAMETER_SCRIPT)
                    {
                        string sBuffer = optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed.ToLower();

                        /*
                         * need to use the just read in parameters here b/c the legData objects are
                         * not created yet
                         */

                        if (sBuffer.Length == 0 || sBuffer == null)
                        {
                            optionStrategy.rollIntoLegInfo = null;
                        }
                        else
                        {
                            optionStrategy.rollIntoLegInfo = new RollLegInfo[numberOfLegs];

                            String[] saParsed = sBuffer.Split(';');//";".ToCharArray());

                            int underlyingContractId = 0;

                            for (int rollParamterLegs = 0; rollParamterLegs < saParsed.Length; rollParamterLegs++)
                            {
                                //if (optionStrategy.legInfo[i].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                if ((int)optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                                        .stateValueParsed[rollParamterLegs] == (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    /*
                                     * this is just a set format, the future leg in the roll script
                                     * will only have the idcontract info
                                     */
                                    optionStrategy.rollIntoLegInfo[rollParamterLegs] =
                                        new RollLegInfo();

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs].idContract =
                                        Convert.ToInt32(saParsed[rollParamterLegs]);

                                    underlyingContractId = optionStrategy.rollIntoLegInfo[rollParamterLegs].idContract;
                                }
                                else
                                {
                                    /*
                                     * set format, the option legs will have 
                                     * monthint,year,strikeoffset
                                     */

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs] =
                                        new RollLegInfo();

                                    String[] legRollParameters = saParsed[rollParamterLegs].Split(',');

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs].optionMonthInt =
                                        Convert.ToInt32(legRollParameters[(int)ROLL_SPREAD_PARAMETERS.OPTION_MONTH_INT]);

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs].optionYear =
                                        Convert.ToInt32(legRollParameters[(int)ROLL_SPREAD_PARAMETERS.OPTION_YEAR_INT]);

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs].strikeLevelOffsetForRoll =
                                        Convert.ToInt32(legRollParameters[(int)ROLL_SPREAD_PARAMETERS.OPTION_STRIKE_LEVEL_INT]);

                                    optionStrategy.rollIntoLegInfo[rollParamterLegs].idUnderlyingContract =
                                        underlyingContractId;

                                }
                            }
                        }

                    }
                    else if (optionStrategy.optionStrategyParameters[i].parseParameter == TBL_DB_PARSE_PARAMETER.PARSE_ENTRY_SCRIPT)
                    {
                        //optionStrategy.optionStrategyParameters[i].stateValueParsed = new double[numberOfLegs];

                        //for (int legCnt = 0; legCnt < numberOfLegs; legCnt++)
                        {
                            //optionStrategy.optionStrategyParameters[i].stateValueParsed[legCnt] =
                            //    optionStrategy.optionStrategyParameters[i].stateValue;

                            ReversePolishNotation rpn = new ReversePolishNotation(optionStrategy, this);

                            optionStrategy.entryRule = rpn;

                            //rpn.Parse("( SYNCLOSE < 6296.5 )  && ( VOLUME <= 3143974 )");

                            //rpn.Parse("(100 < 200) and (300 < 200)");

                            rpn.Parse(optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed);

                            //TSErrorCatch.debugWriteOut(rpn.OriginalExpression);
                            //TSErrorCatch.debugWriteOut(rpn.TransitionExpression);
                            //TSErrorCatch.debugWriteOut(rpn.PostfixExpression);

                            //                             bool result = rpn.Evaluate();
                            //                             Console.WriteLine("orig:   {0}", rpn.OriginalExpression);
                            //                             Console.WriteLine("tran:   {0}", rpn.TransitionExpression);
                            //                             Console.WriteLine("post:   {0}", rpn.PostfixExpression);
                            //                             Console.WriteLine("result: {0}", result);

                            //rpn.Parse("3 == 4");
                            //result = rpn.Evaluate(this);
                            //Console.WriteLine("orig:   {0}", rpn.OriginalExpression);
                            //Console.WriteLine("tran:   {0}", rpn.TransitionExpression);
                            //Console.WriteLine("post:   {0}", rpn.PostfixExpression);
                            //Console.WriteLine("result: {0}", result);

                            //rpn.Parse("3 < 4");
                            //result = rpn.Evaluate(this);
                            //Console.WriteLine("orig:   {0}", rpn.OriginalExpression);
                            //Console.WriteLine("tran:   {0}", rpn.TransitionExpression);
                            //Console.WriteLine("post:   {0}", rpn.PostfixExpression);
                            //Console.WriteLine("result: {0}", result);

                            //rpn.Parse("3 <= 4");
                            //result = rpn.Evaluate(this);
                            //Console.WriteLine("orig:   {0}", rpn.OriginalExpression);
                            //Console.WriteLine("tran:   {0}", rpn.TransitionExpression);
                            //Console.WriteLine("post:   {0}", rpn.PostfixExpression);
                            //Console.WriteLine("result: {0}", result);
                        }
                    }
                    else if (optionStrategy.optionStrategyParameters[i].parseParameter == TBL_DB_PARSE_PARAMETER.PARSE_EXIT_SCRIPT)
                    {
                        ReversePolishNotation rpn = new ReversePolishNotation(optionStrategy, this);

                        optionStrategy.exitRule = rpn;

                        rpn.Parse(optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed);
                    }
                }
            }
        }


        private void checkIfStrategyHasPosition(OptionStrategy optionStrategy)
        {
            if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null)
            {
                int legCnt = 0;

                while (legCnt < optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed.Length)
                {
                    if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCnt]
                            != 0)
                    {
                        optionStrategy.holdsCurrentPosition = true;

                        break;
                    }

                    legCnt++;
                }
            }
        }

        private void checkStrategyAction_FillSignalType_FillRiskType(OptionStrategy optionStrategy)
        {
            optionStrategy.idSignalType =
                Convert.ToInt16(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idSignalType]
                    .stateValue);

            optionStrategy.idRiskType =
                Convert.ToInt16(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.idRiskType]
                    .stateValue);

            if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryRule]
                .stateValueStringNotParsed != null
                && optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryRule]
                .stateValueStringNotParsed.Length > 0)
            {
                if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes]
                .stateValueStringNotParsed != null
                    && optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes]
                .stateValueStringNotParsed.Length > 0)
                {
                    optionStrategy.actionType = SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY_WITH_ROLL;
                }
                else
                {
                    optionStrategy.actionType = SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY;
                }
            }
            else if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitRule]
                .stateValueStringNotParsed != null
                && optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitRule]
                .stateValueStringNotParsed.Length > 0)
            {
                if (optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes]
                .stateValueStringNotParsed != null
                    && optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes]
                .stateValueStringNotParsed.Length > 0)
                {
                    optionStrategy.actionType = SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT_OR_ROLL_OVER;
                }
                else
                {
                    optionStrategy.actionType = SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT;
                }
            }
            else
            {
                optionStrategy.actionType = SPREAD_POTENTIAL_OPTION_ACTION_TYPES.NO_ACTION;
            }
        }

        private double[] parseStrategyStateConfig(String config, int numberOfLegs)
        {

            double[] parsedConfig = new double[numberOfLegs];

#if DEBUG
            try
#endif
            {
                int locationOfStart = 0;
                int commaCount = 0;
                int locationOfEnd = 0;

                while (config != null
                    && locationOfEnd >= 0
                    && locationOfStart < config.Length
                    && commaCount < numberOfLegs)
                {
                    //TSErrorCatch.debugWriteOut(config);

                    locationOfEnd = config.Substring(locationOfStart).IndexOf(",");

                    String lineSection;
                    if (locationOfEnd > 0)
                    {
                        lineSection = config.Substring(locationOfStart, locationOfEnd);
                    }
                    else
                    {
                        lineSection = config.Substring(locationOfStart);
                    }

                    locationOfStart += locationOfEnd + 1;

                    //TSErrorCatch.debugWriteOut(lineSection.Substring(0, 0) + "****" + lineSection.Substring(0, 1));

                    if (lineSection.Substring(0, 1).CompareTo(",") != 0)
                    {
                        parsedConfig[commaCount] = Convert.ToDouble(lineSection);
                    }
                    else
                    {
                        parsedConfig[commaCount] = 0;
                    }

                    //optionConfigSetupGrid.Rows[optionConfigNameCounter].Cells[commaCount].Value = Convert.ToDouble(lineSection);

                    commaCount++;
                }

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return parsedConfig;
        }

        private void fillAllExpressionsForCQGData()
        {
            

            fillExpressionOptionList();

            fillSubstituteEODExpressionOptionList();

            fillExpressionWithDefaultFutures();

            if (sAdmPositionImportWeb != null)
            {
                fillADMExpressionOptionList();

                fillSubstitueEODADMExpressionOptionList();
            }

            //add all options to futures
            int expressionCounter = 0;

            while (expressionCounter < optionSpreadExpressionList.Count)
            {
                if (optionSpreadExpressionList[expressionCounter].optionExpressionType
                        == OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE
                   && optionSpreadExpressionList[expressionCounter].callPutOrFuture
                        == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    for (int countOfOptionExpContainFuture = 0;
                        countOfOptionExpContainFuture < optionSpreadExpressionList.Count; countOfOptionExpContainFuture++)
                    {
                        if (optionSpreadExpressionList[expressionCounter].futureId
                            == optionSpreadExpressionList[countOfOptionExpContainFuture].underlyingFutureId)
                        {
                            optionSpreadExpressionList[expressionCounter].optionExpressionsThatUseThisFutureAsUnderlying
                                .Add(optionSpreadExpressionList[countOfOptionExpContainFuture]);

                            optionSpreadExpressionList[countOfOptionExpContainFuture]
                                .underlyingFutureExpression = optionSpreadExpressionList[expressionCounter];
                        }
                    }
                }

                expressionCounter++;
            }


            // add risk free rate to expressions

            OptionSpreadExpression expLstRiskFreeRate = new OptionSpreadExpression(
                    OPTION_SPREAD_CONTRACT_TYPE.BLANK,
                    OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE);

            //expLstRiskFreeRate.instrument = new Instrument();


            if (initializationParms.useCloudDb)
            {
                TMLAzureModelDBQueries btdb = new TMLAzureModelDBQueries();

                expLstRiskFreeRate.optionInputFieldsFromTblOptionInputSymbols = 
                    btdb.queryOptionInputSymbols(-1,
                    (int)OPTION_FORMULA_INPUT_TYPES.OPTION_RISK_FREE_RATE);
            }
            else
            {
                TMLModelDBQueries btdb = new TMLModelDBQueries();

                btdb.connectDB(initializationParms.dbServerName);
               
                expLstRiskFreeRate.optionInputFieldsFromTblOptionInputSymbols =
                    btdb.queryOptionInputSymbols(-1,
                    (int)OPTION_FORMULA_INPUT_TYPES.OPTION_RISK_FREE_RATE);

                btdb.closeDB();
            }
            
            



            expLstRiskFreeRate.cqgSymbol =
                expLstRiskFreeRate.optionInputFieldsFromTblOptionInputSymbols
                .optionInputCQGSymbol;

            expLstRiskFreeRate.normalSubscriptionRequest = true;

            optionSpreadExpressionList.Add(expLstRiskFreeRate);

            

            sOptionRealtimeMonitor.passExpressionListToRealtimeMonitor(optionSpreadExpressionList);

            sOptionRealtimeMonitor.updateSetupExpressionListGridView();
        }

        private void fillExpressionWithDefaultFutures()
        {
#if DEBUG
            try
#endif
            {


                for (int instrumentDefaultCnt = 0; instrumentDefaultCnt < instrument_DefaultFuturesArray.Length; instrumentDefaultCnt++)
                {


                    for (int futureCnt = 0;
                        futureCnt < instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures.Length; futureCnt++)
                    {
                        //int numberOfLegs = sLiveADMStrategyInfoList[admPositionWebCounter].admLegInfo.Count;

                        //for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {

                            bool foundCqgSymbol = false;
                            int currentExpressionListCounter = 0;

                            while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                            {
                                //   TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                if (instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures[futureCnt].cqgSymbol.CompareTo(
                                    optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                {

                                    foundCqgSymbol = true;
                                    break;
                                }

                                currentExpressionListCounter++;

                            }

                            if (!foundCqgSymbol)
                            {
                                OptionSpreadExpression expLst = new OptionSpreadExpression(
                                    OPTION_SPREAD_CONTRACT_TYPE.FUTURE,
                                    OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                expLst.cqgSymbol = instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures[futureCnt].cqgSymbol;


                                expLst.instrument = instrument_DefaultFuturesArray[instrumentDefaultCnt].instrument;

                                DateTime currentDate = DateTime.Now.Date;

                                //expLst.impliedVolFromSpan =
                                //    sAdmPositionImportWeb[admPositionWebCounter].contractData.impliedVolFromDB;

                                expLst.previousDateTimeBoundaryStart
                                    = instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFuturesData[futureCnt].dataDateTimeFromDB.Date
                                    .AddHours(
                                        instrument_DefaultFuturesArray[instrumentDefaultCnt].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                       instrument_DefaultFuturesArray[instrumentDefaultCnt].instrument.customDayBoundaryTime.Minute)
                                    .AddMinutes(1);

                                expLst.yesterdaySettlement =
                                    instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFuturesData[futureCnt].settlementPriceFromDB;
                                expLst.yesterdaySettlementFilled = true;

                                char callOrPutOrFutureChar = 'F';


                                expLst.callPutOrFutureChar = callOrPutOrFutureChar;


                                expLst.futureId =
                                    instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures[futureCnt].idContract;

                                expLst.futureContractMonthInt = instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures[futureCnt].contractMonthInt;

                                expLst.futureContractYear = instrument_DefaultFuturesArray[instrumentDefaultCnt].defaultFutures[futureCnt].contractYear;


                                optionSpreadExpressionList.Add(expLst);

                                expLst.normalSubscriptionRequest = true;
                            }
                            else
                            {
                                optionSpreadExpressionList[currentExpressionListCounter].normalSubscriptionRequest = true;
                            }




                        }
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

        private void fillExpressionOptionList()
        {
#if DEBUG
            try
#endif
            {
                for (int i = 0; i < optionStrategies.Length; i++)
                {


                    if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null)
                    {
                        int numberOfLegs = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                        for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {

                            bool foundCqgSymbol = false;
                            int currentExpressionListCounter = 0;

                            //TSErrorCatch.debugWriteOut(optionStrategies[i].legInfo[legCounter].cqgSymbol);

                            while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                            {
                                //   TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                if (optionStrategies[i].legInfo[legCounter].cqgSymbol.CompareTo(
                                    optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                {
                                    foundCqgSymbol = true;
                                    break;
                                }
                                currentExpressionListCounter++;
                            }

                            if (!foundCqgSymbol)
                            {
                                OptionSpreadExpression expLst = new OptionSpreadExpression(
                                    optionStrategies[i].legInfo[legCounter].legContractType,
                                    OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                expLst.cqgSymbol = optionStrategies[i].legInfo[legCounter].cqgSymbol;


                                //expLst.optionMonthInt = optionStrategies[i].legInfo[legCounter].optionMonthInt;
                                //expLst.optionYear = optionStrategies[i].legInfo[legCounter].optionYear;

                                //expLst.underlyingMonthInt = optionStrategies[i].legInfo[legCounter].contractMonthInt;
                                //expLst.underlyingYear = optionStrategies[i].legInfo[legCounter].contractYear;


                                expLst.instrument = optionStrategies[i].instrument;

                                DateTime currentDate = DateTime.Now.Date;

                                expLst.impliedVolFromSpan =
                                    optionStrategies[i].legData[legCounter].impliedVolFromDB;

                                expLst.previousDateTimeBoundaryStart
                                    = optionStrategies[i].legData[legCounter].dataDateTimeFromDB.Date
                                    .AddHours(
                                        optionStrategies[i].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                        optionStrategies[i].instrument.customDayBoundaryTime.Minute)
                                    .AddMinutes(1);

                                expLst.yesterdaySettlement =
                                    optionStrategies[i].legData[legCounter].settlementPriceFromDB;
                                expLst.yesterdaySettlementFilled = true;

                                //expLst.impliedVolIndex

                                //expLst.optionExpressionType = OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE;

                                //expLst.callPutOrFuture = optionStrategies[i].legInfo[legCounter].legType;

                                char callOrPutOrFutureChar = 'C';

                                switch (expLst.callPutOrFuture)
                                {
                                    case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                        callOrPutOrFutureChar = 'C';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                        callOrPutOrFutureChar = 'P';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                        callOrPutOrFutureChar = 'F';
                                        break;
                                }

                                expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                                if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    expLst.optionId = optionStrategies[i].legInfo[legCounter].idOption;
                                    expLst.underlyingFutureId = optionStrategies[i].legInfo[legCounter].idUnderlyingContract;

                                    expLst.strikePrice = optionStrategies[i].legInfo[legCounter].optionStrikePrice;

                                    expLst.yearFraction =
                                        calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
                                                            DateTime.Now.Date);

                                    expLst.optionMonthInt = optionStrategies[i].legInfo[legCounter].optionMonthInt;

                                    expLst.optionYear = optionStrategies[i].legInfo[legCounter].optionYear;
                                }
                                else
                                {
                                    expLst.futureId = optionStrategies[i].legInfo[legCounter].idContract;

                                    expLst.futureContractMonthInt = optionStrategies[i].legInfo[legCounter].contractMonthInt;

                                    expLst.futureContractYear = optionStrategies[i].legInfo[legCounter].contractYear;
                                }

                                expLst.spreadIdx.Add(i);
                                expLst.legIdx.Add(legCounter);
                                expLst.rowIdx.Add(optionStrategies[i].liveGridRowLoc[legCounter]);

                                int numberOfContracts = (int)optionStrategies[i].optionStrategyParameters[
                                                (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                                expLst.numberOfLotsHeldForContractSummary += numberOfContracts;

                                optionSpreadExpressionList.Add(expLst);

                                optionStrategies[i].legData[legCounter].optionSpreadExpression = expLst;

                                expLst.normalSubscriptionRequest = true;
                            }
                            else
                            {
                                int numberOfContracts = (int)optionStrategies[i].optionStrategyParameters[
                                                (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                                optionSpreadExpressionList[currentExpressionListCounter].numberOfLotsHeldForContractSummary += numberOfContracts;

                                optionSpreadExpressionList[currentExpressionListCounter].spreadIdx.Add(i);
                                optionSpreadExpressionList[currentExpressionListCounter].legIdx.Add(legCounter);
                                optionSpreadExpressionList[currentExpressionListCounter].rowIdx.Add(
                                    optionStrategies[i].liveGridRowLoc[legCounter]);

                                optionStrategies[i].legData[legCounter].optionSpreadExpression = optionSpreadExpressionList[currentExpressionListCounter];

                                optionSpreadExpressionList[currentExpressionListCounter].normalSubscriptionRequest = true;
                            }


                        }

                        //for roll over info

                        if (optionStrategies[i].rollIntoLegInfo != null)
                        {

                            int rollOverLegCounter = 0;
                            while (rollOverLegCounter < numberOfLegs)
                            {
                                if (optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {



                                    bool foundCqgSymbol = false;
                                    int currentExpressionListCounter = 0;

                                    //TSErrorCatch.debugWriteOut(optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSymbol);

                                    while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                                    {
                                        //TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                        if (optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSymbol.CompareTo(
                                            optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                        {
                                            foundCqgSymbol = true;
                                            break;
                                        }
                                        currentExpressionListCounter++;
                                    }

                                    if (!foundCqgSymbol)
                                    {
                                        OptionSpreadExpression expLst = new OptionSpreadExpression(
                                            optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].legContractType,
                                            OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                        expLst.cqgSymbol = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSymbol;


                                        //expLst.optionMonthInt = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionMonthInt;
                                        //expLst.optionYear = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionYear;

                                        //expLst.underlyingMonthInt = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].contractMonthInt;
                                        //expLst.underlyingYear = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].contractYear;



                                        expLst.instrument = optionStrategies[i].instrument;

                                        expLst.previousDateTimeBoundaryStart
                                            = optionStrategies[i].rollIntoLegData[rollOverLegCounter].dataDateTimeFromDB.Date
                                            .AddHours(
                                                optionStrategies[i].instrument.customDayBoundaryTime.Hour)
                                            .AddMinutes(
                                                optionStrategies[i].instrument.customDayBoundaryTime.Minute)
                                            .AddMinutes(1);

                                        expLst.yesterdaySettlement =
                                            optionStrategies[i].rollIntoLegData[rollOverLegCounter].settlementPriceFromDB;

                                        expLst.yesterdaySettlementFilled = true;

                                        char callOrPutOrFutureChar = 'C';

                                        switch (expLst.callPutOrFuture)
                                        {
                                            case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                                callOrPutOrFutureChar = 'C';
                                                break;

                                            case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                                callOrPutOrFutureChar = 'P';
                                                break;

                                            case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                                callOrPutOrFutureChar = 'F';
                                                break;
                                        }

                                        expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                                        //if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                        //{
                                        //    expLst.optionId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idOption;
                                        //    expLst.underlyingFutureId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idUnderlyingContract;

                                        //    expLst.strikePrice = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionStrikePrice;

                                        //    expLst.yearFraction =
                                        //        calcYearFrac(optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].expirationDate,
                                        //                            DateTime.Now);
                                        //}
                                        //else
                                        {
                                            //only adding futures so adding idContract to futureId
                                            expLst.futureId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idContract;
                                        }

                                        //                                     expLst.spreadIdx.Add(i);
                                        //                                     expLst.legIdx.Add(rollOverLegCounter);
                                        //                                     expLst.rowIdx.Add(optionStrategies[i].liveGridRowLoc[rollOverLegCounter]);

                                        //expLst.rollSpreadIdx.Add(i);
                                        //expLst.rollFutureLegIdx.Add(rollOverLegCounter);

                                        optionSpreadExpressionList.Add(expLst);

                                        optionStrategies[i].rollIntoLegData[rollOverLegCounter].optionSpreadExpression = expLst;

                                        expLst.normalSubscriptionRequest = true;
                                    }
                                    else
                                    {

                                        //optionSpreadExpressionList[currentExpressionListCounter].rollSpreadIdx.Add(i);
                                        //optionSpreadExpressionList[currentExpressionListCounter].rollFutureLegIdx.Add(rollOverLegCounter);

                                        optionStrategies[i].rollIntoLegData[rollOverLegCounter].optionSpreadExpression = optionSpreadExpressionList[currentExpressionListCounter];

                                        optionSpreadExpressionList[currentExpressionListCounter].normalSubscriptionRequest = true;
                                    }

                                }

                                rollOverLegCounter++;
                            }
                        }
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

        private void fillSubstituteEODExpressionOptionList()
        {
#if DEBUG
            try
#endif
            {
                for (int i = 0; i < optionStrategies.Length; i++)
                {


                    if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null
                        &&
                        optionStrategies[i].instrument.substituteSymbolEOD)
                    {
                        int numberOfLegs = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                        for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {

                            bool foundCqgSymbol = false;
                            int currentExpressionListCounter = 0;

                            //TSErrorCatch.debugWriteOut(optionStrategies[i].legInfo[legCounter].cqgSymbol);

                            while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                            {
                                //   TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                if (optionStrategies[i].legInfo[legCounter].cqgSubstituteSymbol.CompareTo(
                                    optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                {
                                    foundCqgSymbol = true;
                                    break;
                                }
                                currentExpressionListCounter++;
                            }

                            if (!foundCqgSymbol)
                            {
                                OptionSpreadExpression expLst = new OptionSpreadExpression(
                                    optionStrategies[i].legInfo[legCounter].legContractType,
                                    OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                expLst.cqgSymbol = optionStrategies[i].legInfo[legCounter].cqgSubstituteSymbol;




                                expLst.instrument = optionStrategies[i].instrument;

                                //DateTime currentDate = DateTime.Now.Date;

                                expLst.impliedVolFromSpan =
                                    optionStrategies[i].legData[legCounter].impliedVolFromDB;

                                expLst.previousDateTimeBoundaryStart
                                    = optionStrategies[i].legData[legCounter].dataDateTimeFromDB.Date
                                    .AddHours(
                                        optionStrategies[i].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                        optionStrategies[i].instrument.customDayBoundaryTime.Minute)
                                    .AddMinutes(1);

                                expLst.yesterdaySettlement =
                                    optionStrategies[i].legData[legCounter].settlementPriceFromDB;
                                expLst.yesterdaySettlementFilled = true;



                                char callOrPutOrFutureChar = 'C';

                                switch (expLst.callPutOrFuture)
                                {
                                    case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                        callOrPutOrFutureChar = 'C';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                        callOrPutOrFutureChar = 'P';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                        callOrPutOrFutureChar = 'F';
                                        break;
                                }

                                expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                                if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    expLst.substituteOptionId = optionStrategies[i].legInfo[legCounter].idOption;
                                    expLst.substituteUnderlyingFutureId = optionStrategies[i].legInfo[legCounter].idUnderlyingContract;

                                    expLst.strikePrice = optionStrategies[i].legInfo[legCounter].optionStrikePrice;

                                    expLst.yearFraction =
                                        calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
                                                            DateTime.Now.Date);

                                    expLst.optionMonthInt = optionStrategies[i].legInfo[legCounter].optionMonthInt;

                                    expLst.optionYear = optionStrategies[i].legInfo[legCounter].optionYear;
                                }
                                else
                                {
                                    expLst.substituteFutureId = optionStrategies[i].legInfo[legCounter].idContract;

                                    expLst.futureContractMonthInt = optionStrategies[i].legInfo[legCounter].contractMonthInt;

                                    expLst.futureContractYear = optionStrategies[i].legInfo[legCounter].contractYear;
                                }


                                expLst.substituteSymbolSpreadIdx.Add(i);
                                expLst.substituteSymbolLegIdx.Add(legCounter);
                                expLst.substituteSymbolRowIdx.Add(optionStrategies[i].liveGridRowLoc[legCounter]);

                                //int numberOfContracts = (int)optionStrategies[i].optionStrategyParameters[
                                //                (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                                //expLst.numberOfLotsHeldForContractSummary += numberOfContracts;

                                optionSpreadExpressionList.Add(expLst);

                                optionStrategies[i].legData[legCounter].optionSpreadSymbolSubstituteExpression = expLst;

                                expLst.mainExpressionSubstitutionUsedFor = optionStrategies[i].legData[legCounter].optionSpreadExpression;

                                optionStrategies[i].legData[legCounter].optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                                expLst.substituteSubscriptionRequest = true;
                            }
                            else
                            {
                                //int numberOfContracts = (int)optionStrategies[i].optionStrategyParameters[
                                //                (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                                //optionSpreadExpressionList[currentExpressionListCounter].numberOfLotsHeldForContractSummary += numberOfContracts;

                                optionSpreadExpressionList[currentExpressionListCounter].substituteSymbolSpreadIdx.Add(i);
                                optionSpreadExpressionList[currentExpressionListCounter].substituteSymbolLegIdx.Add(legCounter);
                                optionSpreadExpressionList[currentExpressionListCounter].substituteSymbolRowIdx.Add(
                                    optionStrategies[i].liveGridRowLoc[legCounter]);

                                optionStrategies[i].legData[legCounter].optionSpreadSymbolSubstituteExpression =
                                    optionSpreadExpressionList[currentExpressionListCounter];

                                //optionSpreadExpressionList[currentExpressionListCounter]
                                //    .mainExpressionSubstitutionUsedFor.Add(optionStrategies[i].legData[legCounter].optionSpreadExpression);

                                optionSpreadExpressionList[currentExpressionListCounter].mainExpressionSubstitutionUsedFor = optionStrategies[i].legData[legCounter].optionSpreadExpression;

                                optionStrategies[i].legData[legCounter].optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                                optionSpreadExpressionList[currentExpressionListCounter].substituteSubscriptionRequest = true;
                            }


                        }

                        //for roll over info

                        if (optionStrategies[i].rollIntoLegInfo != null)
                        {

                            int rollOverLegCounter = 0;
                            while (rollOverLegCounter < numberOfLegs)
                            {
                                if (optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {



                                    bool foundCqgSymbol = false;
                                    int currentExpressionListCounter = 0;

                                    //TSErrorCatch.debugWriteOut(optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSymbol);

                                    while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                                    {
                                        //TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                        if (optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSubstituteSymbol.CompareTo(
                                            optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                        {
                                            foundCqgSymbol = true;
                                            break;
                                        }
                                        currentExpressionListCounter++;
                                    }

                                    if (!foundCqgSymbol)
                                    {
                                        OptionSpreadExpression expLst = new OptionSpreadExpression(
                                            optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].legContractType,
                                            OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                        expLst.cqgSymbol = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].cqgSubstituteSymbol;


                                        //expLst.optionMonthInt = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionMonthInt;
                                        //expLst.optionYear = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionYear;

                                        //expLst.underlyingMonthInt = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].contractMonthInt;
                                        //expLst.underlyingYear = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].contractYear;



                                        expLst.instrument = optionStrategies[i].instrument;

                                        expLst.previousDateTimeBoundaryStart
                                            = optionStrategies[i].rollIntoLegData[rollOverLegCounter].dataDateTimeFromDB.Date
                                            .AddHours(
                                                optionStrategies[i].instrument.customDayBoundaryTime.Hour)
                                            .AddMinutes(
                                                optionStrategies[i].instrument.customDayBoundaryTime.Minute)
                                            .AddMinutes(1);

                                        expLst.yesterdaySettlement =
                                            optionStrategies[i].rollIntoLegData[rollOverLegCounter].settlementPriceFromDB;

                                        expLst.yesterdaySettlementFilled = true;

                                        char callOrPutOrFutureChar = 'C';

                                        switch (expLst.callPutOrFuture)
                                        {
                                            case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                                callOrPutOrFutureChar = 'C';
                                                break;

                                            case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                                callOrPutOrFutureChar = 'P';
                                                break;

                                            case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                                callOrPutOrFutureChar = 'F';
                                                break;
                                        }

                                        expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                                        //if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                        //{
                                        //    expLst.optionId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idOption;
                                        //    expLst.underlyingFutureId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idUnderlyingContract;

                                        //    expLst.strikePrice = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].optionStrikePrice;

                                        //    expLst.yearFraction =
                                        //        calcYearFrac(optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].expirationDate,
                                        //                            DateTime.Now);
                                        //}
                                        //else
                                        {
                                            //only adding futures so adding idContract to futureId
                                            expLst.substituteFutureId = optionStrategies[i].rollIntoLegInfo[rollOverLegCounter].idContract;
                                        }

                                        //                                     expLst.spreadIdx.Add(i);
                                        //                                     expLst.legIdx.Add(rollOverLegCounter);
                                        //                                     expLst.rowIdx.Add(optionStrategies[i].liveGridRowLoc[rollOverLegCounter]);

                                        //expLst.rollSpreadIdx.Add(i);
                                        //expLst.rollFutureLegIdx.Add(rollOverLegCounter);

                                        optionSpreadExpressionList.Add(expLst);

                                        optionStrategies[i].rollIntoLegData[rollOverLegCounter].optionSpreadSymbolSubstituteExpression = expLst;

                                        expLst.mainExpressionSubstitutionUsedFor =
                                            optionStrategies[i].legData[rollOverLegCounter].optionSpreadExpression;

                                        expLst.substituteSubscriptionRequest = true;
                                    }
                                    else
                                    {

                                        //optionSpreadExpressionList[currentExpressionListCounter].rollSpreadIdx.Add(i);
                                        //optionSpreadExpressionList[currentExpressionListCounter].rollFutureLegIdx.Add(rollOverLegCounter);

                                        optionStrategies[i].rollIntoLegData[rollOverLegCounter].optionSpreadSymbolSubstituteExpression =
                                            optionSpreadExpressionList[currentExpressionListCounter];

                                        optionSpreadExpressionList[currentExpressionListCounter].mainExpressionSubstitutionUsedFor =
                                            optionStrategies[i].legData[rollOverLegCounter].optionSpreadExpression;

                                        optionSpreadExpressionList[currentExpressionListCounter].substituteSubscriptionRequest = true;
                                    }

                                }

                                rollOverLegCounter++;
                            }
                        }
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

        private void fillADMExpressionOptionList()
        {
#if DEBUG
            try
#endif
            {


                for (int admPositionWebCounter = 0; admPositionWebCounter < sAdmPositionImportWeb.Count; admPositionWebCounter++)
                {


                    //if (sAdmPositionImportWeb[admPositionWebCounter].admLegInfo != null)
                    {
                        //int numberOfLegs = sLiveADMStrategyInfoList[admPositionWebCounter].admLegInfo.Count;

                        //for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                        {

                            bool foundCqgSymbol = false;
                            int currentExpressionListCounter = 0;

                            while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                            {
                                //   TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                if (sAdmPositionImportWeb[admPositionWebCounter].cqgSymbol != null
                                    && sAdmPositionImportWeb[admPositionWebCounter].cqgSymbol.CompareTo(
                                    optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                {

                                    foundCqgSymbol = true;
                                    break;
                                }

                                currentExpressionListCounter++;

                            }

                            if (!foundCqgSymbol)
                            {
                                OptionSpreadExpression expLst = new OptionSpreadExpression(
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.legContractType,
                                    OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                expLst.cqgSymbol = sAdmPositionImportWeb[admPositionWebCounter].cqgSymbol;


                                expLst.instrument = sAdmPositionImportWeb[admPositionWebCounter].instrument;

                                DateTime currentDate = DateTime.Now.Date;

                                expLst.impliedVolFromSpan =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractData.impliedVolFromDB;

                                expLst.previousDateTimeBoundaryStart
                                    = sAdmPositionImportWeb[admPositionWebCounter].contractData.dataDateTimeFromDB.Date
                                    .AddHours(
                                        sAdmPositionImportWeb[admPositionWebCounter].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                       sAdmPositionImportWeb[admPositionWebCounter].instrument.customDayBoundaryTime.Minute)
                                    .AddMinutes(1);

                                expLst.yesterdaySettlement =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractData.settlementPriceFromDB;
                                expLst.yesterdaySettlementFilled = true;

                                char callOrPutOrFutureChar = 'C';

                                switch (expLst.callPutOrFuture)
                                {
                                    case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                        callOrPutOrFutureChar = 'C';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                        callOrPutOrFutureChar = 'P';
                                        break;

                                    case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                        callOrPutOrFutureChar = 'F';
                                        break;
                                }

                                expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                                if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    expLst.optionId =
                                        sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idOption;

                                    expLst.underlyingFutureId =
                                        sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idUnderlyingContract;

                                    expLst.strikePrice =
                                        sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionStrikePrice;

                                    expLst.yearFraction =
                                        calcYearFrac(
                                        sAdmPositionImportWeb[admPositionWebCounter].contractInfo.expirationDate,
                                                            DateTime.Now.Date);

                                    expLst.optionMonthInt = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionMonthInt;

                                    expLst.optionYear = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionYear;
                                }
                                else
                                {
                                    expLst.futureId =
                                        sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idContract;

                                    expLst.futureContractMonthInt = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.contractMonthInt;

                                    expLst.futureContractYear = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.contractYear;
                                }


                                //expLst.yesterdaySettlement = sAdmPositionImportWeb[admPositionWebCounter]
                                //    .contractData.settlementPriceFromDB;

                                //expLst.yesterdaySettlementFilled = true;

                                //expLst.yesterdaySettlement =
                                //    sAdmPositionImportWeb[admPositionWebCounter].contractData.settlementPriceFromDB;
                                //expLst.yesterdaySettlementFilled = true;

                                //expLst.admStrategyIdx.Add(admPositionWebCounter);
                                //expLst.admLegIdx.Add(legCounter);

                                expLst.admPositionImportWebIdx.Add(admPositionWebCounter);

                                expLst.admRowIdx.Add(sAdmPositionImportWeb[admPositionWebCounter].liveADMRowIdx);

                                optionSpreadExpressionList.Add(expLst);

                                sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression = expLst;

                                expLst.normalSubscriptionRequest = true;
                            }
                            else
                            {
                                //optionSpreadExpressionList[currentExpressionListCounter].admStrategyIdx.Add(admPositionWebCounter);
                                //optionSpreadExpressionList[currentExpressionListCounter].admLegIdx.Add(legCounter);
                                //optionSpreadExpressionList[currentExpressionListCounter].admRowIdx.Add(
                                //    sLiveADMStrategyInfoList[admPositionWebCounter].admLegInfo[legCounter].rowIndex);

                                //sLiveADMStrategyInfoList[admPositionWebCounter].admLegInfo[legCounter].optionSpreadExpression = optionSpreadExpressionList[currentExpressionListCounter];

                                optionSpreadExpressionList[currentExpressionListCounter]
                                    .admPositionImportWebIdx.Add(admPositionWebCounter);

                                optionSpreadExpressionList[currentExpressionListCounter].admRowIdx.Add(
                                    sAdmPositionImportWeb[admPositionWebCounter].liveADMRowIdx);

                                sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression =
                                        optionSpreadExpressionList[currentExpressionListCounter];

                                optionSpreadExpressionList[currentExpressionListCounter].normalSubscriptionRequest = true;
                            }




                        }
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

        private void fillSubstitueEODADMExpressionOptionList()
        {
#if DEBUG
            try
#endif
            {


                for (int admPositionWebCounter = 0; admPositionWebCounter < sAdmPositionImportWeb.Count; admPositionWebCounter++)
                {


                    if (sAdmPositionImportWeb[admPositionWebCounter].instrument.substituteSymbolEOD)
                    {

                        bool foundCqgSymbol = false;
                        int currentExpressionListCounter = 0;

                        while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                        {


                            if (sAdmPositionImportWeb[admPositionWebCounter].cqgSubstituteSymbol != null
                                && sAdmPositionImportWeb[admPositionWebCounter].cqgSubstituteSymbol.CompareTo(
                                optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                            {

                                foundCqgSymbol = true;
                                break;
                            }

                            currentExpressionListCounter++;

                        }

                        if (!foundCqgSymbol)
                        {
                            OptionSpreadExpression expLst = new OptionSpreadExpression(
                                sAdmPositionImportWeb[admPositionWebCounter].contractInfo.legContractType,
                                OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                            expLst.cqgSymbol = sAdmPositionImportWeb[admPositionWebCounter].cqgSubstituteSymbol;


                            expLst.instrument = sAdmPositionImportWeb[admPositionWebCounter].instrument;

                            DateTime currentDate = DateTime.Now.Date;

                            expLst.impliedVolFromSpan =
                                sAdmPositionImportWeb[admPositionWebCounter].contractData.impliedVolFromDB;

                            expLst.previousDateTimeBoundaryStart
                                    = sAdmPositionImportWeb[admPositionWebCounter].contractData.dataDateTimeFromDB.Date
                                    .AddHours(
                                        sAdmPositionImportWeb[admPositionWebCounter].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                       sAdmPositionImportWeb[admPositionWebCounter].instrument.customDayBoundaryTime.Minute)
                                    .AddMinutes(1);

                            expLst.yesterdaySettlement =
                                sAdmPositionImportWeb[admPositionWebCounter].contractData.settlementPriceFromDB;
                            expLst.yesterdaySettlementFilled = true;

                            char callOrPutOrFutureChar = 'C';

                            switch (expLst.callPutOrFuture)
                            {
                                case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                    callOrPutOrFutureChar = 'C';
                                    break;

                                case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                    callOrPutOrFutureChar = 'P';
                                    break;

                                case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                    callOrPutOrFutureChar = 'F';
                                    break;
                            }

                            expLst.callPutOrFutureChar = callOrPutOrFutureChar;

                            if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                expLst.substituteOptionId =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idOption;

                                expLst.substituteUnderlyingFutureId =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idUnderlyingContract;

                                expLst.strikePrice =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionStrikePrice;

                                expLst.yearFraction =
                                    calcYearFrac(
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.expirationDate,
                                                        DateTime.Now.Date);

                                expLst.optionMonthInt = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionMonthInt;

                                expLst.optionYear = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.optionYear;
                            }
                            else
                            {
                                expLst.substituteFutureId =
                                    sAdmPositionImportWeb[admPositionWebCounter].contractInfo.idContract;

                                expLst.futureContractMonthInt = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.contractMonthInt;

                                expLst.futureContractYear = sAdmPositionImportWeb[admPositionWebCounter].contractInfo.contractYear;
                            }


                            expLst.yesterdaySettlement = sAdmPositionImportWeb[admPositionWebCounter]
                                .contractData.settlementPriceFromDB;

                            expLst.yesterdaySettlementFilled = true;

                            //expLst.admStrategyIdx.Add(admPositionWebCounter);
                            //expLst.admLegIdx.Add(legCounter);

                            //expLst.admPositionImportWebIdx.Add(admPositionWebCounter);

                            //expLst.admRowIdx.Add(sAdmPositionImportWeb[admPositionWebCounter].liveADMRowIdx);

                            optionSpreadExpressionList.Add(expLst);

                            sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadSymbolSubstituteExpression = expLst;

                            expLst.mainExpressionSubstitutionUsedFor = sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression;

                            sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                            expLst.substituteSubscriptionRequest = true;
                        }
                        else
                        {

                            //optionSpreadExpressionList[currentExpressionListCounter].admPositionImportWebIdx.Add(admPositionWebCounter);

                            //optionSpreadExpressionList[currentExpressionListCounter].admRowIdx.Add(
                            //    sAdmPositionImportWeb[admPositionWebCounter].liveADMRowIdx);

                            optionSpreadExpressionList[currentExpressionListCounter].mainExpressionSubstitutionUsedFor =
                                sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression;

                            sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadSymbolSubstituteExpression =
                                    optionSpreadExpressionList[currentExpressionListCounter];

                            sAdmPositionImportWeb[admPositionWebCounter].contractData.optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                            optionSpreadExpressionList[currentExpressionListCounter].substituteSubscriptionRequest = true;
                        }


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

        private void fillRollIntoLegExpressions()
        {
#if DEBUG
            try
#endif
            {

                for (int instrumentCnt = 0; instrumentCnt < instruments.Length; instrumentCnt++)
                {
                    fillRollIntoLegExpressions(instrumentCnt);
                }


            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        internal void fillRollIntoLegExpressions(int instrumentCnt)
        {
#if DEBUG
            try
#endif
            {
                TMLModelDBQueries btdb = new TMLModelDBQueries();

                int optionStrategyCount_1 = 0;

                bool containsRollOverStrats = false;

                while (optionStrategyCount_1 < optionStrategies.Length)
                {
                    if (optionStrategies[optionStrategyCount_1].indexOfInstrumentInInstrumentsArray == instrumentCnt
                        && optionStrategies[optionStrategyCount_1].rollIntoLegInfo != null)
                    {
                        btdb.connectDB(initializationParms.dbServerName);

                        containsRollOverStrats = true;

                        break;
                    }

                    optionStrategyCount_1++;
                }

                if (containsRollOverStrats)
                {

                    for (int optionStrategyCount = 0; optionStrategyCount < optionStrategies.Length; optionStrategyCount++)
                    {


                        if (optionStrategies[optionStrategyCount].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs] != null
                            && optionStrategies[optionStrategyCount].indexOfInstrumentInInstrumentsArray == instrumentCnt)
                        {

                            //for roll over info

                            if (optionStrategies[optionStrategyCount].rollIntoLegInfo != null)
                            {

                                //find future leg
                                int futureLegIdx = 0;
                                while (futureLegIdx < optionStrategies[optionStrategyCount].rollIntoLegInfo.Length)
                                {
                                    if (optionStrategies[optionStrategyCount].rollIntoLegInfo[futureLegIdx].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                    {
                                        break;
                                    }

                                    futureLegIdx++;
                                }

                                if (optionStrategies[optionStrategyCount].rollIntoLegData[futureLegIdx].optionSpreadExpression.reachedDecisionBar)
                                {



                                    int rollOverLegCounter = 0;
                                    while (rollOverLegCounter < optionStrategies[optionStrategyCount].rollIntoLegInfo.Length)
                                    {
                                        if (optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].legContractType != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                        {
                                            btdb.queryOptionInfoForRollingOption(optionStrategies[optionStrategyCount],
                                                rollOverLegCounter, optionArrayTypes);


                                            bool foundCqgSymbol = false;
                                            int currentExpressionListCounter = 0;

                                            //TSErrorCatch.debugWriteOut(optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSymbol);

                                            if (optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSymbol != null)
                                            {

                                                while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                                                {
                                                    //TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                                    if (optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSymbol.CompareTo(
                                                        optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                                    {
                                                        foundCqgSymbol = true;
                                                        break;
                                                    }
                                                    currentExpressionListCounter++;
                                                }

                                                if (!foundCqgSymbol)
                                                {
                                                    OptionSpreadExpression expLst = new OptionSpreadExpression(
                                                        optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].legContractType,
                                                        OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                                    expLst.cqgSymbol = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSymbol;



                                                    expLst.instrument = optionStrategies[optionStrategyCount].instrument;

                                                    expLst.impliedVolFromSpan =
                                                        optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].impliedVolFromDB;

                                                    expLst.previousDateTimeBoundaryStart
                                                        = optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].dataDateTimeFromDB.Date
                                                        .AddHours(
                                                            optionStrategies[optionStrategyCount].instrument.customDayBoundaryTime.Hour)
                                                        .AddMinutes(
                                                            optionStrategies[optionStrategyCount].instrument.customDayBoundaryTime.Minute)
                                                        .AddMinutes(1);

                                                    expLst.yesterdaySettlement =
                                                        optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].settlementPriceFromDB;
                                                    expLst.yesterdaySettlementFilled = true;

                                                    char callOrPutOrFutureChar = 'C';

                                                    switch (expLst.callPutOrFuture)
                                                    {
                                                        case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                                            callOrPutOrFutureChar = 'C';
                                                            break;

                                                        case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                                            callOrPutOrFutureChar = 'P';
                                                            break;

                                                        case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                                            callOrPutOrFutureChar = 'F';
                                                            break;
                                                    }

                                                    expLst.callPutOrFutureChar = callOrPutOrFutureChar;


                                                    expLst.optionId = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].idOption;
                                                    expLst.underlyingFutureId = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].idUnderlyingContract;

                                                    expLst.strikePrice = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].optionStrikePrice;

                                                    expLst.yearFraction =
                                                        calcYearFrac(optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].expirationDate,
                                                                            DateTime.Now.Date);

                                                    if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                                    {
                                                        expLst.optionMonthInt = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].optionMonthInt;

                                                        expLst.optionYear = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].optionYear;
                                                    }
                                                    else
                                                    {
                                                        expLst.futureContractMonthInt = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].contractMonthInt;

                                                        expLst.futureContractYear = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].contractYear;
                                                    }


                                                    optionSpreadExpressionList.Add(expLst);

                                                    optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression = expLst;

                                                    expLst.normalSubscriptionRequest = true;

                                                    //add all options to futures
                                                    int expressionCounter = 0;

                                                    while (expressionCounter < optionSpreadExpressionList.Count)
                                                    {
                                                        if (optionSpreadExpressionList[expressionCounter].optionExpressionType
                                                                == OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE
                                                           && optionSpreadExpressionList[expressionCounter].callPutOrFuture
                                                                == OPTION_SPREAD_CONTRACT_TYPE.FUTURE
                                                           && expLst.underlyingFutureId == optionSpreadExpressionList[expressionCounter].futureId
                                                            )
                                                        {

                                                            optionSpreadExpressionList[expressionCounter].optionExpressionsThatUseThisFutureAsUnderlying
                                                                .Add(expLst);

                                                            expLst.underlyingFutureExpression = optionSpreadExpressionList[expressionCounter];

                                                        }

                                                        expressionCounter++;
                                                    }
                                                }
                                                else
                                                {

                                                    optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression = optionSpreadExpressionList[currentExpressionListCounter];

                                                }

                                                //substitute symbol code

                                                if (optionStrategies[optionStrategyCount].instrument.substituteSymbolEOD)
                                                {
                                                    optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSubstituteSymbol =
                                                        optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSymbol.Replace(
                                                            optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].instrumentSymbolPreEOD,
                                                            optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].instrumentSymbolEODSubstitute);


                                                    while (currentExpressionListCounter < optionSpreadExpressionList.Count)
                                                    {
                                                        //TSErrorCatch.debugWriteOut("exp " + optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol);

                                                        if (optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSubstituteSymbol.CompareTo(
                                                            optionSpreadExpressionList[currentExpressionListCounter].cqgSymbol) == 0)
                                                        {
                                                            foundCqgSymbol = true;
                                                            break;
                                                        }
                                                        currentExpressionListCounter++;
                                                    }

                                                    if (!foundCqgSymbol)
                                                    {
                                                        OptionSpreadExpression expLst = new OptionSpreadExpression(
                                                            optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].legContractType,
                                                            OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE);

                                                        expLst.cqgSymbol =
                                                            optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].cqgSubstituteSymbol;



                                                        expLst.instrument = optionStrategies[optionStrategyCount].instrument;

                                                        expLst.impliedVolFromSpan =
                                                            optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].impliedVolFromDB;

                                                        expLst.previousDateTimeBoundaryStart
                                                            = optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].dataDateTimeFromDB.Date
                                                            .AddHours(
                                                                optionStrategies[optionStrategyCount].instrument.customDayBoundaryTime.Hour)
                                                            .AddMinutes(
                                                                optionStrategies[optionStrategyCount].instrument.customDayBoundaryTime.Minute)
                                                            .AddMinutes(1);

                                                        expLst.yesterdaySettlement =
                                                            optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].settlementPriceFromDB;
                                                        expLst.yesterdaySettlementFilled = true;

                                                        char callOrPutOrFutureChar = 'C';

                                                        switch (expLst.callPutOrFuture)
                                                        {
                                                            case OPTION_SPREAD_CONTRACT_TYPE.CALL:
                                                                callOrPutOrFutureChar = 'C';
                                                                break;

                                                            case OPTION_SPREAD_CONTRACT_TYPE.PUT:
                                                                callOrPutOrFutureChar = 'P';
                                                                break;

                                                            case OPTION_SPREAD_CONTRACT_TYPE.FUTURE:
                                                                callOrPutOrFutureChar = 'F';
                                                                break;
                                                        }

                                                        expLst.callPutOrFutureChar = callOrPutOrFutureChar;


                                                        expLst.substituteOptionId = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].idOption;
                                                        expLst.substituteUnderlyingFutureId = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].idUnderlyingContract;

                                                        expLst.strikePrice = optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].optionStrikePrice;

                                                        expLst.yearFraction =
                                                            calcYearFrac(optionStrategies[optionStrategyCount].rollIntoLegInfo[rollOverLegCounter].expirationDate,
                                                                                DateTime.Now.Date);

                                                        if (expLst.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                                        {
                                                            expLst.optionMonthInt = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].optionMonthInt;

                                                            expLst.optionYear = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].optionYear;
                                                        }
                                                        else
                                                        {
                                                            expLst.futureContractMonthInt = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].contractMonthInt;

                                                            expLst.futureContractYear = optionStrategies[optionStrategyCount].legInfo[rollOverLegCounter].contractYear;
                                                        }


                                                        optionSpreadExpressionList.Add(expLst);

                                                        optionStrategies[optionStrategyCount]
                                                            .rollIntoLegData[rollOverLegCounter].optionSpreadSymbolSubstituteExpression = expLst;

                                                        expLst.mainExpressionSubstitutionUsedFor =
                                                            optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression;

                                                        optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                                                        expLst.substituteSubscriptionRequest = true;

                                                        ////add all options to futures
                                                        //int expressionCounter = 0;

                                                        //while (expressionCounter < optionSpreadExpressionList.Count)
                                                        //{
                                                        //    if (optionSpreadExpressionList[expressionCounter].optionExpressionType
                                                        //            == OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE
                                                        //       && optionSpreadExpressionList[expressionCounter].callPutOrFuture
                                                        //            == OPTION_SPREAD_CONTRACT_TYPE.FUTURE
                                                        //       && expLst.underlyingFutureId == optionSpreadExpressionList[expressionCounter].futureId
                                                        //        )
                                                        //    {

                                                        //        optionSpreadExpressionList[expressionCounter].optionExpressionsThatUseThisFutureAsUnderlying
                                                        //            .Add(expLst);

                                                        //        expLst.underlyingFutureExpression = optionSpreadExpressionList[expressionCounter];

                                                        //    }

                                                        //    expressionCounter++;
                                                        //}
                                                    }
                                                    else
                                                    {

                                                        optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadSymbolSubstituteExpression =
                                                            optionSpreadExpressionList[currentExpressionListCounter];

                                                        optionSpreadExpressionList[currentExpressionListCounter].mainExpressionSubstitutionUsedFor =
                                                            optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression;

                                                        optionStrategies[optionStrategyCount].rollIntoLegData[rollOverLegCounter].optionSpreadExpression.useSubstituteSymbolAtEOD = true;

                                                        optionSpreadExpressionList[currentExpressionListCounter].substituteSubscriptionRequest = true;

                                                    }


                                                }

                                            }
                                        }

                                        rollOverLegCounter++;
                                    }
                                }
                            }
                        }
                    }

                    btdb.closeDB();

                }



            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public double calcYearFrac(DateTime expirationDate, DateTime currentDateTime)
        {
            double yearFrac = 0;

#if DEBUG
            try
#endif
            {
                TimeSpan spanBetweenCurrentAndExp =
                                   expirationDate - currentDateTime.Date;

                yearFrac = spanBetweenCurrentAndExp.TotalDays / TradingSystemConstants.DAYS_IN_YEAR;

                if (yearFrac < 0)
                {
                    yearFrac = 0;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
            return yearFrac;
        }

        public void callOptionRealTimeData(bool sendOnlyUnsubscribed)
        {
#if DEBUG
            try
#endif
            {
                //dataErrorCheck.dataInError = false;

                if (optionCQGDataManagement != null)
                {
                    optionCQGDataManagement.sendSubscribeRequest(sendOnlyUnsubscribed);

                    sOptionRealtimeMonitor.hideMessageToReconnect();
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void reInitializeCQG()
        {
            optionCQGDataManagement.resetCQG();
        }

        public void fullReConnectCQG()
        {
#if DEBUG
            try
#endif
            {
                //dataErrorCheck.dataInError = false;

                if (optionCQGDataManagement != null)
                {
                    optionCQGDataManagement.shutDownCQGConn();

                    //shutDownOptionSpreadRealtime();

                    //if (m_CEL != null)
                    //{
                    //    if (m_CEL.IsStarted)
                    //        m_CEL.RemoveAllInstruments();

                    //    //m_CEL.Shutdown();
                    //}

                    for (int i = 0; i < optionSpreadExpressionList.Count; i++)
                    {
                        optionSpreadExpressionList[i].cqgInstrument = null;
                    }

                    Thread.Sleep(1000);

                    optionCQGDataManagement.connectCQG();

                    sOptionRealtimeMonitor.displayMessageToReConnect();

                    optionRealtimeStartup.BringToFront();
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void partialReConnectCQG()
        {
#if DEBUG
            try
#endif
            {
                //dataErrorCheck.dataInError = false;

                if (optionCQGDataManagement != null)
                {
                    optionCQGDataManagement.shutDownCQGConn();

                    //for (int i = 0; i < optionSpreadExpressionList.Count; i++)
                    //{
                    //    optionSpreadExpressionList[i].cqgInstrument = null;
                    //}

                    Thread.Sleep(1000);

                    optionCQGDataManagement.connectCQG();
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void resetDataUpdatesWithLatestExpressions()
        {
            if (optionCQGDataManagement != null)
            {
                optionCQGDataManagement.stopDataManagementAndTotalCalcThreads();
            }

            optionSpreadExpressionList.Clear();

            sOptionRealtimeMonitor.displayMessageToReConnect();

            fillAllExpressionsForCQGData();

            //sOptionRealtimeMonitor.setupExpressionListGridView();

            optionCQGDataManagement.resetThreadStopVariables();

            optionCQGDataManagement.setupCalculateModelValuesAndSummarizeTotals();
        }

        public double strikeLevelCalc(double underlyingFutureClose, double optionStrikePrice,
            Instrument instrument)
        {
            double strikeLevel = 0;

#if DEBUG
            try
#endif
            {
                double strikeDifference = underlyingFutureClose - optionStrikePrice;

                strikeLevel = (int)(strikeDifference / instrument.optionStrikeIncrement);

                if (strikeLevel > 10)
                {
                    strikeLevel = 10;
                }
                else if (strikeLevel < -10)
                {
                    strikeLevel = -10;
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return strikeLevel;
        }

        public static double chooseOptionTickSize(double currentOptionPrice, double optionTickSize,
            double secondaryOptionTickSize, double secondaryOptionTickSizeRule)
        {

            if (currentOptionPrice <= secondaryOptionTickSizeRule)
            {
                optionTickSize = secondaryOptionTickSize;
            }

            return optionTickSize;
        }

        public static double chooseOptionTickDisplay(double currentOptionPrice, double optionTickDisplay,
            double secondaryOptionTickDisplay, double secondaryOptionTickSizeRule)
        {

            if (currentOptionPrice <= secondaryOptionTickSizeRule)
            {
                optionTickDisplay = secondaryOptionTickDisplay;
            }

            return optionTickDisplay;
        }

        public static double chooseOptionTickValue(double currentOptionPrice, double optionTickValue,
            double secondaryOptionTickValue, double secondaryOptionTickSizeRule)
        {
            if (currentOptionPrice <= secondaryOptionTickSizeRule)
            {
                optionTickValue = secondaryOptionTickValue;
            }

            return optionTickValue;
        }

        public double getSyntheticClose(OptionStrategy optionStrategy)
        {
            return optionStrategy.syntheticClose;
        }

        public double getCumulativeVolume(OptionStrategy optionStrategy)
        {
            if (optionStrategy.legData != null
                && optionStrategy.legData[optionStrategy.idxOfFutureLeg] != null
                && optionStrategy.legData[optionStrategy.idxOfFutureLeg].optionSpreadExpression.futureBarData != null
                && optionStrategy.legData[optionStrategy.idxOfFutureLeg].optionSpreadExpression.decisionBar != null)
            {
                return optionStrategy.legData[optionStrategy.idxOfFutureLeg].optionSpreadExpression.decisionBar.cumulativeVolume;
            }
            else
            {
                return 0;
            }
        }

        public void runSpreadTotalCalculations(HashSet<int> spreadsUpdated)
        {



            foreach (int spreadIdx in spreadsUpdated)
            {


                int numberOfLegs = (int)optionStrategies[spreadIdx].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs].stateValue;

                double syntheticClose = 0;

                //StringBuilder test = new StringBuilder();

                if (optionStrategies[spreadIdx].legData != null)
                {
                    calculateSyntheticCloseTheoretical(spreadIdx);


                    optionStrategies[spreadIdx].syntheticCloseFilled = false;

                    double tickSize; //= optionStrategies[spreadIdx].instrument.optionTickSize;
                    double tickValue; //= optionStrategies[spreadIdx].instrument.optionTickValue;

                    double spreadTotalPL = 0;
                    //double spreadTotalPLSettleToSettle = 0;
                    double spreadTotalDelta = 0;




                    for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                    {
                        if (optionStrategies[spreadIdx].legInfo[legCounter].legContractType ==
                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            tickSize = optionStrategies[spreadIdx].instrument.tickSize;
                            tickValue = optionStrategies[spreadIdx].instrument.tickValue;


                        }
                        else
                        {
                            if (optionStrategies[spreadIdx].instrument.secondaryOptionTickSize > 0
                                && optionStrategies[spreadIdx].instrument.secondaryOptionTickSize
                                < optionStrategies[spreadIdx].instrument.optionTickSize)
                            {
                                tickSize = optionStrategies[spreadIdx].instrument.secondaryOptionTickSize;
                                tickValue = optionStrategies[spreadIdx].instrument.secondaryOptionTickValue;
                            }
                            else
                            {
                                tickSize = optionStrategies[spreadIdx].instrument.optionTickSize;
                                tickValue = optionStrategies[spreadIdx].instrument.optionTickValue;
                            }
                        }

                        double numberOfContracts = optionStrategies[spreadIdx].optionStrategyParameters[
                                        (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                        double numberOfCfgContracts = optionStrategies[spreadIdx].optionStrategyParameters[
                                        (int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].stateValueParsed[legCounter];

                        syntheticClose += optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.decisionPrice * numberOfCfgContracts;



                        double currentPLChange = (optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.defaultPrice

                                - optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.yesterdaySettlement)
                                / tickSize
                                * tickValue;



                        double currentPLChangeSettlementToSettlement = (optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.settlement

                                - optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.yesterdaySettlement)
                                / tickSize
                                * tickValue;

                        //if (optionStrategies[spreadIdx].instrument.eodAnalysisAtInstrument)
                        {
                            optionStrategies[spreadIdx].legData[legCounter].pAndLDaySettlementToSettlement =
                                currentPLChangeSettlementToSettlement * numberOfContracts;
                        }

                        optionStrategies[spreadIdx].legData[legCounter].pAndLDay = currentPLChange * numberOfContracts;

                        optionStrategies[spreadIdx].legData[legCounter].delta =
                            numberOfContracts * optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.delta;


                        spreadTotalPL += optionStrategies[spreadIdx].legData[legCounter].pAndLDay;

                        //spreadTotalPLSettleToSettle +=
                        //    optionStrategies[spreadIdx].legData[legCounter].pAndLDaySettlementToSettlement;

                        spreadTotalDelta += optionStrategies[spreadIdx].legData[legCounter].delta;

                    }



                    optionStrategies[spreadIdx].liveSpreadTotals.pAndLDay = spreadTotalPL;

                    //optionStrategies[spreadIdx].liveSpreadTotals.pAndLDaySettlementToSettlement = spreadTotalPLSettleToSettle;

                    optionStrategies[spreadIdx].liveSpreadTotals.delta = spreadTotalDelta;

                    optionStrategies[spreadIdx].syntheticCloseFilled = true;
                }

                optionStrategies[spreadIdx].syntheticClose = syntheticClose;

                //TSErrorCatch.debugWriteOut("*** " + test.ToString() + syntheticClose);

                //optionStrategies[spreadIdx].entryRule.Evaluate();

            }
        }



        public void calculateSyntheticCloseTheoretical(int spreadIdx)
        {
            int futureIdx = optionStrategies[spreadIdx].idxOfFutureLeg;

            if (optionStrategies[spreadIdx].legData[futureIdx]
                            .optionSpreadExpression.futureBarData != null)
            {

                for (int i = optionStrategies[spreadIdx].syntheticCloseTheoretical.Count();
                                i < optionStrategies[spreadIdx].legData[futureIdx]
                                .optionSpreadExpression.futureBarData.Count() - 1; i++)
                {
                    if (!optionStrategies[spreadIdx].legData[futureIdx]
                                .optionSpreadExpression.futureBarData[i].errorBar)
                    {

                        TheoreticalBar bar = new TheoreticalBar();

                        bool completedBar = true;

                        //StringBuilder xtest = new StringBuilder();

                        for (int legCounter = 0; legCounter < optionStrategies[spreadIdx].legData.Count(); legCounter++)
                        {

                            double numberOfCfgContracts = optionStrategies[spreadIdx].optionStrategyParameters[
                                                (int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].stateValueParsed[legCounter];

                            if (optionStrategies[spreadIdx].legInfo[legCounter].legContractType ==
                                            OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                bar.barTime = optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.futureBarData[i].barTime;

                                bar.price += optionStrategies[spreadIdx].legData[legCounter]
                                    .optionSpreadExpression.futureBarData[i].close * numberOfCfgContracts;

                                //xtest.Append(
                                //    optionStrategies[spreadIdx].legInfo[legCounter].legContractType + " " +
                                //    optionStrategies[spreadIdx].legData[legCounter]
                                //    .optionSpreadExpression.futureBarData[i].close + " " + numberOfCfgContracts + " ");
                            }
                            else
                            {
                                if (optionStrategies[spreadIdx].legData[legCounter]
                                .optionSpreadExpression.theoreticalOptionDataList != null
                                    &&
                                    optionStrategies[spreadIdx].legData[legCounter]
                                .optionSpreadExpression.theoreticalOptionDataList.Count > i)
                                {
                                    bar.price += optionStrategies[spreadIdx].legData[legCounter]
                                        .optionSpreadExpression.theoreticalOptionDataList[i].price * numberOfCfgContracts;

                                    //xtest.Append(
                                    //optionStrategies[spreadIdx].legInfo[legCounter].legContractType + " " +
                                    //optionStrategies[spreadIdx].legData[legCounter]
                                    //    .optionSpreadExpression.theoreticalOptionDataList[i].price + " " + numberOfCfgContracts + " ");
                                }
                                else
                                {
                                    completedBar = false;
                                }
                            }

                        }

                        //TSErrorCatch.debugWriteOut(xtest.ToString() + " " + bar.barTime + " " + bar.price);

                        if (completedBar)
                            optionStrategies[spreadIdx].syntheticCloseTheoretical.Add(bar);
                    }
                }
            }
        }

        

        static double MyPow(double num, int exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }

            return result;
        }

        public void runADMSpreadTotalCalculations(HashSet<int> admPositionImportWebUpdated)
        {
            foreach (int admPositionImportWebIdx in admPositionImportWebUpdated)
            {
                if (admPositionImportWebIdx < sAdmPositionImportWeb.Count
                    &&
                    sAdmPositionImportWeb[admPositionImportWebIdx].contractData != null
                    &&
                    sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression != null
                    &&
                    DateTime.Now.Date.CompareTo(
                    sAdmPositionImportWeb[admPositionImportWebIdx].contractInfo.expirationDate.Date)
                        <= 0)
                //if (sAdmPositionImportWeb[admPositionImportWebIdx]. != null)
                {
                    //int numberOfLegs = sLiveADMStrategyInfoList[spreadIdx].admLegInfo.Count;

                    //sLiveADMStrategyInfoList[spreadIdx].optionStrategy

                    //optionStrategies[spreadIdx].syntheticCloseFilled = false;

                    double tickSize; //= optionStrategies[spreadIdx].instrument.optionTickSize;
                    double tickValue; //= optionStrategies[spreadIdx].instrument.optionTickValue;

                    //double spreadTotalPL = 0;
                    //double spreadTotalDelta = 0;

                    //for (int legCounter = 0; legCounter < numberOfLegs; legCounter++)
                    {
                        if (sAdmPositionImportWeb[admPositionImportWebIdx].callPutOrFuture ==
                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            tickSize = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.tickSize;
                            tickValue = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.tickValue;
                        }
                        else
                        {
                            if (sAdmPositionImportWeb[admPositionImportWebIdx].instrument.secondaryOptionTickSize > 0
                                && sAdmPositionImportWeb[admPositionImportWebIdx].instrument.secondaryOptionTickSize
                                < sAdmPositionImportWeb[admPositionImportWebIdx].instrument.optionTickSize)
                            {
                                tickSize = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.secondaryOptionTickSize;
                                tickValue = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.secondaryOptionTickValue;
                            }
                            else
                            {
                                tickSize = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.optionTickSize;
                                tickValue = sAdmPositionImportWeb[admPositionImportWebIdx].instrument.optionTickValue;
                            }
                        }

                        double numberOfContracts = sAdmPositionImportWeb[admPositionImportWebIdx].netContractsEditable;

                        int numberOfLongTrans = sAdmPositionImportWeb[admPositionImportWebIdx].transNetLong;

                        int numberOfShortTrans = -sAdmPositionImportWeb[admPositionImportWebIdx].transNetShort;

                        //double numberOfCfgContracts = optionStrategies[spreadIdx].optionStrategyParameters[
                        //                (int)TBL_STRATEGY_STATE_FIELDS.cfgContracts].stateValueParsed[legCounter];

                        //syntheticClose += sLiveADMStrategyInfoList[spreadIdx].admLegInfo[legCounter]
                        //            .optionSpreadExpression.defaultPrice * numberOfContracts;

                        //TSErrorCatch.debugWriteOut(
                        //    optionStrategies[spreadIdx].instrument.name + "  " +
                        //    optionStrategies[spreadIdx].legData[legCounter]
                        //            .optionSpreadExpression.defaultPrice + " # " + numberOfCfgContracts
                        //            + "  leg " + legCounter);

                        double currentPLChange = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.defaultPrice

                                - sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.yesterdaySettlement)
                                / tickSize
                                * tickValue;

                        double currentLongTransPLChange = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.defaultPrice

                                - sAdmPositionImportWeb[admPositionImportWebIdx].transAvgLongPrice)
                                / tickSize
                                * tickValue;

                        double currentShortTransPLChange = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.defaultPrice

                                - sAdmPositionImportWeb[admPositionImportWebIdx].transAvgShortPrice)
                                / tickSize
                                * tickValue;

                        double currentPLChangeSettlementToSettlement = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.settlement

                                - sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.yesterdaySettlement)
                                / tickSize
                                * tickValue;

                        double currentLongTransPLChangeToSettle = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.settlement

                                - sAdmPositionImportWeb[admPositionImportWebIdx].transAvgLongPrice)
                                / tickSize
                                * tickValue;

                        double currentShortTransPLChangeToSettle = (sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.settlement

                                - sAdmPositionImportWeb[admPositionImportWebIdx].transAvgShortPrice)
                                / tickSize
                                * tickValue;

                        //if (sAdmPositionImportWeb[admPositionImportWebIdx].instrument.eodAnalysisAtInstrument)
                        {
                            sAdmPositionImportWeb[admPositionImportWebIdx]
                                .contractData.pAndLDaySettlementToSettlement =
                                    currentPLChangeSettlementToSettlement * numberOfContracts;
                        }

                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLDaySettleOrders =
                            currentLongTransPLChangeToSettle * numberOfLongTrans
                            + currentShortTransPLChangeToSettle * numberOfShortTrans;

                        //*************************
                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLLongSettlementOrders =
                            currentLongTransPLChangeToSettle * numberOfLongTrans;

                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLShortSettlementOrders =
                            currentShortTransPLChangeToSettle * numberOfShortTrans;
                        //*************************


                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLDay =
                            currentPLChange * numberOfContracts;

                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLDayOrders =
                            currentLongTransPLChange * numberOfLongTrans
                            + currentShortTransPLChange * numberOfShortTrans;


                        //if (numberOfLongTrans != 0 || numberOfShortTrans != 0)
                        //{
                        //    TSErrorCatch.debugWriteOut(numberOfLongTrans + " " + numberOfShortTrans);
                        //}

                        //*************************
                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLLongOrders =
                            currentLongTransPLChange * numberOfLongTrans;

                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.pAndLShortOrders =
                            currentShortTransPLChange * numberOfShortTrans;
                        //*************************


                        sAdmPositionImportWeb[admPositionImportWebIdx]
                            .contractData.delta =
                            (numberOfContracts + numberOfLongTrans + numberOfShortTrans)
                            * sAdmPositionImportWeb[admPositionImportWebIdx].contractData
                                    .optionSpreadExpression.delta;


                        //spreadTotalPL += sAdmPositionImportWeb[admPositionImportWebIdx]
                        //    .legData.pAndLDay;

                        //spreadTotalDelta += sAdmPositionImportWeb[admPositionImportWebIdx]
                        //    .legData.delta;

                    }

                    //sLiveADMStrategyInfoList[spreadIdx].liveSpreadADMTotals.pAndLDay = spreadTotalPL;

                    //sLiveADMStrategyInfoList[spreadIdx].liveSpreadADMTotals.delta = spreadTotalDelta;

                    //optionStrategies[spreadIdx].syntheticCloseFilled = true;
                }



            }
        }

        public void runRollStrikeSelection(HashSet<int> spreadsUpdated)
        {
            foreach (int spreadIdx in spreadsUpdated)
            {
                if (optionStrategies[spreadIdx].rollIntoLegData != null)
                {
                    int futureLegIdx = 0;
                    while (futureLegIdx < optionStrategies[spreadIdx].rollIntoLegInfo.Length)
                    {
                        if (optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            break;
                        }

                        futureLegIdx++;
                    }

                //TODO: DEC 28 2015 FUTURE LEG IDX ON ROLL FIX
                    //futureLegIdx = optionStrategies[spreadIdx].idxOfFutureLeg;

                    double futureClose = optionStrategies[spreadIdx].rollIntoLegData[futureLegIdx].optionSpreadExpression.decisionPrice;

                    optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceUsedToCalculateStrikes = futureClose;

                    optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].reachedBarAfterDecisionBar_ForRoll =
                        optionStrategies[spreadIdx].rollIntoLegData[futureLegIdx].optionSpreadExpression.reachedBarAfterDecisionBar;

                    double futurePriceFactorForAllLegs = futureClose / optionStrategies[spreadIdx].instrument.optionStrikeIncrement;

                    if (optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceFactor !=
                            futurePriceFactorForAllLegs)
                    {
                        //double rem = futureClose % optionStrategies[spreadIdx].instrument.optionStrikeIncrement;

                        optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceFactor =
                            futurePriceFactorForAllLegs;

                        //double tickSize; //= optionStrategies[spreadIdx].instrument.optionTickSize;
                        //double tickValue; //= optionStrategies[spreadIdx].instrument.optionTickValue;

                        //double[,] futurePriceRule = new double[2, TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT];

                        bool calculatedFuturePriceFactors = false;

                        for (int legCounter = 0; legCounter < optionStrategies[spreadIdx].rollIntoLegData.Length; legCounter++)
                        {


                            if (optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].legContractType ==
                                    OPTION_SPREAD_CONTRACT_TYPE.CALL
                                ||
                                optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].legContractType ==
                                    OPTION_SPREAD_CONTRACT_TYPE.PUT)
                            {

                                double futurePriceFactor = roundPriceForCallOrPut(
                                    optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].legContractType,
                                    futurePriceFactorForAllLegs, optionStrategies[spreadIdx].instrument.optionStrikeIncrement);

                                int refStartOfFuturePriceCount = -TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2;

                                for (int strikeCount = 0; strikeCount < TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT; strikeCount++)
                                {

                                    //optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].futurePriceReference[strikeCount] = futurePriceFactor
                                    //        + (refStartOfFuturePriceCount * optionStrategies[spreadIdx].instrument.optionStrikeIncrement);

                                    double tempFuturePriceFactor = futurePriceFactor
                                                + (refStartOfFuturePriceCount * optionStrategies[spreadIdx].instrument.optionStrikeIncrement);

                                    optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].optionStrikePriceReference[strikeCount] =
                                            optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].strikeLevelOffsetForRoll
                                            * optionStrategies[spreadIdx].instrument.optionStrikeIncrement

                                            + tempFuturePriceFactor;

                                    if (!calculatedFuturePriceFactors)
                                    {



                                        if (optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].legContractType == (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)
                                        {
                                            //this fills range from call, uses put futures reference minus strike increment

                                            optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceRule[1, strikeCount]
                                                = tempFuturePriceFactor;
                                            //optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].optionStrikePriceReference[strikeCount];

                                            optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceRule[0, strikeCount]
                                                = tempFuturePriceFactor //optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].optionStrikePriceReference[strikeCount]
                                                    - optionStrategies[spreadIdx].instrument.optionStrikeIncrement;
                                        }
                                        else
                                        {
                                            //this fills range for puts, uses put futures reference plus strike increment

                                            optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceRule[0, strikeCount]
                                                = tempFuturePriceFactor;
                                            //optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].optionStrikePriceReference[strikeCount];

                                            optionStrategies[spreadIdx].rollIntoLegInfo[futureLegIdx].futurePriceRule[1, strikeCount]
                                                = tempFuturePriceFactor //optionStrategies[spreadIdx].rollIntoLegInfo[legCounter].optionStrikePriceReference[strikeCount]
                                                    + optionStrategies[spreadIdx].instrument.optionStrikeIncrement;
                                        }

                                    }

                                    refStartOfFuturePriceCount++;
                                }

                                calculatedFuturePriceFactors = true;

                            }
                        }

                        optionStrategies[spreadIdx].rollStrikesUpdated = true;

                    }
                }
            }
        }

        public double roundPriceForCallOrPut(OPTION_SPREAD_CONTRACT_TYPE callOrPut, double futurePriceFactor,
            double optionStrikeIncrement)
        {
            if (callOrPut == OPTION_SPREAD_CONTRACT_TYPE.CALL)
            {
                futurePriceFactor = ((int)futurePriceFactor + 1) * optionStrikeIncrement;
            }
            else if (callOrPut == OPTION_SPREAD_CONTRACT_TYPE.PUT)
            {
                futurePriceFactor = ((int)futurePriceFactor) * optionStrikeIncrement;
            }

            return futurePriceFactor;

        }

        public void runModelContractTotalPLCalcs()
        {
            for (int expressionCnt = 0;
                    expressionCnt < optionSpreadExpressionList.Count(); expressionCnt++)
            {
                if (optionSpreadExpressionList[expressionCnt].optionExpressionType
                    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                {

                    double tickSize;
                    double tickValue;

                    if (optionSpreadExpressionList[expressionCnt]
                            .callPutOrFuture ==
                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        tickSize = optionSpreadExpressionList[expressionCnt]
                            .instrument.tickSize;
                        tickValue = optionSpreadExpressionList[expressionCnt]
                            .instrument.tickValue;


                    }
                    else
                    {
                        if (optionSpreadExpressionList[expressionCnt]
                                .instrument.secondaryOptionTickSize > 0
                            && optionSpreadExpressionList[expressionCnt]
                                .instrument.secondaryOptionTickSize
                            < optionSpreadExpressionList[expressionCnt]
                                .instrument.optionTickSize)
                        {
                            tickSize = optionSpreadExpressionList[expressionCnt]
                                        .instrument.secondaryOptionTickSize;
                            tickValue = optionSpreadExpressionList[expressionCnt]
                                        .instrument.secondaryOptionTickValue;
                        }
                        else
                        {
                            tickSize = optionSpreadExpressionList[expressionCnt]
                                        .instrument.optionTickSize;
                            tickValue = optionSpreadExpressionList[expressionCnt]
                                        .instrument.optionTickValue;
                        }
                    }

                    double currentPLChange = (
                        optionSpreadExpressionList[expressionCnt]
                            .defaultPrice
                        - optionSpreadExpressionList[expressionCnt]
                            .yesterdaySettlement)
                            / tickSize
                            * tickValue;

                    double currentPLChangeToSettlement = (
                        optionSpreadExpressionList[expressionCnt]
                            .settlement
                        - optionSpreadExpressionList[expressionCnt]
                            .yesterdaySettlement)
                            / tickSize
                            * tickValue;


                    //double plChg = currentPLChange *
                    //    optionSpreadExpressionList[expressionCnt].numberOfLotsHeldForContractSummary;

                    optionSpreadExpressionList[expressionCnt].plChgForContractSummary =
                        currentPLChange
                        * optionSpreadExpressionList[expressionCnt].numberOfLotsHeldForContractSummary;

                    optionSpreadExpressionList[expressionCnt].plChgToSettlement =
                        currentPLChangeToSettlement
                        * optionSpreadExpressionList[expressionCnt].numberOfLotsHeldForContractSummary;


                    optionSpreadExpressionList[expressionCnt].deltaChgForContractSummary =
                        optionSpreadExpressionList[expressionCnt].delta *
                        optionSpreadExpressionList[expressionCnt].numberOfLotsHeldForContractSummary;


                    if (optionSpreadExpressionList[expressionCnt]
                        .transactionPriceFilled
                        && optionSpreadExpressionList[expressionCnt].numberOfOrderContracts != 0)
                    {
                        optionSpreadExpressionList[expressionCnt].plChgOrders =
                            optionSpreadExpressionList[expressionCnt].numberOfOrderContracts
                            * (optionSpreadExpressionList[expressionCnt].defaultPrice
                            - optionSpreadExpressionList[expressionCnt].transactionPrice)
                            / tickSize
                            * tickValue;

                        optionSpreadExpressionList[expressionCnt].plChgOrdersToSettlement =
                            optionSpreadExpressionList[expressionCnt].numberOfOrderContracts
                            * (optionSpreadExpressionList[expressionCnt].settlement
                            - optionSpreadExpressionList[expressionCnt].transactionPrice)
                            / tickSize
                            * tickValue;
                    }
                    else
                    {
                        optionSpreadExpressionList[expressionCnt].plChgOrders = 0;

                        optionSpreadExpressionList[expressionCnt].plChgOrdersToSettlement = 0;
                    }



                    //instrumentModelCalcTotals[
                    //    optionSpreadExpressionList[expressionCnt]
                    //    .instrument.idxOfInstrumentInList].pAndLDay
                    //    +=
                    //    optionSpreadExpressionList[expressionCnt].plChgForContractSummary
                    //    + optionSpreadExpressionList[expressionCnt].plChgOrders;


                    //instrumentModelCalcTotals[
                    //    optionSpreadExpressionList[expressionCnt]
                    //    .instrument.idxOfInstrumentInList].delta
                    //    += optionSpreadExpressionList[expressionCnt].deltaChgForContractSummary;



                    //instrumentModelCalcTotals[
                    //    optionSpreadExpressionList[expressionCnt]
                    //    .instrument.idxOfInstrumentInList].pAndLDaySettlementToSettlement
                    //    +=
                    //    optionSpreadExpressionList[expressionCnt].plChgToSettlement
                    //    + optionSpreadExpressionList[expressionCnt].plChgOrdersToSettlement;


                }


            }
        }

        public void runModelAndADMInstrumentTotals()
        {

            /*
             * reset the instrument spread calc totals
             */
            for (int instrumentCnt = 0; instrumentCnt < instrumentModelCalcTotals.Length; instrumentCnt++)
            {
                instrumentModelCalcTotals[instrumentCnt].pAndLDay = 0;
                instrumentModelCalcTotals[instrumentCnt].delta = 0;

                instrumentModelCalcTotals[instrumentCnt].pAndLDaySettlementToSettlement = 0;

                for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {
                    instrumentADMCalcTotals[instrumentCnt, portfolioGroupCnt].pAndLDay = 0;
                    instrumentADMCalcTotals[instrumentCnt, portfolioGroupCnt].delta = 0;

                    instrumentADMCalcTotals[instrumentCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement = 0;
                }

            }

            portfolioSpreadCalcTotals.pAndLDay = 0;
            portfolioSpreadCalcTotals.delta = 0;
            portfolioSpreadCalcTotals.pAndLDaySettlementToSettlement = 0;


            for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
            {
                portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDay = 0;
                portfolioADMSpreadCalcTotals[portfolioGroupCnt].delta = 0;
                portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDaySettlementToSettlement = 0;
            }




            for (int expressionCnt = 0;
                    expressionCnt < optionSpreadExpressionList.Count(); expressionCnt++)
            {
                if (optionSpreadExpressionList[expressionCnt].optionExpressionType
                    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                {


                    instrumentModelCalcTotals[
                        optionSpreadExpressionList[expressionCnt]
                        .instrument.idxOfInstrumentInList].pAndLDay
                        +=
                        optionSpreadExpressionList[expressionCnt].plChgForContractSummary
                        + optionSpreadExpressionList[expressionCnt].plChgOrders;


                    instrumentModelCalcTotals[
                        optionSpreadExpressionList[expressionCnt]
                        .instrument.idxOfInstrumentInList].delta
                        += optionSpreadExpressionList[expressionCnt].deltaChgForContractSummary;



                    instrumentModelCalcTotals[
                        optionSpreadExpressionList[expressionCnt]
                        .instrument.idxOfInstrumentInList].pAndLDaySettlementToSettlement
                        +=
                        optionSpreadExpressionList[expressionCnt].plChgToSettlement
                        + optionSpreadExpressionList[expressionCnt].plChgOrdersToSettlement;


                }


            }



            //for (int strategyCnt = 0; strategyCnt < optionStrategies.Length; strategyCnt++)
            //{
            //    instrumentModelCalcTotals[optionStrategies[strategyCnt].indexOfInstrumentInInstrumentsArray].pAndLDay
            //        += optionStrategies[strategyCnt].liveSpreadTotals.pAndLDay;

            //    instrumentModelCalcTotals[optionStrategies[strategyCnt].indexOfInstrumentInInstrumentsArray].delta
            //        += optionStrategies[strategyCnt].liveSpreadTotals.delta;

            //    instrumentModelCalcTotals[optionStrategies[strategyCnt].indexOfInstrumentInInstrumentsArray].pAndLDaySettlementToSettlement
            //        += optionStrategies[strategyCnt].liveSpreadTotals.pAndLDaySettlementToSettlement;
            //}

            for (int admPosCnt = 0; admPosCnt < admPositionImportWeb.Count; admPosCnt++)
            {
                instrumentADMCalcTotals[admPositionImportWeb[admPosCnt].instrumentArrayIdx,
                    admPositionImportWeb[admPosCnt].acctGroup].pAndLDay
                    += admPositionImportWeb[admPosCnt].contractData.pAndLDay
                        + admPositionImportWeb[admPosCnt].contractData.pAndLDayOrders;

                instrumentADMCalcTotals[admPositionImportWeb[admPosCnt].instrumentArrayIdx,
                    admPositionImportWeb[admPosCnt].acctGroup].delta
                    += admPositionImportWeb[admPosCnt].contractData.delta;

                instrumentADMCalcTotals[admPositionImportWeb[admPosCnt].instrumentArrayIdx,
                    admPositionImportWeb[admPosCnt].acctGroup].pAndLDaySettlementToSettlement
                        += admPositionImportWeb[admPosCnt].contractData.pAndLDaySettlementToSettlement
                            + admPositionImportWeb[admPosCnt].contractData.pAndLDaySettleOrders;
            }


            for (int instCnt = 0; instCnt < instrumentSpreadTotals.Length; instCnt++)
            {
                instrumentSpreadTotals[instCnt].pAndLDay = instrumentModelCalcTotals[instCnt].pAndLDay;
                instrumentSpreadTotals[instCnt].delta = instrumentModelCalcTotals[instCnt].delta;

                instrumentSpreadTotals[instCnt].pAndLDaySettlementToSettlement = instrumentModelCalcTotals[instCnt].pAndLDaySettlementToSettlement;



                portfolioSpreadCalcTotals.pAndLDay += instrumentModelCalcTotals[instCnt].pAndLDay;
                portfolioSpreadCalcTotals.delta += instrumentModelCalcTotals[instCnt].delta;

                portfolioSpreadCalcTotals.pAndLDaySettlementToSettlement += instrumentModelCalcTotals[instCnt].pAndLDaySettlementToSettlement;



                //ADM data
                for (int portfolioGroupCnt = 0; portfolioGroupCnt < portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {
                    instrumentADMSpreadTotals[instCnt, portfolioGroupCnt].pAndLDay 
                        = instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDay;
                    instrumentADMSpreadTotals[instCnt, portfolioGroupCnt].delta 
                        = instrumentADMCalcTotals[instCnt, portfolioGroupCnt].delta;

                    instrumentADMSpreadTotals[instCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement
                        = instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement;



                    instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].pAndLDay
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDay;
                    instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].delta
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].delta;

                    instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].pAndLDaySettlementToSettlement
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement;




                    portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDay 
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDay;
                    portfolioADMSpreadCalcTotals[portfolioGroupCnt].delta
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].delta;

                    portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDaySettlementToSettlement
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement;


                    //the total calcs includes an extra increment in array size to include sum of all portfoliogroups
                    //***************
                    portfolioADMSpreadCalcTotals[portfolioGroupAllocation.Length].pAndLDay
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDay;
                    portfolioADMSpreadCalcTotals[portfolioGroupAllocation.Length].delta
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].delta;

                    portfolioADMSpreadCalcTotals[portfolioGroupAllocation.Length].pAndLDaySettlementToSettlement
                        += instrumentADMCalcTotals[instCnt, portfolioGroupCnt].pAndLDaySettlementToSettlement;
                    //***************


                }

                instrumentADMSpreadTotals[instCnt, portfolioGroupAllocation.Length].pAndLDay
                    = instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].pAndLDay;
                instrumentADMSpreadTotals[instCnt, portfolioGroupAllocation.Length].delta
                    = instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].delta;

                instrumentADMSpreadTotals[instCnt, portfolioGroupAllocation.Length].pAndLDaySettlementToSettlement
                    = instrumentADMCalcTotals[instCnt, portfolioGroupAllocation.Length].pAndLDaySettlementToSettlement;

            }

            portfolioSpreadTotals.pAndLDay = portfolioSpreadCalcTotals.pAndLDay;
            portfolioSpreadTotals.delta = portfolioSpreadCalcTotals.delta;
            portfolioSpreadTotals.pAndLDaySettlementToSettlement = portfolioSpreadCalcTotals.pAndLDaySettlementToSettlement;


            for (int portfolioGroupCnt = 0; portfolioGroupCnt <= portfolioGroupAllocation.Length; portfolioGroupCnt++)
            {
                portfolioADMSpreadTotals[portfolioGroupCnt].pAndLDay 
                    = portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDay;

                portfolioADMSpreadTotals[portfolioGroupCnt].delta 
                    = portfolioADMSpreadCalcTotals[portfolioGroupCnt].delta;

                portfolioADMSpreadTotals[portfolioGroupCnt].pAndLDaySettlementToSettlement 
                    = portfolioADMSpreadCalcTotals[portfolioGroupCnt].pAndLDaySettlementToSettlement;
            }


        }

        public void shutDownOptionSpreadRealtime()
        {
#if DEBUG
            try
#endif
            {

                writeInitializationConfigFile();

                //calcFutEquivOfSyntheticCloseThreadShouldStop = true;

                if (_stageOrdersLibrary != null)
                {
                    _stageOrdersLibrary.shutDownAndLogOff();
                }

                if (sOptionRealtimeMonitor != null)
                {
                    sOptionRealtimeMonitor.cancelBackgroundWorker();
                }

                //Thread.Sleep(TradingSystemConstants.OPTIONREALTIMEREFRESH);

                

                if (optionCQGDataManagement != null)
                {
                    optionCQGDataManagement.stopDataManagementAndTotalCalcThreads();

                    optionCQGDataManagement.shutDownCQGConn();
                }

                int loopCounter = 0;

                while(threadCount > 0 && loopCounter < 10)
                {
                    Thread.Sleep(1000);//TradingSystemConstants.MODEL_CALC_TIME_REFRESH);
                    loopCounter++;
                }

                optionRealtimeStartup.Close();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        void writeInitializationConfigFile()
        {
#if DEBUG
            try
#endif
            {
                SaveOutputFile sof = new SaveOutputFile(TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY);

                sof.createConfigFile(TradingSystemConstants.INITIALIZE_OPTION_REALTIME_FILE_NAME);

                Type configTypes = typeof(INITIALIZATION_CONFIG_VARS);
                Array configNames = Enum.GetNames(configTypes);

                sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.PORTFOLIOGROUP).ToString(),
                    initializationParmsSaved.portfolioGroupName);

                sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME).ToString(),
                    initializationParmsSaved.dbServerName);

                //sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.BROKER).ToString(),
                //    initializationParmsSaved.FIX_Broker_18220);

                //sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.ACCOUNT).ToString(),
                //    initializationParmsSaved.FIX_Acct);

                sof.closeAndSaveFile();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        public void updateEODMonitorDataSettings(bool sendOnlyUnsubscribed)
        {
            Thread updateEODMonitorDataSettingsThread
             = new Thread(new ParameterizedThreadStart(updateEODMonitorDataSettingsRun));
            updateEODMonitorDataSettingsThread.IsBackground = true;
            updateEODMonitorDataSettingsThread.Start(sendOnlyUnsubscribed);
        }

        private void updateEODMonitorDataSettingsRun(Object obj)
        {
            openThread(null, null);

            bool sendOnlyUnsubscribed = (bool)obj;

            fillRollIntoLegExpressions();

            if (realtimeMonitorSettings.eodAnalysis)
            {
                foreach (OptionSpreadExpression ope in optionSpreadExpressionList)
                {
                    if (ope.optionExpressionType != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                        && ope.instrument.substituteSymbolEOD)
                    {
                        optionCQGDataManagement.fillEODSubstitutePrices(ope);
                    }
                }
            }

            sOptionRealtimeMonitor.updateSetupExpressionListGridView();

            optionCQGDataManagement.sendSubscribeRequest(sendOnlyUnsubscribed);

            updatedRealtimeMonitorSettings(null);

           closeThread(null, null);
        }

        public void updatedRealtimeMonitorSettingsThreadRun()
        {
            Thread updatedRealtimeMonitorSettingsThread
             = new Thread(new ParameterizedThreadStart(updatedRealtimeMonitorSettings));
            updatedRealtimeMonitorSettingsThread.IsBackground = true;
            updatedRealtimeMonitorSettingsThread.Start();
        }

        public void updatedRealtimeMonitorSettings(Object obj)
        {
            openThread(null, null);

            sOptionRealtimeMonitor.updateStatusStripOptionMonitor();


            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                optionCQGDataManagement.manageExpressionPriceCalcs(optionSpreadExpressionList[i]);
            }

            closeThread(null, null);
        }

        //public void updateStatusDataFilling()
        //{
        //    int subscribedCount = 0;
        //    for (int i = 0; i < optionSpreadExpressionList.Count; i++)
        //    {
        //        //optionCQGDataManagement.manageExpressionPriceCalcs(optionSpreadExpressionList[i]);
        //        if (optionSpreadExpressionList[i].setSubscriptionLevel)
        //        {
        //            subscribedCount++;
        //        }
        //    }
        //}

        public String rowHeaderLabelCreate(OptionStrategy optionStrategy)
        {
            StringBuilder header = new StringBuilder();
            header.Append(optionStrategy.idStrategy);
            header.Append(" (");
            header.Append(optionStrategy.instrument.CQGsymbol);
            header.Append(") - ");

            header.Append(optionStrategy.instrument
                .customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));

            //header.Append(" ");
            //header.Append(optionBuilderSpreadStructure_local[
            //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
            //    .spreadInfo.strategyIdFromDB); //.optionSpreadConfigName);
            //header.Append(" - ");
            //header.Append(optionBuilderSpreadStructure_local[
            //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
            //    .instrument.customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));

            return header.ToString().Replace('_', ' ');
        }

        //public String rowHeaderLabelCreate(LiveADMStrategyInfo liveADMStrategyInfo)
        //{
        //    StringBuilder header = new StringBuilder();

        //    header.Append(liveADMStrategyInfo.idStrategy);

        //    if (liveADMStrategyInfo.instrument != null)
        //    {

        //        header.Append(" (");
        //        header.Append(liveADMStrategyInfo.instrument.CQGsymbol);
        //        header.Append(") - ");

        //        header.Append(liveADMStrategyInfo.instrument
        //            .customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));

        //        //header.Append(" ");
        //        //header.Append(optionBuilderSpreadStructure_local[
        //        //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
        //        //    .spreadInfo.strategyIdFromDB); //.optionSpreadConfigName);
        //        //header.Append(" - ");
        //        //header.Append(optionBuilderSpreadStructure_local[
        //        //    currentDateContractListMainIdx_local[currentDateContractListIdx].optionSpreadIdx]
        //        //    .instrument.customDayBoundaryTime.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
        //    }

        //    return header.ToString().Replace('_', ' ');
        //}



        internal void fillADMInputWithWebPositions(String[] configFileNames, ImportFileCheck importFileCheck)
        {

            List<ADMPositionImportWeb> sAdmPositionImportWebForImportDisplayTemp = new List<ADMPositionImportWeb>();

            

            //importFileCheck.importingBackedUpSavedFile = importingBackedUpSavedFile;

            sADMDataCommonMethods.readADMDetailedPositionImportWeb(configFileNames,
                        sAdmPositionImportWebForImportDisplayTemp, importFileCheck,
                        instruments);

            if (importFileCheck.importfile)
            {
                sAdmPositionImportWebForImportDisplay = sAdmPositionImportWebForImportDisplayTemp;

                setupAdmWebDetailedDataForDisplay();

                netOutContractsInDetailedWebView();

                //bool isCSVFile = false;

                //for (int fileCounter = 0; fileCounter < configFileNames.Length; fileCounter++)
                //{
                //    if (configFileNames[fileCounter].EndsWith(".csv"))
                //    {
                //        isCSVFile = true;
                //    }
                //}


                if (initializationParms.useCloudDb)
                {
                    setupAdmWebInputDataFromAzure();
                }
                else
                {
                    setupAdmWebInputData();
                }


                

                dateOfADMPositionsFile
                    = sADMDataCommonMethods.getFileDateTimeOfADMPositionsWeb(configFileNames[0]);

            }

            //return importFileCheck.importfile;
        }

        public void setupAdmWebDetailedDataForDisplay()
        {
            //fills instrument data into ADM data for display


            for (int admRowCounter = 0; admRowCounter < admPositionImportWebForImportDisplay.Count; admRowCounter++)
            {
                StringBuilder pOfficPAcct = new StringBuilder();
                pOfficPAcct.Append(admPositionImportWebForImportDisplay[admRowCounter].POFFIC);
                pOfficPAcct.Append(admPositionImportWebForImportDisplay[admRowCounter].PACCT);

                if (portfolioGroupIdxAcctStringHashSet.ContainsKey(pOfficPAcct.ToString()))
                {
                    admPositionImportWebForImportDisplay[admRowCounter].acctGroup = portfolioGroupIdxAcctStringHashSet[pOfficPAcct.ToString()];
                    admPositionImportWebForImportDisplay[admRowCounter].MODEL_OFFICE_ACCT
                        = portfolioGroupAllocation[admPositionImportWebForImportDisplay[admRowCounter].acctGroup].FCM_POFFIC_PACCT;

                }

                int instrumentCnt = 0;
                while (instrumentCnt < instruments.Length)
                {
                    if (admPositionImportWebForImportDisplay[admRowCounter].PFC.CompareTo(
                        instruments[instrumentCnt].admCode) == 0)
                    {
                        admPositionImportWebForImportDisplay[admRowCounter].instrument = instruments[instrumentCnt];

                        admPositionImportWebForImportDisplay[admRowCounter].instrumentArrayIdx = instrumentCnt;

                        aDMDataCommonMethods.subContractType((ADM_Input_Data)admPositionImportWebForImportDisplay[admRowCounter],
                            admPositionImportWebForImportDisplay[admRowCounter].PSUBTY);

                        if (admPositionImportWebForImportDisplay[admRowCounter].isFuture)
                        {
                            admPositionImportWebForImportDisplay[admRowCounter].TradePrice *=
                                instruments[instrumentCnt].admFuturePriceFactor;
                        }
                        else
                        {
                            admPositionImportWebForImportDisplay[admRowCounter].TradePrice *=
                                instruments[instrumentCnt].admOptionPriceFactor;
                        }

                        break;
                    }

                    instrumentCnt++;
                }


            }

        }

        /// <summary>
        /// Nets the out contracts in detailed web view.
        /// </summary>
        private void netOutContractsInDetailedWebView()
        {
            sAdmPositionImportWeb = new List<ADMPositionImportWeb>();

            HashSet<int> indexAlreadyIn = new HashSet<int>();

            for (int i = 0; i < admPositionImportWebForImportDisplay.Count; i++)
            {
                if (!indexAlreadyIn.Contains(i))
                {

                    indexAlreadyIn.Add(i);

                    ADMPositionImportWeb admPositionImportWeb = new ADMPositionImportWeb();

                    sAdmPositionImportWeb.Add(admPositionImportWeb);

                    sADMDataCommonMethods.copyADMPositionImportWeb(admPositionImportWeb, admPositionImportWebForImportDisplay[i]);

                    admPositionImportWeb.LongQuantity = 0;
                    admPositionImportWeb.ShortQuantity = 0;

                    netOutTypeContractsOfWebImport(admPositionImportWeb, admPositionImportWebForImportDisplay[i]);

                    for (int j = 0; j < admPositionImportWebForImportDisplay.Count; j++)
                    {
                        
                        /*
            E = 10^-12

            A==B  ::  ABS(A-B) < E

            A>B  ::  A-B>= +E

            A<B  ::  A-B<= -E

            A>=B  ::  A-B> -E

            A<=B  ::  A-B< +E

            A!=B  ::  ABS(A-B)>= E
            */

                        if (!indexAlreadyIn.Contains(j))
                        {
                            //if (admPositionImportWeb.PCUSIP.CompareTo(
                            //    admPositionImportWebForImportDisplay[j].PCUSIP) == 0)

                            if (admPositionImportWeb.POFFIC.CompareTo(admPositionImportWebForImportDisplay[j].POFFIC) == 0
                                && admPositionImportWeb.PACCT.CompareTo(admPositionImportWebForImportDisplay[j].PACCT) == 0
                                
                                && admPositionImportWeb.PEXCH == admPositionImportWebForImportDisplay[j].PEXCH
                                && admPositionImportWeb.PFC.CompareTo(admPositionImportWebForImportDisplay[j].PFC) == 0
                                && admPositionImportWeb.PCTYM.CompareTo(admPositionImportWebForImportDisplay[j].PCTYM) == 0
                                && admPositionImportWeb.PSUBTY.CompareTo(admPositionImportWebForImportDisplay[j].PSUBTY) == 0
                                && 
                                Math.Abs(admPositionImportWeb.strikeInDecimal - admPositionImportWebForImportDisplay[j].strikeInDecimal) < 0.0000000001 )
                            {
                                indexAlreadyIn.Add(j);

                                netOutTypeContractsOfWebImport(admPositionImportWeb, admPositionImportWebForImportDisplay[j]);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < sAdmPositionImportWeb.Count; i++)
            {
                if (sAdmPositionImportWeb[i].transNetLong != 0)
                {
                    sAdmPositionImportWeb[i].transAvgLongPrice /= sAdmPositionImportWeb[i].transNetLong;
                }

                if (sAdmPositionImportWeb[i].transNetShort != 0)
                {
                    sAdmPositionImportWeb[i].transAvgShortPrice /= sAdmPositionImportWeb[i].transNetShort;
                }

            }
        }

        private void netOutTypeContractsOfWebImport(ADMPositionImportWeb admPositionImportWeb,
            ADMPositionImportWeb consolidateFromPositionImportWeb)
        {
            if (consolidateFromPositionImportWeb.RecordType.Trim().CompareTo("Position") == 0)
            {
                admPositionImportWeb.LongQuantity += consolidateFromPositionImportWeb.LongQuantity;

                admPositionImportWeb.ShortQuantity += consolidateFromPositionImportWeb.ShortQuantity;

                admPositionImportWeb.Net = admPositionImportWeb.LongQuantity -
                    admPositionImportWeb.ShortQuantity;

                admPositionImportWeb.netContractsEditable = admPositionImportWeb.Net;
            }
            else if (consolidateFromPositionImportWeb.RecordType.Trim().CompareTo("Transaction") == 0)
            {

                if (consolidateFromPositionImportWeb.LongQuantity != 0)
                {
                    admPositionImportWeb.transAvgLongPrice += consolidateFromPositionImportWeb.LongQuantity
                        * consolidateFromPositionImportWeb.TradePrice;

                    admPositionImportWeb.transNetLong += (int)consolidateFromPositionImportWeb.LongQuantity;
                }

                if (consolidateFromPositionImportWeb.ShortQuantity != 0)
                {
                    admPositionImportWeb.transAvgShortPrice += consolidateFromPositionImportWeb.ShortQuantity
                        * consolidateFromPositionImportWeb.TradePrice;

                    admPositionImportWeb.transNetShort += (int)consolidateFromPositionImportWeb.ShortQuantity;
                }
            }
        }



        public void displayADMInputWithWebPositions()
        {
            if (sAdmReportWebPositionsForm == null)
            {
                sAdmReportWebPositionsForm = new AdmReportSummaryForm(optionStrategies,
                    instruments, //sADMDataCommonMethods
                    this);
            }

            sAdmReportWebPositionsForm.fillFileDateTimeLabel(dateOfADMPositionsFile);

            //sAdmReportWebPositionsForm.displayADMInputWithWebPositions(this, optionRealtimeMonitor.contractSummaryInstrumentSelectedIdx);

            sAdmReportWebPositionsForm.Show();

            sAdmReportWebPositionsForm.BringToFront();

            sAdmReportWebPositionsForm.setupAdmInputSummaryGrid(admPositionImportWebForImportDisplay, contractSummaryInstrumentSelectedIdx);


        }

        public void highlightAdmReportWebPositionsInADMStrategyInfoList(int stratCounter, int legCounter)
        {
            //if (!liveADMStrategyInfoList[stratCounter].admLegInfo[legCounter].notInADMPositionsWebData)
            //{
            //    int rowInWebPositionsData = findOfADMWebPositionsData(
            //        liveADMStrategyInfoList[stratCounter].admLegInfo[legCounter].aDMPositionImportWeb.cqgSymbol);

            //    if (sAdmReportWebPositionsForm != null)
            //        sAdmReportWebPositionsForm.highlightRow(rowInWebPositionsData, false);
            //}
        }

        internal void setupAdmWebInputData()
        {
            TMLModelDBQueries btdb = new TMLModelDBQueries();

            btdb.connectDB(initializationParms.dbServerName);

            //for (int admRowCounter = 0; admRowCounter < admPositionImportWebForImportDisplay.Count; admRowCounter++)
            //{
            //    fillInRestOfADMWebInputData(admPositionImportWebForImportDisplay[admRowCounter], btdb);
            //}

            for (int admRowCounter = 0; admRowCounter < admPositionImportWeb.Count; admRowCounter++)
            {
                fillInRestOfADMWebInputData(admPositionImportWeb[admRowCounter], btdb);

                //TSErrorCatch.debugWriteOut(admPositionImportWeb[admRowCounter].cqgSymbol);
            }

            btdb.closeDB();
        }

        /*fillInRestOfADMWebInputData used to fill the variables of the ADM data
         * ADMPositionImportWeb admPositionImportWeb_Local: web import data
         * TMLModelDBQueries btdb: database connection
         * 
         * this fills the CQGSymbol that can be used to compare model contracts and ADM contracts
         */
        internal void fillInRestOfADMWebInputData(ADMPositionImportWeb admPositionImportWeb_Local,
            TMLModelDBQueries btdb)//(List<ADMPositionImportWeb> sAdmPositionImportWeb)
        {

            //             TMLModelDBQueries btdb = new TMLModelDBQueries();
            // 
            //             btdb.connectDB(initializationParms.dbServerName);
            // 
            //             for (int admRowCounter = 0; admRowCounter < sAdmPositionImportWeb.Count; admRowCounter++)
            {
                admPositionImportWeb_Local.dateTime = DateTime.Now.Date;



                String dateFormat = "yyyyMM";

                admPositionImportWeb_Local.PCTYM_dateTime = DateTime.ParseExact(
                    admPositionImportWeb_Local.PCTYM, dateFormat, CultureInfo.InvariantCulture);

                //admPositionImportWeb_Local.cqgSymbol =
                //    aDMDataCommonMethods.generateOptionCQGSymbol(admPositionImportWeb_Local,
                //    admPositionImportWeb_Local.PSUBTY,
                //    admPositionImportWeb_Local.PCTYM_dateTime,
                //    instruments[instrumentCounter]);

                //if (optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.spreadStructure]
                //                .stateValueParsed[legCounter] != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)

                admPositionImportWeb_Local.contractInfo.legContractType = admPositionImportWeb_Local.callPutOrFuture;

                if (admPositionImportWeb_Local.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    admPositionImportWeb_Local.contractInfo.optionYear =
                        admPositionImportWeb_Local.PCTYM_dateTime.Year;

                    admPositionImportWeb_Local.contractInfo.optionMonthInt =
                        admPositionImportWeb_Local.PCTYM_dateTime.Month;



                    btdb.queryOptionInfoForADMWebData(admPositionImportWeb_Local, optionArrayTypes);

                    btdb.queryUnderlyingFutureInfo(admPositionImportWeb_Local.contractInfo.idUnderlyingContract,
                            optionArrayTypes, admPositionImportWeb_Local.contractInfo);

                    admPositionImportWeb_Local.cqgSymbol = admPositionImportWeb_Local.contractInfo.cqgSymbol;

                    btdb.queryOptionExpirations(admPositionImportWeb_Local);

                    btdb.queryOptionData(admPositionImportWeb_Local, optionArrayTypes);
                }
                else
                {
                    admPositionImportWeb_Local.contractInfo.contractYear =
                        admPositionImportWeb_Local.PCTYM_dateTime.Year;

                    admPositionImportWeb_Local.contractInfo.contractMonthInt =
                        admPositionImportWeb_Local.PCTYM_dateTime.Month;



                    btdb.queryFutureInfo(admPositionImportWeb_Local,
                            optionArrayTypes);

                    admPositionImportWeb_Local.cqgSymbol = admPositionImportWeb_Local.contractInfo.cqgSymbol;

                    btdb.queryFuturesData(admPositionImportWeb_Local.dateTime,
                        admPositionImportWeb_Local.contractInfo.idContract,
                            optionArrayTypes,
                            admPositionImportWeb_Local.contractData);

                }

                if (admPositionImportWeb_Local.instrument.substituteSymbolEOD)
                {
                    //sets up data for realtime EOD SPAN symbol

                    admPositionImportWeb_Local.contractInfo.useSubstitueSymbolEOD = true;

                    admPositionImportWeb_Local.contractInfo.instrumentSymbolPreEOD =
                        admPositionImportWeb_Local.instrument.instrumentSymbolPreEOD;

                    admPositionImportWeb_Local.contractInfo.instrumentSymbolEODSubstitute =
                        admPositionImportWeb_Local.instrument.instrumentSymbolEOD;

                    if (admPositionImportWeb_Local.cqgSymbol != null)
                    {
                        //admPositionImportWeb_Local.contractInfo.cqgSubstituteSymbol =
                        //    admPositionImportWeb_Local.cqgSymbol.Replace(
                        //        admPositionImportWeb_Local.contractInfo.instrumentSymbolPreEOD,
                        //        admPositionImportWeb_Local.contractInfo.instrumentSymbolEODSubstitute);

                        generateCQGSymbolForEODSubstitution(
                                    admPositionImportWeb_Local.contractInfo, admPositionImportWeb_Local.instrument);

                        admPositionImportWeb_Local.cqgSubstituteSymbol = admPositionImportWeb_Local.contractInfo.cqgSubstituteSymbol;
                    }
                }

            }
        }

        internal void setupAdmWebInputDataFromAzure()
        {
            TMLAzureModelDBQueries btdb = new TMLAzureModelDBQueries();


            //Dictionary<int, DataSet> optionDataSetHashSet = new Dictionary<int, DataSet>();

            //Dictionary<int, DataSet> futureDataSetHashSet = new Dictionary<int, DataSet>();

            //for (int admRowCounter = 0; admRowCounter < admPositionImportWebForImportDisplay.Count; admRowCounter++)
            //{
            //    fillInRestOfADMWebInputData(admPositionImportWebForImportDisplay[admRowCounter], btdb);
            //}

            for (int admRowCounter = 0; admRowCounter < admPositionImportWeb.Count; admRowCounter++)
            {
                fillInRestOfADMWebInputDataFromAzure(admPositionImportWeb[admRowCounter], btdb);

                //TSErrorCatch.debugWriteOut(admPositionImportWeb[admRowCounter].cqgSymbol);
            }

            //btdb.closeDB();
        }


        /*fillInRestOfADMWebInputData used to fill the variables of the ADM data
         * ADMPositionImportWeb admPositionImportWeb_Local: web import data
         * TMLModelDBQueries btdb: database connection
         * 
         * this fills the CQGSymbol that can be used to compare model contracts and ADM contracts
         */
        internal void fillInRestOfADMWebInputDataFromAzure(ADMPositionImportWeb admPositionImportWeb_Local,
            TMLAzureModelDBQueries btdb)//(List<ADMPositionImportWeb> sAdmPositionImportWeb)
        {

            {
                admPositionImportWeb_Local.dateTime = DateTime.Now.Date;

                

                String dateFormat = "yyyyMM";

                admPositionImportWeb_Local.PCTYM_dateTime = DateTime.ParseExact(
                    admPositionImportWeb_Local.PCTYM, dateFormat, CultureInfo.InvariantCulture);


                admPositionImportWeb_Local.contractInfo.legContractType = admPositionImportWeb_Local.callPutOrFuture;

                if (admPositionImportWeb_Local.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    admPositionImportWeb_Local.contractInfo.optionYear =
                        admPositionImportWeb_Local.PCTYM_dateTime.Year;

                    admPositionImportWeb_Local.contractInfo.optionMonthInt =
                        admPositionImportWeb_Local.PCTYM_dateTime.Month;



                    //btdb.queryOptionInfoForADMWebData(admPositionImportWeb_Local, optionArrayTypes);


                    String keyString = getOptionIdHashSetKeyString(admPositionImportWeb_Local.contractInfo.optionMonthInt,
                                    admPositionImportWeb_Local.contractInfo.optionYear,
                                    admPositionImportWeb_Local.instrument.idInstrument,
                                    admPositionImportWeb_Local.PSUBTY,
                                    admPositionImportWeb_Local.strikeInDecimal);

                    int idOption = 0;

                    if (optionIdFromInfo.ContainsKey(keyString))
                    {
                        idOption = optionIdFromInfo[keyString];
                    }
                    else
                    {
                        idOption = btdb.queryOptionIDFromInfo(admPositionImportWeb_Local);

                        optionIdFromInfo.Add(keyString.ToString(), idOption);
                    }



                    admPositionImportWeb_Local.contractInfo.idOption = idOption;

                    btdb.queryOptionInfoAndDataFromCloud(
                                   admPositionImportWeb_Local.contractInfo.idOption,
                                   admPositionImportWeb_Local.instrument.idInstrument,
                                   admPositionImportWeb_Local.contractInfo,
                                   admPositionImportWeb_Local.contractData,
                                   initializationParms.modelDateTime,
                                   optionArrayTypes,
                                   optionDataSetHashSet);


                    //btdb.queryUnderlyingFutureInfo(admPositionImportWeb_Local.contractInfo.idUnderlyingContract,
                    //        optionArrayTypes, admPositionImportWeb_Local.contractInfo);

                    btdb.queryFutureInfoAndDataFromCloud(
                        admPositionImportWeb_Local.contractInfo.idUnderlyingContract,
                        admPositionImportWeb_Local.contractInfo,
                        admPositionImportWeb_Local.contractData,
                        initializationParms.modelDateTime,
                        optionArrayTypes,
                        futureDataSetHashSet, true, this, futureIdFromInfo);

                    admPositionImportWeb_Local.cqgSymbol = admPositionImportWeb_Local.contractInfo.cqgSymbol;

                    //btdb.queryOptionExpirations(admPositionImportWeb_Local);

                    //btdb.queryOptionData(admPositionImportWeb_Local, optionArrayTypes);

                   
                }
                else // is a future contract
                {
                    admPositionImportWeb_Local.contractInfo.contractYear =
                        admPositionImportWeb_Local.PCTYM_dateTime.Year;

                    admPositionImportWeb_Local.contractInfo.contractMonthInt =
                        admPositionImportWeb_Local.PCTYM_dateTime.Month;




                    String keyString = getFutureContractIdHashSetKeyString(admPositionImportWeb_Local.contractInfo.contractMonthInt,
                                    admPositionImportWeb_Local.contractInfo.contractYear,
                                    admPositionImportWeb_Local.instrument.idInstrument);


                    int idContract = 0;

                    if (futureIdFromInfo.ContainsKey(keyString))
                    {
                        idContract = futureIdFromInfo[keyString];
                    }
                    else
                    {
                        idContract = btdb.queryFutureContractId(admPositionImportWeb_Local);
                    }


                    //int idContract = btdb.queryFutureContractId(admPositionImportWeb_Local);

                    admPositionImportWeb_Local.contractInfo.idContract = idContract;

                    btdb.queryFutureInfoAndDataFromCloud(
                                    admPositionImportWeb_Local.contractInfo.idContract,
                                    admPositionImportWeb_Local.contractInfo,
                                    admPositionImportWeb_Local.contractData,
                                    admPositionImportWeb_Local.dateTime,
                                    optionArrayTypes,
                                    futureDataSetHashSet, false, this, futureIdFromInfo);

                    admPositionImportWeb_Local.cqgSymbol = admPositionImportWeb_Local.contractInfo.cqgSymbol;


                    //btdb.queryFuturesData(admPositionImportWeb_Local.dateTime,
                    //    admPositionImportWeb_Local.contractInfo.idContract,
                    //        optionArrayTypes,
                    //        admPositionImportWeb_Local.contractData);



                }

                if (admPositionImportWeb_Local.instrument.substituteSymbolEOD)
                {
                    //sets up data for realtime EOD SPAN symbol

                    admPositionImportWeb_Local.contractInfo.useSubstitueSymbolEOD = true;

                    admPositionImportWeb_Local.contractInfo.instrumentSymbolPreEOD =
                        admPositionImportWeb_Local.instrument.instrumentSymbolPreEOD;

                    admPositionImportWeb_Local.contractInfo.instrumentSymbolEODSubstitute =
                        admPositionImportWeb_Local.instrument.instrumentSymbolEOD;

                    if (admPositionImportWeb_Local.cqgSymbol != null)
                    {
                        //admPositionImportWeb_Local.contractInfo.cqgSubstituteSymbol =
                        //    admPositionImportWeb_Local.cqgSymbol.Replace(
                        //        admPositionImportWeb_Local.contractInfo.instrumentSymbolPreEOD,
                        //        admPositionImportWeb_Local.contractInfo.instrumentSymbolEODSubstitute);

                        generateCQGSymbolForEODSubstitution(
                                    admPositionImportWeb_Local.contractInfo, admPositionImportWeb_Local.instrument);

                        admPositionImportWeb_Local.cqgSubstituteSymbol = admPositionImportWeb_Local.contractInfo.cqgSubstituteSymbol;
                    }
                }

            }
        }

        /// <summary>
        /// Fills the adm strategy objects from saved files.
        /// </summary>
        public void fillADMStrategyObjectsFromSavedFiles()
        {
            String[] nameOfADMFile = new String[1];
            nameOfADMFile[0] = sADMDataCommonMethods.getNameOfADMPositionImportWebStored();

            ImportFileCheck importFileCheck = new ImportFileCheck();

            importFileCheck.importingBackedUpSavedFile = true;

            fillADMInputWithWebPositions(nameOfADMFile, importFileCheck);

            //readSavedADMStrategyObjects();

            //readADMExludeContractFile();

            //setupAdmWebInputData();

            //fillADMStrategyObjects();
        }



        /// <summary>
        /// Reads the adm exlude contract file.
        /// </summary>
        public void readADMExludeContractFile()
        {
            //String nameOfADMFile = sADMDataCommonMethods.getNameOfADMPositionImportWebStored();

            //admPositionImportWebForImportDisplay
            //            = sADMDataCommonMethods.readADMDetailedPositionImportWeb(nameOfADMFile);

            sADMDataCommonMethods.readADMExcludeContractInfo(this);
        }

        

        public int findOfADMWebPositionsData(String cqgSymbol)
        {
            int rowCounter = 0;

            while (rowCounter < sAdmPositionImportWeb.Count)
            {
                if (sAdmPositionImportWeb[rowCounter].cqgSymbol.CompareTo(cqgSymbol) == 0)
                {
                    return rowCounter;
                }

                rowCounter++;
            }

            return -1;
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adm web positions and model comparison. </summary>
        ///
        /// <remarks>   Steve Pickering, 8/12/2013. </remarks>
        ///
        /// <param name="strategyCount">    LiveADMStrategy strategy count. </param>
        /// <param name="legIdx">           Zero-based index of the leg. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void admWebPositionsAndModelComparison(int strategyCount,
            int legIdx)
        {
            //search through strategies and find futures leg



            //if (sLiveADMStrategyInfoList[strategyCount].optionStrategy != null)
            //{
            //    if (sLiveADMStrategyInfoList[strategyCount].admLegInfo[legIdx].aDMPositionImportWeb.callPutOrFuture
            //        == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            //    {

            //        //TSErrorCatch.debugWriteOut("test");

            //        OptionStrategy optionStrategy = sLiveADMStrategyInfoList[strategyCount].optionStrategy;

            //        int modelLots = 0;

            //        int stratLegCount = 0;

            //        while (stratLegCount < optionStrategy.legInfo.Length)
            //        {
            //            if (optionStrategy.legInfo[stratLegCount].legContractType
            //                == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            //            {
            //                modelLots += (int)optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition]
            //                    .stateValueParsed[stratLegCount];


            //            }

            //            stratLegCount++;
            //        }

            //        sLiveADMStrategyInfoList[strategyCount].admLegInfo[legIdx].numberOfModelContracts = modelLots;
            //    }
            //    //else
            //    //{

            //    //}
            //}
        }

        public void writeIntradayRealtimeStateToDatabase(OptionStrategy optionStrategy)
        {
            if (initializationParms.useCloudDb)
            {
                Thread writeStateToDbThread = new Thread(new ParameterizedThreadStart(writeIntradayRealtimeStateToDatabaseThreadAzure));
                writeStateToDbThread.IsBackground = true;
                writeStateToDbThread.Start(optionStrategy);
            }
            else
            {
                //Thread writeStateToDbThread = new Thread(new ParameterizedThreadStart(writeIntradayRealtimeStateToDatabaseThread));
                //writeStateToDbThread.IsBackground = true;
                writeIntradayRealtimeStateToDatabaseLocalDB(optionStrategy);
            }
        }

        public DateTime writeIntradayRealtimeStateToDatabaseLocalDB(OptionStrategy optionStrategy)
        {


            TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            RealtimeSystemResults[] realtimeSystemResultsList = new RealtimeSystemResults[1];

            int optionSpreadCounter = 0;

            //for (int optionSpreadCounter = 0; optionSpreadCounter < optionStrategies.Length; optionSpreadCounter++)
            {
                realtimeSystemResultsList[optionSpreadCounter] = new RealtimeSystemResults();

                realtimeSystemResultsList[optionSpreadCounter].idPortfoliogroup = optionStrategy.idPortfoliogroup;

                realtimeSystemResultsList[optionSpreadCounter].idStrategy = optionStrategy.idStrategy;

                realtimeSystemResultsList[optionSpreadCounter].date = DateTime.Now;
                //optionStrategy.legData[legCounter].optionSpreadExpression.lastTimeUpdated;

                if (optionStrategy.entryRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategy.entryRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategy.entryRule.Evaluate());
                }

                if (optionStrategy.exitRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategy.exitRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategy.exitRule.Evaluate());
                }

                int totalLegs = optionStrategy.legData.Length;

                //bool dateTimeStampUpdated = false;

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            getSyntheticClose(optionStrategy),
                            optionStrategy.instrument.tickSize,
                            optionStrategy.instrument.optionTickSize,
                            optionStrategy.instrument.secondaryOptionTickSize));

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(";");


                for (int legCounter = 0; legCounter < optionStrategy.legData.Length; legCounter++)
                {
                    if (optionStrategy.legData[legCounter]
                                        .optionSpreadExpression != null)
                    {
                        //                     if (!dateTimeStampUpdated)
                        //                     {
                        //                         realtimeSystemResultsList[optionSpreadCounter].date =
                        //                             optionStrategy.legData[legCounter].optionSpreadExpression.lastTimeUpdated;
                        // 
                        //                         dateTimeStampUpdated = true;
                        //                     }


                        realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                            optionStrategy.legData[legCounter]
                                        .optionSpreadExpression.decisionPrice);
                        realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");


                        realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(
                            Math.Round(optionStrategy.legData[legCounter].pAndLDay, 2).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(
                            Math.Round(optionStrategy.legData[legCounter].delta, 2).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.defaultPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.defaultPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.theoreticalOptionPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.theoreticalOptionPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(
                            Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.impliedVolFromSpan, 4).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(
                            Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.settlementImpliedVol, 4).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.settlement, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.settlement,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        //    ConversionAndFormatting.roundToSmallestIncrement(
                        //        getSyntheticClose(optionStrategy),
                        //        optionStrategy.instrument.tickSize,
                        //        optionStrategy.instrument.optionTickSize, 
                        //        optionStrategy.instrument.secondaryOptionTickSize));
                        //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.transactionPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.transactionPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].transTime.Append(
                            optionStrategy.legData[legCounter].optionSpreadExpression.transactionPriceTime
                                .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                        realtimeSystemResultsList[optionSpreadCounter].transTime.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(
                        //    optionStrategy.legData[legCounter].optionSpreadExpression.transactionPriceTime
                        //        .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                        //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(",");
                    }
                }
            }


            DateTime returnDateTime = tmlModelDBQueries.updateIntradaySystemResults(realtimeSystemResultsList, 1);

            return returnDateTime;
        }

        public void writeIntradayRealtimeStateToDatabaseThreadAzure(Object obj)
        {
            openThread(null, null);

            OptionStrategy optionStrategy = (OptionStrategy)obj;

            //TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();
            //TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            //tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            RealtimeSystemResults[] realtimeSystemResultsList = new RealtimeSystemResults[1];

            int optionSpreadCounter = 0;

            //for (int optionSpreadCounter = 0; optionSpreadCounter < optionStrategies.Length; optionSpreadCounter++)
            {
                realtimeSystemResultsList[optionSpreadCounter] = new RealtimeSystemResults();

                realtimeSystemResultsList[optionSpreadCounter].idPortfoliogroup = optionStrategy.idPortfoliogroup;

                realtimeSystemResultsList[optionSpreadCounter].idStrategy = optionStrategy.idStrategy;

                realtimeSystemResultsList[optionSpreadCounter].date = DateTime.Now;
                //optionStrategy.legData[legCounter].optionSpreadExpression.lastTimeUpdated;

                if (optionStrategy.entryRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategy.entryRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategy.entryRule.Evaluate());
                }

                if (optionStrategy.exitRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategy.exitRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategy.exitRule.Evaluate());
                }

                int totalLegs = optionStrategy.legData.Length;

                //bool dateTimeStampUpdated = false;

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            getSyntheticClose(optionStrategy),
                            optionStrategy.instrument.tickSize,
                            optionStrategy.instrument.optionTickSize,
                            optionStrategy.instrument.secondaryOptionTickSize));

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(";");


                for (int legCounter = 0; legCounter < optionStrategy.legData.Length; legCounter++)
                {
                    if (optionStrategy.legData[legCounter]
                                        .optionSpreadExpression != null)
                    {
                        //                     if (!dateTimeStampUpdated)
                        //                     {
                        //                         realtimeSystemResultsList[optionSpreadCounter].date =
                        //                             optionStrategy.legData[legCounter].optionSpreadExpression.lastTimeUpdated;
                        // 
                        //                         dateTimeStampUpdated = true;
                        //                     }


                        realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                            optionStrategy.legData[legCounter]
                                        .optionSpreadExpression.decisionPrice);
                        realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");


                        realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(
                            Math.Round(optionStrategy.legData[legCounter].pAndLDay, 2).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(
                            Math.Round(optionStrategy.legData[legCounter].delta, 2).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.defaultPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.defaultPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.theoreticalOptionPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.theoreticalOptionPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(
                            Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.impliedVolFromSpan, 4).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(
                            Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.settlementImpliedVol, 4).ToString());
                        realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.settlement, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.settlement,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        //    ConversionAndFormatting.roundToSmallestIncrement(
                        //        getSyntheticClose(optionStrategy),
                        //        optionStrategy.instrument.tickSize,
                        //        optionStrategy.instrument.optionTickSize, 
                        //        optionStrategy.instrument.secondaryOptionTickSize));
                        //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                        //    Math.Round(optionStrategy.legData[legCounter].optionSpreadExpression.transactionPrice, 2).ToString());
                        //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                            ConversionAndFormatting.roundToSmallestIncrement(
                                optionStrategy.legData[legCounter].optionSpreadExpression.transactionPrice,
                                optionStrategy.instrument.tickSize,
                                optionStrategy.instrument.optionTickSize,
                                optionStrategy.instrument.secondaryOptionTickSize));
                        realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                        realtimeSystemResultsList[optionSpreadCounter].transTime.Append(
                            optionStrategy.legData[legCounter].optionSpreadExpression.transactionPriceTime
                                .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                        realtimeSystemResultsList[optionSpreadCounter].transTime.Append(",");

                        //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(
                        //    optionStrategy.legData[legCounter].optionSpreadExpression.transactionPriceTime
                        //        .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                        //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(",");
                    }
                }
            }


            TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();

            //DateTime returnDateTime = 
                tmlAzureModelDBQueries.updateIntradaySystemResults(realtimeSystemResultsList);

            //return returnDateTime;

            closeThread(null, null);
        }

        //private void runUpdateIntradaySystemResults(Object obj)
        //{
        //    TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();

        //    RealtimeSystemResults[] realtimeSystemResultsList = (RealtimeSystemResults[])obj;

        //    tmlAzureModelDBQueries.updateIntradaySystemResults(realtimeSystemResultsList);
        //}

        public DateTime writeRealtimeStateToDatabase()
        {


            TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            RealtimeSystemResults[] realtimeSystemResultsList = new RealtimeSystemResults[optionStrategies.Length];

            for (int optionSpreadCounter = 0; optionSpreadCounter < optionStrategies.Length; optionSpreadCounter++)
            {
                realtimeSystemResultsList[optionSpreadCounter] = new RealtimeSystemResults();

                realtimeSystemResultsList[optionSpreadCounter].idPortfoliogroup = optionStrategies[optionSpreadCounter].idPortfoliogroup;

                realtimeSystemResultsList[optionSpreadCounter].idStrategy = optionStrategies[optionSpreadCounter].idStrategy;

                realtimeSystemResultsList[optionSpreadCounter].date = DateTime.Now;
                //optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.lastTimeUpdated;

                if (optionStrategies[optionSpreadCounter].entryRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategies[optionSpreadCounter].entryRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategies[optionSpreadCounter].entryRule.Evaluate());


                }

                if (optionStrategies[optionSpreadCounter].exitRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategies[optionSpreadCounter].exitRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategies[optionSpreadCounter].exitRule.Evaluate());
                }

                int totalLegs = optionStrategies[optionSpreadCounter].legData.Length;

                //bool dateTimeStampUpdated = false;

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            getSyntheticClose(optionStrategies[optionSpreadCounter]),
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(";");


                for (int legCounter = 0; legCounter < optionStrategies[optionSpreadCounter].legData.Length; legCounter++)
                {
                    //                     if (!dateTimeStampUpdated)
                    //                     {
                    //                         realtimeSystemResultsList[optionSpreadCounter].date =
                    //                             optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.lastTimeUpdated;
                    // 
                    //                         dateTimeStampUpdated = true;
                    //                     }


                    realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        optionStrategies[optionSpreadCounter].legData[legCounter]
                                    .optionSpreadExpression.decisionPrice);
                    realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");



                    realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].pAndLDay, 2).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].delta, 2).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.defaultPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.defaultPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.theoreticalOptionPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.theoreticalOptionPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.impliedVolFromSpan, 4).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlementImpliedVol, 4).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlement, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlement,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                    //    ConversionAndFormatting.roundToSmallestIncrement(
                    //        getSyntheticClose(optionStrategies[optionSpreadCounter]),
                    //        optionStrategies[optionSpreadCounter].instrument.tickSize,
                    //        optionStrategies[optionSpreadCounter].instrument.optionTickSize, 
                    //        optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].transTime.Append(
                        optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPriceTime
                            .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                    realtimeSystemResultsList[optionSpreadCounter].transTime.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(
                    //    optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPriceTime
                    //        .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                    //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(",");
                }
            }


            DateTime returnDateTime = tmlModelDBQueries.updateEndOfDaySystemResults(realtimeSystemResultsList, 2);

            //UPDATE MODEL PL TO DB

            List<RealtimeSystemPLResults> realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                if (optionSpreadExpressionList[i].optionExpressionType
                    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                    &&
                        (optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0
                        ||
                        optionSpreadExpressionList[i].numberOfOrderContracts != 0)
                    )
                {
                    RealtimeSystemPLResults realtimeSystemPLResults = new RealtimeSystemPLResults();
                    realtimeSystemPLResultsList.Add(realtimeSystemPLResults);

                    realtimeSystemPLResults.fcmData = 0;

                    realtimeSystemPLResults.idPortfoliogroup = initializationParms.idPortfolioGroup;
                    realtimeSystemPLResults.contract = optionSpreadExpressionList[i].cqgSymbol;
                    realtimeSystemPLResults.strikePrice = optionSpreadExpressionList[i].strikePrice;
                    realtimeSystemPLResults.idInstrument = optionSpreadExpressionList[i].instrument.idInstrument;
                    realtimeSystemPLResults.date = DateTime.Now;

                    realtimeSystemPLResults.plDayChg = optionSpreadExpressionList[i].plChgForContractSummary;
                    realtimeSystemPLResults.plLongOrderChg = optionSpreadExpressionList[i].plChgOrders;

                    realtimeSystemPLResults.plSettleDayChg = optionSpreadExpressionList[i].plChgToSettlement;
                    realtimeSystemPLResults.plLongSettleOrderChg = optionSpreadExpressionList[i].plChgOrdersToSettlement;

                    realtimeSystemPLResults.totalQty = optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary;
                    realtimeSystemPLResults.longOrders = optionSpreadExpressionList[i].numberOfOrderContracts;

                    realtimeSystemPLResults.delta = optionSpreadExpressionList[i].deltaChgForContractSummary;
                    realtimeSystemPLResults.defaultPrice = optionSpreadExpressionList[i].defaultPrice;
                    realtimeSystemPLResults.theoreticalPrice = optionSpreadExpressionList[i].theoreticalOptionPrice;
                    realtimeSystemPLResults.spanImpliedVol = optionSpreadExpressionList[i].impliedVolFromSpan;
                    realtimeSystemPLResults.settlementImpliedVol = optionSpreadExpressionList[i].settlementImpliedVol;
                    realtimeSystemPLResults.impliedVol = optionSpreadExpressionList[i].impliedVol;
                    realtimeSystemPLResults.bid = optionSpreadExpressionList[i].bid;
                    realtimeSystemPLResults.ask = optionSpreadExpressionList[i].ask;
                    realtimeSystemPLResults.last = optionSpreadExpressionList[i].trade;
                    realtimeSystemPLResults.settle = optionSpreadExpressionList[i].settlement;
                    realtimeSystemPLResults.yesterdaySettle = optionSpreadExpressionList[i].yesterdaySettlement;

                    if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        realtimeSystemPLResults.idContract = optionSpreadExpressionList[i].futureId;
                    }
                    else
                    {
                        realtimeSystemPLResults.idContract = optionSpreadExpressionList[i].underlyingFutureId;
                    }
                    realtimeSystemPLResults.idOption = optionSpreadExpressionList[i].optionId;
                    realtimeSystemPLResults.callOrPutOrFutureChar = optionSpreadExpressionList[i].callPutOrFutureChar;
                }
            }

            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                tmlModelDBQueries.updateEndOfDaySystemPLResults(realtimeSystemPLResultsList);
            }

            //UPDATE ADM PL TO DB

            realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            //List<ADMPositionImportWeb> admPositionImportWeb = admPositionImportWeb;

            for (int i = 0; i < admPositionImportWeb.Count; i++)
            {
                //if (admPositionImportWeb[i].optionExpressionType
                //    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                //    && optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0)
                {
                    RealtimeSystemPLResults realtimeSystemPLResults = new RealtimeSystemPLResults();
                    realtimeSystemPLResultsList.Add(realtimeSystemPLResults);

                    realtimeSystemPLResults.fcmData = 1;

                    realtimeSystemPLResults.idPortfoliogroup = initializationParms.idPortfolioGroup;
                    realtimeSystemPLResults.contract = admPositionImportWeb[i].cqgSymbol;
                    realtimeSystemPLResults.strikePrice = admPositionImportWeb[i].strikeInDecimal;
                    realtimeSystemPLResults.idInstrument = admPositionImportWeb[i].instrument.idInstrument;
                    realtimeSystemPLResults.date = DateTime.Now;

                    realtimeSystemPLResults.plDayChg = admPositionImportWeb[i].contractData.pAndLDay;
                    realtimeSystemPLResults.plLongOrderChg = admPositionImportWeb[i].contractData.pAndLLongOrders;
                    realtimeSystemPLResults.plShortOrderChg = admPositionImportWeb[i].contractData.pAndLShortOrders;

                    realtimeSystemPLResults.plSettleDayChg = admPositionImportWeb[i].contractData.pAndLDaySettlementToSettlement;
                    realtimeSystemPLResults.plLongSettleOrderChg = admPositionImportWeb[i].contractData.pAndLLongSettlementOrders;
                    realtimeSystemPLResults.plShortSettleOrderChg = admPositionImportWeb[i].contractData.pAndLShortSettlementOrders;

                    realtimeSystemPLResults.totalQty = (int)admPositionImportWeb[i].Net;
                    realtimeSystemPLResults.longOrders = admPositionImportWeb[i].transNetLong;
                    realtimeSystemPLResults.shortOrders = admPositionImportWeb[i].transNetShort;

                    realtimeSystemPLResults.longTransAvgPrice = admPositionImportWeb[i].transAvgLongPrice;
                    realtimeSystemPLResults.shortTransAvgPrice = admPositionImportWeb[i].transAvgShortPrice;

                    realtimeSystemPLResults.delta = admPositionImportWeb[i].contractData.delta;
                    realtimeSystemPLResults.defaultPrice = admPositionImportWeb[i].contractData.optionSpreadExpression.defaultPrice;
                    realtimeSystemPLResults.theoreticalPrice = admPositionImportWeb[i].contractData.optionSpreadExpression.theoreticalOptionPrice;
                    realtimeSystemPLResults.spanImpliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.impliedVolFromSpan;
                    realtimeSystemPLResults.settlementImpliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.settlementImpliedVol;
                    realtimeSystemPLResults.impliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.impliedVol;
                    realtimeSystemPLResults.bid = admPositionImportWeb[i].contractData.optionSpreadExpression.bid;
                    realtimeSystemPLResults.ask = admPositionImportWeb[i].contractData.optionSpreadExpression.ask;
                    realtimeSystemPLResults.last = admPositionImportWeb[i].contractData.optionSpreadExpression.trade;
                    realtimeSystemPLResults.settle = admPositionImportWeb[i].contractData.optionSpreadExpression.settlement;
                    realtimeSystemPLResults.yesterdaySettle = admPositionImportWeb[i].contractData.optionSpreadExpression.yesterdaySettlement;

                    if (admPositionImportWeb[i].contractData.optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        realtimeSystemPLResults.idContract =
                            admPositionImportWeb[i].contractData.optionSpreadExpression.futureId;
                    }
                    else
                    {
                        realtimeSystemPLResults.idContract =
                            admPositionImportWeb[i].contractData.optionSpreadExpression.underlyingFutureId;
                    }
                    realtimeSystemPLResults.idOption = admPositionImportWeb[i].contractData.optionSpreadExpression.optionId;
                    realtimeSystemPLResults.callOrPutOrFutureChar =
                        admPositionImportWeb[i].contractData.optionSpreadExpression.callPutOrFutureChar;
                }
            }

            if (admPositionImportWeb.Count > 0)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    tmlModelDBQueries.updateEndOfDaySystemPLResults(realtimeSystemPLResultsList);
                }
            }


            tmlModelDBQueries.closeDB();

            return returnDateTime;
        }

        public DateTime writeRealtimeStateToDatabaseAzure()
        {
            TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();

            //TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            //tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            RealtimeSystemResults[] realtimeSystemResultsList = new RealtimeSystemResults[optionStrategies.Length];

            for (int optionSpreadCounter = 0; optionSpreadCounter < optionStrategies.Length; optionSpreadCounter++)
            {
                realtimeSystemResultsList[optionSpreadCounter] = new RealtimeSystemResults();

                realtimeSystemResultsList[optionSpreadCounter].idPortfoliogroup = optionStrategies[optionSpreadCounter].idPortfoliogroup;

                realtimeSystemResultsList[optionSpreadCounter].idStrategy = optionStrategies[optionSpreadCounter].idStrategy;

                realtimeSystemResultsList[optionSpreadCounter].date = DateTime.Now;
                //optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.lastTimeUpdated;

                if (optionStrategies[optionSpreadCounter].entryRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategies[optionSpreadCounter].entryRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].entryRule.Append(
                        optionStrategies[optionSpreadCounter].entryRule.Evaluate());


                }

                if (optionStrategies[optionSpreadCounter].exitRule != null)
                {
                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategies[optionSpreadCounter].exitRule.parsedWithVariables());

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append("  ");

                    realtimeSystemResultsList[optionSpreadCounter].exitRule.Append(
                        optionStrategies[optionSpreadCounter].exitRule.Evaluate());
                }

                int totalLegs = optionStrategies[optionSpreadCounter].legData.Length;

                //bool dateTimeStampUpdated = false;

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            getSyntheticClose(optionStrategies[optionSpreadCounter]),
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));

                realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(";");


                for (int legCounter = 0; legCounter < optionStrategies[optionSpreadCounter].legData.Length; legCounter++)
                {
                    //                     if (!dateTimeStampUpdated)
                    //                     {
                    //                         realtimeSystemResultsList[optionSpreadCounter].date =
                    //                             optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.lastTimeUpdated;
                    // 
                    //                         dateTimeStampUpdated = true;
                    //                     }


                    realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                        optionStrategies[optionSpreadCounter].legData[legCounter]
                                    .optionSpreadExpression.decisionPrice);
                    realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");



                    realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].pAndLDay, 2).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].plDayChg.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].delta, 2).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].deltaDay.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.defaultPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.defaultPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].dfltPrice.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.theoreticalOptionPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.theoreticalOptionPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].theorPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.impliedVolFromSpan, 4).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].spanImplVol.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(
                        Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlementImpliedVol, 4).ToString());
                    realtimeSystemResultsList[optionSpreadCounter].settleImplVol.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlement, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].settlement.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.settlement,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].settlement.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(
                    //    ConversionAndFormatting.roundToSmallestIncrement(
                    //        getSyntheticClose(optionStrategies[optionSpreadCounter]),
                    //        optionStrategies[optionSpreadCounter].instrument.tickSize,
                    //        optionStrategies[optionSpreadCounter].instrument.optionTickSize, 
                    //        optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    //realtimeSystemResultsList[optionSpreadCounter].syntheticClose.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                    //    Math.Round(optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPrice, 2).ToString());
                    //realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(
                        ConversionAndFormatting.roundToSmallestIncrement(
                            optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPrice,
                            optionStrategies[optionSpreadCounter].instrument.tickSize,
                            optionStrategies[optionSpreadCounter].instrument.optionTickSize,
                            optionStrategies[optionSpreadCounter].instrument.secondaryOptionTickSize));
                    realtimeSystemResultsList[optionSpreadCounter].transPrice.Append(",");

                    realtimeSystemResultsList[optionSpreadCounter].transTime.Append(
                        optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPriceTime
                            .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                    realtimeSystemResultsList[optionSpreadCounter].transTime.Append(",");

                    //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(
                    //    optionStrategies[optionSpreadCounter].legData[legCounter].optionSpreadExpression.transactionPriceTime
                    //        .ToString("HH:mm", DateTimeFormatInfo.InvariantInfo));
                    //realtimeSystemResultsList[optionSpreadCounter].spreadAction.Append(",");
                }
            }


            DateTime returnDateTime = tmlAzureModelDBQueries.updateEndOfDaySystemResults(realtimeSystemResultsList, 2);

            //UPDATE MODEL PL TO DB

            List<RealtimeSystemPLResults> realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                if (optionSpreadExpressionList[i].optionExpressionType
                    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                    &&
                        (optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0
                        ||
                        optionSpreadExpressionList[i].numberOfOrderContracts != 0)
                    )
                {
                    RealtimeSystemPLResults realtimeSystemPLResults = new RealtimeSystemPLResults();
                    realtimeSystemPLResultsList.Add(realtimeSystemPLResults);

                    realtimeSystemPLResults.fcmData = 0;

                    realtimeSystemPLResults.idPortfoliogroup = initializationParms.idPortfolioGroup;
                    realtimeSystemPLResults.contract = optionSpreadExpressionList[i].cqgSymbol;
                    realtimeSystemPLResults.strikePrice = optionSpreadExpressionList[i].strikePrice;
                    realtimeSystemPLResults.idInstrument = optionSpreadExpressionList[i].instrument.idInstrument;
                    realtimeSystemPLResults.date = DateTime.Now;

                    realtimeSystemPLResults.plDayChg = optionSpreadExpressionList[i].plChgForContractSummary;
                    realtimeSystemPLResults.plLongOrderChg = optionSpreadExpressionList[i].plChgOrders;

                    realtimeSystemPLResults.plSettleDayChg = optionSpreadExpressionList[i].plChgToSettlement;
                    realtimeSystemPLResults.plLongSettleOrderChg = optionSpreadExpressionList[i].plChgOrdersToSettlement;

                    realtimeSystemPLResults.totalQty = optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary;
                    realtimeSystemPLResults.longOrders = optionSpreadExpressionList[i].numberOfOrderContracts;

                    realtimeSystemPLResults.delta = optionSpreadExpressionList[i].deltaChgForContractSummary;
                    realtimeSystemPLResults.defaultPrice = optionSpreadExpressionList[i].defaultPrice;
                    realtimeSystemPLResults.theoreticalPrice = optionSpreadExpressionList[i].theoreticalOptionPrice;
                    realtimeSystemPLResults.spanImpliedVol = optionSpreadExpressionList[i].impliedVolFromSpan;
                    realtimeSystemPLResults.settlementImpliedVol = optionSpreadExpressionList[i].settlementImpliedVol;
                    realtimeSystemPLResults.impliedVol = optionSpreadExpressionList[i].impliedVol;
                    realtimeSystemPLResults.bid = optionSpreadExpressionList[i].bid;
                    realtimeSystemPLResults.ask = optionSpreadExpressionList[i].ask;
                    realtimeSystemPLResults.last = optionSpreadExpressionList[i].trade;
                    realtimeSystemPLResults.settle = optionSpreadExpressionList[i].settlement;
                    realtimeSystemPLResults.yesterdaySettle = optionSpreadExpressionList[i].yesterdaySettlement;

                    if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        realtimeSystemPLResults.idContract = optionSpreadExpressionList[i].futureId;
                    }
                    else
                    {
                        realtimeSystemPLResults.idContract = optionSpreadExpressionList[i].underlyingFutureId;
                    }
                    realtimeSystemPLResults.idOption = optionSpreadExpressionList[i].optionId;
                    realtimeSystemPLResults.callOrPutOrFutureChar = optionSpreadExpressionList[i].callPutOrFutureChar;


                    realtimeSystemPLResults.officeAcct = "";
                }
            }

            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                tmlAzureModelDBQueries.updateEndOfDaySystemPLResults(realtimeSystemPLResultsList);
            }

            //UPDATE ADM PL TO DB

            realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            //List<ADMPositionImportWeb> admPositionImportWeb = admPositionImportWeb;

            for (int i = 0; i < admPositionImportWeb.Count; i++)
            {
                //if (admPositionImportWeb[i].optionExpressionType
                //    != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                //    && optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0)
                {
                    RealtimeSystemPLResults realtimeSystemPLResults = new RealtimeSystemPLResults();
                    realtimeSystemPLResultsList.Add(realtimeSystemPLResults);

                    realtimeSystemPLResults.fcmData = 1;

                    realtimeSystemPLResults.idPortfoliogroup = initializationParms.idPortfolioGroup;
                    realtimeSystemPLResults.contract = admPositionImportWeb[i].cqgSymbol;
                    realtimeSystemPLResults.strikePrice = admPositionImportWeb[i].strikeInDecimal;
                    realtimeSystemPLResults.idInstrument = admPositionImportWeb[i].instrument.idInstrument;
                    realtimeSystemPLResults.date = DateTime.Now;

                    realtimeSystemPLResults.plDayChg = admPositionImportWeb[i].contractData.pAndLDay;
                    realtimeSystemPLResults.plLongOrderChg = admPositionImportWeb[i].contractData.pAndLLongOrders;
                    realtimeSystemPLResults.plShortOrderChg = admPositionImportWeb[i].contractData.pAndLShortOrders;

                    realtimeSystemPLResults.plSettleDayChg = admPositionImportWeb[i].contractData.pAndLDaySettlementToSettlement;
                    realtimeSystemPLResults.plLongSettleOrderChg = admPositionImportWeb[i].contractData.pAndLLongSettlementOrders;
                    realtimeSystemPLResults.plShortSettleOrderChg = admPositionImportWeb[i].contractData.pAndLShortSettlementOrders;

                    realtimeSystemPLResults.totalQty = (int)admPositionImportWeb[i].Net;
                    realtimeSystemPLResults.longOrders = admPositionImportWeb[i].transNetLong;
                    realtimeSystemPLResults.shortOrders = admPositionImportWeb[i].transNetShort;

                    realtimeSystemPLResults.longTransAvgPrice = admPositionImportWeb[i].transAvgLongPrice;
                    realtimeSystemPLResults.shortTransAvgPrice = admPositionImportWeb[i].transAvgShortPrice;

                    realtimeSystemPLResults.delta = admPositionImportWeb[i].contractData.delta;
                    realtimeSystemPLResults.defaultPrice = admPositionImportWeb[i].contractData.optionSpreadExpression.defaultPrice;
                    realtimeSystemPLResults.theoreticalPrice = admPositionImportWeb[i].contractData.optionSpreadExpression.theoreticalOptionPrice;
                    realtimeSystemPLResults.spanImpliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.impliedVolFromSpan;
                    realtimeSystemPLResults.settlementImpliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.settlementImpliedVol;
                    realtimeSystemPLResults.impliedVol = admPositionImportWeb[i].contractData.optionSpreadExpression.impliedVol;
                    realtimeSystemPLResults.bid = admPositionImportWeb[i].contractData.optionSpreadExpression.bid;
                    realtimeSystemPLResults.ask = admPositionImportWeb[i].contractData.optionSpreadExpression.ask;
                    realtimeSystemPLResults.last = admPositionImportWeb[i].contractData.optionSpreadExpression.trade;
                    realtimeSystemPLResults.settle = admPositionImportWeb[i].contractData.optionSpreadExpression.settlement;
                    realtimeSystemPLResults.yesterdaySettle = admPositionImportWeb[i].contractData.optionSpreadExpression.yesterdaySettlement;

                    if (admPositionImportWeb[i].contractData.optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        realtimeSystemPLResults.idContract =
                            admPositionImportWeb[i].contractData.optionSpreadExpression.futureId;
                    }
                    else
                    {
                        realtimeSystemPLResults.idContract =
                            admPositionImportWeb[i].contractData.optionSpreadExpression.underlyingFutureId;
                    }
                    realtimeSystemPLResults.idOption = admPositionImportWeb[i].contractData.optionSpreadExpression.optionId;
                    realtimeSystemPLResults.callOrPutOrFutureChar =
                        admPositionImportWeb[i].contractData.optionSpreadExpression.callPutOrFutureChar;

                    realtimeSystemPLResults.officeAcct = admPositionImportWeb[i].MODEL_OFFICE_ACCT;
                }
            }

            if (admPositionImportWeb.Count > 0)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                {
                    tmlAzureModelDBQueries.updateEndOfDaySystemPLResults(realtimeSystemPLResultsList);
                }
            }


            //tmlModelDBQueries.closeDB();

            return returnDateTime;
        }

        public DataRow[][] readRealtimeSystemResultsFromDatabase()
        {
            TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();




            DataRow[][] systemResultsComparison = tmlAzureModelDBQueries.selectOptionSystemResultsComparison(
                initializationParmsSaved.modelDateTime, initializationParmsSaved.idPortfolioGroup);

            //UPDATE MODEL PL TO DB

            //List<RealtimeSystemPLResults> realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            //for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            

            //if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            //{
            //    tmlAzureModelDBQueries.updateEndOfDaySystemPLResults(realtimeSystemPLResultsList);
            //}

            //UPDATE ADM PL TO DB

            //realtimeSystemPLResultsList = new List<RealtimeSystemPLResults>();

            return systemResultsComparison;
        }

        public void updateADMSummaryForm(int contractSummaryInstrumentSelectedIdx)
        {
            if (sAdmReportWebPositionsForm != null)
            {
                sAdmReportWebPositionsForm.updateSelectedInstrumentFromTreeADM(contractSummaryInstrumentSelectedIdx);
            }
        }

        public DataRow[][] getOptionSettlePLPriceCompareAzure(DateTime queryDate, int fcmData)
        {
            TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();


            DataRow[][] systemOptionPlResults = tmlAzureModelDBQueries.selectOptionSystemPlResults(queryDate,
                initializationParms.idPortfolioGroup, fcmData);


            return systemOptionPlResults;
        }

        public DataRow[] getFutureSettlePLPriceCompareAzure(DateTime queryDate, int fcmData)
        {
            TMLAzureModelDBQueries tmlAzureModelDBQueries = new TMLAzureModelDBQueries();
            
            //TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            //tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            //DateTime dateTimeQueryYesterday = tmlModelDBQueries.getPreviousDay(queryDate);

            DataRow[] systemOptionPlResults = tmlAzureModelDBQueries.selectFutureSystemPlResults(queryDate,
                initializationParms.idPortfolioGroup, fcmData);

            //DataRow[] systemFuturePlResults = tmlModelDBQueries.selectFutureSystemPlResults(queryDate, 1, 1);

            //for(int i = 0; i < systemOptionPlResults.Length; i++)
            //{
            //    double x = 
            //        Convert.ToDouble(
            //        systemOptionPlResults[i]["price"]);

            //    TSErrorCatch.debugWriteOut(x + "");
            //}

            //tmlModelDBQueries.closeDB();

            return systemOptionPlResults;
        }


        public DataRow[] getOptionSettlePLPriceCompare(DateTime queryDate, int fcmData)
        {
            TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            //DateTime dateTimeQueryYesterday = tmlModelDBQueries.getPreviousDay(queryDate);

            DataRow[] systemOptionPlResults = tmlModelDBQueries.selectOptionSystemPlResults(queryDate,
                initializationParms.idPortfolioGroup, fcmData);

            //DataRow[] systemFuturePlResults = tmlModelDBQueries.selectFutureSystemPlResults(queryDate, 1, 1);

            //for(int i = 0; i < systemOptionPlResults.Length; i++)
            //{
            //    double x = 
            //        Convert.ToDouble(
            //        systemOptionPlResults[i]["price"]);

            //    TSErrorCatch.debugWriteOut(x + "");
            //}

            return systemOptionPlResults;
        }

        public DataRow[] getFutureSettlePLPriceCompare(DateTime queryDate, int fcmData)
        {
            TMLModelDBQueries tmlModelDBQueries = new TMLModelDBQueries();

            tmlModelDBQueries.connectDB(initializationParms.dbServerName);

            //DateTime dateTimeQueryYesterday = tmlModelDBQueries.getPreviousDay(queryDate);

            DataRow[] systemOptionPlResults = tmlModelDBQueries.selectFutureSystemPlResults(queryDate,
                initializationParms.idPortfolioGroup, fcmData);

            //DataRow[] systemFuturePlResults = tmlModelDBQueries.selectFutureSystemPlResults(queryDate, 1, 1);

            //for(int i = 0; i < systemOptionPlResults.Length; i++)
            //{
            //    double x = 
            //        Convert.ToDouble(
            //        systemOptionPlResults[i]["price"]);

            //    TSErrorCatch.debugWriteOut(x + "");
            //}

            tmlModelDBQueries.closeDB();

            return systemOptionPlResults;
        }


        internal string selectAcct(string tradingTechnologiesExchangeSymbol,
            PortfolioGroupAllocation portfolioGroupAllocation, bool fillingPayoffGrid)
        {
            //TODO: AUG 10 2015 - PortfolioGroupAllocation cfgfile

            //string acct = initializationParmsSaved.FIX_Acct;

            string acct = "";

            if(portfolioGroupAllocation.useConfigFile)
            {
                if(fillingPayoffGrid)
                {
                    //acct.Append(groupAllocationIdx);
                    //acct.Append(";");
                    acct = (portfolioGroupAllocation.cfgfile);
                }
                else if (portfolioGroupAllocation.instrumentAcctHashSet.ContainsKey(tradingTechnologiesExchangeSymbol))
                {
                    acct = (portfolioGroupAllocation.instrumentAcctHashSet[tradingTechnologiesExchangeSymbol]);
                }                
            }
            else
            {
                //if (fillingPayoffGrid)
                //{
                    //acct.Append(groupAllocationIdx);
                    //acct.Append(";");
                acct = portfolioGroupAllocation.account;
                //}
                //else
                //{
                //    acct = portfolioGroupAllocation.account;
                //}
            }

            return acct;
        }

        private String generateCQGSymbolForEODSubstitution(LegInfo legInfo, Instrument instrument)
        {
            string cqgSubstitutionSymbol = "";

            if (legInfo.legContractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                legInfo.cqgSubstituteSymbol =
                                       legInfo.cqgSymbol.Replace(
                                           legInfo.instrumentSymbolPreEOD,
                                           legInfo.instrumentSymbolEODSubstitute);
            }
            else
            {
                //internal Instrument[] instruments { get; set; }
        //private Instrument_DefaultFutures[] instrument_DefaultFuturesArray;

        //internal Dictionary<int, Instrument> substituteInstrumentHash = new Dictionary<int, Instrument>();

                //int idxCnt = 0;
                //while(idxCnt < instruments.Length)
                //{
                //    if(instruments[idxCnt].idInstrument == legInfo.idUnderlyingContract)
                //    {
                //        break;
                //    }

                //    idxCnt++;
                //}

                Instrument instrumentSubstitute =
                    substituteInstrumentHash[instrument.instrumentIdEOD];

                legInfo.cqgSubstituteSymbol =
                    generateOptionCQGSymbolForEODSubstitution(
                        legInfo.optionCallOrPut,
                        legInfo.instrumentSymbolEODSubstitute,
                        legInfo.optionMonth.ToString(),
                        legInfo.optionYear,
                        legInfo.optionStrikePrice,
                        instrumentSubstitute.optionStrikeIncrement,
                        instrumentSubstitute.optionStrikeDisplay,
                        instrumentSubstitute.idInstrument);
            }

            //generateOptionCQGSymbolForEODSubstitution(
            //    optionStrategies[i].rollIntoLegInfo[legCounter].optionCallOrPut,
            //    optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute,
            //    optionStrategies[i].rollIntoLegInfo[legCounter].contractMonth,
            //    optionStrategies[i].rollIntoLegInfo[legCounter].optionYear,
            //    optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePrice,
            //    optionStrategies[i].rollIntoLegInfo[legCounter].instrumentSymbolEODSubstitute);


            return cqgSubstitutionSymbol;
        }

        private String generateOptionCQGSymbolForEODSubstitution(
            char contractTypeOptionOrPut,
            //int contractType, 
            String underlyingSymbol, String month, int year,
            double optionStrikePrice, double optionStrikeIncrement, double optionStrikeDisplay, int instrumentId)
        {
            StringBuilder cqgSymbol = new StringBuilder();

#if DEBUG
            try
#endif
            {
                //cqgSymbol.Append(contractTypeCharSymbolArray.GetValue(contractType).ToString());

                cqgSymbol.Append(contractTypeOptionOrPut);

                //cqgSymbol.Append(".US.");

                cqgSymbol.Append(".");

                cqgSymbol.Append(underlyingSymbol);

                cqgSymbol.Append(month);

                cqgSymbol.Append((year % 100));

                cqgSymbol.Append(ConversionAndFormatting.convertToStrikeForCQGSymbol(
                                        optionStrikePrice,
                                        optionStrikeIncrement,
                                        optionStrikeDisplay, instrumentId));
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

            return cqgSymbol.ToString();
        }
    }
}
