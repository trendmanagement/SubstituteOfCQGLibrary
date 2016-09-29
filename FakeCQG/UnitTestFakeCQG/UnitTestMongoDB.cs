using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG;
using FakeCQG.Helpers;
using FakeCQG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestFakeCQG
{
    [TestClass]
    public class UnitTestMongoDB
    {
        [TestMethod]
        public void MethodAsync_PushOneQueryItem()
        {
            // arrange
            var qType = QueryInfo.QueryType.SetProperty;
            string idTrue = "keyTrue";
            string idFalse = "keyFalse";
            bool isQuery = default(bool);
            string name = "name";
            CQG.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act 1
                await queryHelper.PushQueryAsync(new QueryInfo(qType, idTrue, string.Empty, name, null, null));
                isQuery = await queryHelper.CheckQueryAsync(idTrue);

                // assert 1
                Assert.AreEqual(isQuery, true);

                // act 2
                isQuery = await queryHelper.CheckQueryAsync(idFalse);

                // assert 2
                Assert.AreEqual(isQuery, false);

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void MethodAsync_RemoveOneQueryItem()
        {
            // arrange
            var qType = QueryInfo.QueryType.CallMethod;
            string id = "key";
            bool isQuery = default(bool);
            string name = "name";
            CQG.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                await queryHelper.PushQueryAsync(new QueryInfo(qType, id, string.Empty, name, null, null));
                isQuery = await queryHelper.CheckQueryAsync(id);
                await queryHelper.RemoveQueryAsync(id);
                isQuery = await queryHelper.CheckQueryAsync(id);

            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(isQuery, false);
        }

        List<string> answersInfo;
        [TestMethod]
        public void MethodAsync_ReadInfoQueries()
        {
            // arrange
            var qType = QueryInfo.QueryType.SetProperty;
            string[] idTrue = { "key1True", "key2True", "key3True" };
            string idFalse = "keyFalse";
            string name = "name";
            CQG.LogChange += CQG_LogChange_ReadAll;
            var queryHelper = new QueryHelper();
            queryHelper.NewQueriesReady += CQG_GetQueries_Mock;
            answersInfo = new List<string>();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < idTrue.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, idTrue[i], string.Empty, name, null, null));
                }
                var keysOfQueriesInProgress = new HashSet<string>();
                await queryHelper.ReadQueriesAsync(keysOfQueriesInProgress);
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(answersInfo.Count, 3);
            for (int i = 0; i < answersInfo.Count; i++)
            {
                Assert.AreEqual(answersInfo[i].Contains(idTrue[i]), true);
                Assert.AreEqual(answersInfo[i].Contains(idFalse), false);
            }
        }

        List<QueryInfo> answers;
        [TestMethod]
        public void MethodAsync_GetAllQueries()
        {
            // arrange
            var qType = QueryInfo.QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3", "key4", "key5" };
            string name = "name";
            CQG.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();
            queryHelper.NewQueriesReady += CQG_GetQueries;
            answers = new List<QueryInfo>();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                var keysOfQueriesInProgress = new HashSet<string>();
                await queryHelper.ReadQueriesAsync(keysOfQueriesInProgress);
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(answers.Count, 5);
            for (int i = 0; i < answers.Count; i++)
            {
                Assert.AreEqual((answers[i].Key == keys[i]), true);
            }
        }

        List<string> removedAnswersInfo;
        [TestMethod]
        public void MethodAsync_RemoveAllQueries()
        {
            // arrange
            var qType = QueryInfo.QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3", "key4", "key5" };
            string name = "name";
            CQG.LogChange += CQG_LogChange_For_RemoveAll;
            var queryHelper = new QueryHelper();
            queryHelper.NewQueriesReady += CQG_GetQueries_Mock;
            removedAnswersInfo = new List<string>();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                await queryHelper.ClearQueriesListAsync();
                var keysOfQueriesInProgress = new HashSet<string>();
                await queryHelper.ReadQueriesAsync(keysOfQueriesInProgress);
            }).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(removedAnswersInfo[keys.Length + 1], "Queries list was cleared successfully");
        }

        [TestMethod]
        public void MethodAsync_PushOneAnswerItem()
        {
            // arrange
            string idTrue = "keyTrue";
            string idFalse = "keyFalse";
            string name = "name";
            bool isAnswer = default(bool);
            CQG.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                // act 1
                await answerHelper.PushAnswerAsync(new AnswerInfo(idTrue, string.Empty, name, null, null));
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
            CQG.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                await answerHelper.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));
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
            CQG.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await answerHelper.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            var answer = answerHelper.GetAnswerData(id, out isAnswer);

            // assert
            Assert.IsTrue(isAnswer);
            Assert.AreEqual(id, answer.Key);
        }

        [TestMethod]
        public void Method_DCEventHandler()
        {
            // arrange
            string id = "keyDCEventHandler";
            bool isAnswer = default(bool);
            string name = "name";
            object[] arguments = { "value1", "value2" };
            CQG.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await answerHelper.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            answerHelper.CommonEventHandler(name, arguments);
            var answer = answerHelper.GetAnswerData(id, out isAnswer);

            // assert
            Assert.IsTrue(isAnswer);
            Assert.AreEqual(id, answer.Key);
            //TODO: uncomment after completes deserialize from dictionary to bson
            //Assert.IsNotNull(answer.ArgValues);
            //Assert.AreEqual(arguments, answer.ArgValues[1]);
        }

        #region Extra lines in log message
        List<string> extraFields = new List<string>()
        {
            "queries in collection at",
            "**********************************************************",
            "Query"
        };
        #endregion

        private void CQG_LogChange_For_RemoveAll(string message)
        {
            removedAnswersInfo.Add(message);
        }

        private void CQG_GetQueries(List<QueryInfo> queries)
        {
            answers = queries;
        }

        private void CQG_LogChange_ReadAll(string message)
        {
            bool contains = false;
            foreach (string field in extraFields)
            {
                if (message.Contains(field))
                {
                    contains = false;
                    break;
                }
                else
                {
                    contains = true;
                }
            }
            if (contains)
            {
                answersInfo.Add(message);
            }
        }

        private void CQG_LogChange_Mock(string message)
        {
        }

        private void CQG_GetQueries_Mock(List<QueryInfo> queries)
        {
        }
    }

    [TestClass]
    public class UnitTestLogic
    {
        [TestMethod]
        public void Method_CreateQuery_Properties()
        {
            // arrange
            var queryType = QueryInfo.QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3", "key4" };
            string objectKey = default(string);
            string queryName = "name";
            var argumentKeys = new Dictionary<string, string>() { { "1", "argumentKey" } };
            var argumentValues = new Dictionary<string, object>() { { "1", "argumentValue" } };
            var queryList = new List<QueryInfo>();

            //act
            queryList.Add(CQG.CreateQuery(queryType, keys[0], objectKey, queryName, argumentKeys, argumentValues));
            queryList.Add(CQG.CreateQuery(queryType, keys[1], objectKey, queryName, argumentKeys, null));
            queryList.Add(CQG.CreateQuery(queryType, keys[2], objectKey, queryName, null, argumentValues));
            queryList.Add(CQG.CreateQuery(queryType, keys[3], objectKey, queryName, null, null));

            //assert
            Assert.AreEqual(keys[0], queryList[0].Key);
            Assert.AreEqual(argumentKeys["1"], queryList[0].ArgKeys["1"]);
            Assert.AreEqual(argumentValues["1"], queryList[0].ArgValues["1"]);

            Assert.AreEqual(keys[1], queryList[1].Key);
            Assert.AreEqual(argumentKeys["1"], queryList[1].ArgKeys["1"]);
            Assert.AreEqual(null, queryList[1].ArgValues);

            Assert.AreEqual(keys[2], queryList[2].Key);
            Assert.AreEqual(null, queryList[2].ArgKeys);
            Assert.AreEqual(argumentValues["1"], queryList[2].ArgValues["1"]);

            Assert.AreEqual(keys[3], queryList[3].Key);
            Assert.AreEqual(null, queryList[3].ArgKeys);
            Assert.AreEqual(null, queryList[3].ArgValues);
        }

        [TestMethod]
        public void Method_WaitingForTheAnswer()
        {
            // arrange
            string id = "key";
            string name = "name";
            CQG.LogChange += CQG_LogChange_Mock;
            var answerHelper = new AnswerHelper();
            var answerInput = new AnswerInfo(id, string.Empty, name, null, null);
            AnswerInfo answerOutput = null;
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await answerHelper.PushAnswerAsync(answerInput);
            }).GetAwaiter().GetResult();

            answerOutput = CQG.WaitingForAnAnswer(id, QueryInfo.QueryType.GetProperty);

            // assert
            Assert.AreEqual(answerInput.Key, answerOutput.Key);
            Assert.AreEqual(answerInput.QueryName, answerOutput.QueryName);
        }

        string NoAnswerMessage = string.Empty;
        //TODO: uncomment if not - const int QueryTemeout = int.MaxValue
        [TestMethod]
        public void Method_ExecuteTheQuery_TimerElapsed()
        {
            // arrange
            var queryType = QueryInfo.QueryType.SetProperty;
            string name = "name";
            Timer timer = new Timer();
            bool isThrownException = false;
            CQG.QueryTimeout = 1000;
            CQG.LogChange += CQG_LogChange_NoAnswer;
            var answerHelper = new AnswerHelper();
            var queryHelper = new QueryHelper();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            // act
            try
            {
                var answer = CQG.ExecuteTheQuery(queryType, string.Empty, name, null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (TimeoutException ex)
            {
                isThrownException = (ex == null) ? false : true;
            }

            // assert
            Assert.IsTrue(isThrownException);
        }

        private void CQG_LogChange_NoAnswer(string message)
        {
            NoAnswerMessage = message;
        }

        private void CQG_LogChange_Mock(string message)
        {
        }
    }
}