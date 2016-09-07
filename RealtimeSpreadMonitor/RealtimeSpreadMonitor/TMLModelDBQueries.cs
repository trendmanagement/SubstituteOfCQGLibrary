using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;
//using CQG;
using System.Data;
//using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace RealtimeSpreadMonitor
{

    public class TMLModelDBQueries : TMLModelDBConnection
    {

        public TMLModelDBQueries()
        {

        }

        //~TMLModelDBQueries()
        //{
        //}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Queries a portfolio groups. </summary>
        ///
        /// <remarks>   Steve Pickering, 6/18/2013. </remarks>
        ///
        /// <param name="optionSpreadGroup">    Group the option spread belongs to. </param>
        ///
        /// <returns>   The portfolio groups. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public PortfolioGroupsStruct[] queryPortfolioGroups(int optionSpreadGroup)
        {
            PortfolioGroupsStruct[] pgs = null;

            try
            {

                //DataSet portfolioDataSet = new DataSet();
                ////DataSet periodicityDataSet = new DataSet();

                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT tblportfoliogroups.idportfoliogroup, tblportfoliogroups.portfolioname");

                //dataQuery.Append(" FROM tblportfoliogroups WHERE optionspreadgroup = ");
                //dataQuery.Append(optionSpreadGroup);
                //dataQuery.Append(" ORDER BY tblportfoliogroups.idportfoliogroup");

                ////Debug.WriteLine(dataQuery);

                ////MySqlDataAdapter cmdGetPortfolioData = new MySqlDataAdapter(dataQuery.ToString(), conn);
                ////int instRows = cmdGetPortfolioData.Fill(portfolioDataSet);

                //DataRow[] portfolioArray = portfolioDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{
                //    //instruments = new array<Instrument>(instRows);
                //    pgs = new PortfolioGroupsStruct[instRows];


                //    for (int i = 0; i < instRows; i++)
                //    {
                //        pgs[i] = new PortfolioGroupsStruct();
                //        pgs[i].idPortfolioGroup = Convert.ToInt32(portfolioArray[i].ItemArray.GetValue(0));
                //        pgs[i].portfolioName = Convert.ToString(portfolioArray[i].ItemArray.GetValue(1));

                //    }
                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return pgs;

        }


        public OptionStrategy[] queryStrategies(int idPortfoliogroup, DateTime lookupDate)
        {
            OptionStrategy[] optionStrategies = null;

            //Jan 12 2015 this is where to change idportfoliogroup etc

            try
            {
                ///*
                // * SELECT tblportfoliodailyselection.idstrategy, tblportfoliodailyselection.idinstrument,
                //    tblstrategies.strategyname, tblstrategies.strategyfolder
                //     FROM tblstrategies, tblportfoliodailyselection
                // * WHERE tblstrategies.idstrategy = tblportfoliodailyselection.idstrategy
                //     AND tblportfoliodailyselection.idportfoliogroup = 1
                // */

                //DataSet portfolioDataSet = new DataSet();

                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT tblstrategystate.idstrategy,");
                //dataQuery.Append(" tblstrategystate.instrumentIdx");
                ////dataQuery.Append(" tblstrategies.strategyname, tblstrategies.strategyfolder");
                //dataQuery.Append(" FROM tblstrategystate");
                //dataQuery.Append(" WHERE");
                //dataQuery.Append(" tblstrategystate.idPortfolioGroup = ");
                //dataQuery.Append(idPortfoliogroup);
                ////dataQuery.Append(" AND tblstrategystate.idStrategy = tblstrategies.idstrategy");
                //dataQuery.Append(" AND tblstrategystate.dateTime = '");
                //dataQuery.Append(lookupDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("'");
                //dataQuery.Append(" ORDER BY tblstrategystate.idstrategy");
    

                ///// <summary> Information describing the command get portfolio </summary>
                ////MySqlDataAdapter cmdGetPortfolioData = new MySqlDataAdapter(dataQuery.ToString(), conn);
                //int instRows = cmdGetPortfolioData.Fill(portfolioDataSet);

                //DataRow[] portfolioArray = portfolioDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{
                //    optionStrategies = new OptionStrategy[instRows];

                //    for (int i = 0; i < instRows; i++)
                //    {
                //        optionStrategies[i] = new OptionStrategy();
                //        optionStrategies[i].idPortfoliogroup = idPortfoliogroup;
                //        optionStrategies[i].idStrategy = Convert.ToInt32(portfolioArray[i].ItemArray.GetValue(0));
                //        optionStrategies[i].idInstrument = Convert.ToInt32(portfolioArray[i].ItemArray.GetValue(1));
                //        //optionStrategies[i].strategyName = Convert.ToString(portfolioArray[i].ItemArray.GetValue(2));
                //        //optionStrategies[i].strategyFolder = Convert.ToString(portfolioArray[i].ItemArray.GetValue(3));
                //    }
                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return optionStrategies;

        }

        public PortfolioGroupAllocation[] queryPortfolioGroupAllocation(int idPortfoliogroup)
        {
            PortfolioGroupAllocation[] portfolioGroupAllocationArray = new PortfolioGroupAllocation[0];

            //Jan 12 2015 this is where to change idportfoliogroup etc

            try
            {
                //DataSet portfolioDataSet = new DataSet();

                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT tblportfoliogroupallocation.multiple,");
                //dataQuery.Append(" tblportfoliogroupallocation.broker,");
                //dataQuery.Append(" tblportfoliogroupallocation.account,");
                //dataQuery.Append(" tblportfoliogroupallocation.cfgfile,");

                //dataQuery.Append(" tblportfoliogroupallocation.FCM_POFFIC_PACCT");

                //dataQuery.Append(" FROM tblportfoliogroupallocation");
                //dataQuery.Append(" WHERE");
                //dataQuery.Append(" tblportfoliogroupallocation.idportfoliogroup = ");
                //dataQuery.Append(idPortfoliogroup);
                //dataQuery.Append(" ORDER BY tblportfoliogroupallocation.idportfoliogroupallocation");


                ///// <summary> Information describing the command get portfolio </summary>
                //MySqlDataAdapter cmdGetPortfolioData = new MySqlDataAdapter(dataQuery.ToString(), conn);
                //int instRows = cmdGetPortfolioData.Fill(portfolioDataSet);

                //DataRow[] portfolioArray = portfolioDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{
                //    portfolioGroupAllocationArray = new PortfolioGroupAllocation[instRows];

                //    for (int i = 0; i < instRows; i++)
                //    {
                //        portfolioGroupAllocationArray[i] = new PortfolioGroupAllocation();
                //        portfolioGroupAllocationArray[i].idportfoliogroup = idPortfoliogroup;
                //        //portfolioGroupAllocationArray[i].idportfoliogroupAllocation = Convert.ToInt32(portfolioArray[i].ItemArray.GetValue(0));
                //        portfolioGroupAllocationArray[i].multiple = Convert.ToInt32(portfolioArray[i].ItemArray.GetValue(0));
                //        portfolioGroupAllocationArray[i].broker = Convert.ToString(portfolioArray[i].ItemArray.GetValue(1));
                //        portfolioGroupAllocationArray[i].account = Convert.ToString(portfolioArray[i].ItemArray.GetValue(2));
                //        portfolioGroupAllocationArray[i].cfgfile = Convert.ToString(portfolioArray[i].ItemArray.GetValue(3));

                //        portfolioGroupAllocationArray[i].FCM_POFFIC_PACCT = Convert.ToString(portfolioArray[i].ItemArray.GetValue(4));

                //        //TODO PARSE FCM_POFFIC_PACCT
                //        string[] officeAcct = portfolioGroupAllocationArray[i].FCM_POFFIC_PACCT.Split(';');
                        
                //        for(int officeAcctCnt = 0; officeAcctCnt < officeAcct.Length; officeAcctCnt++)
                //        {
                //            string[] x = officeAcct[officeAcctCnt].Split(':');

                //            FCM_POFFIC_PACCT xFCM_POFFIC_PACCT = new FCM_POFFIC_PACCT();
                //            xFCM_POFFIC_PACCT.FCM_POFFIC = x[0];
                //            xFCM_POFFIC_PACCT.FCM_PACCT = x[1];

                //            //portfolioGroupAllocationArray[i].FCM_POFFIC_PACCT_List.Add(xFCM_POFFIC_PACCT);

                //            StringBuilder fcmOfficeKey = new StringBuilder();
                //            fcmOfficeKey.Append(xFCM_POFFIC_PACCT.FCM_POFFIC);
                //            fcmOfficeKey.Append(xFCM_POFFIC_PACCT.FCM_PACCT);

                //            portfolioGroupAllocationArray[i].FCM_POFFIC_PACCT_hashset.TryAdd(
                //                fcmOfficeKey.ToString(),
                //                xFCM_POFFIC_PACCT);
                //        }

                    //}
                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return portfolioGroupAllocationArray;

        }

        /// <summary>
        /// Queries the instruments.
        /// </summary>
        /// <param name="idPortfoliogroup">The identifier portfoliogroup.</param>
        /// <returns>
        /// Instrument[] array of instruments
        /// </returns>
        public Instrument[] queryInstruments(int idPortfoliogroup)
        {
            Instrument[] instruments = null;

            try
            {
            //    /*
            //     * SELECT distinct tblinstruments.*
            //         FROM tblinstruments, tblportfoliodailyselection
            //         WHERE tblinstruments.idinstrument = tblportfoliodailyselection.idinstrument
            //         AND tblportfoliodailyselection.idportfoliogroup = 1
            //     */

            //    DataSet portfolioDataSet = new DataSet();

            //    StringBuilder dataQuery = new StringBuilder();

            //    //dataQuery.Append("SELECT distinct tblinstruments.*");
            //    //dataQuery.Append(" FROM tblinstruments, tblportfoliodailyselection");

            //    dataQuery.Append("SELECT tblinstruments.idinstrument,symbol,description,idinstrumentgroup,ticksize,");
            //    dataQuery.Append(" tickdisplay,tickvalue,margin,timeshifthours,cqgsymbol,");
            //    dataQuery.Append(" commissionpercontract, listedspread, exchangesymbol, stoptype, limittickoffset,");
            //    dataQuery.Append(" optionticksize, optiontickdisplay, optiontickvalue, spanoptionstart, optionstrikedisplay,");
            //    dataQuery.Append(" optionstrikeincrement,");
            //    dataQuery.Append(" secondaryoptionticksize, secondaryoptiontickvalue, secondaryoptiontickdisplay, secondaryoptionticksizerule, usedailycustomdata, customdayboundarytime,");
            //    dataQuery.Append(" admcode, admexchangecode, optionadmstrikedisplay, admfuturepricefactor, admoptionpricefactor, admoptionftpfilestrikedisplay,");
            //    dataQuery.Append(" decisionoffsetminutes,");
            //    dataQuery.Append(" spanfuturecode, spanoptioncode,");
            //    dataQuery.Append(" tblexchange.exchange, tblexchange.spanexchangesymbol, tblexchange.spanExchWebAPISymbol,");
            //    dataQuery.Append(" tblinstruments.substitutesymbol_eod, tblinstruments.instrumentsymbol_pre_eod, tblinstruments.instrumentsymboleod_eod,");
            //    dataQuery.Append(" tblinstruments.instrumentid_eod, tradingTechnologiesExchange, optionexchangesymbol, optionstrikedisplayTT,");
            //    dataQuery.Append(" exchangesymbolTT, optionexchangesymbolTT, tradingTechnologiesGateway, settlementtime");
            //    //dataQuery.Append(" tickdisplayTT, optiontickdisplayTT");

            //    dataQuery.Append(" FROM tblinstruments, tblportfoliogroups, tblportfolioinstrumentlist, tblexchange");

            //    dataQuery.Append(" WHERE tblinstruments.idinstrument = tblportfolioinstrumentlist.idinstrument");
            //    dataQuery.Append(" AND tblinstruments.idexchange = tblexchange.idexchange");
            //    dataQuery.Append(" AND tblportfoliogroups.idportfoliogroup = ");
            //    dataQuery.Append(idPortfoliogroup);
            //    dataQuery.Append(" AND tblportfoliogroups.idportfolioinstrumentlist = tblportfolioinstrumentlist.idportfolioinstrumentlist");
            //    dataQuery.Append(" ORDER BY tblinstruments.idinstrument");

            //    //TSErrorCatch.debugWriteOut(dataQuery.ToString());

            //    MySqlDataAdapter cmdGetPortfolioData = new MySqlDataAdapter(dataQuery.ToString(), conn);
            //    int instRows = cmdGetPortfolioData.Fill(portfolioDataSet);

            //    DataRow[] instrumentListFromDB = portfolioDataSet.Tables[0].Select();

            //    if (instRows > 0)
            //    {
            //        instruments = new Instrument[instRows];



            //        for (int counterDBInstrumentList = 0; counterDBInstrumentList < instRows; counterDBInstrumentList++)
            //        {
            //            instruments[counterDBInstrumentList] = fillInstrument(instrumentListFromDB, counterDBInstrumentList);

            //            //Instrument instrument = new Instrument();

            //            //instruments[counterDBInstrumentList] = instrument;

            //            //instrument.idxOfInstrumentInList = counterDBInstrumentList;

            //            //instrument.idInstrument = Convert.ToInt32(instrumentListFromDB[counterDBInstrumentList]["idinstrument"]);

            //            //instrument.name = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["symbol"]);
            //            //instrument.idInstrumentGroup = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["idinstrumentgroup"]);

            //            //instrument.tickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["ticksize"]);
            //            //instrument.tickDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["tickdisplay"]);
            //            //instrument.tickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["tickvalue"]);
            //            //instrument.margin = Convert.ToInt32(instrumentListFromDB[counterDBInstrumentList]["margin"]);
            //            //instrument.timeShiftHours = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["timeshifthours"]);
            //            //instrument.CQGsymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["cqgsymbol"]);
            //            //instrument.commissionPerContract = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["commissionpercontract"]);
            //            //instrument.isListedSpread = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["listedspread"]);
            //            //instrument.exchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchangesymbol"]);
            //            //instrument.stopType = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["stoptype"]);
            //            //instrument.limitTickOffset = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["limittickoffset"]);
            //            //instrument.optionTickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionticksize"]);
            //            //instrument.optionTickDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optiontickdisplay"]);
            //            //instrument.optionTickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optiontickvalue"]);

            //            //instrument.optionSpreadStart = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["spanoptionstart"]);

            //            //instrument.optionStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikedisplay"]);

            //            //instrument.optionStrikeIncrement = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikeincrement"]);

            //            //instrument.secondaryOptionTickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptionticksize"]);
            //            //instrument.secondaryOptionTickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptiontickvalue"]);

            //            //instrument.secondaryoptiontickdisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptiontickdisplay"]);
                        
            //            //instrument.secondaryOptionTickSizeRule = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptionticksizerule"]);

            //            //instrument.useDailyCustomData = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["usedailycustomdata"]);
            //            //instrument.customDayBoundaryTime = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["customdayboundarytime"]);

            //            //instrument.admCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["admcode"]);
            //            //instrument.admExchangeCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["admexchangecode"]);

            //            //instrument.optionADMStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionadmstrikedisplay"]);

            //            //instrument.admFuturePriceFactor = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admfuturepricefactor"]);
            //            //instrument.admOptionPriceFactor = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admoptionpricefactor"]);

            //            //instrument.admOptionFtpFileStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admoptionftpfilestrikedisplay"]);

            //            //instrument.decisionOffsetMinutes = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["decisionoffsetminutes"]);

            //            //instrument.spanFutureCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanfuturecode"]);
            //            //instrument.spanOptionCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanoptioncode"]);

            //            //instrument.exchange = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchange"]);
            //            //instrument.spanExchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanexchangesymbol"]);
            //            //instrument.spanExchWebAPISymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanExchWebAPISymbol"]);

            //            //instrument.substituteSymbolEOD = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["substitutesymbol_eod"]);
            //            //instrument.instrumentSymbolPreEOD = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["instrumentsymbol_pre_eod"]);
            //            //instrument.instrumentSymbolEOD = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["instrumentsymboleod_eod"]);

            //            //instrument.instrumentIdEOD = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["instrumentid_eod"]);

            //            //instrument.tradingTechnologiesExchange = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["tradingTechnologiesExchange"]);

            //            //instrument.optionExchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["optionexchangesymbol"]);

            //            //instrument.optionStrikeDisplayTT = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikedisplayTT"]);

            //            //instrument.exchangeSymbolTT = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchangesymbolTT"]);
            //            //instrument.optionExchangeSymbolTT = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["optionexchangesymbolTT"]);

            //            //instrument.tradingTechnologiesGateway = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["tradingTechnologiesGateway"]);

            //            //instrument.settlementTime = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["settlementtime"]);

            //        }
            //    }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return instruments;

        }

        public Dictionary<int, Instrument> fillSubstituteInstrumentHash(Instrument[] instrumentArray)
        {
            Dictionary<int, Instrument> substituteInstrumentHash = new Dictionary<int, Instrument>();

            List<int> idInstrumentListOfSubstitutes = new List<int>();

            for (int i = 0; i < instrumentArray.Length; i++)
            {
                if (instrumentArray[i].substituteSymbolEOD)
                {
                    idInstrumentListOfSubstitutes.Add(instrumentArray[i].instrumentIdEOD);
                }
            }

            if (idInstrumentListOfSubstitutes.Count() > 0)
            {
                DataSet portfolioDataSet = new DataSet();

                StringBuilder queryWhereString = new StringBuilder();



                for (int i = 0; i < idInstrumentListOfSubstitutes.Count; i++)
                {
                    if (i > 0)
                    {
                        queryWhereString.Append(" AND ");
                    }

                    queryWhereString.Append(" tblinstruments.idinstrument = ");

                    queryWhereString.Append(instrumentArray[i].instrumentIdEOD);
                }

                queryWhereString.Append(" AND ");

                StringBuilder dataQuery = new StringBuilder();

                //**********************
                dataQuery.Append("SELECT tblinstruments.idinstrument,symbol,description,idinstrumentgroup,ticksize,");
                dataQuery.Append(" tickdisplay,tickvalue,margin,timeshifthours,cqgsymbol,");
                dataQuery.Append(" commissionpercontract, listedspread, exchangesymbol, stoptype, limittickoffset,");
                dataQuery.Append(" optionticksize, optiontickdisplay, optiontickvalue, spanoptionstart, optionstrikedisplay,");
                dataQuery.Append(" optionstrikeincrement,");
                dataQuery.Append(" secondaryoptionticksize, secondaryoptiontickvalue, secondaryoptiontickdisplay, secondaryoptionticksizerule, usedailycustomdata, customdayboundarytime,");
                dataQuery.Append(" admcode, admexchangecode, optionadmstrikedisplay, admfuturepricefactor, admoptionpricefactor, admoptionftpfilestrikedisplay,");
                dataQuery.Append(" decisionoffsetminutes,");
                dataQuery.Append(" spanfuturecode, spanoptioncode,");
                dataQuery.Append(" tblexchange.exchange, tblexchange.spanexchangesymbol, tblexchange.spanExchWebAPISymbol,");
                dataQuery.Append(" tblinstruments.substitutesymbol_eod, tblinstruments.instrumentsymbol_pre_eod, tblinstruments.instrumentsymboleod_eod,");
                dataQuery.Append(" tblinstruments.instrumentid_eod, tradingTechnologiesExchange, optionexchangesymbol, optionstrikedisplayTT,");
                dataQuery.Append(" exchangesymbolTT, optionexchangesymbolTT, tradingTechnologiesGateway, settlementtime");
                

                dataQuery.Append(" FROM tblinstruments, tblexchange");

                dataQuery.Append(" WHERE ");

                dataQuery.Append(queryWhereString.ToString());

                dataQuery.Append(" tblinstruments.idexchange = tblexchange.idexchange");

                dataQuery.Append(" ORDER BY tblinstruments.idinstrument");
                //**********************

                //MySqlDataAdapter cmdGetPortfolioData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetPortfolioData.Fill(portfolioDataSet);

                DataRow[] instrumentListFromDB = portfolioDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{
                //    Instrument instrument = fillInstrument(instrumentListFromDB, 0);

                //    substituteInstrumentHash.Add(instrument.idInstrument, instrument);
                //}
            }

            return substituteInstrumentHash;
        }

        internal Instrument fillInstrument(DataRow[] instrumentListFromDB, int counterDBInstrumentList)
        {
            Instrument instrument = new Instrument();

            instrument.idxOfInstrumentInList = counterDBInstrumentList;

            instrument.idInstrument = Convert.ToInt32(instrumentListFromDB[counterDBInstrumentList]["idinstrument"]);

            instrument.name = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["symbol"]);
            instrument.idInstrumentGroup = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["idinstrumentgroup"]);

            instrument.tickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["ticksize"]);
            instrument.tickDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["tickdisplay"]);
            instrument.tickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["tickvalue"]);
            instrument.margin = Convert.ToInt32(instrumentListFromDB[counterDBInstrumentList]["margin"]);
            instrument.timeShiftHours = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["timeshifthours"]);
            instrument.CQGsymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["cqgsymbol"]);
            instrument.commissionPerContract = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["commissionpercontract"]);
            instrument.isListedSpread = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["listedspread"]);
            instrument.exchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchangesymbol"]);
            instrument.stopType = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["stoptype"]);
            instrument.limitTickOffset = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["limittickoffset"]);
            instrument.optionTickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionticksize"]);
            instrument.optionTickDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optiontickdisplay"]);
            instrument.optionTickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optiontickvalue"]);

            instrument.optionSpreadStart = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["spanoptionstart"]);

            instrument.optionStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikedisplay"]);

            instrument.optionStrikeIncrement = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikeincrement"]);

            instrument.secondaryOptionTickSize = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptionticksize"]);
            instrument.secondaryOptionTickValue = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptiontickvalue"]);

            instrument.secondaryoptiontickdisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptiontickdisplay"]);

            instrument.secondaryOptionTickSizeRule = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["secondaryoptionticksizerule"]);

            instrument.useDailyCustomData = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["usedailycustomdata"]);
            instrument.customDayBoundaryTime = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["customdayboundarytime"]);

            instrument.admCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["admcode"]);
            instrument.admExchangeCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["admexchangecode"]);

            instrument.optionADMStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionadmstrikedisplay"]);

            instrument.admFuturePriceFactor = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admfuturepricefactor"]);
            instrument.admOptionPriceFactor = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admoptionpricefactor"]);

            instrument.admOptionFtpFileStrikeDisplay = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["admoptionftpfilestrikedisplay"]);

            instrument.decisionOffsetMinutes = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["decisionoffsetminutes"]);

            instrument.spanFutureCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanfuturecode"]);
            instrument.spanOptionCode = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanoptioncode"]);

            instrument.exchange = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchange"]);
            instrument.spanExchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanexchangesymbol"]);
            instrument.spanExchWebAPISymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["spanExchWebAPISymbol"]);

            instrument.substituteSymbolEOD = Convert.ToBoolean(instrumentListFromDB[counterDBInstrumentList]["substitutesymbol_eod"]);
            instrument.instrumentSymbolPreEOD = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["instrumentsymbol_pre_eod"]);
            instrument.instrumentSymbolEOD = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["instrumentsymboleod_eod"]);

            instrument.instrumentIdEOD = Convert.ToInt16(instrumentListFromDB[counterDBInstrumentList]["instrumentid_eod"]);

            instrument.tradingTechnologiesExchange = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["tradingTechnologiesExchange"]);

            instrument.optionExchangeSymbol = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["optionexchangesymbol"]);

            instrument.optionStrikeDisplayTT = Convert.ToDouble(instrumentListFromDB[counterDBInstrumentList]["optionstrikedisplayTT"]);

            instrument.exchangeSymbolTT = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["exchangesymbolTT"]);
            instrument.optionExchangeSymbolTT = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["optionexchangesymbolTT"]);

            instrument.tradingTechnologiesGateway = Convert.ToString(instrumentListFromDB[counterDBInstrumentList]["tradingTechnologiesGateway"]);

            instrument.settlementTime = Convert.ToDateTime(instrumentListFromDB[counterDBInstrumentList]["settlementtime"]);

            return instrument;
        }

        

        public void queryDataForCalendar(Instrument[] instruments)
        {
            try
            {
                //DataSet calendarDataSet; // = new DataSet();

                //for (int instrumentCounter = 0; instrumentCounter < instruments.Length; instrumentCounter++)
                //{
                //    calendarDataSet = new DataSet();

                //    String dataQuery;

                //    if (instruments[instrumentCounter].isSpread)
                //    {
                //        dataQuery = "SELECT tbltradecalendar.idtbltradecalendar,";
                //        dataQuery += " tbltradecalendar.datetime, tbltradecalendar.iddaytype";
                //        dataQuery += " FROM tbltradecalendar";
                //        //dataQuery += " tbltradecalendar.iddaytype = tbldaytype.iddaytype";
                //        //dataQuery += " AND (tbldaytype.idinstrumentgroup = " + instruments[instrumentCounter].idInstrumentGroup;
                //        //dataQuery += " OR tbldaytype.idinstrumentgroup = 1)";
                //        dataQuery += " ORDER BY DATE(tbltradecalendar.datetime) asc";
                //    }
                //    else
                //    {
                //        dataQuery = "SELECT tbltradecalendar.idtbltradecalendar,";
                //        dataQuery += " tbltradecalendar.datetime, tbltradecalendar.iddaytype";
                //        dataQuery += " FROM tbltradecalendar, tbldaytype where";
                //        dataQuery += " tbltradecalendar.iddaytype = tbldaytype.iddaytype";
                //        dataQuery += " AND (tbldaytype.idinstrumentgroup = " + instruments[instrumentCounter].idInstrumentGroup;
                //        dataQuery += " OR tbldaytype.idinstrumentgroup = 1)";
                //        dataQuery += " ORDER BY DATE(tbltradecalendar.datetime) asc, tbldaytype.priority asc";
                //    }

                //    MySqlDataAdapter cmdGetCalendarData = new MySqlDataAdapter(dataQuery, conn);
                //    int nRows = cmdGetCalendarData.Fill(calendarDataSet);

                //    DataRow[] dtArray = calendarDataSet.Tables[0].Select();

                //    List<TradeCalendarData> tradeCalendarData = new List<TradeCalendarData>();

                //    for (int i = 0; i < nRows; i++)
                //    {
                //        TradeCalendarData tempTradeCalendarData = new TradeCalendarData();

                //        tempTradeCalendarData.tcDateTime = Convert.ToDateTime(dtArray[i].ItemArray.GetValue(1));
                //        tempTradeCalendarData.tcTypeId = (int)Convert.ToDouble(dtArray[i].ItemArray.GetValue(2));

                //        tradeCalendarData.Add(tempTradeCalendarData);
                //    }

                //    //***************************

                //    calendarDataSet = new DataSet();

                //    if (instruments[instrumentCounter].isSpread)
                //    {
                //        dataQuery = "SELECT DISTINCT tbldaytype.iddaytype, tbldaytype.daydescription,";
                //        dataQuery += "tbldaytype.idinstrument, tbldaytype.priority,";
                //        dataQuery += "tbldaytype.timedescription, tbldaytype.extdescription, tbldaytype.sensitivity";
                //        //dataQuery += " WHERE (tbldaytype.idinstrumentgroup = " + instruments[instrumentCounter].idInstrumentGroup;
                //        //dataQuery += " OR tbldaytype.idinstrumentgroup = 1)";
                //        dataQuery += " FROM tbldaytype";
                //        dataQuery += " ORDER BY tbldaytype.iddaytype";
                //    }
                //    else
                //    {
                //        dataQuery = "SELECT tbldaytype.iddaytype, tbldaytype.daydescription,";
                //        dataQuery += "tbldaytype.idinstrument, tbldaytype.priority,";
                //        dataQuery += "tbldaytype.timedescription, tbldaytype.extdescription, tbldaytype.sensitivity";
                //        dataQuery += " FROM cqgdb.tbldaytype";
                //        dataQuery += " WHERE (tbldaytype.idinstrumentgroup = " + instruments[instrumentCounter].idInstrumentGroup;
                //        dataQuery += " OR tbldaytype.idinstrumentgroup = 1)";
                //        dataQuery += " ORDER BY iddaytype";
                //    }

                //    //Debug.WriteLine(dataQuery);

                //    cmdGetCalendarData = new MySqlDataAdapter(dataQuery, conn);
                //    int nRowsDayType = cmdGetCalendarData.Fill(calendarDataSet);

                //    //Debug.WriteLine(nRows);

                //    TradeCalendarDescription[] tradeCalendarDescription =
                //        new TradeCalendarDescription[nRowsDayType];

                //    dtArray = calendarDataSet.Tables[0].Select();

                //    for (int i = 0; i < nRowsDayType; i++)
                //    {
                //        tradeCalendarDescription[i] = new TradeCalendarDescription();
                //        tradeCalendarDescription[i].tcDayTypeDescription = Convert.ToString(dtArray[i].ItemArray.GetValue(1));
                //        tradeCalendarDescription[i].tcTimeDescription = Convert.ToString(dtArray[i].ItemArray.GetValue(4));
                //        tradeCalendarDescription[i].tcDayTypeIdDescription = (int)Convert.ToDouble(dtArray[i].ItemArray.GetValue(0));
                //    }

                //    instruments[instrumentCounter].tradeCalendarData = tradeCalendarData;
                //    instruments[instrumentCounter].tradeCalendarDescription = tradeCalendarDescription;
                //}

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryStrategyState(OptionStrategy optionStrategy, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                ///*
                // * SELECT distinct tblinstruments.*
                //     FROM tblinstruments, tblportfoliodailyselection
                //     WHERE tblinstruments.idinstrument = tblportfoliodailyselection.idinstrument
                //     AND tblportfoliodailyselection.idportfoliogroup = 1
                // */

                //optionStrategy.optionStrategyParameters = new OptionStrategyParameter[
                //        optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0)];

                //for (int i = 0; i < optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0); i++)
                //{
                //    optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();
                //}

                //StringBuilder dataQuery = new StringBuilder();

                

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tblstrategystate.");
                //    dataQuery.Append(optionArrayTypes.tblStrategyStateFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tblstrategystate");

                //dataQuery.Append(" WHERE tblstrategystate.idStrategy = ");
                //dataQuery.Append(optionStrategy.idStrategy);

                //dataQuery.Append(" AND tblstrategystate.idPortfoliogroup = ");
                //dataQuery.Append(optionStrategy.idPortfoliogroup);
                
                //dataQuery.Append(" AND tblstrategystate.dateTime = '");
                //dataQuery.Append(optionStrategy.dateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("'");

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{



                //    for (int i = 0; i < optionArrayTypes.tblStrategyStateFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_STRATEGY_STATE_FIELDS.numberOfLegs:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.numberOfLegs;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.spreadStructure:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.spreadStructure;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.activeContractIndexes;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rollIntoContractIndexes;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_ROLL_PARAMETER_SCRIPT;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            //case (int)TBL_STRATEGY_STATE_FIELDS.startingPoint:
                //            //    optionStrategy.optionStrategyParameters[i].strategyLegParameters = TBL_STRATEGY_STATE_FIELDS.startingPoint;
                //            //    optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //            //    optionStrategy.optionStrategyParameters[i].stateValue =
                //            //        Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //            //    break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.cfgContracts:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.cfgContracts;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.currentPosition:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.currentPosition;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.entryRule:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.entryRule;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_ENTRY_SCRIPT;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.entryLots:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.entryLots;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.exitRule:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.exitRule;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_EXIT_SCRIPT;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.exitLots:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.exitLots;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValueStringNotParsed =
                //                    strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.marginRequirement:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.marginRequirement;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.rRisk:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rRisk;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.rRisk_R:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rRisk_R;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.oneR:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.oneR;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.rStatus:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rStatus;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.rStatus_R:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.rStatus_R;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.idRiskType:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.idRiskType;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_STRATEGY_STATE_FIELDS.idSignalType:
                //                optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.idSignalType;
                //                optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //                optionStrategy.optionStrategyParameters[i].stateValue =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            //case (int)TBL_STRATEGY_STATE_FIELDS.lockedIn_R:
                //            //    optionStrategy.optionStrategyParameters[i].strategyStateFieldType = TBL_STRATEGY_STATE_FIELDS.lockedIn_R;
                //            //    optionStrategy.optionStrategyParameters[i].parseParameter = TBL_DB_PARSE_PARAMETER.DONOT_PARSE_PARAMETER;
                //            //    optionStrategy.optionStrategyParameters[i].stateValue =
                //            //        optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue
                //            //        +
                //            //        optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.oneR].stateValue
                //            //        -
                //            //        optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;
                //            //    break;
                //        }
                //    }

                //    optionStrategy.lockedIn_R =
                //        optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rStatus].stateValue
                //        -
                //        optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.rRisk].stateValue;



                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionInfo(OptionStrategy optionStrategy, int legCounter, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptions.");
                //    dataQuery.Append(optionArrayTypes.tblOptionFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptions");

                //dataQuery.Append(" WHERE tbloptions.idoption = ");
                //dataQuery.Append(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                //    .stateValueParsed[legCounter]);
                

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_OPTIONS_FIELDS.idoption:
                //                optionStrategy.legInfo[legCounter].idOption = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionname:
                //                optionStrategy.legInfo[legCounter].optionName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonth:
                //                optionStrategy.legInfo[legCounter].optionMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonthint:
                //                optionStrategy.legInfo[legCounter].optionMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionyear:
                //                optionStrategy.legInfo[legCounter].optionYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.strikeprice:
                //                optionStrategy.legInfo[legCounter].optionStrikePrice =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.callorput:
                //                optionStrategy.legInfo[legCounter].optionCallOrPut = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.expirationdate:

                //                optionStrategy.legInfo[legCounter].expirationDate = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.idcontract:
                //                optionStrategy.legInfo[legCounter].idUnderlyingContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.cqgsymbol:
                //                optionStrategy.legInfo[legCounter].cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionInfoForADMWebData(ADMPositionImportWeb admPositionImportWeb,
            OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptions.");
                //    dataQuery.Append(optionArrayTypes.tblOptionFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptions");

                //dataQuery.Append(" WHERE tbloptions.optionmonthint = ");
                //dataQuery.Append(admPositionImportWeb.contractInfo.optionMonthInt);
                //dataQuery.Append(" AND tbloptions.optionyear = ");
                //dataQuery.Append(admPositionImportWeb.contractInfo.optionYear);
                //dataQuery.Append(" AND tbloptions.idinstrument = ");
                //dataQuery.Append(admPositionImportWeb.instrument.idInstrument);
                //dataQuery.Append(" AND tbloptions.callorput = '");
                //dataQuery.Append(admPositionImportWeb.PSUBTY);
                //dataQuery.Append("'");
                //dataQuery.Append(" AND tbloptions.strikeprice = ");
                //dataQuery.Append(admPositionImportWeb.strikeInDecimal);




                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //if (instRows == 0)
                //{
                //    String caption = "CAN'T FIND ADM OPTION IN OUR DATABASE";
                //    String message = "CAN'T FIND ADM OPTION IN OUR DATABASE:  " +
                //        dataQuery;
                //    MessageBoxButtons buttons = MessageBoxButtons.OK;
                //    System.Windows.Forms.DialogResult result;

                //    Displays the MessageBox.
                //   result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                //}

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //    {

                //        optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_OPTIONS_FIELDS.idoption:
                //                admPositionImportWeb.contractInfo.idOption = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionname:
                //                admPositionImportWeb.contractInfo.optionName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonth:
                //                admPositionImportWeb.contractInfo.optionMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonthint:
                //                admPositionImportWeb.contractInfo.optionMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionyear:
                //                admPositionImportWeb.contractInfo.optionYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.strikeprice:
                //                admPositionImportWeb.contractInfo.optionStrikePrice =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.callorput:
                //                admPositionImportWeb.contractInfo.optionCallOrPut = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.expirationdate:

                //                admPositionImportWeb.contractInfo.expirationDate = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.idcontract:
                //                admPositionImportWeb.contractInfo.idUnderlyingContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.cqgsymbol:
                //                admPositionImportWeb.contractInfo.cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionInfoForRollingOptionPreSymbol(OptionStrategy optionStrategy, int legCounter, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptions.");
                //    dataQuery.Append(optionArrayTypes.tblOptionFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptions");

                //dataQuery.Append(" WHERE tbloptions.optionmonthint = ");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionMonthInt);
                //dataQuery.Append(" AND tbloptions.optionyear = ");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionYear);
                //dataQuery.Append(" AND tbloptions.idinstrument = ");
                //dataQuery.Append(optionStrategy.instrument.idInstrument);
                //dataQuery.Append(" AND tbloptions.callorput = '");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionCallOrPut);
                //dataQuery.Append("'");
                ////dataQuery.Append(" ORDER BY strikeprice DESC LIMIT 1");

                //dataQuery.Append(" LIMIT 1");

                
                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //if (instRows == 0)
                //{
                //    String caption = "CAN'T FIND ADM OPTION IN OUR DATABSE";
                //    String message = "CAN'T FIND ADM OPTION IN OUR DATABSE:  " +
                //        dataQuery;
                //    MessageBoxButtons buttons = MessageBoxButtons.OK;
                //    System.Windows.Forms.DialogResult result;

                //    // Displays the MessageBox.
                //    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                //}

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_OPTIONS_FIELDS.idoption:
                //                optionStrategy.rollIntoLegInfo[legCounter].idOption = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionname:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonth:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonthint:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionyear:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.strikeprice:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionStrikePrice =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.callorput:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionCallOrPut = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.expirationdate:
                //                optionStrategy.rollIntoLegInfo[legCounter].expirationDate = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.idcontract:
                //                optionStrategy.rollIntoLegInfo[legCounter].idUnderlyingContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.cqgsymbol:
                //                optionStrategy.rollIntoLegInfo[legCounter].cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionInfoForRollingOption(OptionStrategy optionStrategy, int legCounter, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptions.");
                //    dataQuery.Append(optionArrayTypes.tblOptionFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptions");

                //dataQuery.Append(" WHERE tbloptions.optionmonthint = ");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionMonthInt);
                //dataQuery.Append(" AND tbloptions.optionyear = ");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionYear);
                //dataQuery.Append(" AND tbloptions.idinstrument = ");
                //dataQuery.Append(optionStrategy.instrument.idInstrument);
                //dataQuery.Append(" AND tbloptions.callorput = '");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionCallOrPut);
                //dataQuery.Append("'");
                //dataQuery.Append(" AND tbloptions.strikeprice = ");
                //dataQuery.Append(optionStrategy.rollIntoLegInfo[legCounter].optionStrikePriceReference[
                //            optionStrategy.rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange]);




                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_OPTIONS_FIELDS.idoption:
                //                optionStrategy.rollIntoLegInfo[legCounter].idOption = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionname:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonth:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionmonthint:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.optionyear:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.strikeprice:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionStrikePrice =
                //                    Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.callorput:
                //                optionStrategy.rollIntoLegInfo[legCounter].optionCallOrPut = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.expirationdate:
                //                optionStrategy.rollIntoLegInfo[legCounter].expirationDate = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.idcontract:
                //                optionStrategy.rollIntoLegInfo[legCounter].idUnderlyingContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTIONS_FIELDS.cqgsymbol:
                //                optionStrategy.rollIntoLegInfo[legCounter].cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionExpirations(OptionStrategy optionStrategy, LegInfo legInfo)
        {
            queryOptionExpirations(optionStrategy.instrument.idInstrument, legInfo);
        }

        public void queryOptionExpirations(ADMPositionImportWeb admPositionImportWeb)
        {
            queryOptionExpirations(admPositionImportWeb.instrument.idInstrument, admPositionImportWeb.contractInfo);
        }

        public void queryOptionExpirations(int idInstrument, LegInfo legInfo)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT expirationlasttradetime");
                //dataQuery.Append(" FROM tbloptionproperties");
                //dataQuery.Append(" WHERE");
                //dataQuery.Append(" idinstrument = " + idInstrument);
                //dataQuery.Append(" AND optionmonthint = ");
                //dataQuery.Append(legInfo.optionMonthInt);


                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{
                //    legInfo.optionExpirationTime =
                //        Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(0));
                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }



        public void queryOptionData(OptionStrategy optionStrategy, int legCounter, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionDataFields.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptiondata.");
                //    dataQuery.Append(optionArrayTypes.tblOptionDataFields.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionDataFields.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptiondata");

                //dataQuery.Append(" WHERE tbloptiondata.idoption = ");
                //dataQuery.Append(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                //    .stateValueParsed[legCounter]);
                //dataQuery.Append(" AND tbloptiondata.dateTime >= '");
                //dataQuery.Append(optionStrategy.dateTime.AddDays(-5).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("' ORDER BY datetime DESC LIMIT 1");

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionDataFields.GetLength(0); i++)
                //    {
                //        switch (i)
                //        {
                //            case (int)TBL_OPTION_DATA_FIELDS.datetime:
                //                optionStrategy.legData[legCounter].dataDateTimeFromDB = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.price:
                //                optionStrategy.legData[legCounter].settlementPriceFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.impliedvol:
                //                optionStrategy.legData[legCounter].impliedVolFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));

                //                optionStrategy.legData[legCounter].impliedVolFromDB =
                //                    checkImpliedVol(optionStrategy.legData[legCounter].impliedVolFromDB);
                                
                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.timetoexpinyears:
                //                optionStrategy.legData[legCounter].timeToExpInYearsFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryOptionData(ADMPositionImportWeb admPositionImportWeb, OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblOptionDataFields.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbloptiondata.");
                //    dataQuery.Append(optionArrayTypes.tblOptionDataFields.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblOptionDataFields.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbloptiondata");

                //dataQuery.Append(" WHERE tbloptiondata.idoption = ");
                //dataQuery.Append(admPositionImportWeb.contractInfo.idOption);
                //dataQuery.Append(" AND tbloptiondata.dateTime >= '");
                //dataQuery.Append(admPositionImportWeb.dateTime.AddDays(-5).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("' ORDER BY datetime DESC LIMIT 1");

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblOptionDataFields.GetLength(0); i++)
                //    {
                //        switch (i)
                //        {
                //            case (int)TBL_OPTION_DATA_FIELDS.datetime:
                //                admPositionImportWeb.contractData.dataDateTimeFromDB = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.price:
                //                admPositionImportWeb.contractData.settlementPriceFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.impliedvol:
                //                admPositionImportWeb.contractData.impliedVolFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));

                //                admPositionImportWeb.contractData.impliedVolFromDB =
                //                    checkImpliedVol(admPositionImportWeb.contractData.impliedVolFromDB);

                //                break;

                //            case (int)TBL_OPTION_DATA_FIELDS.timetoexpinyears:
                //                admPositionImportWeb.contractData.timeToExpInYearsFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        private double checkImpliedVol(double impliedVolIn)
        {
            return impliedVolIn > 0.9 ? 0.9 : impliedVolIn;
        }

        public void queryFutureInfo(ADMPositionImportWeb aDMPositionImportWeb,
            OptionArrayTypes optionArrayTypes)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tblcontracts.");
                //    dataQuery.Append(optionArrayTypes.tblFutureContractFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tblcontracts");

                //dataQuery.Append(" WHERE tblcontracts.idinstrument = ");
                //dataQuery.Append(aDMPositionImportWeb.instrument.idInstrument);
                //dataQuery.Append(" AND tblcontracts.monthint = ");
                //dataQuery.Append(aDMPositionImportWeb.contractInfo.contractMonthInt);
                //dataQuery.Append(" AND tblcontracts.year = ");
                //dataQuery.Append(aDMPositionImportWeb.contractInfo.contractYear);

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.idcontract:
                //                aDMPositionImportWeb.contractInfo.idContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.contractname:
                //                aDMPositionImportWeb.contractInfo.contractName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.month:
                //                aDMPositionImportWeb.contractInfo.contractMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.monthint:
                //                aDMPositionImportWeb.contractInfo.contractMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.year:
                //                aDMPositionImportWeb.contractInfo.contractYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.expirationdate:
                //                aDMPositionImportWeb.contractInfo.expirationDate =
                //                    Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.cqgsymbol:
                //                aDMPositionImportWeb.contractInfo.cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public void queryUnderlyingFutureInfo(int idContract, OptionArrayTypes optionArrayTypes,
            LegInfo legInfo)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tblcontracts.");
                //    dataQuery.Append(optionArrayTypes.tblFutureContractFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tblcontracts");

                //dataQuery.Append(" WHERE tblcontracts.idcontract = ");
                //dataQuery.Append(idContract);
                ////dataQuery.Append(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                ////    .stateValueParsed[legCounter]);

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.idcontract:
                //                legInfo.idContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.contractname:
                //                legInfo.contractName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.month:
                //                legInfo.contractMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.monthint:
                //                legInfo.contractMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.year:
                //                legInfo.contractYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            //case (int)TBL_FUTURE_CONTRACT_FIELDS.expirationdate:
                //            //    legInfo.expirationDate =
                //            //        Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //            //    break;

                //            //case (int)TBL_FUTURE_CONTRACT_FIELDS.cqgsymbol:
                //            //    legInfo.cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //            //    break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public LegInfo[] queryInstrumentDefaultFutureInfo(
            DateTime queryDate,
            int idInstrument, OptionArrayTypes optionArrayTypes)
        {
            LegInfo[] legInfoArray = null;

            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tblcontracts.");
                //    dataQuery.Append(optionArrayTypes.tblFutureContractFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tblcontracts");

                //dataQuery.Append(" WHERE tblcontracts.idinstrument = ");
                //dataQuery.Append(idInstrument);
                //dataQuery.Append(" AND tblcontracts.expirationdate >= '");
                //dataQuery.Append(queryDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("' ORDER BY tblcontracts.expirationdate LIMIT 2");

                ////dataQuery.Append(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                ////    .stateValueParsed[legCounter]);

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //legInfoArray = new LegInfo[instRows];

                //for(int rowCount = 0; rowCount < instRows; rowCount++)
                //{
                //    legInfoArray[rowCount] = new LegInfo();

                //    for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.idcontract:
                //                legInfoArray[rowCount].idContract = Convert.ToInt32(strategyDataRows[rowCount].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.contractname:
                //                legInfoArray[rowCount].contractName = strategyDataRows[rowCount].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.month:
                //                legInfoArray[rowCount].contractMonth = Convert.ToChar(strategyDataRows[rowCount].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.monthint:
                //                legInfoArray[rowCount].contractMonthInt =
                //                    Convert.ToInt16(strategyDataRows[rowCount].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.year:
                //                legInfoArray[rowCount].contractYear =
                //                    Convert.ToInt16(strategyDataRows[rowCount].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.expirationdate:
                //                legInfoArray[rowCount].expirationDate =
                //                    Convert.ToDateTime(strategyDataRows[rowCount].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.cqgsymbol:
                //                legInfoArray[rowCount].cqgSymbol = strategyDataRows[rowCount].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return legInfoArray;

        }

        internal int queryFutureContractId(int idInstrument,
            int contractYear, int contractMonthInt)
        {
            int contractId = 0;

            //StringBuilder dataQuery = new StringBuilder();
            //dataQuery.Append("SELECT idcontract FROM");
            //dataQuery.Append(" tblcontracts WHERE");
            //dataQuery.Append(" idinstrument =");
            //dataQuery.Append(idInstrument);
            //dataQuery.Append(" AND year =");
            //dataQuery.Append(contractYear);
            //dataQuery.Append(" AND monthint =");
            //dataQuery.Append(contractMonthInt);


            //MySqlCommand cmdTestForUpdate = new MySqlCommand(dataQuery.ToString(), conn);

            //MySqlDataReader testreader = cmdTestForUpdate.ExecuteReader();

            //if (testreader.Read())
            //{
            //    contractId = testreader.GetInt32("idcontract");
            //}

            //testreader.Close();


            return contractId;
        }

        public void queryFutureInfo(int idContract, OptionArrayTypes optionArrayTypes,
            LegInfo legInfo)
        {


            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tblcontracts.");
                //    dataQuery.Append(optionArrayTypes.tblFutureContractFieldTypesArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tblcontracts");

                //dataQuery.Append(" WHERE tblcontracts.idcontract = ");
                //dataQuery.Append(idContract);
                ////dataQuery.Append(optionStrategy.optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.activeContractIndexes]
                ////    .stateValueParsed[legCounter]);

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblFutureContractFieldTypesArray.GetLength(0); i++)
                //    {

                //        //optionStrategy.optionStrategyParameters[i] = new OptionStrategyParameter();

                //        switch (i)
                //        {
                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.idcontract:
                //                legInfo.idContract = Convert.ToInt32(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.contractname:
                //                legInfo.contractName = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.month:
                //                legInfo.contractMonth = Convert.ToChar(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.monthint:
                //                legInfo.contractMonthInt =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.year:
                //                legInfo.contractYear =
                //                    Convert.ToInt16(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.expirationdate:
                //                legInfo.expirationDate =
                //                    Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_CONTRACT_FIELDS.cqgsymbol:
                //                legInfo.cqgSymbol = strategyDataRows[0].ItemArray.GetValue(i).ToString();
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void queryFuturesData(DateTime dateTime, int idContract, OptionArrayTypes optionArrayTypes,
            LegData legData)
        {
            try
            {
                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("SELECT");

                //for (int i = 0; i < optionArrayTypes.tblFutureSettlementDataFieldsArray.GetLength(0); i++)
                //{
                //    dataQuery.Append(" tbldailycontractsettlements.");
                //    dataQuery.Append(optionArrayTypes.tblFutureSettlementDataFieldsArray.GetValue(i).ToString());

                //    if (i < optionArrayTypes.tblFutureSettlementDataFieldsArray.GetLength(0) - 1)
                //    {
                //        dataQuery.Append(",");
                //    }
                //}

                //dataQuery.Append(" FROM tbldailycontractsettlements");

                //dataQuery.Append(" WHERE tbldailycontractsettlements.idcontract = ");
                //dataQuery.Append(idContract);
                //dataQuery.Append(" AND tbldailycontractsettlements.date >= '");
                //dataQuery.Append(dateTime.AddDays(-5).ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("' ORDER BY date DESC LIMIT 1");

                ////TSErrorCatch.debugWriteOut(dataQuery.ToString());

                //DataSet strategyDataSet = new DataSet();

                //MySqlDataAdapter cmdGetStrategyData = new MySqlDataAdapter(dataQuery.ToString(), conn);

                //int instRows = cmdGetStrategyData.Fill(strategyDataSet);

                //DataRow[] strategyDataRows = strategyDataSet.Tables[0].Select();

                //if (instRows > 0)
                //{

                //    for (int i = 0; i < optionArrayTypes.tblFutureSettlementDataFieldsArray.GetLength(0); i++)
                //    {
                //        switch (i)
                //        {
                //            case (int)TBL_FUTURE_SETTLEMENT_DATA_FIELDS.date:
                //                legData.dataDateTimeFromDB = Convert.ToDateTime(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;

                //            case (int)TBL_FUTURE_SETTLEMENT_DATA_FIELDS.settlement:
                //                legData.settlementPriceFromDB = Convert.ToDouble(strategyDataRows[0].ItemArray.GetValue(i));
                //                break;
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        public OptionInputFieldsFromTblOptionInputSymbols queryOptionInputSymbols(
            int idinstrument, int idoptioninputtype)
        {
            try
            {
                //OptionInputFieldsFromTblOptionInputSymbols optionInputFieldsFromTblOptionInputSymbols = null;

                //DataSet instrumentDataSet = new DataSet();

                //StringBuilder dataQuery = new StringBuilder();

                //if (idoptioninputtype == (int)OPTION_FORMULA_INPUT_TYPES.OPTION_RISK_FREE_RATE)
                //{
                //    dataQuery.Append("SELECT idoptioninputsymbol, optioninputcqgsymbol, idinstrument, idoptioninputtype, multiplier");
                //    dataQuery.Append(" FROM tbloptioninputsymbols");
                //    dataQuery.Append(" WHERE ");
                //    dataQuery.Append(" idoptioninputtype = ");
                //    dataQuery.Append(idoptioninputtype);
                //}
                //else
                //{
                //    dataQuery.Append("SELECT idoptioninputsymbol, optioninputcqgsymbol, idinstrument, idoptioninputtype, multiplier");
                //    dataQuery.Append(" FROM tbloptioninputsymbols");
                //    dataQuery.Append(" WHERE idinstrument = ");
                //    dataQuery.Append(idinstrument);
                //    dataQuery.Append(" AND idoptioninputtype = ");
                //    dataQuery.Append(idoptioninputtype);
                //}

                //MySqlDataAdapter cmdGetInstruments = new MySqlDataAdapter(dataQuery.ToString(), conn);
                //int instrumentRows = cmdGetInstruments.Fill(instrumentDataSet);

                //DataRow[] instrumentArray = instrumentDataSet.Tables[0].Select();

                //if (instrumentRows > 0)
                //{
                //    optionInputFieldsFromTblOptionInputSymbols = new OptionInputFieldsFromTblOptionInputSymbols();

                //    int i = 0;

                //    optionInputFieldsFromTblOptionInputSymbols.idOptionInputSymbol =
                //        Convert.ToInt16(instrumentArray[i].ItemArray.GetValue(0));
                //    optionInputFieldsFromTblOptionInputSymbols.optionInputCQGSymbol =
                //        Convert.ToString(instrumentArray[i].ItemArray.GetValue(1));
                //    optionInputFieldsFromTblOptionInputSymbols.idInstrument =
                //        Convert.ToInt16(instrumentArray[i].ItemArray.GetValue(2));
                //    optionInputFieldsFromTblOptionInputSymbols.idOptionInputType =
                //        Convert.ToInt16(instrumentArray[i].ItemArray.GetValue(3));
                //    optionInputFieldsFromTblOptionInputSymbols.multiplier =
                //        Convert.ToDouble(instrumentArray[i].ItemArray.GetValue(4));
                //}

                //return optionInputFieldsFromTblOptionInputSymbols;
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return null;

        }

        public DateTime updateIntradaySystemResults(RealtimeSystemResults[] realtimeSystemResults, int realtimeModel)
        {
            DateTime returnDateTime = new DateTime();

            try
            {
                ////TSErrorCatch.debugWriteOut(underlyingFutureContractProps.cqgTimedBars.Count + " test");

                ////if (underlyingFutureContractProps.cqgTimedBars != null &&
                ////    underlyingFutureContractProps.cqgTimedBars.Count > 0)
                //{
                //    returnDateTime = realtimeSystemResults[0].date;

                //    StringBuilder dataQuery = new StringBuilder();

                //    dataQuery.Append("SELECT * FROM tblsystemresults");
                //    dataQuery.Append(" WHERE idportfoliogroup = ");
                //    dataQuery.Append(realtimeSystemResults[0].idPortfoliogroup);
                //    dataQuery.Append(" AND realtimemodel = " + realtimeModel);
                //    dataQuery.Append(" AND date = '");
                //    dataQuery.Append(realtimeSystemResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //    dataQuery.Append("'");
                //    dataQuery.Append(" AND idstrategy = ");
                //    dataQuery.Append(realtimeSystemResults[0].idStrategy);

                //    MySqlCommand cmdTestForUpdate = new MySqlCommand(dataQuery.ToString(), conn);

                //    MySqlDataReader testreader = cmdTestForUpdate.ExecuteReader();

                //    bool continueToInsert = true;

                //    if (testreader.Read())
                //    {
                //        continueToInsert = false;

                //    }

                //    testreader.Close();

                //    //TSErrorCatch.debugWriteOut(dataQuery);

                //    //MySqlCommand cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);
                //    //cmdInsertContractBarData.ExecuteNonQuery();

                //    if (continueToInsert)
                //    {

                //        dataQuery.Clear();

                //        dataQuery.Append("INSERT INTO tblsystemresults ");
                //        dataQuery.Append("(realtimemodel,idportfoliogroup,idstrategy,date,pldaychg,deltaday,dfltprice,");
                //        dataQuery.Append("theorprice,spanimplvol,settleimplvol,settlement,syntheticclose,");
                //        dataQuery.Append("transprice,transtime,entryrule,exitrule)");
                //        dataQuery.Append(" VALUE");
                //        dataQuery.Append("(");
                //        dataQuery.Append(realtimeModel);
                //        dataQuery.Append(",?IDPORTFOLIOGROUP,?IDSTRATEGY,?DATE,?PLDAYCHG,?DELTADAY,?DFLTPRICE,");
                //        dataQuery.Append("?THEORPRICE,?SPANIMPLVOL,?SETTLEIMPLVOL,?SETTLEMENT,?SYNTHETICCLOSE,");
                //        dataQuery.Append("?TRANSPRICE,?TRANSTIME,?ENTRYRULE,?EXITRULE)");

                //        MySqlCommand cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);

                //        cmdInsertContractBarData.Prepare();
                //        cmdInsertContractBarData.Parameters.AddWithValue("?IDPORTFOLIOGROUP", realtimeSystemResults[0].idPortfoliogroup);
                //        cmdInsertContractBarData.Parameters.AddWithValue("?IDSTRATEGY", realtimeSystemResults[0].idStrategy);
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DATE",
                //                realtimeSystemResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?PLDAYCHG", realtimeSystemResults.plDayChg.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?DELTADAY", realtimeSystemResults.deltaDay.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?DFLTPRICE", realtimeSystemResults.dfltPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?THEORPRICE", realtimeSystemResults.theorPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SPANIMPLVOL", realtimeSystemResults.spanImplVol.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEIMPLVOL", realtimeSystemResults.settleImplVol.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SYNTHETICCLOSE", realtimeSystemResults.syntheticClose.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?TRANSPRICE", realtimeSystemResults.transPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?TRANSTIME", realtimeSystemResults.transTime.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SPREADACTION", realtimeSystemResults.spreadAction.ToString());

                //        cmdInsertContractBarData.Parameters.AddWithValue("?PLDAYCHG", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DELTADAY", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DFLTPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?THEORPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SPANIMPLVOL", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEIMPLVOL", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEMENT", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SYNTHETICCLOSE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?TRANSPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?TRANSTIME", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?ENTRYRULE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?EXITRULE", "");

                //        //cmdInsertContractBarData.ExecuteNonQuery();

                //        for (int i = 0; i < realtimeSystemResults.Length; i++)
                //        {
                //            cmdInsertContractBarData.Parameters["?IDPORTFOLIOGROUP"].Value = realtimeSystemResults[i].idPortfoliogroup;
                //            cmdInsertContractBarData.Parameters["?IDSTRATEGY"].Value = realtimeSystemResults[i].idStrategy;
                //            cmdInsertContractBarData.Parameters["?DATE"].Value =
                //                    realtimeSystemResults[i].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);


                //            cmdInsertContractBarData.Parameters["?PLDAYCHG"].Value = realtimeSystemResults[i].plDayChg.ToString();

                //            cmdInsertContractBarData.Parameters["?DELTADAY"].Value = realtimeSystemResults[i].deltaDay.ToString();
                //            cmdInsertContractBarData.Parameters["?DFLTPRICE"].Value = realtimeSystemResults[i].dfltPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?THEORPRICE"].Value = realtimeSystemResults[i].theorPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?SPANIMPLVOL"].Value = realtimeSystemResults[i].spanImplVol.ToString();
                //            cmdInsertContractBarData.Parameters["?SETTLEIMPLVOL"].Value = realtimeSystemResults[i].settleImplVol.ToString();
                //            cmdInsertContractBarData.Parameters["?SETTLEMENT"].Value = realtimeSystemResults[i].settlement.ToString();
                //            cmdInsertContractBarData.Parameters["?SYNTHETICCLOSE"].Value = realtimeSystemResults[i].syntheticClose.ToString();
                //            cmdInsertContractBarData.Parameters["?TRANSPRICE"].Value = realtimeSystemResults[i].transPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?TRANSTIME"].Value = realtimeSystemResults[i].transTime.ToString();
                //            cmdInsertContractBarData.Parameters["?ENTRYRULE"].Value = realtimeSystemResults[i].entryRule.ToString();
                //            cmdInsertContractBarData.Parameters["?EXITRULE"].Value = realtimeSystemResults[i].exitRule.ToString();

                //            cmdInsertContractBarData.ExecuteNonQuery();
                //        }

                //    }

                //}

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return returnDateTime;

        }

        public DateTime updateEndOfDaySystemResults(RealtimeSystemResults[] realtimeSystemResults, int realtimeModel)
        {
            DateTime returnDateTime = new DateTime();

            try
            {
                ////TSErrorCatch.debugWriteOut(underlyingFutureContractProps.cqgTimedBars.Count + " test");

                ////if (underlyingFutureContractProps.cqgTimedBars != null &&
                ////    underlyingFutureContractProps.cqgTimedBars.Count > 0)
                //{
                //    returnDateTime = realtimeSystemResults[0].date;

                //    StringBuilder dataQuery = new StringBuilder();

                //    dataQuery.Append("DELETE FROM tblsystemresults");
                //    dataQuery.Append(" WHERE idportfoliogroup = ");
                //    dataQuery.Append(realtimeSystemResults[0].idPortfoliogroup);
                //    dataQuery.Append(" AND realtimemodel = " + realtimeModel);
                //    dataQuery.Append(" AND date = '");
                //    dataQuery.Append(realtimeSystemResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //    dataQuery.Append("'");
                //    //dataQuery.Append(" AND idstrategy = ");
                //    //dataQuery.Append(realtimeSystemResults[0].idStrategy);

                //    MySqlCommand cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);
                //    cmdInsertContractBarData.ExecuteNonQuery();

                //    //MySqlCommand cmdTestForUpdate = new MySqlCommand(dataQuery.ToString(), conn);

                //    //MySqlDataReader testreader = cmdTestForUpdate.ExecuteReader();

                //    //bool continueToInsert = true;

                //    //if (testreader.Read())
                //    //{
                //    //    continueToInsert = false;

                //    //}

                //    //testreader.Close();

                //    //TSErrorCatch.debugWriteOut(dataQuery);

                //    //MySqlCommand cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);
                //    //cmdInsertContractBarData.ExecuteNonQuery();

                //    //if (continueToInsert)
                //    {

                //        dataQuery.Clear();

                //        dataQuery.Append("INSERT INTO tblsystemresults ");
                //        dataQuery.Append("(realtimemodel,idportfoliogroup,idstrategy,date,pldaychg,deltaday,dfltprice,");
                //        dataQuery.Append("theorprice,spanimplvol,settleimplvol,settlement,syntheticclose,");
                //        dataQuery.Append("transprice,transtime,entryrule,exitrule)");
                //        dataQuery.Append(" VALUE");
                //        dataQuery.Append("(");
                //        dataQuery.Append(realtimeModel);
                //        dataQuery.Append(",?IDPORTFOLIOGROUP,?IDSTRATEGY,?DATE,?PLDAYCHG,?DELTADAY,?DFLTPRICE,");
                //        dataQuery.Append("?THEORPRICE,?SPANIMPLVOL,?SETTLEIMPLVOL,?SETTLEMENT,?SYNTHETICCLOSE,");
                //        dataQuery.Append("?TRANSPRICE,?TRANSTIME,?ENTRYRULE,?EXITRULE)");

                //        cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);

                //        cmdInsertContractBarData.Prepare();
                //        cmdInsertContractBarData.Parameters.AddWithValue("?IDPORTFOLIOGROUP", realtimeSystemResults[0].idPortfoliogroup);
                //        cmdInsertContractBarData.Parameters.AddWithValue("?IDSTRATEGY", realtimeSystemResults[0].idStrategy);
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DATE",
                //                realtimeSystemResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?PLDAYCHG", realtimeSystemResults.plDayChg.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?DELTADAY", realtimeSystemResults.deltaDay.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?DFLTPRICE", realtimeSystemResults.dfltPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?THEORPRICE", realtimeSystemResults.theorPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SPANIMPLVOL", realtimeSystemResults.spanImplVol.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEIMPLVOL", realtimeSystemResults.settleImplVol.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SYNTHETICCLOSE", realtimeSystemResults.syntheticClose.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?TRANSPRICE", realtimeSystemResults.transPrice.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?TRANSTIME", realtimeSystemResults.transTime.ToString());
                //        //cmdInsertContractBarData.Parameters.AddWithValue("?SPREADACTION", realtimeSystemResults.spreadAction.ToString());

                //        cmdInsertContractBarData.Parameters.AddWithValue("?PLDAYCHG", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DELTADAY", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?DFLTPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?THEORPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SPANIMPLVOL", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEIMPLVOL", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEMENT", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?SYNTHETICCLOSE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?TRANSPRICE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?TRANSTIME", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?ENTRYRULE", "");
                //        cmdInsertContractBarData.Parameters.AddWithValue("?EXITRULE", "");

                //        //cmdInsertContractBarData.ExecuteNonQuery();

                //        for (int i = 0; i < realtimeSystemResults.Length; i++)
                //        {
                //            cmdInsertContractBarData.Parameters["?IDPORTFOLIOGROUP"].Value = realtimeSystemResults[i].idPortfoliogroup;
                //            cmdInsertContractBarData.Parameters["?IDSTRATEGY"].Value = realtimeSystemResults[i].idStrategy;
                //            cmdInsertContractBarData.Parameters["?DATE"].Value =
                //                    realtimeSystemResults[i].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);


                //            cmdInsertContractBarData.Parameters["?PLDAYCHG"].Value = realtimeSystemResults[i].plDayChg.ToString();

                //            cmdInsertContractBarData.Parameters["?DELTADAY"].Value = realtimeSystemResults[i].deltaDay.ToString();
                //            cmdInsertContractBarData.Parameters["?DFLTPRICE"].Value = realtimeSystemResults[i].dfltPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?THEORPRICE"].Value = realtimeSystemResults[i].theorPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?SPANIMPLVOL"].Value = realtimeSystemResults[i].spanImplVol.ToString();
                //            cmdInsertContractBarData.Parameters["?SETTLEIMPLVOL"].Value = realtimeSystemResults[i].settleImplVol.ToString();
                //            cmdInsertContractBarData.Parameters["?SETTLEMENT"].Value = realtimeSystemResults[i].settlement.ToString();
                //            cmdInsertContractBarData.Parameters["?SYNTHETICCLOSE"].Value = realtimeSystemResults[i].syntheticClose.ToString();
                //            cmdInsertContractBarData.Parameters["?TRANSPRICE"].Value = realtimeSystemResults[i].transPrice.ToString();
                //            cmdInsertContractBarData.Parameters["?TRANSTIME"].Value = realtimeSystemResults[i].transTime.ToString();
                //            cmdInsertContractBarData.Parameters["?ENTRYRULE"].Value = realtimeSystemResults[i].entryRule.ToString();
                //            cmdInsertContractBarData.Parameters["?EXITRULE"].Value = realtimeSystemResults[i].exitRule.ToString();

                //            cmdInsertContractBarData.ExecuteNonQuery();
                //        }

                //    }

                //}

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return returnDateTime;

        }



        public DateTime updateEndOfDaySystemPLResults(List<RealtimeSystemPLResults> realtimeSystemPLResults)
        {
            DateTime returnDateTime = new DateTime();

            try
            {
                //returnDateTime = realtimeSystemPLResults[0].date;

                //StringBuilder dataQuery = new StringBuilder();

                //dataQuery.Append("DELETE FROM tblsystemplresults");
                //dataQuery.Append(" WHERE idportfoliogroup = ");
                //dataQuery.Append(realtimeSystemPLResults[0].idPortfoliogroup);
                //dataQuery.Append(" AND realtimemodel = 0");
                //dataQuery.Append(" AND fcmData = ");
                //dataQuery.Append(realtimeSystemPLResults[0].fcmData);
                //dataQuery.Append(" AND date = '");
                //dataQuery.Append(realtimeSystemPLResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
                //dataQuery.Append("'");

                ////TSErrorCatch.debugWriteOut(dataQuery);

                //MySqlCommand cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);
                //cmdInsertContractBarData.ExecuteNonQuery();

                //dataQuery.Clear();

                //dataQuery.Append("INSERT INTO tblsystemplresults ");
                //dataQuery.Append("(realtimemodel,fcmData,idportfoliogroup,contract,strikePrice,idinstrument,date,");
                //dataQuery.Append("plDaychg,plLongOrderChg,plShortOrderChg,plSettleDayChg,plLongSettleOrderChg,");
                //dataQuery.Append("plShortSettleOrderChg,totalQty,longOrders,shortOrders,longTransAvgPrice,shortTransAvgPrice,");
                //dataQuery.Append("delta,defaultPrice,theoreticalPrice,spanImpliedVol,");
                //dataQuery.Append("settlementImpliedVol,impliedVol,bid,ask,last,settle,yesterdaySettle,idContract,idOption,callOrPutOrFuture)");
                //dataQuery.Append(" VALUE");
                //dataQuery.Append("(0,?FCMDATA,?IDPORTFOLIOGROUP,?CONTRACT,?STRIKEPRICE,?IDINSTRUMENT,?DATE,");
                //dataQuery.Append("?PLDAYCHG,?PLLONGORDERCHG,?PLSHORTORDERCHG,?PLSETTLEDAYCHG,?PLLONGSETTLEORDERCHG,?PLSHORTSETTLEORDERCHG,");
                //dataQuery.Append("?TOTALQTY,?LONGORDERS,?SHORTORDERS,?LONGTRANSAVGPRICE,?SHORTTRANSAVGPRICE,");
                //dataQuery.Append("?DELTA,?DEFAULTPRICE,?THEORETICALPRICE,?SPANIMPLIEDVOL,");
                //dataQuery.Append("?SETTLEMENTIMPLIEDVOL,?IMPLIEDVOL,?BID,?ASK,?LAST,?SETTLE,?YESTSETTLE,?IDCONTRACT,?IDOPTION,?CALLORPUTORFUTURE)");

                //cmdInsertContractBarData = new MySqlCommand(dataQuery.ToString(), conn);

                //cmdInsertContractBarData.Prepare();
                //cmdInsertContractBarData.Parameters.AddWithValue("?FCMDATA", realtimeSystemPLResults[0].fcmData);
                //cmdInsertContractBarData.Parameters.AddWithValue("?IDPORTFOLIOGROUP", realtimeSystemPLResults[0].idPortfoliogroup);
                //cmdInsertContractBarData.Parameters.AddWithValue("?DATE",
                //        realtimeSystemPLResults[0].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));



                //cmdInsertContractBarData.Parameters.AddWithValue("?CONTRACT", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?STRIKEPRICE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?IDINSTRUMENT", "");
                
                //cmdInsertContractBarData.Parameters.AddWithValue("?PLDAYCHG", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?PLLONGORDERCHG", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?PLSHORTORDERCHG", "");

                //cmdInsertContractBarData.Parameters.AddWithValue("?PLSETTLEDAYCHG", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?PLLONGSETTLEORDERCHG", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?PLSHORTSETTLEORDERCHG", "");

                //cmdInsertContractBarData.Parameters.AddWithValue("?TOTALQTY", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?LONGORDERS", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?SHORTORDERS", "");

                //cmdInsertContractBarData.Parameters.AddWithValue("?LONGTRANSAVGPRICE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?SHORTTRANSAVGPRICE", "");

                //cmdInsertContractBarData.Parameters.AddWithValue("?DELTA", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?DEFAULTPRICE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?THEORETICALPRICE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?SPANIMPLIEDVOL", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?SETTLEMENTIMPLIEDVOL", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?IMPLIEDVOL", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?BID", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?ASK", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?LAST", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?SETTLE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?YESTSETTLE", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?IDCONTRACT", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?IDOPTION", "");
                //cmdInsertContractBarData.Parameters.AddWithValue("?CALLORPUTORFUTURE", "");

                //for (int i = 0; i < realtimeSystemPLResults.Count; i++)
                //{
                //    cmdInsertContractBarData.Parameters["?FCMDATA"].Value = realtimeSystemPLResults[i].fcmData;
                //    cmdInsertContractBarData.Parameters["?IDPORTFOLIOGROUP"].Value = realtimeSystemPLResults[i].idPortfoliogroup;
                //    cmdInsertContractBarData.Parameters["?DATE"].Value =
                //            realtimeSystemPLResults[i].date.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);

                //    cmdInsertContractBarData.Parameters["?CONTRACT"].Value = realtimeSystemPLResults[i].contract.ToString();
                //    cmdInsertContractBarData.Parameters["?STRIKEPRICE"].Value = realtimeSystemPLResults[i].strikePrice.ToString();
                //    cmdInsertContractBarData.Parameters["?IDINSTRUMENT"].Value = realtimeSystemPLResults[i].idInstrument.ToString();
                    
                //    cmdInsertContractBarData.Parameters["?PLDAYCHG"].Value = realtimeSystemPLResults[i].plDayChg.ToString();
                //    cmdInsertContractBarData.Parameters["?PLLONGORDERCHG"].Value = realtimeSystemPLResults[i].plLongOrderChg.ToString();
                //    cmdInsertContractBarData.Parameters["?PLSHORTORDERCHG"].Value = realtimeSystemPLResults[i].plShortOrderChg.ToString();
                    
                //    cmdInsertContractBarData.Parameters["?PLSETTLEDAYCHG"].Value = realtimeSystemPLResults[i].plSettleDayChg.ToString();
                //    cmdInsertContractBarData.Parameters["?PLLONGSETTLEORDERCHG"].Value = realtimeSystemPLResults[i].plLongSettleOrderChg.ToString();
                //    cmdInsertContractBarData.Parameters["?PLSHORTSETTLEORDERCHG"].Value = realtimeSystemPLResults[i].plShortSettleOrderChg.ToString();
                    
                //    cmdInsertContractBarData.Parameters["?TOTALQTY"].Value = realtimeSystemPLResults[i].totalQty.ToString();
                //    cmdInsertContractBarData.Parameters["?LONGORDERS"].Value = realtimeSystemPLResults[i].longOrders.ToString();
                //    cmdInsertContractBarData.Parameters["?SHORTORDERS"].Value = realtimeSystemPLResults[i].shortOrders.ToString();

                //    cmdInsertContractBarData.Parameters["?LONGTRANSAVGPRICE"].Value = realtimeSystemPLResults[i].longTransAvgPrice.ToString();
                //    cmdInsertContractBarData.Parameters["?SHORTTRANSAVGPRICE"].Value = realtimeSystemPLResults[i].shortTransAvgPrice.ToString();
                    
                    
                //    cmdInsertContractBarData.Parameters["?DELTA"].Value = realtimeSystemPLResults[i].delta.ToString();
                //    cmdInsertContractBarData.Parameters["?DEFAULTPRICE"].Value = realtimeSystemPLResults[i].defaultPrice.ToString();
                //    cmdInsertContractBarData.Parameters["?THEORETICALPRICE"].Value = realtimeSystemPLResults[i].theoreticalPrice.ToString();
                //    cmdInsertContractBarData.Parameters["?SPANIMPLIEDVOL"].Value = realtimeSystemPLResults[i].spanImpliedVol.ToString();
                //    cmdInsertContractBarData.Parameters["?SETTLEMENTIMPLIEDVOL"].Value = realtimeSystemPLResults[i].settlementImpliedVol.ToString();
                //    cmdInsertContractBarData.Parameters["?IMPLIEDVOL"].Value = realtimeSystemPLResults[i].impliedVol.ToString();
                //    cmdInsertContractBarData.Parameters["?BID"].Value = realtimeSystemPLResults[i].bid.ToString();
                //    cmdInsertContractBarData.Parameters["?ASK"].Value = realtimeSystemPLResults[i].ask.ToString();
                //    cmdInsertContractBarData.Parameters["?LAST"].Value = realtimeSystemPLResults[i].last.ToString();
                //    cmdInsertContractBarData.Parameters["?SETTLE"].Value = realtimeSystemPLResults[i].settle.ToString();
                //    cmdInsertContractBarData.Parameters["?YESTSETTLE"].Value = realtimeSystemPLResults[i].yesterdaySettle.ToString();
                //    cmdInsertContractBarData.Parameters["?IDCONTRACT"].Value = realtimeSystemPLResults[i].idContract.ToString();
                //    cmdInsertContractBarData.Parameters["?IDOPTION"].Value = realtimeSystemPLResults[i].idOption.ToString();
                //    cmdInsertContractBarData.Parameters["?CALLORPUTORFUTURE"].Value = realtimeSystemPLResults[i].callOrPutOrFutureChar.ToString();

                //    cmdInsertContractBarData.ExecuteNonQuery();
                //}

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            return returnDateTime;

        }

        public DateTime getPreviousDayXXX(DateTime queryDateTime)
        {
            //StringBuilder dataQuery = new StringBuilder();

            //dataQuery.Append("SELECT date FROM tbldailycontractsettlements WHERE date < '");
            //dataQuery.Append(queryDateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
            //dataQuery.Append("' ORDER BY date DESC LIMIT 1");

            //MySqlCommand cmdTestForUpdate = new MySqlCommand(dataQuery.ToString(), conn);

            //MySqlDataReader testreader = cmdTestForUpdate.ExecuteReader();

            //if (testreader.Read())
            //{
            //    queryDateTime = testreader.GetDateTime("date");
            //}

            //testreader.Close();

            return queryDateTime;
        }

        public DataRow[] selectOptionSystemPlResults(DateTime queryDateTime, int idPortfoliogroup, int fcmData)
        {
            //StringBuilder dataQuery = new StringBuilder();

            //dataQuery.Append("SELECT tblsystemplresults.contract, tblsystemplresults.idinstrument,");
            //dataQuery.Append(" tblsystemplresults.date,");
            //dataQuery.Append(" tblsystemplresults.plSettleDayChg, tblsystemplresults.plLongSettleOrderChg, tblsystemplresults.plShortSettleOrderChg,");
            //dataQuery.Append(" tblsystemplresults.totalQty, tblsystemplresults.longOrders, tblsystemplresults.shortOrders,");
            //dataQuery.Append(" tblsystemplresults.longTransAvgPrice, tblsystemplresults.shortTransAvgPrice,");
            //dataQuery.Append(" tblsystemplresults.settle, tblsystemplresults.yesterdaySettle, tblsystemplresults.settlementImpliedVol,");
            //dataQuery.Append(" tblsystemplresults.idContract, tblsystemplresults.idOption, tblsystemplresults.callOrPutOrFuture,");

            //dataQuery.Append(" tbloptiondata.datetime, tbloptiondata.price, tbloptiondata.impliedVol,");

            //dataQuery.Append(" tbloptions.expirationdate");

            //dataQuery.Append(" FROM tblsystemplresults, tbloptiondata, tbloptions");

            //dataQuery.Append(" WHERE tblsystemplresults.callOrPutOrFuture != 'F'");
            
            //dataQuery.Append(" AND tblsystemplresults.idportfoliogroup = ");
            //dataQuery.Append(idPortfoliogroup);

            //dataQuery.Append(" AND tblsystemplresults.fcmData = ");
            //dataQuery.Append(fcmData);

            //dataQuery.Append(" AND tblsystemplresults.date = '");
            //dataQuery.Append(queryDateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));

            //dataQuery.Append("' AND tblsystemplresults.idoption = tbloptiondata.idoption");

            //dataQuery.Append(" AND tblsystemplresults.idoption = tbloptions.idoption");

            //dataQuery.Append(" AND tbloptiondata.datetime = '");
            //dataQuery.Append(queryDateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));
            
            //dataQuery.Append("' ORDER BY idinstrument");

            //MySqlDataAdapter cmdGetSystemPlResults = new MySqlDataAdapter(dataQuery.ToString(), conn);

            DataSet systemPlResultsDataSet = new DataSet();

            //int configRows = cmdGetSystemPlResults.Fill(systemPlResultsDataSet);

            DataRow[] systemPlResults = systemPlResultsDataSet.Tables[0].Select();

            

            return systemPlResults;
        }

        public DataRow[] selectFutureSystemPlResults(DateTime queryDateTime, int idPortfoliogroup, int fcmData)
        {
            //StringBuilder dataQuery = new StringBuilder();

            //dataQuery.Append("SELECT tblsystemplresults.contract, tblsystemplresults.idinstrument,");
            //dataQuery.Append(" tblsystemplresults.date,");
            //dataQuery.Append(" tblsystemplresults.plSettleDayChg, tblsystemplresults.plLongSettleOrderChg, tblsystemplresults.plShortSettleOrderChg,");
            //dataQuery.Append(" tblsystemplresults.totalQty, tblsystemplresults.longOrders, tblsystemplresults.shortOrders,");
            //dataQuery.Append(" tblsystemplresults.longTransAvgPrice, tblsystemplresults.shortTransAvgPrice,");
            //dataQuery.Append(" tblsystemplresults.settle, tblsystemplresults.yesterdaySettle,");
            //dataQuery.Append(" tblsystemplresults.idContract, tblsystemplresults.idOption, tblsystemplresults.callOrPutOrFuture,");

            //dataQuery.Append(" tbldailycontractsettlements.date, tbldailycontractsettlements.settlement,");

            //dataQuery.Append(" tblcontracts.expirationdate");

            //dataQuery.Append(" FROM tblsystemplresults, tbldailycontractsettlements, tblcontracts");

            //dataQuery.Append(" WHERE tblsystemplresults.callOrPutOrFuture = 'F'");

            //dataQuery.Append(" AND tblsystemplresults.idportfoliogroup = ");
            //dataQuery.Append(idPortfoliogroup);

            //dataQuery.Append(" AND tblsystemplresults.fcmData = ");
            //dataQuery.Append(fcmData);

            //dataQuery.Append(" AND tblsystemplresults.date = '");
            //dataQuery.Append(queryDateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));

            //dataQuery.Append("' AND tblsystemplresults.idContract = tbldailycontractsettlements.idcontract");

            //dataQuery.Append(" AND tblsystemplresults.idContract = tblcontracts.idcontract");

            //dataQuery.Append(" AND tbldailycontractsettlements.date = '");
            //dataQuery.Append(queryDateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));

            //dataQuery.Append("' ORDER BY idinstrument");

            //MySqlDataAdapter cmdGetSystemPlResults = new MySqlDataAdapter(dataQuery.ToString(), conn);

            DataSet systemPlResultsDataSet = new DataSet();

            //int configRows = cmdGetSystemPlResults.Fill(systemPlResultsDataSet);

            DataRow[] systemPlResults = systemPlResultsDataSet.Tables[0].Select();

            return systemPlResults;
        }


        
    }
}
