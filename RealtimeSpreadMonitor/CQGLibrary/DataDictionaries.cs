using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCQG
{
    //Here placed all dictionaries with data that transferred by the key and methods for its managing
    public static class DataDictionaries
    {
        private static Dictionary<string, object> objDictionary = new Dictionary<string, object>();

        private static Dictionary<string, object> answerDictionary = new Dictionary<string, object>();

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
    }
}
