using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace RealtimeSpreadMonitor.Forms
{
    public partial class InitializationForm : Form
    {
        /// <summary>
        /// The initialization parms
        /// </summary>
        InitializationParms initializationParms;
        /// <summary>
        /// The PGS
        /// </summary>
        PortfolioGroupsStruct[] pgs;


        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationForm"/> class.
        /// </summary>
        /// <param name="initializationParms">The initialization parms.</param>
        public InitializationForm(InitializationParms initializationParms)
        {
            this.initializationParms = initializationParms;
            initializationParms.portfolioGroupName = "";

            //this.tmlSystemRunType = tmlSystemRunType;

            InitializeComponent();
            loadPortfolioGroups();

            this.Text = "OPTIONS REALTIME";

            transactionTime.Value = new DateTime(2014, 1, 1, 9, 30, 0);

#if DEBUG
            TSErrorCatch.errorCatchSetup();
#endif
        }

        /// <summary>
        /// Handles the Click event of the btnRunSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void btnRunSystem_Click(Object sender, EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                //bool continueToRunSystem = true;

                initializationParms.runLiveSystem = true;

                initializationParms.dbServerName = (String)(cmbxDatabase.SelectedItem);
                if (initializationParms.dbServerName == null)
                {
                    initializationParms.dbServerName = TradingSystemConstants.DB_DEFAULTSERVERIP;
                }

                if (initializationParms.runFromDb)
                {
                    initializationParms.idPortfolioGroup = pgs[cmbxPortfolio.SelectedIndex].idPortfolioGroup;

                    initializationParms.portfolioGroupName = pgs[cmbxPortfolio.SelectedIndex].portfolioName;
                }


                initializationParms.modelDateTime = modelDateTimePicker.Value.Date;

                initializationParms.useHalfday = checkBoxUseHalfDay.Checked;
                initializationParms.halfDayTransactionTime = transactionTime.Value;
                initializationParms.halfDayDecisionOffsetMinutes = (int)decisionMinuteOffset.Value;


                initializationParms.useCloudDb = checkBoxCloudDB.Checked;


                //writeInitializationConfigFile();

                //if (continueToRunSystem)
                
                this.Dispose();

            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        /// <summary>
        /// Writes the initialization configuration file.
        /// </summary>
        void writeInitializationConfigFile()
        {
#if DEBUG
            try
#endif
            {
                SaveOutputFile sof = new SaveOutputFile(TradingSystemConstants.INITIALIZE_CONFIG_DIRECTORY);

                sof.createConfigFile(TradingSystemConstants.INITIALIZE_OPTION_REALTIME_FILE_NAME);

                Type configTypes = typeof(INITIALIZATION_CONFIG_VARS);
                Array configNames = Enum.GetNames(configTypes);

                sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.PORTFOLIOGROUP).ToString(),
                    (String)(cmbxPortfolio.SelectedItem));

                sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME).ToString(),
                    (String)(cmbxDatabase.SelectedItem));

                //sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.BROKER).ToString(),
                //    initializationParms.FIX_Broker_18220);

                //sof.writeConfigLineFile(configNames.GetValue((int)INITIALIZATION_CONFIG_VARS.ACCOUNT).ToString(),
                //    initializationParms.FIX_Acct);

                sof.closeAndSaveFile();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        /// <summary>
        /// Handles the Click event of the btnExitSystem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void btnExitSystem_Click(System.Object sender, System.EventArgs e)
        {
#if DEBUG
            try
#endif
            {
                initializationParms.runLiveSystem = false;

                //writeInitializationConfigFile();



                this.Close();
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif
        }

        void loadPortfolioGroups()
        {
#if DEBUG
            try
#endif
            {
                SaveOutputFile sof = new SaveOutputFile();


                String[] initializationConfigs = sof.readInitializeConfigFile(
                    TradingSystemConstants.INITIALIZE_OPTION_REALTIME_FILE_NAME,
                    (int)REALTIME_CONFIG_FILE_INPUT_TYPE.INITIALIZATION_FILE);

                initializationParms.initializationConfigs = initializationConfigs;

                String[] realtimeConfiguration = sof.readInitializeConfigFile(
                    TradingSystemConstants.REALTIME_CONFIGURATION,
                    (int)REALTIME_CONFIG_FILE_INPUT_TYPE.REALTIME_CONFIGURATION);


                //String[] brokerList =
                //    realtimeConfiguration[(int)REALTIME_CONFIGURATION.MULTIBROKER].Split(',');

                //initializationParms.fixBrokerList = brokerList;


                //String[] acctList =
                //    realtimeConfiguration[(int)REALTIME_CONFIGURATION.ACCOUNT].Split(',');

                //initializationParms.fixAcctList = acctList;



                if (initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME] == null
                    || initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME].Length == 0)
                {
                    initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME] =
                        TradingSystemConstants.DB_DEFAULTSERVERIP;
                }

                String[] dbServerList =
                    realtimeConfiguration[(int)REALTIME_CONFIGURATION.SERVERNAME].Split(',');

                //ArrayList dbServerList = sof.readDBserverListFile();

                //sof.closeAndSaveFile();

                if (dbServerList != null && dbServerList.Length > 0)
                {
                    int dbSelectedIndex = 0;
                    for (int i = 0; i < dbServerList.Length; i++)
                    {
                        String serverName = (String)(dbServerList[i]);
                        cmbxDatabase.Items.Add(serverName);
                        if (serverName.CompareTo(initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME]) == 0)
                        {
                            dbSelectedIndex = i;
                        }
                        //itemCounter++;
                    }

                    if (cmbxDatabase.Items.Count > 0)
                    {
                        cmbxDatabase.SelectedIndex = dbSelectedIndex;
                    }
                }
                else
                {
                    cmbxDatabase.Items.Add(initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME]);
                    cmbxDatabase.SelectedIndex = 0;
                }

                

                //if (brokerList != null && brokerList.Length > 0)
                //{
                //    int dbSelectedIndex = 0;
                //    for (int i = 0; i < brokerList.Length; i++)
                //    {
                //        String brokerName = (String)(brokerList[i]);
                //        cmbxBroker.Items.Add(brokerName);
                //        if (brokerName.CompareTo(initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.BROKER]) == 0)
                //        {
                //            dbSelectedIndex = i;
                //        }
                //        //itemCounter++;
                //    }

                //    if (cmbxBroker.Items.Count > 0)
                //    {
                //        cmbxBroker.SelectedIndex = dbSelectedIndex;
                //    }
                //}
                //else
                //{
                //    cmbxBroker.Items.Add(initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.BROKER]);
                //    cmbxBroker.SelectedIndex = 0;
                //}


                if (initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME]
                    .CompareTo("Azure") == 0)
                {
                    TMLAzureModelDBQueries btdb = new TMLAzureModelDBQueries();

                    pgs = btdb.queryPortfolioGroups(0);
                }
                else
                {

                    TMLModelDBQueries btdb = new TMLModelDBQueries();

                    btdb.connectDB(initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.DBSERVERNAME]);

                    pgs = btdb.queryPortfolioGroups(0);

                    btdb.closeDB();
                }

                bool setSelectedPortfolio = false;
                for (int i = 0; i < pgs.Length; i++)
                {
                    StringBuilder portfolioNumberAndName = new StringBuilder();
                    portfolioNumberAndName.Append(pgs[i].idPortfolioGroup);
                    portfolioNumberAndName.Append(". ");
                    portfolioNumberAndName.Append(pgs[i].portfolioName);

                    cmbxPortfolio.Items.Add(portfolioNumberAndName.ToString());
                    //if(  )
                    if (initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.PORTFOLIOGROUP] != null
                        && initializationConfigs[(int)INITIALIZATION_CONFIG_VARS.PORTFOLIOGROUP].CompareTo(pgs[i].portfolioName) == 0)
                    {
                        //pgs[i].selected
                        pgs[i].selected = true;

                        cmbxPortfolio.SelectedIndex = i;

                        setSelectedPortfolio = true;
                    }
                    //itemCounter++;
                }

                if (!setSelectedPortfolio)
                {
                    cmbxPortfolio.SelectedIndex = 0;
                }

                initializationParms.runFromDb = true;
            }
#if DEBUG
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
#endif



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timeGroupBox.Enabled = true;
        }

    }
}
