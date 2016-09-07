using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class OptionPLChart : Form
    {
        delegate void ThreadSafeUpdateMargin(double marginVal);

        private bool highlightingIntheMoney = false;

        private Instrument instrument;

        //private Array optionSpreadContractTypesArray;

        private double rRisk;

        private PAYOFF_CHART_TYPE chartType;

        private int countOfTest = 1000;

        private List<int> contractSummaryExpressionListIdx;

        private List<OptionSpreadExpression> optionSpreadExpressionList;

        private OptionArrayTypes optionArrayTypes;

        private OptionSpreadManager optionSpreadManager;

        private int brokerAccountChosen;

        private Dictionary<string, int> brokerAcctDictionary = new Dictionary<string, int>();

        //double futurePrice = 0;

        public OptionPLChart(string x, OptionArrayTypes optionArrayTypes, OptionSpreadManager optionSpreadManager)
        {
            InitializeComponent();

            this.optionSpreadManager = optionSpreadManager;

            this.optionArrayTypes = optionArrayTypes;

            setupGrid();

            setupTreeViewBrokerAcct();
        }

        public enum PAYOFF_CHART_TYPE
        {
            CONTRACT_SUMMARY_PAYOFF,
            ORDER_SUMMARY_PAYOFF,
            CONTRACT_AND_ORDER_SUMMARY_PAYOFF
        }

        public enum OPTION_PL_COLUMNS
        {


            CNTRT_TYPE,

            STRIKE,

            AVG_PRC,

            NET,

            IMPL_VOL,

            DAYS_TO_EXP,

            YEAR,

            MTH_AS_INT,

            BRKR,

            ACCT,

            DEL_ROW,

            SEL_ROW


        };

        private void addGridDeleteImg(int row, int cell)
        {
            //DataGridViewImageCell imageCell = new DataGridViewImageCell();

            //imageCell.Value 

            gridViewSpreadGrid.Rows[row].Cells[cell].Value = "X";


        }

        private void setupGrid()
        {
            Type optionPLColTypes = typeof(OPTION_PL_COLUMNS);
            Array optionPLColTypesArray = Enum.GetNames(optionPLColTypes);

            //Type optionSpreadContractTypes = typeof(OPTION_SPREAD_CONTRACT_TYPE);
            //optionSpreadContractTypesArray = Enum.GetNames(optionSpreadContractTypes);


            gridViewSpreadGrid.ColumnCount = optionPLColTypesArray.Length - 4;

            DataGridViewComboBoxColumn myCboBox = new DataGridViewComboBoxColumn();
            myCboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            for (int contractTypeCount = 0; contractTypeCount < optionArrayTypes.optionSpreadContractTypesArray.Length; contractTypeCount++)
            {
                myCboBox.Items.Add(optionArrayTypes.optionSpreadContractTypesArray.GetValue(contractTypeCount).ToString());
            }

            gridViewSpreadGrid.Columns.Insert(0, myCboBox);


            //*************
            DataGridViewComboBoxColumn brokerCboBox = new DataGridViewComboBoxColumn();
            brokerCboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            DataGridViewComboBoxColumn acctCboBox = new DataGridViewComboBoxColumn();
            acctCboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            HashSet<string> brokerAdded = new HashSet<string>();
            HashSet<string> acctAdded = new HashSet<string>();



            for (int brokerCnt = 0; brokerCnt < optionSpreadManager.portfolioGroupAllocation.Length; brokerCnt++)
            {
                StringBuilder brokerAcct = new StringBuilder();
                brokerAcct.Append(optionSpreadManager.portfolioGroupAllocation[brokerCnt].broker);
                brokerAcct.Append(optionSpreadManager.portfolioGroupAllocation[brokerCnt].account);

                brokerAcctDictionary.Add(brokerAcct.ToString(), brokerCnt);

                if (!brokerAdded.Contains(optionSpreadManager.portfolioGroupAllocation[brokerCnt].broker))
                {
                    brokerAdded.Add(optionSpreadManager.portfolioGroupAllocation[brokerCnt].broker);
                    brokerCboBox.Items.Add(optionSpreadManager.portfolioGroupAllocation[brokerCnt].broker);
                }

                string acct = optionSpreadManager.selectAcct(
                                "",
                                    optionSpreadManager.portfolioGroupAllocation[brokerCnt], true);

                //if (optionSpreadManager.portfolioGroupAllocation[brokerCnt].useConfigFile)
                //{
                //    acctAdded.Add(optionSpreadManager.portfolioGroupAllocation[brokerCnt].cfgfile);
                //    acctCboBox.Items.Add(optionSpreadManager.portfolioGroupAllocation[brokerCnt].cfgfile);
                //}
                //else 
                if (!acctAdded.Contains(acct))
                {
                    acctAdded.Add(acct);
                    acctCboBox.Items.Add(acct);
                }
            }

            gridViewSpreadGrid.Columns.Insert(8, brokerCboBox);
            gridViewSpreadGrid.Columns.Insert(9, acctCboBox);
            //*************


            DataGridViewCheckBoxColumn myChkBox = new DataGridViewCheckBoxColumn();
            gridViewSpreadGrid.Columns.Add(myChkBox);


            gridViewSpreadGrid.EnableHeadersVisualStyles = false;

            DataGridViewCellStyle colTotalPortStyle = gridViewSpreadGrid.ColumnHeadersDefaultCellStyle;
            colTotalPortStyle.BackColor = Color.Black;
            colTotalPortStyle.ForeColor = Color.White;

            DataGridViewCellStyle rowTotalPortStyle = gridViewSpreadGrid.RowHeadersDefaultCellStyle;
            rowTotalPortStyle.BackColor = Color.Black;
            rowTotalPortStyle.ForeColor = Color.White;

            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Width = 55;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.STRIKE].Width = 50;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.AVG_PRC].Width = 50;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.NET].Width = 30;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.IMPL_VOL].Width = 50;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Width = 35;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.YEAR].Width = 40;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Width = 30;

            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.BRKR].Width = 50;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.ACCT].Width = 100;

            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DEL_ROW].Width = 30;
            //gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DEL_ROW].DefaultCellStyle.BackColor = Color.Black;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DEL_ROW].DefaultCellStyle.ForeColor = Color.Red;
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DEL_ROW].DefaultCellStyle.Font =
                new Font("Tahoma", 8, FontStyle.Bold);
            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DEL_ROW].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

            gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.SEL_ROW].Width = 30;

            //gridViewSpreadGrid.Columns[(int)OPTION_PL_COLUMNS.DELETE].DefaultCellStyle = Color.Red;
            //Font = new Font("Tahoma", 7);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < optionPLColTypesArray.Length; i++)
            {
                sb.Clear();

                sb.Append(optionPLColTypesArray.GetValue(i).ToString());

                gridViewSpreadGrid
                    .Columns[i]
                    .HeaderCell.Value = sb.ToString().Replace('_', ' ');
            }
        }

        public void setupTreeViewBrokerAcct()
        {
            brokerAccountChosen = optionSpreadManager.portfolioGroupAllocation.Length;

            for (int groupAllocCnt = 0; groupAllocCnt <= optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
            {

                if (groupAllocCnt == optionSpreadManager.portfolioGroupAllocation.Length)
                {


                    treeViewBrokerAcct.Nodes.Add(groupAllocCnt.ToString(), "ALL ACCTS");

                    treeViewBrokerAcct.SelectedNode = treeViewBrokerAcct.Nodes[groupAllocCnt];
                }
                else
                {
                    StringBuilder treeVal = new StringBuilder();

                    if (optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].useConfigFile)
                    {
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                        treeVal.Append(" | CFG:");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].cfgfile);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].FCM_POFFIC_PACCT);
                    }
                    else
                    {
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].account);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple);
                        treeVal.Append(" | ");
                        treeVal.Append(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].FCM_POFFIC_PACCT);
                    }

                    treeViewBrokerAcct.Nodes.Add(groupAllocCnt.ToString(), treeVal.ToString());
                }
            }
        }


        private void treeViewBrokerAcct_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode x = treeViewBrokerAcct.SelectedNode;

            if (x != null)
            {

                brokerAccountChosen = x.Index;

                int legCount = 0;
                while (legCount < gridViewSpreadGrid.RowCount)
                {
                    gridViewSpreadGrid.Rows[legCount].DefaultCellStyle.BackColor = Color.White;

                    if (brokerAccountChosen < optionSpreadManager.portfolioGroupAllocation.Length)
                    {
                        if ((gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value != null
                            &&
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value != null)
                            &&
                            (
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString().CompareTo(
                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].broker) != 0
                            ||
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString().CompareTo(
                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].account) != 0
                            ))
                        {
                            gridViewSpreadGrid.Rows[legCount].DefaultCellStyle.BackColor = Color.DarkGray;
                            //continueThisLeg = false;
                        }
                    }

                    legCount++;
                }

                fillChart();

                //optionSpreadManager.portfolioGroupAllocationChosen.Clear();

                //optionSpreadManager.acctGroupSelected = x.Index;

                //if (x.Index == optionSpreadManager.portfolioGroupAllocation.Length)
                //{
                //    //selected all portfoliogroups



                //    optionSpreadManager.portfolioGroupTotalMultiple = 0;

                //    for (int i = 0; i < optionSpreadManager.portfolioGroupAllocation.Length; i++)
                //    {
                //        optionSpreadManager.portfolioGroupAllocationChosen.Add(i);

                //        optionSpreadManager.portfolioGroupTotalMultiple += optionSpreadManager.portfolioGroupAllocation[i].multiple;
                //    }
                //}
                //else
                //{
                //    optionSpreadManager.portfolioGroupAllocationChosen.Add(x.Index);

                //    optionSpreadManager.portfolioGroupTotalMultiple = optionSpreadManager.portfolioGroupAllocation[x.Index].multiple;
                //}


                //updateOrderSummaryAfterInstrumentSelect();

                //sendUpdateToPortfolioTotalGrid();

                //if (optionSpreadManager.realtimeMonitorSettings.eodAnalysis)
                //{
                //    sendUpdateToPortfolioTotalSettlementGrid();
                //}

                //optionSpreadManager.gridViewContractSummaryManipulation.sendUpdateToContractSummaryLiveData(this);

                //optionSpreadManager.resetPortfolioGroupFcmOfficeAcctChosenHashSet();

                //updateSelectedInstrumentFromTree();
            }
        }


        public void fillGridFromContractSummary(
            OptionSpreadManager optionSpreadManager,
            List<int> contractSummaryExpressionListIdx,
            List<OptionSpreadExpression> optionSpreadExpressionList,
            Instrument instrument,
            double rRisk, PAYOFF_CHART_TYPE chartType)
        {
            fillGrid(
                optionSpreadManager,
                contractSummaryExpressionListIdx,
                optionSpreadExpressionList,
                instrument,
                rRisk, chartType,
                null);
        }

        /// <summary>
        /// Fills the grid.
        /// </summary>
        /// <param name="optionSpreadManager">The option spread manager.</param>
        /// <param name="contractSummaryExpressionListIdx">Index of the contract summary expression list.</param>
        /// <param name="optionSpreadExpressionList">The option spread expression list.</param>
        /// <param name="instrument">The instrument.</param>
        /// <param name="rRisk">The r risk.</param>
        /// <param name="chartType">Type of the chart.</param>
        /// <param name="contractListOfRollovers">The contract list of rollovers.</param>
        public void fillGrid(
            OptionSpreadManager optionSpreadManager,
            List<int> contractSummaryExpressionListIdx,
            List<OptionSpreadExpression> optionSpreadExpressionList,
            Instrument instrument,
            double rRisk, PAYOFF_CHART_TYPE chartType,
            List<ContractList> contractListOfRollovers)
        {

            this.optionSpreadManager = optionSpreadManager;

            this.contractSummaryExpressionListIdx = contractSummaryExpressionListIdx;
            this.optionSpreadExpressionList = optionSpreadExpressionList;

            this.instrument = instrument;

            this.rRisk = rRisk;

            this.chartType = chartType;

            StringBuilder title = new StringBuilder();
            title.Append(instrument.CQGsymbol);

            title.Append(" ");
            title.Append(optionSpreadManager.initializationParmsSaved.portfolioGroupName);
            title.Append(" ");

            if (chartType == PAYOFF_CHART_TYPE.ORDER_SUMMARY_PAYOFF)
            {
                title.Append(" Order ");
            }
            else if (chartType == PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF)
            {
                title.Append(" Contract ");
            }
            else
            {
                title.Append(" Contract And Order ");
            }

            //title.Append("Summary Payoff Structure");
            title.Append(": ");

            //TODO: AUG 10 2015 - PortfolioGroupAllocation
            //title.Append(optionSpreadManager.initializationParmsSaved.FIX_Broker_18220);



            this.Text = title.ToString();



            int numberOfRows = 0;
            int numberOfRolloverRows = 0;

            for (int count = 0; count < contractSummaryExpressionListIdx.Count; count++)
            {
                if (optionSpreadExpressionList[contractSummaryExpressionListIdx[count]].instrument.idxOfInstrumentInList
                    == instrument.idxOfInstrumentInList)
                {
                    numberOfRows++;
                }
            }

            if (contractListOfRollovers != null)
            {
                numberOfRows += contractListOfRollovers.Count;

                numberOfRolloverRows = contractListOfRollovers.Count;
            }

            numberOfRows *= optionSpreadManager.portfolioGroupAllocation.Length;
            numberOfRolloverRows *= optionSpreadManager.portfolioGroupAllocation.Length;

            rRiskTextBox.Text = Math.Round(rRisk).ToString();


            countTextBox.Text = countOfTest.ToString();

            if ((contractSummaryExpressionListIdx.Count > 0 || numberOfRolloverRows > 0)
                && numberOfRows > 0)
            {



                //gridViewSpreadGrid.RowCount = numberOfRows;

                int rowCount = 0;

                for (int portfolioGroupCount = 0;
                    portfolioGroupCount < optionSpreadManager.portfolioGroupAllocation.Length; portfolioGroupCount++)
                {

                    for (int contractCount = 0; contractCount < contractSummaryExpressionListIdx.Count; contractCount++)
                    {
                        if (optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.idxOfInstrumentInList
                            == instrument.idxOfInstrumentInList)
                        {
                            int numOfContracts = 0;
                            if (chartType == PAYOFF_CHART_TYPE.ORDER_SUMMARY_PAYOFF)
                            {
                                numOfContracts =
                                    optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfOrderContracts;
                            }
                            else if (chartType == PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF)
                            {
                                numOfContracts =
                                    optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfLotsHeldForContractSummary;
                            }
                            else
                            {
                                numOfContracts =
                                    optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfOrderContracts
                                    +
                                    optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfLotsHeldForContractSummary;
                            }

                            numOfContracts *=
                                optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].multiple;

                            string acct = optionSpreadManager.selectAcct(
                                //optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.exchangeSymbol,
                                    instrument.exchangeSymbol,
                                    optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount], true);

                            if (fillInGridDataRow(rowCount,
                                optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].broker,
                                acct,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].callPutOrFuture,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].strikePrice,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].defaultPrice,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].impliedVol * 100,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].futureContractYear,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].futureContractMonthInt,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].optionYear,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].optionMonthInt,
                                numOfContracts,
                                optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].yearFraction
                                * 365
                                ))
                            {
                                rowCount++;
                            }
                        }
                    }
                }

                int expCount = 0;

                while (expCount < optionSpreadExpressionList.Count())
                {
                    if (optionSpreadExpressionList[expCount].optionExpressionType ==
                            OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                    {
                        double rfr = optionSpreadExpressionList[expCount].riskFreeRate
                            * 100;

                        riskFreeTextBox.Text = rfr.ToString();

                        break;
                    }

                    expCount++;
                }

                //#####################
                if (contractListOfRollovers != null)
                {
                    //numberOfRows += contractListOfRollovers.Count;

                    //numberOfRolloverRows = contractListOfRollovers.Count;

                    for (int portfolioGroupCount = 0;
                        portfolioGroupCount < optionSpreadManager.portfolioGroupAllocation.Length;
                        portfolioGroupCount++)
                    {

                        for (int contractCount = 0; contractCount < contractListOfRollovers.Count; contractCount++)
                        {
                            double defaultPrice = 0;
                            double impliedVol = 0;
                            if (contractListOfRollovers[contractCount].expression != null)
                            {
                                defaultPrice = contractListOfRollovers[contractCount].expression.defaultPrice;
                                impliedVol = contractListOfRollovers[contractCount].expression.impliedVol * 100;
                            }

                            int numberOfContracts =
                                contractListOfRollovers[contractCount].numberOfContracts
                                * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].multiple;


                            string acct = optionSpreadManager.selectAcct(
                                    instrument.exchangeSymbol,
                                //optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.exchangeSymbol,
                                    optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount], true);


                            if (fillInGridDataRow(rowCount,
                                optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].broker,
                                acct,
                                contractListOfRollovers[contractCount].contractType,
                                contractListOfRollovers[contractCount].strikePrice,
                                defaultPrice,
                                impliedVol,
                                contractListOfRollovers[contractCount].contractYear,
                                contractListOfRollovers[contractCount].contractMonthInt,
                                contractListOfRollovers[contractCount].optionYear,
                                contractListOfRollovers[contractCount].optionMonthInt,
                                contractListOfRollovers[contractCount].numberOfContracts,
                                calcYearFrac(contractListOfRollovers[contractCount].expirationDate, DateTime.Now.Date)
                                    * 365
                                ))
                            {
                                rowCount++;
                            }
                        }
                    }
                }

                int futureIndexLeg = findFutureLeg();

                if (futureIndexLeg < 0)
                {
                    addFutureRow(null, null, 0);
                }

            }

        }

        private double calcYearFrac(DateTime expirationDate, DateTime currentDateTime)
        {
            double yearFrac = 0;



            TimeSpan spanBetweenCurrentAndExp =
                                expirationDate - currentDateTime.Date;

            yearFrac = spanBetweenCurrentAndExp.TotalDays / TradingSystemConstants.DAYS_IN_YEAR;

            if (yearFrac < 0)
            {
                yearFrac = 0;
            }


            return yearFrac;
        }

        public void fillGridWithFCMData(
            OptionSpreadManager optionSpreadManager,
            List<int> contractSummaryExpressionListIdx,
            List<ContractList> contractListOfRollovers,
            List<ADMPositionImportWeb> admPositionImportWebList,
            List<OptionSpreadExpression> optionSpreadExpressionList,
            Instrument instrument, double rRisk)
        {
#if DEBUG
            try
#endif
            {
                //this.contractSummaryExpressionListIdx = contractSummaryExpressionListIdx;
                this.optionSpreadExpressionList = optionSpreadExpressionList;

                this.instrument = instrument;

                this.rRisk = rRisk;

                //this.chartType = chartType;

                StringBuilder title = new StringBuilder();
                title.Append(instrument.CQGsymbol);

                title.Append(" ");
                title.Append(optionSpreadManager.initializationParmsSaved.portfolioGroupName);
                title.Append(" ");

                title.Append(" @ FCM: ");

                //TODO: AUG 10 2015 - PortfolioGroupAllocation
                //title.Append(optionSpreadManager.initializationParmsSaved.FIX_Broker_18220);


                this.Text = title.ToString();



                int numberOfRows = 0;

                for (int count = 0; count < admPositionImportWebList.Count; count++)
                {
                    if (admPositionImportWebList[count].instrument.idxOfInstrumentInList
                        == instrument.idxOfInstrumentInList)
                    {
                        numberOfRows++;
                    }
                }

                if (contractSummaryExpressionListIdx != null)
                {
                    for (int count = 0; count < contractSummaryExpressionListIdx.Count; count++)
                    {
                        if (optionSpreadExpressionList[contractSummaryExpressionListIdx[count]].instrument.idxOfInstrumentInList
                            == instrument.idxOfInstrumentInList)
                        {
                            numberOfRows++;
                        }
                    }
                }

                if (contractListOfRollovers != null)
                {
                    numberOfRows += contractListOfRollovers.Count;
                }


                rRiskTextBox.Text = Math.Round(rRisk).ToString();


                countTextBox.Text = countOfTest.ToString();

                if (admPositionImportWebList.Count > 0 && numberOfRows > 0)
                {

                    //gridViewSpreadGrid.RowCount = numberOfRows;

                    int rowCount = 0;

                    //for (int portfolioGroupCount = 0;
                    //        portfolioGroupCount < optionSpreadManager.portfolioGroupAllocation.Length;
                    //        portfolioGroupCount++)
                    //{

                    for (int contractCount = 0; contractCount < admPositionImportWebList.Count; contractCount++)
                    {
                        if (admPositionImportWebList[contractCount].instrument.idxOfInstrumentInList
                        == instrument.idxOfInstrumentInList)
                        {
                            StringBuilder officeAcct = new StringBuilder();
                            officeAcct.Append(admPositionImportWebList[contractCount].POFFIC);
                            officeAcct.Append(admPositionImportWebList[contractCount].PACCT);

                            if (optionSpreadManager.portfolioGroupIdxAcctStringHashSet.ContainsKey(officeAcct.ToString()))
                            {
                                int indexOfPortfolioAllocation = optionSpreadManager.portfolioGroupIdxAcctStringHashSet[officeAcct.ToString()];


                                //numOfContracts *=
                                //    optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].multiple;

                                string acct = optionSpreadManager.selectAcct(
                                    //admPositionImportWebList[contractCount].instrument.exchangeSymbol,
                                    instrument.exchangeSymbol,
                                    optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation], true);

                                if (fillInGridDataRow(rowCount,
                                    optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation].broker,
                                    acct,
                                    admPositionImportWebList[contractCount].callPutOrFuture,
                                    admPositionImportWebList[contractCount].contractInfo.optionStrikePrice,
                                    admPositionImportWebList[contractCount].contractData.optionSpreadExpression.defaultPrice,
                                    admPositionImportWebList[contractCount].contractData.optionSpreadExpression.impliedVol * 100,
                                    admPositionImportWebList[contractCount].contractInfo.contractYear,
                                    admPositionImportWebList[contractCount].contractInfo.contractMonthInt,
                                    admPositionImportWebList[contractCount].contractInfo.optionYear,
                                    admPositionImportWebList[contractCount].contractInfo.optionMonthInt,
                                    Convert.ToInt32(admPositionImportWebList[contractCount].netContractsEditable
                                        + admPositionImportWebList[contractCount].transNetLong
                                        - admPositionImportWebList[contractCount].transNetShort),
                                    admPositionImportWebList[contractCount].contractData.optionSpreadExpression.yearFraction
                                    * 365
                                    ))
                                {
                                    rowCount++;
                                }
                            }
                        }
                    }
                    //}

                    if (contractSummaryExpressionListIdx != null)
                    {

                        for (int portfolioGroupCount = 0;
                                portfolioGroupCount < optionSpreadManager.portfolioGroupAllocation.Length;
                                portfolioGroupCount++)
                        {

                            for (int contractCount = 0; contractCount < contractSummaryExpressionListIdx.Count; contractCount++)
                            {
                                if (optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.idxOfInstrumentInList
                                    == instrument.idxOfInstrumentInList)
                                {
                                    int numOfContracts = 0;
                                    //if (chartType == PAYOFF_CHART_TYPE.ORDER_SUMMARY_PAYOFF)
                                    {
                                        numOfContracts =
                                            optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfOrderContracts;
                                    }
                                    //else if (chartType == PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF)
                                    //{
                                    //    numOfContracts =
                                    //        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfLotsHeldForContractSummary;
                                    //}
                                    //else
                                    //{
                                    //    numOfContracts =
                                    //        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfOrderContracts
                                    //        +
                                    //        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].numberOfLotsHeldForContractSummary;
                                    //}

                                    numOfContracts *= optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].multiple;

                                    string acct = optionSpreadManager.selectAcct(
                                        //optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.exchangeSymbol,
                                        instrument.exchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount], true);

                                    if (fillInGridDataRow(rowCount,
                                        optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].broker,
                                        acct,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].callPutOrFuture,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].strikePrice,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].defaultPrice,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].impliedVol * 100,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].futureContractYear,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].futureContractMonthInt,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].optionYear,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].optionMonthInt,
                                        numOfContracts,
                                        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].yearFraction
                                        * 365
                                        ))
                                    {
                                        rowCount++;
                                    }
                                }
                            }
                        }
                    }

                    if (contractListOfRollovers != null)
                    {
                        //numberOfRows += contractListOfRollovers.Count;

                        //numberOfRolloverRows = contractListOfRollovers.Count;

                        for (int portfolioGroupCount = 0;
                                portfolioGroupCount < optionSpreadManager.portfolioGroupAllocation.Length;
                                portfolioGroupCount++)
                        {

                            for (int contractCount = 0; contractCount < contractListOfRollovers.Count; contractCount++)
                            {
                                double defaultPrice = 0;
                                double impliedVol = 0;
                                if (contractListOfRollovers[contractCount].expression != null)
                                {
                                    defaultPrice = contractListOfRollovers[contractCount].expression.defaultPrice;
                                    impliedVol = contractListOfRollovers[contractCount].expression.impliedVol * 100;
                                }

                                int numOfContracts =
                                    contractListOfRollovers[contractCount].numberOfContracts
                                     * optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].multiple;

                                string acct = optionSpreadManager.selectAcct(
                                    //optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.exchangeSymbol,
                                        instrument.exchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount], true);

                                if (fillInGridDataRow(rowCount,
                                    optionSpreadManager.portfolioGroupAllocation[portfolioGroupCount].broker,
                                    acct,
                                    contractListOfRollovers[contractCount].contractType,
                                    contractListOfRollovers[contractCount].strikePrice,
                                    defaultPrice,
                                    impliedVol,
                                    contractListOfRollovers[contractCount].contractYear,
                                    contractListOfRollovers[contractCount].contractMonthInt,
                                    contractListOfRollovers[contractCount].optionYear,
                                    contractListOfRollovers[contractCount].optionMonthInt,
                                    numOfContracts,
                                    calcYearFrac(contractListOfRollovers[contractCount].expirationDate, DateTime.Now.Date)
                                        * 365
                                    ))
                                {
                                    rowCount++;
                                }
                            }
                        }
                    }

                    int expCount = 0;

                    while (expCount < optionSpreadExpressionList.Count())
                    {
                        if (optionSpreadExpressionList[expCount].optionExpressionType ==
                                OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                        {
                            double rfr = optionSpreadExpressionList[expCount].riskFreeRate
                                * 100;

                            riskFreeTextBox.Text = rfr.ToString();

                            break;
                        }

                        expCount++;
                    }

                    int futureIndexLeg = findFutureLeg();

                    if (futureIndexLeg < 0)
                    {
                        addFutureRow(null, null, 0);
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

        public void fillGridFromFCMModelDiff(
            OptionSpreadManager optionSpreadManager,
            List<ADMPositionImportWeb> admPositionImportWebListForCompare,
            List<OptionSpreadExpression> optionSpreadExpressionList,
            Instrument instrument, bool includeOrders)
        {
            //this.contractSummaryExpressionListIdx = contractSummaryExpressionListIdx;
            this.optionSpreadExpressionList = optionSpreadExpressionList;

            this.instrument = instrument;

            //this.chartType = chartType;

            StringBuilder title = new StringBuilder();
            title.Append(instrument.CQGsymbol);

            title.Append(" ");
            title.Append(optionSpreadManager.initializationParmsSaved.portfolioGroupName);
            title.Append(" ");

            title.Append(" @ FCM: ");

            //TODO: AUG 10 2015 - PortfolioGroupAllocation
            //title.Append(optionSpreadManager.initializationParmsSaved.FIX_Broker_18220);


            this.Text = title.ToString();



            int numberOfRows = 0;

            for (int count = 0; count < admPositionImportWebListForCompare.Count; count++)
            {
                if (admPositionImportWebListForCompare[count].instrumentArrayIdx
                    == instrument.idxOfInstrumentInList)
                {
                    numberOfRows++;
                }
            }


            //rRiskTextBox.Text = Math.Round(rRisk).ToString();


            countTextBox.Text = countOfTest.ToString();

            if (admPositionImportWebListForCompare.Count > 0 && numberOfRows > 0)
            {

                //gridViewSpreadGrid.RowCount = numberOfRows;

                int rowCount = 0;

                for (int contractCount = 0; contractCount < admPositionImportWebListForCompare.Count; contractCount++)
                {
                    if (admPositionImportWebListForCompare[contractCount].instrumentArrayIdx
                    == instrument.idxOfInstrumentInList)
                    {
                        int netLots = 0;
                        if (includeOrders)
                        {
                            netLots = Convert.ToInt32(admPositionImportWebListForCompare[contractCount].rebalanceLotsForPayoffWithOrders);
                        }
                        else
                        {
                            netLots = Convert.ToInt32(admPositionImportWebListForCompare[contractCount].rebalanceLotsForPayoffNoOrders);
                        }

                        OPTION_SPREAD_CONTRACT_TYPE callPutOrFuture;
                        double defaultPrice = 0;
                        double impliedVol = 0;
                        double yearFraction = 0;
                        double strikePrice = 0;
                        int contractYear;
                        int contractMonth;
                        int optionYear;
                        int optionMonth;

                        if (admPositionImportWebListForCompare[contractCount].optionSpreadExpression != null)
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.callPutOrFuture;

                            defaultPrice = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.defaultPrice;

                            impliedVol = admPositionImportWebListForCompare[contractCount]
                                .optionSpreadExpression.impliedVol * 100;

                            yearFraction = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.yearFraction
                                * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.strikePrice;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.futureContractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.futureContractMonthInt;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.optionMonthInt;
                        }
                        else if (admPositionImportWebListForCompare[contractCount].contractData != null &&
                            admPositionImportWebListForCompare[contractCount].contractData.optionSpreadExpression != null)
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.callPutOrFuture;

                            defaultPrice = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.defaultPrice;

                            impliedVol = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.impliedVol * 100;

                            yearFraction = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.yearFraction
                                * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.strikePrice;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.futureContractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.futureContractMonthInt;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.optionMonthInt;
                        }
                        else
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount].callPutOrFuture;

                            yearFraction = admPositionImportWebListForCompare[contractCount].yearFraction * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount].strikeInDecimal;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount].contractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount].contractMonth;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount].optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount].optionMonth;

                        }



                        //
                        //string acct = optionSpreadManager.selectAcct(
                        //            admPositionImportWebList[contractCount].instrument.exchangeSymbol,
                        //                optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation]);

                        //if (fillInGridDataRow(rowCount,
                        //    optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation].broker,
                        //    acct,
                        //

                        string acct = optionSpreadManager.selectAcct(
                                        instrument.exchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                            admPositionImportWebListForCompare[contractCount].acctGroup], true);

                        if (fillInGridDataRow(rowCount,
                            optionSpreadManager.portfolioGroupAllocation[admPositionImportWebListForCompare[contractCount].acctGroup].broker,
                            acct,
                            callPutOrFuture,
                            strikePrice,
                            defaultPrice,
                            impliedVol,
                            contractYear,
                            contractMonth,
                            optionYear,
                            optionMonth,
                            netLots,
                            yearFraction
                            ))
                        {
                            rowCount++;
                        }
                    }
                }



                int expCount = 0;

                while (expCount < optionSpreadExpressionList.Count())
                {
                    if (optionSpreadExpressionList[expCount].optionExpressionType ==
                            OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                    {
                        double rfr = optionSpreadExpressionList[expCount].riskFreeRate
                            * 100;

                        riskFreeTextBox.Text = rfr.ToString();

                        break;
                    }

                    expCount++;
                }

                int futureIndexLeg = findFutureLeg();

                if (futureIndexLeg < 0)
                {
                    addFutureRow(null, null, 0);
                }
            }

        }


        public void fillGridFromFCMModelNetDifference(
            OptionSpreadManager optionSpreadManager,
            List<ADMPositionImportWeb> admPositionImportWebListForCompare,
            List<OptionSpreadExpression> optionSpreadExpressionList,
            Instrument instrument, bool includeOrders, string titlePassed)
        {
            //this.contractSummaryExpressionListIdx = contractSummaryExpressionListIdx;
            this.optionSpreadExpressionList = optionSpreadExpressionList;

            this.instrument = instrument;

            //this.chartType = chartType;

            StringBuilder title = new StringBuilder();
            title.Append(instrument.CQGsymbol);

            title.Append(" ");
            title.Append(optionSpreadManager.initializationParmsSaved.portfolioGroupName);
            title.Append(" ");

            title.Append(titlePassed);

            //TODO: AUG 10 2015 - PortfolioGroupAllocation
            //title.Append(optionSpreadManager.initializationParmsSaved.FIX_Broker_18220);


            this.Text = title.ToString();



            int numberOfRows = 0;

            for (int count = 0; count < admPositionImportWebListForCompare.Count; count++)
            {
                if (admPositionImportWebListForCompare[count].instrumentArrayIdx
                    == instrument.idxOfInstrumentInList)
                {
                    numberOfRows++;
                }
            }


            //rRiskTextBox.Text = Math.Round(rRisk).ToString();


            countTextBox.Text = countOfTest.ToString();

            if (admPositionImportWebListForCompare.Count > 0 && numberOfRows > 0)
            {

                //gridViewSpreadGrid.RowCount = numberOfRows;

                int rowCount = 0;

                for (int contractCount = 0; contractCount < admPositionImportWebListForCompare.Count; contractCount++)
                {
                    if (admPositionImportWebListForCompare[contractCount].instrumentArrayIdx
                    == instrument.idxOfInstrumentInList)
                    {
                        int netLots = 0;
                        if (includeOrders)
                        {
                            netLots = -Convert.ToInt32(admPositionImportWebListForCompare[contractCount].rebalanceLotsForPayoffWithOrders);
                        }
                        else
                        {
                            netLots = -Convert.ToInt32(admPositionImportWebListForCompare[contractCount].rebalanceLotsForPayoffNoOrders);
                        }

                        OPTION_SPREAD_CONTRACT_TYPE callPutOrFuture;
                        double defaultPrice = 0;
                        double impliedVol = 0;
                        double yearFraction = 0;
                        double strikePrice = 0;
                        int contractYear;
                        int contractMonth;
                        int optionYear;
                        int optionMonth;

                        if (admPositionImportWebListForCompare[contractCount].optionSpreadExpression != null)
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.callPutOrFuture;

                            defaultPrice = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.defaultPrice;

                            impliedVol = admPositionImportWebListForCompare[contractCount]
                                .optionSpreadExpression.impliedVol * 100;

                            yearFraction = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.yearFraction
                                * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount].optionSpreadExpression.strikePrice;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.futureContractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.futureContractMonthInt;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount].optionSpreadExpression.optionMonthInt;
                        }
                        else if (admPositionImportWebListForCompare[contractCount].contractData != null &&
                            admPositionImportWebListForCompare[contractCount].contractData.optionSpreadExpression != null)
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.callPutOrFuture;

                            defaultPrice = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.defaultPrice;

                            impliedVol = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.impliedVol * 100;

                            yearFraction = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.yearFraction
                                * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.strikePrice;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.futureContractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.futureContractMonthInt;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount]
                                .contractData.optionSpreadExpression.optionMonthInt;
                        }
                        else
                        {
                            callPutOrFuture = admPositionImportWebListForCompare[contractCount].callPutOrFuture;

                            yearFraction = admPositionImportWebListForCompare[contractCount].yearFraction * 365;

                            strikePrice = admPositionImportWebListForCompare[contractCount].strikeInDecimal;

                            contractYear =
                                admPositionImportWebListForCompare[contractCount].contractYear;
                            contractMonth =
                                admPositionImportWebListForCompare[contractCount].contractMonth;
                            optionYear =
                                admPositionImportWebListForCompare[contractCount].optionYear;
                            optionMonth =
                                admPositionImportWebListForCompare[contractCount].optionMonth;

                        }



                        //
                        //string acct = optionSpreadManager.selectAcct(
                        //            admPositionImportWebList[contractCount].instrument.exchangeSymbol,
                        //                optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation]);

                        //if (fillInGridDataRow(rowCount,
                        //    optionSpreadManager.portfolioGroupAllocation[indexOfPortfolioAllocation].broker,
                        //    acct,
                        //

                        string acct = optionSpreadManager.selectAcct(
                                        instrument.exchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[
                                            admPositionImportWebListForCompare[contractCount].acctGroup], true);

                        if (fillInGridDataRow(rowCount,
                            optionSpreadManager.portfolioGroupAllocation[admPositionImportWebListForCompare[contractCount].acctGroup].broker,
                            acct,
                            callPutOrFuture,
                            strikePrice,
                            defaultPrice,
                            impliedVol,
                            contractYear,
                            contractMonth,
                            optionYear,
                            optionMonth,
                            netLots,
                            yearFraction
                            ))
                        {
                            rowCount++;
                        }
                    }
                }



                int expCount = 0;

                while (expCount < optionSpreadExpressionList.Count())
                {
                    if (optionSpreadExpressionList[expCount].optionExpressionType ==
                            OPTION_EXPRESSION_TYPES.OPTION_EXPRESSION_RISK_FREE_RATE)
                    {
                        double rfr = optionSpreadExpressionList[expCount].riskFreeRate
                            * 100;

                        riskFreeTextBox.Text = rfr.ToString();

                        break;
                    }

                    expCount++;
                }

                int futureIndexLeg = findFutureLeg();

                if (futureIndexLeg < 0)
                {
                    addFutureRow(null, null, 0);
                }
            }

        }


        private int getFutureExpressionIdx()
        {
            int futureExpCnt = 0;

            while (futureExpCnt < optionSpreadExpressionList.Count())
            {
                if (optionSpreadExpressionList[futureExpCnt].instrument != null
                    && instrument.idxOfInstrumentInList ==
                        optionSpreadExpressionList[futureExpCnt].instrument.idxOfInstrumentInList
                    &&
                    optionSpreadExpressionList[futureExpCnt].callPutOrFuture ==
                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    break;
                }

                futureExpCnt++;
            }

            if (futureExpCnt >= optionSpreadExpressionList.Count())
            {
                futureExpCnt--;
            }

            return futureExpCnt;
        }

        public void addFutureRow(string broker, string acct, int numberOfLots)
        {
            //int futureIndexLeg = findFutureLeg();

            //if (futureIndexLeg < 0)
            {


                int futureExpCnt = getFutureExpressionIdx();

                //while (futureExpCnt < optionSpreadExpressionList.Count())
                //{
                //    if (optionSpreadExpressionList[futureExpCnt].instrument != null
                //        && instrument.idxOfInstrumentInList ==
                //            optionSpreadExpressionList[futureExpCnt].instrument.idxOfInstrumentInList
                //        &&
                //        optionSpreadExpressionList[futureExpCnt].callPutOrFuture ==
                //            OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                //    {
                //        //futureAvgPrice = optionSpreadExpressionList[expCount].defaultPrice;

                //        break;
                //    }

                //    futureExpCnt++;
                //}

                //if (futureExpCnt >= optionSpreadExpressionList.Count())
                //{
                //    futureExpCnt--;
                //}

                int row = gridViewSpreadGrid.Rows.Add();

                addGridDeleteImg(row, (int)OPTION_PL_COLUMNS.DEL_ROW);

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value =
                    optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                                .ToString();


                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value = 0;



                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value =


                ConversionAndFormatting.convertToTickMovesString(
                        optionSpreadExpressionList[futureExpCnt].defaultPrice,
                        instrument.tickSize,
                        instrument.tickDisplay);

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value = 0;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value = 0;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value =
                        optionSpreadExpressionList[futureExpCnt].futureContractYear;
                //optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].futureContractYear;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value =
                        optionSpreadExpressionList[futureExpCnt].futureContractMonthInt;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.NET].Value = numberOfLots;


                if (broker != null)
                {
                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.BRKR].Value =
                            broker;
                }

                if (acct != null)
                {
                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.ACCT].Value =
                            acct;
                }

            }
        }

        public int findFutureLeg()
        {
            int legCount = 0;
            int futureIdx = -1;

            while (legCount < gridViewSpreadGrid.RowCount)
            {
                if (gridViewSpreadGrid.Rows[legCount]
                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                {
                    String contractType = gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                    {
                        futureIdx = legCount;
                        break;
                    }
                }

                legCount++;
            }

            return futureIdx;
        }

        public void fillChart()
        {
            Series serPrice = new Series();

            serPrice.ChartType = SeriesChartType.Line;

            serPrice.Name = "Spread PL";

            Series serPriceAtExp = new Series();

            serPriceAtExp.ChartType = SeriesChartType.Line;

            serPriceAtExp.Name = "Spread PL At Expiration";


            Series futurePriceSeries = new Series();

            futurePriceSeries.ChartType = SeriesChartType.Line;

            futurePriceSeries.Name = "Future Price";

            //Series rRiskSeries = null;

            Series deltaSeries = new Series();

            deltaSeries.ChartType = SeriesChartType.Line;

            deltaSeries.Name = "Delta";

            Series deltaSeriesAtExp = new Series();

            deltaSeriesAtExp.ChartType = SeriesChartType.Line;

            deltaSeriesAtExp.Name = "Delta At Exp";


            //if (chartType )
            {

                //rRiskSeries = new Series();

                //rRiskSeries.ChartType = SeriesChartType.Line;

                //rRiskSeries.Name = "R-Risk";

                rRiskTextBox.Text = Math.Round(rRisk).ToString();

                rRisk = Convert.ToDouble(rRiskTextBox.Text);
            }

            double maxTotal = double.NegativeInfinity;
            double minTotal = double.PositiveInfinity;

            countOfTest = Convert.ToInt16(countTextBox.Text);

            int legCount = 0;

            int futureIdx = findFutureLeg();

            //while (legCount < gridViewSpreadGrid.RowCount)
            //{
            //    if (gridViewSpreadGrid.Rows[legCount]
            //                .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value != null)
            //    {
            //        String contractType = gridViewSpreadGrid.Rows[legCount]
            //                    .Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value.ToString();

            //        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
            //                (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
            //        {
            //            futureIdx = legCount;
            //            break;
            //        }
            //    }

            //    legCount++;
            //}



            double futureAvgPrice = returnFuturesAveragePrice(futureIdx);

            

            //if (futureIdx >= 0)
            //{
            //    if (gridViewSpreadGrid.Rows[futureIdx]
            //                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value != null)
            //    {
            //        futureAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
            //                    gridViewSpreadGrid.Rows[futureIdx]
            //                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
            //                    instrument.tickSize, instrument.tickDisplay);
            //    }
            //    else
            //    {
            //        String caption = "FILL IN A PRICE FOR THE FUTURE CONTRACT";
            //        String message = "FILL IN A PRICE FOR THE FUTURE CONTRACT";
            //        MessageBoxButtons buttons = MessageBoxButtons.OK;
            //        System.Windows.Forms.DialogResult result;

            //        // Displays the MessageBox.
            //        result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{


            //    int expCount = 0;

            //    while (expCount < optionSpreadExpressionList.Count())
            //    {
            //        if (optionSpreadExpressionList[expCount].instrument != null
            //            && instrument.idxOfInstrumentInList ==
            //                optionSpreadExpressionList[expCount].instrument.idxOfInstrumentInList
            //            &&
            //            optionSpreadExpressionList[expCount].callPutOrFuture ==
            //                OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
            //        {
            //            futureAvgPrice = optionSpreadExpressionList[expCount].defaultPrice;

            //            break;
            //        }

            //        expCount++;
            //    }

            //}

            lblFuturePrice.Text = "Future: " +

            ConversionAndFormatting.convertToTickMovesString(
                                futureAvgPrice,
                                instrument.tickSize,
                                instrument.tickDisplay);


            double[] futurePriceArray = new double[countOfTest * 2 + 1];



            double startOfTest =
                futureAvgPrice
                - countOfTest *
                instrument.tickSize;


            if (startOfTest < 0)
            {
                startOfTest = 0;
            }

            for (int count = 0; count < futurePriceArray.Length; count++)
            {
                futurePriceArray[count] = startOfTest +
                    count *
                    instrument.tickSize;

            }


            //double[,] legPls = new double[gridViewSpreadGrid.RowCount, futurePriceArray.Length];

            //double[] spreadPl = new double[futurePriceArray.Length];
            //double[] spreadPlAtExp = new double[futurePriceArray.Length];

            //double[] deltaTotalSeries = new double[futurePriceArray.Length];
            //double[] deltaTotalAtExpSeries = new double[futurePriceArray.Length];

            double riskFreeRate = Convert.ToDouble(riskFreeTextBox.Text) / 100;

            for (int futurePointCount = 0; futurePointCount < futurePriceArray.Length; futurePointCount++)
            {
                double plTotal = 0;
                double plAtExpTotal = 0;

                double deltaTotal = 0;
                double deltaTotalAtExp = 0;


                legCount = 0;

                while (legCount < gridViewSpreadGrid.RowCount)
                {
                    bool continueThisLeg = true;

                    if (brokerAccountChosen < optionSpreadManager.portfolioGroupAllocation.Length)
                    {
                        if ((gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value != null
                            &&
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value != null)
                            &&
                            (
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString().CompareTo(
                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].broker) != 0
                            ||
                            gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString().CompareTo(
                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].account) != 0
                            ))
                        {
                            continueThisLeg = false;
                        }
                    }



                    int cellCount = 0;

                    while (continueThisLeg && cellCount <= (int)OPTION_PL_COLUMNS.DEL_ROW)  //gridViewSpreadGrid.Rows[legCount].Cells.Count)
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
                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                        //double strike = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                        //                        .Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);

                        double strike =
                            //ConversionAndFormatting.convertToTickMovesDouble(
                        Convert.ToDouble(gridViewSpreadGrid.Rows[legCount].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);
                        //instrument.optionStrikeIncrement,
                        //instrument.optionStrikeDisplay
                        //instrument.tickSize, instrument.tickDisplay
                        //);

                        double daysToExp = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value);
                        daysToExp /= 365;

                        double implVol = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value);
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
                                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                instrument.tickSize, instrument.tickDisplay);
                        }
                        else
                        {
                            double tempAvgPrice;

                            if (instrument.secondaryOptionTickSizeRule > 0)
                            {
                                tempAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                    instrument.secondaryOptionTickSize, instrument.secondaryoptiontickdisplay);
                            }
                            else
                            {
                                tempAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                    instrument.optionTickSize, instrument.optionTickDisplay);
                            }

                            double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                                tempAvgPrice,
                                instrument.optionTickSize,
                                instrument.secondaryOptionTickSize,
                                instrument.secondaryOptionTickSizeRule);

                            double tickDisplay = OptionSpreadManager.chooseOptionTickSize(
                                tempAvgPrice,
                                instrument.optionTickDisplay,
                                instrument.secondaryoptiontickdisplay,
                                instrument.secondaryOptionTickSizeRule);


                            ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                    optionTickSize, tickDisplay);


                            //if (instrument.secondaryOptionTickSizeRule > 0)
                            //{
                            //    avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                            //        gridViewSpreadGrid.Rows[legCount]
                            //            .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                            //        instrument.secondaryOptionTickSize, instrument.optionTickDisplay);
                            //}
                            //else
                            //{
                            //    avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                            //        gridViewSpreadGrid.Rows[legCount]
                            //            .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                            //        instrument.optionTickSize, instrument.optionTickDisplay);
                            //}
                        }

                        char typeSymbol;
                        bool run = true;

                        double price;
                        double priceAtExp;

                        double tickSize;
                        double tickValue;

                        double delta;
                        double deltaAtExp;


                        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                        {
                            typeSymbol = 'C';

                            price = OptionCalcs.blackScholes(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         daysToExp, riskFreeRate,
                                        implVol);

                            priceAtExp = OptionCalcs.blackScholes(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         0, riskFreeRate,
                                        implVol);

                            tickSize = instrument.optionTickSize;
                            tickValue = instrument.optionTickValue;

                            delta = OptionCalcs.gDelta(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         daysToExp, riskFreeRate, 0,
                                        implVol);

                            deltaAtExp = OptionCalcs.gDelta(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         0, riskFreeRate, 0,
                                        implVol);
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

                            priceAtExp = OptionCalcs.blackScholes(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         0, riskFreeRate,
                                        implVol);

                            tickSize = instrument.optionTickSize;
                            tickValue = instrument.optionTickValue;

                            delta = OptionCalcs.gDelta(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         daysToExp, riskFreeRate, 0,
                                        implVol);

                            deltaAtExp = OptionCalcs.gDelta(typeSymbol,
                                futurePriceArray[futurePointCount],
                                        strike,
                                         0, riskFreeRate, 0,
                                        implVol);
                        }
                        else
                        {
                            typeSymbol = 'F';

                            price = futurePriceArray[futurePointCount];
                            priceAtExp = futurePriceArray[futurePointCount];

                            tickSize = instrument.tickSize;
                            tickValue = instrument.tickValue;

                            delta = 1;
                            deltaAtExp = 1;
                        }

                        if (run)
                        {
                            double pl =
                                    numOfContracts *
                                    (price - avgPrice)
                                        / tickSize
                                        * tickValue;

                            plTotal += pl;

                            pl =
                                    numOfContracts *
                                    (priceAtExp - avgPrice)
                                        / tickSize
                                        * tickValue;

                            plAtExpTotal += pl;

                            deltaTotal += numOfContracts * delta * 100;

                            deltaTotalAtExp += numOfContracts * deltaAtExp * 100;

                        }
                    }
                    legCount++;
                }

                if (plTotal > maxTotal)
                {
                    maxTotal = plTotal;
                }

                if (plTotal < minTotal)
                {
                    minTotal = plTotal;
                }

                //spreadPl[futurePointCount] = plTotal;

                //spreadPlAtExp[futurePointCount] = plAtExpTotal;



                DataPoint dp = new DataPoint();

                dp.SetValueXY(
                    //ConversionAndFormatting.convertToTickMovesString(
                    //        futurePriceArray[futurePointCount],
                    //        instrument.tickSize, instrument.tickDisplay),
                    futurePriceArray[futurePointCount],
                    plTotal);

                serPrice.Points.Add(dp);

                DataPoint dpAtExp = new DataPoint();

                dpAtExp.SetValueXY(
                    futurePriceArray[futurePointCount],
                    plAtExpTotal);

                serPriceAtExp.Points.Add(dpAtExp);


                //deltaTotalSeries[futurePointCount] = plTotal;

                DataPoint deltaP = new DataPoint();

                deltaP.SetValueXY(
                    //ConversionAndFormatting.convertToTickMovesString(
                    //        futurePriceArray[futurePointCount],
                    //        instrument.tickSize, instrument.tickDisplay),
                    futurePriceArray[futurePointCount],
                    deltaTotal);

                deltaSeries.Points.Add(deltaP);


                DataPoint deltaPAtExp = new DataPoint();

                deltaPAtExp.SetValueXY(
                    //ConversionAndFormatting.convertToTickMovesString(
                    //        futurePriceArray[futurePointCount],
                    //        instrument.tickSize, instrument.tickDisplay),
                    futurePriceArray[futurePointCount],
                    deltaTotalAtExp);

                deltaSeriesAtExp.Points.Add(deltaPAtExp);

            }

            //if (!chartType)
            //{
            //    DataPoint rRiskPt = new DataPoint();
            //    double rRiskOffset = spreadPl[countOfTest + 1] - rRisk;
            //    rRiskPt.SetValueXY(futurePriceArray[0], rRiskOffset);
            //    rRiskSeries.Points.Add(rRiskPt);

            //    rRiskPt = new DataPoint();
            //    rRiskPt.SetValueXY(futurePriceArray[futurePriceArray.Length - 1], rRiskOffset);
            //    rRiskSeries.Points.Add(rRiskPt);
            //}

            DataPoint futPt = new DataPoint();
            futPt.SetValueXY(
                //ConversionAndFormatting.convertToTickMovesString(
                //        futureAvgPrice,
                //        instrument.tickSize, instrument.tickDisplay),
                    futureAvgPrice,
                            minTotal);
            futurePriceSeries.Points.Add(futPt);

            futPt = new DataPoint();
            futPt.SetValueXY(
                //ConversionAndFormatting.convertToTickMovesString(
                //        futureAvgPrice,
                //        instrument.tickSize, instrument.tickDisplay),
                    futureAvgPrice,
                            maxTotal);
            futurePriceSeries.Points.Add(futPt);

            chart1.Series.Clear();
            chart1.ResetAutoValues();

            chart1.Series.Add(serPrice);
            chart1.Series.Add(serPriceAtExp);

            chart1.Series.Add(futurePriceSeries);


            //if (chartType == PAYOFF_CHART_TYPE.CONTRACT_SUMMARY_PAYOFF)
            //{
            //    chart1.Series.Add(rRiskSeries);
            //}

            chart2.Series.Clear();
            chart2.ResetAutoValues();
            chart2.Series.Add(deltaSeries);
            chart2.Series.Add(deltaSeriesAtExp);
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
                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                        {
                            gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value
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
                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
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
                int row = gridViewSpreadGrid.Rows.Add();

                addGridDeleteImg(row, (int)OPTION_PL_COLUMNS.DEL_ROW);
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

            //hideNet0Rows(true);
        }

        private void gridViewSpreadGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {

                return;

            }

            if (e.ColumnIndex == (int)OPTION_PL_COLUMNS.DEL_ROW)
            {
                gridViewSpreadGrid.Cursor = Cursors.Hand;
            }
            else
            {
                gridViewSpreadGrid.Cursor = Cursors.Default;

            }
        }

        private void gridViewSpreadGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {

                return;

            }

            if (e.ColumnIndex == (int)OPTION_PL_COLUMNS.DEL_ROW)
            {

                gridViewSpreadGrid.Rows.RemoveAt(e.RowIndex);

            }
        }

        public void updateInitialMargin(double value)
        {
            ThreadSafeUpdateMargin d = new ThreadSafeUpdateMargin(updateInitialMarginThreadSafe);

            this.Invoke(d, value);
        }

        private void updateInitialMarginThreadSafe(double value)
        {
            lblInitialMargin.Text = value.ToString();
        }

        public void updateMaintenanceMargin(double value)
        {
            ThreadSafeUpdateMargin d = new ThreadSafeUpdateMargin(updateMaintenanceMarginThreadSafe);

            this.Invoke(d, value);
        }

        private void updateMaintenanceMarginThreadSafe(double value)
        {
            lblMaintenanceMargin.Text = value.ToString();
        }

        private void btnMargin_Click(object sender, EventArgs e)
        {
            //Type optionPLColTypes = typeof(OPTION_PL_COLUMNS);
            //Array optionPLColTypesArray = Enum.GetNames(optionPLColTypes);

            //for (int i = 0; i < optionPLColTypesArray.Length; i++)
            //{
            //    sb.Clear();

            //    sb.Append(optionPLColTypesArray.GetValue(i).ToString());

            //    gridViewSpreadGrid
            //        .Columns[i]
            //        .HeaderCell.Value = sb.ToString().Replace('_', ' ');
            //}

            int futureIdx = -1;
            int legCount = 0;

            while (legCount < gridViewSpreadGrid.RowCount)
            {
                if (gridViewSpreadGrid.Rows[legCount]
                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                {
                    String contractType = gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                    {
                        futureIdx = legCount;
                        break;
                    }
                }

                legCount++;
            }

            if (futureIdx > -1)
            {

                List<OptionChartMargin> contractList = new List<OptionChartMargin>();

                for (int i = 0; i < gridViewSpreadGrid.Rows.Count; i++)
                {
                    bool continueToCallMargin = true;

                    int cellCnt = 0;

                    while (cellCnt < (int)OPTION_PL_COLUMNS.SEL_ROW)
                    {
                        if (gridViewSpreadGrid.Rows[i]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value == null)
                        {
                            continueToCallMargin = false;
                            break;
                        }

                        cellCnt++;
                    }

                    if (continueToCallMargin)
                    {

                        OptionChartMargin ocm = new OptionChartMargin();



                        ocm.strikePrice =
                            Convert.ToDouble(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);

                        ocm.intrument = instrument;

                        ocm.numberOfContracts =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.NET].Value);


                        //        double daysToExp = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                        //                                .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value);
                        //        daysToExp /= 365;

                        //        double implVol = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                        //                                .Cells[(int)OPTION_PL_COLUMNS.IMPLIED_VOL].Value);
                        //        //implVol = 15;
                        //        implVol /= 100;

                        //        double numOfContracts = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                        //                                .Cells[(int)OPTION_PL_COLUMNS.NET].Value);


                        String contractType = gridViewSpreadGrid.Rows[i]
                                    .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                        {
                            ocm.contractType = OPTION_SPREAD_CONTRACT_TYPE.FUTURE;

                            ocm.contractYear =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                            ocm.contractMonthInt =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);
                        }
                        else
                        {
                            ocm.contractYear =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[futureIdx].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                            ocm.contractMonthInt =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[futureIdx].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                            ocm.optionYear =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                            ocm.optionMonthInt =
                                Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                            if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                               (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                            {
                                ocm.contractType = OPTION_SPREAD_CONTRACT_TYPE.CALL;
                            }
                            else if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
                            {
                                ocm.contractType = OPTION_SPREAD_CONTRACT_TYPE.PUT;
                            }
                        }


                        //ocm.contractType gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.CONTRACT_TYPE].Value;


                        contractList.Add(ocm);
                    }
                }

                CMEMarginCallSingleInstrument cmeMarginCall = new CMEMarginCallSingleInstrument(contractList,
                   instrument, this, null, false);

                cmeMarginCall.generateMarginRequest();
            }


        }

        private void btnStageEntry_Click(object sender, EventArgs e)
        {
            stageOrders(false, false);
        }

        private void stageOrders(bool reverseLots, bool sendSelectedRows)
        {
            if (optionSpreadManager.stageOrdersLibrary != null)
            {
                List<StageOrdersToTTWPFLibrary.Model.OrderModel> contractListToStage = new List<StageOrdersToTTWPFLibrary.Model.OrderModel>();

                int futureIdx = -1;
                int legCount = 0;

                while (legCount < gridViewSpreadGrid.RowCount)
                {
                    if (gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                    {
                        String contractType = gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                        {
                            futureIdx = legCount;
                            break;
                        }
                    }

                    legCount++;
                }

                if (futureIdx > -1)
                {

                    {
                        for (int rowIdx = 0; rowIdx < gridViewSpreadGrid.Rows.Count; rowIdx++)
                        {
                            if (!sendSelectedRows
                                ||
                                (sendSelectedRows &&
                                Convert.ToBoolean(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.SEL_ROW].Value)))
                            {

                                int stageOrderLots = Convert.ToInt16(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                                if (reverseLots)
                                {
                                    stageOrderLots = -1 * stageOrderLots;
                                }

                                if (stageOrderLots != 0
                                    && gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.BRKR].Value != null
                                    && gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.ACCT].Value != null)
                                {
                                    bool continueToSubmitThisLeg = true;

                                    if (brokerAccountChosen < optionSpreadManager.portfolioGroupAllocation.Length)
                                    {
                                        if (gridViewSpreadGrid.Rows[rowIdx]
                                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString().CompareTo(
                                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].broker) != 0
                                            ||
                                            gridViewSpreadGrid.Rows[rowIdx]
                                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString().CompareTo(
                                            optionSpreadManager.portfolioGroupAllocation[brokerAccountChosen].account) != 0
                                            )
                                        {
                                            continueToSubmitThisLeg = false;
                                        }
                                    }

                                    if (continueToSubmitThisLeg)
                                    {
                                        contractListToStage.Add(getOrder(futureIdx, rowIdx,
                                            stageOrderLots));
                                    }

                                }
                            }
                        }

                    }

                }

                if (contractListToStage.Count > 0)
                {
                    optionSpreadManager.stageOrdersLibrary.stageOrders(contractListToStage);
                }

            }
        }


        private StageOrdersToTTWPFLibrary.Model.OrderModel getOrder(int futureIdx, int rowIdx,
            int stageOrderLots)
        {

            //for (int i = 0; i < gridViewSpreadGrid.Rows.Count; i++)
            {
                //int stageOrderLots = Convert.ToInt16(gridViewSpreadGrid.Rows[i].Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                //if (stageOrderLots != 0)
                {

                    StageOrdersToTTWPFLibrary.Model.OrderModel orderModel =
                                new StageOrdersToTTWPFLibrary.Model.OrderModel();

                    orderModel.cqgSymbol = "";


                    orderModel.underlyingExchange = instrument.tradingTechnologiesExchange;

                    orderModel.underlyingGateway = instrument.tradingTechnologiesGateway;



                    orderModel.orderQty = Math.Abs(stageOrderLots);

                    //orderModel.underlyingExchangeSymbol = instrument.exchangeSymbolTT;

                    String contractType = gridViewSpreadGrid.Rows[rowIdx]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                        (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                    {
                        orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.FUTURE;

                        orderModel.contractMonthint =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                        orderModel.contractYear =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                        orderModel.maturityMonthYear =
                                    new DateTime(orderModel.contractYear, orderModel.contractMonthint, 1)
                                        .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                        orderModel.underlyingExchangeSymbol = instrument.exchangeSymbolTT;

                    }
                    else
                    {
                        orderModel.securityType = StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.OPTION;

                        orderModel.underlyingExchangeSymbol =
                                instrument.optionExchangeSymbolTT;

                        orderModel.contractYear =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[futureIdx].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                        orderModel.contractMonthint =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[futureIdx].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                        orderModel.optionYear =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                        orderModel.optionMonthInt =
                            Convert.ToInt16(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                        orderModel.maturityMonthYear =
                            new DateTime(orderModel.optionYear, orderModel.optionMonthInt, 1)
                                .ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);

                        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                           (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                        {
                            orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.CALL;
                        }
                        else if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
                        {
                            orderModel.optionType = StageOrdersToTTWPFLibrary.Enums.OPTION_TYPE.PUT;
                        }

                        double sd = instrument.optionStrikeDisplayTT;

                        orderModel.optionStrikePrice =
                                (decimal)ConversionAndFormatting.convertToStrikeForTT(
                                Convert.ToDouble(gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value),
                                instrument.optionStrikeIncrement,
                                sd,
                                instrument.idInstrument);
                    }

                    if (stageOrderLots > 0)
                    {
                        orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Buy;
                    }
                    else
                    {
                        orderModel.side = StageOrdersToTTWPFLibrary.Enums.Side.Sell;
                    }

                    orderModel.stagedOrderMessage = "SIGNAL TRIGGER ORDER FROM PAYOFF";

                    //TODO: AUG 10 2015 - PortfolioGroupAllocation
                    //orderModel.broker_18220 = optionSpreadManager.initializationParmsSaved.FIX_Broker_18220;


                    orderModel.orderPrice = findLimitPrice(
                                    orderModel.side,
                                    orderModel.securityType,
                                    gridViewSpreadGrid.Rows[rowIdx].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                    instrument);

                    orderModel.orderPlacementType = optionSpreadManager.initializationParmsSaved.FIX_OrderPlacementType;

                    //TODO
                    //orderModel.acct =
                    //    optionSpreadManager.selectAcct(orderModel.underlyingExchangeSymbol);


                    string brkrFromGrid = gridViewSpreadGrid.Rows[rowIdx]
                                                    .Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString();

                    string acctFromGrid = gridViewSpreadGrid.Rows[rowIdx]
                                    .Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString();

                    string brkAndAcct = brkrFromGrid.Trim() + acctFromGrid.Trim();

                    int brokerAcctIdx = 0;

                    if (brokerAcctDictionary.ContainsKey(brkAndAcct))
                    {
                        brokerAcctIdx = brokerAcctDictionary[brkAndAcct];
                    }

                    string acct =
                        optionSpreadManager.selectAcct(instrument.exchangeSymbol,
                        optionSpreadManager.portfolioGroupAllocation[brokerAcctIdx], false);


                    orderModel.broker_18220 = brkrFromGrid;

                    orderModel.acct = acct;


                    string instrumentSpecificFieldKey = optionSpreadManager.getInstrumentSpecificFieldKey(orderModel);


                    if (optionSpreadManager.instrumentSpecificFIXFieldHashSet.ContainsKey(instrumentSpecificFieldKey))
                    {
                        InstrumentSpecificFIXFields instrumentSpecificFIXFields
                            = optionSpreadManager.instrumentSpecificFIXFieldHashSet[instrumentSpecificFieldKey];

                        orderModel.TAG47_Rule80A.useTag = true;
                        orderModel.TAG47_Rule80A.tagCharValue = instrumentSpecificFIXFields.TAG_47_Rule80A;

                        orderModel.TAG204_CustomerOrFirm.useTag = true;
                        orderModel.TAG204_CustomerOrFirm.tagIntValue = instrumentSpecificFIXFields.TAG_204_CustomerOrFirm;

                        orderModel.TAG18205_TTAccountType.useTag = true;
                        orderModel.TAG18205_TTAccountType.tagStringValue = instrumentSpecificFIXFields.TAG_18205_TTAccountType;

                        orderModel.TAG440_ClearingAccount.useTag = true;
                        orderModel.TAG440_ClearingAccount.tagStringValue = instrumentSpecificFIXFields.TAG_440_ClearingAccount;

                        orderModel.TAG16102_FFT2.useTag = true;
                        orderModel.TAG16102_FFT2.tagStringValue = orderModel.acct;
                    }


                    return orderModel;

                    //contractListToStage.Add(orderModel);

                }

            }



        }

        private decimal findLimitPrice(StageOrdersToTTWPFLibrary.Enums.Side side,
            StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE securityType,
            String avgPriceAsString, Instrument instrument)
        {

            decimal priceToReturn = 0;
            double avgPrice = 0;
            double offsetPrice = 0;
            int tickOffset = Convert.ToInt16(tickOffsetBox.Text);

            double tickSize = 0;
            double tickDisplay = 0;

            //double tickDisplayTT_multiplier = 1;

            bool isOptionContract = true;

            if (securityType == StageOrdersToTTWPFLibrary.Enums.SECURITY_TYPE.FUTURE)
            {
                tickSize = instrument.tickSize;
                tickDisplay = instrument.tickDisplay;

                isOptionContract = false;

                //tickDisplayTT_multiplier = instrument.tickDisplayTT;
            }
            else
            {
                double tempAvgPrice;

                if (instrument.secondaryOptionTickSizeRule > 0)
                {
                    tempAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                        avgPriceAsString,
                        instrument.secondaryOptionTickSize, instrument.secondaryoptiontickdisplay);
                }
                else
                {
                    tempAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                        avgPriceAsString,
                        instrument.optionTickSize, instrument.optionTickDisplay);
                }

                //double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                //    tempAvgPrice,
                //    instrument.optionTickSize,
                //    instrument.secondaryOptionTickSize,
                //    instrument.secondaryOptionTickSizeRule);




                tickSize = OptionSpreadManager.chooseOptionTickSize(
                    tempAvgPrice,
                    instrument.optionTickSize,
                    instrument.secondaryOptionTickSize,
                    instrument.secondaryOptionTickSizeRule);

                tickDisplay = OptionSpreadManager.chooseOptionTickSize(
                    tempAvgPrice,
                    instrument.optionTickDisplay,
                    instrument.secondaryoptiontickdisplay,
                    instrument.secondaryOptionTickSizeRule);
                    

                //tickDisplayTT_multiplier = instrument.optionTickDisplayTT;
            }

            avgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                avgPriceAsString,
                                tickSize, tickDisplay);

            if (side == StageOrdersToTTWPFLibrary.Enums.Side.Buy)
            {
                offsetPrice = avgPrice - tickOffset * tickSize;

            }
            else
            {
                offsetPrice = avgPrice + tickOffset * tickSize;
            }

            priceToReturn =
                    Convert.ToDecimal(ConversionAndFormatting.convertToOrderPriceForTT(
                        offsetPrice,
                        tickSize,
                        tickDisplay,
                        instrument.idInstrument, isOptionContract));

            if (priceToReturn < 0)
            {
                priceToReturn = 0;
            }

            return priceToReturn;

        }

        private void btnStageEntrySelected_Click(object sender, EventArgs e)
        {
            stageOrders(false, true);
        }

        private void btnStageLiquidate_Click(object sender, EventArgs e)
        {
            stageOrders(true, false);
        }

        private void btnStageLiquidateSelected_Click(object sender, EventArgs e)
        {
            stageOrders(true, true);
        }

        private void addFutureRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFutureRow(null, null, 0);
        }

        private int clearGridDataRowContractsAndReturnLots(
            string broker, string acct,
            string contractType,
            double strikePrice,
            int futureContractYear, int futureContractMonth,
            int optionYear, int optionMonth)
        {
            int returnNumberOfLots = 0;

            int checkRow = 0;
            //bool alreadyIn = false;
            while (checkRow < gridViewSpreadGrid.Rows.Count)
            {
                int colCheck = 0;
                bool continueToCheckRow = true;
                while (colCheck <= (int)OPTION_PL_COLUMNS.DEL_ROW)
                {
                    if (gridViewSpreadGrid.Rows[checkRow].Cells[colCheck].Value == null)
                    {
                        continueToCheckRow = false;
                        break;
                    }

                    colCheck++;
                }


                if (continueToCheckRow
                        && gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value
                        .ToString().CompareTo(contractType) == 0)
                {

                    bool brokerAcctMatch = false;

                    if (gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString()
                        .CompareTo(broker) == 0
                        &&
                        gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString()
                        .CompareTo(acct) == 0)
                    {
                        brokerAcctMatch = true;
                    }

                    if (contractType.CompareTo(
                        optionArrayTypes.optionSpreadContractTypesArray.GetValue((int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                            .ToString()) == 0)
                                                
                    {
                        if (brokerAcctMatch
                            &&
                            Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value) ==
                            futureContractYear

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value) ==
                            futureContractMonth
                            )
                        {
                            returnNumberOfLots = Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                            gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value = 0;
                                //Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value)
                                //    + numberOfContracts;

                            //alreadyIn = true;
                            break;
                        }
                    }
                    else
                    {
                        if (brokerAcctMatch
                            &&
                            Convert.ToDouble(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value) ==
                            strikePrice

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value) ==
                            optionYear

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value) ==
                            optionMonth
                            )
                        {
                            returnNumberOfLots = Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                            gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value = 0;

                            //gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                            //    Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value)
                            //        + numberOfContracts;

                            //alreadyIn = true;
                            break;
                        }
                    }

                }

                checkRow++;
            }

            return returnNumberOfLots;
        }

        private bool fillInGridDataRow(int row,
            string broker, string acct,
            OPTION_SPREAD_CONTRACT_TYPE osct,
            double strikePrice, double defaultPrice,
            double impliedVol, int futureContractYear, int futureContractMonth,
            int optionYear, int optionMonth,
            int numberOfContracts, double daysToExp)
        {
            int checkRow = 0;
            bool alreadyIn = false;
            while (checkRow < gridViewSpreadGrid.Rows.Count)
            {
                int colCheck = 0;
                bool continueToCheckRow = true;
                while (colCheck <= (int)OPTION_PL_COLUMNS.DEL_ROW)
                {
                    if (gridViewSpreadGrid.Rows[checkRow].Cells[colCheck].Value == null)
                    {
                        continueToCheckRow = false;
                        break;
                    }

                    colCheck++;
                }


                if (continueToCheckRow
                        && gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value
                        .ToString().CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue((int)osct)
                            .ToString()) == 0)
                {

                    bool brokerAcctMatch = false;

                    if (gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.BRKR].Value.ToString()
                        .CompareTo(broker) == 0
                        &&
                        gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.ACCT].Value.ToString()
                        .CompareTo(acct) == 0)
                    {
                        brokerAcctMatch = true;
                    }

                    if (osct == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        if (brokerAcctMatch
                            &&
                            Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value) ==
                            futureContractYear

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value) ==
                            futureContractMonth
                            )
                        {
                            gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                                Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value)
                                    + numberOfContracts;

                            alreadyIn = true;
                            break;
                        }
                    }
                    else
                    {
                        if (brokerAcctMatch
                            &&
                            Convert.ToDouble(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value) ==
                            strikePrice

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value) ==
                            optionYear

                        && Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value) ==
                            optionMonth
                            )
                        {
                            gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                                Convert.ToInt32(gridViewSpreadGrid.Rows[checkRow].Cells[(int)OPTION_PL_COLUMNS.NET].Value)
                                    + numberOfContracts;

                            alreadyIn = true;
                            break;
                        }
                    }

                }

                checkRow++;
            }

            if (!alreadyIn)
            {
                gridViewSpreadGrid.Rows.Add();

                addGridDeleteImg(row, (int)OPTION_PL_COLUMNS.DEL_ROW);

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.BRKR].Value = broker;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.ACCT].Value = acct;

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value =
                    optionArrayTypes.optionSpreadContractTypesArray.GetValue((int)osct)
                            .ToString();

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value =
                    strikePrice;

                if (osct == OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                {
                    if (instrument != null)
                    {

                        //avgPrice = contractListOfRollovers[contractCount].expression.defaultPrice;

                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value =
                            ConversionAndFormatting.convertToTickMovesString(
                            defaultPrice,
                            instrument.tickSize,
                            instrument.tickDisplay);



                    }
                    else
                    {
                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value = 0;

                    }

                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value = 0;

                    //ConversionAndFormatting.convertToTickMovesString(
                    //        avgPrice,
                    //        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.tickSize,
                    //        optionSpreadExpressionList[contractSummaryExpressionListIdx[contractCount]].instrument.tickDisplay);

                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value =
                        futureContractYear;

                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value =
                        futureContractMonth;
                }
                else
                {

                    if (instrument != null)
                    {
                        
                        double optionTickSize = OptionSpreadManager.chooseOptionTickSize(
                            defaultPrice,
                            instrument.optionTickSize,
                            instrument.secondaryOptionTickSize,
                            instrument.secondaryOptionTickSizeRule);

                        double tickDisplay = OptionSpreadManager.chooseOptionTickSize(
                            defaultPrice,
                            instrument.optionTickDisplay,
                            instrument.secondaryoptiontickdisplay,
                            instrument.secondaryOptionTickSizeRule);

                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value =
                                ConversionAndFormatting.convertToTickMovesString(
                                defaultPrice,
                                optionTickSize,
                                tickDisplay);

                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value =
                            impliedVol;
                    }
                    else
                    {
                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value = 0;

                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value = 0;
                    }


                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.YEAR].Value =
                        optionYear;

                    gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value =
                        optionMonth;
                }

                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.NET].Value =
                    numberOfContracts;



                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value =
                    daysToExp;
            }

            //hideNet0Rows(true);

            return !alreadyIn;

        }



        private void addOptionWing(OPTION_SPREAD_CONTRACT_TYPE contractTypeWing)
        {
            //#if DEBUG
            try
            //#endif
            {

                int futureIdx = -1;
                int legCount = 0;

                double optionImplVol = 0;
                double optionCount = 0;

                double daysToExpOfOption = 0;
                bool daysToExpFilled = false;

                int optionYear = 0;
                int optionMonthInt = 0;

                DataGridViewSelectedRowCollection selectedRows = gridViewSpreadGrid.SelectedRows;

                bool foundImplVol = false;

                if (selectedRows.Count > 0)
                {

                    String contractType = selectedRows[0]
                               .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0
                        || contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                            (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0
                        )
                    {
                        if (selectedRows[0].Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value != null)
                        {
                            optionImplVol = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value);

                            foundImplVol = true;
                        }

                        if (selectedRows[0].Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value != null)
                        {
                            daysToExpOfOption = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value) / 365;

                            optionYear = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                            optionMonthInt = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                            daysToExpFilled = true;
                        }

                    }
                }


                while (legCount < gridViewSpreadGrid.RowCount)
                {
                    if (gridViewSpreadGrid.Rows[legCount]
                                .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                    {
                        String contractType = gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                        if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                        {
                            futureIdx = legCount;
                            //break;
                        }
                        else
                        {
                            if (!foundImplVol)
                            {
                                if (gridViewSpreadGrid.Rows[legCount]
                                    .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value != null)
                                {
                                    optionImplVol += Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                            .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value);

                                    optionCount++;
                                }
                            }

                            if (!daysToExpFilled
                                && gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value != null)
                            {
                                daysToExpOfOption = Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value) / 365;

                                optionYear = Convert.ToInt16(gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                                optionMonthInt = Convert.ToInt16(gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                                daysToExpFilled = true;
                            }
                        }
                    }

                    legCount++;
                }

                if (!foundImplVol)
                {
                    if (optionCount > 0)
                    {
                        optionImplVol /= optionCount; 
                    }
                }

                optionImplVol /= 100;


                if (futureIdx > -1)
                {
                    if (gridViewSpreadGrid.Rows[futureIdx]
                                .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value != null)
                    {

                        double futureClose = ConversionAndFormatting.convertToTickMovesDouble(
                                    gridViewSpreadGrid.Rows[futureIdx]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                    instrument.tickSize, instrument.tickDisplay);


                        double futurePriceFactorForAllLegs = futureClose / instrument.optionStrikeIncrement;

                        double futurePriceFactor = optionSpreadManager.roundPriceForCallOrPut(
                                            OPTION_SPREAD_CONTRACT_TYPE.PUT,
                                            futurePriceFactorForAllLegs, instrument.optionStrikeIncrement);

                        int refStartOfFuturePriceCount = -TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2;

                        int strikeCount = 0;

                        int strikeCountLimit = 20;

                        if (priceLimitTickTextBox.Text != null)
                        {
                            strikeCountLimit = Convert.ToInt16(strikeIncrLimitTextBox.Text);
                        }

                        while (strikeCount <= strikeCountLimit)
                        {

                            char contractCallOrPutChar = 'C';
                            double strikeDirection = 1;

                            if (contractTypeWing == OPTION_SPREAD_CONTRACT_TYPE.PUT)
                            {
                                contractCallOrPutChar = 'P';
                                strikeDirection = -1;
                            }



                            double tempFuturePriceFactor = futurePriceFactor
                                        + (refStartOfFuturePriceCount * instrument.optionStrikeIncrement);

                            double strikeTest =
                                strikeDirection * strikeCount * instrument.optionStrikeIncrement
                                    + tempFuturePriceFactor;


                            double rfr = Convert.ToDouble(riskFreeTextBox.Text) / 100;



                            double price = OptionCalcs.blackScholes(contractCallOrPutChar,
                                futureClose,
                                        strikeTest,
                                        daysToExpOfOption, rfr,
                                        optionImplVol);

                            //TSErrorCatch.debugWriteOut(price + " priceinstrument.optionTickSize " + price / instrument.optionTickSize
                            //    + " ticksize " + instrument.optionTickSize);

                            int priceTickLimit = 5;

                            if (priceLimitTickTextBox.Text != null)
                            {
                                priceTickLimit = Convert.ToInt16(priceLimitTickTextBox.Text);
                            }

                            if (price / instrument.optionTickSize <= priceTickLimit
                                ||
                                strikeCount == strikeCountLimit)
                            {
                                int rowCount = gridViewSpreadGrid.Rows.Count;

                                for (int groupAllocCnt = 0; groupAllocCnt < optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
                                {

                                    string acct = optionSpreadManager.selectAcct(
                                        instrument.exchangeSymbol,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt], true);

                                    if (
                                        fillInGridDataRow(rowCount,
                                    optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                    acct,
                                    contractTypeWing,
                                    strikeTest,
                                    price,
                                    optionImplVol * 100,
                                    0,
                                    0,
                                    optionYear,
                                    optionMonthInt,
                                    optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple,
                                    daysToExpOfOption * 365
                                    ))
                                    {
                                        rowCount++;
                                    }
                                }

                                break; //breaks while strike loop

                            }

                            strikeCount++;
                        }
                    }
                }

            }

//#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
            //#endif

            //hideNet0Rows(true);
        }

        private void addPutWingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOptionWing(OPTION_SPREAD_CONTRACT_TYPE.PUT);
        }

        private void addCallWingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addOptionWing(OPTION_SPREAD_CONTRACT_TYPE.CALL);
        }

        private void syntheticOption(
            OPTION_SPREAD_CONTRACT_TYPE contractBeingReplaced,
            OPTION_SPREAD_CONTRACT_TYPE syntheticReplacementContract, 
            int futureMultiplier, int optionMultiplier)
        {
#if DEBUG
            try
#endif
            {
                DataGridViewSelectedRowCollection selectedRows = gridViewSpreadGrid.SelectedRows;



                if (selectedRows.Count > 0)
                {
                    string contractTypeInSelectedRow = selectedRows[0].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractTypeInSelectedRow.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                    (int)contractBeingReplaced)) == 0)
                    {



                        int futureIdx = -1;
                        int legCount = 0;

                        double optionImplVol = 0;
                        double optionCount = 0;

                        //double daysToExpOfOption = 0;
                        //bool daysToExpFilled = false;

                        //int optionYear = 0;
                        //int optionMonthInt = 0;


                        double daysToExpOfOption = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value) / 365;

                        int optionYear = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                        int optionMonthInt = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                        double strike = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);


                        while (legCount < gridViewSpreadGrid.RowCount)
                        {
                            if (gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                            {
                                String contractType = gridViewSpreadGrid.Rows[legCount]
                                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                                if (contractType.CompareTo(
                                    optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                        (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                                {
                                    futureIdx = legCount;
                                    //break;
                                }
                                else
                                {
                                    if (gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value != null)
                                    {
                                        optionImplVol += Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value);

                                        optionCount++;
                                    }


                                }
                            }

                            legCount++;
                        }

                        if (optionCount > 0)
                        {
                            optionImplVol /= optionCount;

                            optionImplVol /= 100;
                        }

                        if (futureIdx > -1)
                        {
                            if (gridViewSpreadGrid.Rows[futureIdx]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value != null)
                            {

                                double futureClose = ConversionAndFormatting.convertToTickMovesDouble(
                                            gridViewSpreadGrid.Rows[futureIdx]
                                                .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                            instrument.tickSize, instrument.tickDisplay);


                                double futurePriceFactorForAllLegs = futureClose / instrument.optionStrikeIncrement;

                                double futurePriceFactor = optionSpreadManager.roundPriceForCallOrPut(
                                                    OPTION_SPREAD_CONTRACT_TYPE.PUT,
                                                    futurePriceFactorForAllLegs, instrument.optionStrikeIncrement);

                                int refStartOfFuturePriceCount = -TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2;

                                int futureExpressionIdx = getFutureExpressionIdx();

                                //int strikeCount = 0;

                                //int strikeCountLimit = 20;



                                //while (strikeCount <= strikeCountLimit)
                                {

                                    char contractCallOrPutChar = 'C';
                                    //int futureLongOrShort = -1;

                                    if (syntheticReplacementContract == OPTION_SPREAD_CONTRACT_TYPE.PUT)
                                    {
                                        contractCallOrPutChar = 'P';
                                        //futureLongOrShort = 1;
                                    }



                                    double tempFuturePriceFactor = futurePriceFactor
                                                + (refStartOfFuturePriceCount * instrument.optionStrikeIncrement);

                                    //double strikeTest =
                                    //    strikeDirection * strikeCount * instrument.optionStrikeIncrement
                                    //        + tempFuturePriceFactor;


                                    double rfr = Convert.ToDouble(riskFreeTextBox.Text) / 100;



                                    double price = OptionCalcs.blackScholes(contractCallOrPutChar,
                                        futureClose,
                                                strike,
                                                 daysToExpOfOption, rfr,
                                                optionImplVol);



                                    int rowCount = gridViewSpreadGrid.Rows.Count;

                                    for (int groupAllocCnt = 0; groupAllocCnt < optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
                                    {

                                        string acct = optionSpreadManager.selectAcct(
                                            instrument.exchangeSymbol,
                                            optionSpreadManager.portfolioGroupAllocation[groupAllocCnt], true);

                                        if (
                                            fillInGridDataRow(rowCount,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                        acct,
                                        syntheticReplacementContract,
                                        strike,
                                        price,
                                        optionImplVol * 100,
                                        0,
                                        0,
                                        optionYear,
                                        optionMonthInt,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple
                                            * optionMultiplier,
                                        daysToExpOfOption * 365
                                        ))
                                        {
                                            rowCount++;
                                        }



                                        if (
                                            fillInGridDataRow(rowCount,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                        acct,
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE,
                                        0,
                                        futureClose,
                                        0,
                                        optionSpreadExpressionList[futureExpressionIdx].futureContractYear,
                                        optionSpreadExpressionList[futureExpressionIdx].futureContractMonthInt,
                                        0,
                                        0,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple
                                            * futureMultiplier,
                                        0
                                        ))
                                        {
                                            rowCount++;
                                        }


                                    }

                                }
                            }
                        }
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


        private void replaceWithSyntheticOption(
            OPTION_SPREAD_CONTRACT_TYPE contractBeingReplaced,
            OPTION_SPREAD_CONTRACT_TYPE syntheticReplacementContract)
        {
#if DEBUG
            try
#endif
            {
                DataGridViewSelectedRowCollection selectedRows = gridViewSpreadGrid.SelectedRows;



                if (selectedRows.Count > 0)
                {
                    string contractTypeInSelectedRow = selectedRows[0].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                    if (contractTypeInSelectedRow.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                    (int)contractBeingReplaced)) == 0)
                    {



                        int futureIdx = -1;
                        int legCount = 0;

                        double optionImplVol = 0;
                        double optionCount = 0;


                        double daysToExpOfOption = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.DAYS_TO_EXP].Value) / 365;

                        int optionYear = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.YEAR].Value);

                        int optionMonthInt = Convert.ToInt16(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.MTH_AS_INT].Value);

                        double strike = Convert.ToDouble(selectedRows[0]
                                    .Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);


                        while (legCount < gridViewSpreadGrid.RowCount)
                        {
                            if (gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null)
                            {
                                String contractType = gridViewSpreadGrid.Rows[legCount]
                                            .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                                if (contractType.CompareTo(
                                    optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                        (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) == 0)
                                {
                                    futureIdx = legCount;
                                    //break;
                                }
                                else
                                {
                                    if (gridViewSpreadGrid.Rows[legCount]
                                        .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value != null)
                                    {
                                        optionImplVol += Convert.ToDouble(gridViewSpreadGrid.Rows[legCount]
                                                .Cells[(int)OPTION_PL_COLUMNS.IMPL_VOL].Value);

                                        optionCount++;
                                    }


                                }
                            }

                            legCount++;
                        }

                        if (optionCount > 0)
                        {
                            optionImplVol /= optionCount;

                            optionImplVol /= 100;
                        }

                        if (futureIdx > -1)
                        {
                            if (gridViewSpreadGrid.Rows[futureIdx]
                                        .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value != null)
                            {

                                double futureClose = ConversionAndFormatting.convertToTickMovesDouble(
                                            gridViewSpreadGrid.Rows[futureIdx]
                                                .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                            instrument.tickSize, instrument.tickDisplay);


                                double futurePriceFactorForAllLegs = futureClose / instrument.optionStrikeIncrement;

                                double futurePriceFactor = optionSpreadManager.roundPriceForCallOrPut(
                                                    OPTION_SPREAD_CONTRACT_TYPE.PUT,
                                                    futurePriceFactorForAllLegs, instrument.optionStrikeIncrement);

                                int refStartOfFuturePriceCount = -TradingSystemConstants.STRIKE_PRICE_REFERENCE_COUNT / 2;

                                int futureExpressionIdx = getFutureExpressionIdx();


                                //while (strikeCount <= strikeCountLimit)
                                {

                                    char contractCallOrPutChar = 'C';
                                    //int futureLongOrShort = -1;

                                    if (syntheticReplacementContract == OPTION_SPREAD_CONTRACT_TYPE.PUT)
                                    {
                                        contractCallOrPutChar = 'P';
                                        //futureLongOrShort = 1;
                                    }



                                    double tempFuturePriceFactor = futurePriceFactor
                                                + (refStartOfFuturePriceCount * instrument.optionStrikeIncrement);


                                    double rfr = Convert.ToDouble(riskFreeTextBox.Text) / 100;



                                    double price = OptionCalcs.blackScholes(contractCallOrPutChar,
                                        futureClose,
                                                strike,
                                                 daysToExpOfOption, rfr,
                                                optionImplVol);



                                    int rowCount = gridViewSpreadGrid.Rows.Count;

                                    for (int groupAllocCnt = 0; groupAllocCnt < optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
                                    {

                                        string acct = optionSpreadManager.selectAcct(
                                            instrument.exchangeSymbol,
                                            optionSpreadManager.portfolioGroupAllocation[groupAllocCnt], true);

                                        int net = clearGridDataRowContractsAndReturnLots(
                                            optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                            acct,
                                            optionArrayTypes.optionSpreadContractTypesArray.GetValue((int)contractBeingReplaced).ToString(),                                            
                                            strike,
                                            optionYear,
                                            optionMonthInt,
                                            optionYear,
                                            optionMonthInt);

                                        int[] numberOfContractsToReplace = getSyntheticNumberOfContracts(net,
                                            optionArrayTypes.optionSpreadContractTypesArray.GetValue((int)contractBeingReplaced).ToString());


                                        if (
                                            fillInGridDataRow(rowCount,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                        acct,
                                        syntheticReplacementContract,
                                        strike,
                                        price,
                                        optionImplVol * 100,
                                        0,
                                        0,
                                        optionYear,
                                        optionMonthInt,
                                        numberOfContractsToReplace[1],
                                        daysToExpOfOption * 365
                                        ))
                                        {
                                            rowCount++;
                                        }



                                        if (
                                            fillInGridDataRow(rowCount,
                                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker,
                                        acct,
                                        OPTION_SPREAD_CONTRACT_TYPE.FUTURE,
                                        0,
                                        futureClose,
                                        0,
                                        optionSpreadExpressionList[futureExpressionIdx].futureContractYear,
                                        optionSpreadExpressionList[futureExpressionIdx].futureContractMonthInt,
                                        0,
                                        0,
                                        numberOfContractsToReplace[0],
                                        0
                                        ))
                                        {
                                            rowCount++;
                                        }


                                    }

                                }
                            }
                        }
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

        private void addSyntheticPutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntheticOption(
                OPTION_SPREAD_CONTRACT_TYPE.PUT,
                OPTION_SPREAD_CONTRACT_TYPE.CALL, -1, 1);
        }

        private void addSyntheticCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntheticOption(
                OPTION_SPREAD_CONTRACT_TYPE.CALL,
                OPTION_SPREAD_CONTRACT_TYPE.PUT, 1, 1);
        }

        private void addSyntheticPutShortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntheticOption(
                OPTION_SPREAD_CONTRACT_TYPE.PUT,
                OPTION_SPREAD_CONTRACT_TYPE.CALL, 1, -1);
        }

        private void addSyntheticCallShortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            syntheticOption(
                OPTION_SPREAD_CONTRACT_TYPE.CALL,
                OPTION_SPREAD_CONTRACT_TYPE.PUT, -1, -1);
        }

        private void replaceWithSyntheticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = gridViewSpreadGrid.SelectedRows;

            if (selectedRows.Count > 0)
            {
                //string contractTypeInSelectedRow = selectedRows[0].Cells[(int)OPTION_PL_COLUMNS.NET].Value.ToString();

                int net = Convert.ToInt16(selectedRows[0]
                                        .Cells[(int)OPTION_PL_COLUMNS.NET].Value);

                int absnet = Math.Abs(net);

                String contractType = selectedRows[0]
                               .Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value.ToString();

                if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                        (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                {
                    if (net > 0)
                    {
                        replaceWithSyntheticOption(
                            OPTION_SPREAD_CONTRACT_TYPE.CALL,
                            OPTION_SPREAD_CONTRACT_TYPE.PUT);
                    }
                    else if (net < 0)
                    {
                        replaceWithSyntheticOption(
                            OPTION_SPREAD_CONTRACT_TYPE.CALL,
                            OPTION_SPREAD_CONTRACT_TYPE.PUT);
                    }
                }
                else if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                        (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
                {
                    if (net > 0)
                    {
                        replaceWithSyntheticOption(
                            OPTION_SPREAD_CONTRACT_TYPE.PUT,
                            OPTION_SPREAD_CONTRACT_TYPE.CALL);
                    }
                    else if (net < 0)
                    {
                        replaceWithSyntheticOption(
                            OPTION_SPREAD_CONTRACT_TYPE.PUT,
                            OPTION_SPREAD_CONTRACT_TYPE.CALL);
                    }
                }


                
            }
        }

        private int[] getSyntheticNumberOfContracts(int net, String contractType)
        {
            int[] numberOfLotsToReturn = new int[2];


            int absnet = Math.Abs(net);

            if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                    (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
            {
                if (net > 0)
                {
                    numberOfLotsToReturn[0] = absnet;
                    numberOfLotsToReturn[1] = absnet;

                    //syntheticOption(
                    //    OPTION_SPREAD_CONTRACT_TYPE.CALL,
                    //    OPTION_SPREAD_CONTRACT_TYPE.PUT, absnet, absnet);
                }
                else if (net < 0)
                {
                    numberOfLotsToReturn[0] = -absnet;
                    numberOfLotsToReturn[1] = -absnet;

                    //syntheticOption(
                    //    OPTION_SPREAD_CONTRACT_TYPE.CALL,
                    //    OPTION_SPREAD_CONTRACT_TYPE.PUT, -absnet, -absnet);
                }
            }
            else if (contractType.CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                    (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
            {
                if (net > 0)
                {
                    numberOfLotsToReturn[0] = -absnet;
                    numberOfLotsToReturn[1] = absnet;

                    //syntheticOption(
                    //    OPTION_SPREAD_CONTRACT_TYPE.PUT,
                    //    OPTION_SPREAD_CONTRACT_TYPE.CALL, -absnet, absnet);
                }
                else if (net < 0)
                {
                    numberOfLotsToReturn[0] = absnet;
                    numberOfLotsToReturn[1] = -absnet;

                    //syntheticOption(
                    //    OPTION_SPREAD_CONTRACT_TYPE.PUT,
                    //    OPTION_SPREAD_CONTRACT_TYPE.CALL, absnet, -absnet);
                }
            }

            return numberOfLotsToReturn;

        }

        private void hideNet0Rows(bool hide)
        {
            int hiddenRows = 0;

            for (int row = 0; row < gridViewSpreadGrid.Rows.Count; row++)
            {
                if (hide)
                {
                    if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null
                        &&
                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value
                        .ToString().CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.FUTURE)) != 0)
                    {
                        if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.NET].Value != null
                            &&
                            Convert.ToInt16(
                            gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.NET].Value) == 0)
                        {
                            gridViewSpreadGrid.Rows[row].Visible = false;
                            hiddenRows++;
                        }
                        else
                        {
                            gridViewSpreadGrid.Rows[row].Visible = true;
                        }
                    }
                }
                else
                {
                    gridViewSpreadGrid.Rows[row].Visible = true;
                }
            }

            lblTotalRows.Text = "Total Rows: " + gridViewSpreadGrid.Rows.Count;

            lblHiddenRows.Text = " Hidden Rows: " + hiddenRows;
        }

        private void hideNet0RowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideNet0Rows(true);
        }

        private void unhideNet0OptionRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideNet0Rows(false);
        }


        private double returnFuturesAveragePrice(int futureIdx)
        {
            double futureAvgPrice = 0;

            if (futureIdx >= 0)
            {
                if (gridViewSpreadGrid.Rows[futureIdx]
                                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value != null)
                {
                    futureAvgPrice = ConversionAndFormatting.convertToTickMovesDouble(
                                gridViewSpreadGrid.Rows[futureIdx]
                                    .Cells[(int)OPTION_PL_COLUMNS.AVG_PRC].Value.ToString(),
                                instrument.tickSize, instrument.tickDisplay);
                }
                else
                {
                    String caption = "FILL IN A PRICE FOR THE FUTURE CONTRACT";
                    String message = "FILL IN A PRICE FOR THE FUTURE CONTRACT";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    System.Windows.Forms.DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
                }
            }
            else
            {


                int expCount = 0;

                while (expCount < optionSpreadExpressionList.Count())
                {
                    if (optionSpreadExpressionList[expCount].instrument != null
                        && instrument.idxOfInstrumentInList ==
                            optionSpreadExpressionList[expCount].instrument.idxOfInstrumentInList
                        &&
                        optionSpreadExpressionList[expCount].callPutOrFuture ==
                            OPTION_SPREAD_CONTRACT_TYPE.FUTURE)
                    {
                        futureAvgPrice = optionSpreadExpressionList[expCount].defaultPrice;

                        break;
                    }

                    expCount++;
                }

            }

            return futureAvgPrice;
        }

        private void testMoneynessOfOptions(bool runHighlight)
        {
            int futureIndexLeg = findFutureLeg();

            double futureAveragePrice = returnFuturesAveragePrice(futureIndexLeg);

            for (int row = 0; row < gridViewSpreadGrid.Rows.Count; row++)
            {

                bool highlightedRow = false;
                if (runHighlight)
                {
                    if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null
                        &&
                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value
                        .ToString().CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.CALL)) == 0)
                    {
                        if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value != null)
                        {
                            double strikePrice = Convert.ToDouble(
                                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);

                            if ((futureAveragePrice - strikePrice) / instrument.optionStrikeIncrement >= Convert.ToInt16(comboBox1.Text))
                            {
                                gridViewSpreadGrid.Rows[row].DefaultCellStyle.BackColor = Color.Yellow;

                                highlightedRow = true;
                            }
                        }
                    }
                    else if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value != null
                        &&
                        gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.CNTRT_TYPE].Value
                        .ToString().CompareTo(optionArrayTypes.optionSpreadContractTypesArray.GetValue(
                                (int)OPTION_SPREAD_CONTRACT_TYPE.PUT)) == 0)
                    {
                        if (gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value != null)
                        {
                            double strikePrice = Convert.ToDouble(
                                gridViewSpreadGrid.Rows[row].Cells[(int)OPTION_PL_COLUMNS.STRIKE].Value);

                            if ((strikePrice - futureAveragePrice) / instrument.optionStrikeIncrement >= Convert.ToInt16(comboBox1.Text))
                            {
                                gridViewSpreadGrid.Rows[row].DefaultCellStyle.BackColor = Color.Yellow;

                                highlightedRow = true;
                            }
                        }
                    }
                }

                if(!highlightedRow)
                {
                    gridViewSpreadGrid.Rows[row].DefaultCellStyle.BackColor = Color.White;
                }

            }
        }

        private void highlightUnhighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            highlightingIntheMoney = !highlightingIntheMoney;
            
            testMoneynessOfOptions(highlightingIntheMoney);
            
            if (highlightingIntheMoney)
            {
                highlightUnhighlightToolStripMenuItem.BackColor = Color.Yellow;
            }
            else
            {
                highlightUnhighlightToolStripMenuItem.BackColor = menuStrip1.BackColor;
            }
        }

        internal void adjustChartSplitter()
        {
            splitContainerWithChart.SplitterDistance = 170;
        }

        

    }

}
