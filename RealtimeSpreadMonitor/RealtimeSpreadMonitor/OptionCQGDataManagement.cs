using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CQG;
using RealtimeSpreadMonitor.Forms;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;
using CQGLibrary;

namespace RealtimeSpreadMonitor
{
    class OptionCQGDataManagement
    {
        OptionSpreadManager optionSpreadManager;
        //OptionRealtimeStartup optionRealtimeStartup;
        List<OptionSpreadExpression> optionSpreadExpressionList;

        ConcurrentDictionary<string, int> optionSpreadExpressionFutureTimedBarsListIdx = new ConcurrentDictionary<string, int>();

        ConcurrentDictionary<string, int> optionSpreadExpressionCheckSubscribedListIdx = new ConcurrentDictionary<string, int>();
        ConcurrentDictionary<string, int> optionSpreadExpressionListHashTableIdx = new ConcurrentDictionary<string, int>();

        //CQG.CQGCEL m_CEL;
        CQGLibrary.CQG.CQGCEL m_CEL_fake;

        private bool resetOrderPageOnceAllDataIn = false;

        //DataErrorCheck dataErrorCheck;
        //List<OptionSpreadExpressionList> optionSpreadExpressionList;
        //List<CurrentDateContractListMap> currentDateContractListMainIdx;
        //OptionBuildCommonMethods optionBuildCommonMethods;

        //List<int> tempListOfExpressionsFilled = new List<int>();

        //delegate void ThreadSafeSendSubscribeRequestRunDelegate(bool sendOnlyUnsubscribed);

        private Thread subscriptionThread;
        private bool subscriptionThreadShouldStop = false;
        private const int SUBSCRIPTION_TIMEDELAY_CONSTANT = 125;

        private Thread calculateModelValuesAndSummarizeTotalsThread;
        private bool calculateModelValuesThreadShouldStop = false;
        //private const int 

        //private OptionSpreadExpressionList optionSpreadExpressionBarRequest;
        //private int barDataExpressionCounter;

        public OptionCQGDataManagement(
            OptionSpreadManager optionSpreadManager,
            OptionRealtimeStartup optionRealtimeStartup,
            List<OptionSpreadExpression> optionSpreadExpressionList)
        {
            this.optionSpreadManager = optionSpreadManager;

            //this.optionRealtimeStartup = optionRealtimeStartup;

            this.optionSpreadExpressionList = optionSpreadExpressionList;

            setupCalculateModelValuesAndSummarizeTotals();
        }

        public void stopDataManagementAndTotalCalcThreads()
        {
            if (subscriptionThread != null && subscriptionThread.IsAlive)
                subscriptionThreadShouldStop = true;

            if (calculateModelValuesAndSummarizeTotalsThread != null && calculateModelValuesAndSummarizeTotalsThread.IsAlive)
                calculateModelValuesThreadShouldStop = true;

            //Thread.Sleep(TradingSystemConstants.MODEL_CALC_TIME_REFRESH);
        }

        public void resetThreadStopVariables()
        {
            subscriptionThreadShouldStop = false;

            calculateModelValuesThreadShouldStop = false;
        }

        public void shutDownCQGConn()
        {
            if (optionSpreadManager.optionRealtimeMonitor != null)
            {
                optionSpreadManager.optionRealtimeMonitor.updateCQGDataStatus("CQG DATA DOWN", Color.Black, Color.Red);
            }

            if (m_CEL_fake != null)
            {
                if (m_CEL_fake.IsStarted)
                    m_CEL_fake.RemoveAllInstruments();

                //m_CEL.Shutdown();
            }


        }

        public void connectCQG()
        {
            if (m_CEL_fake != null)
            {
                m_CEL_fake.Startup();
            }

            if (optionSpreadManager.optionRealtimeMonitor != null)
            {
                optionSpreadManager.optionRealtimeMonitor.updateCQGDataStatus("CQG UP", Color.Black, Color.Yellow);
            }
        }

        public void resetCQG()
        {
            if (!m_CEL_fake.IsStarted)
            {
                initializeCQGAndCallbacks();
            }
        }

        public void initializeCQGAndCallbacks()
        {
            try
            {

                m_CEL_fake = new CQGLibrary.CQG.CQGCEL();

                m_CEL_CELDataConnectionChg(CQGLibrary.CQG.eConnectionStatus.csConnectionDown);
                //(callsFromCQG,&CallsFromCQG.m_CEL_CELDataConnectionChg);

                m_CEL_fake.DataConnectionStatusChanged += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_DataConnectionStatusChangedEventHandler(m_CEL_CELDataConnectionChg);

                //m_CEL.LineTimeChanged += new CQG._ICQGCELEvents_LineTimeChangedEventHandler(m_CEL_LineTimeChanged);

                // 		//m_CEL.DataError += new _ICQGCELEvents_DataErrorEventHandler(CEL_DataError);
                //m_CEL.InstrumentsGroupResolved += new CQG._ICQGCELEvents_InstrumentsGroupResolvedEventHandler(m_CEL_InstrumentsGroupResolved);
                // 		m_CEL.InstrumentsGroupChanged += new _ICQGCELEvents_InstrumentsGroupChangedEventHandler(m_CEL_InstrumentsGroupChanged);

                m_CEL_fake.TimedBarsResolved += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_TimedBarsResolvedEventHandler(m_CEL_TimedBarResolved);
                m_CEL_fake.TimedBarsAdded += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_TimedBarsAddedEventHandler(m_CEL_TimedBarsAdded);
                m_CEL_fake.TimedBarsUpdated += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_TimedBarsUpdatedEventHandler(m_CEL_TimedBarsUpdated);

                //m_CEL.IncorrectSymbol += new _ICQGCELEvents_IncorrectSymbolEventHandler(CEL_IncorrectSymbol);
                m_CEL_fake.InstrumentSubscribed += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_InstrumentSubscribedEventHandler(m_CEL_InstrumentSubscribed);
                m_CEL_fake.InstrumentChanged += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_InstrumentChangedEventHandler(m_CEL_InstrumentChanged);

                m_CEL_fake.DataError += new CQGLibrary.CQG.CQGCEL._ICQGCELEvents_DataErrorEventHandler(m_CEL_DataError);

                //m_CEL.APIConfiguration.NewInstrumentMode = true;

                m_CEL_fake.APIConfiguration.ReadyStatusCheck = CQGLibrary.CQG.eReadyStatusCheck.rscOff;

                m_CEL_fake.APIConfiguration.CollectionsThrowException = false;

                m_CEL_fake.APIConfiguration.TimeZoneCode = CQGLibrary.CQG.eTimeZone.tzPacific;
                //m_CEL.Startup();

                connectCQG();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }
        
        private void m_CEL_CELDataConnectionChg(CQGLibrary.CQG.eConnectionStatus new_status)
        {
            StringBuilder connStatusString = new StringBuilder();
            StringBuilder connStatusShortString = new StringBuilder();
            Color connColor = Color.Red;
            
            try
            {
                if (m_CEL_fake.IsStarted)
                {
                    if (optionSpreadManager.optionRealtimeMonitor != null)
                    {
                        optionSpreadManager.optionRealtimeMonitor.updateCQGReConnectBtn(false);
                    }

                    connStatusString.Append("CQG API:");
                    connStatusString.Append(m_CEL_fake.Environment.CELVersion);
                    connStatusShortString.Append("CQG:");

                    if (new_status != CQGLibrary.CQG.eConnectionStatus.csConnectionUp)
                    if (new_status != CQGLibrary.CQG.eConnectionStatus.csConnectionUp)
                    {
                        if (new_status == CQGLibrary.CQG.eConnectionStatus.csConnectionDelayed)
                        {
                            connColor = Color.BlanchedAlmond;
                            connStatusString.Append(" - CONNECTION IS DELAYED");
                            connStatusShortString.Append("DELAYED");
                        }
                        else
                        {
                            connStatusString.Append(" - CONNECTION IS DOWN");
                            connStatusShortString.Append("DOWN");
                        }
                    }
                    else
                    {
                        connColor = Color.LawnGreen;
                        connStatusString.Append(" - CONNECTION IS UP");
                        connStatusShortString.Append("UP");
                    }
                }
                else
                {
                    connStatusString.Append("WAITING FOR API CONNECTION");

                    connStatusShortString.Append("WAITING");

                    if (optionSpreadManager.optionRealtimeMonitor != null)
                    {
                        optionSpreadManager.optionRealtimeMonitor.updateCQGReConnectBtn(true);
                    }
                }

                optionSpreadManager.ConnectionStatusString = connStatusString.ToString();
                optionSpreadManager.ConnectionStatusShortString = connStatusShortString.ToString();
                optionSpreadManager.ConnectionStatusColor = connColor;

                if (optionSpreadManager.optionRealtimeMonitor != null)
                {
                    optionSpreadManager.optionRealtimeMonitor.updateCQGConnectionStatus(
                        connStatusString.ToString(), connColor, connStatusShortString.ToString());
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void m_CEL_DataError(System.Object cqg_error, System.String error_description)
        {
            try
            {
                if (optionSpreadManager.optionRealtimeMonitor != null)
                {
                    optionSpreadManager.optionRealtimeMonitor.updateCQGDataStatus("CQG ERROR", Color.Yellow, Color.Red);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void m_CEL_InstrumentSubscribed(String symbol, CQGLibrary.CQGInstrument cqgInstrument)
        {
            try
            {
                //TSErrorCatch.debugWriteOut("symbol " + symbol + " ");
                if (optionSpreadManager.optionRealtimeMonitor != null)
                {
                    optionSpreadManager.optionRealtimeMonitor.updateCQGDataStatus("CQG GOOD", Color.Black, Color.LawnGreen);
                }

                //optionSpreadExpressionFutureTimedBarsListIdx

                int expressionCounter = 0;

                if (optionSpreadExpressionCheckSubscribedListIdx.Count > 0
                        &&
                        optionSpreadExpressionCheckSubscribedListIdx.ContainsKey(symbol))
                {

                    //optionSpreadExpressionCheckSubscribedListIdx

                    expressionCounter = optionSpreadExpressionCheckSubscribedListIdx[symbol];

                    //while (expressionCounter < optionSpreadExpressionList.Count)
                    //{
                    if (!optionSpreadExpressionList[expressionCounter].stopUpdating
                        //&& symbol.CompareTo(optionSpreadExpressionList[expressionCounter].cqgSymbol) == 0
                        && !optionSpreadExpressionList[expressionCounter].setSubscriptionLevel)
                    {
                        optionSpreadExpressionList[expressionCounter].setSubscriptionLevel = true;

                        optionSpreadExpressionList[expressionCounter].cqgInstrument = cqgInstrument;


                        int idx = expressionCounter;

                        optionSpreadExpressionListHashTableIdx.AddOrUpdate(
                                cqgInstrument.FullName, idx,
                                (oldKey, oldValue) => idx);

                        //if (cqgInstrument.FullName.CompareTo("P.US.EU6J1511100") == 0)
                        //{
                        //    Console.WriteLine(cqgInstrument.FullName);
                        //}

                        fillPricesFromQuote(optionSpreadExpressionList[expressionCounter],
                            optionSpreadExpressionList[expressionCounter].cqgInstrument.Quotes);

                        //if is an option (not a future)
                        if (optionSpreadExpressionList[expressionCounter].callPutOrFuture !=
                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            fillDefaultMidPrice(optionSpreadExpressionList[expressionCounter]);

                            manageExpressionPriceCalcs(optionSpreadExpressionList[expressionCounter]);
                        }


                        if (optionSpreadExpressionList[expressionCounter].optionExpressionType
                            == OPTION_EXPRESSION_TYPES.SPREAD_LEG_PRICE)
                        {
                            //optionSpreadExpressionList[expressionCounter].updatedDataFromCQG = true;

                            if (optionSpreadExpressionList[expressionCounter].callPutOrFuture ==
                                    OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                optionSpreadExpressionList[expressionCounter].cqgInstrument.DataSubscriptionLevel
                                    = CQGLibrary.CQG.eDataSubscriptionLevel.dsQuotes;
                            }
                            else
                            {
                                optionSpreadExpressionList[expressionCounter].cqgInstrument.DataSubscriptionLevel
                                    = CQGLibrary.CQG.eDataSubscriptionLevel.dsQuotesAndBBA;

                                optionSpreadExpressionList[expressionCounter].cqgInstrument.BBAType
                                     = CQGLibrary.CQG.eDOMandBBAType.dbtCombined;
                            }

                        }
                        else
                        {
                            //TSErrorCatch.debugWriteOut(symbol + " interest rate no update");

                            //set updates for interest rate to stop
                            optionSpreadExpressionList[expressionCounter].cqgInstrument.DataSubscriptionLevel
                                = CQGLibrary.CQG.eDataSubscriptionLevel.dsNone;

                        }

                        //break;
                    }

                    //expressionCounter++;
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void m_CEL_InstrumentChanged(CQGLibrary.CQGInstrument cqgInstrument,
                                 CQGLibrary.CQGQuotes quotes,
                                 CQGLibrary.CQG.CQGInstrumentProperties props)
        {
            try
            {

                //TSErrorCatch.debugWriteOut("symbol " + cqgInstrument.FullName + " ");

                int expressionCounter = 0;

                //optionSpreadExpressionFutureTimedBarsListIdx

                //while (expressionCounter < optionSpreadExpressionList.Count)
                //{

                if (optionSpreadExpressionListHashTableIdx.Count > 0
                        &&
                        optionSpreadExpressionListHashTableIdx.ContainsKey(cqgInstrument.FullName))
                {

                    //optionSpreadExpressionCheckSubscribedListIdx

                    expressionCounter = optionSpreadExpressionListHashTableIdx[cqgInstrument.FullName];
                    
                    if (optionSpreadExpressionList!= null
                        && optionSpreadExpressionList.Count > expressionCounter
                        && optionSpreadExpressionList[expressionCounter] != null
                        && !optionSpreadExpressionList[expressionCounter].stopUpdating
                        && optionSpreadExpressionList[expressionCounter].cqgInstrument != null

                        && cqgInstrument.CEL != null)

                    //&& cqgInstrument.FullName.CompareTo(optionSpreadExpressionList[expressionCounter].cqgInstrument.FullName) == 0)
                    {
                        //optionSpreadExpressionList[expressionCounter].cqgInstrument = cqgInstrument;

                        //foreach (DictionaryEntry item in m_QuoteTypeToData)
                        {

                            CQGLibrary.CQGQuote quoteAsk = quotes[CQGLibrary.CQG.eQuoteType.qtAsk];
                            CQGLibrary.CQGQuote quoteBid = quotes[CQGLibrary.CQG.eQuoteType.qtBid];
                            CQGLibrary.CQGQuote quoteTrade = quotes[CQGLibrary.CQG.eQuoteType.qtTrade];
                            CQGLibrary.CQGQuote quoteSettlement = quotes[CQGLibrary.CQG.eQuoteType.qtSettlement];
                            CQGLibrary.CQGQuote quoteYestSettlement = quotes[CQGLibrary.CQG.eQuoteType.qtYesterdaySettlement];

                            if ((quoteAsk != null)
                                || (quoteBid != null)
                                || (quoteTrade != null)
                                || (quoteSettlement != null)
                                || (quoteYestSettlement != null))
                            {
                                //                                 if (optionSpreadExpressionList[expressionCounter].callPutOrFuture
                                //                                     != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                //                                 {
                                //                                     TSErrorCatch.debugWriteOut(
                                //                                         cqgInstrument.FullName + "  " +
                                //                                         optionSpreadExpressionList[expressionCounter].cqgInstrument.FullName + "  " +
                                //                                         optionSpreadExpressionList[expressionCounter].cqgSymbol + "  " +
                                //                                         "ASK " + ((quoteAsk != null && quoteAsk.IsValid) ? quoteAsk.Price.ToString() : "blank") + " " +
                                //                                         "BID " + ((quoteBid != null && quoteBid.IsValid) ? quoteBid.Price.ToString() : "blank") + " " +
                                //                                         "TRADE " + ((quoteTrade != null && quoteTrade.IsValid) ? quoteTrade.Price.ToString() : "blank") + " " +
                                //                                         "SETTL " + ((quoteSettlement != null && quoteSettlement.IsValid) ? quoteSettlement.Price.ToString() : "blank") + " " +
                                //                                         "YEST " + ((quoteYestSettlement != null && quoteYestSettlement.IsValid) ? quoteYestSettlement.Price.ToString() : "blank") + " "
                                //                                         );
                                //                                 }

                                //                                 quoteValue =
                                //                                     optionSpreadExpressionList[expressionCounter].cqgInstrument.ToDisplayPrice(quote.Price);

                                fillPricesFromQuote(optionSpreadExpressionList[expressionCounter],
                                    optionSpreadExpressionList[expressionCounter].cqgInstrument.Quotes);

                                if (optionSpreadExpressionList[expressionCounter].callPutOrFuture !=
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                {
                                    fillDefaultMidPrice(optionSpreadExpressionList[expressionCounter]);

                                    manageExpressionPriceCalcs(optionSpreadExpressionList[expressionCounter]);
                                }

                            }
                        }

                        //break;
                    }

                    //expressionCounter++;
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        /// <summary>
        /// M_s the ce l_ timed bar resolved.
        /// </summary>
        /// <param name="cqg_TimedBarsIn">The CQG_ timed bars in.</param>
        /// <param name="cqg_error">The cqg_error.</param>
        private void m_CEL_TimedBarResolved(CQGLibrary.CQGTimedBars cqg_TimedBarsIn, CQGLibrary.CQG.CQGError cqg_error)
        {
            //Debug.WriteLine("m_CEL_ExpressionResolved" + cqg_expression.Count);
            try
            {
                //TSErrorCatch.debugWriteOut(cqg_TimedBarsIn.Id);

                if (cqg_error == null)
                {
                    int expressionCounter = 0;

                    if (optionSpreadExpressionFutureTimedBarsListIdx.Count > 0
                        &&
                        optionSpreadExpressionFutureTimedBarsListIdx.ContainsKey(cqg_TimedBarsIn.Id))
                    {

                        expressionCounter = optionSpreadExpressionFutureTimedBarsListIdx[cqg_TimedBarsIn.Id];

                        //while (expressionCounter < optionSpreadExpressionList.Count)
                        //{
                        if (!optionSpreadExpressionList[expressionCounter].stopUpdating
                            && optionSpreadExpressionList[expressionCounter].futureTimedBars != null)
                        //&& cqg_TimedBarsIn.Id.CompareTo(optionSpreadExpressionList[expressionCounter].futureTimedBars.Id) == 0)
                        //&& !optionSpreadExpressionList[expressionCounter].requestedMinuteBars)
                        {
                            optionSpreadExpressionList[expressionCounter].requestedMinuteBars = true;

                            optionSpreadExpressionList[expressionCounter].futureBarData
                                = new List<OHLCData>(TradingSystemConstants.MINUTES_IN_DAY);

                            //OHLCData ohlcData = new OHLCData();

                            //optionSpreadExpressionList[expressionCounter].futureBarData.Add(ohlcData);

                            //TSErrorCatch.debugWriteOut("resolved " + cqg_TimedBarsIn.Id
                            //    + "  " + cqg_TimedBarsIn.Count);

                            if (cqg_TimedBarsIn.Count > 0)
                            {

                                int timeBarsIn_CurrentDay_TransactionIdx = cqg_TimedBarsIn.Count - 1;


                                //optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary
                                //    = cqg_TimedBarsIn[timeBarsIn_CurrentDay_TransactionIdx].Timestamp.Date
                                //    .AddHours(
                                //        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Hour)
                                //    .AddMinutes(
                                //        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Minute);

                                //optionSpreadExpressionList[expressionCounter].todayDecisionTime
                                //    = cqg_TimedBarsIn[timeBarsIn_CurrentDay_TransactionIdx].Timestamp.Date
                                //    .AddHours(
                                //        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Hour)
                                //    .AddMinutes(
                                //        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Minute
                                //        - optionSpreadExpressionList[expressionCounter].instrument.decisionOffsetMinutes);

                                
                                //changed decision and transaction date to the modelDateTime rather than
                                //the date of the latest bar. Because it was failing on instruments like cattle
                                //and hogs that don't have data
                                optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary
                                    = optionSpreadManager.initializationParmsSaved.modelDateTime.Date
                                    .AddHours(
                                        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Minute);

                                optionSpreadExpressionList[expressionCounter].todayDecisionTime
                                    = optionSpreadManager.initializationParmsSaved.modelDateTime.Date
                                    .AddHours(
                                        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Hour)
                                    .AddMinutes(
                                        optionSpreadExpressionList[expressionCounter].instrument.customDayBoundaryTime.Minute
                                        - optionSpreadExpressionList[expressionCounter].instrument.decisionOffsetMinutes);


                                //TSErrorCatch.debugWriteOut(
                                //    optionSpreadExpressionList[expressionCounter].cqgSymbol + " " +
                                //    optionSpreadExpressionList[expressionCounter].todayDecisionTime)

                                //optionSpreadExpressionList[expressionCounter].settlementDateTimeMarker
                                //    = optionSpreadManager.initializationParmsSaved.modelDateTime.Date
                                //    .AddHours(
                                //        optionSpreadExpressionList[expressionCounter].instrument.settlementTime.Hour)
                                //    .AddMinutes(
                                //        optionSpreadExpressionList[expressionCounter].instrument.settlementTime.Minute
                                //        + 15);

                                //TSErrorCatch.debugWriteOut(
                                //    optionSpreadExpressionList[expressionCounter].instrument.CQGsymbol + "  " +
                                //    optionSpreadExpressionList[expressionCounter].settlementDateTimeMarker);


                                int timeBarsIn_Counter = 0;
                                int cumVolume = 0;

                                while (timeBarsIn_Counter < cqg_TimedBarsIn.Count)
                                {
                                    
                                    if (cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                        .CompareTo(optionSpreadExpressionList[expressionCounter].previousDateTimeBoundaryStart) >= 0)
                                    //&& cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                    //.CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) <= 0)
                                    {
                                        bool error = false;

                                        OHLCData ohlcData = new OHLCData();

                                        optionSpreadExpressionList[expressionCounter].futureBarData.Add(ohlcData);

                                        ohlcData.barTime = cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp;

                                        int volume = 0;

                                        if (cqg_TimedBarsIn[timeBarsIn_Counter].ActualVolume
                                            != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                        {
                                            volume = cqg_TimedBarsIn[timeBarsIn_Counter].ActualVolume;
                                        }
                                        else
                                        {
                                            error = true;
                                        }

                                        //if (timeBarsIn_Counter == 0)
                                        //{
                                        //    TSErrorCatch.debugWriteOut(
                                        //        optionSpreadExpressionList[expressionCounter].cqgSymbol +
                                        //        " VOL BAR " + cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp);
                                        //}
                                        //else if (timeBarsIn_Counter == cqg_TimedBarsIn.Count - 1)
                                        //{
                                        //    TSErrorCatch.debugWriteOut(
                                        //        optionSpreadExpressionList[expressionCounter].cqgSymbol +
                                        //        " VOL BAR " + cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp);
                                        //}

                                        cumVolume += volume;

                                        ohlcData.cumulativeVolume = cumVolume;

                                        ohlcData.volume = volume;

                                        

                                        if (cqg_TimedBarsIn[timeBarsIn_Counter].Open
                                            != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                        {
                                            ohlcData.open = cqg_TimedBarsIn[timeBarsIn_Counter].Open;
                                        }
                                        else
                                        {
                                            error = true;
                                        }

                                        if (cqg_TimedBarsIn[timeBarsIn_Counter].High
                                            != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                        {
                                            ohlcData.high = cqg_TimedBarsIn[timeBarsIn_Counter].High;
                                        }
                                        else
                                        {
                                            error = true;
                                        }

                                        if (cqg_TimedBarsIn[timeBarsIn_Counter].Low
                                            != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                        {
                                            ohlcData.low = cqg_TimedBarsIn[timeBarsIn_Counter].Low;
                                        }
                                        else
                                        {
                                            error = true;
                                        }


                                        if (cqg_TimedBarsIn[timeBarsIn_Counter].Close
                                        != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                        {
                                            ohlcData.close = cqg_TimedBarsIn[timeBarsIn_Counter].Close;
                                        }
                                        else
                                        {
                                            error = true;
                                        }

                                        ohlcData.errorBar = error;

                                        //OHLCData futureBarData = optionSpreadExpressionList[expressionCounter].futureBarData[0];

                                        //futureBarData.barTime = ohlcData.barTime;

                                        //futureBarData.open = ohlcData.open;

                                        //futureBarData.high = ohlcData.high;

                                        //futureBarData.low = ohlcData.low;

                                        //futureBarData.close = ohlcData.close;

                                        //futureBarData.volume = ohlcData.volume;

                                        //futureBarData.cumulativeVolume = ohlcData.cumulativeVolume;

                                        //futureBarData.errorBar = ohlcData.errorBar;

                                        //TSErrorCatch.debugWriteOut(
                                        //    optionSpreadExpressionList[expressionCounter].instrument.name
                                        //    + ","
                                        //    + ohlcData.barTime
                                        //    + ","
                                        //    + ohlcData.volume
                                        //    + ","
                                        //    + ohlcData.cumulativeVolume
                                        //    + ","
                                        //    + ohlcData.close
                                        //    );


                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) <= 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].todayTransactionBar = ohlcData;
                                        }

                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) >= 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary = true;
                                        }

                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) <= 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].decisionBar = ohlcData;
                                        }

                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) >= 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].reachedDecisionBar = true;


                                        }

                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) > 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar = true;
                                        }

                                        if (!error
                                            && !optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot
                                            && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                            .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime.AddMinutes(1)) > 0)
                                        {
                                            optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot = true;
                                        }

                                        //if (!error
                                        //    && !optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument
                                        //    && cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                        //    .CompareTo(optionSpreadExpressionList[expressionCounter].settlementDateTimeMarker) > 0)
                                        //{
                                        //    optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument = true;
                                        //}

                                        //TSErrorCatch.debugWriteOut(ohlcData.barTime.ToString()
                                        //    + " C " + ohlcData.close
                                        //    + "  " + cqg_TimedBarsIn[timeBarsIn_Counter].Timestamp
                                        //    + " C " + cqg_TimedBarsIn[timeBarsIn_Counter].Close);



                                    }

                                    timeBarsIn_Counter++;
                                }

                                int backTimeCounter = cqg_TimedBarsIn.Count - 1;

                                while (backTimeCounter >= 0)
                                {
                                    if (cqg_TimedBarsIn[backTimeCounter].Close
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                    {
                                        optionSpreadExpressionList[expressionCounter].lastTimeFuturePriceUpdated =
                                            cqg_TimedBarsIn[backTimeCounter].Timestamp;

                                        optionSpreadExpressionList[expressionCounter].trade =
                                            cqg_TimedBarsIn[backTimeCounter].Close;

                                        optionSpreadExpressionList[expressionCounter].tradeFilled = true;

                                        break;
                                    }

                                    backTimeCounter--;
                                }



                                fillDefaultMidPrice(optionSpreadExpressionList[expressionCounter]);

                                manageExpressionPriceCalcs(optionSpreadExpressionList[expressionCounter]);

                            }

                            //break;  //while (expressionCounter < optionSpreadExpressionList.Count)

                        }

                        //expressionCounter++;

                    }


                }
                else
                {
                    if (optionSpreadManager.optionRealtimeMonitor != null)
                    {
                        optionSpreadManager.optionRealtimeMonitor.updateCQGDataStatus("CQG ERROR", Color.Yellow, Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }


        private void m_CEL_TimedBarsAdded(CQGLibrary.CQGTimedBars cqg_TimedBarsIn)
        {
            try
            {
                //TSErrorCatch.debugWriteOut(cqg_TimedBarsIn.Id);

                {
                    int expressionCounter = 0;

                    if (optionSpreadExpressionFutureTimedBarsListIdx.Count > 0
                        &&
                        optionSpreadExpressionFutureTimedBarsListIdx.ContainsKey(cqg_TimedBarsIn.Id))
                    {

                        expressionCounter = optionSpreadExpressionFutureTimedBarsListIdx[cqg_TimedBarsIn.Id];

                        //while (expressionCounter < optionSpreadExpressionList.Count)
                        //{
                        if (!optionSpreadExpressionList[expressionCounter].stopUpdating
                            && optionSpreadExpressionList[expressionCounter].futureTimedBars != null)

                        //while (expressionCounter < optionSpreadExpressionList.Count)
                        //{
                        //    if (!optionSpreadExpressionList[expressionCounter].stopUpdating
                        //        && optionSpreadExpressionList[expressionCounter].futureTimedBars != null
                        //        && cqg_TimedBarsIn.Id.CompareTo(optionSpreadExpressionList[expressionCounter].futureTimedBars.Id) == 0)
                        //&& !optionSpreadExpressionList[expressionCounter].requestedMinuteBars)
                        {
                            int lastTimdBarsInIdx = cqg_TimedBarsIn.Count - 1;

                            while (lastTimdBarsInIdx >= 0)
                            {
                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].Timestamp.CompareTo(
                                    optionSpreadExpressionList[expressionCounter].futureBarData.Last().barTime) <= 0)
                                {
                                    lastTimdBarsInIdx++;
                                    break;
                                }

                                lastTimdBarsInIdx--;
                            }

                            if (lastTimdBarsInIdx < 0)
                            {
                                lastTimdBarsInIdx = 0;
                            }

                            while (lastTimdBarsInIdx < cqg_TimedBarsIn.Count)
                            {

                                bool error = false;


                                OHLCData ohlcData = new OHLCData();

                                optionSpreadExpressionList[expressionCounter].futureBarData.Add(ohlcData);

                                ohlcData.barTime = cqg_TimedBarsIn[lastTimdBarsInIdx].Timestamp;

                                int volume = 0;

                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].ActualVolume
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    volume = cqg_TimedBarsIn[lastTimdBarsInIdx].ActualVolume;
                                }
                                else
                                {
                                    error = true;
                                }



                                ohlcData.volume = volume;


                                int cumVolume = 0;

                                int cumCounter = 0;

                                foreach (OHLCData ohlcDataCumvol in optionSpreadExpressionList[expressionCounter].futureBarData)
                                {
                                    if (ohlcDataCumvol.barTime.CompareTo(
                                        optionSpreadExpressionList[expressionCounter].previousDateTimeBoundaryStart) >= 0)
                                    {
                                        //if (cumCounter == 0)
                                        //{
                                        //    TSErrorCatch.debugWriteOut(
                                        //        optionSpreadExpressionList[expressionCounter].cqgSymbol +
                                        //        " VOL BAR " + ohlcDataCumvol.barTime);
                                        //}
                                        //else if (cumCounter == 
                                        //    optionSpreadExpressionList[expressionCounter].futureBarData.Count - 1)
                                        //{
                                        //    TSErrorCatch.debugWriteOut(
                                        //        optionSpreadExpressionList[expressionCounter].cqgSymbol +
                                        //        " VOL BAR " + ohlcDataCumvol.barTime);
                                        //}

                                        cumCounter++;

                                        cumVolume += ohlcDataCumvol.volume;

                                        ohlcDataCumvol.cumulativeVolume = cumVolume;
                                    }
                                }


                                //int timeBarsIn_Counter = 0;

                                //while (timeBarsIn_Counter < cqg_TimedBarsIn.Count)
                                //{
                                    

                                //    if (cqg_TimedBarsIn[timeBarsIn_Counter].ActualVolume
                                //                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                //    {
                                //        //volume = cqg_TimedBarsIn[timeBarsIn_Counter].ActualVolume;

                                //        cumVolume += cqg_TimedBarsIn[timeBarsIn_Counter].ActualVolume;
                                //    }

                                //    timeBarsIn_Counter++;

                                //}

                                ohlcData.cumulativeVolume = cumVolume;


                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].Open
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    ohlcData.open = cqg_TimedBarsIn[lastTimdBarsInIdx].Open;
                                }
                                else
                                {
                                    error = true;
                                }

                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].High
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    ohlcData.high = cqg_TimedBarsIn[lastTimdBarsInIdx].High;
                                }
                                else
                                {
                                    error = true;
                                }

                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].Low
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    ohlcData.low = cqg_TimedBarsIn[lastTimdBarsInIdx].Low;
                                }
                                else
                                {
                                    error = true;
                                }

                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].Close
                                    != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    ohlcData.close = cqg_TimedBarsIn[lastTimdBarsInIdx].Close;
                                }
                                else
                                {
                                    error = true;
                                }

                                ohlcData.errorBar = error;


                                //OHLCData futureBarData = optionSpreadExpressionList[expressionCounter].futureBarData[0];

                                //futureBarData.barTime = ohlcData.barTime;

                                //futureBarData.open = ohlcData.open;

                                //futureBarData.high = ohlcData.high;

                                //futureBarData.low = ohlcData.low;

                                //futureBarData.close = ohlcData.close;

                                //futureBarData.volume = ohlcData.volume;

                                //futureBarData.cumulativeVolume = ohlcData.cumulativeVolume;

                                //futureBarData.errorBar = ohlcData.errorBar;


                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) <= 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].todayTransactionBar = ohlcData;
                                }

                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) >= 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary = true;
                                }

                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) <= 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].decisionBar = ohlcData;
                                }

                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) >= 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].reachedDecisionBar = true;
                                }

                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) > 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar = true;
                                }

                                if (!error
                                    && !optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot
                                    && ohlcData.barTime
                                    .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime.AddMinutes(1)) > 0)
                                {
                                    optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot = true;
                                }

                                //if (!error
                                //    && !optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument
                                //    && ohlcData.barTime
                                //    .CompareTo(optionSpreadExpressionList[expressionCounter].settlementDateTimeMarker) > 0)
                                //{
                                //    //optionSpreadExpressionList[expressionCounter].reachedSettlementDateTimeMarker = true;

                                //    optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument = true;
                                //}


                                if (cqg_TimedBarsIn[lastTimdBarsInIdx].Close
                                        != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                                {
                                    optionSpreadExpressionList[expressionCounter].lastTimeFuturePriceUpdated =
                                                cqg_TimedBarsIn[lastTimdBarsInIdx].Timestamp;

                                    optionSpreadExpressionList[expressionCounter].trade =
                                            cqg_TimedBarsIn[lastTimdBarsInIdx].Close;

                                    optionSpreadExpressionList[expressionCounter].tradeFilled = true;

                                    fillDefaultMidPrice(optionSpreadExpressionList[expressionCounter]);

                                    manageExpressionPriceCalcs(optionSpreadExpressionList[expressionCounter]);
                                }

                                lastTimdBarsInIdx++;

                            }



                        }

                        //expressionCounter++;
                    }





                    //for (int test = 0; test < optionSpreadExpressionList[expressionCounter]
                    //                            .futureBarData.Length; test++)
                    //{
                    //    TSErrorCatch.debugWriteOut(
                    //        optionSpreadExpressionList[expressionCounter]
                    //                                .futureBarData[test].barTime + "  " +

                    //        optionSpreadExpressionList[expressionCounter]
                    //                                .futureBarData[test].open + "  " +

                    //        optionSpreadExpressionList[expressionCounter]
                    //                                .futureBarData[test].cumulativeVolume);
                    //}

                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void m_CEL_TimedBarsUpdated(CQGLibrary.CQGTimedBars cqg_TimedBarsIn, int index)
        {
            //Debug.WriteLine("m_CEL_ExpressionResolved" + cqg_expression.Count);
            try
            {

                int expressionCounter = 0;

                //int insideMethodIdx = index;

                if (optionSpreadExpressionFutureTimedBarsListIdx.Count > 0
                    &&
                    optionSpreadExpressionFutureTimedBarsListIdx.ContainsKey(cqg_TimedBarsIn.Id))
                {

                    expressionCounter = optionSpreadExpressionFutureTimedBarsListIdx[cqg_TimedBarsIn.Id];


                    if (optionSpreadExpressionList.Count > expressionCounter
                        && !optionSpreadExpressionList[expressionCounter].stopUpdating
                        && optionSpreadExpressionList[expressionCounter].futureTimedBars != null
                        && optionSpreadExpressionList[expressionCounter].futureBarData != null
                        && optionSpreadExpressionList[expressionCounter].futureBarData.Count > 0)
                    {
                        int futureBarDataCounter = optionSpreadExpressionList[expressionCounter].futureBarData.Count - 1;

                        bool foundBar = false;

                        while (futureBarDataCounter >= 0)
                        {
                            if (cqg_TimedBarsIn[index].Timestamp.CompareTo(
                                optionSpreadExpressionList[expressionCounter].futureBarData[futureBarDataCounter].barTime) == 0)
                            {
                                foundBar = true;
                                break;
                            }

                            futureBarDataCounter--;
                        }

                        if(foundBar)
                        {

                            //***********************************


                            OHLCData ohlcData = optionSpreadExpressionList[expressionCounter].futureBarData[futureBarDataCounter];

                            bool error = false;

                            int volume = 0;

                            if (cqg_TimedBarsIn[index].ActualVolume
                                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                volume = cqg_TimedBarsIn[index].ActualVolume;
                            }
                            else
                            {
                                error = true;
                            }

                            ohlcData.volume = volume;

                            if (cqg_TimedBarsIn[index].Open
                                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                ohlcData.open = cqg_TimedBarsIn[index].Open;
                            }
                            else
                            {
                                error = true;
                            }

                            //**********************************************


                            if (cqg_TimedBarsIn[index].High
                                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                ohlcData.high = cqg_TimedBarsIn[index].High;
                            }
                            else
                            {
                                error = true;
                            }

                            if (cqg_TimedBarsIn[index].Low
                                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                ohlcData.low = cqg_TimedBarsIn[index].Low;
                            }
                            else
                            {
                                error = true;
                            }

                            if (cqg_TimedBarsIn[index].Close
                                != -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                ohlcData.close = cqg_TimedBarsIn[index].Close;
                            }
                            else
                            {
                                error = true;
                            }

                            ohlcData.errorBar = error;


                            //**********************************************




                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) <= 0)
                            {
                                optionSpreadExpressionList[expressionCounter].todayTransactionBar = ohlcData;
                            }

                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayTransactionTimeBoundary) >= 0)
                            {
                                optionSpreadExpressionList[expressionCounter].reachedTransactionTimeBoundary = true;
                            }

                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) <= 0)
                            {
                                optionSpreadExpressionList[expressionCounter].decisionBar = ohlcData;
                            }

                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reachedDecisionBar
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) >= 0)
                            {
                                optionSpreadExpressionList[expressionCounter].reachedDecisionBar = true;
                            }

                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime) > 0)
                            {
                                optionSpreadExpressionList[expressionCounter].reachedBarAfterDecisionBar = true;
                            }

                            if (!error
                                && !optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot
                                && ohlcData.barTime
                                .CompareTo(optionSpreadExpressionList[expressionCounter].todayDecisionTime.AddMinutes(1)) > 0)
                            {
                                optionSpreadExpressionList[expressionCounter].reached1MinAfterDecisionBarUsedForSnapshot = true;
                            }

                            //if (!error
                            //    && !optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument
                            //    && ohlcData.barTime
                            //    .CompareTo(optionSpreadExpressionList[expressionCounter].settlementDateTimeMarker) > 0)
                            //{
                            //    optionSpreadExpressionList[expressionCounter].instrument.eodAnalysisAtInstrument = true;
                            //}

                            //***********************************


                            if (!optionSpreadExpressionList[expressionCounter].futureBarData.Last().errorBar)
                                //!= -TradingSystemConstants.CQG_DATA_ERROR_CODE)
                            {
                                optionSpreadExpressionList[expressionCounter].lastTimeFuturePriceUpdated =
                                    optionSpreadExpressionList[expressionCounter].futureBarData.Last().barTime;

                                optionSpreadExpressionList[expressionCounter].trade =
                                    optionSpreadExpressionList[expressionCounter].futureBarData.Last().close;

                                fillDefaultMidPrice(optionSpreadExpressionList[expressionCounter]);

                                manageExpressionPriceCalcs(optionSpreadExpressionList[expressionCounter]);

                            }

                        }

                        //break;  //while (expressionCounter < optionSpreadExpressionList.Count)

                    }

                    //expressionCounter++;
                }






                //TSErrorCatch.debugWriteOut(
                //    optionSpreadExpressionList[expressionCounter]
                //                            .futureBarData[test].barTime + "  " +

                //    optionSpreadExpressionList[expressionCounter]
                //                            .futureBarData[test].open + "  " +

                //    optionSpreadExpressionList[expressionCounter]
                //                            .futureBarData[test].cumulativeVolume);




            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }



        public void sendSubscribeRequest(bool sendOnlyUnsubscribed)
        {

#if DEBUG
            try
#endif
            {
                //if (this.InvokeRequired)
                {
                    //ThreadSafeSendSubscribeRequestRunDelegate d = new ThreadSafeSendSubscribeRequestRunDelegate(sendSubscribeRequestRun);

                    //optionSpreadExpressionFutureTimedBarsListIdx.Clear();

                    subscriptionThread = new Thread(new ParameterizedThreadStart(sendSubscribeRequestRun));
                    subscriptionThread.IsBackground = true;
                    subscriptionThread.Start(sendOnlyUnsubscribed);

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(sendSubscribeRequestRun));

                    //this.Invoke(d, sendOnlyUnsubscribed);
                }
                //                 else
                //                 {
                //                     sendSubscribeRequestRun(sendOnlyUnsubscribed);
                //                 }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

        }

        public void sendSubscribeRequestRun(Object obj)
        {
            optionSpreadManager.openThread(null, null);

            try
            {
                //m_CEL.RemoveAllTimedBars();
                //Thread.Sleep(3000);

                if (m_CEL_fake.IsStarted)
                {
                    bool sendOnlyUnsubscribed = (bool)obj;

                    int i = 0;

                    //m_CEL.NewInstrument("C.US.EPN1312600");

                    while (!subscriptionThreadShouldStop && i < optionSpreadExpressionList.Count)
                    {

                        //TSErrorCatch.debugWriteOut("SUBSCRIBE " + optionSpreadExpressionList[i].cqgSymbol);

                        if (sendOnlyUnsubscribed)
                        {


                            if (!optionSpreadExpressionList[i].setSubscriptionLevel)
                            {
                                Thread.Sleep(SUBSCRIPTION_TIMEDELAY_CONSTANT);



                                int count = i + 1;

                                if (optionSpreadManager.optionRealtimeMonitor != null)
                                {
                                    optionSpreadManager.optionRealtimeMonitor.updateStatusSubscribeData(
                                        "SUBSCRIBE " + optionSpreadExpressionList[i].cqgSymbol
                                        + " : " + count + " OF " +
                                        optionSpreadExpressionList.Count);
                                }

                                m_CEL_fake.NewInstrument(optionSpreadExpressionList[i].cqgSymbol);

                                int idx = i;

                                optionSpreadExpressionCheckSubscribedListIdx.AddOrUpdate(
                                    optionSpreadExpressionList[i].cqgSymbol, idx,
                                    (oldKey, oldValue) => idx);

                            }

                            if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                if (!optionSpreadExpressionList[i].requestedMinuteBars
                                    && optionSpreadExpressionList[i].normalSubscriptionRequest)
                                {
                                    requestFutureContractTimeBars(optionSpreadExpressionList[i],
                                        i);
                                }
                            }

                        }
                        else
                        {
                            optionSpreadExpressionList[i].setSubscriptionLevel = false;

                            Thread.Sleep(SUBSCRIPTION_TIMEDELAY_CONSTANT);



                            int count = i + 1;

                            if (optionSpreadManager.optionRealtimeMonitor != null)
                            {
                                optionSpreadManager.optionRealtimeMonitor.updateStatusSubscribeData(
                                        "SUBSCRIBE " + optionSpreadExpressionList[i].cqgSymbol
                                        + " : " + count + " OF " +
                                        optionSpreadExpressionList.Count);
                            }

                            m_CEL_fake.NewInstrument(optionSpreadExpressionList[i].cqgSymbol);

                            int idx = i;

                            optionSpreadExpressionCheckSubscribedListIdx.AddOrUpdate(
                                optionSpreadExpressionList[i].cqgSymbol, idx,
                                (oldKey, oldValue) => idx);

                            if (optionSpreadExpressionList[i].callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            {
                                optionSpreadExpressionList[i].requestedMinuteBars = false;
                                ;

                                if (optionSpreadExpressionList[i].normalSubscriptionRequest)
                                {
                                    requestFutureContractTimeBars(optionSpreadExpressionList[i],
                                        i);
                                }

                            }
                        }



                        i++;
                    }

                    Thread.Sleep(SUBSCRIPTION_TIMEDELAY_CONSTANT);

                    if (optionSpreadManager.optionRealtimeMonitor != null)
                    {
                        optionSpreadManager.optionRealtimeMonitor.updateStatusSubscribeData("");
                    }

                    if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                        && !optionSpreadManager.realtimeMonitorSettings.alreadyWritten)
                    {
                        Thread writeStateToDbThread = new Thread(new ParameterizedThreadStart(runRealtimeWriteStateToDB));
                        writeStateToDbThread.IsBackground = true;
                        writeStateToDbThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            optionSpreadManager.closeThread(null, null);
        }

        private void runRealtimeWriteStateToDB(Object obj)
        {
            optionSpreadManager.openThread(null, null);

            Thread.Sleep(10000);

            DateTime writeDate;// = optionSpreadManager.writeRealtimeStateToDatabase();


            if (optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                writeDate = optionSpreadManager.writeRealtimeStateToDatabaseAzure();
            }
            else
            {
                writeDate = optionSpreadManager.writeRealtimeStateToDatabase();
            }


            optionSpreadManager.realtimeMonitorSettings.alreadyWritten = true;

            //optionSpreadManager.realtimeMonitorSettings.
            if (optionSpreadManager.optionRealtimeMonitor.settings != null)
            {
                optionSpreadManager.optionRealtimeMonitor.settings
                    .updateWriteDate(writeDate);
            }

            optionSpreadManager.closeThread(null, null);
        }

        public void requestFutureContractTimeBars(OptionSpreadExpression optionSpreadExpression,
            int optionSpreadExpressionIdx)
        {
            try
            {
                //m_CEL.RemoveAllInstruments();

                //Thread.Sleep(5000);
                
                CQGLibrary.CQGTimedBarsRequest timedBarsRequest = m_CEL_fake.CreateTimedBarsRequest();
                //timedBarsRequest.Symbol = underlyingFutureContractProps.underlyingInstrumentName;

                timedBarsRequest.Symbol = optionSpreadExpression.cqgSymbol;

                //timedBarsRequest.Symbol = "F.US.USAH1";
                timedBarsRequest.SessionsFilter = 31;
                //timedBarsRequest.SessionFlags = CQG.eSessionFlag.sfDailyFromIntraday;

                //if (underlyingFutureMinOrDaily == TradingSystemConstants.DATA_COLLECTION_MINUTE_BARS)
                //{
                timedBarsRequest.IntradayPeriod = 1;
                //}
                //else
                //{
                //    timedBarsRequest.HistoricalPeriod = CQG.eHistoricalPeriod.hpDaily;
                //}

                //if (runningForContinuousContract)
                //{
                //    timedBarsRequest.Continuation = eTimeSeriesContinuationType.tsctActive;
                //    timedBarsRequest.EqualizeCloses = false;
                //}
                //else
                //{
                timedBarsRequest.Continuation = CQGLibrary.CQG.eTimeSeriesContinuationType.tsctNoContinuation;
                //}

                //timedBarsRequest.Continuation = CQG.eTimeSeriesContinuationType.tsctNoContinuation;
                //timedBarsRequest.EqualizeCloses = false;
                //DateTime rangeStart = m_CEL.Environment.LineTime.AddDays(-1);

                DateTime rangeStart = optionSpreadExpression.previousDateTimeBoundaryStart;

                DateTime rangeEnd = m_CEL_fake.Environment.LineTime;

                timedBarsRequest.RangeStart = rangeStart;
                timedBarsRequest.RangeEnd = rangeEnd;

                timedBarsRequest.IncludeEnd = true;

                timedBarsRequest.UpdatesEnabled = true;

                //timedBarsRequest.

                optionSpreadExpression.futureTimedBars = m_CEL_fake.RequestTimedBars(timedBarsRequest);


                optionSpreadExpressionFutureTimedBarsListIdx.AddOrUpdate(optionSpreadExpression.futureTimedBars.Id,
                    optionSpreadExpressionIdx, (oldKey, oldValue) => optionSpreadExpressionIdx);
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void manageExpressionPriceCalcs(OptionSpreadExpression optionSpreadExpression)
        {
            //             if (!optionSpreadExpression.riskFreeRateFilled)
            //             {
            //                 optionSpreadExpression.riskFreeRate = 0.01;
            //             }

            //if (!optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
            //optionSpreadExpression.lastTimeUpdated = DateTime.Now;

            //             fillPricesFromQuote(optionSpreadExpression, quotes);
            // 
            //             fillDefaultPrice(optionSpreadExpression);

            if (optionSpreadExpression.optionExpressionType == OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
            {
                // fill all instruments with the latest interest rate

                for (int expressionCounter = 0;
                        expressionCounter < optionSpreadExpressionList.Count; expressionCounter++)
                {
                    optionSpreadExpressionList[expressionCounter].riskFreeRateFilled = true;

                    optionSpreadExpressionList[expressionCounter].riskFreeRate = optionSpreadExpression.riskFreeRate;

                    //only update if subscribed
                    if (optionSpreadExpressionList[expressionCounter].setSubscriptionLevel)
                    {
                        optionSpreadExpressionList[expressionCounter].totalCalcsRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
                    }
                }
            }
            else
            {
                if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {

                    optionSpreadExpression.impliedVol = 0;
                    optionSpreadExpression.delta = 1 * TradingSystemConstants.OPTION_DELTA_MULTIPLIER;

                    fillFutureDecisionAndTransactionPrice(optionSpreadExpression);

                    foreach (OptionSpreadExpression optionSpreadThatUsesFuture in optionSpreadExpression.optionExpressionsThatUseThisFutureAsUnderlying)
                    {

                        fillEodAnalysisPrices(optionSpreadThatUsesFuture);

                        fillTheoreticalOptionPrice(optionSpreadThatUsesFuture);

                        generatingGreeks(optionSpreadThatUsesFuture);

                        if (optionSpreadThatUsesFuture.setSubscriptionLevel)
                        {
                            optionSpreadThatUsesFuture.totalCalcsRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
                        }

                        //tempListOfExpressionsFilled.Add(optionSpreadExpression.optionExpressionIdxUsedInFuture[expressionCounter]);
                    }


                }
                else
                {
                    generatingGreeks(optionSpreadExpression);
                }



                optionSpreadExpression.totalCalcsRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
            }
        }

        private void fillPricesFromQuote(OptionSpreadExpression optionSpreadExpression, CQGLibrary.CQGQuotes quotes)
        {
            //double defaultPrice = 0;

            //CQGQuote quoteAsk = optionSpreadExpression.cqgInstrument.Quotes[eQuoteType.qtAsk];
            //CQGQuote quoteBid = optionSpreadExpression.cqgInstrument.Quotes[eQuoteType.qtBid];
            //CQGQuote quoteTrade = optionSpreadExpression.cqgInstrument.Quotes[eQuoteType.qtTrade];
            //CQGQuote quoteSettlement = optionSpreadExpression.cqgInstrument.Quotes[eQuoteType.qtSettlement];
            //CQGQuote quoteYestSettlement = optionSpreadExpression.cqgInstrument.Quotes[eQuoteType.qtYesterdaySettlement];

            CQGLibrary.CQGQuote quoteAsk = quotes[CQGLibrary.CQG.eQuoteType.qtAsk];
            CQGLibrary.CQGQuote quoteBid = quotes[CQGLibrary.CQG.eQuoteType.qtBid];
            CQGLibrary.CQGQuote quoteTrade = quotes[CQGLibrary.CQG.eQuoteType.qtTrade];
            CQGLibrary.CQGQuote quoteSettlement = quotes[CQGLibrary.CQG.eQuoteType.qtSettlement];
            CQGLibrary.CQGQuote quoteYestSettlement = quotes[CQGLibrary.CQG.eQuoteType.qtYesterdaySettlement];

            if (optionSpreadExpression.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                if (quoteAsk != null)
                {
                    if (quoteAsk.IsValid)
                    {
                        optionSpreadExpression.ask = quoteAsk.Price;

                        optionSpreadExpression.askFilled = true;
                    }
                    else
                    {
                        optionSpreadExpression.ask = 0;

                        optionSpreadExpression.askFilled = false;
                    }
                }

                if (quoteBid != null)
                {
                    if (quoteBid.IsValid)
                    {
                        optionSpreadExpression.bid = quoteBid.Price;

                        optionSpreadExpression.bidFilled = true;
                    }
                    else
                    {
                        optionSpreadExpression.bid = 0;

                        optionSpreadExpression.bidFilled = false;
                    }
                }

                if (quoteTrade != null)
                {
                    if (quoteTrade.IsValid)
                    {
                        optionSpreadExpression.trade = quoteTrade.Price;

                        optionSpreadExpression.tradeFilled = true;
                    }
                    else if (optionSpreadExpression.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        optionSpreadExpression.trade = 0;

                        optionSpreadExpression.tradeFilled = false;
                    }
                }
            }

            if (quoteSettlement != null)
            {
                if (quoteSettlement.IsValid)
                {
                    if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                        || (optionSpreadExpression.instrument != null
                            && optionSpreadExpression.instrument.eodAnalysisAtInstrument))
                    {
                        if (optionSpreadExpression.substituteSubscriptionRequest
                            || !optionSpreadExpression.useSubstituteSymbolAtEOD)
                        {
                            if (!optionSpreadExpression.manuallyFilled)
                            {
                                optionSpreadExpression.settlement = quoteSettlement.Price;

                                optionSpreadExpression.settlementDateTime = quoteSettlement.ServerTimestamp;
                            }

                        }
                    }
                    else
                    {
                        if (!optionSpreadExpression.manuallyFilled)
                        {
                            optionSpreadExpression.settlement = quoteSettlement.Price;

                            optionSpreadExpression.settlementDateTime = quoteSettlement.ServerTimestamp;
                        }
                    }

                    if (optionSpreadExpression.settlementDateTime.Date.CompareTo(DateTime.Now.Date) == 0)
                    {
                        optionSpreadExpression.settlementIsCurrentDay = true;
                    }


                    optionSpreadExpression.settlementFilled = true;

                    fillEODSubstitutePrices(optionSpreadExpression);

                    //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
                    //    && optionSpreadExpression.substituteSubscriptionRequest)
                    //{
                    //    foreach(OptionSpreadExpression optionSpreadExpressionSingle in 
                    //        optionSpreadExpression.mainExpressionSubstitutionUsedFor)
                    //    {
                    //        optionSpreadExpressionSingle.settlement = optionSpreadExpression.settlement;

                    //        optionSpreadExpressionSingle.settlementDateTime = optionSpreadExpression.settlementDateTime;

                    //        optionSpreadExpressionSingle.settlementIsCurrentDay = optionSpreadExpression.settlementIsCurrentDay;

                    //        optionSpreadExpressionSingle.settlementFilled = true;
                    //    }
                    //}

                    //TSErrorCatch.debugWriteOut(quoteSettlement.Timestamp + "  " + quoteSettlement.ServerTimestamp);
                }
                else
                {
                    if (!optionSpreadExpression.manuallyFilled)
                    {
                        optionSpreadExpression.settlement = 0;

                        optionSpreadExpression.settlementFilled = false;
                    }
                }


            }

            //if (quoteYestSettlement != null)
            //{
            //    if (quoteYestSettlement.IsValid)
            //    {
            //        optionSpreadExpression.yesterdaySettlement = quoteYestSettlement.Price;

            //        optionSpreadExpression.yesterdaySettlementFilled = true;
            //    }
            //    else
            //    {
            //        optionSpreadExpression.yesterdaySettlement = 0;

            //        optionSpreadExpression.yesterdaySettlementFilled = false;
            //    }


            //}





            //if (optionSpreadExpression.callPutOrFuture
            //                        != (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            //{
            //    TSErrorCatch.debugWriteOut("fillPricesFromQuote "
            //        + optionSpreadExpression.cqgSymbol
            //        + " bid " + optionSpreadExpression.bid
            //        + " ask " + optionSpreadExpression.ask
            //        + " trade " + optionSpreadExpression.trade);
            //}

        }

        public void fillEODSubstitutePrices(OptionSpreadExpression optionSpreadExpression)
        {
            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
            if (optionSpreadExpression.instrument != null
                && optionSpreadExpression.instrument.eodAnalysisAtInstrument
                && optionSpreadExpression.substituteSubscriptionRequest)
            {
                //foreach (OptionSpreadExpression optionSpreadExpressionSingle in
                //    optionSpreadExpression.mainExpressionSubstitutionUsedFor)
                {
                    //if (!optionSpreadExpression.manuallyFilled)

                    if (!optionSpreadExpression.mainExpressionSubstitutionUsedFor.manuallyFilled)
                    {
                        optionSpreadExpression.mainExpressionSubstitutionUsedFor.settlement = optionSpreadExpression.settlement;

                        optionSpreadExpression.mainExpressionSubstitutionUsedFor.settlementDateTime = optionSpreadExpression.settlementDateTime;

                        optionSpreadExpression.mainExpressionSubstitutionUsedFor.settlementIsCurrentDay = optionSpreadExpression.settlementIsCurrentDay;

                        optionSpreadExpression.mainExpressionSubstitutionUsedFor.settlementFilled = true;
                    }
                }
            }
        }

        private void fillDefaultMidPrice(OptionSpreadExpression optionSpreadExpression)
        {
            double defaultPrice = 0;

            if (optionSpreadExpression.optionExpressionType == OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
            {
                if (optionSpreadExpression.tradeFilled)
                {
                    defaultPrice = optionSpreadExpression.trade;
                }
                else if (optionSpreadExpression.settlementFilled)
                {
                    defaultPrice = optionSpreadExpression.settlement;
                }
                else if (optionSpreadExpression.yesterdaySettlementFilled)
                {
                    defaultPrice = optionSpreadExpression.yesterdaySettlement;
                }

                if (defaultPrice == 0)
                {
                    defaultPrice = 0.01;
                }

                defaultPrice = defaultPrice == 0 ? 0 :
                    ((int)((100 - defaultPrice + TradingSystemConstants.EPSILON) * 1000) / 100000.0);

                optionSpreadExpression.riskFreeRate = defaultPrice;
            }
            else
            {

                if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                    //{
                    //    //    //defaultPrice = optionSpreadExpression.decisionBar.close;

                    //    //    optionSpreadExpression.decisionPrice = optionSpreadExpression.decisionBar.close;

                    //    //    optionSpreadExpression.transactionPrice = optionSpreadExpression.futureBarData.Last().close;

                    //    //    optionSpreadExpression.decisionPriceTime = optionSpreadExpression.decisionBar.barTime;

                    //    //    optionSpreadExpression.transactionPriceTime = optionSpreadExpression.futureBarData.Last().barTime;

                    //    optionSpreadExpression.lastTimeUpdated = optionSpreadExpression.decisionBar.barTime;
                    //    //optionSpreadExpression.settlementDateTime;

                    //    defaultPrice = optionSpreadExpression.decisionBar.close;

                    //    //optionSpreadExpression.defaultMidPriceBeforeTheor = defaultPrice;

                    //    //can set default price for futures here b/c no further price possibilities for future;
                    //    optionSpreadExpression.defaultPrice = defaultPrice;

                    //    optionSpreadExpression.defaultPriceFilled = true;
                    //}
                    //else  //if (!optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                    {
                        optionSpreadExpression.lastTimeUpdated = optionSpreadExpression.lastTimeFuturePriceUpdated;

                        TimeSpan span = DateTime.Now - optionSpreadExpression.lastTimeUpdated;

                        optionSpreadExpression.minutesSinceLastUpdate = span.TotalMinutes;

                        if (optionSpreadExpression.tradeFilled)
                        {
                            defaultPrice = optionSpreadExpression.trade;
                        }
                        else if (optionSpreadExpression.settlementFilled)
                        {
                            defaultPrice = optionSpreadExpression.settlement;
                        }
                        else if (optionSpreadExpression.yesterdaySettlementFilled)
                        {
                            defaultPrice = optionSpreadExpression.yesterdaySettlement;
                        }

                        if (defaultPrice == 0)
                        {
                            defaultPrice = TradingSystemConstants.OPTION_ZERO_PRICE;
                        }

                        //optionSpreadExpression.defaultMidPriceBeforeTheor = defaultPrice;

                        //can set default price for futures here b/c no further price possibilities for future;
                        optionSpreadExpression.defaultPrice = defaultPrice;

                        optionSpreadExpression.defaultPriceFilled = true;

                        //optionSpreadExpression.transactionPrice = defaultPrice;

                        //optionSpreadExpression.decisionPriceTime = optionSpreadExpression.lastTimeUpdated;

                        //optionSpreadExpression.transactionPriceTime = optionSpreadExpression.lastTimeUpdated;
                    }
                }
                else //CHANGED DEC 30 2015 
                    //to else from else if
                    //if(!optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                {
                    
                    {
                        optionSpreadExpression.lastTimeUpdated = DateTime.Now;

                        optionSpreadExpression.minutesSinceLastUpdate = 0;

                        if (optionSpreadExpression.bidFilled
                        && optionSpreadExpression.askFilled)
                        {


                            defaultPrice = (optionSpreadExpression.bid + optionSpreadExpression.ask) / 2;


                            //rounding to nearest tick;
                            if (optionSpreadExpression.instrument != null)
                            {

                                double optionTickSize = OptionSpreadManager.chooseOptionTickSize(defaultPrice,
                                    optionSpreadExpression.instrument.optionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSize,
                                    optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                                defaultPrice = ((int)((defaultPrice + optionTickSize / 2) /
                                    optionTickSize)) * optionTickSize;

                            }


                            //if (Math.Abs((optionSpreadExpression.bid - optionSpreadExpression.ask)
                            //    / OptionSpreadManager.chooseOptionTickSize(optionSpreadExpression.ask,
                            //        optionSpreadExpression.instrument.optionTickSize,
                            //        optionSpreadExpression.instrument.secondaryOptionTickSize,
                            //        optionSpreadExpression.instrument.secondaryOptionTickSizeRule))
                            //    < TradingSystemConstants.OPTION_ACCEPTABLE_BID_ASK_SPREAD)
                            //{
                            //    optionSpreadExpression.midPriceAcceptedDefault = true;

                            //    //TSErrorCatch.debugWriteOut(optionSpreadExpression.cqgSymbol + " true " + defaultPrice);
                            //}
                            //else
                            //{
                            //    optionSpreadExpression.midPriceAcceptedDefault = false;
                            //    //TSErrorCatch.debugWriteOut(optionSpreadExpression.cqgSymbol + " false " + defaultPrice);
                            //}
                        }
                        else if (optionSpreadExpression.askFilled)
                        {
                            defaultPrice = optionSpreadExpression.ask;
                        }
                        else if (optionSpreadExpression.bidFilled)
                        {
                            defaultPrice = optionSpreadExpression.bid;
                        }

                        if (defaultPrice == 0)
                        {
                            defaultPrice = optionSpreadExpression.instrument.optionTickSize;  //OptionConstants.OPTION_ZERO_PRICE;
                        }

                        optionSpreadExpression.defaultMidPriceBeforeTheor = defaultPrice;

                        if (optionSpreadExpression.askFilled)
                        {
                            optionSpreadExpression.defaultAskPriceBeforeTheor = optionSpreadExpression.ask;
                        }
                        else
                        {
                            optionSpreadExpression.defaultAskPriceBeforeTheor = defaultPrice;
                        }

                        if (optionSpreadExpression.bidFilled)
                        {
                            optionSpreadExpression.defaultBidPriceBeforeTheor = optionSpreadExpression.bid;
                        }
                        else
                        {
                            optionSpreadExpression.defaultBidPriceBeforeTheor = defaultPrice;
                        }

                        //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                        //{

                        //}
                        //else
                        //{
                        //optionSpreadExpression.decisionPrice = defaultPrice;

                        //optionSpreadExpression.transactionPrice = defaultPrice;

                        //optionSpreadExpression.decisionPriceTime = optionSpreadExpression.lastTimeUpdated;

                        //optionSpreadExpression.transactionPriceTime = optionSpreadExpression.lastTimeUpdated;
                        //}

                    }
                }
            }
        }



        public void generatingGreeks(OptionSpreadExpression optionSpreadExpression)
        //OptionSpreadExpression futureExpression)
        //OptionSpreadExpression optionSpreadExpression)
        {


            //             double yearFrac =
            //              optionSpreadHistoricalBuild[spreadCounter].
            //                 calcYearFrac(optionBuilderSpreadStructure[
            //                                     currentDateContractListMainIdx[spreadCounter][(int)OPTION_SPREAD_ROLL_INDEXES.OPTION_SPREAD_IDX]
            //                                     ]
            //                                 .optionBuildLegStructureArray[legCounter]
            //                                 .optionBuildLegDataAtRollDateList[
            //                                     currentDateContractListMainIdx[spreadCounter][(int)OPTION_SPREAD_ROLL_INDEXES.OPTION_SPREAD_ROLL_IDX]
            //                                     ]
            //                                 .optionExpirationRollInto.optionExpiration,
            //                                                 DateTime.Now);

            if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                optionSpreadExpression.impliedVol = 0;
                optionSpreadExpression.delta = 1 * TradingSystemConstants.OPTION_DELTA_MULTIPLIER;
            }

            else
            {

                OptionSpreadExpression futureExpression = optionSpreadExpression.underlyingFutureExpression;

                if (optionSpreadExpression.defaultPriceFilled
                    && futureExpression.defaultPriceFilled)
                {
                    //                 optionSpreadExpression.settlementImpliedVol =
                    //                     OptionCalcs.calculateOptionVolatilityNR(
                    //                        optionSpreadExpression.callPutOrFutureChar,
                    //                        optionSpreadExpression.underlyingSettlementPrice,
                    //                        optionSpreadExpression.strikePrice,
                    //                        optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                    //                        optionSpreadExpression.settlement) * 100;



                    //if (!optionSpreadManager.realtimeMonitorSettings.eodAnalysis)

                    double optionTickSize = optionSpreadExpression.instrument.optionTickSize;

                    if (optionSpreadExpression.instrument.secondaryOptionTickSize > 0)
                    {
                        optionTickSize = optionSpreadExpression.instrument.secondaryOptionTickSize;
                    }

                    optionSpreadExpression.impliedVol =
                        OptionCalcs.calculateOptionVolatilityNR(
                           optionSpreadExpression.callPutOrFutureChar,
                           futureExpression.defaultPrice,
                           optionSpreadExpression.strikePrice,
                           optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                           optionSpreadExpression.defaultPrice, optionTickSize); // *100;


                    if (optionSpreadExpression.impliedVol > 1000
                        || optionSpreadExpression.impliedVol < -1000
                        )
                    {
                        optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;

                        optionSpreadExpression.impliedVol = optionSpreadExpression.impliedVolFromSpan;

                        //optionSpreadExpression.impliedVol = optionSpreadExpression.impliedVolIndexShifted;

                        

                        //                     OptionCalcs.calculateOptionVolatilityNR(
                        //                        optionSpreadExpression.callPutOrFutureChar,
                        //                        optionSpreadExpression.underlyingDefaultPrice,
                        //                        optionSpreadExpression.strikePrice,
                        //                        optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                        //                        optionSpreadExpression.optionDefaultPrice);

                        optionSpreadExpression.defaultPriceFilled = true;

                        //optionSpreadExpression.defaultPriceForDisplay = optionSpreadExpression.defaultPrice;

                        optionSpreadExpression.totalCalcsRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
                    }

                    optionSpreadExpression.impliedVolFilled = true;

                    optionSpreadExpression.delta =
                        OptionCalcs.gDelta(
                           optionSpreadExpression.callPutOrFutureChar,
                           futureExpression.defaultPrice,
                           optionSpreadExpression.strikePrice,
                           optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                           0,
                           optionSpreadExpression.impliedVol) * TradingSystemConstants.OPTION_DELTA_MULTIPLIER;

                    //optionSpreadExpression.gamma =
                    //    OptionCalcs.gGamma(
                    //    //optionSpreadExpression.callPutOrFutureChar,
                    //       futureExpression.defaultPrice,
                    //       optionSpreadExpression.strikePrice,
                    //       optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                    //       0,
                    //       optionSpreadExpression.impliedVol);

                    //optionSpreadExpression.vega =
                    //    OptionCalcs.gVega(
                    //    //optionSpreadExpression.callPutOrFutureChar,
                    //       futureExpression.defaultPrice,
                    //       optionSpreadExpression.strikePrice,
                    //       optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                    //       0,
                    //       optionSpreadExpression.impliedVol);

                    //optionSpreadExpression.theta =
                    //    OptionCalcs.gTheta(
                    //        optionSpreadExpression.callPutOrFutureChar,
                    //       futureExpression.defaultPrice,
                    //       optionSpreadExpression.strikePrice,
                    //       optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                    //       0,
                    //       optionSpreadExpression.impliedVol);
                }
            }

        }

        public void generatingSettlementGreeks(OptionSpreadExpression optionSpreadExpression)
        //OptionSpreadExpression futureExpression)
        {



            //if (futureExpression.settlementFilled
            if (optionSpreadExpression.settlementFilled)
            {

                OptionSpreadExpression futureExpression = optionSpreadExpression.underlyingFutureExpression;

                if (optionSpreadExpression.settlementFilled)
                {

                    double optionTickSize = optionSpreadExpression.instrument.optionTickSize;

                    if (optionSpreadExpression.instrument.secondaryOptionTickSize > 0)
                    {
                        optionTickSize = optionSpreadExpression.instrument.secondaryOptionTickSize;
                    }

                    optionSpreadExpression.settlementImpliedVol =
                        OptionCalcs.calculateOptionVolatilityNR(
                           optionSpreadExpression.callPutOrFutureChar,
                           futureExpression.settlement,
                           optionSpreadExpression.strikePrice,
                           optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                           optionSpreadExpression.settlement, optionTickSize);// *100;
                }

                //if (optionBuildCommonMethods.usingEODSettlements)
                //{
                //optionSpreadThatUsesFuture.impliedVol = optionSpreadExpression.settlementImpliedVol;
                //}


            }

        }

        public void fillFutureDecisionAndTransactionPrice(OptionSpreadExpression optionSpreadExpression)
        {
            if (optionSpreadExpression.decisionBar != null && optionSpreadExpression.todayTransactionBar != null)
            {
                //if (!optionSpreadExpression.filledAfterReachedDecisionBar)
                {
                    optionSpreadExpression.decisionPrice =
                        optionSpreadExpression.decisionBar.close;

                    optionSpreadExpression.decisionPriceTime =
                        optionSpreadExpression.decisionBar.barTime;

                    optionSpreadExpression.decisionPriceFilled = true;

                    //if (optionSpreadExpression.reachedDecisionBar)
                    //{
                    //    optionSpreadExpression.filledAfterReachedDecisionBar = true;
                    //}
                }

                //if (!optionSpreadExpression.filledAfterTransactionTimeBoundary)
                {


                    optionSpreadExpression.transactionPrice =
                        optionSpreadExpression.todayTransactionBar.close;

                    optionSpreadExpression.transactionPriceTime =
                        optionSpreadExpression.todayTransactionBar.barTime;


                    if (optionSpreadExpression.reachedTransactionTimeBoundary)
                    {
                        //optionSpreadExpression.filledAfterTransactionTimeBoundary = true;

                        optionSpreadExpression.transactionPriceFilled = true;

                        foreach (OptionSpreadExpression ose in optionSpreadExpression.optionExpressionsThatUseThisFutureAsUnderlying)
                        {
                            ose.transactionPriceFilled = true;
                        }

                    }


                }



                //CHANGED DEC 30 2015 
                //if (optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                //{
                //    optionSpreadExpression.defaultPrice =
                //                optionSpreadExpression.transactionPrice;

                //    optionSpreadExpression.minutesSinceLastUpdate = 0;

                //    optionSpreadExpression.lastTimeUpdated =
                //        optionSpreadExpression.transactionPriceTime;

                //    optionSpreadExpression.defaultPriceFilled = true;
                //}

            }

        }

        public void fillEodAnalysisPrices(OptionSpreadExpression optionSpreadExpression)
        {
            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)

            generatingSettlementGreeks(optionSpreadExpression);

            if (optionSpreadExpression.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                //optionSpreadExpression.strikeLevel =
                //        optionSpreadManager.strikeLevelCalc(
                //                optionSpreadExpression.underlyingTransactionTimePrice,
                //                optionSpreadExpression.strikePrice,
                //                optionSpreadExpression.instrument);
                OptionSpreadExpression futureExpression = optionSpreadExpression.underlyingFutureExpression;

                //optionSpreadExpression.theoreticalOptionPrice =

                //BELOW IS CHANGE FOR PREVIOUS SETTLEMENT
                //OCT 23 2014

                optionSpreadExpression.decisionPrice =
                    OptionCalcs.blackScholes(
                        optionSpreadExpression.callPutOrFutureChar,
                           futureExpression.decisionPrice,
                           optionSpreadExpression.strikePrice,
                           optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                           optionSpreadExpression.impliedVolFromSpan);

                double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                            optionSpreadExpression.decisionPrice,
                            optionSpreadExpression.instrument.optionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                optionSpreadExpression.decisionPrice =
                    ((int)((optionSpreadExpression.decisionPrice + optionTickSize/2) /
                        optionTickSize)) * optionTickSize;

                //StringBuilder testingOut = new StringBuilder();
                //testingOut.Append("DECISION,");
                //testingOut.Append(optionSpreadExpression.instrument.name);
                //testingOut.Append(",CALL OR PUT, ");
                //testingOut.Append(optionSpreadExpression.callPutOrFutureChar);
                //testingOut.Append(",FUT PRICE, ");
                //testingOut.Append(futureExpression.decisionPrice);
                //testingOut.Append(",STRIKE PRICE, ");
                //testingOut.Append(optionSpreadExpression.strikePrice);
                //testingOut.Append(",YEAR FRACTION, ");
                //testingOut.Append(optionSpreadExpression.yearFraction);
                //testingOut.Append(",RFR, ");
                //testingOut.Append(optionSpreadExpression.riskFreeRate);
                //testingOut.Append(",SETTLE, ");
                //testingOut.Append(optionSpreadExpression.settlementImpliedVol);
                //testingOut.Append(",OPTION PRICE, ");
                //testingOut.Append(optionSpreadExpression.decisionPrice);

                //TSErrorCatch.debugWriteOut(testingOut.ToString());

                optionSpreadExpression.decisionPriceTime = futureExpression.decisionPriceTime;

                optionSpreadExpression.decisionPriceFilled = true;

                optionSpreadExpression.reachedDecisionBar = futureExpression.reachedDecisionBar;

                optionSpreadExpression.reachedBarAfterDecisionBar = futureExpression.reachedBarAfterDecisionBar;

                optionSpreadExpression.reached1MinAfterDecisionBarUsedForSnapshot = futureExpression.reached1MinAfterDecisionBarUsedForSnapshot;

                //optionSpreadExpression.instrument.eodAnalysisAtInstrument = futureExpression.instrument.eodAnalysisAtInstrument;

                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                if (futureExpression.instrument.eodAnalysisAtInstrument)
                {
                    optionSpreadExpression.transactionPrice =
                        OptionCalcs.blackScholes(
                            optionSpreadExpression.callPutOrFutureChar,
                               futureExpression.transactionPrice,
                               optionSpreadExpression.strikePrice,
                               optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                               optionSpreadExpression.settlementImpliedVol);
                }
                else
                {
                    //if (optionSpreadExpression.instrument.idInstrument == 53
                    //    && optionSpreadExpression.strikePrice == 0.00920
                    //    && optionSpreadExpression.callPutOrFuture.CompareTo("PUT") == 0)
                    //{
                    //    TSErrorCatch.debugWriteOut("test");
                    //}
                    if (!optionSpreadExpression.filledAfterTransactionTimeBoundary)
                    {
                        if (optionSpreadExpression.impliedVolFilled && optionSpreadExpression.transactionPriceFilled)
                        {
                            optionSpreadExpression.transactionPrice =
                                OptionCalcs.blackScholes(
                                    optionSpreadExpression.callPutOrFutureChar,
                                       futureExpression.transactionPrice,
                                       optionSpreadExpression.strikePrice,
                                       optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                                       optionSpreadExpression.impliedVol);

                            optionSpreadExpression.filledAfterTransactionTimeBoundary = true;

                        }
                    }
                }

                optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                            optionSpreadExpression.transactionPrice,
                            optionSpreadExpression.instrument.optionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                optionSpreadExpression.transactionPrice =
                    ((int)((optionSpreadExpression.transactionPrice + optionTickSize / 2) /
                        optionTickSize)) * optionTickSize;

                //testingOut.Clear();
                //testingOut.Append("TRANS,");
                //testingOut.Append(optionSpreadExpression.instrument.name);
                //testingOut.Append(",CALL OR PUT, ");
                //testingOut.Append(optionSpreadExpression.callPutOrFutureChar);
                //testingOut.Append(",FUT PRICE, ");
                //testingOut.Append(futureExpression.transactionPrice);
                //testingOut.Append(",STRIKE PRICE, ");
                //testingOut.Append(optionSpreadExpression.strikePrice);
                //testingOut.Append(",YEAR FRACTION, ");
                //testingOut.Append(optionSpreadExpression.yearFraction);
                //testingOut.Append(",RFR, ");
                //testingOut.Append(optionSpreadExpression.riskFreeRate);
                //testingOut.Append(",SETTLE, ");
                //testingOut.Append(optionSpreadExpression.settlementImpliedVol);
                //testingOut.Append(",OPTION PRICE, ");
                //testingOut.Append(optionSpreadExpression.transactionPrice);

                //TSErrorCatch.debugWriteOut(testingOut.ToString());

                optionSpreadExpression.transactionPriceTime = futureExpression.transactionPriceTime;

                optionSpreadExpression.decisionPriceFilled = true;

                optionSpreadExpression.reachedTransactionTimeBoundary = futureExpression.reachedTransactionTimeBoundary;

                //optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;

                //CHANGED DEC 30 2015 
                //if (optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                //{
                //    optionSpreadExpression.defaultPrice =
                //        optionSpreadExpression.transactionPrice;

                //    optionSpreadExpression.minutesSinceLastUpdate = 0;

                //    optionSpreadExpression.lastTimeUpdated =
                //        optionSpreadExpression.transactionPriceTime;

                //    optionSpreadExpression.defaultPriceFilled = true;
                //}
            }
        }

        public void fillTheoreticalOptionPrice(OptionSpreadExpression optionSpreadExpression)
        {



            if (optionSpreadExpression.callPutOrFuture != OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            {
                OptionSpreadExpression futureExpression = optionSpreadExpression.underlyingFutureExpression;

                if (optionSpreadExpression.theoreticalOptionDataList == null)
                {
                    optionSpreadExpression.theoreticalOptionDataList = new List<TheoreticalBar>();
                }

                if (optionSpreadExpression.riskFreeRateFilled
                    && futureExpression.futureBarData != null
                    && (futureExpression.futureBarData.Count - 1) > optionSpreadExpression.theoreticalOptionDataList.Count)
                {
                    for (int i = optionSpreadExpression.theoreticalOptionDataList.Count; i < futureExpression.futureBarData.Count - 1;
                        i++)
                    {
                        TheoreticalBar bar = new TheoreticalBar();

                        bar.barTime = futureExpression.futureBarData[i].barTime;

                        bar.price =
                            OptionCalcs.blackScholes(
                            optionSpreadExpression.callPutOrFutureChar,
                               futureExpression.futureBarData[i].close,
                               optionSpreadExpression.strikePrice,
                               optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                               optionSpreadExpression.impliedVolFromSpan);  // / 100);

                        double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                            bar.price,
                            optionSpreadExpression.instrument.optionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                        bar.price =
                            ((int)((bar.price + optionTickSize / 2)/
                                optionTickSize)) * optionTickSize;

                        optionSpreadExpression.theoreticalOptionDataList.Add(bar);

                        //if (futureExpression.futureBarData[i].close != 0)

                        //TSErrorCatch.debugWriteOut(futureExpression.futureBarData[i].close + "," + bar.price);
                    }
                }

                //if (optionSpreadExpression.cqgSymbol.CompareTo("C.US.TYAM1412600") == 0)
                //{
                //    TSErrorCatch.debugWriteOut("TEST");
                //}

                if (futureExpression.defaultPriceFilled)
                    //CHANGED DEC 30 2015 
                    //&& !optionSpreadExpression.instrument.eodAnalysisAtInstrument)
                {


                    {
                        //optionSpreadExpression.strikeLevel =
                        //        optionSpreadManager.strikeLevelCalc(
                        //                futureExpression.defaultPrice,
                        //                optionSpreadExpression.strikePrice,
                        //                optionSpreadExpression.instrument);

                        optionSpreadExpression.theoreticalOptionPrice =
                            OptionCalcs.blackScholes(
                            optionSpreadExpression.callPutOrFutureChar,
                               futureExpression.defaultPrice,
                               optionSpreadExpression.strikePrice,
                               optionSpreadExpression.yearFraction, optionSpreadExpression.riskFreeRate,
                               optionSpreadExpression.impliedVolFromSpan);  // / 100);

                        double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                            optionSpreadExpression.theoreticalOptionPrice,
                            optionSpreadExpression.instrument.optionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSize,
                            optionSpreadExpression.instrument.secondaryOptionTickSizeRule);

                        optionSpreadExpression.theoreticalOptionPrice =
                            ((int)((optionSpreadExpression.theoreticalOptionPrice + optionTickSize / 2) /
                                optionTickSize)) * optionTickSize;

                        if (optionSpreadExpression.theoreticalOptionPrice == 0)
                        {
                            optionSpreadExpression.theoreticalOptionPrice = optionSpreadExpression.instrument.optionTickSize; // OptionConstants.OPTION_ZERO_PRICE;
                        }

                        if (optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType
                                == REALTIME_PRICE_FILL_TYPE.PRICE_MID_BID_ASK)
                        {
                            optionSpreadExpression.defaultPrice = optionSpreadExpression.defaultMidPriceBeforeTheor;
                        }
                        else if (optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType
                                == REALTIME_PRICE_FILL_TYPE.PRICE_ASK)
                        {
                            optionSpreadExpression.defaultPrice = optionSpreadExpression.defaultAskPriceBeforeTheor;
                        }
                        else if (optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType
                                == REALTIME_PRICE_FILL_TYPE.PRICE_BID)
                        {
                            optionSpreadExpression.defaultPrice = optionSpreadExpression.defaultBidPriceBeforeTheor;
                        }
                        else if (optionSpreadManager.realtimeMonitorSettings.realtimePriceFillType
                                == REALTIME_PRICE_FILL_TYPE.PRICE_DEFAULT)
                        {
                            bool midPriceAcceptable = false;

                            if (optionSpreadExpression.bidFilled && optionSpreadExpression.askFilled
                                &&
                                Math.Abs((optionSpreadExpression.bid - optionSpreadExpression.ask)
                            / OptionSpreadManager.chooseOptionTickSize(optionSpreadExpression.ask,
                                optionSpreadExpression.instrument.optionTickSize,
                                optionSpreadExpression.instrument.secondaryOptionTickSize,
                                optionSpreadExpression.instrument.secondaryOptionTickSizeRule))
                            < TradingSystemConstants.OPTION_ACCEPTABLE_BID_ASK_SPREAD)
                            {
                                midPriceAcceptable = true;
                            }

                            if (midPriceAcceptable
                                ||
                                Math.Abs((optionSpreadExpression.defaultMidPriceBeforeTheor
                                - optionSpreadExpression.theoreticalOptionPrice)
                                / optionSpreadExpression.theoreticalOptionPrice) <= TradingSystemConstants.OPTION_DEFAULT_THEORETICAL_PRICE_RANGE)
                            {
                                //                         TSErrorCatch.debugWriteOut(optionSpreadExpression.cqgSymbol + "  NOT theoretical Price "
                                //                             + optionSpreadExpression.optionDefaultPriceWithoutTheoretical);

                                //optionSpreadExpression.underlyingDefaultPrice = defaultPrice;
                                //if (optionSpreadExpression.callPutOrFuture != (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)

                                bool filledDefaultPrice = false;

                                if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.CALL)
                                {
                                    if (futureExpression.defaultPrice >
                                        optionSpreadExpression.strikePrice + (TradingSystemConstants.STRIKE_COUNT_FOR_DEFAULT_TO_THEORETICAL
                                            * optionSpreadExpression.instrument.optionStrikeIncrement))
                                    {
                                        optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;
                                        filledDefaultPrice = true;
                                    }
                                }
                                else if (optionSpreadExpression.callPutOrFuture == OPTION_SPREAD_CONTRACT_TYPE.PUT)
                                {
                                    if (futureExpression.defaultPrice <
                                        optionSpreadExpression.strikePrice - (TradingSystemConstants.STRIKE_COUNT_FOR_DEFAULT_TO_THEORETICAL
                                            * optionSpreadExpression.instrument.optionStrikeIncrement))
                                    {
                                        optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;
                                        filledDefaultPrice = true;
                                    }
                                }

                                if (!filledDefaultPrice)
                                {
                                    optionSpreadExpression.defaultPrice = optionSpreadExpression.defaultMidPriceBeforeTheor;
                                }
                            }
                            else
                            {
                                /*TSErrorCatch.debugWriteOut(optionSpreadExpression.cqgSymbol + "  IS theoretical Price");*/

                                optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;
                            }
                        }
                        else
                        {
                            optionSpreadExpression.defaultPrice = optionSpreadExpression.theoreticalOptionPrice;
                        }
                    }


                    optionSpreadExpression.defaultPriceFilled = true;

                    //optionSpreadExpression.defaultPriceForDisplay = optionSpreadExpression.defaultPrice;
                }
            }
        }

        public void setupCalculateModelValuesAndSummarizeTotals()
        {

#if DEBUG
            try
#endif
            {
                calculateModelValuesAndSummarizeTotalsThread = new Thread(new ParameterizedThreadStart(calculateModelValuesAndSummarizeTotalsThreadRun));
                calculateModelValuesAndSummarizeTotalsThread.IsBackground = true;
                calculateModelValuesAndSummarizeTotalsThread.Start();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

        }

        private void calculateModelValuesAndSummarizeTotalsThreadRun(Object obj)
        {
            optionSpreadManager.openThread(null, null);

            try
            {
                while (!calculateModelValuesThreadShouldStop)
                {
                    calculateModelValuesAndSummarizeTotals();

                    Thread.Sleep(TradingSystemConstants.MODEL_CALC_TIME_REFRESH);

                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            optionSpreadManager.closeThread(null, null);
        }

        private void calculateModelValuesAndSummarizeTotals()
        {
            int allDataComingInCounter = 0;

            HashSet<int> spreadsUpdated = new HashSet<int>();

            HashSet<int> admStrategyUpdated = new HashSet<int>();

            for (int i = 0; i < optionSpreadExpressionList.Count; i++)
            {
                if (optionSpreadExpressionList[i].setSubscriptionLevel
                    && !resetOrderPageOnceAllDataIn)
                {

                    if (allDataComingInCounter == optionSpreadExpressionList.Count - 1)
                    {
                        resetOrderPageOnceAllDataIn = true;

                        //initialFillGridOrdersPage();

                        //initialFillGridOrdersAll();

                        //updateSetupOfGridOrdersPageNewVersionApril2013();

                        for (int j = 0; j < optionSpreadExpressionList.Count; j++)
                        {
                            optionSpreadExpressionList[j].totalCalcsRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
                            //optionSpreadExpressionList[j].updatedDataFromCQG[(int)UPDATE_DATA_FROM_CQG.TOTAL_CALCS] = CQG_REFRESH_STATE.DATA_UPDATED;

                        }
                    }

                    allDataComingInCounter++;

                    //checkUpdateStatus(optionSpreadExpressionList[i]);
                }

                if (optionSpreadExpressionList[i].totalCalcsRefresh == CQG_REFRESH_STATE.DATA_UPDATED
                    && optionSpreadExpressionList[i].setSubscriptionLevel
                    && optionSpreadExpressionList[i].cqgInstrument != null)
                {
                    optionSpreadExpressionList[i].totalCalcsRefresh = CQG_REFRESH_STATE.NOTHING;



                    int spreadIdxCounter = 0;

                    while (spreadIdxCounter < optionSpreadExpressionList[i].spreadIdx.Count)
                    {
                        if (!spreadsUpdated.Contains(optionSpreadExpressionList[i].spreadIdx[spreadIdxCounter]))
                        {
                            spreadsUpdated.Add(optionSpreadExpressionList[i].spreadIdx[spreadIdxCounter]);
                        }

                        spreadIdxCounter++;
                    }

                    int admStrategyCounter = 0;

                    //

                    while (admStrategyCounter < optionSpreadExpressionList[i].admPositionImportWebIdx.Count)
                    {
                        if (!admStrategyUpdated.Contains(optionSpreadExpressionList[i].admPositionImportWebIdx[admStrategyCounter]))
                        {
                            admStrategyUpdated.Add(optionSpreadExpressionList[i].admPositionImportWebIdx[admStrategyCounter]);
                        }

                        admStrategyCounter++;
                    }


                    optionSpreadExpressionList[i].guiRefresh = CQG_REFRESH_STATE.DATA_UPDATED;
                }
            }

            optionSpreadManager.runSpreadTotalCalculations(spreadsUpdated);

            optionSpreadManager.runRollStrikeSelection(spreadsUpdated);

            optionSpreadManager.runADMSpreadTotalCalculations(admStrategyUpdated);

            optionSpreadManager.runModelContractTotalPLCalcs();

            optionSpreadManager.runModelAndADMInstrumentTotals();



        }

    }
}
