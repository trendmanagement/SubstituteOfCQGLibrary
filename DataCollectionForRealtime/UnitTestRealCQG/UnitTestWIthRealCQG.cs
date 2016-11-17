using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG;
using FakeCQG.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRealCQG
{
    [TestClass]
    [DeploymentItem("Interop.CQG.dll")]
    //This tests should run if CQG client is connected
    public class UnitTestWIthRealCQG
    {
        public string status = string.Empty;
        [TestMethod]
        public void FakeCQG_EventHandlersWork()
        {
            // arrange
            UnitTestHelper.StartUp();
            string statusConnectionUp = "csConnectionUp";
            Timer timer = new Timer();
            timer.Interval = 30;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            CQGCEL fakeCQGCel = new CQGCELClass();
            fakeCQGCel.DataConnectionStatusChanged += new _ICQGCELEvents_DataConnectionStatusChangedEventHandler(Cell_DataConnectionStatusChanged);
            Core.LogChange += CQG_LogChange;

            // act
            fakeCQGCel.Startup();
            Task.Delay(300).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(statusConnectionUp, status);
        }

        [TestMethod]
        public void FakeCQG_TimedBarsRequest()
        {
            // arrange
            #region Timer
            Timer timer = new Timer();
            timer.Interval = 30;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            #endregion
            UnitTestHelper.StartUp();
            CQGCEL fakeCQGCel = new CQGCELClass();
            Core.LogChange += CQG_LogChange;
            CQGTimedBarsRequest m_TimedBarsRequest = default(CQGTimedBarsRequest);
            eHistoricalPeriod historicalPeriod = default(eHistoricalPeriod);
            CQGTimedBars CQGTimedBars = default(CQGTimedBars);
            var startTime = new DateTime(2016, 06, 06);
            List<int> TimedBarsRequestOutputs = new List<int>() { 0, 1, 2 };
            // act

            m_TimedBarsRequest.Symbol = "h";

                m_TimedBarsRequest.RangeStart = startTime;
                m_TimedBarsRequest.RangeEnd = DateTime.Now;


            historicalPeriod = (eHistoricalPeriod)1;

            m_TimedBarsRequest.HistoricalPeriod = historicalPeriod;

            m_TimedBarsRequest.TickFilter = (eTickFilter)1;

            m_TimedBarsRequest.Continuation = (eTimeSeriesContinuationType)1;

            m_TimedBarsRequest.IgnoreEventsOnHistoricalBars = true;


                m_TimedBarsRequest.SessionFlags = eSessionFlag.sfUndefined;

            m_TimedBarsRequest.ExcludeAllOutputs();

            foreach (eTimedBarsRequestOutputs selectedOutput in TimedBarsRequestOutputs)
            {
                m_TimedBarsRequest.IncludeOutput(selectedOutput, true);
            }

            CQGTimedBars = fakeCQGCel.AllTimedBars.get_ItemById("h");
            // assert
            Assert.IsNotNull(CQGTimedBars);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UnitTestHelper.QueryHandler.ReadQueries();
            UnitTestHelper.QueryHandler.ProcessEntireQueryList();
        }

        private void Cell_DataConnectionStatusChanged(eConnectionStatus new_status)
        {
            status = new_status.ToString();
        }

        private void CQG_LogChange(string message)
        {
        }
    }
}
