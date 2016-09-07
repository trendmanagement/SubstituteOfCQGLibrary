using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RealtimeSpreadMonitor.Mongo
{
    class MongoObjectsForRealtime
    {
    }

    public class MongoDB_OptionSpreadExpression
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        //[BsonElement("cqgSymbol")]
        public string cqgSymbol { get; set; }

        public int idInstrument;

        public Instrument instrument;

        //this is used for margin calculation from option payoff chart
        //******************************************
        public int optionMonthInt;
        public int optionYear;

        public int futureContractMonthInt;
        public int futureContractYear;
        //******************************************

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

        public double minutesSinceLastUpdate = 0;

        public DateTime lastTimeFuturePriceUpdated; //is separate b/c can get time stamp off of historical bars


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
    }
}
