using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeSpreadMonitor.FormManipulation
{
    class StrategyAction
    {

        //public void sendUpdateToOrderGrid()
        //{

        //    //resetNumberOfContractsForEachExpression();


        //    bool actionTest;

        //    StringBuilder actionRule = new StringBuilder();

        //    for (int i = 0; i < optionStrategies.Length; i++)
        //    {
        //        actionTest = false;

        //        actionRule.Clear();

        //        switch (optionStrategies[i].actionType)
        //        {
        //            case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.NO_ACTION:
        //                {
        //                    //                             bool actionTest = optionStrategies[i].entryRule.Evaluate();
        //                    // 
        //                    //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
        //                    //                                 optionStrategies[i].entryRule.parsedWithVariables(),  //OriginalExpression,
        //                    //                                 true, actionTest ? 1 : -1);
        //                    // 
        //                    //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_TEST,
        //                    //                                 actionTest.ToString(),
        //                    //                                 true, actionTest ? 1 : -1);

        //                    break;
        //                }

        //            case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY:
        //                {
        //                    actionTest = optionStrategies[i].entryRule.Evaluate();

        //                    actionRule.Append(optionStrategies[i].entryRule.parsedWithVariables());
        //                    actionRule.Append(" : ");
        //                    //actionRule.Append(actionTest);

        //                    if (actionTest)
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ENTER));
        //                    }
        //                    else
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_ENTER));
        //                    }

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    actionRule.Clear();
        //                    actionRule.Append("FUT: ");
        //                    actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
        //                        .optionSpreadExpression.decisionPrice);

        //                    //actionRule.Append("\nINTRP: ");
        //                    //actionRule.Append(optionStrategies[i].entryRule.strategyStateComparisonValues.syntheticCloseFutureValue);

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    //                             fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_TEST,
        //                    //                                 actionTest.ToString(),
        //                    //                                 true, actionTest ? 1 : -1);

        //                    int rowIdx = 0;

        //                    for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
        //                    {
        //                        int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots]
        //                                .stateValueParsed[legCounter];

        //                        LEG_ACTION_TYPES buyOrSell;

        //                        if (contractsToEnter > 0)
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
        //                        }
        //                        else
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
        //                        }


        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
        //                            Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                            optionStrategies[i].legInfo[legCounter].cqgSymbol,
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
        //                            contractsToEnter.ToString(),
        //                            true, actionTest ? 1 : -1);

        //                        //if (actionTest)
        //                        {
        //                            updateNumberOfTempContracts(
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression,
        //                                    contractsToEnter, actionTest);
        //                        }

        //                        if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
        //                            && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
        //                        {

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
        //                                    optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                            //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
        //                                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                        }
        //                        else
        //                        {
        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                "", false, -1);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                "", false, -1);
        //                        }

        //                        rowIdx++;
        //                    }

        //                    break;
        //                }

        //            case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.ENTRY_WITH_ROLL:
        //                {
        //                    optionStrategies[i].rollStrikesUpdated = false;

        //                    sendUpdateToRollStrikes(i);

        //                    sendFutureValueUpdateToRollStrikes(i);

        //                    actionTest = optionStrategies[i].entryRule.Evaluate();

        //                    actionRule.Append(optionStrategies[i].entryRule.parsedWithVariables());
        //                    actionRule.Append(" : ");
        //                    //actionRule.Append(actionTest);

        //                    if (actionTest)
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ENTER_WITH_ROLL));
        //                    }
        //                    else
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_ENTER));
        //                    }

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    actionRule.Clear();
        //                    actionRule.Append("FUT: ");
        //                    actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
        //                        .optionSpreadExpression.decisionPrice);

        //                    //actionRule.Append("\nINTRP: ");
        //                    //actionRule.Append(optionStrategies[i].entryRule.strategyStateComparisonValues.syntheticCloseFutureValue);

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    int rowIdx = 0;

        //                    for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
        //                    {
        //                        int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.entryLots]
        //                                .stateValueParsed[legCounter];

        //                        LEG_ACTION_TYPES buyOrSell;

        //                        if (contractsToEnter > 0)
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
        //                        }
        //                        else
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
        //                        }


        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
        //                            Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
        //                            true, actionTest ? 1 : -1);


        //                        StringBuilder cqgSymbol = new StringBuilder();
        //                        double strikePrice = 0;

        //                        if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
        //                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
        //                        {
        //                            cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                                cqgSymbol.ToString(),
        //                                true, actionTest ? 1 : -1);
        //                        }
        //                        else
        //                        {

        //                            cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover);
        //                            cqgSymbol.Append(
        //                                ConversionAndFormatting.convertToTickMovesString(
        //                                    optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                                                [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange],
        //                                                optionStrategies[i].instrument.optionStrikeIncrement,
        //                                                optionStrategies[i].instrument.optionStrikeDisplay)
        //                                                );

        //                            strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                                                [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                                cqgSymbol.ToString(),
        //                                true, actionTest ? 1 : -1);

        //                        }

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
        //                            contractsToEnter.ToString(),
        //                            true, actionTest ? 1 : -1);

        //                        ContractList rollIntoContracts;


        //                        if (optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                    .contractHashTable.Count > 0
        //                            && optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                    .contractHashTable.ContainsKey(cqgSymbol.ToString()))
        //                        {
        //                            rollIntoContracts = optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                .contractHashTable[cqgSymbol.ToString()];
        //                        }
        //                        else
        //                        {
        //                            rollIntoContracts = new ContractList();

        //                            optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                .contractHashTable
        //                                .TryAdd(cqgSymbol.ToString(), rollIntoContracts);
        //                        }

        //                        rollIntoContracts.cqgSymbol = cqgSymbol.ToString();



        //                        rollIntoContracts.contractType = optionStrategies[i].rollIntoLegInfo[legCounter].legContractType;


        //                        if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
        //                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
        //                        {
        //                            rollIntoContracts.contractMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].contractMonthInt;

        //                            rollIntoContracts.contractYear = optionStrategies[i].rollIntoLegInfo[legCounter].contractYear;
        //                        }
        //                        else
        //                        {
        //                            rollIntoContracts.optionMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt;

        //                            rollIntoContracts.optionYear = optionStrategies[i].rollIntoLegInfo[legCounter].optionYear;

        //                            rollIntoContracts.expirationDate = optionStrategies[i].rollIntoLegInfo[legCounter].expirationDate;

        //                            //rollIntoContracts.yearFraction = 
        //                            //    optionSpreadManager.calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
        //                            //                        DateTime.Now.Date);

        //                            //rollIntoContracts.idUnderlyingContract = optionStrategies[i].rollIntoLegInfo[legCounter].idUnderlyingContract;

        //                            rollIntoContracts.strikePrice = strikePrice;
        //                        }





        //                        rollIntoContracts.indexOfInstrumentInInstrumentsArray = optionStrategies[i].indexOfInstrumentInInstrumentsArray;

        //                        rollIntoContracts.expression =
        //                            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression;

        //                        //rollIntoContracts.currentlyRollingContract = true;

        //                        //rollIntoContracts.tempNumberOfContracts += contractsToEnter;

        //                        //rollIntoContracts.actionTest = actionTest;

        //                        updateNumberOfRollIntoContracts(rollIntoContracts, contractsToEnter, actionTest);

        //                        //if (actionTest)
        //                        //{
        //                        //    if (optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null)
        //                        //    {
        //                        //        updateNumberOfTempContracts(
        //                        //            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression,
        //                        //                contractsToEnter);
        //                        //    }
        //                        //}

        //                        if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
        //                            && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null
        //                            && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument != null)
        //                        {

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
        //                                    optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPrice),
        //                                true,
        //                                actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                            //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPriceTime
        //                                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
        //                                true,
        //                                actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
        //                        }
        //                        else
        //                        {
        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                "", false, -1);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                "", false, -1);
        //                        }

        //                        rowIdx++;
        //                    }

        //                    break;
        //                }

        //            case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT:
        //                {
        //                    actionTest = optionStrategies[i].exitRule.Evaluate();

        //                    actionRule.Append(optionStrategies[i].exitRule.parsedWithVariables());
        //                    actionRule.Append(" : ");
        //                    //actionRule.Append(actionTest);

        //                    if (actionTest)
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.EXIT_ONLY));
        //                    }
        //                    else
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.DO_NOT_EXIT));
        //                    }

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    actionRule.Clear();
        //                    actionRule.Append("FUT: ");
        //                    actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
        //                        .optionSpreadExpression.decisionPrice);

        //                    //actionRule.Append("\nINTRP: ");
        //                    //actionRule.Append(optionStrategies[i].exitRule.strategyStateComparisonValues.syntheticCloseFutureValue);

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    int rowIdx = 0;

        //                    for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
        //                    {
        //                        int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
        //                                .stateValueParsed[legCounter];

        //                        LEG_ACTION_TYPES buyOrSell;

        //                        if (contractsToEnter > 0)
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.CLOSE_BUY;
        //                        }
        //                        else
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.CLOSE_SELL;
        //                        }

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
        //                            Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                            optionStrategies[i].legInfo[legCounter].cqgSymbol,
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
        //                            optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
        //                                .stateValueParsed[legCounter].ToString(),
        //                            true, actionTest ? 1 : -1);

        //                        //if (actionTest)
        //                        {
        //                            updateNumberOfTempContracts(
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression,
        //                                    contractsToEnter, actionTest);
        //                        }

        //                        if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
        //                            && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
        //                        {

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
        //                                    optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                            //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
        //                                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
        //                        }
        //                        else
        //                        {
        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                "", false, -1);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                "", false, -1);
        //                        }

        //                        rowIdx++;
        //                    }

        //                    break;
        //                }

        //            case SPREAD_POTENTIAL_OPTION_ACTION_TYPES.EXIT_OR_ROLL_OVER:
        //                {
        //                    optionStrategies[i].rollStrikesUpdated = false;

        //                    sendUpdateToRollStrikes(i);

        //                    sendFutureValueUpdateToRollStrikes(i);

        //                    bool exitTest = optionStrategies[i].exitRule.Evaluate();

        //                    //if (!exitTest)
        //                    //{

        //                    //}

        //                    //ALWAYS WILL EXIT B/C WILL BE ROLLING IF NOT EXITING
        //                    actionTest = true;

        //                    actionRule.Append(optionStrategies[i].exitRule.parsedWithVariables());
        //                    actionRule.Append(" : ");

        //                    if (exitTest)
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.EXIT_ONLY));
        //                    }
        //                    else
        //                    {
        //                        actionRule.Append(Enum.GetName(typeof(SPREAD_ACTION_TYPES), SPREAD_ACTION_TYPES.ROLL_OVER));
        //                    }

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.ACTION_RULE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    actionRule.Clear();
        //                    actionRule.Append("FUT: ");
        //                    actionRule.Append(optionStrategies[i].legData[optionStrategies[i].idxOfFutureLeg]
        //                        .optionSpreadExpression.decisionPrice);

        //                    //actionRule.Append("\nINTRP: ");
        //                    //actionRule.Append(optionStrategies[i].exitRule.strategyStateComparisonValues.syntheticCloseFutureValue);

        //                    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[0], (int)OPTION_ORDERS_COLUMNS.INTERPOLATED_SYNCLOSE,
        //                        actionRule.ToString(),  //OriginalExpression,
        //                        true, actionTest ? 1 : -1);

        //                    int rowIdx = 0;

        //                    for (int legCounter = 0; legCounter < optionStrategies[i].legInfo.Length; legCounter++)
        //                    {
        //                        int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
        //                                .stateValueParsed[legCounter];

        //                        LEG_ACTION_TYPES buyOrSell;

        //                        if (contractsToEnter > 0)
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.CLOSE_BUY;
        //                        }
        //                        else
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.CLOSE_SELL;
        //                        }

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
        //                            Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                            optionStrategies[i].legInfo[legCounter].cqgSymbol,
        //                            true, actionTest ? 1 : -1);

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
        //                            optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.exitLots]
        //                                .stateValueParsed[legCounter].ToString(),
        //                            true, actionTest ? 1 : -1);

        //                        //if (actionTest)
        //                        {
        //                            updateNumberOfTempContracts(
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression,
        //                                    contractsToEnter, actionTest);
        //                        }

        //                        if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
        //                            && optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument != null)
        //                        {

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
        //                                    optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPrice),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                            //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                optionStrategies[i].legData[legCounter].optionSpreadExpression.transactionPriceTime
        //                                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
        //                                true,
        //                                actionTest && optionStrategies[i].legData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].legData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
        //                        }
        //                        else
        //                        {
        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                "", false, -1);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                "", false, -1);
        //                        }

        //                        rowIdx++;
        //                    }


        //                    //************************



        //                    //int rowIdx = 0;

        //                    if (exitTest)
        //                    {
        //                        actionTest = false;  //don't roll into spread because exit rule true;
        //                    }

        //                    for (int legCounter = 0; legCounter < optionStrategies[i].rollIntoLegInfo.Length; legCounter++)
        //                    {
        //                        int contractsToEnter = (int)optionStrategies[i].optionStrategyParameters[(int)TBL_STRATEGY_STATE_FIELDS.currentPosition]
        //                                .stateValueParsed[legCounter];

        //                        LEG_ACTION_TYPES buyOrSell;

        //                        if (contractsToEnter > 0)
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_BUY;
        //                        }
        //                        else
        //                        {
        //                            buyOrSell = LEG_ACTION_TYPES.ENTER_SELL;
        //                        }


        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.SPREAD_ACTION,
        //                            Enum.GetName(typeof(LEG_ACTION_TYPES), buyOrSell).Replace('_', ' '),
        //                            true, actionTest ? 1 : -1);


        //                        StringBuilder cqgSymbol = new StringBuilder();

        //                        //StringBuilder symbolComparison = new StringBuilder();

        //                        double strikePrice = 0;

        //                        if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
        //                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
        //                        {
        //                            cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                                cqgSymbol.ToString(),
        //                                true, actionTest ? 1 : -1);
        //                        }
        //                        else
        //                        {
        //                            //if (optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol == null)
        //                            {
        //                                //TSErrorCatch.debugWriteOut(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol + "  "
        //                                //    + cqgSymbol.ToString());

        //                                //cqgSymbol = new StringBuilder();
        //                                cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbolWithoutStrike_ForRollover);
        //                                //cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                                //                    [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange]);

        //                                cqgSymbol.Append(
        //                                    ConversionAndFormatting.convertToTickMovesString(
        //                                        optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                                                    [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange],
        //                                                    optionStrategies[i].instrument.optionStrikeIncrement,
        //                                                    optionStrategies[i].instrument.optionStrikeDisplay)
        //                                                    );

        //                                strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                                                [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

        //                                fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                                    cqgSymbol.ToString(),
        //                                    true, actionTest ? 1 : -1);
        //                            }
        //                            //else
        //                            //{
        //                            //    cqgSymbol.Append(optionStrategies[i].rollIntoLegInfo[legCounter].cqgSymbol);

        //                            //    strikePrice = optionStrategies[i].rollIntoLegInfo[legCounter].optionStrikePriceReference
        //                            //                    [optionStrategies[i].rollIntoLegInfo[legCounter].strikeIndexOfStrikeRange];

        //                            //    fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.CONTRACT,
        //                            //        cqgSymbol.ToString(),
        //                            //        true, actionTest ? 1 : -1);
        //                            //}



        //                        }

        //                        fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.QTY,
        //                            contractsToEnter.ToString(),
        //                            true, actionTest ? 1 : -1);

        //                        //if (actionTest)
        //                        //{
        //                        //    if (optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null)
        //                        //    {
        //                        //        updateNumberOfTempContracts(
        //                        //            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression,
        //                        //                contractsToEnter);
        //                        //    }
        //                        //}

        //                        ContractList rollIntoContracts;

        //                        if (optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                    .contractHashTable.Count > 0
        //                            && optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                    .contractHashTable.ContainsKey(cqgSymbol.ToString()))
        //                        {
        //                            rollIntoContracts = optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                .contractHashTable[cqgSymbol.ToString()];
        //                        }
        //                        else
        //                        {
        //                            rollIntoContracts = new ContractList();

        //                            //instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                            //    .contractHashTable.Add(cqgSymbol.ToString(), rollIntoContracts);

        //                            //int removeAttempts = 0;

        //                            //while (!instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                            //    .contractHashTable
        //                            //    .TryAdd(cqgSymbol.ToString(), rollIntoContracts) && removeAttempts < 10)
        //                            //{
        //                            //    removeAttempts++;
        //                            //}

        //                            optionSpreadManager.instrumentRollIntoSummary[optionStrategies[i].indexOfInstrumentInInstrumentsArray]
        //                                .contractHashTable
        //                                .TryAdd(cqgSymbol.ToString(), rollIntoContracts);
        //                        }

        //                        rollIntoContracts.cqgSymbol = cqgSymbol.ToString();


        //                        rollIntoContracts.contractType = optionStrategies[i].rollIntoLegInfo[legCounter].legContractType;

        //                        if (optionStrategies[i].rollIntoLegInfo[legCounter].legContractType ==
        //                                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
        //                        {
        //                            rollIntoContracts.contractMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].contractMonthInt;

        //                            rollIntoContracts.contractYear = optionStrategies[i].rollIntoLegInfo[legCounter].contractYear;
        //                        }
        //                        else
        //                        {
        //                            rollIntoContracts.optionMonthInt = optionStrategies[i].rollIntoLegInfo[legCounter].optionMonthInt;

        //                            rollIntoContracts.optionYear = optionStrategies[i].rollIntoLegInfo[legCounter].optionYear;


        //                            rollIntoContracts.expirationDate = optionStrategies[i].rollIntoLegInfo[legCounter].expirationDate;

        //                            //rollIntoContracts.yearFraction =
        //                            //    optionSpreadManager.calcYearFrac(optionStrategies[i].legInfo[legCounter].expirationDate,
        //                            //                        DateTime.Now.Date);

        //                            //rollIntoContracts.idUnderlyingContract = optionStrategies[i].rollIntoLegInfo[legCounter].idUnderlyingContract;

        //                            rollIntoContracts.strikePrice = strikePrice;
        //                        }

        //                        rollIntoContracts.indexOfInstrumentInInstrumentsArray = optionStrategies[i].indexOfInstrumentInInstrumentsArray;

        //                        rollIntoContracts.expression =
        //                            optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression;

        //                        //rollIntoContracts.currentlyRollingContract = true;

        //                        //rollIntoContracts.tempNumberOfContracts += contractsToEnter;

        //                        //rollIntoContracts.actionTest = actionTest;

        //                        updateNumberOfRollIntoContracts(rollIntoContracts, contractsToEnter, actionTest);

        //                        if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis
        //                            && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression != null
        //                            && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument != null)
        //                        {

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.cqgInstrument.ToDisplayPrice(
        //                                    optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPrice),
        //                                true,
        //                                actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);



        //                            //gridOrders.Columns[(int)OPTION_ORDERS_COLUMNS.TRANSACTION_TIME].DefaultCellStyle.Font = new Font("Tahoma", 6);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.transactionPriceTime
        //                                    .ToString("yyyy-MM-dd HH:mm", DateTimeFormatInfo.InvariantInfo),
        //                                true,
        //                                actionTest && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.reachedTransactionTimeBoundary
        //                                && optionStrategies[i].rollIntoLegData[legCounter].optionSpreadExpression.settlementIsCurrentDay ? 1 : -1);
        //                        }
        //                        else
        //                        {
        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_PRICE,
        //                                "", false, -1);

        //                            fillLiveOrderPage(optionStrategies[i].orderGridRowLoc[rowIdx], (int)OPTION_ORDERS_COLUMNS.TRANS_TIME,
        //                                "", false, -1);
        //                        }

        //                        rowIdx++;
        //                    }

        //                    break;

        //                }


        //        }
        //    }

        //    //copyNumberOfContractsFromTemp();

        //}
    }
}
