using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using CQG;

namespace DataCollectionForRealtime
{
    class CQGDataManagement
    {
        public CQGDataManagement(DCMainForm mainForm, DCMiniMonitor miniMonitor)
        {
            this.mainForm = mainForm;
            this.miniMonitor = miniMonitor;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            this.CQGAssm = Assembly.LoadFile(assmPath);

            ThreadPool.QueueUserWorkItem(new WaitCallback(initializeCQGAndCallbacks));
        }

        private DCMainForm mainForm;
        private DCMiniMonitor miniMonitor;
        public Assembly CQGAssm;

        private CQGCEL m_CEL;
        private string m_CEL_key;

        public CQGCEL CEL
        {
            get { return m_CEL; }
        }

        public string CEL_key
        {
            get { return m_CEL_key; }
        }

        internal void connectCQG()
        {
            if (m_CEL != null)
            {
                m_CEL.Startup();
            }
        }

        internal void shutDownCQGConn()
        {
            if (m_CEL != null)
            {
                m_CEL.Shutdown();
            }
        }

        internal void initializeCQGAndCallbacks(Object obj)
        {
            try
            {
                // Create real CQGCEL object and put it into the dictionary
                // Remark: we do not use "m_CEL = new CQG.CQGCEL();" to facilitate further reflection on this COM object
                string typeName = "CQG.CQGCELClass";
                m_CEL = (CQGCEL)CQGAssm.CreateInstance(typeName);

                m_CEL_key = FakeCQG.Internal.Core.CreateUniqueKey();
                FakeCQG.Internal.ServerDictionaries.PutObjectToTheDictionary(m_CEL_key, m_CEL);

                m_CEL_CELDataConnectionChg(eConnectionStatus.csConnectionDown);

                m_CEL.DataConnectionStatusChanged += new _ICQGCELEvents_DataConnectionStatusChangedEventHandler(m_CEL_CELDataConnectionChg);

                m_CEL.DataError += new _ICQGCELEvents_DataErrorEventHandler(m_CEL_DataError);

                m_CEL.APIConfiguration.ReadyStatusCheck = eReadyStatusCheck.rscOff;

                m_CEL.APIConfiguration.CollectionsThrowException = false;

                m_CEL.APIConfiguration.TimeZoneCode = eTimeZone.tzPacific;

                connectCQG();
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private bool isCQGStarted;

        public bool IsCQGStarted
        {
            get
            {
                isCQGStarted = m_CEL.IsStarted;
                return isCQGStarted;
            }
        }

        private void m_CEL_DataError(System.Object cqg_error, System.String error_description)
        {
            try
            {
                if (mainForm != null)
                {
                    mainForm.UpdateCQGDataStatus(
                        "CQG ERROR", Color.Yellow, Color.Red);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }

        private void m_CEL_CELDataConnectionChg(eConnectionStatus new_status)
        {
            StringBuilder connStatusString = new StringBuilder();
            StringBuilder connStatusShortString = new StringBuilder();
            Color connColor = Color.Red;

            try
            {
                if (m_CEL.IsStarted)
                {
                    connStatusString.Append("CQG API:");
                    connStatusString.Append(m_CEL.Environment.CELVersion);
                    connStatusShortString.Append("CQG:");

                    if (new_status != eConnectionStatus.csConnectionUp)
                    {
                        if (new_status == eConnectionStatus.csConnectionDelayed)
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
                }

                if (mainForm != null)
                {
                    mainForm.UpdateConnectionStatus(
                        connStatusString.ToString(), connColor);
                }
            }
            catch (Exception ex)
            {
                TSErrorCatch.errorCatchOut(Convert.ToString(this), ex);
            }
        }
    }
}
