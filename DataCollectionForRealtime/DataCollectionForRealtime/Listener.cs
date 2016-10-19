﻿using System;
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
        //Private fields
        const int HandshakingQueryInterval = 1500;   // 1.5 s
        static HandshakingInfo handshaking = new HandshakingInfo();
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

        //Checking for subscribers and call OnSubscribersAdded event
        private static void CheckSubscribers(IMongoCollection<HandshakingInfo> collection)
        {
            var filter = Builders<HandshakingInfo>.Filter.Empty;
            List<HandshakingInfo> subscribers = new List<HandshakingInfo>();
            subscribers = collection.Find(filter).ToList();
            OnSubscribersAdded(subscribers);
        }

        private static void OnSubscribersAdded(List<HandshakingInfo> subscribers)
        {
            //TODO: Implement logic for variant without subscribers and for only one subscriber
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

        //Send and delete handshaking model
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
