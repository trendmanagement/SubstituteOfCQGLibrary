//using CQG;
using RealtimeSpreadMonitor.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CQGLibrary = FakeCQG;

namespace RealtimeSpreadMonitor.FormManipulation
{
    internal class GridViewFCMPostionManipulation
    {

        internal GridViewFCMPostionManipulation(
            OptionSpreadManager optionSpreadManager)
            //StatusAndConnectedUpdates statusAndConnectedUpdates)
        {
            this.optionSpreadManager = optionSpreadManager;
            //this.statusAndConnectedUpdates = statusAndConnectedUpdates;
        }

        private OptionSpreadManager optionSpreadManager;// { get; set; }
        //private StatusAndConnectedUpdates statusAndConnectedUpdates;// { get; set; }


        internal void setupGridLiveADMData(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridLiveFCMData = optionRealtimeMonitor.getGridLiveFCMData;

            try
            {

                Type liveColTypes = typeof(OPTION_LIVE_ADM_DATA_COLUMNS);
                Array liveColTypesArray = Enum.GetNames(liveColTypes);

                gridLiveFCMData.ColumnCount = liveColTypesArray.Length;

                gridLiveFCMData.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridLiveFCMData.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridLiveFCMData.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;

                gridLiveFCMData.Columns[0].Frozen = true;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < gridLiveFCMData.ColumnCount; i++)
                {
                    gridLiveFCMData.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    if (i != (int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_EDITABLE)
                    {
                        gridLiveFCMData.Columns[i].ReadOnly = true;
                    }

                    sb.Clear();

                    sb.Append(liveColTypesArray.GetValue(i).ToString());

                    gridLiveFCMData
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    gridLiveFCMData.Columns[i].Width = 50;
                }



                //for (int i = 0; i < liveColTypesArray.Length; i++)
                //{

                //}

                //************
                //gridLiveADMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CONTRACT].Width = 115;

                //gridLiveADMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LEG].Width = 30;
                //gridLiveADMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LEG].DefaultCellStyle.Font = new Font("Tahoma", 7);

                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME].Width = 70;
                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);
                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.EXPR].DefaultCellStyle.Font = new Font("Tahoma", 6);
                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.EXPR].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //************

                //List<LiveADMStrategyInfo> liveADMStrategyInfoList = optionSpreadManager.liveADMStrategyInfoList;

                fillGridLiveADMData(optionRealtimeMonitor);

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        delegate void ThreadSafeFillGridModelADMComparison(OptionRealtimeMonitor optionRealtimeMonitor);

        internal void fillGridLiveADMData(OptionRealtimeMonitor optionRealtimeMonitor)
        {
            DataGridView gridLiveFCMData = optionRealtimeMonitor.getGridLiveFCMData;

            if (optionRealtimeMonitor.InvokeRequired)
            {

                ThreadSafeFillGridModelADMComparison d = new ThreadSafeFillGridModelADMComparison(threadsafefillGridLiveADMData);

                optionRealtimeMonitor.Invoke(d, optionRealtimeMonitor);
            }
            else
            {
                threadsafefillGridLiveADMData(optionRealtimeMonitor);
            }

        }

        internal void threadsafefillGridLiveADMData(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridLiveFCMData = optionRealtimeMonitor.getGridLiveFCMData;

            try
            {

                List<ADMPositionImportWeb> admPositionImportWeb = optionSpreadManager.admPositionImportWeb;



                gridLiveFCMData.RowCount = admPositionImportWeb.Count;


                int rowIdx = 0;


                //Color rowColor1 = Color.DarkGray;
                //Color rowColor2 = Color.Black;

                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.DarkBlue;

                Color currentRowColor = rowColor1;

                for (int instrumentCnt = 0; instrumentCnt <= optionSpreadManager.instruments.Length; instrumentCnt++)
                {

                    for (int admWebPositionCounter = 0; admWebPositionCounter < admPositionImportWeb.Count; admWebPositionCounter++)
                    {
                        if (admPositionImportWeb[admWebPositionCounter].instrument.idxOfInstrumentInList == instrumentCnt)
                        {
                            admPositionImportWeb[admWebPositionCounter].liveADMRowIdx = rowIdx;

                            switch (rowIdx % 2)
                            {
                                case 0:
                                    currentRowColor = rowColor1;
                                    break;

                                case 1:
                                    currentRowColor = rowColor2;
                                    break;

                            }


                            gridLiveFCMData
                                    .Rows[rowIdx]
                                        .HeaderCell.Style.BackColor = currentRowColor;

                            gridLiveFCMData
                                    .Rows[rowIdx]
                                    .HeaderCell.Value =
                                        admPositionImportWeb[admWebPositionCounter].cqgSymbol;

                            //admPositionsCheckAndFillGrid(admPositionImportWeb[stratCounter],
                            //            rowIdx, true);

                            DateTime currentDate = DateTime.Now;



                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.POFFIC].Value =
                                admPositionImportWeb[admWebPositionCounter].POFFIC;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.PACCT].Value =
                                admPositionImportWeb[admWebPositionCounter].PACCT;



                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_EDITABLE].Value =
                                admPositionImportWeb[admWebPositionCounter].netContractsEditable;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.NET_AT_ADM].Value =
                                admPositionImportWeb[admWebPositionCounter].Net;


                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.LONG_TRANS].Value =
                                admPositionImportWeb[admWebPositionCounter].transNetLong;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.SHORT_TRANS].Value =
                                admPositionImportWeb[admWebPositionCounter].transNetShort;


                            //gridLiveADMData.Rows[rowCounter].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.MODEL].Value =
                            //                liveADMStrategyInfo.admLegInfo[legCounter].numberOfModelContracts;


                            TimeSpan span = admPositionImportWeb[admWebPositionCounter].contractInfo.expirationDate.Date - currentDate.Date;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CNTDN].Value =
                                                span.Days;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.EXPR].Value =
                                new DateTime(
                                                    admPositionImportWeb[admWebPositionCounter].contractInfo.expirationDate.Year,
                                                    admPositionImportWeb[admWebPositionCounter].contractInfo.expirationDate.Month,
                                                    admPositionImportWeb[admWebPositionCounter].contractInfo.expirationDate.Day,
                                                    admPositionImportWeb[admWebPositionCounter].contractInfo.optionExpirationTime.Hour,
                                                    admPositionImportWeb[admWebPositionCounter].contractInfo.optionExpirationTime.Minute,
                                                    0
                                                )
                                                .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo);

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.AVG_LONG_TRANS_PRC].Value =
                                admPositionImportWeb[admWebPositionCounter].transAvgLongPrice;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.AVG_SHORT_TRANS_PRC].Value =
                                admPositionImportWeb[admWebPositionCounter].transAvgShortPrice;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.STRIKE].Value =
                                admPositionImportWeb[admWebPositionCounter].strike;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.DESCRIP].Value =
                                admPositionImportWeb[admWebPositionCounter].Description;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.CUSIP].Value =
                                admPositionImportWeb[admWebPositionCounter].PCUSIP;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.ADMPOSWEB_IDX].Value =
                                admWebPositionCounter;

                            gridLiveFCMData.Rows[rowIdx].Cells[(int)OPTION_LIVE_ADM_DATA_COLUMNS.INSTRUMENT_ID].Value = instrumentCnt;


                            //*********************************


                            gridLiveFCMData
                                    .Rows[rowIdx]
                                        .HeaderCell.Style.BackColor = currentRowColor;

                            rowIdx++;

                            //rowIdx += liveADMStrategyInfoList[stratCounter].admLegInfo.Count + 1;



                        }
                    }
                }

                //updateColorOfADMStrategyGrid();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            //return liveLegRowIdexes;
        }

        public void sendUpdateToADMPositionsGrid(OptionRealtimeMonitor optionRealtimeMonitor)  //*eQuoteType cqgQuoteType,*/ int spreadExpressionIdx /*int colIdx*/)
        {
            //CQGQuote quote = optionSpreadExpressionList[spreadExpressionIdx].cqgInstrument.Quotes[cqgQuoteType];

            DataGridView gridLiveFCMData = optionRealtimeMonitor.getGridLiveFCMData;

            try
            {

                List<ADMPositionImportWeb> admPositionImportWeb = optionSpreadManager.admPositionImportWeb;

                //int optionSpreadCounter = 0;

                //List<LiveADMStrategyInfo> liveADMStrategyInfoList = optionSpreadManager.liveADMStrategyInfoList;

                for (int admWebPositionCounter = 0; admWebPositionCounter < admPositionImportWeb.Count; admWebPositionCounter++)
                {

                    //    int totalLegs = liveADMStrategyInfoList[optionSpreadCounter].admLegInfo.Count;

                    //    //if (optionSpreadExpressionList[spreadExpressionIdx].cqgInstrument != null)
                    //    for (int legCounter = 0; legCounter < totalLegs; legCounter++)
                    //    {
                    if (admPositionImportWeb[admWebPositionCounter].contractData.optionSpreadExpression != null)
                    {
                        CQGLibrary.CQGInstrument cqgInstrument = admPositionImportWeb[admWebPositionCounter].contractData.optionSpreadExpression.cqgInstrument;

                        if (cqgInstrument != null)  // && CQG. cqgInstrument)
                        {
                            OptionSpreadExpression optionSpreadExpressionList =
                                admPositionImportWeb[admWebPositionCounter].contractData.optionSpreadExpression;

                            //checkUpdateStatus(admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                            //    optionSpreadExpressionList, true);

                            optionSpreadManager.statusAndConnectedUpdates.checkUpdateStatus(gridLiveFCMData, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME,
                                    optionSpreadExpressionList);

                            //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                            if (optionSpreadExpressionList.instrument.eodAnalysisAtInstrument)
                            {
                                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                                fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME,
                                        optionSpreadExpressionList.lastTimeUpdated.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }
                            else
                            {
                                gridLiveFCMData.Columns[(int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 8);

                                fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.TIME,
                                        optionSpreadExpressionList.lastTimeUpdated.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        false, 0);
                            }

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.ASK,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.ask), false, optionSpreadExpressionList.ask);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.BID,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.bid), false, optionSpreadExpressionList.bid);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.LAST,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.trade), false, optionSpreadExpressionList.trade);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.DFLT_PRICE,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.defaultPrice),
                                        false, optionSpreadExpressionList.defaultPrice);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                    (int)OPTION_LIVE_ADM_DATA_COLUMNS.STTLE,
                                    cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.settlement),
                                        false, optionSpreadExpressionList.settlement);

                            if (optionSpreadExpressionList.settlementDateTime.Date.CompareTo(DateTime.Now.Date) >= 0)
                            {
                                fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadExpressionList.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, 1);
                            }
                            else
                            {
                                fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadExpressionList.settlementDateTime.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                            true, -1);
                            }

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.YEST_STTLE,
                                        cqgInstrument.ToDisplayPrice(optionSpreadExpressionList.yesterdaySettlement),
                                            false, optionSpreadExpressionList.yesterdaySettlement);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.impliedVol, 2).ToString(),
                                        false, optionSpreadExpressionList.impliedVol);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.THEOR_PRICE,
                                        cqgInstrument.ToDisplayPrice(
                                        optionSpreadExpressionList.theoreticalOptionPrice),
                                        false, optionSpreadExpressionList.theoreticalOptionPrice);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.SPAN_IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.impliedVolFromSpan, 2).ToString(),
                                        false, optionSpreadExpressionList.impliedVolFromSpan);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.SETL_IMPL_VOL,
                                        Math.Round(optionSpreadExpressionList.settlementImpliedVol, 2).ToString(),
                                        false, optionSpreadExpressionList.settlementImpliedVol);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.RFR,
                                        optionSpreadExpressionList.riskFreeRate.ToString(),
                                        false, optionSpreadExpressionList.riskFreeRate);


                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.PL_DAY_CHG,
                                            Math.Round(admPositionImportWeb[admWebPositionCounter].contractData.pAndLDay, 2).ToString(),
                                            true, admPositionImportWeb[admWebPositionCounter].contractData.pAndLDay);

                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.PL_TRANS,
                                            Math.Round(admPositionImportWeb[admWebPositionCounter].contractData.pAndLDayOrders, 2).ToString(),
                                            true, admPositionImportWeb[admWebPositionCounter].contractData.pAndLDayOrders);

                            //fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                            //            (int)OPTION_LIVE_ADM_DATA_COLUMNS.PL_DAY_CHG,
                            //                Math.Round(admPositionImportWeb[admWebPositionCounter].contractData.pAndLDay, 2).ToString(),
                            //                true, admPositionImportWeb[admWebPositionCounter].contractData.pAndLDay);


                            fillLiveADMDataPage(optionRealtimeMonitor, admPositionImportWeb[admWebPositionCounter].liveADMRowIdx,
                                        (int)OPTION_LIVE_ADM_DATA_COLUMNS.DELTA,
                                            Math.Round(admPositionImportWeb[admWebPositionCounter].contractData.delta, 2).ToString(),
                                            true, admPositionImportWeb[admWebPositionCounter].contractData.delta);



                            //            //int numberOfContracts = (int)optionStrategies[optionSpreadCounter].optionStrategyParameters[
                            //            //                    (int)TBL_STRATEGY_STATE_FIELDS.currentPosition].stateValueParsed[legCounter];

                            //            //fillLiveADMDataPage(liveADMStrategyInfoList[optionSpreadCounter].admLegInfo[legCounter].rowIndex,
                            //            //            (int)OPTION_LIVE_ADM_DATA_COLUMNS.SPREAD_QTY,
                            //            //                numberOfContracts.ToString(), true, numberOfContracts);
                            //        }
                        }
                    }

                    //    fillLiveADMDataPage(liveADMStrategyInfoList[optionSpreadCounter].summaryRowIdx,
                    //                (int)OPTION_LIVE_ADM_DATA_COLUMNS.PL_DAY_CHG,
                    //                    Math.Round(liveADMStrategyInfoList[optionSpreadCounter].liveSpreadADMTotals.pAndLDay, 2).ToString(),
                    //                    true, liveADMStrategyInfoList[optionSpreadCounter].liveSpreadADMTotals.pAndLDay);

                    //    fillLiveADMDataPage(liveADMStrategyInfoList[optionSpreadCounter].summaryRowIdx,
                    //                (int)OPTION_LIVE_ADM_DATA_COLUMNS.DELTA,
                    //                    Math.Round(liveADMStrategyInfoList[optionSpreadCounter].liveSpreadADMTotals.delta, 2).ToString(),
                    //                    true, liveADMStrategyInfoList[optionSpreadCounter].liveSpreadADMTotals.delta);

                    //    optionSpreadCounter++;
                }



            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

        }

        delegate void ThreadSafeFillLiveDataPageDelegate(OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value);

        public void fillLiveADMDataPage(OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (optionRealtimeMonitor.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillLiveADMDataPage);

                    optionRealtimeMonitor.Invoke(d, optionRealtimeMonitor, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillLiveADMDataPage(optionRealtimeMonitor, row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillLiveADMDataPage(OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value)
        {
            DataGridView gridLiveFCMData = optionRealtimeMonitor.getGridLiveFCMData;

            try
            {
                int rowToUpdate = row;

                if (gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    )
                {
                    gridLiveFCMData.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        gridLiveFCMData.Rows[rowToUpdate].Cells[col].Style.BackColor = CommonFormManipulation.plUpDownColor(value);
                    }
                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

    }
}
