﻿using System;
using System.Collections.Generic;

namespace FakeCQG.Internal
{
    // Here placed all dictionaries with data that transferred by the key and methods for its managing
    public class ServerDictionaries
    {
        static Dictionary<string, object> objDictionary = new Dictionary<string, object>();

        public static HashSet<Guid> RealtimeIds = new HashSet<Guid>();

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

        public static void DeleteFromServerDictionaries(Models.HandshakingModel subscriber)
        {
            foreach(var obj in subscriber.ObjectKeys)
            {
                objDictionary.Remove(obj);
            }
            RealtimeIds.Remove(subscriber.ID);
        }
    }
}
