using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using CQGLibrary = FakeCQG;

namespace RealtimeSpreadMonitor
{

    public class InitializationParms
    {
        public int tmlSystemRunType;
        public bool runLiveSystem;
        public bool connectToXtrader;
        public bool initializePersistence;
        public bool runFromDb;
        public String dbServerName;

        public int idPortfolioGroup;

        public String portfolioGroupName;

        public DateTime modelDateTime;

        public bool useHalfday;
        public DateTime halfDayTransactionTime;
        public int halfDayDecisionOffsetMinutes;

        //public string FIX_Broker_18220;
        //public string[] fixBrokerList;

        //public string FIX_Acct;
        //public string[] fixAcctList;

        public String[] initializationConfigs;

        public int FIX_OrderPlacementType;

        public bool useCloudDb = false;
    };

    public class PortfolioGroupsStruct
    {
        public int idPortfolioGroup;
        public String portfolioName;
        public bool selected;
    };

    public class FCM_POFFIC_PACCT
    {
        public string FCM_POFFIC;
        public string FCM_PACCT;
    }

    public class InstrumentSpecificFIXFields
    {
        public string FCM;
        public string TTGateway;
        public string TTExchange;
        public string TTSymbol;

        public char TAG_47_Rule80A;
        public int TAG_204_CustomerOrFirm;
        public string TAG_18205_TTAccountType;
        public string TAG_440_ClearingAccount;
        public string TAG_16102_FFT2;
    }

    public class PortfolioGroupAllocation
    {
        //public int idportfoliogroupAllocation { get; set; }
        public int idportfoliogroup { get; set; }
        public int multiple { get; set; }
        public string broker { get; set; }
        public string account { get; set; }
        public string cfgfile { get; set; }

        public string FCM_POFFIC_PACCT { get; set; }
        //public List<FCM_POFFIC_PACCT> FCM_POFFIC_PACCT_List = new List<FCM_POFFIC_PACCT>();

        public ConcurrentDictionary<string, FCM_POFFIC_PACCT> FCM_POFFIC_PACCT_hashset = 
            new ConcurrentDictionary<string, FCM_POFFIC_PACCT>();

        public Dictionary<string, string> instrumentAcctHashSet;

        public bool useConfigFile = false;
    }

    public class Instrument
    {
        public bool continueRealtimeDataCurrent;

        public bool isSpread;

        public bool isListedSpread;

        public int idxOfInstrumentInList;

        public String CQGsymbol;
        public String name;
        public String exchangeSymbol;
        public String optionExchangeSymbol;
        public String exchangeSymbolTT;
        public String optionExchangeSymbolTT;

        public String description;
        public String country;
        public String currency;

        public DateTime dataStart;
        public DateTime dataStop;
        public DateTime expiration;

        public int year;
        public char month;

        public int idInstrument;
        public int idBarInfo;

        public int idPortfolioGroup;

        public int idInstrumentGroup;
        public String instrumentGroup;

        public double tickSize;
        public double tickDisplay;

        /// <summary>
        /// The tick display for TT used to multiply, usually just value of 1
        /// but for example soybeans price is sent from CQG and SPAN in cents 904.25
        /// and optionTickDisplay is 2 makes value 9042
        /// TT needs price sent in dollars, so this price is 9.042 this value is 0.001
        /// </summary>
        //public double tickDisplayTT;

        public double tickValue;
        public int margin;
        public int timeShiftHours;
        public double commissionPerContract;


        public bool displayInStrategySummary;

        public bool continueWithOptimize;

        public bool configuredSeries;
        public int idcontractseriescfg;

        public double optionStrikeIncrement;

        public int stopType;

        public int limitTickOffset;

        public double optionTickSize;
        public double optionTickDisplay;
        
        /// <summary>
        /// The option tick display for TT used to multiply, usually just value of 1
        /// but for example soybeans price is sent from CQG and SPAN in cents 904.25
        /// and optionTickDisplay is 2 makes value 9042
        /// TT needs price sent in dollars, so this price is 9.042 this value is 0.001
        /// </summary>
        //public double optionTickDisplayTT;

        public double optionTickValue;
        public double optionStrikeDisplay;
        public double optionStrikeDisplayTT;

        public double optionADMStrikeDisplay;
        public double admOptionFtpFileStrikeDisplay;

        public double admFuturePriceFactor;
        public double admOptionPriceFactor;


        public double secondaryOptionTickSize;
        public double secondaryOptionTickValue;

        public double secondaryoptiontickdisplay;

        public double secondaryOptionTickSizeRule;

        public DateTime customDayBoundaryTime;
        public bool useDailyCustomData;
        public int decisionOffsetMinutes; 

        public DateTime optionSpreadStart;

        public String admCode;
        public String admExchangeCode;

        public String exchange;
        public String spanExchangeSymbol;
        public String spanExchWebAPISymbol;
        public String tradingTechnologiesExchange;
        public String tradingTechnologiesGateway;

        public String spanFutureCode;
        public String spanOptionCode;

        public List<TradeCalendarData> tradeCalendarData;
        public TradeCalendarDescription[] tradeCalendarDescription;

        public string coreAPImarginId;
        public double coreAPIinitialMargin;
        public double coreAPImaintenanceMargin;

        public string coreAPI_FCM_marginId;
        public double coreAPI_FCM_initialMargin;
        public double coreAPI_FCM_maintenanceMargin;

        public bool substituteSymbolEOD;
        public string instrumentSymbolPreEOD;
        public string instrumentSymbolEOD;
        public int instrumentIdEOD;
        

        public DateTime settlementTime;



        public bool eodAnalysisAtInstrument;
        public DateTime settlementDateTimeMarker;
        //public bool reachedSettlementDateTimeMarker;

    };

    public class LiveRowInfoIdx
    {
        public int legIdx = -1;
        public int rowIdx;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Option strategy.  </summary>
    ///
    /// <remarks>   Steve Pickering, 7/15/2013. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class OptionStrategy
    {
        public bool supplementContract = false;

        public int idPortfoliogroup;
        public int idStrategy;
        public int idInstrument;
        //public String strategyName;
        //public String strategyFolder;

        /// <summary> Array of index of instrument in instruments </summary>
        public int indexOfInstrumentInInstrumentsArray;
        public Instrument instrument;

        public DateTime dateTime;

        public OptionStrategyParameter[] optionStrategyParameters;

        public bool holdsCurrentPosition;
        public int idxOfStratsHoldingPositions = -1;

        public double lockedIn_R;

        public LegInfo[] legInfo;

        public int idxOfFutureLeg;

        public LegData[] legData;

        public RollLegInfo[] rollIntoLegInfo;

        public LegData[] rollIntoLegData;

        public bool rollStrikesUpdated;

        public int[] liveGridRowLoc;

        public int[] orderGridRowLoc;


        public List<LiveRowInfoIdx> rollStrikeGridRows;

        public LiveSpreadTotals deltaHedgeTotals = new LiveSpreadTotals();
        public LiveSpreadTotals liveSpreadTotals = new LiveSpreadTotals();

        

        public ReversePolishNotation entryRule;

        public ReversePolishNotation exitRule;

        public List<TheoreticalBar> syntheticCloseTheoretical = new List<TheoreticalBar>();

        //modeling variables
        public double syntheticClose;
        public bool syntheticCloseFilled = false;

        //used for the order page to display the type of order
        public SPREAD_POTENTIAL_OPTION_ACTION_TYPES actionType;

        //public RealtimeSpreadMonitor.Forms.OrderChartForm orderChart;  // = new RealtimeSpreadMonitor.Forms.OrderChart();

        public bool wroteIntradaySnapshotToDB;

        public int idSignalType;
        public int idRiskType;
    }

    public class OptionStrategyParameter
    {

        public TBL_DB_PARSE_PARAMETER parseParameter;
        public TBL_STRATEGY_STATE_FIELDS strategyStateFieldType;

        public String stateValueStringNotParsed;
        
        public double stateValue;

        public double[] stateValueParsed;

    }



    public class LegInfo
    {
        public OPTION_SPREAD_CONTRACT_TYPE legContractType;

        //option info
        public int idOption;
        public String optionName;
        public char optionMonth;
        public int optionMonthInt;
        public int optionYear;
        public double optionStrikePrice;
        public char optionCallOrPut;
        public int idUnderlyingContract;

        //future info
        public int idContract;
        public String contractName;
        public char contractMonth;
        public int contractMonthInt;
        public int contractYear;
        
        //common to both futures and options
        public DateTime expirationDate;

        //if this part of the ADM data it is filled by fillInRestOfADMWebInputData
        private String xcqgSymbol;

        public String cqgSymbol
        {
            set { xcqgSymbol = value; }
            get { return xcqgSymbol; }
        }

        public bool useSubstitueSymbolEOD;
        public String instrumentSymbolPreEOD;
        public String instrumentSymbolEODSubstitute;
        
        public String cqgSubstituteSymbol;
        public String cqgSubstituteSymbolWithoutStrike_ForRollover;

        public String cqgSymbolWithoutStrike_ForRollover;

        public DateTime optionExpirationTime;

    }

    public class Instrument_DefaultFutures
    {
        public int idInstrument;

        public Instrument instrument;

        public LegInfo[] defaultFutures;

        public LegData[] defaultFuturesData;
    }

    public class RollLegInfo : LegInfo
    {
        public int strikeLevelOffsetForRoll;

        public int strikeIndexOfStrikeRange = TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT/2;
        //public double[] futurePriceReference = new double[TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT];
        public double[] optionStrikePriceReference = new double[TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT];

        //future info
        public double futurePriceUsedToCalculateStrikes;
        public double futurePriceFactor;

        public bool reachedBarAfterDecisionBar_ForRoll;

        public double[,] futurePriceRule = new double[2, TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT];

    }

    public class LiveSpreadTotals
    {
        public double delta;
        
        public double pAndLDay;
        public double pAndLDayOrders;        

        public double pAndLDaySettlementToSettlement;
        public double pAndLDaySettleOrders;


        //USED FOR SAVING FCM DATA TO DATABASE
        public double pAndLLongOrders;
        public double pAndLShortOrders;

        public double pAndLLongSettlementOrders;
        public double pAndLShortSettlementOrders;

        public double longTransAvgPrice;
        public double shortTransAvgPrice;


        public double initialMarginTotals;
        public double maintenanceMarginTotals;

        public double initialFCM_MarginTotals;
        public double maintenanceFCM_MarginTotals;
        
        //public double hedgeTotal;
        //public double yesterdayPL;
        //public double yesterdayHedgePL;
        //public double deltaTotalForOrdersPage;
    }

    public class LegData : LiveSpreadTotals
    {
        public DateTime dataDateTimeFromDB;
        public double settlementPriceFromDB;
        public double impliedVolFromDB;
        public double timeToExpInYearsFromDB;

        //public double delta;
        //public double pAndL;

        public OptionSpreadExpression optionSpreadExpression;

        public OptionSpreadExpression optionSpreadSymbolSubstituteExpression;
    }

//     public class LegInfo
//     {
//         
// 
//         public FutureLegInfo futureLegInfo;
//         public OptionLegInfo optionLegInfo;
// 
//     }

    public class OHLCData
    {
        public DateTime barTime;
        public double open;
        public double high;
        public double low;
        public double close;
        public int volume;
        public int cumulativeVolume;
        
        public bool errorBar;
    };

    public class TheoreticalBar
    {
        public DateTime barTime;
        public double price;
    }

    public class TradeCalendarData
    {
        public DateTime tcDateTime;
        public int tcTypeId;
    };

    public class TradeCalendarDescription
    {
        public String tcDayTypeDescription;
        public int tcDayTypeIdDescription;
        public String tcTimeDescription;
    };

    public class OptionInputFieldsFromTblOptionInputSymbols
    {
        public int idOptionInputSymbol;
        public String optionInputCQGSymbol;
        public int idInstrument;
        public int idOptionInputType;
        public double multiplier;
    }

    

    public class OptionSpreadExpression
    {
        public CQGLibrary.CQGInstrument cqgInstrument;
        public CQGLibrary.CQGTimedBars futureTimedBars;

        public bool stopUpdating = false;

        public bool newStrikeLevel = false;

        public Instrument instrument;

        public OptionInputFieldsFromTblOptionInputSymbols optionInputFieldsFromTblOptionInputSymbols;

        public String cqgSymbol;

        //public String cqgSubstituteSymbol;


        public bool normalSubscriptionRequest = false;
        public bool substituteSubscriptionRequest = false;

        public bool useSubstituteSymbolAtEOD = false;

        //this is used for margin calculation from option payoff chart
        //******************************************
        public int optionMonthInt;
        public int optionYear;

        public int futureContractMonthInt;
        public int futureContractYear;
        //******************************************

        public bool setSubscriptionLevel = false;
        public bool requestedMinuteBars = false;

        public OPTION_EXPRESSION_TYPES optionExpressionType;

        public OPTION_SPREAD_CONTRACT_TYPE callPutOrFuture;

        public char callPutOrFutureChar;


        public int optionId; //only filled if contract is an option
        public int underlyingFutureId;
        
        public int substituteOptionId; //only filled if contract is an option
        public int substituteUnderlyingFutureId;


        public int futureId; //only filled if contract is a future
        public int substituteFutureId;


        public double strikePrice;

        public double riskFreeRate = 0.01;
        public bool riskFreeRateFilled = false;

        public double yearFraction;

        public DateTime lastTimeUpdated;

        public double  minutesSinceLastUpdate = 0;

        public DateTime lastTimeFuturePriceUpdated; //is separate b/c can get time stamp off of historical bars

        //public DateTime decisionTime;
        //public DateTime transactionTime;
        //public DateTime latestDecisionTimeUpdated;
        //public DateTime latestTransactionTimeUpdated;

        public double ask;
        public bool askFilled;

        public double bid;
        public bool bidFilled;

        public double trade;
        public bool tradeFilled;

        public double settlement;
        public bool settlementFilled;
        public bool manuallyFilled;
        public DateTime settlementDateTime;
        public bool settlementIsCurrentDay;

        public double yesterdaySettlement;
        public bool yesterdaySettlementFilled;


        public double defaultBidPriceBeforeTheor;
        //public bool defaultBidPriceBeforeTheorFilled;

        public double defaultAskPriceBeforeTheor;
        //public bool defaultAskPriceBeforeTheorFilled;        
        
        public double defaultMidPriceBeforeTheor;

        public double defaultPrice;
        public bool defaultPriceFilled;



        public double decisionPrice;
        public DateTime decisionPriceTime;
        public bool decisionPriceFilled = false;

        public double transactionPrice;
        public DateTime transactionPriceTime;
        public bool transactionPriceFilled = false;


        public double impliedVolFromSpan;


        public double theoreticalOptionPrice;

        public double settlementImpliedVol;
        
        public double impliedVol;

        public bool impliedVolFilled = false; //used for calculating option transaction price

        public double delta;
        //public double gamma;
        //public double vega;
        //public double theta;



        public List<int> spreadIdx = new List<int>();
        public List<int> legIdx = new List<int>();
        public List<int> rowIdx = new List<int>();


        public List<int> substituteSymbolSpreadIdx = new List<int>();
        public List<int> substituteSymbolLegIdx = new List<int>();
        public List<int> substituteSymbolRowIdx = new List<int>();

        public OptionSpreadExpression mainExpressionSubstitutionUsedFor;


        //public List<int> admStrategyIdx = new List<int>();
        public List<int> admPositionImportWebIdx = new List<int>();
        public List<int> admRowIdx = new List<int>();


        public List<OHLCData> futureBarData;
        public List<DateTime> futureBarTimeRef;
        public List<TheoreticalBar> theoreticalOptionDataList;

        
        public DateTime previousDateTimeBoundaryStart;

        public OHLCData todayTransactionBar;
        public DateTime todayTransactionTimeBoundary;
        public bool reachedTransactionTimeBoundary = false;
        public bool filledAfterTransactionTimeBoundary = false;

        public OHLCData decisionBar;
        public DateTime todayDecisionTime;
        public bool reachedDecisionBar = false;
        public bool reachedBarAfterDecisionBar = false;
        public bool reached1MinAfterDecisionBarUsedForSnapshot = false;

        //public DateTime settlementDateTimeMarker;
        //public bool reachedSettlementDateTimeMarker;


        public CQG_REFRESH_STATE guiRefresh = CQG_REFRESH_STATE.NOTHING;
        public CQG_REFRESH_STATE totalCalcsRefresh = CQG_REFRESH_STATE.NOTHING;

        public OptionSpreadExpression underlyingFutureExpression;

        public List<OptionSpreadExpression> optionExpressionsThatUseThisFutureAsUnderlying; 


        //************************
        //Order summary variables
        public int numberOfOrderContractsTempForCalc = 0;
        public int numberOfOrderContractsTempForCalcNotActive = 0;
        
        public int numberOfOrderContracts = 0;
        public int numberOfOrderContractsNotActive = 0;

        public bool contractHasOrder;

        //public bool orderActionTest;
        //*************************

        //************************
        //Contract summary variables
        public int numberOfLotsHeldForContractSummary = 0;
        public double plChgForContractSummary = 0;
        public double deltaChgForContractSummary = 0;

        public double plChgOrders = 0;

        public double plChgOrdersToSettlement = 0;

        public double plChgToSettlement = 0;


        //Expression List grid
        public int dataGridExpressionListRow;

        //************************

        public OptionSpreadExpression(OPTION_SPREAD_CONTRACT_TYPE callPutOrFuture, 
            OPTION_EXPRESSION_TYPES optionExpressionType)
        {
            this.callPutOrFuture = callPutOrFuture;
            this.optionExpressionType = optionExpressionType;

            if (optionExpressionType == OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE
                && callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                optionExpressionsThatUseThisFutureAsUnderlying
                    = new List<OptionSpreadExpression>();
            }

        }
    }


    public class RealtimeMonitorSettings
    {
        public REALTIME_PRICE_FILL_TYPE realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_DEFAULT;
        public bool eodAnalysis = false;
        public bool alreadyWritten = false;
    }

    public class RealtimeSystemResults
    {
        public int idPortfoliogroup;
        public int idStrategy;
        public DateTime date;
        public StringBuilder plDayChg       = new StringBuilder();
        public StringBuilder deltaDay       = new StringBuilder();
        public StringBuilder dfltPrice      = new StringBuilder();
        public StringBuilder theorPrice     = new StringBuilder();
        public StringBuilder spanImplVol    = new StringBuilder();
        public StringBuilder settleImplVol  = new StringBuilder();
        public StringBuilder settlement     = new StringBuilder();
        public StringBuilder syntheticClose = new StringBuilder();
        public StringBuilder transPrice     = new StringBuilder();
        public StringBuilder transTime      = new StringBuilder();
        public StringBuilder entryRule      = new StringBuilder();
        public StringBuilder exitRule       = new StringBuilder();

    }

    public class RealtimeSystemPLResults
    {
        public int fcmData;

        public int idPortfoliogroup;
        public string contract;
        public double strikePrice;
        public int idInstrument;       
        public DateTime date;
        
        public double plDayChg;
        public double plLongOrderChg;
        public double plShortOrderChg;


        public double plSettleDayChg;
        public double plLongSettleOrderChg;
        public double plShortSettleOrderChg;


        public int totalQty;
        public int longOrders;
        public int shortOrders;

        public double longTransAvgPrice;
        public double shortTransAvgPrice;

        public double delta;
        public double defaultPrice;
        public double theoreticalPrice;
        public double spanImpliedVol;
        public double settlementImpliedVol;
        public double impliedVol;
        public double bid;
        public double ask;
        public double last;
        public double settle;
        public double yesterdaySettle;
        public int idContract = -1;
        public int idOption = -1;
        public char callOrPutOrFutureChar; 
       
        public string officeAcct;
    }

    public class OptionChartMargin
    {
        public OPTION_SPREAD_CONTRACT_TYPE contractType;
        
        public int optionYear;
        public int optionMonthInt;
        
        public int contractYear;
        public int contractMonthInt;

        public double strikePrice;
        public Instrument intrument;
        public int numberOfContracts;

    }


    
}
