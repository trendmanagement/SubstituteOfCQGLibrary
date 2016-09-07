using RealtimeSpreadMonitor.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealtimeSpreadMonitor.FormManipulation
{
    internal class ModelADMCompareCalculationAndDisplay
    {
        internal ModelADMCompareCalculationAndDisplay(
            //OptionRealtimeMonitor optionRealtimeMonitor,
            //DataGridView gridViewContractSummary,
            OptionSpreadManager optionSpreadManager)
        {
            //this.optionRealtimeMonitor = optionRealtimeMonitor;
            //this.gridViewContractSummary = gridViewContractSummary;
            this.optionSpreadManager = optionSpreadManager;

        }

        private OptionSpreadManager optionSpreadManager;// { get; set; }


        public void setupGridModelADMComparison(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridViewModelADMCompare = optionRealtimeMonitor.getGridViewModelADMCompare;

            try
            {

                Type liveColTypes = typeof(ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED);
                Array liveColTypesArray = Enum.GetNames(liveColTypes);

                gridViewModelADMCompare.ColumnCount = liveColTypesArray.Length;

                gridViewModelADMCompare.EnableHeadersVisualStyles = false;

                DataGridViewCellStyle colTotalPortStyle = gridViewModelADMCompare.ColumnHeadersDefaultCellStyle;
                colTotalPortStyle.BackColor = Color.Black;
                colTotalPortStyle.ForeColor = Color.White;

                DataGridViewCellStyle rowTotalPortStyle = gridViewModelADMCompare.RowHeadersDefaultCellStyle;
                rowTotalPortStyle.BackColor = Color.Black;
                rowTotalPortStyle.ForeColor = Color.White;

                gridViewModelADMCompare.Columns[0].Frozen = true;

                DataGridViewCheckBoxColumn zeroPriceCol = new DataGridViewCheckBoxColumn();
                {
                    zeroPriceCol.HeaderText = liveColTypesArray.GetValue(
                        (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE).ToString().Replace('_', ' ');
                    //column.Name = ColumnName.OutOfOffice.ToString();
                    zeroPriceCol.AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.DisplayedCells;
                    zeroPriceCol.FlatStyle = FlatStyle.Standard;
                    //column.ThreeState = true;
                    zeroPriceCol.CellTemplate = new DataGridViewCheckBoxCell();
                    zeroPriceCol.CellTemplate.Style.BackColor = Color.LightBlue;
                    zeroPriceCol.ReadOnly = false;
                    //column.e = false;
                }

                DataGridViewCheckBoxColumn exceptCol = new DataGridViewCheckBoxColumn();
                {
                    exceptCol.HeaderText = liveColTypesArray.GetValue(
                        (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS).ToString().Replace('_', ' ');
                    //column.Name = ColumnName.OutOfOffice.ToString();
                    exceptCol.AutoSizeMode =
                        DataGridViewAutoSizeColumnMode.DisplayedCells;
                    exceptCol.FlatStyle = FlatStyle.Standard;
                    //column.ThreeState = true;
                    exceptCol.CellTemplate = new DataGridViewCheckBoxCell();
                    exceptCol.CellTemplate.Style.BackColor = Color.Cyan;
                    exceptCol.ReadOnly = false;
                    //column.e = false;
                }



                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < gridViewModelADMCompare.ColumnCount; i++)
                {
                    gridViewModelADMCompare.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                    //if (i != (int)OPTION_LIVE_ADM_DATA_COLUMNS.NET && i != (int)OPTION_LIVE_ADM_DATA_COLUMNS.AVERAGE_PRC)
                    //{
                    //    gridViewModelADMCompare.Columns[i].ReadOnly = true;
                    //}

                    sb.Clear();

                    sb.Append(liveColTypesArray.GetValue(i).ToString());

                    gridViewModelADMCompare
                        .Columns[i]
                        .HeaderCell.Value = sb.ToString().Replace('_', ' ');

                    gridViewModelADMCompare.Columns[i].Width = 50;

                    if (i == (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE)
                    {
                        gridViewModelADMCompare.Columns.RemoveAt(i);
                        gridViewModelADMCompare.Columns.Insert(i, zeroPriceCol);
                    }
                    else if (i == (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS)
                    {
                        gridViewModelADMCompare.Columns.RemoveAt(i);
                        gridViewModelADMCompare.Columns.Insert(i, exceptCol);
                    }
                    else
                    {
                        gridViewModelADMCompare.Columns[i].ReadOnly = true;
                    }
                }

                gridViewModelADMCompare.Columns[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE].Width = 75;




            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        //private void updateModelADMCompareViewable()
        //{
        //    if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instruments.Length)
        //    {
        //        for (int modelADMCompareCntRow = 0;
        //                        modelADMCompareCntRow < gridViewModelADMCompare.RowCount; modelADMCompareCntRow++)
        //        {
        //            hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, true);
        //        }
        //    }
        //    else
        //    {
        //        for (int modelADMCompareCntRow = 0;
        //                modelADMCompareCntRow < gridViewModelADMCompare.RowCount; modelADMCompareCntRow++)
        //        {
        //            int instrumentId = Convert.ToInt16(gridViewModelADMCompare.Rows[modelADMCompareCntRow].Cells[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.INSTRUMENT_ID].Value);

        //            if (optionSpreadManager.contractSummaryInstrumentSelectedIdx == instrumentId)
        //            {
        //                hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, true);
        //            }
        //            else
        //            {
        //                hideUnhideSummaryData(gridViewModelADMCompare, modelADMCompareCntRow, false);
        //            }
        //        }
        //    }
        //}


        delegate void ThreadSafeFillGridModelADMComparison(OptionRealtimeMonitor optionRealtimeMonitor);

        public void fillGridModelADMComparison(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            if (optionRealtimeMonitor.InvokeRequired)
            {

                ThreadSafeFillGridModelADMComparison d = new ThreadSafeFillGridModelADMComparison(threadSafeFillGridModelADMComparison);

                optionRealtimeMonitor.Invoke(d, optionRealtimeMonitor);
            }
            else
            {
                threadSafeFillGridModelADMComparison(optionRealtimeMonitor);
            }

        }

        private string makeDictionaryKeyOfFCMPositionCompare(int acctGroup, string cqgSymbol)
        {
            StringBuilder dictionaryKey = new StringBuilder();
            dictionaryKey.Append(acctGroup);
            dictionaryKey.Append(":");
            dictionaryKey.Append(cqgSymbol);

            return dictionaryKey.ToString();
        }

        public void threadSafeFillGridModelADMComparison(OptionRealtimeMonitor optionRealtimeMonitor)
        {

            DataGridView gridViewModelADMCompare = optionRealtimeMonitor.getGridViewModelADMCompare;

            try
            {
                //List<LiveADMStrategyInfo> liveADMStrategyInfoList = optionSpreadManager.liveADMStrategyInfoList;

                optionSpreadManager.admPositionImportWebListForCompare.Clear();// = new List<ADMPositionImportWeb>();

                Dictionary<string, ADMPositionImportWeb> modelContractAcctGrpFCMCompareDictionary = new Dictionary<string, ADMPositionImportWeb>();

                //****************
                for (int webPos = 0; webPos < optionSpreadManager.admPositionImportWeb.Count; webPos++)
                {
                    ADMPositionImportWeb aDMPositionImportWeb = new ADMPositionImportWeb();

                    optionSpreadManager.admPositionImportWebListForCompare.Add(aDMPositionImportWeb);

                    optionSpreadManager.aDMDataCommonMethods.copyADMPositionImportWeb(
                        aDMPositionImportWeb, optionSpreadManager.admPositionImportWeb[webPos]);

                    string key = makeDictionaryKeyOfFCMPositionCompare(aDMPositionImportWeb.acctGroup,
                        aDMPositionImportWeb.cqgSymbol);

                    if (!modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                    {
                        modelContractAcctGrpFCMCompareDictionary.Add(key, aDMPositionImportWeb);
                    }

                    //StringBuilder pOfficPAcct = new StringBuilder();
                    //pOfficPAcct.Append(aDMPositionImportWeb.POFFIC);
                    //pOfficPAcct.Append(aDMPositionImportWeb.PACCT);

                    //if (optionSpreadManager.portfolioGroupIdxAcctStringHashSet.ContainsKey(pOfficPAcct.ToString()))
                    //{
                    //    aDMPositionImportWeb.acctGroup = optionSpreadManager.portfolioGroupIdxAcctStringHashSet[pOfficPAcct.ToString()];
                    //    aDMPositionImportWeb.MODEL_OFFICE_ACCT = optionSpreadManager.portfolioGroupAllocation[aDMPositionImportWeb.acctGroup].FCM_POFFIC_PACCT;

                    //}
                }
                //****************


                HashSet<string> modelContractsAlreadyExamined = new HashSet<string>();
                

                //for (int portfolioGroupCnt = 0; portfolioGroupCnt < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {

                    for (int webPos = 0; webPos < optionSpreadManager.admPositionImportWebListForCompare.Count; webPos++)
                    {
                        ADMPositionImportWeb aDMPositionImportWeb = optionSpreadManager.admPositionImportWebListForCompare[webPos];

                        //optionSpreadManager.admPositionImportWebListForCompare.Add(aDMPositionImportWeb);

                        //optionSpreadManager.aDMDataCommonMethods.copyADMPositionImportWeb(
                        //    aDMPositionImportWeb, optionSpreadManager.admPositionImportWeb[webPos]);

                        for (int portfolioGroupCnt = 0; portfolioGroupCnt < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCnt++)
                        {
                            int modelContractsCnt = 0;
                            while (modelContractsCnt < optionSpreadManager.contractSummaryExpressionListIdx.Count)
                            {
                                StringBuilder modelContractAcctGroupIdentifier = new StringBuilder();
                                modelContractAcctGroupIdentifier.Append(portfolioGroupCnt);
                                modelContractAcctGroupIdentifier.Append(":");
                                modelContractAcctGroupIdentifier.Append(optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]);
                                //modelContractAcctGroupIdentifier.Append(":");
                                //modelContractAcctGroupIdentifier.Append(webPos);

                                StringBuilder pOfficPAcct = new StringBuilder();
                                pOfficPAcct.Append(aDMPositionImportWeb.POFFIC);
                                pOfficPAcct.Append(aDMPositionImportWeb.PACCT);

                                if (!modelContractsAlreadyExamined.Contains(modelContractAcctGroupIdentifier.ToString()))
                                {
                                    if (optionSpreadManager.optionSpreadExpressionList[
                                            optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].cqgSymbol
                                                .CompareTo(aDMPositionImportWeb.cqgSymbol) == 0

                                        && optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].FCM_POFFIC_PACCT_hashset
                                                .ContainsKey(pOfficPAcct.ToString()))
                                    {
                                        //if (!modelContractsAlreadyExamined.Contains(modelContractAcctGroupIdentifier.ToString()))
                                        {
                                            modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());
                                        }                                       


                                        aDMPositionImportWeb.MODEL_OFFICE_ACCT = optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].FCM_POFFIC_PACCT;



                                        aDMPositionImportWeb.modelLots =
                                            optionSpreadManager.optionSpreadExpressionList[
                                                optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]]
                                                    .numberOfLotsHeldForContractSummary
                                                     * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                        aDMPositionImportWeb.optionSpreadExpression =
                                            optionSpreadManager.optionSpreadExpressionList[
                                                optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]];

                                        //aDMPositionImportWeb.rebalanceLots = 
                                        //    aDMPositionImportWeb.modelLots - aDMPositionImportWeb.Net;

                                        break;
                                    }


                                }

                                modelContractsCnt++;
                            }
                        }
                    }

                }


                for (int portfolioGroupCnt = 0; portfolioGroupCnt < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {
                    for (int modelContractsCnt = 0; modelContractsCnt < optionSpreadManager.contractSummaryExpressionListIdx.Count; modelContractsCnt++)
                    {
                        //TSErrorCatch.debugWriteOut("*** " + optionSpreadManager.optionSpreadExpressionList[
                        //        optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].cqgSymbol + " " +
                        //        optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]);

                        StringBuilder modelContractAcctGroupIdentifier = new StringBuilder();
                        modelContractAcctGroupIdentifier.Append(portfolioGroupCnt);
                        modelContractAcctGroupIdentifier.Append(":");
                        modelContractAcctGroupIdentifier.Append(optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]);

                        if (!modelContractsAlreadyExamined.Contains(modelContractAcctGroupIdentifier.ToString()))
                        {
                            ADMPositionImportWeb aDMPositionImportWeb = new ADMPositionImportWeb();

                            optionSpreadManager.admPositionImportWebListForCompare.Add(aDMPositionImportWeb);

                            

                            modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());

                            aDMPositionImportWeb.acctGroup = portfolioGroupCnt;

                            aDMPositionImportWeb.MODEL_OFFICE_ACCT = optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].FCM_POFFIC_PACCT;

                            aDMPositionImportWeb.cqgSymbol =
                                optionSpreadManager.optionSpreadExpressionList[
                                optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].cqgSymbol;

                            aDMPositionImportWeb.modelLots =
                                optionSpreadManager.optionSpreadExpressionList[
                                    optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]]
                                        .numberOfLotsHeldForContractSummary
                                         * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                            aDMPositionImportWeb.optionSpreadExpression =
                                        optionSpreadManager.optionSpreadExpressionList[
                                            optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]];

                            //aDMPositionImportWeb.rebalanceLots =
                            //    aDMPositionImportWeb.modelLots - aDMPositionImportWeb.Net;

                            aDMPositionImportWeb.strike =
                                //optionSpreadManager.optionSpreadExpressionList[
                                //    optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]]
                                //        .strikePrice.ToString();
                                ConversionAndFormatting.convertToTickMovesString(
                                    optionSpreadManager.optionSpreadExpressionList[
                                        optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]]
                                        .strikePrice,
                                        optionSpreadManager.optionSpreadExpressionList[
                                        optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].instrument.optionStrikeIncrement,
                                        optionSpreadManager.optionSpreadExpressionList[
                                        optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].instrument.optionStrikeDisplay);

                            //NOV 11 2014 FIXED FORMAT OF STRIKE

                            //aDMPositionImportWeb.strikeInDecimal = optionSpreadManager.optionSpreadExpressionList[
                            //            optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]]
                            //            .strikePrice;


                            aDMPositionImportWeb.instrumentArrayIdx =
                                optionSpreadManager.optionSpreadExpressionList[
                                    optionSpreadManager.contractSummaryExpressionListIdx[modelContractsCnt]].instrument.idxOfInstrumentInList;


                            string key = makeDictionaryKeyOfFCMPositionCompare(aDMPositionImportWeb.acctGroup,
                                aDMPositionImportWeb.cqgSymbol);

                            if (!modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                            {
                                modelContractAcctGrpFCMCompareDictionary.Add(key, aDMPositionImportWeb);
                            }

                        }

                        //modelContractsCnt++;
                    }
                }


                //modelContractsAlreadyExamined.Clear();

                for (int portfolioGroupCnt = 0; portfolioGroupCnt < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {

                    for (int cntExpressionList = 0; cntExpressionList < optionSpreadManager.optionSpreadExpressionList.Count; cntExpressionList++)
                    {


                        if (optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts != 0)
                        {
                            string key = makeDictionaryKeyOfFCMPositionCompare(portfolioGroupCnt,
                                optionSpreadManager.optionSpreadExpressionList[cntExpressionList].cqgSymbol);

                            bool foundContract = false;

                            if (modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                            {
                                ADMPositionImportWeb admPIW = modelContractAcctGrpFCMCompareDictionary[key];





                                admPIW.orderLots =
                                            optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts
                                             * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                //TSErrorCatch.debugWriteOut(
                                //    portfolioGroupCnt
                                //    + "," +
                                //    cntExpressionList
                                //    + "," +
                                //    optionSpreadManager.admPositionImportWebListForCompare[webPos].Description
                                //    + ", orderLots ," +
                                //    optionSpreadManager.admPositionImportWebListForCompare[webPos].orderLots
                                //    + ", noOrderContracts, " +
                                //    optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts
                                //    + ", multiple, " +
                                //    optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple
                                //    + ", webPos, " +
                                //    webPos
                                //    );

                                foundContract = true;
                            }

                            //while (webPos < optionSpreadManager.admPositionImportWebListForCompare.Count)
                            //{
                            //    StringBuilder modelContractAcctGroupIdentifier = new StringBuilder();
                            //    //modelContractAcctGroupIdentifier.Append(portfolioGroupCnt);
                            //    //modelContractAcctGroupIdentifier.Append(":");
                            //    modelContractAcctGroupIdentifier.Append(cntExpressionList);
                            //    modelContractAcctGroupIdentifier.Append(":");
                            //    modelContractAcctGroupIdentifier.Append(webPos);

                            //    if (!modelContractsAlreadyExamined.Contains(modelContractAcctGroupIdentifier.ToString()))
                            //    {
                            //        //modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());

                            //        if (optionSpreadManager.optionSpreadExpressionList[cntExpressionList].cqgSymbol
                            //                        .CompareTo(optionSpreadManager.admPositionImportWebListForCompare[webPos].cqgSymbol) == 0
                            //            &&
                            //            optionSpreadManager.admPositionImportWebListForCompare[webPos].acctGroup == portfolioGroupCnt
                            //            )
                            //        {
                            //            modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());

                            //            optionSpreadManager.admPositionImportWebListForCompare[webPos].orderLots =
                            //                optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts
                            //                 * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                            //            TSErrorCatch.debugWriteOut(
                            //                portfolioGroupCnt
                            //                + "," +
                            //                cntExpressionList
                            //                + "," +
                            //                optionSpreadManager.admPositionImportWebListForCompare[webPos].Description
                            //                + ", orderLots ," +
                            //                optionSpreadManager.admPositionImportWebListForCompare[webPos].orderLots
                            //                + ", noOrderContracts, " +
                            //                optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts
                            //                + ", multiple, " +
                            //                optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple
                            //                + ", webPos, " +    
                            //                webPos
                            //                );

                            //            foundContract = true;

                            //            break;
                            //        }
                            //    }

                            //    webPos++;
                            //}

                            if (!foundContract)
                            {
                                ADMPositionImportWeb aDMPositionImportWeb = new ADMPositionImportWeb();

                                optionSpreadManager.admPositionImportWebListForCompare.Add(aDMPositionImportWeb);

                                if (!modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                                {
                                    modelContractAcctGrpFCMCompareDictionary.Add(key, aDMPositionImportWeb);
                                }


                                aDMPositionImportWeb.acctGroup = portfolioGroupCnt;

                                aDMPositionImportWeb.MODEL_OFFICE_ACCT = optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].FCM_POFFIC_PACCT;

                                aDMPositionImportWeb.cqgSymbol =
                                    optionSpreadManager.optionSpreadExpressionList[cntExpressionList].cqgSymbol;

                                aDMPositionImportWeb.orderLots =
                                    optionSpreadManager.optionSpreadExpressionList[cntExpressionList].numberOfOrderContracts
                                     * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                aDMPositionImportWeb.optionSpreadExpression =
                                        optionSpreadManager.optionSpreadExpressionList[cntExpressionList];

                                //aDMPositionImportWeb.rebalanceLots =
                                //    aDMPositionImportWeb.modelLots - aDMPositionImportWeb.Net;

                                aDMPositionImportWeb.strike =
                                    //optionSpreadManager.optionSpreadExpressionList[cntExpressionList].strikePrice.ToString();
                                    ConversionAndFormatting.convertToTickMovesString(
                                        optionSpreadManager.optionSpreadExpressionList[cntExpressionList].strikePrice,
                                            optionSpreadManager.optionSpreadExpressionList[cntExpressionList].instrument.optionStrikeIncrement,
                                            optionSpreadManager.optionSpreadExpressionList[cntExpressionList].instrument.optionStrikeDisplay);
                                //NOV 11 2014 CHG FORMAT STRIKE

                                //aDMPositionImportWeb.strikeInDecimal = optionSpreadManager.optionSpreadExpressionList[cntExpressionList].strikePrice;

                                //aDMPositionImportWeb.contractYear = optionSpreadManager.optionSpreadExpressionList[cntExpressionList].futureContractYear;
                                //aDMPositionImportWeb.contractMonth = aDMPositionImportWeb.contractInfo.contractMonthInt;
                                //aDMPositionImportWeb.optionYear = aDMPositionImportWeb.contractInfo.optionYear;
                                //aDMPositionImportWeb.optionMonth = aDMPositionImportWeb.contractInfo.optionMonthInt;

                                aDMPositionImportWeb.instrumentArrayIdx =
                                    optionSpreadManager.optionSpreadExpressionList[cntExpressionList].instrument.idxOfInstrumentInList;

                                

                                
                            }
                        }
                    }
                }


                modelContractsAlreadyExamined.Clear();

                for (int portfolioGroupCnt = 0; portfolioGroupCnt < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCnt++)
                {

                    for (int i = 0; i < optionSpreadManager.instruments.Length; i++)
                    {
                        //symbolsToRemove.Clear();

                        foreach (ContractList rollIntoContracts in optionSpreadManager.instrumentRollIntoSummary[i].contractHashTable.Values)
                        {
                            //if (!rollIntoContracts.currentlyRollingContract)
                            //{
                            //    symbolsToRemove.Add(rollIntoContracts.cqgSymbol);
                            //}

                            if (rollIntoContracts.currentlyRollingContract
                                && rollIntoContracts.numberOfContracts != 0)
                            {

                                //********
                                string key = makeDictionaryKeyOfFCMPositionCompare(portfolioGroupCnt,
                                    rollIntoContracts.cqgSymbol);

                                bool foundContract = false;

                                if (modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                                {
                                    ADMPositionImportWeb admPIW = modelContractAcctGrpFCMCompareDictionary[key];

                                    admPIW.rollIntoLots =
                                        rollIntoContracts.numberOfContracts
                                                * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                    foundContract = true;
                                }

                                //********


                                //while (webPos < optionSpreadManager.admPositionImportWebListForCompare.Count)
                                //{
                                //    StringBuilder modelContractAcctGroupIdentifier = new StringBuilder();
                                //    modelContractAcctGroupIdentifier.Append(portfolioGroupCnt);
                                //    modelContractAcctGroupIdentifier.Append(":");
                                //    modelContractAcctGroupIdentifier.Append(rollIntoContracts.cqgSymbol);

                                //    if (!modelContractsAlreadyExamined.Contains(modelContractAcctGroupIdentifier.ToString()))
                                //    {
                                //        //if (rollIntoContracts.cqgSymbol
                                //        //                .CompareTo(optionSpreadManager.admPositionImportWebListForCompare[webPos].cqgSymbol) == 0)

                                //        if (rollIntoContracts.indexOfInstrumentInInstrumentsArray == i)
                                //        {
                                //            if (rollIntoContracts.contractType == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                //            {
                                //                //if (rollIntoContracts.contractMonthInt
                                //                //        == optionSpreadManager.admPositionImportWebListForCompare[webPos].contractInfo.contractMonthInt
                                //                //    &&
                                //                //    rollIntoContracts.contractYear
                                //                //        == optionSpreadManager.admPositionImportWebListForCompare[webPos].contractInfo.contractYear)

                                //                if (rollIntoContracts.cqgSymbol.CompareTo(
                                //                    optionSpreadManager.admPositionImportWebListForCompare[webPos].cqgSymbol) == 0)
                                //                {
                                //                    modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());

                                //                    optionSpreadManager.admPositionImportWebListForCompare[webPos].rollIntoLots =
                                //                        rollIntoContracts.numberOfContracts 
                                //                        * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                //                    foundContract = true;

                                //                    break;
                                //                }


                                //            }
                                //            else
                                //            {

                                //                if (rollIntoContracts.cqgSymbol.CompareTo(
                                //                    optionSpreadManager.admPositionImportWebListForCompare[webPos].cqgSymbol) == 0)
                                //                {
                                //                    modelContractsAlreadyExamined.Add(modelContractAcctGroupIdentifier.ToString());

                                //                    optionSpreadManager.admPositionImportWebListForCompare[webPos].rollIntoLots =
                                //                        rollIntoContracts.numberOfContracts 
                                //                        * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                //                    foundContract = true;

                                //                    break;
                                //                }
                                //            }
                                //        }
                                //    }

                                //    webPos++;
                                //}

                                if (!foundContract)
                                {
                                    ADMPositionImportWeb aDMPositionImportWeb = new ADMPositionImportWeb();

                                    optionSpreadManager.admPositionImportWebListForCompare.Add(aDMPositionImportWeb);


                                    if (!modelContractAcctGrpFCMCompareDictionary.ContainsKey(key))
                                    {
                                        modelContractAcctGrpFCMCompareDictionary.Add(key, aDMPositionImportWeb);
                                    }


                                    aDMPositionImportWeb.acctGroup = portfolioGroupCnt;

                                    aDMPositionImportWeb.MODEL_OFFICE_ACCT = optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].FCM_POFFIC_PACCT;

                                    //aDMPositionImportWeb.acctGroup = portfolioGroupCnt;

                                    aDMPositionImportWeb.callPutOrFuture = rollIntoContracts.contractType;

                                    aDMPositionImportWeb.cqgSymbol =
                                        rollIntoContracts.cqgSymbol;

                                    aDMPositionImportWeb.rollIntoLots =
                                        rollIntoContracts.numberOfContracts 
                                        * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCnt].multiple;

                                    //aDMPositionImportWeb.rebalanceLots =
                                    //    aDMPositionImportWeb.modelLots - aDMPositionImportWeb.Net;

                                    aDMPositionImportWeb.strike =
                                        ConversionAndFormatting.convertToTickMovesString(rollIntoContracts.strikePrice,
                                            optionSpreadManager.instruments[i].optionStrikeIncrement,
                                            optionSpreadManager.instruments[i].optionStrikeDisplay);

                                    aDMPositionImportWeb.strikeInDecimal = rollIntoContracts.strikePrice;

                                    aDMPositionImportWeb.contractYear = rollIntoContracts.contractYear;
                                    aDMPositionImportWeb.contractMonth = rollIntoContracts.contractMonthInt;
                                    aDMPositionImportWeb.optionYear = rollIntoContracts.optionYear;
                                    aDMPositionImportWeb.optionMonth = rollIntoContracts.optionMonthInt;

                                    aDMPositionImportWeb.yearFraction =
                                        optionSpreadManager.calcYearFrac(rollIntoContracts.expirationDate,
                                                                DateTime.Now.Date);

                                    aDMPositionImportWeb.futurePriceUsedToCalculateStrikes = rollIntoContracts.futurePriceUsedToCalculateStrikes;

                                    //rollIntoContracts.strikePrice.ToString();
                                    //NOV 11 2014 change

                                    aDMPositionImportWeb.instrumentArrayIdx =
                                        rollIntoContracts.indexOfInstrumentInInstrumentsArray;

                                   

                                    
                                }



                            }
                        }



                    }
                }                


                gridViewModelADMCompare.RowCount = optionSpreadManager.admPositionImportWebListForCompare.Count;

                if (optionSpreadManager.includeExcludeOrdersInModelADMCompare)
                {
                    gridViewModelADMCompare.Columns[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.NEW_ORDERS].Visible = true;
                    gridViewModelADMCompare.Columns[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ROLL_INTO_ORDERS].Visible = true;
                }
                else
                {
                    gridViewModelADMCompare.Columns[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.NEW_ORDERS].Visible = false;
                    gridViewModelADMCompare.Columns[(int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ROLL_INTO_ORDERS].Visible = false;
                }

                //Color rowColor1 = Color.DarkGray;
                //Color rowColor2 = Color.DarkBlue;

                //Color currentRowColor = rowColor1;

                Color nonAccentCell = Color.LightGray;
                Color accentCell = Color.White;
                Color rebalanceAccentCell = Color.Yellow;


                //HashSet<int> alreadUsedExcluded = new HashSet<int>();

                for (int admRowCounter = 0; admRowCounter < optionSpreadManager.admPositionImportWebListForCompare.Count; admRowCounter++)
                {

                    //this calculates the pl for each contract on the compare page
                    //JAN 30 2015

                    if (optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].optionSpreadExpression != null)
                    {

                        optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelPL =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].optionSpreadExpression.plChgForContractSummary
                            +
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].optionSpreadExpression.plChgOrders;


                    }

                    optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].fcmPL =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].contractData.pAndLDay
                            +
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].contractData.pAndLDayOrders;




                    int netContracts = (int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].Net
                        + optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].transNetLong
                        - optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].transNetShort;


                    if (optionSpreadManager.includeExcludeOrdersInModelADMCompare)
                    {
                        optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLots =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots
                            + optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].orderLots
                            + optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rollIntoLots
                            - netContracts;
                    }
                    else
                    {
                        optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLots =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots
                            - netContracts;
                    }

                    optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLotsForPayoffWithOrders =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots
                            + optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].orderLots
                            + optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rollIntoLots
                            - netContracts;

                    optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLotsForPayoffNoOrders =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots
                            - netContracts;



                    //switch (admRowCounter % 2)
                    //{
                    //    case 0:
                    //        currentRowColor = rowColor1;
                    //        break;

                    //    case 1:
                    //        currentRowColor = rowColor2;
                    //        break;

                    //}


                    if (gridViewModelADMCompare.Rows[admRowCounter].HeaderCell.Value == null
                        ||
                        gridViewModelADMCompare.Rows[admRowCounter].HeaderCell.Value.ToString().CompareTo(
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].cqgSymbol) != 0
                        )
                    {
                        gridViewModelADMCompare.Rows[admRowCounter]
                            .HeaderCell.Value =
                            optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].cqgSymbol;

                        //gridViewModelADMCompare.Rows[admRowCounter]
                        //        .HeaderCell.Style.BackColor = currentRowColor;
                    }

                    

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.MODEL_OFFICE_ACCT,
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].MODEL_OFFICE_ACCT,
                                Color.LawnGreen);

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.FCM_OFFICE_ACCT,
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].POFFIC +
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].PACCT,
                                Color.LawnGreen);

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.MODEL_PL,
                                Math.Round(optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelPL).ToString(),
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelPL >= 0 ? Color.LawnGreen :
                                Color.Red);

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.FCM_PL,
                                Math.Round(optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].fcmPL).ToString(),
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].fcmPL >= 0 ? Color.LawnGreen :
                                Color.Red);

                    double diff = optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].fcmPL
                        - optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelPL;

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.DIFF_PL,
                                Math.Round(diff).ToString(),
                                diff >= 0 ? Color.LawnGreen :
                                Color.Red);

                    if (optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots != 0)
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.MODEL,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots).ToString(), accentCell);
                    }
                    else
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.MODEL,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].modelLots).ToString(), nonAccentCell);
                    }

                    if (optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].orderLots != 0)
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.NEW_ORDERS,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].orderLots).ToString(), accentCell);
                    }
                    else
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.NEW_ORDERS,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].orderLots).ToString(), nonAccentCell);
                    }


                    if (optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rollIntoLots != 0)
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ROLL_INTO_ORDERS,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rollIntoLots).ToString(), accentCell);
                    }
                    else
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ROLL_INTO_ORDERS,
                                ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rollIntoLots).ToString(), nonAccentCell);
                    }


                    if (netContracts != 0)
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.FCM,
                                netContracts.ToString(), accentCell);
                    }
                    else
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.FCM,
                                netContracts.ToString(), nonAccentCell);
                    }

                    if (optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLots != 0)
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                            ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLots).ToString(), rebalanceAccentCell);
                    }
                    else
                    {
                        fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                            ((int)optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].rebalanceLots).ToString(), nonAccentCell);
                    }


                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.Strike,
                        optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].strike, accentCell);


                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.INSTRUMENT_ID,
                        optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].instrumentArrayIdx.ToString(), accentCell);

                    fillGridViewADMCompareFields(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ACCT_GROUP_IDX,
                                optionSpreadManager.admPositionImportWebListForCompare[admRowCounter].acctGroup.ToString(),
                                Color.White);

                    setBackgroundZeroPrice_ModelADMCompare(optionRealtimeMonitor, admRowCounter, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE);
                }

                optionRealtimeMonitor.updateModelADMCompareViewable();

            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }

            //return liveLegRowIdexes;
        }


        delegate void ThreadSafeFillGridViewValueAsStringAndColorDelegate(
            OptionRealtimeMonitor optionRealtimeMonitor, int rowToUpdate, int col, string displayValue, Color color);

        public void fillGridViewADMCompareFields(
            OptionRealtimeMonitor optionRealtimeMonitor, int rowToUpdate, int col, string displayValue, Color color)
        {
            try
            {
                if (optionRealtimeMonitor.InvokeRequired)
                {
                    ThreadSafeFillGridViewValueAsStringAndColorDelegate d = new ThreadSafeFillGridViewValueAsStringAndColorDelegate(
                        threadSafefillGridViewADMCompareFields);

                    optionRealtimeMonitor.Invoke(d, optionRealtimeMonitor, rowToUpdate, col, displayValue, color);
                }
                else
                {
                    threadSafefillGridViewADMCompareFields(optionRealtimeMonitor, rowToUpdate, col, displayValue, color);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void threadSafefillGridViewADMCompareFields(
            OptionRealtimeMonitor optionRealtimeMonitor, int rowToUpdate, int col, string displayValue, Color color)
        {
            DataGridView gridViewModelADMCompare = optionRealtimeMonitor.getGridViewModelADMCompare;

            if (gridViewModelADMCompare.Rows[rowToUpdate].Cells[col].Value == null
                        ||
                        gridViewModelADMCompare.Rows[rowToUpdate].Cells[col].Value.ToString().CompareTo(displayValue) != 0
                        )
            {
                gridViewModelADMCompare.Rows[rowToUpdate].Cells[col].Value = displayValue;

                gridViewModelADMCompare.Rows[rowToUpdate].Cells[col].Style.BackColor = color;

            }
        }

        internal void setBackgroundZeroPrice_ModelADMCompare(OptionRealtimeMonitor optionRealtimeMonitor, int row, int col)
        {
            DataGridView gridViewModelADMCompare = optionRealtimeMonitor.getGridViewModelADMCompare;

            if (optionSpreadManager.zeroPriceContractList.Contains(
                        optionSpreadManager.admPositionImportWebListForCompare[row].cqgSymbol))
            {
                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE,
                        "true", Color.Red);

                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS,
                        "false", Color.Cyan);

                optionSpreadManager.statusAndConnectedUpdates.markLiveAsConnected(gridViewModelADMCompare, row,
                    (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                    false, Color.Red);
            }
            else if (optionSpreadManager.exceptionContractList.Contains(
                        optionSpreadManager.admPositionImportWebListForCompare[row].cqgSymbol))
            {
                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE,
                        "false", Color.LightBlue);

                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS,
                        "true", Color.MediumPurple);

                optionSpreadManager.statusAndConnectedUpdates.markLiveAsConnected(gridViewModelADMCompare, row,
                    (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                    false, Color.MediumPurple);
            }
            else
            {

                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.ZERO_PRICE,
                        "false", Color.LightBlue);

                fillGridViewADMCompareFields(optionRealtimeMonitor, row, (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.EXCEPTIONS,
                        "false", Color.Cyan);

                if (optionSpreadManager.admPositionImportWebListForCompare != null && row < optionSpreadManager.admPositionImportWebListForCompare.Count
                    && optionSpreadManager.admPositionImportWebListForCompare[row].rebalanceLots == 0)
                {
                    optionSpreadManager.statusAndConnectedUpdates.markLiveAsConnected(gridViewModelADMCompare, row,
                        (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                        false, Color.LightGray);
                }
                else
                {
                    optionSpreadManager.statusAndConnectedUpdates.markLiveAsConnected(gridViewModelADMCompare, row,
                        (int)ADM_MODEL_POSITION_COMPARE_FIELDS_DISPLAYED.REBALANCE,
                        false, Color.Yellow);
                }
            }
        }




    }
}
