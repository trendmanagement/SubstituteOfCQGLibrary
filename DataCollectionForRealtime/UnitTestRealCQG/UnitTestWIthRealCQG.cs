using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
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

        #region AutoGenQueryProcessing

        [TestMethod]
        public void AutoGenQueryProcessing_GetProperty()
        {
            // arrange
            string id = "GetProperty";
            string name = "GetProperty";
            string objType = "CQGAccountClassAccountMarginDetailing";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.GetProperty, id, objType, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            Task.Run(async () =>
            {
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void AutoGenQueryProcessing_SetProperty()
        {
            // arrange
            string id = "SetProperty";
            string name = "SetProperty";
            string objType = "CQGAccountClassAccountMarginDetailing";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.SetProperty, id, objType, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            Task.Run(async () =>
            {
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void AutoGenQueryProcessing_CallMethod()
        {
            // arrange
            string id = "CallMethod";
            string name = "CallMethod";
            string objType = "_ICQGCELEvents_SinkHelperAccountChanged";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.CallMethod, id, objType, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            Task.Run(async () =>
            {
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void AutoGenQueryProcessing_SubscribeToEvent()
        {
            // arrange
            string id = "key";
            string name = "name";
            string objType = "_ICQGCELEvents_EventAccountChanged";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.SubscribeToEvent, id, objType, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            Task.Run(async () =>
            {
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void AutoGenQueryProcessing_UnsubscribeFromEvent()
        {
            // arrange
            string id = "key";
            string name = "name";
            string objType = "_ICQGCELEvents_EventAccountChanged";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.UnsubscribeFromEvent, id, objType, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            Task.Run(async () =>
            {
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        #endregion

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
