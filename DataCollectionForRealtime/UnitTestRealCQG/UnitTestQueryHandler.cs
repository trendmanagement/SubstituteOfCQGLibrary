using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataCollectionForRealtime;
using System.Collections.Generic;
using FakeCQG.Models;
using System.Threading.Tasks;

namespace UnitTestRealCQG
{
    [TestClass]
    public class UnitTestQueryHandler
    {
        QueryHandler QueryHandler;
        CQGDataManagement CQGDataManagment;
        RealtimeDataManagement RealtimeDataManagement;

        #region ctors

        [TestMethod]
        public void Ctor_InputCQGDataManagment()
        {
            // arrange
            RealtimeDataManagement = new RealtimeDataManagement();
            CQGDataManagment = new CQGDataManagement(RealtimeDataManagement);

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
            var queryType = QueryInfo.QueryType.Property;
            string[] keys = { "key1", "key2", "key3" };
            var list = new List<QueryInfo>()
            {
                new QueryInfo(queryType, keys[0], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[1], string.Empty, string.Empty, null, null),
                new QueryInfo(queryType, keys[2], string.Empty, string.Empty, null, null)
            };
            RealtimeDataManagement = new RealtimeDataManagement();
            CQGDataManagment = new CQGDataManagement(RealtimeDataManagement);

            // act
            QueryHandler = new QueryHandler(CQGDataManagment, list);

            // assert
            Assert.IsNotNull(QueryHandler.QueryList);
            Assert.IsNotNull(QueryHandler.CQGAssembly);
            Assert.AreEqual(list.Count, QueryHandler.QueryList.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i].Key, QueryHandler.QueryList[i].Key);
            }
        }

        #endregion

        #region methods

        [TestMethod]
        public void Method_SetQueryList()
        {
            // arrange
            var queryType = QueryInfo.QueryType.Property;
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
                Assert.AreEqual(list[i].Key, QueryHandler.QueryList[i].Key);
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
            FakeCQG.CQG.LogChange += CQG_LogChange;
            StartUp();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            // act 1
            Task.Run(async () =>
            {
                await FakeCQG.CQG.LoadInQueryAsync(new QueryInfo(QueryInfo.QueryType.Property, id, string.Empty, name, null, null));
                isQueryTrue = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.DeleteProcessedQuery(id);

            Task.Run(async () =>
            {
                isQueryFalse = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
        }

        [TestMethod]
        public void Method_LoadInAnswer()
        {
            // arrange
            string id = "key";
            string name = "name";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            var query = new QueryInfo(QueryInfo.QueryType.Property, id, string.Empty, name, null, null);
            var answer = new AnswerInfo(id, string.Empty, name, null, null);
            FakeCQG.CQG.LogChange += CQG_LogChange;
            StartUp();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await FakeCQG.CQG.LoadInQueryAsync(query);
                isQueryTrue = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.LoadInAnswer(answer);

            Task.Run(async () =>
            {
                isQueryFalse = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            // assert
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
        }

        [TestMethod]
        public void Method_QueryProcessing_QueryTypeConstructor_CQGCELQueryName()
        {
            // arrange
            string id = "key";
            string objectKey = "objectKey";
            string name = "CQG.CQGCELClass";
            bool isQueryTrue = default(bool);
            bool isQueryFalse = default(bool);
            var query = new QueryInfo(QueryInfo.QueryType.Constructor, id, objectKey, name, null, null);
            AnswerInfo answer = default(AnswerInfo);
            FakeCQG.CQG.LogChange += CQG_LogChange;
            FakeCQG.CQG.GetQueries += CQG_GetQueries;
            StartUp();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await FakeCQG.CQG.ClearAnswersAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await FakeCQG.CQG.LoadInQueryAsync(query);
                isQueryTrue = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.QueryProcessing(query);

            Task.Run(async () =>
            {
                isQueryFalse = await FakeCQG.CQG.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();
            answer = FakeCQG.CQG.GetAnswerData(id);
            var objectValue = FakeCQG.DataDictionaries.GetObjectFromTheDictionary(objectKey);

            // assert
            Assert.AreEqual(name, answer.QueryName);
            Assert.AreEqual(objectKey, answer.ObjectKey);
            Assert.IsTrue(isQueryTrue);
            Assert.IsFalse(isQueryFalse);
            Assert.IsNull(objectValue);
        }

        private void CQG_GetQueries(List<QueryInfo> queries)
        {
        }

        private void CQG_LogChange(string message)
        {
        }

        #endregion

        void StartUp()
        {
            RealtimeDataManagement = new RealtimeDataManagement();
            CQGDataManagment = new CQGDataManagement(RealtimeDataManagement);
            QueryHandler = new QueryHandler(CQGDataManagment);
        }
    }
}
internal class MyClass
    {
    }

