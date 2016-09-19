﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakeCQG
{
    //Here placed all dictionaries with data that transferred by the key and methods for its managing
    public static class DataDictionaries
    {
        private static Dictionary<string, object> objDictionary = new Dictionary<string, object>();

        private static Dictionary<string, object> answerDictionary = new Dictionary<string, object>();

        private static Dictionary<string, bool> isAnswer = new Dictionary<string, bool>();

        private static Dictionary<string, bool> eventCheckingDictionary = new Dictionary<string, bool>();

        public static Dictionary<string, bool> IsAnswer
        {
            get
            {
                return isAnswer;
            }
        }

        public static Dictionary<string, bool> EventCheckingDictionary
        {
            get
            {
                return eventCheckingDictionary;
            }
        }

        public static bool KeyExistInObjectDictionary(string key)
        {
            return objDictionary.ContainsKey(key);
        }

        public static void PutObjectToTheDictionary(string key, object obj)
        {
            objDictionary[key] = obj;
        }

        public static object GetObjectFromTheDictionary(string key)
        {
            return objDictionary[key];
        }

        public static void PutAnswerToTheDictionary(string key, object answer)
        {
            answerDictionary[key] = answer;
        }

        public static object GetAnswerFromTheDictionary(string key)
        {
            return answerDictionary[key];
        }

        public static void FillEventCheckingDictionary()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            var assm = Assembly.LoadFrom(assmPath);

            foreach (Type type in assm.ExportedTypes.Where(type => type.IsClass))
            {
                IEnumerable<EventInfo> einfos = type.GetEvents();
#if DEBUG
                einfos = einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
#endif
                foreach (EventInfo einfo in einfos)
                {
                    eventCheckingDictionary[einfo.Name] = false;
                }
            }
        }

        public static void RemoveObject(string key)
        {
            objDictionary.Remove(key);
        }
    }
}
