﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataCollectionForRealtime;
using FakeCQG.Helpers;
using FakeCQG.Models;
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
            FakeCQG.CQG.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            StartUp();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();

            // act 1
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(new QueryInfo(QueryType.SetProperty, id, string.Empty, name, null, null));
                isQueryTrue = await queryHelper.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            queryHelper.DeleteProcessedQuery(id);

            Task.Run(async () =>
            {
                isQueryFalse = await queryHelper.CheckQueryAsync(id);
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
            FakeCQG.CQG.LogChange += CQG_LogChange;
            var queryHelper = new QueryHelper();
            var answerHelper = new AnswerHelper();
            StartUp();
            Task.Run(async () =>
            {
                await queryHelper.ClearQueriesListAsync();
            }).GetAwaiter().GetResult();
            Task.Run(async () =>
            {
                await answerHelper.ClearAnswersListAsync();
            }).GetAwaiter().GetResult();

            // act
            Task.Run(async () =>
            {
                await queryHelper.PushQueryAsync(query);
                isQueryTrue = await queryHelper.CheckQueryAsync(id);
            }).GetAwaiter().GetResult();

            QueryHandler.PushAnswerAndDeleteQuery(answer);

            Task.Run(async () =>
            {
                isQueryFalse = await queryHelper.CheckQueryAsync(id);
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
            FakeCQG.CQG.InitializeServer(null, null);
            QueryHandler = new QueryHandler(CQGDataManagment);
        }
    }
}

internal class MyClass
{
}
