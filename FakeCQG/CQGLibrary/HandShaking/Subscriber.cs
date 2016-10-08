using System;
using System.Linq;
using System.Threading.Tasks;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace FakeCQG.Handshaking
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
                CQG.OnLogChange("Listening for handshacking is started");

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
                            collection.InsertOne(handshaker);
                            CQG.OnLogChange(handshaker.ID.ToString());
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
            collection = mongo.GetCollectionSubscribers;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit; ;
        }

        public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                handshaker.UnSubscribe = true;
                handshaker.ObjectKeys = ClientDictionaries.ObjectNames.ToList();
                handshaker.UnsubscribeEventList = ConvertDictionary(ClientDictionaries.EventCheckingDictionary);
                mongo.GetCollectionUnsubscribers.InsertOne(handshaker);
                CQG.OnLogChange(handshaker.ID.ToString());
            }
            catch (Exception)
            {

            }

        }

        static Dictionary<string, string> ConvertDictionary(Dictionary<string, Dictionary<string, bool>> fullEventsDictionary)
        {
            Dictionary<string, string> selectedEventDictionary = new Dictionary<string, string>();
            foreach(var dics in fullEventsDictionary)
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

