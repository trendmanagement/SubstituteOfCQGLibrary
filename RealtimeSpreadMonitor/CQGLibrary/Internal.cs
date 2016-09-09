using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;
using FakeCQG;

namespace FakeCQG
{
    public partial class CQG 
    {
        //private string thisCQGObjUnqKey;
        private static System.Timers.Timer eventCheckingTimer;
        private static bool eventsCheckingON = false;

        #region Help objects
        public delegate void GetQueryInfosHandler(List<QueryInfo> queries);
        public static event GetQueryInfosHandler GetQueries;
        public delegate void LogHandler(string message);
        public static event LogHandler LogChange;
        private static string _log;
        private static int waitForRequestTime;

        public string Key { get; set; }
        const int index = 0;
        const int maxRequestTime = 30000; //30 s
        const int firstWaitForRequestTime = 5000; //5s
        const int nextWaitForRequestTime = 3000; //3s
        const int firstWaitForBoolTime = 1000; //1s
        const string IdName = "Key";
        public const string NoAnswerMessage = "Timer elapsed. No answer.";
        static System.Timers.Timer timer;
        static bool timerStoped;
        #endregion

        public CQG(string _Key)
        {
            this.Key = _Key;

        }

        public partial class CQGCEL 
        {
            private string thisCQGCELObjUnqKey;

            #region Structs of args
            struct TimedBarsResolvedArgs
            {
                public CQGTimedBars cqg_timed_bars;
                public CQGError cqg_error;
            }

            struct TimedBarsUpdatedArgs
            {
                public CQGTimedBars cqg_TimedBarsIn;
                public int index;
            }

            struct InstrumentSubscribedArgs
            {
                public string symbol;
                public CQGInstrument cqgInstrument;
            }

            struct InstrumentChangedArgs
            {
                public CQGInstrument cqgInstrument;
                public CQGQuotes quotes;
                public CQGInstrumentProperties props;
            }

            struct DataErrorArgs
            {
                public object cqg_error;
                public string error_description;
            }
            #endregion

            #region Event request strings
            string dataConnectionStatusChangedERS = "_ICQGCELEvents_DataConnectionStatusChangedEventHandler";
            string timedBarsResolvedERS = "_ICQGCELEvents_TimedBarsResolvedEventHandler";
            string timedBarsAddedERS = "_ICQGCELEvents_TimedBarsAddedEventHandler";
            string timedBarsUpdatedERS = "_ICQGCELEvents_TimedBarsUpdatedEventHandler";
            string instrumentSubscribedERS = "_ICQGCELEvents_InstrumentSubscribedEventHandler";
            string instrumentChangedEventERS = "_ICQGCELEvents_InstrumentChangedEventHandler";
            string dataErrorERS = "_ICQGCELEvents_DataErrorEventHandler";
            #endregion 

        }

        #region Internal CQG methods
        public static void SetProperty(string name, string objKey, object value)
        {
            bool isTrue = (bool)ExecuteTheQuery(QueryInfo.QueryType.Property, objKey, name, new object[] { value });
            if (!isTrue)
            {
                OnLogChange(String.Format("Setter ICQGCEL.{0} wasn't successfully completed", name));
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
                answer.ArgValues[0] = "!";
                answer.ArgValues[1] = args;

            }
            catch (Exception ex)
            {
                OnLogChange(ex.Message);
            }
        }
        public static object ExecuteTheQuery(QueryInfo.QueryType qType, string objKey, string name, object[] args = null)
        {
            if(!eventsCheckingON)
            {
                DataDictionaries.FillEventCheckingDictionary();

                eventsCheckingON = true;
            }

            Dictionary<int, string> argKeys = new Dictionary<int, string>();
            Dictionary<int, object> argVals = new Dictionary<int, object>();

            HashSet<Type> argTypes = new HashSet<Type> { typeof(object), typeof(int), typeof(double), typeof(string), typeof(char), typeof(string),
                typeof(long), typeof(short), typeof(sbyte), typeof(uint), typeof(bool) };

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
                    if (argTypes.Contains(arg.GetType()) || arg.GetType().IsEnum || arg.GetType().Assembly.FullName == coreAsmName)
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

            AnswerInfo result = WaitingForAnAnswer(key).GetAwaiter().GetResult();

            if (result != null)
            {
                if (result.Key == "value")
                {
                    return result.Value;
                }
                else
                {
                    return result.Key;
                }
            }
            else
            {
                OnLogChange(NoAnswerMessage);
                return result;
            }
        }
        public static Task<AnswerInfo> WaitingForAnAnswer(string key)
        {
            return Task.Run( () => {
                Thread.Sleep(firstWaitForRequestTime);
                AnswerInfo answer = default(AnswerInfo);
                bool isAnswer = false;
                do
                {
                    answer = GetAnswerData(key, out isAnswer);
                    if (!isAnswer)
                    {
                        Thread.Sleep(nextWaitForRequestTime);
                    }
                    else
                    {
                        return answer;
                        }
                    } while (!timerStoped);
                return answer;
            });
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
                    if (argKeys == null)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argVals: argVals);
                        OnLogChange(model.ToString());
                    }
                    else if (argVals == null)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argKeys);
                        OnLogChange(model.ToString());
                    }
                    else if (argKeys == null && argVals == null)
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
        public static AnswerInfo GetAnswerData(string id, out bool isAnswer)
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
                isAnswer = true;
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
                isAnswer = false;
                return default(AnswerInfo);
            }
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
                    OnLogChange("Queries was cleared successful");
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
                    OnLogChange("Answers was cleared successful");
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
            LogChange(_log);
        }
        private static void OnLogChange(string message)
        {
            _log = message;
            LogChange(_log);
        }
        private static void OnGetQueries(List<QueryInfo> queries)
        {
            GetQueries(queries);
        }
        #endregion

        #region Event handlers
        private void CQGLibrary_DataError(object cqg_error, string error_description)
        {
            Cqg_DataError(cqg_error, error_description);
        }
        private void Cqg_DataError(object cqg_error, string error_description)
        {
            //TODO: Relocate event handler
        }

        private void CQGLibrary_InstrumentSubscribed(string _symbol, CQGInstrument cqgInstrument)
        {
            global::CQG.CQGInstrument _cqgInstrument = (global::CQG.CQGInstrument)Enum.Parse(typeof(global::CQG.CQGInstrument), cqgInstrument.ToString());
            Cqg_InstrumentSubscribed(_symbol, _cqgInstrument);
        }
        private void Cqg_InstrumentSubscribed(string symbol, global::CQG.CQGInstrument cqg_instrument)
        {
            //TODO: Relocate event handler
        }

        private void CQGLibrary_TimedBarsUpdated(CQGTimedBars cqg_TimedBarsIn, int _index)
        {
            global::CQG.CQGTimedBars _cqg_TimedBarsIn = (global::CQG.CQGTimedBars)Enum.Parse(typeof(global::CQG.CQGTimedBars), cqg_TimedBarsIn.ToString());
            Cqg_TimedBarsUpdated(_cqg_TimedBarsIn, _index);
        }
        private void Cqg_TimedBarsUpdated(global::CQG.CQGTimedBars cqg_timed_bars, int index)
        {
            //TODO: Relocate event handler
        }

        private void CQGLibrary_TimedBarsAdded(CQGTimedBars cqg_TimedBarsIn)
        {
            global::CQG.CQGTimedBars _cqg_timed_bars = (global::CQG.CQGTimedBars)Enum.Parse(typeof(global::CQG.CQGTimedBars), cqg_TimedBarsIn.ToString());
            Cqg_TimedBarsAdded(_cqg_timed_bars);
        }
        private void Cqg_TimedBarsAdded(global::CQG.CQGTimedBars cqg_timed_bars)
        {
            //TODO: Relocate event handler
        }

        private void CQGLibrary_DataConnectionStatusChanged(eConnectionStatus new_status)
        {
            global::CQG.eConnectionStatus _new_status = (global::CQG.eConnectionStatus)Enum.Parse(typeof(global::CQG.eConnectionStatus), new_status.ToString());
            Cqg_DataConnectionStatusChanged(_new_status);
        }
        private void Cqg_DataConnectionStatusChanged(global::CQG.eConnectionStatus new_status)
        {
            //TODO: Relocate event handler
        }

        private void CQGLibrary_TimedBarsResolved(CQGTimedBars cqg_timed_bars, CQGError cqg_error)
        {
            global::CQG.CQGTimedBars _cqg_timed_bars = (global::CQG.CQGTimedBars)Enum.Parse(typeof(global::CQG.CQGTimedBars), cqg_timed_bars.ToString());
            global::CQG.CQGError _cqg_error = (global::CQG.CQGError)Enum.Parse(typeof(global::CQG.CQGError), cqg_error.ToString());
            Cqg_TimedBarsResolved(_cqg_timed_bars, _cqg_error);
        }
        private void Cqg_TimedBarsResolved(global::CQG.CQGTimedBars cqg_timed_bars, global::CQG.CQGError cqg_error)
        {
            //TODO: Relocate event handler
        }
        #endregion
    }
}
