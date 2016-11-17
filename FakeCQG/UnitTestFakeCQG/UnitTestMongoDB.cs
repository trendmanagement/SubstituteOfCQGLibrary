using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
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
            var qType = QueryType.SetProperty;
            string idTrue = "keyTrue";
            bool isTrue = default(bool);
            string name = "name";
            Core.LogChange += CQG_LogChange_Mock;
            var queryHelper = new QueryHelper();

            Task.Run(async () =>
            {
                try
                {
                    await queryHelper.PushQueryAsync(new QueryInfo(qType, idTrue, string.Empty, name, null, null));
                    isTrue = true;
                }
                catch(Exception ex)
                {
                    isTrue = false;
                    CQG_LogChange_Mock(ex.ToString());
                }
                
                Assert.AreEqual(isTrue, true);

            }).GetAwaiter().GetResult();
        }

        private void CQG_LogChange_Mock(string message)
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
            var queryType = QueryType.SetProperty;
            string[] keys = { "key1", "key2", "key3", "key4" };
            string objectType = default(string);
            string objectKey = default(string);
            string memberName = "name";
            var argumentKeys = new Dictionary<int, string>() { { 0, "argumentKey" } };
            var argumentValues = new Dictionary<int, object>() { { 1, "argumentValue" } };
            var queryList = new List<QueryInfo>();

            // act
            queryList.Add(Core.CreateQuery(queryType, keys[0], objectType, objectKey, memberName, argumentKeys, argumentValues));
            queryList.Add(Core.CreateQuery(queryType, keys[1], objectType, objectKey, memberName, argumentKeys, null));
            queryList.Add(Core.CreateQuery(queryType, keys[2], objectType, objectKey, memberName, null, argumentValues));
            queryList.Add(Core.CreateQuery(queryType, keys[3], objectType, objectKey, memberName, null, null));

            // assert
            Assert.AreEqual(keys[0], queryList[0].QueryKey);
            Assert.AreEqual(argumentKeys[0], queryList[0].ArgKeys[0]);
            Assert.AreEqual(argumentValues[1], queryList[0].ArgValues[1]);

            Assert.AreEqual(keys[1], queryList[1].QueryKey);
            Assert.AreEqual(argumentKeys[0], queryList[1].ArgKeys[0]);
            Assert.AreEqual(null, queryList[1].ArgValues);

            Assert.AreEqual(keys[2], queryList[2].QueryKey);
            Assert.AreEqual(null, queryList[2].ArgKeys);
            Assert.AreEqual(argumentValues[1], queryList[2].ArgValues[1]);

            Assert.AreEqual(keys[3], queryList[3].QueryKey);
            Assert.AreEqual(null, queryList[3].ArgKeys);
            Assert.AreEqual(null, queryList[3].ArgValues);
        }

        private void CQG_LogChange_Mock(string message)
        {
        }
    }
}