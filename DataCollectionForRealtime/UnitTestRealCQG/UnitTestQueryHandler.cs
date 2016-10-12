using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCollectionForRealtime;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRealCQG
{
    [TestClass]
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

        private void CQG_LogChange(string message)
        {
        }

        #endregion

        void StartUp()
        {
            DCMainForm = new DCMainForm();
            CQGDataManagment = new CQGDataManagement(DCMainForm, null);  
            QueryHandler = new QueryHandler(CQGDataManagment);
            QueryHandler.HelpersInit();

            Task.Run(async () =>
            {
                await QueryHandler.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();
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
            string id = "key";
            bool isAnswer = default(bool);
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            var answer = answerHelper.GetAnswerData(id, out isAnswer);

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
            string name = "name";
            var argValues = new Dictionary<int, object>() { { 0, "value1" }, { 1, "value2" } };
            Core.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await AnswerHandler.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            var answer = answerHelper.GetAnswerData(id, out isAnswer);

            // assert
            Assert.IsTrue(isAnswer);
            Assert.AreEqual(id, answer.AnswerKey);
            //TODO: uncomment after completes deserialize from dictionary to bson
            //Assert.IsNotNull(answer.ArgValues);
            //Assert.AreEqual(arguments, answer.ArgValues[1]);
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

    }

}



internal class MyClass
{
}
