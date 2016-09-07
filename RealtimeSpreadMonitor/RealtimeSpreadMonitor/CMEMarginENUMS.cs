using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeSpreadMonitor
{
    //class CMEMarginENUMS
    //{
    //}

    public enum AnalyticType
    {
        PMOPT
    }

    public enum AsyncReportStatus
    {
        ERROR,
        PROCESSING,
        SUCCESS
    }

    public enum CMECurrency
    {
        AUD,
        BRL,
        CAD,
        CHF,
        CHP,
        EUR,
        GBP,
        JPY,
        NZD,
        NOK,
        SEK,
        USD,
    }

    public enum SettlementIndicator
    {
        N, Y
    }

    public enum SettlementQualifier
    {
        COMP,
        EARLY,
        FINAL
    }

    public enum TransactionType
    {
        DELTA_LADDER,
        TRADE
    }

    public enum EntityStatus
    {
        DELETED,
        ERROR,
        INSERTED,
        UPDATED
    }

    public enum TransactionFormat
    {
        CSV,
        FIXML,
        FPML
    }
}
