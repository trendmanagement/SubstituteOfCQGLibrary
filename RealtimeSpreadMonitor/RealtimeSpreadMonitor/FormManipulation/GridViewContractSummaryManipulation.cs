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
    internal class GridViewContractSummaryManipulation
    {
        internal GridViewContractSummaryManipulation(
            //OptionRealtimeMonitor optionRealtimeMonitor,
            //DataGridView gridViewContractSummary,
            OptionSpreadManager optionSpreadManager,
            StatusAndConnectedUpdates statusAndConnectedUpdates)
        {
            //this.optionRealtimeMonitor = optionRealtimeMonitor;
            //this.gridViewContractSummary = gridViewContractSummary;
            this.optionSpreadManager = optionSpreadManager;
            this.statusAndConnectedUpdates = statusAndConnectedUpdates;
        }

        //private OptionRealtimeMonitor optionRealtimeMonitor { get; set; }
        //private DataGridView gridViewContractSummary { get; set; }
        private OptionSpreadManager optionSpreadManager;// { get; set; }
        private StatusAndConnectedUpdates statusAndConnectedUpdates;// { get; set; }


        public void setupContractSummaryLiveData(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridViewContractSummary = optionRealtimeMonitor.getGridViewContractSummary;

            try
            {
                Type contractSummaryColTypes = typeof(CONTRACTSUMMARY_DATA_COLUMNS);
                Array contractSummaryColTypesArray = Enum.GetNames(contractSummaryColTypes);


                gridViewContractSummary.ColumnCount = contractSummaryColTypesArray.Length;

                gridViewContractSummary.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridViewContractSummary.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridViewContractSummary.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;


                gridViewContractSummary.Columns[0].Frozen = true;

                for (int i = 0; i < gridViewContractSummary.ColumnCount; i++)
                {
                    gridViewContractSummary.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < contractSummaryColTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(contractSummaryColTypesArray.GetValue(i).ToString());

                    gridViewContractSummary
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    gridViewContractSummary.Columns[i].Width = 50;
                }

                //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.CONTRACT].Width = 115;

                //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.LEG].Width = 30;
                //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.LEG].DefaultCellStyle.Font = new Font("Tahoma", 7);

                gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.TIME].Width = 70;
                gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


                gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);
                gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_TIME].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.EXPR].DefaultCellStyle.Font = new Font("Tahoma", 6);
                //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.EXPR].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                int rowCount = 0;

                for (int i = 0; i < optionSpreadManager.optionSpreadExpressionList.Count; i++)
                {
                    if (optionSpreadManager.optionSpreadExpressionList[i].optionExpressionType
                        != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                        && optionSpreadManager.optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0)
                    {
                        rowCount++;
                    }
                }

                gridViewContractSummary.RowCount = rowCount;

                Color rowColor1 = Color.DarkGray;
                Color rowColor2 = Color.Black;

                Color currentRowColor = rowColor1;

                int heldLotsExpressionCnt = 0;



                for (int instrumentCnt = 0; instrumentCnt <= optionSpreadManager.instruments.Length; instrumentCnt++)
                {


                    for (int i = 0; i < optionSpreadManager.optionSpreadExpressionList.Count; i++)
                    {
                        if (optionSpreadManager.optionSpreadExpressionList[i].optionExpressionType
                            != OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE
                            &&
                            optionSpreadManager.optionSpreadExpressionList[i].instrument.idxOfInstrumentInList == instrumentCnt)
                        {

                            if (optionSpreadManager.optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary != 0)
                            {

                                switch (heldLotsExpressionCnt % 2)
                                {
                                    case 0:
                                        currentRowColor = rowColor1;
                                        break;

                                    case 1:
                                        currentRowColor = rowColor2;
                                        break;
                                }

                                optionSpreadManager.contractSummaryExpressionListIdx.Add(i);

                                //TSErrorCatch.debugWriteOut(optionSpreadManager.optionSpreadExpressionList[i].cqgSymbol + " " + i);

                                gridViewContractSummary
                                    .Rows[heldLotsExpressionCnt]
                                        .HeaderCell.Style.BackColor = currentRowColor;

                                gridViewContractSummary
                                    .Rows[heldLotsExpressionCnt]
                                        .HeaderCell.Value =
                                            optionSpreadManager.optionSpreadExpressionList[i].cqgSymbol;

                                for (int j = 0; j < gridViewContractSummary.ColumnCount; j++)
                                {
                                    gridViewContractSummary.Rows[heldLotsExpressionCnt].Cells[j] = new CustomDataGridViewCell(true);
                                }

                                //gridViewContractSummary
                                //    .Rows[heldLotsExpressionCnt].Cells[(int)CONTRACTSUMMARY_DATA_COLUMNS.TOTAL_QTY].Value =
                                //    optionSpreadManager.optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary;



                                gridViewContractSummary
                                        .Rows[heldLotsExpressionCnt].Cells[(int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_TIME].Value =
                                        optionSpreadManager.optionSpreadExpressionList[i].settlementDateTime;

                                gridViewContractSummary
                                        .Rows[heldLotsExpressionCnt].Cells[(int)CONTRACTSUMMARY_DATA_COLUMNS.STRIKE_PRICE].Value =
                                        optionSpreadManager.optionSpreadExpressionList[i].strikePrice;

                                gridViewContractSummary
                                        .Rows[heldLotsExpressionCnt].Cells[(int)CONTRACTSUMMARY_DATA_COLUMNS.INSTRUMENT_ID].Value =
                                        optionSpreadManager.optionSpreadExpressionList[i].instrument.idxOfInstrumentInList;

                                heldLotsExpressionCnt++;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void sendUpdateToContractSummaryLiveData(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridViewContractSummary = optionRealtimeMonitor.getGridViewContractSummary;

            try
            {

                for (int contractSummaryExpressionCnt = 0;
                    contractSummaryExpressionCnt < optionSpreadManager.contractSummaryExpressionListIdx.Count(); contractSummaryExpressionCnt++)
                {
                    CQGLibrary.CQGInstrument cqgInstrument =
                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                            .cqgInstrument;

                    if (cqgInstrument != null)
                    {

                        statusAndConnectedUpdates.checkUpdateStatus(gridViewContractSummary, contractSummaryExpressionCnt,
                                    (int)CONTRACTSUMMARY_DATA_COLUMNS.TIME,
                                    optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]);
                        

                        //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                        if(optionSpreadManager
                            .optionSpreadExpressionList
                                [optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                    .instrument.eodAnalysisAtInstrument)
                        {
                            //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

                            fillContractSummaryLiveData(optionRealtimeMonitor,
                                contractSummaryExpressionCnt,
                                                (int)CONTRACTSUMMARY_DATA_COLUMNS.TIME,
                                                optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                                    .lastTimeUpdated.ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                                false, 0);
                        }
                        else
                        {
                            //gridViewContractSummary.Columns[(int)CONTRACTSUMMARY_DATA_COLUMNS.TIME].DefaultCellStyle.Font = new Font("Tahoma", 8);

                            fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                                (int)CONTRACTSUMMARY_DATA_COLUMNS.TIME,
                                                optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                                    .lastTimeUpdated.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo),
                                                false, 0);
                        }


                        //                gridViewContractSummary
                        //                            .Rows[heldLotsExpressionCnt].Cells[(int)CONTRACTSUMMARY_DATA_COLUMNS.TOTAL_QTY].Value =
                        //                            optionSpreadExpressionList[i].numberOfLotsHeldForContractSummary
                        //                            * optionSpreadManager.portfolioGroupTotalMultiple;






                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                    (int)CONTRACTSUMMARY_DATA_COLUMNS.ASK,
                                    cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].ask),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].ask);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.BID,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].bid),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].bid);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.LAST,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].trade),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].trade);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.DFLT_PRICE,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].defaultPrice),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].defaultPrice);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.STTLE,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlement),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlement);

                        if (optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                .settlementDateTime.Date.CompareTo(DateTime.Now.Date) >= 0)
                        {
                            fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlementDateTime
                                        .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true, 1);
                        }
                        else
                        {
                            fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_TIME,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlementDateTime
                                        .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
                                        true, -1);
                        }

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.YEST_STTLE,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].yesterdaySettlement),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].yesterdaySettlement);



                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.IMPL_VOL,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].impliedVol),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].impliedVol);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.THEOR_PRICE,
                                cqgInstrument.ToDisplayPrice(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].theoreticalOptionPrice),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].theoreticalOptionPrice);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.SPAN_IMPL_VOL,
                                Math.Round(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].impliedVolFromSpan,
                                        2).ToString(),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].impliedVolFromSpan);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.SETL_IMPL_VOL,
                                Math.Round(
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlementImpliedVol,
                                        2).ToString(),
                                        false,
                                        optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].settlementImpliedVol);


                        int lots = optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]]
                                    .numberOfLotsHeldForContractSummary
                                    * optionSpreadManager.portfolioGroupTotalMultiple;

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                    (int)CONTRACTSUMMARY_DATA_COLUMNS.TOTAL_QTY,
                                    lots.ToString(),
                                        false,
                                        lots);


                        double delta = Math.Round(
                            optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].deltaChgForContractSummary                                        
                                    * optionSpreadManager.portfolioGroupTotalMultiple,2);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.DELTA,
                                delta.ToString(),
                                        true,
                                        delta);


                        double plChg = Math.Round(
                            optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].plChgForContractSummary
                                    * optionSpreadManager.portfolioGroupTotalMultiple, 2);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.PL_DAY_CHG,
                                plChg.ToString(),
                                        true,
                                        plChg);


                        int numberOfOrderContracts = 
                            optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].numberOfOrderContracts
                                    * optionSpreadManager.portfolioGroupTotalMultiple;

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.ORDER_QTY,
                                numberOfOrderContracts.ToString(),
                                        false,
                                       numberOfOrderContracts);


                        double plChgOrders = Math.Round(
                            optionSpreadManager.optionSpreadExpressionList[optionSpreadManager.contractSummaryExpressionListIdx[contractSummaryExpressionCnt]].plChgOrders
                                    * optionSpreadManager.portfolioGroupTotalMultiple, 2);

                        fillContractSummaryLiveData(optionRealtimeMonitor, contractSummaryExpressionCnt,
                                (int)CONTRACTSUMMARY_DATA_COLUMNS.ORDER_PL_CHG,
                               plChgOrders.ToString(),
                                        true,
                                        plChgOrders);

                    }

                }

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        delegate void ThreadSafeFillLiveDataPageDelegate(
            OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value);

        public void fillContractSummaryLiveData(OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                if (optionRealtimeMonitor.InvokeRequired)
                {
                    ThreadSafeFillLiveDataPageDelegate d = new ThreadSafeFillLiveDataPageDelegate(threadSafeFillContractSummaryLiveData);

                    optionRealtimeMonitor.Invoke(d, optionRealtimeMonitor, row, col, displayValue, updateColor, value);
                }
                else
                {
                    threadSafeFillContractSummaryLiveData(optionRealtimeMonitor, row, col, displayValue, updateColor, value);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        public void threadSafeFillContractSummaryLiveData(
            OptionRealtimeMonitor optionRealtimeMonitor,
            int row, int col, String displayValue,
            bool updateColor, double value)
        {
            try
            {
                int rowToUpdate = row;

                if (optionRealtimeMonitor.getGridViewContractSummary.Rows[rowToUpdate].Cells[col].Value == null
                    ||
                    optionRealtimeMonitor.getGridViewContractSummary.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                    )
                {
                    optionRealtimeMonitor.getGridViewContractSummary.Rows[rowToUpdate].Cells[col].Value = displayValue;

                    if (updateColor)
                    {
                        optionRealtimeMonitor.getGridViewContractSummary.Rows[rowToUpdate].Cells[col].Style.BackColor
                            = CommonFormManipulation.plUpDownColor(value);
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
