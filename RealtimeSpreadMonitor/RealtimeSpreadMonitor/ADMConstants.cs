using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RealtimeSpreadMonitor
{
    class ADMConstants
    {
    }

    public enum ADM_IMPORT_FILE_TYPES
    {
        ADM_WEB_POSITIONS,
        ADM_FTP_ADATDTN
    }

    public enum OPTION_LIVE_ADM_DATA_MAP_ROW_TYPE
    {
        SPREAD_LEG,
        SUMMARY_ROW
    }

    public enum ADM_SUMMARY_WEB_POSITION_FIELDS_DISPLAYED
    {
        RecordType,
        //CQG_SYMBOL,

        Office,
        Acct,

        Description,
        LongQuantity,
        ShortQuantity,

        //AveragePrice,
        TradeDate,
        TradePrice,
        WeightedPrice,
        //RealTimePrice,
        SettledPrice,
        SettledValue,
        Currency,
        PFC,
        Strike,
        PCTYM,
        INSTRUMENT_ID
    }

    public enum ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED
    {
        //CQG_SYMBOL,
        //Description,
        
        MODEL_OFFICE_ACCT,
        FCM_OFFICE_ACCT,
        MODEL_PL,        
        FCM_PL,
        DIFF_PL,
        MODEL,
        NEW_ORDERS,
        ROLL_INTO_ORDERS,
        FCM,
        REBALANCE,
        //AveragePrice,
        //SettledPrice,
        //SettledValue,
        //Currency,
        //PFC,
        Strike,
        
        ZERO_PRICE,
        EXCEPTIONS,

        INSTRUMENT_ID,
        ACCT_GROUP_IDX
    }

    public enum ADM_SUMMARY_FIELDS
    {
        RecordType,
        PCUSIP,
        PCUSIP2,
        Description,
        LongQuantity,
        ShortQuantity,
        Net,
        WeightedPrice,
        AveragePrice,
        RealTimePrice,
        SettledPrice,
        PrelimPrice,
        Value,
        ClosedValue,
        SettledValue,
        Currency,
        PSUBTY,
        PEXCH,
        PFC,
        Strike,
        PCTYM,
    }

    public enum ADM_DETAIL_FIELDS
    {
        RecordType,
        POFFIC,
        PACCT,
        PCUSIP,
        PCUSIP2,
        Description,
        LongQuantity,
        ShortQuantity,
        TradeDate,
        TradePrice,
        WeightedPrice,
        RealTimePrice,
        SettledPrice,
        PrelimPrice,
        Value,
        ClosedValue,
        SettledValue,
        Currency,
        PSUBTY,
        PEXCH,
        PFC,
        Strike,
        PCTYM,
        PCARD
    }

    public enum OPTION_LIVE_ADM_DATA_COLUMNS
    {

        TIME,
        POFFIC,
        PACCT,
        NET_AT_ADM,
        NET_EDITABLE,
        LONG_TRANS,
        SHORT_TRANS,

        PL_DAY_CHG,
        PL_TRANS,        

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


        //STRIKE_PRICE,
        //YEST_PRICE,
        RFR,

        CNTDN,
        EXPR,

        AVG_LONG_TRANS_PRC,
        AVG_SHORT_TRANS_PRC,

        STRIKE,
        DESCRIP,
        CUSIP,
        ADMPOSWEB_IDX,
        INSTRUMENT_ID,
    };
}
