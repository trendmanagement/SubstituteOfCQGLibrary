﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FakeCQG.Helpers;
using FakeCQG.Models;

namespace FakeCQG
{
    static class Keys
    {
        public const string QueryKey = "QueryKey";
        public const string AnswerKey = "AnswerKey";
        public const string EventKey = "EventKey";
        public const string IdName = "Key";
        public const string QueryName = "QueryName";
        public const string EventName = "EventName";
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
        public static EventHelper EventHelper;

        static bool FirstCall = true;

        #endregion

        #region MongoDB communication methods

        public static object ExecuteTheQuery(
            QueryType queryType,
            string dcObjKey = null,
            string memName = null,
            object[] args = null)
        {
            if (FirstCall)
            {
                // Lazy connection to MongoDB
                QueryHelper = new QueryHelper();
                AnswerHelper = new AnswerHelper();
                EventHelper = new EventHelper();
            }

            Dictionary<int, string> argKeys;
            Dictionary<int, object> argVals;
            PutArgsFromArrayIntoTwoDicts(args, true, out argKeys, out argVals);

            string queryKey = CreateUniqueKey();

            QueryInfo queryInfo = CreateQuery(queryType, queryKey, dcObjKey, memName, argKeys, argVals);

            Task.Run(() => QueryHelper.PushQueryAsync(queryInfo));

            AnswerInfo result = WaitingForAnAnswer(queryKey, queryType);

            if (FirstCall)
            {
                // Start handshaking
                Handshaking.Subscriber.ListenForHanshaking();

                FirstCall = false;
            }

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

        public static AnswerInfo WaitingForAnAnswer(string queryKey, QueryType queryType)
        {
            AnswerInfo answer = null;
            Task task = Task.Run(() => { answer = AnswerHelper.GetAnswerData(queryKey); });
            bool success = task.Wait(QueryTimeout);
            if (success)
            {
                ClientDictionaries.IsAnswer.Remove(queryKey);
                if (queryType == QueryType.CallCtor)
                {
                    ClientDictionaries.FillEventCheckingDictionary(answer.ValueKey, answer.MemberName);
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
            QueryType qType,
            string qKey,
            string objKey,
            string memName,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argVals = null)
        {
            QueryInfo model;

            switch (qType)
            {
                case QueryType.CallCtor:
                    model = new QueryInfo(qType, qKey, memberName: memName);
                    ClientDictionaries.ObjectNames.Add(objKey);
                    break;
                case QueryType.CallDtor:
                    model = new QueryInfo(qType, qKey, objectKey: objKey);
                    ClientDictionaries.ObjectNames.Remove(objKey);
                    break;
                case QueryType.GetProperty:
                case QueryType.SetProperty:
                case QueryType.CallMethod:
                    if (argKeys.Count == 0 && argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, qKey, objKey, memName);
                    }
                    else if (argKeys.Count == 0)
                    {
                        model = new QueryInfo(qType, qKey, objKey, memName, argValues: argVals);
                    }
                    else if (argVals.Count == 0)
                    {
                        model = new QueryInfo(qType, qKey, objKey, memName, argKeys);
                    }
                    else
                    {
                        model = new QueryInfo(qType, qKey, objKey, memName, argKeys, argVals);
                    }
                    break;
                case QueryType.SubscribeToEvent:
                case QueryType.UnsubscribeFromEvent:
                    model = new QueryInfo(qType, qKey, objKey, memName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnLogChange(model.ToString());

            ClientDictionaries.IsAnswer.Add(qKey, false);

            return model;
        }

        #endregion

        #region Handlers

        public static void CommonEventHandler(
            string name,
            object[] args = null)
        {
            Dictionary<int, string> argKeys;
            Dictionary<int, object> argVals;
            PutArgsFromArrayIntoTwoDicts(args, false, out argKeys, out argVals);

            string eventKey = CreateUniqueKey();

            var eventInfo = new Models.EventInfo(eventKey, name, argKeys, argVals);

            Task.Run(() => EventHelper.FireEvent(eventInfo));
        }

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

        public static void PutArgsFromArrayIntoTwoDicts(
            object[] args,
            bool isClientOrServer,
            out Dictionary<int, string> argKeys,
            out Dictionary<int, object> argVals)
        {
            argKeys = new Dictionary<int, string>();
            argVals = new Dictionary<int, object>();

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    object arg = args[i];
                    Type argType = arg.GetType();

                    if (IsSerializableType(argType))
                    {
                        argVals.Add(i, arg);
                    }
                    else
                    {
                        string argKey;
                        if (isClientOrServer)
                        {
                            argKey = (string)argType.GetField("dcObjKey", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(arg);
                        }
                        else
                        {
                            argKey = CQG.CreateUniqueKey();
                            ServerDictionaries.PutObjectToTheDictionary(argKey, arg);
                        }
                        argKeys.Add(i, argKey);
                    }
                }
            }
        }

        private static object[] GetArgsIntoArrayFromTwoDicts(
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null)
        {
            int numArgs = (argKeys != null ? argKeys.Count : 0) + (argValues != null ? argValues.Count : 0);
            var args = new object[numArgs];

            if (numArgs != 0)
            {
                if (argKeys != null)
                {
                    foreach (KeyValuePair<int, string> argPair in argKeys)
                    {
                        args[argPair.Key] = ServerDictionaries.GetObjectFromTheDictionary(argPair.Value);
                    }
                }

                if (argValues != null)
                {
                    foreach (KeyValuePair<int, object> argPair in argValues)
                    {
                        args[argPair.Key] = argPair.Value;
                    }
                }
            }

            return args;
        }

        public static object[] ParseInputArgsFromQueryInfo(QueryInfo queryInfo)
        {
            return GetArgsIntoArrayFromTwoDicts(queryInfo.ArgKeys, queryInfo.ArgValues);
        }

        public static object[] ParseInputArgsFromEventInfo(Models.EventInfo eventInfo)
        {
            return GetArgsIntoArrayFromTwoDicts(eventInfo.ArgKeys, eventInfo.ArgValues);
        }

        public static void SubscriberChecking(string name, string objKey, bool isSubscriptionRequired, bool isUnsubscriptionRequired)
        {
            if (isUnsubscriptionRequired)
            {
                ExecuteTheQuery(QueryType.UnsubscribeFromEvent, objKey, name);
                ClientDictionaries.EventCheckingDictionary[objKey][name] = false;
            }
            else if (isSubscriptionRequired)
            {
                ExecuteTheQuery(QueryType.SubscribeToEvent, objKey, name);
                ClientDictionaries.EventCheckingDictionary[objKey][name] = true;
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
