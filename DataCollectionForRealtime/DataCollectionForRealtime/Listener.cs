using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Internal;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace DataCollectionForRealtime
{
    class Listener
    {
        private const int HandshakingQueryInterval = 1500;   // 1.5 s
        private static HandshakingInfo handshaking = new HandshakingInfo();
        private static Timer timer;
        private static bool locked;
        private static HandshakingHelper mongo = new HandshakingHelper();

        public static int SubscribersCount { get; set; }

        public delegate void ListenerEventHandler(HandshakingEventArgs args);
        public static event ListenerEventHandler SubscribersAdded;

        public static void StartHandshaking()
        {
            var collectionSubscribers = mongo.GetCollectionSubscribers;
            
            // Clear collection
            ClearCollection(collectionSubscribers);

            // Send handshaking query
            SendHandshakingQuery(collectionSubscribers);

            // Check subscribers
            CheckSubscribers(collectionSubscribers);

        }

        // Checking for subscribers and firing of OnSubscribersAdded event
        private static void CheckSubscribers(IMongoCollection<HandshakingInfo> collection)
        {
            var filter = Builders<HandshakingInfo>.Filter.Empty;
            List<HandshakingInfo> subscribers = new List<HandshakingInfo>();
            subscribers = collection.Find(filter).ToList();
            OnSubscribersAdded(subscribers);
        }

        private static void OnSubscribersAdded(List<HandshakingInfo> subscribers)
        {
            if (subscribers.Count == 0)
            {
                SubscribersCount = 0;
                AsyncTaskListener.LogMessage("No subscribers for handshaking");
                SubscribersAdded(new HandshakingEventArgs());
                AsyncTaskListener.AllLog.Clear();
            }
            else
            {
                SubscribersCount = subscribers.Count;
                AsyncTaskListener.LogMessage(string.Format("DC has {0} subscriber(s)", subscribers.Count));
                SubscribersAdded(new HandshakingEventArgs(subscribers));
            }
            timer.Start();
        }

        // Sending of handshaking model and deleting it 
        private static void SendHandshakingQuery(IMongoCollection<HandshakingInfo> collection)
        {
            var filter = Builders<HandshakingInfo>.Filter.Eq(Keys.HandshakerId, handshaking.ID);
            collection.InsertOne(handshaking);
            Task.Delay(HandshakingQueryInterval).GetAwaiter().GetResult();
            collection.DeleteOne(filter);
        }

        static void ClearCollection(IMongoCollection<HandshakingInfo> collection)
        {
            var filter = Builders<HandshakingInfo>.Filter.Empty;
            collection.DeleteMany(filter);
        }

        public static void DeleteUnsubscriber(Guid id)
        {
            var filter = Builders<HandshakingInfo>.Filter.Eq(Keys.HandshakerId, id);
            mongo.GetCollectionUnsubscribers.DeleteOne(filter);
        }


        public static void StartListening(int time)
        {
            timer = new Timer(time);
            timer.AutoReset = false;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                StartHandshaking();
            }
            finally
            {
                timer.Start();
            }
        }
    }
}
