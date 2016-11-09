using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DataCollectionForRealtime;
using FakeCQG;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRealCQG
{
    [TestClass]
    [DeploymentItem("Interop.CQG.dll")]
    public class UnitTestQueryHandler
    {
        QueryHandler QueryHandler;
        CQGDataManagement CQGDataManagment;
        DCMainForm DCMainForm;

        #region ctors

        [TestMethod]
        public void Ctor_InputCQGDataManagment()
        {
            // arrange
            DCMainForm = new DCMainForm();
            CQGDataManagment = new CQGDataManagement(DCMainForm, null);

            // act
            QueryHandler = new QueryHandler(CQGDataManagment);

            // assert
            Assert.IsNotNull(QueryHandler.QueryList);
            Assert.IsNotNull(QueryHandler.CQGAssembly);
        }

        [TestMethod]
        public void Ctor_InputCQGDataManagmentWithQueriesList()
        {
            // arrange
            var queryType = QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3" };
            var list = new List<QueryInfo>()
            {
                new QueryInfo(queryType, keys[0], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[1], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[2], string.Empty, string.Empty, null, null)
            };
            DCMainForm = new DCMainForm();
            CQGDataManagment = new CQGDataManagement(DCMainForm, null);

            // act
            QueryHandler = new QueryHandler(CQGDataManagment, list);

            // assert
            Assert.IsNotNull(QueryHandler.QueryList);
            Assert.IsNotNull(QueryHandler.CQGAssembly);
            Assert.AreEqual(list.Count, QueryHandler.QueryList.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i].QueryKey, QueryHandler.QueryList[i].QueryKey);
            }
        }

        #endregion

        #region methods

        [TestMethod]
        public void Method_SetQueryList()
        {
            // arrange
            var queryType = QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3" };
            var list = new List<QueryInfo>()
            {
                new QueryInfo(queryType, keys[0], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[1], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[2], string.Empty, string.Empty, null, null)
            };
            StartUp();

            // act
            QueryHandler.SetQueryList(list);

            // assert
            Assert.IsNotNull(QueryHandler.QueryList);
            Assert.AreEqual(list.Count, QueryHandler.QueryList.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i].QueryKey, QueryHandler.QueryList[i].QueryKey);
            }
        }

        [TestMethod]
        public void Method_FindDelegateType()
        {
            // arrange
            string[,] names =
            {
                { "TimedBarsResolved", "_ICQGCELEvents_TimedBarsResolvedEventHandler" },
                { "TimedBarsAdded", "_ICQGCELEvents_TimedBarsAddedEventHandler" },
                { "TimedBarsUpdated", "_ICQGCELEvents_TimedBarsUpdatedEventHandler"}
            };
            Type[] types = new Type[names.GetLength(0)];
            StartUp();

            // act
            var nullType = QueryHandler.FindDelegateType(QueryHandler.CQGAssembly, "name");
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = QueryHandler.FindDelegateType(QueryHandler.CQGAssembly, names[i, 0]);
            }

            // assert
            Assert.IsNull(nullType);
            for (int i = 0; i < types.Length; i++)
            {
                Assert.IsNotNull(types[i]);
                Assert.AreEqual(names[i, 1], types[i].Name);
            }
        }

        delegate void myDelegate1();
        delegate void myDelegate2();
        delegate void myDelegate3();
        [TestMethod]
        public void Method_IsDelegate()
        {
            // arrange
            Type[] noDelegateTypes = { typeof(string), typeof(int), typeof(MyClass) };
            Type[] delegateTypes = { typeof(myDelegate1), typeof(myDelegate2), typeof(myDelegate3) };
            List<bool> resaltFalse = new List<bool>();
            List<bool> resaltTrue = new List<bool>();
            StartUp();

            // act
            foreach (var type in noDelegateTypes)
            {
                resaltFalse.Add(QueryHandler.IsDelegate(type));
            }
            foreach (var type in delegateTypes)
            {
                resaltTrue.Add(QueryHandler.IsDelegate(type));
            }

            // assert
            for (int i = 0; i < resaltFalse.Count; i++)
            {
                Assert.IsFalse(resaltFalse[i]);
                Assert.IsTrue(resaltTrue[i]);
            }
        }

        [TestMethod]
        public void Method_DeleteProcessedQuery()
        {
            // arrange
            string id = "key";
            string name = "name";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            Core.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            StartUp();

            // act 1
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(new QueryInfo(QueryType.SetProperty, id, string.Empty, name, null, null));
                isQueryTrue = await QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.DeleteProcessedQuery(id);

            Task.Run(async () =>
            {
                isQueryFalse = await QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
        }

        [TestMethod]
        public void Method_PushAnswer()
        {
            // arrange
            string id = "key";
            string name = "name";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            var query = new QueryInfo(QueryType.SetProperty, id, string.Empty, name, null, null);
            var answer = new AnswerInfo(id, string.Empty, name, null, null);
            Core.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQueryTrue = await QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.PushAnswerAndDeleteQuery(answer);

            Task.Run(async () =>
            {
                isQueryFalse = await QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
        }

        List<string> loggedInfo;
        [TestMethod]
        public void MethodAsync_ReadInfoQueries()
        {
            // arrange
            var qType = QueryType.SetProperty;
            string[] idTrue = { "key1True", "key2True", "key3True" };
            string idFalse = "keyFalse";
            string name = "name";
            Core.LogChange += CQG_LogChange_ReadAll;
            var queryHelper = new QueryHelper();  
            StartUp();
            QueryHandler.NewQueriesReady += CQG_GetQueries_Mock;
            loggedInfo = new List<string>();

           Task.Run(async () =>
            {
                // act
                for (int i = 0; i < idTrue.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, idTrue[i], string.Empty, name, null, null));
                }
                QueryHandler.ReadQueries();
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(loggedInfo.Count, 3);
            for (int i = 0; i < loggedInfo.Count; i++)
            {
                Assert.AreEqual(loggedInfo[i].Contains(idTrue[i]), true);
                Assert.AreEqual(loggedInfo[i].Contains(idFalse), false);
            }
        }

        List<QueryInfo> answers;
        [TestMethod]
        public void MethodAsync_GetAllQueries()
        {
            // arrange
            var qType = QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3", "key4", "key5" };
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            StartUp();
            QueryHandler.NewQueriesReady += CQG_GetQueries;
            answers = new List<QueryInfo>();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                QueryHandler.ReadQueries();
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(answers.Count, 5);
            for (int i = 0; i < answers.Count; i++)
            {
                Assert.AreEqual((answers[i].QueryKey == keys[i]), true);
            }
        }

        [TestMethod]
        public void MethodAsync_RemoveOneQueryItem()
        {
            // arrange
            var qType = QueryType.CallMethod;
            string id = "key";
            bool isQuery = default(bool);
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            StartUp();

            Task.Run(async () =>
            {
                // act
                await queryHelper.PushQueryAsync(new QueryInfo(qType, id, string.Empty, name, null, null));
                isQuery = await QueryHandler.CheckQueryAsync(id);
                await QueryHandler.RemoveQueryAsync(id);
                isQuery = await QueryHandler.CheckQueryAsync(id);

            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(isQuery, false);
        }

        [TestMethod]
        public void MethodAsync_PushOneAnswerItem()
        {
            // arrange
            string idTrue = "keyTrue";
            string idFalse = "keyFalse";
            string name = "name";
            bool isAnswer = default(bool);
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                // act 1
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(idTrue, string.Empty, name, null, null));
                isAnswer = await answerHelper.CheckAnswerAsync(idTrue);

                // assert 1
                Assert.AreEqual(isAnswer, true);

                // act 2
                isAnswer = await answerHelper.CheckAnswerAsync(idFalse);

                // assert 2
                Assert.AreEqual(isAnswer, false);

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void MethodAsync_RemoveOneAnswerItem()
        {
            // arrange
            string id = "key";
            bool isAnswer = default(bool);
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));
                isAnswer = await answerHelper.CheckAnswerAsync(id);
                await answerHelper.RemoveAnswerAsync(id);
                isAnswer = await answerHelper.CheckAnswerAsync(id);

            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(isAnswer, false);
        }

        [TestMethod]
        public void Method_GetAnswerData()
        {
            // arrange
            AnswerInfo answer = default(AnswerInfo);
            string id = "key";
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name));
                answer = answerHelper.GetAnswerData(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void Method_DCEventHandler()
        {
            // arrange
            string id = "keyDCEventHandler";
            string name = "name";
            var argValues = new Dictionary<int, object>() { { 0, "value1" }, { 1, "value2" } };
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, argValues));
            }).GetAwaiter().GetResult();
            var answer = answerHelper.GetAnswerData(id);

            // assert
            Assert.AreEqual(id, answer.AnswerKey);
            Assert.IsNotNull(answer.ArgValues);
            Assert.AreEqual("value1", answer.ArgValues[0]);
            Assert.AreEqual("value2", answer.ArgValues[1]);
        }

        #endregion

        #region Logic

        [TestMethod]
        public void ProcessedQuery_WithFastAct()
        {
            // arrange
            string id = "key";
            string name = "name";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            StartUp();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, string.Empty, name);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            QueryHandler.ProcessQuery(query);

            Task.Run(async () =>
            {
                isQuery = await QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        //10 sec act test 
        public async void ProcessedQuery_WithLongAct()
        {
            // arrange
            string id = "key";
            string name = "name";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            StartUp();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, string.Empty, name);
            await queryHelper.PushQueryAsync(query);
            isQuery = await QueryHandler.CheckQueryAsync(id);

            // assert 1
            Assert.IsTrue(isQuery);

            QueryHandler.ProcessQuery(query);

            //Waight some time after procssed query
            Task.Delay(10000).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                isQuery = await QueryHandler.CheckQueryAsync(id);

                // assert 2
                Assert.IsFalse(isQuery);
            }).GetAwaiter().GetResult();

            var answer = answerHelper.GetAnswerData(id);

            // assert 3
            Assert.IsNotNull(answer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void ProcessedQuery_CheckingQueryAnswerAppropriate()
        {
            // arrange
            #region Timer
            Timer timer = new Timer();
            timer.Interval = 30;
            timer.AutoReset = true;
            timer.Elapsed += TimerElapsed_CheckingQueryAnswerAppropriate;
            timer.Start();
            #endregion

            string[] ids = { "key1",  "key2",  "key3",  "key4",  "key5",  "key6",  "key7",  "key8",  "key9",  "key10",
                             "key11", "key12", "key13", "key14", "key15", "key16", "key17", "key18", "key19", "key20",
                             "key21", "key22", "key23", "key24", "key25", "key26", "key27", "key28", "key29", "key30",
                             "key31", "key32", "key33", "key34", "key35", "key36", "key37", "key38", "key39", "key40",
                             "key41", "key42", "key43", "key44", "key45", "key46", "key47", "key48", "key49", "key50",
                             "key51", "key52", "key53", "key54", "key55", "key56", "key57", "key58", "key59", "key60",
                             "key61", "key62", "key63", "key64", "key65", "key66", "key67", "key68", "key69", "key70",
                             "key71", "key72", "key73", "key74", "key75", "key76", "key77", "key78", "key79", "key80",
                             "key81", "key82", "key83", "key84", "key85", "key86", "key87", "key88", "key89", "key90",
                             "key91", "key92", "key93", "key94", "key95", "key96", "key97", "key98", "key99", "key100"};

            string name = "name";
            var queries = new List<QueryInfo>();
            var answers = new List<AnswerInfo>();
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            StartUp();

            // act
            for(int i = 0; i < ids.Length; i++)
            {
                var query = Core.CreateQuery(QueryType.CallCtor, ids[i], string.Empty, name);
                queries.Add(query);
            }

            foreach (var query in queries)
            {
                Task.Run(async () =>
                {
                    await queryHelper.PushQueryAsync(query);
                }).GetAwaiter().GetResult();
            }

            foreach (var id in ids)
            {
                var answer = answerHelper.GetAnswerData(id);
                answers.Add(answer);
            }

            // assert
            Assert.AreEqual(answers.Count, queries.Count);
            for(int i = 0; i < answers.Count; i++)
            {
                Assert.IsTrue(queries[i].QueryKey == answers[i].AnswerKey);
            }
        }

        public string status = string.Empty;
        [TestMethod]
        //This test should run if CQG client is connected
        public void FakeCQG_EventHandlersWork()
        {
            // arrange
            StartUp();
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

        #endregion

        #region Helpers

        void StartUp()
        {
            DCMainForm = new DCMainForm();
            CQGDataManagment = new CQGDataManagement(DCMainForm, null);
            QueryHandler = new QueryHandler(CQGDataManagment);
            QueryHandler.QueryList = new List<QueryInfo>();
            QueryHandler.HelpersInit();

            Task.Run(async () =>
            {
                await QueryHandler.ClearQueriesListAsync();
                await Core.AnswerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
        }

        // Parts of log messages to skip
        List<string> skippedMsgParts = new List<string>()
        {
            "Queries list was cleared successfully",
            "Answers list was cleared successfully",
            "Events list was cleared successfully",
            "**********************************************************",
            " new quer(y/ies) in database at ",
            "QUERY"
        };

        private void CQG_LogChange(string message)
        {
        }

        private void CQG_LogChange_ReadAll(string message)
        {
            if (skippedMsgParts.All(skippedMsgPart => !message.Contains(skippedMsgPart)))
            {
                loggedInfo.Add(message);
            }
        }

        private void CQG_GetQueries(List<QueryInfo> queries)
        {
            answers = queries;
        }

        private void CQG_LogChange_Mock(string message)
        {
        }

        private void CQG_GetQueries_Mock(List<QueryInfo> queries)
        {
        }

        private void Cell_DataConnectionStatusChanged(eConnectionStatus new_status)
        {
            status = new_status.ToString();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            QueryHandler.ReadQueries();
            QueryHandler.ProcessEntireQueryList();
        }

        private void TimerElapsed_CheckingQueryAnswerAppropriate(object sender, ElapsedEventArgs e)
        {
            QueryHandler.ReadQueries();
            QueryHandler.ProcessEntireQueryList();
        }
        #endregion
    }

    internal class MyClass
    {
    }
}


