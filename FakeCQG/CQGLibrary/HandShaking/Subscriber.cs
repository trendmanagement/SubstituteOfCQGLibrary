using System;
using System.Linq;
using System.Threading.Tasks;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Handshaking
{
    public static class Subscriber
    {
        static string key = CQG.CreateUniqueKey();
        static HandshakerModel handshaker = new HandshakerModel(key);

        public static Task ListenForHanshaking()
        {
            return Task.Run(() =>
            {
                CQG.OnLogChange("Listerning handshacking is started");

                const string key = "HANDSHAKING";
                MongoHelper mongo = new MongoHelper();
                var collection = mongo.GetCollection;
                var filterKey = Builders<HandshakerModel>.Filter.Eq(Keys.IdName, key);
                var filterId = Builders<HandshakerModel>.Filter.Eq(Keys.HandshakerId, handshaker.ID);
                while (true)
                {
                    try
                    {
                        bool isHandshakingQuery = (collection.Find(filterKey).FirstOrDefault() != null);
                        bool isAnswer = (collection.Find(filterId).FirstOrDefault() != null);

                        if (isHandshakingQuery && !isAnswer)
                        {
                            collection.InsertOne(handshaker);
                            CQG.OnLogChange(key);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        CQG.OnLogChange(message);
                    }
                }
            });
        }
    }
}
