using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RealtimeSpreadMonitor
{
    public class ADM_Input_Data
    {
        public DateTime dateTime;

        public int instrumentArrayIdx;

        public Instrument instrument;


        public String cqgSymbol;
        
        public String cqgSubstituteSymbol;


        public bool isFuture;

        public OPTION_SPREAD_CONTRACT_TYPE callPutOrFuture;

        public double strikeInDecimal;

        public String strike;

        public LegInfo contractInfo = new LegInfo();
        public LegData contractData = new LegData();

        public double modelLots;
        public double orderLots;
        public double rollIntoLots;
        public double rebalanceLots;

        //used for rebalance fcm and model compare
        //****************************************
        
        public double rebalanceLotsForPayoffNoOrders;
        public double rebalanceLotsForPayoffWithOrders;
        public int contractYear;
        public int contractMonth;
        public int optionYear;
        public int optionMonth;
        public double yearFraction;
        public double futurePriceUsedToCalculateStrikes; //only used for fcm model compare roll into contracts that are new
        //*****************************************



        public int liveADMRowIdx;

        public double netContractsEditable;


        //public List<double>

        public double transAvgLongPrice;
        public double transAvgShortPrice;

        public int transNetLong;
        public int transNetShort;

        public double Net;


        public OptionSpreadExpression optionSpreadExpression;

        public double modelPL;
        public double fcmPL;

        //public bool exclude; //this is used in the model ADM compare and specifies whether the trade is done
    }

    public class ADMPositionImportWeb : ADM_Input_Data
    {
        public String RecordType;

        public string MODEL_OFFICE_ACCT;
        public int acctGroup;

        public String POFFIC;
        public String PACCT;
        public String PCUSIP;
        public String PCUSIP2;
        public String Description;
        public double LongQuantity;
        public double ShortQuantity;
        
        public String TradeDate;
        public DateTime TradeDate_datetime;

        public double TradePrice;
        public double TradePriceToSave;
        //public double Net;
        public double WeightedPrice;
        //public double AveragePrice;
        public double RealTimePrice;
        public double SettledPrice;
        public double PrelimPrice;
        public double Value;
        public double ClosedValue;
        public double SettledValue;
        public String Currency;
        public String PSUBTY;
        public int PEXCH;
        public String PFC;

        public String aDMStrike;

        public String PCTYM;
        public DateTime PCTYM_dateTime;

        public String PCARD;

    }

    public class ADM_DAYDTN_Import : ADM_Input_Data
    {
        public String PRECID_recordIdCode;           //1
        public String PFIRM_firmLetterCode;           //2
        public String POFFIC_officeCode;           //3
        public String PACCT_acctNum;           //4
        public String PATYPE_acctType;           //5
        public String PCUSIP_cusipNum;           //6
        public DateTime PCTYM_contractYearMonth;           //7
        public String PSBCUS_subCusip;           //8
        public String PSTYPE_securityType;           //9
        public String PSUBTY_securitySubType;           //10
        public String PSTYP2_securitySubType2;           //11
        public double PSTRIK_strikePrice;           //12
        public DateTime PEXPDT_expDate;           //13
        public DateTime PTDATE_tradeDate;           //14
        public double PTPRIC_tradePrice;           //15
        public int PBS_buySellCode;           //16
        public String PSPRED_spreadCode;           //17
        public String PACNTY_acctCntryCode;           //18
        public String PRR_salesmanCode;           //19
        public double PQTY_quantity;           //20
        public String PSDSC1_securityDesc1;           //21
        public String PSDSC2_securityDesc2;           //22
        public double PMKVAL_marketValue;           //23
        public double PYSTMV_prevMarketValue;           //24
        public double PMULTF_multFactor;           //25
        public DateTime PSDATE_settleDate;           //26
        public double PDELTA_optionDelta;           //27
        public DateTime PLTDAT_lastTradingDate;           //28
        public String PPCNTY_productCntryCode;           //29
        public String PEXCH_exchgCode_admExchangeCode;           //30
        public String PFC_futuresCode_admCode;           //31
        public String PTYPE_tradeType;           //32
        public String PSYMBL_symbol;           //33
        public String PPTYPE_productType;           //34
        public String PSUBEX_subExchg;           //35
        public DateTime PMATDT_maturityDate;           //36
        public String PPRTPR_printablePrice;           //37
        public double PBASIS_basisPrice;           //38
        public double PCLOSE_closingPrice;           //39
        public DateTime PPRCDT_datePriced;           //40
        public String PPRTCP_printableClosePrice;           //41
        public double PUNDCP_underlyingClosePrice;           //42
        public String PPRTUC_printableUnderlyingClose;           //43
        public double PPRVCP_prevClosingPrice;           //44
        public String PCURSY_productCrncySymbol;           //45
        public String PCURCD_productCrncyCode;           //46
        public String PCMNT1_commentCode1;           //47
        public String PCMNT2_commentCode2;           //48
        public String PCMNT3_commentCode3;           //49
        public String PRTHT_round_half_Turn;           //50
        public String POC_openCloseCode;           //51
        public String PGIVIO_giveInOut;           //52
        public String PGIVF_giveInOutFirm;           //53
        public String PCARD_cardNumber;           //54
        public double PCOMM_commissionAmt;           //55
        public double PFEE1;           //56
        public double PFEE2;           //57
        public double PFEE3;           //58
        public double PFEE4;           //59
        public double PFEE5;           //60
        public double PFEE6;           //61
        public double PFEE7;           //62
        public double PFEE8;           //63
        public double PFEE9;           //64
        public double PGICHG_brokerageCharge;           //65
        public double PBKCHG_brokerageCharge;           //66
        public double POTHER_otherFees;           //67
        public double PGROSS_grossAmt;           //68
        public double PNET_netAmt;           //69
        public String PATCOM_acctTypeForCommission;           //70
        public String PATFE1_acctTypeForFees;           //71
        public double PBQTY_buyQty;           //72
        public double PSQTY_sellQty;           //73
        public String PTRACE_traceNumber;           //74
        public double PMVARN_variationDur;           //75
        public String PUNDCN_isinNumber;           //76
        public String PCURAT_currencyCode;           //77
        public String PLOC_acctLocationCode;           //78
        public String PCLASS_acctLocationCode;           //79
        public DateTime PTIME_executionTime;           //80

        //public String aDMStrike;
        //public double strikeInDecimal;
        //public String strike;

        //public String PCTYM;

        //public int instrumentArrayIdx;

        //public Instrument instrument;

        ////public double notAllocated;

        ////public String cqgSymbol;

        //public bool isFuture;

        //public int callPutOrFuture;
    }

    public class ADMLegInfoData
    {
        //public LegInfo legInfo = new LegInfo();
        //public LegData legData = new LegData();

        public ADMPositionImportWeb aDMPositionImportWeb;

        public bool loadedFromModel;

        public bool notInADMPositionsWebData;

        //public bool strategyLegNotInCurrentModel = true;

        //public Color backgroundColor;

        public int rowIndex;

        //only save as csv if this value != 0
        public int numberOfContracts;

        //public int modelFutures;

        public int numberOfModelContracts;

        public OptionSpreadExpression optionSpreadExpression;
    }


    //public class LiveADMStrategyInfo
    //{
    //    public int idStrategy;
    //    public int idInstrument;

    //    public int indexOfInstrumentInInstrumentsArray;

    //    public bool holdsPosition;

    //    public OptionStrategy optionStrategy = null;
    //    public Instrument instrument = null;

    //    //public List<ADMLegInfoData> admLegInfo = new List<ADMLegInfoData>(5);

    //    public ADMLegInfoData admLegInfo = new ADMLegInfoData();

    //    public LiveSpreadTotals deltaHedgeADMTotals = new LiveSpreadTotals();
    //    public LiveSpreadTotals liveSpreadADMTotals = new LiveSpreadTotals();

    //    //public int topRowIdx;
    //    //public int summaryRowIdx;
    //}

    public class ADMDragOverData
    {
        public ADM_IMPORT_FILE_TYPES fileType;
        public int row;
    }

//     public class ADMGridDataMap
//     {
//         public int idStrategy;
//         public OPTION_LIVE_ADM_DATA_MAP_ROW_TYPE rowType;
//     }

    internal enum BrokerImportFiles
    {
        ADM_WEB_IMPORT_FILES,
        GMI_IMPORT_CSV_FILES,
        BACKUP_SAVED_FILE
    }

    internal enum FTPInputFileTypes
    {
        ADM_WEB_INTERFACE_FILES,
        POSITION_FILE_WEDBUSH,
        TRANSACTION_FILE_WEDBUSH,
        POSITION_FILE_ADM,
        TRANSACTION_FILE_ADM
    }

    internal class ImportFileCheck
    {
        internal bool importfile = false;
        //internal BrokerImportFiles brokerImportFiles;
        //internal FTPInputFileTypes cSVImportFileType;
        internal bool importingBackedUpSavedFile = false;
    }

}
