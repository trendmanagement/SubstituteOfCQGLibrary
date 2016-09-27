using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace FakeCQG
{
    // Here placed all dictionaries with data that transferred by the key and methods for its managing
    public static class DataDictionaries
    {
        static Dictionary<string, object> objDictionary = new Dictionary<string, object>();

        static Dictionary<string, bool> isAnswer = new Dictionary<string, bool>();

        static Dictionary<string, Dictionary<string, bool>> eventCheckingDictionary = new Dictionary<string, Dictionary<string, bool>>();

        public static Dictionary<string, bool> IsAnswer
        {
            get
            {
                return isAnswer;
            }
        }

        public static Dictionary<string, Dictionary<string, bool>> EventCheckingDictionary
        {
            get
            {
                return eventCheckingDictionary;
            }
        }

        public static HashSet<Guid> RealTimeIds = new HashSet<Guid>();

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

        public static void RemoveObjectFromTheDictionary(string key)
        {
            objDictionary.Remove(key);
        }

        public static void FillEventCheckingDictionary(string objKey, string objTName)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            var assm = Assembly.LoadFrom(assmPath);

            Type objT = assm.GetType(objTName, true); 

            //foreach (Type type in assm.ExportedTypes.Where(type => type.IsClass))
            //{
            IEnumerable<EventInfo> einfos = objT.GetEvents();

            if(einfos != null)
            {
#if DEBUG
                einfos = einfos.OrderBy(einfo => einfo.EventHandlerType.Name);
#endif
                Dictionary<string, bool> objEventCheckingDictionary = new Dictionary<string, bool>();
                foreach (EventInfo einfo in einfos)
                {
                    objEventCheckingDictionary.Add(einfo.Name, false);
                }
                eventCheckingDictionary.Add(objKey, objEventCheckingDictionary);
            }
        }

        public static void RemoveObject(string key)
        {
            objDictionary.Remove(key);
        }

        public static void ClearAllDictionaris()
        {
            objDictionary = new Dictionary<string, object>();
            isAnswer = new Dictionary<string, bool>();
            eventCheckingDictionary = new Dictionary<string, Dictionary<string, bool>>();
        }
    }
}
