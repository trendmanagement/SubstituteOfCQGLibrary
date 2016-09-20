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
                FakeCQG.CQG.OnLogChange("Listerning handshacking is started");

                const string key = "HANDSHAKING";
                MongoHelper mongo = new MongoHelper();
                var collection = mongo.GetCollection;
                var filterKey = Builders<HandShakerModel>.Filter.Eq("Key", key);
                var filterId = Builders<HandShakerModel>.Filter.Eq("_id", handShaker.ID);
                while (true)
                {
                    try
                    {
                        bool isHandShakingQuery = (collection.Find(filterKey).FirstOrDefault() != null);
                        bool isAnswer = (collection.Find(filterId).FirstOrDefault() != null);

                        if (isHandShakingQuery && !isAnswer)
                        {
                            collection.InsertOne(handShaker);
                            FakeCQG.CQG.OnLogChange(key);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        FakeCQG.CQG.OnLogChange(message);
                    }
                }
            });
        }

    }
}
