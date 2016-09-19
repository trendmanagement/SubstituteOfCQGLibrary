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

        public delegate void ListenerEventHandler(List<HandShakerModel> subscribers);
        public static event ListenerEventHandler OnSubscribersAdded;

        public static Task CheckListeners()
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
            while (true)
            {
                subscribers = collection.Find(filter).ToList();
                OnSubscribersAdded(subscribers);
            }
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

        public static void StartListerning(int timer)
        {
            //TODO: Implement periodic push handshaking queries
        }

    }
}
