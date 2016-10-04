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
        const string handshakingKey = "HANDSHAKING";

        static string key = CQG.CreateUniqueKey();
        static HandshakingModel handshaker = new HandshakingModel(key);

        static HandshakingHelper mongo;
        static IMongoCollection<HandshakingModel> collection;

        public static Task ListenForHanshaking()
        {
            return Task.Run(() =>
            {
                CQG.OnLogChange("Listening for handshacking is started");

                var filterKey = Builders<HandshakingModel>.Filter.Eq(Keys.IdName, handshakingKey);
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

        static Subscriber()
        {
            mongo = new HandshakingHelper();
            collection = mongo.GetCollection;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit; ;
        }

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                handshaker.UnSubscribe = true;
                collection.InsertOne(handshaker);
                CQG.OnLogChange(key);
            }
            catch (Exception)
            {

            }

        }
    }
}

