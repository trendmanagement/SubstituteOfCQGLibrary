using System.Collections.Generic;
using System.Threading.Tasks;
using DataCollectionForRealtime;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using MongoDB.Driver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRealCQG
{
    [TestClass]
    [DeploymentItem("Interop.CQG.dll")]
    public class UnitTestHandshaking
    {
        HandshakingEventArgs Handshaking = new HandshakingEventArgs();
        List<HandshakingInfo> handshakerList;

        [TestMethod]
        public void NoSubscribers()
        {
            // arrange
            var mongo = new HandshakingHelper();
            IMongoCollection<HandshakingInfo> collection = mongo.GetCollectionSubscribers;

            // act
            Listener.StartListening(100);
            Listener.SubscribersAdded += Listener_SubscribersAdded_NoSubscribers;
        }

        private void Listener_SubscribersAdded_NoSubscribers(HandshakingEventArgs args)
        {
            // assert
            Assert.IsNotNull(args);
            Assert.IsTrue(args.NoSubscribers);
        }

        [TestMethod]
        public void GetDifferentCountOfSubscribers()
        {
            // arrange
            var mongo = new HandshakingHelper();
            IMongoCollection<HandshakingInfo> collection = mongo.GetCollectionSubscribers;

            Listener.StartListening(100);
            Listener.SubscribersAdded += Listener_SubscribersAdded;

            // act 1
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo(),
                new HandshakingInfo(),
            };
            collection.InsertMany(handshakerList);

            // act 2
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo(),
                new HandshakingInfo(),
                new HandshakingInfo(),
                new HandshakingInfo()
            };
            collection.InsertMany(handshakerList);

            // act 3
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo()
            };
            collection.InsertMany(handshakerList);
        }

        private void Listener_SubscribersAdded(HandshakingEventArgs args)
        {
            // assert
            Handshaking = args.Subscribers == null? Handshaking : args;
            Assert.AreEqual(handshakerList.Count, Handshaking.Subscribers.Count);
            Assert.IsFalse(args.NoSubscribers);
        }

        List<int> countSubscribers = new List<int>();
        int countCheckingOfSubscribers;
        [TestMethod]
        public void GetDifferentCountOfSubscribers_WithLongAction()
        {
            // arrange
            var mongo = new HandshakingHelper();
            IMongoCollection<HandshakingInfo> collection = mongo.GetCollectionSubscribers;

            Listener.StartListening(100);
            Listener.SubscribersAdded += Listener_SubscribersAdded_LongAction;

            // act 1
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo(),
                new HandshakingInfo(),
            };
            collection.InsertMany(handshakerList);

            // act 2
            Task.Delay(1500).GetAwaiter().GetResult();
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo(),
                new HandshakingInfo(),
                new HandshakingInfo(),
                new HandshakingInfo()
            };
            collection.InsertMany(handshakerList);

            // act 3
            Task.Delay(2500).GetAwaiter().GetResult();
            handshakerList = new List<HandshakingInfo>()
            {
                new HandshakingInfo()
            };
            collection.InsertMany(handshakerList);
        }

        private void Listener_SubscribersAdded_LongAction(HandshakingEventArgs args)
        {
            if(args.Subscribers != null)
            {
                countSubscribers.Add(args.Subscribers.Count);
            }
            countCheckingOfSubscribers++;

            // assert
            if (countSubscribers.Count == 3)
            {
                Assert.AreEqual(2, countSubscribers[0]);
                Assert.AreEqual(4, countSubscribers[1]);
                Assert.AreEqual(1, countSubscribers[2]);
                Assert.IsTrue(countSubscribers.Count < countCheckingOfSubscribers);
            }
        }

    }
}
