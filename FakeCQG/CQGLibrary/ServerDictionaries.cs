using FakeCQG.Internal.Models;
using System;
using System.Collections.Generic;

namespace FakeCQG.Internal
{
    // Here placed all dictionaries with data that transferred by the key and methods for its managing
    public class ServerDictionaries
    {
        static Dictionary<string, object> objDictionary = new Dictionary<string, object>();

        public static List<HandshakingModel> RealtimeIds = new List<HandshakingModel>();

        public static void PutObjectToTheDictionary(string key, object obj)
        {
            objDictionary[key] = obj;
        }

        public static object GetObjectFromTheDictionary(string key)
        {
            return objDictionary[key];
        }

        public static void RemoveObjectFromTheDictionary(string key)
        {
            objDictionary.Remove(key);
        }

        public static void ClearAllDictionaries()
        {
            objDictionary.Clear();
        }

        public static void DeleteFromServerDictionaries(HandshakingModel subscriber)
        {
            foreach(var obj in subscriber.ObjectKeys)
            {
                objDictionary.Remove(obj);
            }
            RealtimeIds.Remove(subscriber);
        }
    }
}
