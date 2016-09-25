using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act 1
                await FakeCQG.CQG.PushQueryAsync(new QueryInfo(qType, idTrue, string.Empty, name, null, null));
                isQuery = await FakeCQG.CQG.CheckQueryAsync(idTrue);

                // assert 1
                Assert.AreEqual(isQuery, true);

                // act 2
                isQuery = await FakeCQG.CQG.CheckQueryAsync(idFalse);

                // assert 2
                Assert.AreEqual(isQuery, false);

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void MethodAsync_RemoveOneQueryItem()
        {
            // arrange
            var qType = QueryInfo.QueryType.Method;
            string id = "key";
            bool isQuery = default(bool);
            string name = "name";
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                await FakeCQG.CQG.PushQueryAsync(new QueryInfo(qType, id, string.Empty, name, null, null));
                isQuery = await FakeCQG.CQG.CheckQueryAsync(id);
                await FakeCQG.CQG.RemoveQueryAsync(id);
                isQuery = await FakeCQG.CQG.CheckQueryAsync(id);

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
            FakeCQG.CQG.LogChange += CQG_LogChange_ReadAll;
            FakeCQG.CQG.GetQueries += CQG_GetQueries_Mock;
            answersInfo = new List<string>();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < idTrue.Length; i++)
                {
                    await FakeCQG.CQG.PushQueryAsync(new QueryInfo(qType, idTrue[i], string.Empty, name, null, null));
                }
                await FakeCQG.CQG.ReadQueriesAsync();
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
            FakeCQG.CQG.GetQueries += CQG_GetQueries;
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            answers = new List<QueryInfo>();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await FakeCQG.CQG.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                await FakeCQG.CQG.ReadQueriesAsync();
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
            FakeCQG.CQG.LogChange += CQG_LogChange_For_RemoveAll;
            FakeCQG.CQG.GetQueries += CQG_GetQueries_Mock;
            removedAnswersInfo = new List<string>();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                for (int i = 0; i < keys.Length; i++)
                {
                    await FakeCQG.CQG.PushQueryAsync(new QueryInfo(qType, keys[i], string.Empty, name, null, null));
                }
                await FakeCQG.CQG.ClearQueriesListAsync();
                await FakeCQG.CQG.ReadQueriesAsync();
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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                // act 1
                await FakeCQG.CQG.PushAnswerAsync(new AnswerInfo(idTrue, string.Empty, name, null, null));
                isAnswer = await FakeCQG.CQG.CheckAnswerAsync(idTrue);

                // assert 1
                Assert.AreEqual(isAnswer, true);

                // act 2
                isAnswer = await FakeCQG.CQG.CheckAnswerAsync(idFalse);

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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                // act
                await FakeCQG.CQG.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));
                isAnswer = await FakeCQG.CQG.CheckAnswerAsync(id);
                await FakeCQG.CQG.RemoveAnswerAsync(id);
                isAnswer = await FakeCQG.CQG.CheckAnswerAsync(id);

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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await FakeCQG.CQG.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            var answer = FakeCQG.CQG.GetAnswerData(id, out isAnswer);

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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await FakeCQG.CQG.PushAnswerAsync(new AnswerInfo(id, string.Empty, name, null, null));

            }).GetAwaiter().GetResult();
            FakeCQG.CQG.CommonEventHandler(name, arguments);
            var answer = FakeCQG.CQG.GetAnswerData(id, out isAnswer);

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
            "Queries list was cleared successfully",
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
            foreach (var field in extraFields)
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
            queryList.Add(FakeCQG.CQG.CreateQuery(queryType, keys[0], objectKey, queryName, argumentKeys, argumentValues));
            queryList.Add(FakeCQG.CQG.CreateQuery(queryType, keys[1], objectKey, queryName, argumentKeys, null));
            queryList.Add(FakeCQG.CQG.CreateQuery(queryType, keys[2], objectKey, queryName, null, argumentValues));
            queryList.Add(FakeCQG.CQG.CreateQuery(queryType, keys[3], objectKey, queryName, null, null));

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
            FakeCQG.CQG.LogChange += CQG_LogChange_Mock;
            var answerInput = new AnswerInfo(id, string.Empty, name, null, null);
            AnswerInfo answerOutput = null;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await FakeCQG.CQG.PushAnswerAsync(answerInput);
            }).GetAwaiter().GetResult();

            answerOutput = FakeCQG.CQG.WaitingForAnAnswer(id);

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
            FakeCQG.CQG.QueryTimeout = 1000;
            FakeCQG.CQG.LogChange += CQG_LogChange_NoAnswer;
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            // act
            try
            {
                var answer = FakeCQG.CQG.ExecuteTheQuery(queryType, string.Empty, name, null);
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