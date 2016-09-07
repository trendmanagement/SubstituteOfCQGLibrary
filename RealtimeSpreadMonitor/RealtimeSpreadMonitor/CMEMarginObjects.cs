using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RealtimeSpreadMonitor
{
    class Analytic
    {
        public DateTime createTime { get; set; }
        public string id { get; set; }
        public string portfolioId { get; set; }
        public AnalyticType type { get; set; }
        public DateTime updateTime { get; set; }
    }

    class Error
    {
        public string msg { get; set; }
        public int code { get; set; }
    }

    class Margin
    {

        public MarginAmounts amounts { get; set; }
        //public DateTime asOftime { get; set; }
        //public DateTime createTime { get; set; }
        //public string id { get; set; }
        //public string portfolioId { get; set; }
        //public SettlementIndicator settleInd { get; set; }
        //public SettlementQualifier settleQual { get; set; }
        //public DateTime updateTime { get; set; }
        public Transaction[] transactions { get; set; }
    }

    class MarginAmounts
    {
        public double conc { get; set; }
        public CMECurrency ccy { get; set; }
        public double init { get; set; }
        public double maint { get; set; }
        public double nonOptVal { get; set; }
        public double optVal { get; set; }
        public ScenarioPoint scPt { get; set; }
    }

    class Portfolio
    {
        public DateTime createTime { get; set; }
        public string desc { get; set; }
        public PortfolioEntities entities { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public CMECurrency rptCcy { get; set; }
        public DateTime updateTime { get; set; }
    }

    class PortfolioEntities
    {
        public string clrMbrFirmId { get; set; }
        public string custAcctId { get; set; }
    }

    class PortfolioMarginAmounts
    {
        public double futPreAmt { get; set; }
        public double futPostAmt { get; set; }
        //public double futPreAmt { get; set; }
        public double otcPostAmt { get; set; }
        public double diffAmt { get; set; }
    }

    class ScenarioPoint
    {
        public double amt { get; set; }
    }

    class Transaction
    {
        public string id { get; set; }
        //public DateTime createTime { get; set; }
        //public EntityStatus status { get; set; }
        //public DateTime updatetime { get; set; }
        //public string portfolioId { get; set; }
        public TransactionType type { get; set; }
        //public Error error { get; set; }
        public TransactionPayload payload { get; set; }
    }

    class TransactionPayload
    {
        public TransactionFormat format { get; set; }
        public string encoding { get; set; }
        public String STRING { get; set; }
    }

    public class StringWriterWithEncoding : StringWriter
    {
        private Encoding myEncoding;
        public override Encoding Encoding
        {
            get
            {
                return myEncoding;
            }
        }
        public StringWriterWithEncoding(Encoding encoding)
            : base()
        {
            myEncoding = encoding;
        }
    }
}
