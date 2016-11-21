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
            UnitTestHelper.StartUp();
            #region Timer
            Timer timer = new Timer();
            timer.Interval = 30;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            #endregion
            CQGCEL fakeCQGCel = new CQGCELClass();
            Core.LogChange += CQG_LogChange;
            CQGTimedBarsRequest m_TimedBarsRequest;
            CQGTimedBars CQGTimedBars;
            var startTime = new DateTime(2016, 06, 06);
            List<eTimedBarsRequestOutputs> TimedBarsRequestOutputs = new List<eTimedBarsRequestOutputs>();
            TimedBarsRequestOutputs.Add(eTimedBarsRequestOutputs.tbrActualVolume);
            TimedBarsRequestOutputs.Add(eTimedBarsRequestOutputs.tbrTickVolume);
            TimedBarsRequestOutputs.Add(eTimedBarsRequestOutputs.tbrAskVolume);
            TimedBarsRequestOutputs.Add(eTimedBarsRequestOutputs.tbrBidVolume);
            TimedBarsRequestOutputs.Add(eTimedBarsRequestOutputs.tbrOpenInterest);
            int recurrenceСount = 10;

            // act
            while (recurrenceСount > 0)
            {
                m_TimedBarsRequest = fakeCQGCel.CreateTimedBarsRequest();
                m_TimedBarsRequest.Symbol = "h";
                m_TimedBarsRequest.IncludeEnd = false;
                m_TimedBarsRequest.RangeStart = startTime;
                m_TimedBarsRequest.RangeEnd = DateTime.Now;
                m_TimedBarsRequest.HistoricalPeriod = eHistoricalPeriod.hpWeekly;
                m_TimedBarsRequest.TickFilter = eTickFilter.tfDefault;
                m_TimedBarsRequest.Continuation = eTimeSeriesContinuationType.tsctNoContinuation;
                m_TimedBarsRequest.EqualizeCloses = true;
                m_TimedBarsRequest.DaysBeforeExpiration = 0;
                m_TimedBarsRequest.UpdatesEnabled = false;
                m_TimedBarsRequest.IgnoreEventsOnHistoricalBars = false;
                m_TimedBarsRequest.SessionsFilter = 0;
                m_TimedBarsRequest.SessionFlags = eSessionFlag.sfUndefined;
                m_TimedBarsRequest.ExcludeAllOutputs();
                foreach (eTimedBarsRequestOutputs selectedOutput in TimedBarsRequestOutputs)
                {
                    m_TimedBarsRequest.IncludeOutput(selectedOutput, true);
                }
                CQGTimedBars TimedBars = fakeCQGCel.RequestTimedBars(m_TimedBarsRequest);
                CQGTimedBars = fakeCQGCel.AllTimedBars.get_ItemById(TimedBars.Id);

                // assert
                Assert.IsNotNull(CQGTimedBars);
                Assert.AreEqual(25, CQGTimedBars.Count);
                recurrenceСount--;
            }
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
