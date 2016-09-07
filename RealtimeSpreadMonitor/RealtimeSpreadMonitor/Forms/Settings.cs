using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class Settings : Form
    {
        delegate void ThreadSafeUpdateWriteDate(DateTime writeDate);

        private OptionSpreadManager optionSpreadManager;
        private RealtimeMonitorSettings realtimeMonitorSettings;
        //private OptionRealtimeMonitor optionRealtimeMonitor;

        public Settings(OptionSpreadManager optionSpreadManager)
        //OptionRealtimeMonitor optionRealtimeMonitor)
        {
            this.optionSpreadManager = optionSpreadManager;

            //this.optionRealtimeMonitor = optionRealtimeMonitor;

            InitializeComponent();

            realtimeMonitorSettings =
                optionSpreadManager.realtimeMonitorSettings;

            setupSettings();

            //getRealtimeSystemResultsFromDatabase();

            if (optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                runConflictList();
            }
        }

        private void setupSettings()
        {
            switch (realtimeMonitorSettings.realtimePriceFillType)
            {
                case REALTIME_PRICE_FILL_TYPE.PRICE_DEFAULT:
                    radioBtnDefaultPriceRules.Checked = true;
                    break;
                case REALTIME_PRICE_FILL_TYPE.PRICE_MID_BID_ASK:
                    radioBtnMidPriceRules.Checked = true;
                    break;
                case REALTIME_PRICE_FILL_TYPE.PRICE_THEORETICAL:
                    radioBtnTheorPriceRules.Checked = true;
                    break;
            }

            if (realtimeMonitorSettings.eodAnalysis)
            {
                chkBoxEodSettlements.Checked = true;
            }
            else
            {
                chkBoxEodSettlements.Checked = false;
            }

            setupInstrumentAcctList();

            setupAdditionalFIXFieldsList();
        }

        private void setupInstrumentAcctList()
        {
            instrumentAcctList.Columns.Add("Brkr", 75, HorizontalAlignment.Left);
            instrumentAcctList.Columns.Add("Inst", 75, HorizontalAlignment.Left);
            instrumentAcctList.Columns.Add("Acct", 100, HorizontalAlignment.Left);
            instrumentAcctList.Columns.Add("Mult", 25, HorizontalAlignment.Left);


            for (int groupAllocCnt = 0;
                groupAllocCnt < optionSpreadManager.portfolioGroupAllocation.Length; groupAllocCnt++)
            {
                if (optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].useConfigFile)
                {
                    foreach (KeyValuePair<string, string> instrumentAcct in
                        optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].instrumentAcctHashSet)
                    {
                        ListViewItem item = new ListViewItem(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                        item.BackColor = Color.Black;
                        item.ForeColor = Color.White;
                        item.UseItemStyleForSubItems = false;

                        instrumentAcctList.Items.Add(item);

                        item.SubItems.Add(instrumentAcct.Key,
                                    Color.Black, Color.LawnGreen, item.Font);

                        item.SubItems.Add(instrumentAcct.Value,
                                    Color.Black, Color.LawnGreen, item.Font);

                        item.SubItems.Add(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple.ToString(),
                                    Color.Black, Color.LawnGreen, item.Font);

                    }
                }
                else
                {
                    ListViewItem item = new ListViewItem(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].broker);
                    item.BackColor = Color.Black;
                    item.ForeColor = Color.White;
                    item.UseItemStyleForSubItems = false;

                    instrumentAcctList.Items.Add(item);

                    item.SubItems.Add("",
                                Color.Black, Color.Black, item.Font);

                    item.SubItems.Add(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].account,
                                Color.Black, Color.LightBlue, item.Font);

                    item.SubItems.Add(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple.ToString(),
                                Color.Black, Color.LightBlue, item.Font);
                }
            }
        }

        private void setupAdditionalFIXFieldsList()
        {
            additionalFixFieldsList.Columns.Add("FCM", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("TT Gateway", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("TT Exchange", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("TT Symbol", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("47 Rule80A", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("204 CustomerOrFirm", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("18205 TT Acct", 75, HorizontalAlignment.Left);
            additionalFixFieldsList.Columns.Add("440 ClearingAccount", 75, HorizontalAlignment.Left);
            //additionalFixFieldsList.Columns.Add("16102 FFT2", 75, HorizontalAlignment.Left);




            foreach (KeyValuePair<string, InstrumentSpecificFIXFields> instrumentSpecificFIXValues in optionSpreadManager.instrumentSpecificFIXFieldHashSet)
            {

                ListViewItem item = new ListViewItem(instrumentSpecificFIXValues.Value.FCM);
                item.BackColor = Color.Black;
                item.ForeColor = Color.White;
                item.UseItemStyleForSubItems = false;

                additionalFixFieldsList.Items.Add(item);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TTGateway,
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TTExchange,
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TTSymbol,
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TAG_47_Rule80A.ToString(),
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TAG_204_CustomerOrFirm.ToString(),
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TAG_18205_TTAccountType.ToString(),
                            Color.Black, Color.LawnGreen, item.Font);

                item.SubItems.Add(instrumentSpecificFIXValues.Value.TAG_440_ClearingAccount.ToString(),
                            Color.Black, Color.LawnGreen, item.Font);

            }
        }

        private void radioBtnDefaultPriceRules_CheckedChanged(object sender, EventArgs e)
        {
            optionPriceFillTypeChanged();
        }

        private void radioBtnAskPriceRules_CheckedChanged(object sender, EventArgs e)
        {
            optionPriceFillTypeChanged();
        }

        private void radioBtnMidPriceRules_CheckedChanged(object sender, EventArgs e)
        {
            optionPriceFillTypeChanged();
        }

        private void radioBtnBidPriceRules_CheckedChanged(object sender, EventArgs e)
        {
            optionPriceFillTypeChanged();
        }

        private void radioBtnTheorPriceRules_CheckedChanged(object sender, EventArgs e)
        {
            optionPriceFillTypeChanged();
        }

        private void optionPriceFillTypeChanged()
        {
            REALTIME_PRICE_FILL_TYPE realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_DEFAULT;

            if (radioBtnMidPriceRules.Checked)
            {
                realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_MID_BID_ASK;
            }
            else if (radioBtnTheorPriceRules.Checked)
            {
                realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_THEORETICAL;
            }
            else if (radioBtnAskPriceRules.Checked)
            {
                realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_ASK;
            }
            else if (radioBtnBidPriceRules.Checked)
            {
                realtimePriceFillType = REALTIME_PRICE_FILL_TYPE.PRICE_BID;
            }

            realtimeMonitorSettings.realtimePriceFillType = realtimePriceFillType;

            optionSpreadManager.updatedRealtimeMonitorSettingsThreadRun();

            //optionRealtimeMonitor.updateStatusStripOptionMonitor();
        }

        public void updateSettings()
        {
            setupSettings();

            groupBox1.Enabled = !realtimeMonitorSettings.eodAnalysis;

            btnWriteRealtimeStateToDB.Enabled = realtimeMonitorSettings.eodAnalysis;
        }

        private void chkBoxEodSettlements_CheckedChanged(object sender, EventArgs e)
        {
            realtimeMonitorSettings.eodAnalysis = chkBoxEodSettlements.Checked;

            groupBox1.Enabled = !chkBoxEodSettlements.Checked;

            btnWriteRealtimeStateToDB.Enabled = chkBoxEodSettlements.Checked;

            if (realtimeMonitorSettings.eodAnalysis)
            {
                realtimeMonitorSettings.alreadyWritten = false;
            }

            optionSpreadManager.updateEODMonitorDataSettings(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {


            this.Visible = false;
        }

        private void btnWriteRealtimeStateToDB_Click(object sender, EventArgs e)
        {
            saveEODDataToDB();

        }

        private void saveEODDataToDB()
        {
            DateTime returnDateTime;

            if(optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                returnDateTime = optionSpreadManager.writeRealtimeStateToDatabaseAzure();
            }
            else
            {
                returnDateTime = optionSpreadManager.writeRealtimeStateToDatabase();
            }

            //DateTime returnDateTime = optionSpreadManager.writeRealtimeStateToDatabase();

            updateWriteDate(returnDateTime);

            //getRealtimeSystemResultsFromDatabase();

            if (optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                runConflictList();
            }
        }

        private void runConflictList()
        {
            Thread runConflictListThread = new Thread(new ParameterizedThreadStart(getRealtimeSystemResultsFromDatabaseThreadSafe));
            runConflictListThread.IsBackground = true;
            runConflictListThread.Start();
        }

        delegate void getRealtimeSystemResultsFromDatabaseDelegate();

        private void getRealtimeSystemResultsFromDatabaseThreadSafe(Object obj)
        {
            //this.Invoke(new EventHandler(optionSpreadManager.openThread));

            optionSpreadManager.openThread(null, null);

            if (eodConflictList.InvokeRequired)
            {
                getRealtimeSystemResultsFromDatabaseDelegate d =
                    new getRealtimeSystemResultsFromDatabaseDelegate(getRealtimeSystemResultsFromDatabase);
                eodConflictList.Invoke(d);
            }
            else
            {
                getRealtimeSystemResultsFromDatabase();
            }

            //this.Invoke(new EventHandler(optionSpreadManager.closeThread));

            optionSpreadManager.closeThread(null, null);
        }


        private void getRealtimeSystemResultsFromDatabase()
        {
            try
            {
                eodConflictList.Clear();

                eodConflictList.Columns.Add("Rule Type", 75, HorizontalAlignment.Left);

                eodConflictList.Columns.Add("Realtime Rule", 200, HorizontalAlignment.Left);
                eodConflictList.Columns.Add("EOD Rule", 200, HorizontalAlignment.Left);

                eodConflictList.Columns.Add("strategy", 50, HorizontalAlignment.Left);

                eodConflictList.Columns.Add("instrument", 50, HorizontalAlignment.Left);





                int stratListOfIds = 0;
                int realtime = 1;
                int eod = 2;

                DataRow[][] systemResultsComparison = optionSpreadManager.readRealtimeSystemResultsFromDatabase();

                Dictionary<int, DataRow> intradayRows = new Dictionary<int, DataRow>();
                for (int i = 0; i < systemResultsComparison[realtime].Length; i++)
                {
                    if (!intradayRows.ContainsKey(Convert.ToInt32(systemResultsComparison[realtime][i]["idstrategy"])))
                    {
                        intradayRows.Add(Convert.ToInt32(systemResultsComparison[realtime][i]["idstrategy"]),
                            systemResultsComparison[realtime][i]);
                    }
                }

                Dictionary<int, DataRow> eodRows = new Dictionary<int, DataRow>();
                for (int i = 0; i < systemResultsComparison[eod].Length; i++)
                {
                    if (!eodRows.ContainsKey(Convert.ToInt32(systemResultsComparison[eod][i]["idstrategy"])))
                    {
                        eodRows.Add(Convert.ToInt32(systemResultsComparison[eod][i]["idstrategy"]),
                            systemResultsComparison[eod][i]);
                    }
                }

                OptionStrategy[] optionStrategies = optionSpreadManager.getOptionStrategies;
                Dictionary<int, OptionStrategy> optionStrategiesHashTable = new Dictionary<int, OptionStrategy>();
                for (int i = 0; i < optionStrategies.Length; i++)
                {
                    if (!optionStrategiesHashTable.ContainsKey(optionStrategies[i].idStrategy))
                    {
                        optionStrategiesHashTable.Add(optionStrategies[i].idStrategy,
                            optionStrategies[i]);
                    }
                }

                for (int i = 0; i < systemResultsComparison[stratListOfIds].Length; i++)
                {
                    int idstrategy = Convert.ToInt32(systemResultsComparison[stratListOfIds][i]["idstrategy"]);

                    if (intradayRows.ContainsKey(idstrategy) && eodRows.ContainsKey(idstrategy))
                    {
                        DataRow realtimeRow = intradayRows[idstrategy];

                        DataRow eod_1_Row = eodRows[idstrategy];

                        OptionStrategy optionStrategy = optionStrategiesHashTable[idstrategy];

                        //TSErrorCatch.debugWriteOut(realtimeRow["entryrule"].ToString());
                        //TSErrorCatch.debugWriteOut(eod_1_Row["entryrule"].ToString());

                        string realtimeEntry = realtimeRow["entryrule"].ToString();
                        string eodEntry = eod_1_Row["entryrule"].ToString();

                        string realtimeExit = realtimeRow["exitrule"].ToString();
                        string eodExit = eod_1_Row["exitrule"].ToString();

                        bool entryConflict = true;
                        if (realtimeEntry.Length > 4 && eodEntry.Length > 4
                            && realtimeEntry.Substring(realtimeEntry.Length - 4).CompareTo(
                                eodEntry.Substring(eodEntry.Length - 4)) == 0)
                        {
                            entryConflict = false;
                        }

                        if (entryConflict)
                        {
                            //StringBuilder entry = new StringBuilder();
                            //entry.Append(realtimeEntry);
                            //entry.Append("\n");
                            //entry.Append(eodEntry);

                            ListViewItem item = new ListViewItem("Entry Conflict");
                            item.BackColor = Color.LawnGreen;
                            item.ForeColor = Color.Black;
                            item.UseItemStyleForSubItems = false;

                            eodConflictList.Items.Add(item);



                            item.SubItems.Add(realtimeEntry,
                                        Color.Black, Color.LightGreen, item.Font);
                            item.SubItems.Add(eodEntry,
                                        Color.Black, Color.LightGreen, item.Font);

                            item.SubItems.Add(idstrategy.ToString(),
                                        Color.Black, Color.LightGreen, item.Font);

                            item.SubItems.Add(optionStrategy.instrument.exchangeSymbol,
                                        Color.Black, Color.LightGreen, item.Font);

                            //item.SubItems.Add(instrumentAcct.Value,
                            //            Color.Black, Color.LawnGreen, item.Font);

                            //item.SubItems.Add(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple.ToString(),
                            //            Color.Black, Color.LawnGreen, item.Font);
                        }

                        bool exitConflict = true;
                        if (realtimeExit.Length > 4 && eodExit.Length > 4
                            && realtimeExit.Substring(realtimeExit.Length - 4).CompareTo(
                                eodExit.Substring(eodExit.Length - 4)) == 0)
                        {
                            exitConflict = false;
                        }

                        if (exitConflict)
                        {
                            //StringBuilder entry = new StringBuilder();
                            //entry.Append(realtimeEntry);
                            //entry.Append("\n");
                            //entry.Append(eodEntry);

                            ListViewItem item = new ListViewItem("Exit Conflict");
                            item.BackColor = Color.Red;
                            item.ForeColor = Color.White;
                            item.UseItemStyleForSubItems = false;

                            eodConflictList.Items.Add(item);



                            item.SubItems.Add(realtimeExit,
                                        Color.Black, Color.LightPink, item.Font);
                            item.SubItems.Add(eodExit,
                                        Color.Black, Color.LightPink, item.Font);

                            item.SubItems.Add(idstrategy.ToString(),
                                        Color.Black, Color.LightPink, item.Font);

                            item.SubItems.Add(optionStrategy.instrument.exchangeSymbol,
                                        Color.Black, Color.LightPink, item.Font);

                            //item.SubItems.Add(instrumentAcct.Value,
                            //            Color.Black, Color.LawnGreen, item.Font);

                            //item.SubItems.Add(optionSpreadManager.portfolioGroupAllocation[groupAllocCnt].multiple.ToString(),
                            //            Color.Black, Color.LawnGreen, item.Font);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }



        public void updateWriteDate(DateTime updateWriteDateParam)
        {
            if (this.InvokeRequired)
            {
                ThreadSafeUpdateWriteDate d =
                    new ThreadSafeUpdateWriteDate(updateWriteDateThreadSafe);

                this.Invoke(d, updateWriteDateParam);
            }
            else
            {
                updateWriteDateThreadSafe(updateWriteDateParam);
            }
        }

        private void updateWriteDateThreadSafe(DateTime updateWriteDate)
        {
            StringBuilder lblUpdate = new StringBuilder();
            lblUpdate.Append("DB updates to: ");
            lblUpdate.Append(updateWriteDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo));

            lblRealtimeUpdatedTo.Text = lblUpdate.ToString();
        }

        private void btnGetEOD_Click(object sender, EventArgs e)
        {
            if (optionSpreadManager.initializationParmsSaved.useCloudDb)
            {
                runConflictList();
            }
        }

    }
}
