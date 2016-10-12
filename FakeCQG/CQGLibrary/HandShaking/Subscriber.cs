using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Handshaking
{
    public static class Subscriber
    {
        static HandshakingModel handshaker = new HandshakingModel();

        static HandshakingHelper mongo;
        static IMongoCollection<HandshakingModel> collection;

        public static Task ListenForHanshaking()
        {
            return Task.Run(() =>
            {
                Core.OnLogChange("Listening for handshacking is started");

                var filter = Builders<HandshakingModel>.Filter.Empty;
                var filterId = Builders<HandshakingModel>.Filter.Eq(Keys.HandshakerId, handshaker.ID);
                while (true)
                {
                    try
                    {
                        bool isHandshakingQuery = (collection.Find(filter).FirstOrDefault() != null);
                        bool isAnswer = (collection.Find(filterId).FirstOrDefault() != null);

                        if (isHandshakingQuery && !isAnswer)
                        {
                            handshaker.ObjectKeys = ClientDictionaries.ObjectNames.ToList();
                            handshaker.UnsubscribeEventList = ClientDictionaries.EventCheckingDictionary;
                            collection.InsertOne(handshaker);
                            Core.OnLogChange(handshaker.ID.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        Core.OnLogChange(message);
                    }
                }
            });
        }

        static Subscriber()
        {
            mongo = new HandshakingHelper();
            collection = mongo.GetCollectionSubscribers;
        }

        static Dictionary<string, string> ConvertDictionary(Dictionary<string, Dictionary<string, bool>> fullEventsDictionary)
        {
            Dictionary<string, string> selectedEventDictionary = new Dictionary<string, string>();
            foreach (var dics in fullEventsDictionary)
            {
                string key = dics.Key;

                foreach (var dic in dics.Value)
                {
                    if (dic.Value)
                    {
                        selectedEventDictionary.Add(key, dic.Key);
                    }
                }
            }
            return selectedEventDictionary;
        }
    }
}

