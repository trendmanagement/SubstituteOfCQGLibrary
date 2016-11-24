using System;
using System.Collections.Generic;
using FakeCQG.Internal.Models;

namespace FakeCQG.Internal
{
    // Here placed dictionaries and methods for its managing that are used on the data collector's side
    public class ServerDictionaries
    {
        static Dictionary<string, object> objDictionary = new Dictionary<string, object>(StringComparer.Ordinal);

        public static List<HandshakingInfo> RealtimeIds = new List<HandshakingInfo>();

        public static void PutObjectToTheDictionary(string key, object obj)
        {
            objDictionary[key] = obj;
        }

        public static object GetObjectFromTheDictionary(string key)
        {
            object objectValue = default(object);

            if(!objDictionary.TryGetValue(key, out objectValue))
            {
                Core.OnLogChange(string.Concat("Key not found with key: ", key));
            }

            return objectValue;
        }

        public static void RemoveObjectFromTheDictionary(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                objDictionary.Remove(key);
            }
        }

        public static void ClearAllDictionaries()
        {
            objDictionary.Clear();
        }

        public static void DeleteFromServerDictionaries(HandshakingInfo subscriber)
        {
            foreach (string key in subscriber.ObjectKeys)
            {
                RemoveObjectFromTheDictionary(key);
            }
            RealtimeIds.Remove(subscriber);
        }
    }
}
