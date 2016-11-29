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
using MongoDB.Driver;

namespace UnitTestRealCQG
{
    [TestClass]
    [DeploymentItem("Interop.CQG.dll")]
    public class UnitTestQueryHandler
    {
        #region ctors

        [TestMethod]
         public void Ctor_InputCQGDataManagment()
        {
            // arrange
            UnitTestHelper.DCMainForm = new DCMainForm();
            UnitTestHelper.CQGDataManagment = new CQGDataManagement(UnitTestHelper.DCMainForm, null);

            // act
            UnitTestHelper.QueryHandler = new QueryHandler(UnitTestHelper.CQGDataManagment);

            // assert
            Assert.IsNotNull(UnitTestHelper.QueryHandler.QueryList);
            Assert.IsNotNull(UnitTestHelper.QueryHandler.CQGAssembly);
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
            UnitTestHelper.DCMainForm = new DCMainForm();
            UnitTestHelper.CQGDataManagment = new CQGDataManagement(UnitTestHelper.DCMainForm, null);

            // act
            UnitTestHelper.QueryHandler = new QueryHandler(UnitTestHelper.CQGDataManagment, list);

            // assert
            Assert.IsNotNull(UnitTestHelper.QueryHandler.QueryList);
            Assert.IsNotNull(UnitTestHelper.QueryHandler.CQGAssembly);
            Assert.AreEqual(list.Count, UnitTestHelper.QueryHandler.QueryList.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i].QueryKey, UnitTestHelper.QueryHandler.QueryList[i].QueryKey);
            }
        }

        #endregion

        #region methods

        [TestMethod]
        public void Method_SetQueryList()
        {
            // arrange
            var queryType = QueryType.SetProperty;
            string[] keys = { "SetQueryList1", "SetQueryList2", "SetQueryList3" };
            var list = new List<QueryInfo>()
            {
                new QueryInfo(queryType, keys[0], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[1], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[2], string.Empty, string.Empty, null, null)
            };
            UnitTestHelper.StartUp();

            // act
            UnitTestHelper.QueryHandler.SetQueryList(list);

            // assert
            Assert.IsNotNull(UnitTestHelper.QueryHandler.QueryList);
            Assert.AreEqual(list.Count, UnitTestHelper.QueryHandler.QueryList.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i].QueryKey, UnitTestHelper.QueryHandler.QueryList[i].QueryKey);
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
            UnitTestHelper.StartUp();

            // act
            var nullType = QueryHandler.FindDelegateType(UnitTestHelper.QueryHandler.CQGAssembly, "name");
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = QueryHandler.FindDelegateType(UnitTestHelper.QueryHandler.CQGAssembly, names[i, 0]);
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
            UnitTestHelper.StartUp();

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
            string id = "DeleteProcessedQuery";
            string name = "DeleteProcessedQuery";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            Core.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            UnitTestHelper.StartUp();

            // act 1
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(new QueryInfo(QueryType.SetProperty, id, string.Empty, name, null, null));
                isQueryTrue = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.DeleteProcessedQuery(id);

            Task.Run(async () =>
            {
                isQueryFalse = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
        }

        [TestMethod]
        public void Method_PushAnswer()
        {
            // arrange
            string id = "PushAnswer";
            string name = "PushAnswer";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            var query = new QueryInfo(QueryType.SetProperty, id, string.Empty, name, null, null);
            var answer = new AnswerInfo(id, string.Empty, name, null, null);
            Core.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            UnitTestHelper.StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQueryTrue = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.PushAnswerAndDeleteQuery(answer);

            Task.Run(async () =>
            {
                isQueryFalse = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
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
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.NewQueriesReady += CQG_GetQueries;
            answers = new List<QueryInfo>();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                UnitTestHelper.QueryHandler.ReadQueries();
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
            string id = "RemoveOneQueryItem";
            bool isQuery = default(bool);
            string name = "RemoveOneQueryItem";
            Core.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            UnitTestHelper.StartUp();

            Task.Run(async () =>
            {
                // act
                await queryHelper.PushQueryAsync(new QueryInfo(qType, id, string.Empty, name, null, null));
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);
                await UnitTestHelper.QueryHandler.RemoveQueryAsync(id);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

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
            string id = "RemoveOneAnswerItem";
            bool isAnswer = default(bool);
            string name = "RemoveOneAnswerItem";
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            UnitTestHelper.StartUp();
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
            string id = "GetAnswerData";
            bool isAnswer = default(bool);
            string name = "GetAnswerData";
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            UnitTestHelper.StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name));
                answer = answerHelper.GetAnswerData(id, out isAnswer);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isAnswer);
            Assert.AreEqual(id, answer.AnswerKey);
        }

        [TestMethod]
        public void Method_DCEventHandler()
        {
            // arrange
            string id = "keyDCEventHandler";
            bool isAnswer = default(bool);
            string name = "DCEventHandler";
            var argValues = new Dictionary<int, object>() { { 0, "value1" }, { 1, "value2" } };
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            UnitTestHelper.StartUp();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, argValues));
            }).GetAwaiter().GetResult();
            var answer = answerHelper.GetAnswerData(id, out isAnswer);

            // assert
            Assert.IsTrue(isAnswer);
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
            string id = "FastAct";
            string name = "FastAct";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, string.Empty, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.ProcessQuery(query);

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

        //10 sec act test 
        [TestMethod]
        public async void ProcessedQuery_WithLongAct()
        {
            // arrange
            string id = "LongAct";
            string name = "LongAct";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, string.Empty, name, string.Empty);
            await queryHelper.PushQueryAsync(query);
            isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

            // assert 1
            Assert.IsTrue(isQuery);

            UnitTestHelper.QueryHandler.ProcessQuery(query);

            //Waight some time after procssed query
            Task.Delay(10000).GetAwaiter().GetResult();

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

            int countQueries = 1000;
            string[] ids = new string[countQueries];
            var queries = new List<QueryInfo>();
            var answers = new List<AnswerInfo>();
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();

            //Generate values array
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = string.Concat("key", i.ToString());
            }

            // act
            for (int i = 0; i < ids.Length; i++)
            {
                var query = Core.CreateQuery(QueryType.CallCtor, ids[i], string.Empty, string.Empty, string.Empty);
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
            Assert.AreEqual(countQueries, queries.Count);
            Assert.AreEqual(countQueries, answers.Count);
            for (int i = 0; i < answers.Count; i++)
            {
                Assert.IsTrue(queries[i].QueryKey == answers[i].AnswerKey);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AutoGenQueryProcessing_TrowArgumentException()
        {
            // arrange
            string id = "TrowArgumentException";
            string name = "TrowArgumentException";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, string.Empty, name, string.Empty);
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQuery = await UnitTestHelper.QueryHandler.CheckQueryAsync(id);

                // assert 1
                Assert.IsTrue(isQuery);
            }).GetAwaiter().GetResult();

            UnitTestHelper.QueryHandler.AutoGenQueryProcessing(query);

            // assert 2
            Assert.Fail("");
        }

        [TestMethod]
        public void AutoGenQueryProcessing_CreateCtor()
        {
            // arrange
            string id = "CreateCtor";
            string name = "CreateCtor";
            string objType = "CQGCELClass";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, objType, name, string.Empty);
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
        public void AutoGenQueryProcessing_CreateDctor()
        {
            // arrange
            string id = "CreateDctor";
            string name = "CreateDctor";
            string objType = "CQGCELClass";
            bool isQuery;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            Core.LogChange += CQG_LogChange;
            UnitTestHelper.StartUp();
            UnitTestHelper.QueryHandler.InitHMethodDict();

            // act
            var query = Core.CreateQuery(QueryType.CallCtor, id, objType, name, string.Empty);
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
        }

        [TestMethod]
        public void SimultaneouslyPush2ListsOfAnswers()
        {
            // arrange
            string id = "key";
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            List<AnswerInfo> answerList = new List<AnswerInfo>();
            int countQueries = 100;
            UnitTestHelper.StartUp();

            for (int i = 0; i < countQueries; i++)
            {
                answerList.Add(new AnswerInfo($"{id}{i.ToString()}", string.Empty, name, null, null));
            }

            // act
            Task.Run(() =>
            {
                //Simulation of simultaneous pushing answers by two client applications
                Task.Run(async () =>
                {
                    for (int i = 0; i < countQueries / 2; i++)
                    {
                        await AnswerHandler.PushAnswerAsync(answerList[i]);
                    } 
                });

                Task.Run(async () =>
                {
                    for (int i = countQueries / 2; i < countQueries; i++)
                    {
                        await AnswerHandler.PushAnswerAsync(answerList[i]);
                    }
                });

                Task.Delay(1000).GetAwaiter().GetResult();
            }).GetAwaiter().GetResult();

            var filter = Builders<AnswerInfo>.Filter.Empty;
            var collection = Core.AnswerHelper.GetCollection.Find(filter).ToList();

            // assert
            Assert.AreEqual(countQueries, collection.Count);
        }

        #endregion

        #region Helpers

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

        private void CQG_GetQueries(List<QueryInfo> queries)
        {
            answers = queries;
        }

        private void CQG_LogChange_Mock(string message)
        {
        }

        private void TimerElapsed_CheckingQueryAnswerAppropriate(object sender, ElapsedEventArgs e)
        {
            UnitTestHelper.QueryHandler.ReadQueries();
            UnitTestHelper.QueryHandler.ProcessEntireQueryList();
        }

        #endregion
    }

    internal class MyClass
    {
    }
}


