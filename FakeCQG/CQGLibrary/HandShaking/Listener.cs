using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Handshaking
{
    public static class Listener
    {
        const string key = "HANDSHAKING";
        const int timeForQuery = 500; // 0.5 s
        static HandshakingModel handshaking = new HandshakingModel(key);
        static Timer timer;

        public delegate void ListenerEventHandler(HandshakingEventArgs args);
        public static event ListenerEventHandler SubscribersAdded;

        public static Task StartHandshaking()
        {
            // !! Do not create this object once again for each handshaking
            HandshakingHelper mongo = new HandshakingHelper();
            var collection = mongo.GetCollection;

            return Task.Run(() => 
            {
                // Clear collection
                ClearCollection(collection);

                // Send handshaking query
                SendHandshakingQuery(collection);

                // Check subscribers
                CheckSubscribers(collection);
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
                CQG.OnLogChange("No subscribers for handshaking");
                SubscribersAdded(new HandshakingEventArgs());
            }
            else
            {
                CQG.OnLogChange(string.Format("DC has {0} subscriber(s)", subscribers.Count));
                SubscribersAdded(new HandshakingEventArgs(subscribers));
            }
            timer.Start();
        }

        private static void SendHandshakingQuery(IMongoCollection<HandshakingModel> collection)
        {
            var filter = Builders<HandshakingModel>.Filter.Eq(Keys.IdName, key);
            collection.InsertOne(handshaking);
            Task.Delay(timeForQuery).GetAwaiter().GetResult();
            collection.DeleteOne(filter);
        }

        static void ClearCollection(IMongoCollection<HandshakingModel> collection)
        {
            var filter = Builders<HandshakingModel>.Filter.Empty;
            collection.DeleteMany(filter);
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
