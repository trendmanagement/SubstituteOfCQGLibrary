using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace DataCollectionForRealtime
{
    class Listener
    {
        static bool isStartedListenUnsubscribers = false;

        const int HandshakingQueryInterval = 1500;   // 1.5 s
        static HandshakingModel handshaking = new HandshakingModel();
        static Timer timer;
        static HandshakingHelper mongo = new HandshakingHelper();

        public delegate void ListenerEventHandler(HandshakingEventArgs args);
        public static event ListenerEventHandler SubscribersAdded;

        public static Task StartHandshaking()
        {
            var collectionSubscribers = mongo.GetCollectionSubscribers;

            return Task.Run(() =>
            {
                // Clear collection
                ClearCollection(collectionSubscribers);

                // Send handshaking query
                SendHandshakingQuery(collectionSubscribers);

                // Check subscribers
                CheckSubscribers(collectionSubscribers);
            });
        }

        private static void CheckSubscribers(IMongoCollection<HandshakingModel> collection)
        {
            var filter = Builders<HandshakingModel>.Filter.Empty;
            List<HandshakingModel> subscribers = new List<HandshakingModel>();
            subscribers = collection.Find(filter).ToList();
            OnSubscribersAdded(subscribers);
        }

        private static void OnSubscribersAdded(List<HandshakingModel> subscribers)
        {
            //TODO: Implement logic for variant without subscribers and for only one suscriber
            if (subscribers.Count == 0)
            {
                AsyncTaskListener.LogMessage("No subscribers for handshaking");
                SubscribersAdded(new HandshakingEventArgs());
            }
            else
            {
                AsyncTaskListener.LogMessage(string.Format("DC has {0} subscriber(s)", subscribers.Count));
                SubscribersAdded(new HandshakingEventArgs(subscribers));
            }
            timer.Start();
        }

        private static void SendHandshakingQuery(IMongoCollection<HandshakingModel> collection)
        {
            var filter = Builders<HandshakingModel>.Filter.Eq(Keys.HandshakerId, handshaking.ID);
            collection.InsertOne(handshaking);
            Task.Delay(HandshakingQueryInterval).GetAwaiter().GetResult();
            collection.DeleteOne(filter);
        }

        static void ClearCollection(IMongoCollection<HandshakingModel> collection)
        {
            var filter = Builders<HandshakingModel>.Filter.Empty;
            collection.DeleteMany(filter);
        }

        public static void DeleteUnsubscriber(Guid id)
        {
            var filter = Builders<HandshakingModel>.Filter.Eq(Keys.HandshakerId, id);
            mongo.GetCollectionUnsubscribers.DeleteOne(filter);
        }


        public static void StartListening(int time)
        {
            timer = new Timer(time);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            //TODO: Implement periodic push handshaking queries
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StartHandshaking();
        }
    }
}
