using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using FakeCQG.Internal.Handshaking;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;

namespace FakeCQG
{
    namespace Internal
    {
        public static class Keys
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

        public enum LogModeEnum
        {
            Muted,
            Filtered,
            Unfiltered
        }

        public static partial class Core
        {
            #region Helper objects

            static string Log;
            public static object LogLock = new object();
            static HashSet<string> LogHash = new HashSet<string>();
            public static LogModeEnum LogMode = LogModeEnum.Filtered;

            public delegate void LogHandler(string message);
            public static event LogHandler LogChange;

            // Changed the access level of visibility for testing
            public static int QueryTimeout = int.MaxValue;  // Currently set to the max value for debugging
            public const string NoAnswerMessage = "Timer elapsed. No answer.";

            // Main helper objects of each database collection
            public static QueryHelper QueryHelper;
            public static AnswerHelper AnswerHelper;
            public static EventHelper EventHelper;

            // Variable that contains information about is data collector form closed or not.
            public static bool isDCClosed = false;
            static Timer isDCClosedChekingTimer = new Timer();
            static int isDCClosedChekingInterval = 30;
            static bool locked;

            static bool FirstCall = true;

            #endregion

            #region MongoDB communication methods

            // Main method that contains all parts of executing the query on application side
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

                    isDCClosedChekingTimer.Interval = isDCClosedChekingInterval;
                    isDCClosedChekingTimer.Elapsed += isDCClosedChekingTimer_Tick;
                    isDCClosedChekingTimer.AutoReset = true;
                    isDCClosedChekingTimer.Enabled = true;
                }

                Dictionary<int, string> argKeys;
                Dictionary<int, object> argVals;
                PutArgsFromArrayIntoTwoDicts(args, true, out argKeys, out argVals);

                string queryKey = CreateUniqueKey();

                QueryInfo queryInfo = CreateQuery(queryType, queryKey, dcObjKey, memName, argKeys, argVals);

                QueryHelper.PushQuery(queryInfo);

                AnswerInfo result = WaitingForAnAnswer(queryKey, queryType);

                if (FirstCall)
                {
                    // Start handshaking
                    Subscriber.ListenForHanshaking();

                    FirstCall = false;
                }

                if (result == null)
                {
                    return default(object);
                }

                if (result.IsCQGException)
                {
                    var exception = new Exception(result.CQGException.Message);
                    exception.Source = result.CQGException.Sourse;
                    throw exception;
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
                answer = AnswerHelper.GetAnswerData(queryKey);

                if (answer != null)
                {
                    ClientDictionaries.IsAnswer.Remove(queryKey);

                    // If query type of successfully extracted non empty answer tells about creation of new object
                    // own dictionary of event checking must be created, filled for that object and added 
                    // to the common dictionary of current application
                    if (queryType == QueryType.CallCtor)
                    {
                        ClientDictionaries.FillEventCheckingDictionary(answer.ValueKey, answer.MemberName);
                    }
                }
                else
                {
                    OnLogChange(NoAnswerMessage);
                }
                return answer;
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

                if (argKeys != null && argKeys.Count == 0)
                {
                    argKeys = null;
                }
                if (argVals != null && argVals.Count == 0)
                {
                    argVals = null;
                }

                switch (qType)
                {
                    case QueryType.CallCtor:
                        model = new QueryInfo(qType, qKey, memberName: memName, argKeys: argKeys, argValues: argVals);
                        ClientDictionaries.ObjectNames.Add(objKey);
                        break;
                    case QueryType.CallDtor:
                        model = new QueryInfo(qType, qKey, objectKey: objKey);
                        ClientDictionaries.ObjectNames.Remove(objKey);
                        break;
                    case QueryType.GetProperty:
                    case QueryType.SetProperty:
                    case QueryType.CallMethod:
                        model = new QueryInfo(qType, qKey, objKey, memName, argKeys, argVals);
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

            // Check by timer, whether the form of data collector is closed
            private static void isDCClosedChekingTimer_Tick(Object source, System.Timers.ElapsedEventArgs e)
            {
                if (locked)
                {
                    return;
                }
                else
                {
                    locked = true;
                    try
                    {
                        object[] isDCClosedArg;
                        if (EventHelper.CheckWhetherEventHappened("DCClosed", out isDCClosedArg))
                        {
                            isDCClosed = true;
                        }
                    }
                    finally
                    {
                        locked = false;
                    }
                }
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
                LogChange?.Invoke(Log);
            }

            internal static void OnLogChange(string message)
            {
                Log = message;
                bool isNewMessage = LogHash.Add(message);
                switch (LogMode)
                {
                    case LogModeEnum.Muted:
                        break;
                    case LogModeEnum.Filtered:
                        if (isNewMessage)
                        {
                            LogChange?.Invoke(Log);
                        }
                        break;
                    case LogModeEnum.Unfiltered:
                        LogChange?.Invoke(Log);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            #endregion

            #region Utilities

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
                                argKey = Core.CreateUniqueKey();
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
                // Keep this method in sync with the same method in CodeGenerator project
                return type.IsValueType || (type.Assembly.FullName.Substring(0, 8) == "mscorlib" && type.Name != "__ComObject");
            }

            public static string CreateUniqueKey()
            {
                return Guid.NewGuid().ToString("N");
            }

            #endregion
        }
    }
}
