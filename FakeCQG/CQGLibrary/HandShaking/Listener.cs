using CQGLibrary.Helpers;
using CQGLibrary.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CQGLibrary.HandShaking
{
    public static class Listener
    {
        const string key = "HANDSHAKING";
        const int timeForQuery = 500; // 0.5 s
        static HandShakerModel handShaking = new HandShakerModel(key);
        static Timer timer;

        public delegate void ListenerEventHandler(HandShakingEventArgs args);
        public static event ListenerEventHandler SubscribersAdded;

        public static Task StartHandShaking()
        {
            MongoHelper mongo = new MongoHelper();
            var collection = mongo.GetCollection;

            return Task.Run(() => 
            {
                //Clear collection
                Clearcollection(collection);

                //Send handshaking query
                SendHandShakerQuery(collection);

                //Check subscribers
                CheckSubscribers(collection);
            });
        }

        private static void CheckSubscribers(IMongoCollection<HandShakerModel> collection)
        {
            var filter = Builders<HandShakerModel>.Filter.Empty;
            List<HandShakerModel> subscribers = new List<HandShakerModel>();
            subscribers = collection.Find(filter).ToList();
            OnSubscribersAdded(subscribers);
        }

        private static void OnSubscribersAdded(List<HandShakerModel> subscribers)
        {
            //TODO: Implement logic for variant without subscribers and for only one suscriber
            if(subscribers.Count == 0)
            {
                FakeCQG.CQG.OnLogChange("No subscribers for handshaking");
                SubscribersAdded(new HandShakingEventArgs());
            }
            else
            {
                FakeCQG.CQG.OnLogChange(string.Format("DC has {0} subscribers", subscribers.Count));
                SubscribersAdded(new HandShakingEventArgs(subscribers));
            }
            timer.Start();
        }

        private static void SendHandShakerQuery(IMongoCollection<HandShakerModel> collection)
        {
            var filter = Builders<HandShakerModel>.Filter.Eq("Key", key);
            collection.InsertOne(handShaking);
            Task.Delay(timeForQuery).GetAwaiter().GetResult();
            collection.DeleteOne(filter);
        }

        static void Clearcollection(IMongoCollection<HandShakerModel> collection)
        {
            var filter = Builders<HandShakerModel>.Filter.Empty;
            collection.DeleteMany(filter);
        }

        public static void StartListerning(int time)
        {
            timer = new Timer(time);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            //TODO: Implement periodic push handshaking queries
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StartHandShaking();
        }
    }
}
