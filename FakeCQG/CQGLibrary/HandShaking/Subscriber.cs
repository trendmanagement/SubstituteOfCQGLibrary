using CQGLibrary.Helpers;
using CQGLibrary.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGLibrary.HandShaking
{
    public static class Subscriber
    {

        static string key = Guid.NewGuid().ToString();
        static HandShakerModel handShaker = new HandShakerModel(key);

        public static Task ListenForHanshaking()
        {
            return Task.Run(() =>
            {
                const string key = "HANDSHAKING";
                MongoHelper mongo = new MongoHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<HandShakerModel>.Filter.Eq("Key", key);
                while (true)
                {
                    var subscriber = collection.Find(filter).FirstOrDefault();

                    if (subscriber != null)
                    {
                        collection.InsertOne(handShaker);
                    }
                }
            });
        }

    }
}
