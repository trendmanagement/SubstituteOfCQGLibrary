using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataCollectionForRealtime;
using System.Collections.Generic;
using FakeCQG.Models;
using System.Reflection;
using System.IO;

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
            string[] keys = { "key1",  "key2", "key3" };
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
            for(int i = 0; i < types.Length; i++)
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

        #endregion

        void StartUp()
        {
            RealtimeDataManagement = new RealtimeDataManagement();
            CQGDataManagment = new CQGDataManagement(RealtimeDataManagement);
            QueryHandler = new QueryHandler(CQGDataManagment);
        }
    }
}
