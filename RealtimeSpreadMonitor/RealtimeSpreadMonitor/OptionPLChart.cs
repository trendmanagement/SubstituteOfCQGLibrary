using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using TML_XT_CommonInterfacesAndTypes;

namespace TMLCommonMethodsAndTypes.Option_Section
{
    public partial class OptionPLChart : Form
    {
        private Instrument instrument;
        //private LiveADMDataList liveADMDataList;
        //private Array optionSpreadContractTypesArray;
        private double futurePrice;

        private OptionArrayTypes optionArrayTypes;

        public OptionPLChart(OptionArrayTypes optionArrayTypes)
        {
            InitializeComponent();

            this.optionArrayTypes = optionArrayTypes;
        }

        public void fillGrid(//Instrument instrument, 
            LiveADMDataList liveADMDataList, Instrument instrument, double futurePrice)
        {

            //this.liveADMDataList = liveADMDataList;
            this.instrument = instrument;
            this.futurePrice = futurePrice;

            if (liveADMDataList.liveADMLegList.Count > 0)
            {



//                 Type optionPLColTypes = typeof(OPTION_PL_COLUMNS);
//                 Array optionPLColTypesArray = Enum.GetNames(optionPLColTypes);

                //Type optionSpreadContractTypes = typeof(OPTION_SPREAD_CONTRACT_TYPE);
                //optionSpreadContractTypesArray = Enum.GetNames(optionSpreadContractTypes);


                gridViewSpreadGrid.ColumnCount = optionArrayTypes.optionPLColTypesArray.Length - 1;

                DataGridViewComboBoxColumn myCboBox = new DataGridViewComboBoxColumn();

                for (int contractTypeCount = 0; contractTypeCount < optionArrayTypes.optionSpreadContractTypesArray.Length; contractTypeCount++)
                {
                    myCboBox.Items.Add(optionArrayTypes.optionSpreadContractTypesArray.GetValue(contractTypeCount).ToString());
                }

                gridViewSpreadGrid.Columns.Insert(0, myCboBox);

                gridViewSpreadGrid.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridViewSpreadGrid.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridViewSpreadGrid.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;

                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Width = 80;
                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.STRIKE].Width = 60;
                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.AVG_PRICE].Width = 60;
                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.NET].Width = 60;
                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Width = 60;
                gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Width = 60;
                //gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.RISK_FREE].Width = 60;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < optionArrayTypes.optionPLColTypesArray.Length; i++)
                {
                    sb.Clear();

                    sb.Append(optionArrayTypes.optionPLColTypesArray.GetValue(i).ToString());

                    gridViewSpreadGrid
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');
                }

                gridViewSpreadGrid.RowCount = liveADMDataList.liveADMLegList.Count;

                for (int legCount = 0; legCount < liveADMDataList.liveADMLegList.Count; legCount++)
                {
                    gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value =
                        optionArrayTypes.optionSpreadContractTypesArray.GetValue(liveADMDataList.liveADMLegList[legCount].callPutOrFuture).ToString();

                    //gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value =
                    //    liveADMDataList.liveADMLegList[legCount].strike;

                    gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value =
                        ConversionAndFormatting.convertToTickMovesString(
                        liveADMDataList.liveADMLegList[legCount].strike,
                            instrument.optionStrikeIncrement,
                            instrument.optionStrikeDisplay);

                    //ConversionAndFormatting.convertToTickMovesString(
                    //            avgPrice,
                    //            instrument.optionTickSize, instrument.optionTickDisplay);

                    double avgPrice = liveADMDataList.liveADMLegList[legCount].averagePrice;
                    //* instrument.admFuturePriceFactor;

                    if (liveADMDataList.liveADMLegList[legCount].callPutOrFuture ==
                        (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        //double decimals = Math.Pow(10,-(instrument.tickSize.ToString().Substring(
                        //instrument.tickSize.ToString().IndexOf(".") + 1).Length));

                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                        //liveADMDataList.liveADMLegList[legCount].averagePrice;

                        ConversionAndFormatting.convertToTickMovesString(
                                avgPrice,
                                instrument.tickSize, instrument.tickDisplay);
                    }
                    else
                    {
                        //double decimals = Math.Pow(10,-(instrument.optionTickSize.ToString().Substring(
                        //instrument.optionTickSize.ToString().IndexOf(".") + 1).Length));

                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                        //liveADMDataList.liveADMLegList[legCount].averagePrice;

                        ConversionAndFormatting.convertToTickMovesString(
                                avgPrice,
                                instrument.optionTickSize, instrument.optionTickDisplay);
                    }





                    //double1.ToString().Substring(double1.ToString().In dexOf(".")+1).Length

                    //gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                    //    liveADMDataList.liveADMLegList[legCount].averagePrice;

                    gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                        liveADMDataList.liveADMLegList[legCount].numberOfContracts;

                    gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Value = 0;

                    gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value = 0;

                    //gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.RISK_FREE].Value = 0;
                }
            }

        }

        public void fillGrid(//Instrument instrument, 
            OptionBuilderSpreadStructure optionBuilderSpreadStructure,
            Instrument instrument, double futurePrice)
        {
#if DEBUG
            try
#endif
            {

                //this.liveADMDataList = liveADMDataList;
                this.instrument = instrument;
                this.futurePrice = futurePrice;

                //if (liveADMDataList.liveADMLegList.Count > 0)
                {



                    //Type optionPLColTypes = typeof(OPTION_PL_COLUMNS);
                    //Array optionPLColTypesArray = Enum.GetNames(optionPLColTypes);

                    //Type optionSpreadContractTypes = typeof(OPTION_SPREAD_CONTRACT_TYPE);
                    //optionSpreadContractTypesArray = Enum.GetNames(optionSpreadContractTypes);


                    gridViewSpreadGrid.ColumnCount = optionArrayTypes.optionPLColTypesArray.Length - 1;

                    DataGridViewComboBoxColumn myCboBox = new DataGridViewComboBoxColumn();

                    for (int contractTypeCount = 0; contractTypeCount < optionArrayTypes.optionSpreadContractTypesArray.Length; contractTypeCount++)
                    {
                        myCboBox.Items.Add(optionArrayTypes.optionSpreadContractTypesArray.GetValue(contractTypeCount).ToString());
                    }

                    gridViewSpreadGrid.Columns.Insert(0, myCboBox);

                    gridViewSpreadGrid.EnableHeadersVisualStyles = false;

                    DataGridViewCellStyle colTotalPortStyle = gridViewSpreadGrid.ColumnHeadersDefaultCellStyle;
                    colTotalPortStyle.BackColor = Color.Black;
                    colTotalPortStyle.ForeColor = Color.White;

                    DataGridViewCellStyle rowTotalPortStyle = gridViewSpreadGrid.RowHeadersDefaultCellStyle;
                    rowTotalPortStyle.BackColor = Color.Black;
                    rowTotalPortStyle.ForeColor = Color.White;

                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Width = 80;
                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.STRIKE].Width = 60;
                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.AVG_PRICE].Width = 60;
                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.NET].Width = 60;
                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Width = 60;
                    gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Width = 60;
                    //gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.RISK_FREE].Width = 60;

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < optionArrayTypes.optionPLColTypesArray.Length; i++)
                    {
                        sb.Clear();

                        sb.Append(optionArrayTypes.optionPLColTypesArray.GetValue(i).ToString());

                        gridViewSpreadGrid
                            .Columns[i]
                            .HeaderCell.Value = sb.ToString().Replace('_', ' ');
                    }

                    gridViewSpreadGrid.RowCount = optionBuilderSpreadStructure.optionBuildLegStructureArray.Length;

                    for (int legCount = 0; legCount < optionBuilderSpreadStructure.optionBuildLegStructureArray.Length; legCount++)
                    {
                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value =
                            optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                        .optionParameters[(int)OPTION_CONFIG_LIST.OPT_CALL_PUT_OR_FUT]).ToString();

                        //gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value =
                        //    liveADMDataList.liveADMLegList[legCount].strike;



                        //ConversionAndFormatting.convertToTickMovesString(
                        //            avgPrice,
                        //            instrument.optionTickSize, instrument.optionTickDisplay);


                        //* instrument.admFuturePriceFactor;

                        if ((int)optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                        .optionParameters[(int)OPTION_CONFIG_LIST.OPT_CALL_PUT_OR_FUT] ==
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                        {
                            //double decimals = Math.Pow(10,-(instrument.tickSize.ToString().Substring(
                            //instrument.tickSize.ToString().IndexOf(".") + 1).Length));
                            //bool notFound = true;
                            int tempLegCounter = 0;

                            while (tempLegCounter < optionBuilderSpreadStructure.optionBuildLegStructureArray.Length)
                            {
                                if (
                                optionBuilderSpreadStructure.optionBuildLegStructureArray[tempLegCounter]
                                    .optionBuildLegDataAtRollDateList[1].optionSettlementDataArray.Length > 0)
                                {
                                    break;
                                }

                                tempLegCounter++;
                            }

                            if (tempLegCounter < optionBuilderSpreadStructure.optionBuildLegStructureArray.Length)
                            {
                                double avgPrice = optionBuilderSpreadStructure.optionBuildLegStructureArray[tempLegCounter]
                                    .optionBuildLegDataAtRollDateList[1].optionSettlementDataArray.Last().underlyingFuturesContractSettlement;

                                gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                                ConversionAndFormatting.convertToTickMovesString(
                                        avgPrice,
                                        instrument.tickSize, instrument.tickDisplay);
                            }
                            else
                            {
                                double avgPrice = optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                    .optionBuildLegDataAtRollDateList[1].optionBuildContractLegDataPoint.Last().underlyingFutureClose;

                                gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                                ConversionAndFormatting.convertToTickMovesString(
                                        avgPrice,
                                        instrument.tickSize, instrument.tickDisplay);
                            }
                        }
                        else
                        {
                            //double decimals = Math.Pow(10,-(instrument.optionTickSize.ToString().Substring(
                            //instrument.optionTickSize.ToString().IndexOf(".") + 1).Length));

                            gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value =

                            ConversionAndFormatting.convertToTickMovesString(
                                //optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                //    .optionBuildLegDataAtRollDateList.Last().optionStrikePrice,

                                 optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                .optionBuildLegDataAtRollDateList[1]
                                .optionSettlementDataArray.Last().strikePrice,

                                instrument.optionStrikeIncrement,
                                instrument.optionStrikeDisplay);

                            double avgPrice = optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                .optionBuildLegDataAtRollDateList[1].optionSettlementDataArray.Last().optionSettlementPrice;

                            gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value =

                            //liveADMDataList.liveADMLegList[legCount].averagePrice;

                            ConversionAndFormatting.convertToTickMovesString(
                                    avgPrice,
                                    instrument.optionTickSize, instrument.optionTickDisplay);
                        }

                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                            optionBuilderSpreadStructure.optionBuildLegStructureArray[legCount]
                                        .optionParameters[(int)OPTION_CONFIG_LIST.OPT_CONTRACTS];

                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Value = 0;

                        gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value = 0;
                    }
                }

                fillChart();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif

        }

        public void fillChart()
        {
#if DEBUG
            try
#endif
            {
                Series serPrice = new Series();

                serPrice.ChartType = SeriesChartType.Line;

                serPrice.Name = "Option PL";

                //gridViewSpreadGrid.RowCount = liveADMDataList.liveADMLegList.Count;

                int legCount = 0;

                int futureIdx = -1;

                while (legCount < gridViewSpreadGrid.RowCount)
                {
                    String contractType = gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value.ToString();

                    if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                    {
                        futureIdx = legCount;
                        break;
                    }

                    legCount++;
                }

                int countOftest = 1000;

                double futureAvgPrice;

                if (futureIdx >= 0)
                {
                    futureAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[futureIdx]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value.ToString(),
                                    instrument.tickSize, instrument.tickDisplay);
                }
                else
                {
                    futureAvgPrice = futurePrice;
                }


                double[] futurePriceArray = new double[countOftest * 2 + 1];

                //double futureAvgPrice = (double)gridViewSpreadGrid.Rows[futureIdx]
                //                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value;

                //double futureAvgPrice = (double)gridViewSpreadGrid.Rows[futureIdx]
                //                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value;
                //double futureAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                //                gridViewSpreadGrid.Rows[futureIdx]
                //                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value.ToString(),
                //                instrument.tickSize, instrument.tickDisplay);

                //futureAvgPrice /= instrument.admFuturePriceFactor;

                double startOfTest =
                    futureAvgPrice
                    - countOftest *
                    instrument.tickSize;
                //optionBuilderSpreadStructure[
                //    currentDateContractListMainIdx[aDMDataListSpreadCount][(int)OPTION_SPREAD_ROLL_INDEXES.OPTION_SPREAD_IDX]
                //    ].instrument.tickSize;

                if (startOfTest < 0)
                {
                    startOfTest = 0;
                }

                for (int count = 0; count < futurePriceArray.Length; count++)
                {
                    futurePriceArray[count] = startOfTest +
                        count *
                        instrument.tickSize;
                    //optionBuilderSpreadStructure[
                    //currentDateContractListMainIdx[aDMDataListSpreadCount][(int)OPTION_SPREAD_ROLL_INDEXES.OPTION_SPREAD_IDX]
                    //].instrument.tickSize;
                }


                double[,] legPls = new double[gridViewSpreadGrid.RowCount, futurePriceArray.Length];

                double[] spreadPl = new double[futurePriceArray.Length];

                double riskFreeRate = Convert.ToDouble(riskFreeTextBox.Text) / 100;

                for (int futurePointCount = 0; futurePointCount < futurePriceArray.Length; futurePointCount++)
                {
                    double plTotal = 0;

                    legCount = 0;

                    //for (int legCounter = 0; legCounter < gridViewSpreadGrid.RowCount   ;
                    //    legCounter++)
                    while (legCount < gridViewSpreadGrid.RowCount)
                    {
                        int cellCount = 0;
                        bool continueThisLeg = true;
                        while (cellCount < gridViewSpreadGrid.Rows[legCount].Cells.Count)
                        {
                            if (gridViewSpreadGrid.Rows[legCount].Cells[cellCount].Value == null)
                            {
                                continueThisLeg = false;
                                break;
                            }

                            cellCount++;
                        }

                        if (continueThisLeg)
                        {
                            String contractType = gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value.ToString();

                            //double strike = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                            //                        .Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);

                            double strike =
                            ConversionAndFormatting.convertToTickMovesDouble(
                            gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value.ToString(),
                            instrument.optionStrikeIncrement,
                            instrument.optionStrikeDisplay);

                            double daysToExp = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                    .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value);
                            daysToExp /= 365;

                            double implVol = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                    .Cells[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Value);
                            //implVol = 15;
                            implVol /= 100;

                            double numOfContracts = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                    .Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                            double avgPrice = 0;  // Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                            //   .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value);

                            if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                            {
                                avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value.ToString(),
                                    instrument.tickSize, instrument.tickDisplay);
                            }
                            else
                            {
                                if (instrument.secondaryOptionTickSizeRule > 0)
                                {
                                    avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                        gridViewSpreadGrid.Rows[legCount]
                                            .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value.ToString(),
                                        instrument.secondaryOptionTickSize, instrument.optionTickDisplay);
                                }
                                else
                                {
                                    avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                        gridViewSpreadGrid.Rows[legCount]
                                            .Cells[(int)OPTION_PL_COLUMNS.AVG_PRICE].Value.ToString(),
                                        instrument.optionTickSize, instrument.optionTickDisplay);
                                }
                            }

                            char typeSymbol;
                            bool run = true;
                            double price;
                            double tickSize;
                            double tickValue;

                            if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                    (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                            {
                                typeSymbol = 'C';

                                price = OptionCalcs.blackScholes(typeSymbol,
                                    futurePriceArray[futurePointCount],
                                            strike,
                                             daysToExp, riskFreeRate,
                                            implVol);

                                tickSize = instrument.optionTickSize;
                                tickValue = instrument.optionTickValue;

                                //run = false;
                            }
                            else if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                    (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
                            {
                                typeSymbol = 'P';

                                price = OptionCalcs.blackScholes(typeSymbol,
                                    futurePriceArray[futurePointCount],
                                            strike,
                                             daysToExp, riskFreeRate,
                                            implVol);

                                tickSize = instrument.optionTickSize;
                                tickValue = instrument.optionTickValue;

                                //run = false;
                            }
                            else
                            {
                                typeSymbol = 'F';

                                price = futurePriceArray[futurePointCount];

                                tickSize = instrument.tickSize;
                                tickValue = instrument.tickValue;

                                //run = true;
                            }

                            if (run)
                            {
                                double pl =
                                        numOfContracts *
                                        (price - avgPrice)
                                            / tickSize
                                            * tickValue;

                                plTotal += pl;

                                legPls[legCount, futurePointCount] = pl;
                            }
                        }
                        legCount++;
                    }

                    spreadPl[futurePointCount] = plTotal;

                    DataPoint dp = new DataPoint();

                    dp.SetValueXY(
                        ConversionAndFormatting.convertToTickMovesString(
                                futurePriceArray[futurePointCount],
                                instrument.tickSize, instrument.tickDisplay),
                        //Math.Round(futurePriceArray[futurePointCount], 2),
                        plTotal);
                    //dp.SetValueY(plTotal);

                    serPrice.Points.Add(dp);

                }

                chart1.Series.Clear();
                chart1.ResetAutoValues();

                chart1.Series.Add(serPrice);

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }


        private void btnChart_Click(object sender, EventArgs e)
        {
            fillChart();
        }

        private void btnVolFill_Click(object sender, EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                int legCount = 0;

                double vol = 0;

                if (volTextBox.Text != null)
                {
                    vol = Convert.ToDouble(volTextBox.Text);

                    while (legCount < gridViewSpreadGrid.RowCount)
                    {
                        if (gridViewSpreadGrid.Rows[legCount]
                            .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value != null)
                        {
                            gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Value
                                = vol;
                        }

                        legCount++;
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void btnDaysFill_Click(object sender, EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                int legCount = 0;

                double days = 0;

                if (daysTextBox.Text != null)
                {
                    days = Convert.ToDouble(daysTextBox.Text);

                    while (legCount < gridViewSpreadGrid.RowCount)
                    {
                        if (gridViewSpreadGrid.Rows[legCount]
                            .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value != null)
                        {
                            gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value
                                = days;
                        }

                        legCount++;
                    }
                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void gridViewSpreadGrid_MouseClick(object sender, MouseEventArgs e)
        {
#if DEBUG
            try
#endif
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    optionPLChartDataMenuStrip.Show(gridViewSpreadGrid, new Point(e.X, e.Y));
                    //optionConfigMenuStrip.Show(optionConfigSetupGrid, new Point(e.X, e.Y));

                }
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void addRowMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                gridViewSpreadGrid.Rows.Add();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        private void deleteRowMenuItem_Click(object sender, EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                if (gridViewSpreadGrid.SelectedCells.Count > 0)
                {
                    int selectedRow = gridViewSpreadGrid.SelectedCells[0].RowIndex;
                    gridViewSpreadGrid.Rows.RemoveAt(selectedRow);
                }

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

    }
}
