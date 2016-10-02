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
        static HandshakingModel handshaker = new HandshakingModel(key);

        public static Task ListenForHanshaking()
        {
            return Task.Run(() =>
            {
                CQG.OnLogChange("Listening for handshacking is started");

                const string key = "HANDSHAKING";
                HandshakingHelper mongo = new HandshakingHelper();
                var collection = mongo.GetCollection;
                var filterKey = Builders<HandshakingModel>.Filter.Eq(Keys.IdName, key);
                var filterId = Builders<HandshakingModel>.Filter.Eq(Keys.HandshakerId, handshaker.ID);
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
