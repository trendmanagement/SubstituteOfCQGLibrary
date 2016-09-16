using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG
{
    public partial class CQG 
    {
        private static System.Timers.Timer eventCheckingTimer;
        private static bool eventsCheckingON = false;

        static HashSet<Type> SerializableArgTypes = new HashSet<Type> {
            typeof(object),
            typeof(int),
            typeof(double),
            typeof(string),
            typeof(char),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(uint),
            typeof(bool) };

        #region Help objects
        private static string _log;
       
        private const int index = 0;
        private const int maxRequestTime = 30000;       // 30s
        private const string IdName = "Key";
        private static System.Timers.Timer timer;
        private static bool timerStoped;


        public string Key { get; set; }

        public delegate void GetQueryInfosHandler(List<QueryInfo> queries);
        public static event GetQueryInfosHandler GetQueries;
        public delegate void LogHandler(string message);
        public static event LogHandler LogChange;
        
        //Changed the access level of visibility for testing
        public static int QueryTemeout = int.MaxValue;  // Currently set to the max value for debugging
        public const string NoAnswerMessage = "Timer elapsed. No answer.";
        #endregion

        #region Internal CQG methods
        public static void SetProperty(string name, string objKey, object value)
        {

            bool isTrue = (bool)ExecuteTheQuery(QueryInfo.QueryType.Property, objKey, name, new object[] { value });
            if (!isTrue)
            {
                OnLogChange(String.Format("Setter ICQGCEL.{0} wasn't successfully completed", name));
            }
        }

        public static void GetPropertiesFromMatryoshka(ref object parentObj, string parentObjKey)
        {
            PropertyInfo[] pinfos = parentObj.GetType().GetProperties();
            foreach (var pi in pinfos)
            {
                Type propType = pi.PropertyType;
                //if (propType == "IEnumerable`1")
                //{
                //    propType = "IEnumerable";
                //}
                if (pi.PropertyType.Assembly.FullName.Substring(0, 8) == "mscorlib" || pi.PropertyType.IsEnum)
                {
                    parentObj.GetType().GetProperty(pi.Name).SetValue(parentObj, ExecuteTheQuery(QueryInfo.QueryType.Property,
                        parentObjKey, pi.Name));
                }
                else
                {
                    string propKey = (string)ExecuteTheQuery(QueryInfo.QueryType.Property,
                        parentObjKey, pi.Name);
                    object piObj;
                    if (pi.PropertyType.IsInterface)
                    {
                        propType = Type.GetType(propType.ToString() + "Class ");
                        piObj = Activator.CreateInstance(propType);
                    }
                    else
                    {
                        piObj = Activator.CreateInstance(propType);
                    }
                    GetPropertiesFromMatryoshka( ref piObj, propKey);
                }
            }
        }

        #endregion

        #region MongoDB communication methods
        public static void CommonEventHandler(string name, object[] args = null)
        {
            AnswerHelper mongo = new AnswerHelper();
            var collAnswer = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("QueryName", name);
            AnswerInfo answer = default(AnswerInfo);
            try
            {
                answer = collAnswer.Find(filter).First();
                var argValues = new Dictionary<int, object>();
                argValues.Add(0, "!");
                argValues.Add(1, args);
                var update = Builders<AnswerInfo>.Update.Set("ArgValues", argValues);
                //TODO: deserialize argValues from dictionary to bson
                //collAnswer.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                OnLogChange(ex.Message);
            }
        }

        public static object ExecuteTheQuery(QueryInfo.QueryType qType, string objKey, string name, object[] args = null)
        {
            if (!eventsCheckingON)
            {
                DataDictionaries.FillEventCheckingDictionary();

                eventsCheckingON = true;
            }

            Dictionary<int, string> argKeys = new Dictionary<int, string>();
            Dictionary<int, object> argVals = new Dictionary<int, object>();

            string coreAsmName = "";
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assem.GetName().Name == "mscorlib")
                {
                    coreAsmName = assem.FullName;
                }
            }

            string key = Guid.NewGuid().ToString("D");

            if (args != null)
            {
                int i = 0;
                foreach (var arg in args)
                {
                    if (SerializableArgTypes.Contains(arg.GetType()) || arg.GetType().IsEnum || arg.GetType().Assembly.FullName == coreAsmName)
                    {
                        argVals.Add(i, arg);
                    }
                    else
                    {
                        argKeys.Add(i, arg.GetType().GetField("thisObjUnqKey", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(arg).ToString());
                    }
                }
            }

            timerStoped = false;
            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = maxRequestTime;
            timer.Start();

            Task.Run(() => LoadInQueryAsync(CreateQuery(qType, key, objKey, name, argKeys, argVals)));

            AnswerInfo result = WaitingForAnAnswer(key);

            if (result.ValueKey == "value")
            {
                return result.Value;
            }
            else if(result.ValueKey == "true")
            {
                return true;
            }
            else
            {
                return result.ValueKey;
            }
        }

        public static AnswerInfo WaitingForAnAnswer(string key)
        {
            
            AnswerInfo answer = default(AnswerInfo);
            Task task = Task.Run(() => { answer = GetAnswerData(key); });
            //while (!task.IsCompleted)
            //{

            //}
            //DataDictionaries.IsAnswer.Remove(key);
            //return answer;
            bool success = task.Wait(QueryTemeout);
            if (success)
            {
                DataDictionaries.IsAnswer.Remove(key);
                return answer;
            }
            else
            {
                OnLogChange(NoAnswerMessage);
                throw new TimeoutException();
            }
        }

        public static QueryInfo CreateQuery(QueryInfo.QueryType qType, string key, string objKey, string name,
            Dictionary<int, string> argKeys = null, Dictionary<int, object> argVals = null)
        {
            QueryInfo model = new QueryInfo(qType, key, objKey, name);

            switch (qType)
            {
                case QueryInfo.QueryType.Constructor:
                case QueryInfo.QueryType.Property:
                case QueryInfo.QueryType.Method:
                    if (argKeys.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argVals: argVals);
                        OnLogChange(model.ToString());
                    }
                    else if (argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argKeys);
                        OnLogChange(model.ToString());
                    }
                    else if (argKeys.Count == 0 && argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name);
                        OnLogChange(model.ToString());
                    }
                    else
                    {
                        model = new QueryInfo(qType, key, objKey, name, argKeys, argVals);
                        OnLogChange(model.ToString());
                    }
                    break;
                case QueryInfo.QueryType.Event:
                    model = new QueryInfo(qType, key, objKey, name, argVals: argVals);
                    OnLogChange(model.ToString());
                    break;
                default:
                    break;
            }
            DataDictionaries.IsAnswer.Add(key, false);
            return model;
        }

        public static Task LoadInQueryAsync(QueryInfo model)
        {
            QueryHelper mongo = new QueryHelper();
            return Task.Run(() =>
            {
                var callQuery = mongo.GetCollection;
                try
                {
                    callQuery.InsertOne(model);
                    OnLogChange(model.Key, model.QueryName, true);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
        }

        public static Task LoadInAnswerAsync(AnswerInfo model)
        {
            AnswerHelper mongo = new AnswerHelper();
            return Task.Run(() =>
            {
                var collection = mongo.GetCollection;
                try
                {
                    collection.InsertOne(model);
                    OnLogChange(model.Key, model.QueryName, true);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
        }

        public static AnswerInfo GetAnswerData(string id, out bool isAns)
        {
            AnswerHelper mongo = new AnswerHelper();
            var collAnswer = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("Key", id);
            AnswerInfo answer = default(AnswerInfo);
            try
            {
                answer = collAnswer.Find(filter).First();
                OnLogChange(answer.Key, answer.ValueKey, false);
                RemoveAnswerAsync(answer.Key);
                isAns = true;
                //if (answer.Key == "value")
                //{
                //    isVal = true;
                //    return answer.Value;
                //}
                //else
                //{
                //    isVal = false;
                //    return answer;
                //}
                return answer;
            }
            catch (Exception ex)
            {
                OnLogChange(id, "null", false);
                isAns = false;
                return default(AnswerInfo);
            }
        }
        public static AnswerInfo GetAnswerData(string id)
        {
            AnswerHelper mongo = new AnswerHelper();
            var collAnswer = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("Key", id);
            AnswerInfo answer = default(AnswerInfo);
            while(!DataDictionaries.IsAnswer[id])
            {
                try
                {
                    answer = collAnswer.Find(filter).First();
                    OnLogChange(answer.Key, answer.ValueKey, false);
                    RemoveAnswerAsync(answer.Key);
                    DataDictionaries.IsAnswer[id] = true;
                    
                }
                catch (Exception ex)
                {
                    //OnLogChange(id, "null", false);
                    //if (!DataDictionaries.IsAnswer[id])
                    //{
                    //    return GetAnswerData(id);
                    //}
                    //else
                    //{
                    //    return answer;
                    //}

                    ////TODO: Create type of exception for  variant "no answer"
                    //throw new Exception("No answer in MongoDB");
                }
            }
            return answer;
        }
        public static Task<bool> CheckQueryAsync(string Id)
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Eq(IdName, Id);
                try
                {
                    QueryInfo result = default(QueryInfo);
                    try
                    {
                        result = collection.Find(filter).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        OnLogChange(ex.Message);
                    }
                    return ((result != null) ? true : false);
                }
                catch (Exception ex)
                {
                    //TODO: Add logic for different exceptions
                    return false;
                }
            });
        }

        public static Task<bool> CheckAnswerAsync(string Id)
        {
            return Task.Run(() =>
            {
                AnswerHelper mongo = new AnswerHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<AnswerInfo>.Filter.Eq(IdName, Id);
                try
                {
                    AnswerInfo result = default(AnswerInfo);
                    try
                    {
                        result = collection.Find(filter).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        OnLogChange(ex.Message);
                    }
                    return (result != null) ? true : false;
                }
                catch (Exception ex)
                {
                    //TODO: Add logic for different exceptions
                    return false;
                }
            });
        }
        public static object[] CheckWhetherEventHappened(string name)
        {
            var argValues = new Dictionary<int, object>();

            AnswerHelper mongo = new AnswerHelper();
            var collAnswer = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("QueryName", name);
            AnswerInfo answer = default(AnswerInfo);

            answer = collAnswer.Find(filter).First();
            argValues = answer.ArgValues;
            
            return (object[])argValues[1];
        }


        public static Task RemoveQueryAsync(string key)
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Eq(IdName, key);
                collection.DeleteOne(filter);
            });
        }

        public static Task RemoveAnswerAsync(string key)
        {
            return Task.Run(() =>
            {
                AnswerHelper mongo = new AnswerHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<AnswerInfo>.Filter.Eq(IdName, key);
                collection.DeleteOne(filter);
            });
        }

        public static Task ReadQueriesAsync()
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                var collection = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Empty;
                List<QueryInfo> result = new List<QueryInfo>();
                try
                {
                    result = collection.Find(filter).ToList();
                    OnGetQueries(result);
                    OnLogChange("**********************************************************");
                    OnLogChange(string.Format("{0} queries in collection at {1}", result.Count, DateTime.Now));
                    foreach (var query in result)
                    {
                        OnLogChange(string.Format("Id: {0}", query.Key));
                    }
                    OnLogChange("**********************************************************");
                }
                catch (Exception ex)
                {
                    OnLogChange(ex.Message);
                }
            });
        }

        public static Task ClearQueriesListAsync()
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                try
                {
                    var collection = mongo.GetCollection;
                    var filter = Builders<QueryInfo>.Filter.Empty;
                    collection.DeleteMany(filter);
                    OnLogChange("Queries list was cleared successfully");
                }
                catch (Exception exc)
                {
                    OnLogChange(exc.Message);
                }
            });
        }

        public static Task ClearAnswersAsync()
        {
            return Task.Run(() =>
            {
                AnswerHelper mongo = new AnswerHelper();
                try
                {
                    var collection = mongo.GetCollection;
                    var filter = Builders<AnswerInfo>.Filter.Empty;
                    collection.DeleteMany(filter);
                    OnLogChange("Answers list was cleared successfully");
                }
                catch (Exception exc)
                {
                    OnLogChange(exc.Message);
                }
            });
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timerStoped = true;
            //TODO: Clear query
        }

        #endregion

        #region Handlers
        private static void OnLogChange(string key, string value, bool isQuery)
        {
            if (isQuery)
            {
                _log = string.Format("Query - key {0}, parameter name {1}", key, value);
            }
            else
            {
                _log = string.Format("Answer - key {0}, value {1}", key, value);
            }
            if (LogChange != null)
            {
                LogChange(_log);
            }
        }

        private static void OnLogChange(string message)
        {
            _log = message;
            if (LogChange != null)
            {
                LogChange(_log);
            }
        }

        private static void OnGetQueries(List<QueryInfo> queries)
        {
            GetQueries(queries);
        }
        #endregion
    }
}
