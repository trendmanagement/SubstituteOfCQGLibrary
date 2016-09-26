using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG
{
    public partial class CQG 
    {
        #region Helper objects
        static bool EventsCheckingON = false;

        static string Log;

        const string IdName = "Key";

        public delegate void GetQueryInfosHandler(List<QueryInfo> queries);
        public static event GetQueryInfosHandler GetQueries;
        public delegate void LogHandler(string message);
        public static event LogHandler LogChange;

        // Changed the access level of visibility for testing
        public static int QueryTimeout = int.MaxValue;  // Currently set to the max value for debugging
        public const string NoAnswerMessage = "Timer elapsed. No answer.";
#endregion

#region MongoDB communication methods
        public static void CommonEventHandler(string name, object[] args = null)
        {
            AnswerHelper mongo = new AnswerHelper();
            var allAnswers = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("QueryName", name);
            AnswerInfo answer = null;
            try
            {
                answer = allAnswers.Find(filter).First();
                var argValues = new Dictionary<int, object>();
                argValues.Add(0, "!");
                argValues.Add(1, args);
                var update = Builders<AnswerInfo>.Update.Set("ArgValues", argValues);
                //TODO: deserialize argValues from dictionary to bson
                //allAnswers.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                OnLogChange(ex.Message);
            }
        }

        public static object ExecuteTheQuery(QueryInfo.QueryType qType, string objKey, string name, object[] args = null)
        {
            if (!EventsCheckingON)
            {
                DataDictionaries.FillEventCheckingDictionary();

                EventsCheckingON = true;
            }

            var argKeys = new Dictionary<string, string>();
            var argVals = new Dictionary<string, object>();

            string queryKey = CreateUniqueKey();

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    object arg = args[i];
                    Type argType = arg.GetType();

                    if (IsSerializableType(argType))
                    {
                        argVals.Add(i.ToString(), arg);
                    }
                    else
                    {
                        argKeys.Add(i.ToString(), argType.GetField("dcObjKey", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(arg).ToString());
                    }
                }
            }

            QueryInfo qInfo = CreateQuery(qType, queryKey, objKey, name, argKeys, argVals);
            Task.Run(() => PushQueryAsync(qInfo));

            AnswerInfo result = WaitingForAnAnswer(queryKey);

            if (result.ValueKey == "value")
            {
                return result.Value;
            }
            else if (result.ValueKey == "true")
            {
                return true;
            }
            else
            {
                return result.ValueKey;
            }
        }

        public static AnswerInfo WaitingForAnAnswer(string queryKey)
        {
            AnswerInfo answer = null;
            Task task = Task.Run(() => { answer = GetAnswerData(queryKey); });
            bool success = task.Wait(QueryTimeout);
            if (success)
            {
                DataDictionaries.IsAnswer.Remove(queryKey);
                return answer;
            }
            else
            {
                OnLogChange(NoAnswerMessage);
                task.Dispose();
                throw new TimeoutException();
            }
        }

        public static QueryInfo CreateQuery(
            QueryInfo.QueryType qType,
            string key,
            string objKey,
            string name,
            Dictionary<string, string> argKeys = null,
            Dictionary<string, object> argVals = null)
        {
            QueryInfo model;

            switch (qType)
            {
                case QueryInfo.QueryType.Ctor:
                case QueryInfo.QueryType.Dtor:
                case QueryInfo.QueryType.GetProperty:
                case QueryInfo.QueryType.SetProperty:
                case QueryInfo.QueryType.Method:
                    if (argKeys.Count == 0 && argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name);
                    }
                    else if (argKeys.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argVals: argVals);
                    }
                    else if (argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, key, objKey, name, argKeys);
                    }
                    else
                    {
                        model = new QueryInfo(qType, key, objKey, name, argKeys, argVals);
                    }
                    break;
                case QueryInfo.QueryType.Event:
                    model = new QueryInfo(qType, key, objKey, name, argVals: argVals);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnLogChange(model.ToString());

            DataDictionaries.IsAnswer.Add(key, false);

            return model;
        }

        public static Task PushQueryAsync(QueryInfo model)
        {
            return Task.Run(() => PushQuery(model));
        }

        static void PushQuery(QueryInfo model)
        {
            QueryHelper mongo = new QueryHelper();
            var allQueries = mongo.GetCollection;
            allQueries.InsertOne(model);
            OnLogChange(model.Key, model.QueryName, true);
        }

        public static Task PushAnswerAsync(AnswerInfo model)
        {
            return Task.Run(() => PushAnswer(model));
        }

        static void PushAnswer(AnswerInfo model)
        {
            AnswerHelper mongo = new AnswerHelper();
            var allAnswers = mongo.GetCollection;
            allAnswers.InsertOne(model);
            OnLogChange(model.Key, model.QueryName, true);
        }

        public static AnswerInfo GetAnswerData(string id, out bool isAns)
        {
            AnswerHelper mongo = new AnswerHelper();
            var allAnswers = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq(IdName, id);
            AnswerInfo answer = null;
            try
            {
                answer = allAnswers.Find(filter).First();
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
            catch (Exception)
            {
                OnLogChange(id, "null", false);
                isAns = false;
                return null;
            }
        }

        public static AnswerInfo GetAnswerData(string id)
        {
            AnswerHelper mongo = new AnswerHelper();
            var allAnswers = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq(IdName, id);
            AnswerInfo answer = null;
            while (!DataDictionaries.IsAnswer[id])
            {
                try
                {
                    answer = allAnswers.Find(filter).First();
                    OnLogChange(answer.Key, answer.ValueKey, false);
                    RemoveAnswerAsync(answer.Key);
                    DataDictionaries.IsAnswer[id] = true;
                }
                catch (Exception)
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
                var allQueries = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Eq(IdName, Id);
                try
                {
                    QueryInfo result = null;
                    try
                    {
                        result = allQueries.Find(filter).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        OnLogChange(ex.Message);
                    }
                    return result != null;
                }
                catch (Exception)
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
                var allAnswers = mongo.GetCollection;
                var filter = Builders<AnswerInfo>.Filter.Eq(IdName, Id);
                try
                {
                    AnswerInfo result = null;
                    try
                    {
                        result = allAnswers.Find(filter).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        OnLogChange(ex.Message);
                    }
                    return (result != null) ? true : false;
                }
                catch (Exception)
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
            var allAnswers = mongo.GetCollection;
            var filter = Builders<AnswerInfo>.Filter.Eq("QueryName", name);
            AnswerInfo answer = null;

            answer = allAnswers.Find(filter).First();
            argValues = answer.ArgValues;
            
            return (object[])argValues[1];
        }

        public static Task RemoveQueryAsync(string key)
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                var allQueries = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Eq(IdName, key);
                allQueries.DeleteOne(filter);
            });
        }

        public static Task RemoveAnswerAsync(string key)
        {
            return Task.Run(() =>
            {
                AnswerHelper mongo = new AnswerHelper();
                var allAnswers = mongo.GetCollection;
                var filter = Builders<AnswerInfo>.Filter.Eq(IdName, key);
                allAnswers.DeleteOne(filter);
            });
        }

        public static Task ReadQueriesAsync()
        {
            return Task.Run(() =>
            {
                QueryHelper mongo = new QueryHelper();
                var allQueries = mongo.GetCollection;
                var filter = Builders<QueryInfo>.Filter.Empty;
                List<QueryInfo> result = new List<QueryInfo>();
                try
                {
                    result = allQueries.Find(filter).ToList();
                    OnGetQueries(result);
                    OnLogChange("**********************************************************");
                    OnLogChange(string.Format("{0} quer(y/ies) in collection at {1}", result.Count, DateTime.Now));
                    foreach (QueryInfo query in result)
                    {
                        OnLogChange(string.Format("QueryType: \"{0}\", ObjectType: \"{1}\", QueryId: \"{2}\"", query.TypeOfQuery, query.QueryName, query.Key));
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
                    var allQueries = mongo.GetCollection;
                    var filter = Builders<QueryInfo>.Filter.Empty;
                    allQueries.DeleteMany(filter);
                    OnLogChange("Queries list was cleared successfully");
                }
                catch (Exception exc)
                {
                    OnLogChange(exc.Message);
                }
            });
        }

        public static Task ClearAnswersListAsync()
        {
            return Task.Run(() =>
            {
                AnswerHelper mongo = new AnswerHelper();
                try
                {
                    var allAnswers = mongo.GetCollection;
                    var filter = Builders<AnswerInfo>.Filter.Empty;
                    allAnswers.DeleteMany(filter);
                    OnLogChange("Answers list was cleared successfully");
                }
                catch (Exception exc)
                {
                    OnLogChange(exc.Message);
                }
            });
        }

#endregion

#region Handlers
        internal static void OnLogChange(string key, string value, bool isQuery)
        {
            if (isQuery)
            {
                Log = string.Format("Query - key {0}, parameter name {1}", key, value);
            }
            else
            {
                Log = string.Format("Answer - key {0}, value {1}", key, value);
            }
            if (LogChange != null)
            {
                LogChange(Log);
            }
        }

        internal static void OnLogChange(string message)
        {
            Log = message;
            if (LogChange != null)
            {
                LogChange(Log);
            }
        }

        private static void OnGetQueries(List<QueryInfo> queries)
        {
            GetQueries(queries);
        }
#endregion

        public static bool IsSerializableType(Type type)
        {
            // Keep this method in sync the same method in CodeGenerator project
            return type.IsValueType || (type.Assembly.FullName.Substring(0, 8) == "mscorlib" && type.Name != "__ComObject");
        }

        public static string CreateUniqueKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
