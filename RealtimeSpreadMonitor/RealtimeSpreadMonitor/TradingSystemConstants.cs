using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RealtimeSpreadMonitor
{
    public enum TBL_STRATEGY_STATE_FIELDS
    {
        //dateTime,
        //idStrategy,
        numberOfLegs,
        spreadStructure,
        //instrumentIdx,
        activeContractIndexes,
        rollIntoContractIndexes,
        //seriesDelta,
        //startingPoint,
        cfgContracts,
        currentPosition,
        entryRule,
        entryLots,
        exitRule,
        exitLots,
        marginRequirement,
        rRisk,
        rRisk_R,
        oneR,
        rStatus,
        rStatus_R,
        idRiskType,
        idSignalType
    }
    
    public enum TBL_DB_PARSE_PARAMETER
    {
        PARSE_PARAMETER,
        DONOT_PARSE_PARAMETER,
        PARSE_ENTRY_SCRIPT,
        PARSE_EXIT_SCRIPT,
        PARSE_ROLL_PARAMETER_SCRIPT
    }

    public enum TBL_OPTIONS_FIELDS
    {
        idoption,
        optionname,
        optionmonth,
        optionmonthint,
        optionyear,
        strikeprice,
        callorput,
        expirationdate,
        idcontract,
        cqgsymbol
    }

    public enum TBL_OPTION_DATA_FIELDS
    {
        idoption,
        datetime,
        price,
        impliedvol,
        timetoexpinyears
    }

    public enum TBL_FUTURE_CONTRACT_FIELDS
    {
        idcontract,
        contractname,
        month,
        monthint,
        year,
        idinstrument,
        expirationdate,
        cqgsymbol
    }

    public enum TBL_FUTURE_SETTLEMENT_DATA_FIELDS
    {
        idcontract,
        date,
        settlement,
    }

    

    public class OptionArrayTypes
    {
        public Array tblStrategyStateFieldTypesArray;
        public Array tblOptionFieldTypesArray;
        public Array tblOptionDataFields;
        public Array tblFutureContractFieldTypesArray;
        public Array tblFutureSettlementDataFieldsArray;

        public Array optionSpreadContractTypesArray;

        //public Array spreadOptionActionTypesArray;

        public OptionArrayTypes()
        {
            Type tblStrategyStateFieldTypes = typeof(TBL_STRATEGY_STATE_FIELDS);
            tblStrategyStateFieldTypesArray = Enum.GetNames(tblStrategyStateFieldTypes);
            
            

            Type tblOptionFieldTypes = typeof(TBL_OPTIONS_FIELDS);
            tblOptionFieldTypesArray = Enum.GetNames(tblOptionFieldTypes);

            Type tblOptionDataFieldTypes = typeof(TBL_OPTION_DATA_FIELDS);
            tblOptionDataFields = Enum.GetNames(tblOptionDataFieldTypes);

            Type tblFutureContractFieldTypes = typeof(TBL_FUTURE_CONTRACT_FIELDS);
            tblFutureContractFieldTypesArray = Enum.GetNames(tblFutureContractFieldTypes);

            Type tblFutureSettlementDataFieldTypes = typeof(TBL_FUTURE_SETTLEMENT_DATA_FIELDS);
            tblFutureSettlementDataFieldsArray = Enum.GetNames(tblFutureSettlementDataFieldTypes);

            //Type tblFutureSettlementDataFieldTypes = typeof(SPREAD_OPTION_ACTION_TYPES);
            //tblFutureSettlementDataFieldsArray = Enum.GetNames(tblFutureSettlementDataFieldTypes);

            Type optionSpreadContractTypes = typeof(OPTION_SPREAD_CONTRACT_TYPE);
            optionSpreadContractTypesArray = Enum.GetNames(optionSpreadContractTypes);
        }
    }

    public enum REALTIME_PRICE_FILL_TYPE
    {
        PRICE_DEFAULT,
        PRICE_ASK,
        PRICE_MID_BID_ASK,
        PRICE_BID,
        PRICE_THEORETICAL
    }

    public enum OPTION_EXPRESSION_TYPES 
    {
        OPTION_EXPRESSION_RISK_FREE_RATE,
        SPREAD_LEG_PRICE,
    }

    public enum OPTION_FORMULA_INPUT_TYPES 
    {
        INSTRUMENT,
        OPTION_RISK_FREE_RATE,
        OPTION_CALL_IMPLIED_VOL,
        OPTION_PUT_IMPLIED_VOL,
    };

    public enum OPTION_SPREAD_CONTRACT_TYPE 
    {
        CALL,
        PUT,
        FUTURE,
        BLANK
    }

    public enum OPTION_LIVE_DATA_COLUMNS 
    {
        CONTRACT,
        LEG,
        TIME,
        //PL,

        PL_DAY_CHG,

        //YEST_PL,

        SPREAD_QTY,

        DELTA,
        DFLT_PRICE,
        
        THEOR_PRICE,
        SPAN_IMPL_VOL,
        SETL_IMPL_VOL,
        IMPL_VOL,
        //IMPL_VOL_INDX,
        BID,
        ASK,
        LAST,
        STTLE,
        SETL_TIME,
        YEST_STTLE,

        CNTDN,
        EXPR,
        STRIKE_PRICE,
        MARGIN,
        
        R_RISK,
        R_RISK_R,
        ONE_R,
        R_STATUS,
        R_STATUS_R,
        LOCKED_IN_R,

        RFR,

        SPREAD_ID,
    };

    public enum CONTRACTSUMMARY_DATA_COLUMNS
    {
        //CONTRACT,
        //LEG,
        TIME,

        TOTAL_QTY,
        ORDER_QTY,

        PL_DAY_CHG,
        ORDER_PL_CHG,        

        DELTA,
        DFLT_PRICE,

        THEOR_PRICE,
        SPAN_IMPL_VOL,
        SETL_IMPL_VOL,
        IMPL_VOL,
        BID,
        ASK,
        LAST,
        STTLE,
        SETL_TIME,
        YEST_STTLE,

        //GAMMA,
        //VEGA,
        //THETA,

        //CNTDN,
        //EXPR,
        STRIKE_PRICE,
        //RFR,

        //SPREAD_ID,
        INSTRUMENT_ID
    };

    public enum OPTION_LIVE_DATA_SUMMARY_ROWS
    {
        DELTA_HEDGING_ROW,
        TOTAL_ROW
    }

    public enum INSTRUMENT_SUMMARY_GRID_ROWS
    {
        MARGIN_SUMMARY,
        R_RISK_SUMMARY,
        R_RISK_PERCENT,
        ONE_R,
        R_STATUS,
        LOCKED_IN_R,
        SPAN_INIT_MARGIN,
        SPAN_MAINT_MARGIN,
        SPAN_INIT_FCM_MARGIN,
        SPAN_MAINT_FCM_MARGIN
    }

    public enum INSTRUMENT_SUMMARY_STRATEGY_RRISK
    {
        MARGIN,
        R_RISK,
        R_RISK_R,
        ONE_R,
        R_STATUS,
        R_STATUS_R,
        LOCKED_IN_R
    }

    public enum PORTFOLIO_SUMMARY_GRID_ROWS 
    {
        //YEST_PL, 
        PL_CHG,
        //CURRENT_TOTAL, 
        TOTAL_DELTA,

        //____,

        //ADM_SETTLE_PL, 
        ADM_PL_CHG, 
        //ADM_TOTAL, 
        ADM_TOTAL_DELTA
    }

    public enum PORTFOLIO_SETTLEMENT_SUMMARY_GRID_ROWS
    {        
        TOTAL_MODEL_SETTLEMENT_PL_CHG,
        ____,
        
        TOTAL_ADM_SETTLEMENT_PL_CHG
    }

    public enum INITIALIZATION_CONFIG_VARS 
    {
        PORTFOLIOGROUP, DBSERVERNAME //BROKER, ACCOUNT
    };

    public enum REALTIME_CONFIGURATION
    {
        SERVERNAME //MULTIBROKER, ACCOUNT
    }

    public enum REALTIME_CONFIG_FILE_INPUT_TYPE
    {
        INITIALIZATION_FILE, REALTIME_CONFIGURATION
    }

    public enum CQG_REFRESH_STATE
    {
        NOTHING,
        DATA_UPDATED
    }

    public enum OPTION_ORDERS_COLUMNS 
    {
        POTENTIAL_ACTION,
        SIGNAL_TYPE,
        ACTION_RULE,
        INTERPOLATED_SYNCLOSE,
        //ACTION_TEST,
        //SYNCLOSE,
        //VOL,

        SPREAD_ACTION,
        CONTRACT,
        QTY,
        TRANS_PRICE,
        TRANS_TIME,
        SPREAD_ID,
    };



    public enum SPREAD_POTENTIAL_OPTION_ACTION_TYPES
    {
        NO_ACTION,
        ENTRY,
        ENTRY_WITH_ROLL,
        EXIT,
        EXIT_OR_ROLL_OVER
    }

    public enum SPREAD_ACTION_TYPES
    {
        ENTER,
        ENTER_WITH_ROLL,
        DO_NOT_ENTER,
        EXIT_ONLY,
        DO_NOT_EXIT,
        ROLL_OVER,
    }

    public enum LEG_ACTION_TYPES
    {
        ENTER_BUY,
        ENTER_SELL,
        CLOSE_BUY,
        CLOSE_SELL
    }

    public enum ROLL_SPREAD_PARAMETERS
    {
        OPTION_MONTH_INT,
        OPTION_YEAR_INT,
        OPTION_STRIKE_LEVEL_INT
    }

    public enum OPTION_WATCH_ROLL_STRIKES_COLUMNS 
    {
        CONTRACT,
        OFFSET,
        LEG,
        STRIKES,
    };

    public enum OPTION_MONTHS 
    {
        F, G, H, J, K, M, N, Q, U, V, X, Z,
    }

    public enum ORDER_CHART_SERIES
    {
        FUTURE,
        SYNCLOSE,
        ENTRY_THRESHOLD,
        ENTRY_THRESHOLD_SYNCLOSE_FUTURE,
        EXIT_THRESHOLD,
        EXIT_THRESHOLD_SYNCLOSE_FUTURE,
        FUTURE_VOLUME,
        ENTRY_THRESHOLD_VOLUME,
        EXIT_THRESHOLD_VOLUME
    }

    public enum PREVIOUS_PRICE_COMPARE_ANALYSIS
    {

        OFFICE_ACCT,
        EOD_SETTLE,
        SPAN_SETTLE,
        PRICE_DIFF,
        PL_DIFF,
        DATE,
        PL,
        LONG_ORDER_PL,
        SHORT_ORDER_PL,
        TOTAL_QTY,
        LONG_ORDER_QTY,
        SHORT_ORDER_QTY,
        LONG_AVG_PRICE,
        SHORT_AVG_PRICE,
        
        PREVIOUS_DAY_SETTLE,
        SETTLE_IMPLIED_VOL,
        
        SPAN_IMPLIED_VOL,
        
        INSTRUMENT_ID
    }

    public enum PREVIOUS_PL_COMPARE_ANALYSIS
    {
        PL,
        PL_DIFF,
        PL_NET
    }

    public enum EXPRESSION_LIST_VIEW
    {
        TIME,

        MANUAL_STTLE,
        STTLE,
        SETL_TIME,
        YEST_STTLE,

        DELTA,

        TRANS_PRICE,
        ORDER_PL,

        DFLT_PRICE,
        CUM_VOL,

        THEOR_PRICE,
        SPAN_IMPL_VOL,
        SETL_IMPL_VOL,
        IMPL_VOL,

        BID,
        ASK,
        LAST,
        

        INSTRUMENT_ID,
        EXPRESSION_IDX
    }

    public class TradingSystemConstants
    {
        public const int MILLISECOND_CQG_CALL_PAUSE = 1000;
        public const int BAR_TO_LOOKBACK_FOR_CQG_REQUEST = -1700;

        public const int SPREAD_ID_INSTRUMENT_GROUP = 7;
        public const int INITIAL_ARRAY_SIZE = 2500;
        public const int INITIAL_ARRAY_SIZE_BACKTESTING = 50000;

        public const int MINUTES_IN_DAY = 1440;
        public const int SESSION_RESET_MINUTE_IN_DAY = 930;
        public const int SESSION_START_HOUR = 15;
        public const int SESSION_START_MIN = 30;

        public const int ENTRY_ORDER_PRE_TIME_EXIT_ENTRY_BUFFER = 5;

        public const int PANDF_NUMBER_OF_COLUMNS_PERSISTENT = 5;

        public const byte TS_LONG = 0;
        public const byte TS_SHORT = 1;

        public const byte BUY_TYPE = 0;
        public const byte SELL_TYPE = 1;

        public const byte CCIXTYPEUP = 0;
        public const byte CCIXTYPEDN = 1;

        public const String tt_buySymbol = "B";
        public const String tt_sellSymbol = "S";

        public const int MARKET_PROFILE_INITIAL_ARRAY_SIZE_LIVE = 3;
        public const int MARKET_PROFILE_INITIAL_ARRAY_SIZE_BACKTESTING = 1000;

        public const int MARKET_PROFILE_DAY_ARRAY_LENGTH = 1000;
        public const int MARKET_PROFILE_DAY_ARRAY_MID_IDX = MARKET_PROFILE_DAY_ARRAY_LENGTH / 2;

        public const int DATAIMPORT_SIGDIGITS = 10;

        public const int VOLSYS_INIT_LIVE_LENGTH = 10;
        public const int VOLSYS_INIT_BACKTEST_LENGTH = 200;

        public const byte VOLSYS_TREND_UP = 0;
        public const byte VOLSYS_TREND_DN = 1;

        public const byte CRB_DIRECTION_UP = 0;
        public const byte CRB_DIRECTION_DN = 1;

        public const byte EXT_PERIODICITY_X_BAR_START = 0;
        public const byte EXT_PERIODICITY_X_BAR_STOP = 1;
        public const byte EXT_PERIODICITY_X_BAR_CALCULATED = 2;

        public const double EPSILON = 0.000000001;
        //public const double EPSILON = 0.000001;

        public const byte NUMBER_OF_PARAMETER_DAYS = 7;
        public const byte DEFAULT_DAY_IDX = 0;

        public const byte CRB_BUILD_TYPE_DEFAULT = 0;
        public const byte CRB_BUILD_TYPE_OPPOSITE = 1;

        public const String BACKTESTING_BLANK_STRATEGY = "BACKTESTING____";

        public const byte ENTRY_TYPE = 0;
        public const byte EXIT_TYPE = 1;

        public const byte X_AXIS_TYPE_COUNT = 0;
        public const byte X_AXIS_TYPE_TIME = 1;
        public const byte X_AXIS_TYPE_DATE = 2;

        public const byte MAX_OPTION_LEGS = 10;

        public const byte INSTRUMENT_PL_AT_OPEN_IDX = 0;
        public const byte INSTRUMENT_PL_AT_CLOSE_IDX = 1;

        public const byte STRATEGY_LIST_DATE_TIME_IDX = 0;
        public const byte STRATEGY_LIST_TSDATA_IDX = 1;

//         public const byte TBL_INSTRUMENTS_ID_INSTRUMENT_IDX = 0;
//         public const byte TBL_INSTRUMENTS_CQG_SYMBOL_IDX = 1;
//         public const byte TBL_INSTRUMENTS_ID_BARINFO_IDX = 2;

        public const long CQG_DATA_ERROR_CODE = 2147483648;
        public const long TML_ERROR_CODE = 10000000;

        public const byte TBLBARINFO_PERIODICITYTYPE_MINUTES = 2;

        public const byte TBLBARINFO_TYPE_CONTINUOUS_CONTRACT = 0;
        public const byte TBLBARINFO_TYPE_DISABLED_CONTRACT = 1;
        public const byte TBLBARINFO_TYPE_DISCRETE_CONTRACT = 2;

        public const byte OPTION_AND_FUTURE_CONTRACT_DATA_QUERY_PROGRESS = 1;
        public const byte OPTION_AND_FUTURE_CONTRACT_DATA_QUERY_PROGRESS_COMPLETE = 2;
        public const byte CONTINUOUS_DATA_QUERY_PROGRESS_TYPE_FUTURES = 3;
        public const byte CONTINUOUS_FUTURES_DATA_QUERY_PROGRESS_TYPE_COMPLETE = 4;
        public const byte SYMBOL_LOADING_MAINTENANCE = 5;
        public const byte DATA_FILLING_CUSTOM_DAY_BARS_PROGRESS = 6;
        public const byte DATA_FILLING_CUSTOM_DAY_BARS_PROGRESS_COMPLETE = 7;

        public const byte DATA_COLLECTION_TYPE_INITIALIZE_INSTRUMENT = 1;
        public const byte DATA_COLLECTION_TYPE_FUTURES = 2;
        public const byte DATA_COLLECTION_TYPE_FUTURES_ROLLED = 3;
        public const byte DATA_COLLECTION_TYPE_OPTION_INPUT_SYMBOLS = 4;
        public const byte DATA_FILLING_CUSTOM_DAY_BARS = 5;

        public const byte DATA_COLLECTION_DAILY_BARS = 0;
        public const byte DATA_COLLECTION_MINUTE_BARS = 1;

        public const byte OPTIMIZE_UI_CURRENT_SYSTEM_TYPE_GENERAL = 0;
        public const byte OPTIMIZE_UI_CURRENT_SYSTEM_TYPE_SPREAD_GENERATION = 1;
        public const byte OPTIMIZE_UI_CURRENT_SYSTEM_TYPE_OPTION = 2;

        public const String INITIALIZE_CONFIG_DIRECTORY = "INITIALIZE_SYSTEM_config";
        //public const String INITIALIZE_REALTIME_FILE_NAME = "INITIALIZE_REALTIME_SYSTEM_config.cfg";
        //public const String INITIALIZE_BACKTEST_FILE_NAME = "INITIALIZE_BACKTEST_SYSTEM_config.cfg";
        public const String INITIALIZE_OPTION_REALTIME_FILE_NAME = "INITIALIZE_OPTION_REALTIME_SYSTEM_config.cfg";

        public const string INSTRUMENT_SPECIFIC_FIX_FIELDS = "INSTRUMENT_SPECIFIC_FIX_FIELDS";

        public const string FCM_FTP = "FCM_FTP";

        //static const String^ LIVERUN_CONFIG_DIRECTORY = "LIVERUN_config";
        //public const String LIVERUN_CONFIG_FILE_NAME = "LIVERUN_config.cfg";
        //public const String DB_SERVER_LIST_CONFIG_FILE_NAME = "DB_SERVER_LIST_config.cfg";

        public const String SUPPLEMENT_CONTRACTS = "SUPPLEMENT_CONTRACTS.cfg";

        public const String REALTIME_CONFIGURATION = "REALTIME_CONFIGURATION.cfg";


        public const String FCM_DATA_FOLDER = "FCM_DATA_FOLDER";
        public const String FCM_EXCLUDE_CONTRACT_INFO_FILE = "FCM_EXCLUDE_CONTRACT_INFO_FILE.csv";
        public const String FCM_STORED_DATA_FILE = "FCM_STORED_DATA_FILE.txt";

        public const String DB_USERNAME = "steve";
        public const String DB_PASSWORD = "nj111174";
        public const String DB_DATABASENAME = "cqgdb";

        public const String DB_DEFAULTSERVERIP = "192.168.1.15";

        public const byte OPTION_SUBSCRIPTION_MAX_ATTEMPTS = 3;

        public const short STRATEGY_SUMMARY_FLASH_TIME = 5;

        //public const short OPTION_INPUT_TYPE_RISK_FREE_RATE = 1;
        //public const short OPTION_INPUT_TYPE_CALL_IMPLIED_VOL = 2;
        //public const short OPTION_INPUT_TYPE_PUT_IMPLIED_VOL = 3;

        public const short DAYS_IN_MONTH = 30;
        public const short DAYS_IN_YEAR = 365;

        public const double OPTION_DELTA_MULTIPLIER = 100;

        public const double OPTION_MARGIN_MULTIPLIER = 3;

        public const int PORTFOLIO_SUMMARY_STARTUP_GRID_WIDTH = 50;

        //public const int OPTIONREALTIMEREFRESH = 5000;

        public const int STRIKE_PRICE_REFERENCE_COUNT = 7;
        public const String N_A = "N/A";
        public const double OPTION_DEFAULT_THEORETICAL_PRICE_RANGE = 0.17;
        public const double OPTION_ZERO_PRICE = 0.000001;
        public const double OPTION_ACCEPTABLE_BID_ASK_SPREAD = 20;
        public const int OPTIONREALTIMEREFRESH = 5000;

        public const int MODEL_CALC_TIME_REFRESH = 5000;

        public const int MINUTES_STALE_LIVE_UPDATE = 15;

        public const int ACTUAL_FUTURES_WINDOW_WIDTH = 215;

        public const int STRIKE_COUNT_FOR_DEFAULT_TO_THEORETICAL = 4;
    };
}
