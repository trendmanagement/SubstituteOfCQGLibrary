using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FakeCQG.Helpers;
using FakeCQG.Models;

namespace FakeCQG
{
    static class Keys
    {
        public const string IdName = "Key";
        public const string QueryName = "QueryName";
        public const string ArgValues = "ArgValues";
        public const string HandshakerId = "_id";
    }

    public static partial class CQG
    {
        #region Helper objects

        static string Log;
        public static object LogLock = new object();

        public delegate void LogHandler(string message);
        public static event LogHandler LogChange;

        // Changed the access level of visibility for testing
        public static int QueryTimeout = int.MaxValue;  // Currently set to the max value for debugging
        public const string NoAnswerMessage = "Timer elapsed. No answer.";

        public static QueryHelper QueryHelper;
        public static AnswerHelper AnswerHelper;

        static bool FirstCall = true;

        #endregion

        #region MongoDB communication methods

        public static object ExecuteTheQuery(QueryInfo.QueryType qType, string objKey, string name, object[] args = null)
        {
            if (FirstCall)
            {
                // Lazy connection to MongoDB
                QueryHelper = new QueryHelper();
                AnswerHelper = new AnswerHelper();

                // Start handshaking
                Handshaking.Subscriber.ListenForHanshaking();

                FirstCall = false;
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
            Task.Run(() => QueryHelper.PushQueryAsync(qInfo));

            AnswerInfo result = WaitingForAnAnswer(queryKey, qType);

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

        public static AnswerInfo WaitingForAnAnswer(string queryKey, QueryInfo.QueryType qType)
        {
            AnswerInfo answer = null;
            Task task = Task.Run(() => { answer = AnswerHelper.GetAnswerData(queryKey); });
            bool success = task.Wait(QueryTimeout);
            if (success)
            {
                DataDictionaries.IsAnswer.Remove(queryKey);
                if (qType == QueryInfo.QueryType.CallCtor)
                {
                    DataDictionaries.FillEventCheckingDictionary(answer.ValueKey, answer.QueryName);
                }
                return answer;
            }
            else
            {
                OnLogChange(NoAnswerMessage);
                task = null;
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
                case QueryInfo.QueryType.CallCtor:
                case QueryInfo.QueryType.CallDtor:
                case QueryInfo.QueryType.GetProperty:
                case QueryInfo.QueryType.SetProperty:
                case QueryInfo.QueryType.CallMethod:

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
                case QueryInfo.QueryType.SubscribeToEvent:
                case QueryInfo.QueryType.UnsubscribeFromEvent:
                    model = new QueryInfo(qType, key, objKey, name);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnLogChange(model.ToString());

            DataDictionaries.IsAnswer.Add(key, false);

            return model;
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

        #endregion

        public static void SubscriberChecking(string name, string objKey, bool isSubscriptionRequired, bool isUnsubscriptionRequired)
        {
            if (isUnsubscriptionRequired)
            {
                ExecuteTheQuery(QueryInfo.QueryType.UnsubscribeFromEvent, objKey, name);
                DataDictionaries.EventCheckingDictionary[objKey][name] = false;
            }
            else if (isSubscriptionRequired)
            {
                ExecuteTheQuery(QueryInfo.QueryType.SubscribeToEvent, objKey, name);
                DataDictionaries.EventCheckingDictionary[objKey][name] = true;
            }
        }

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
