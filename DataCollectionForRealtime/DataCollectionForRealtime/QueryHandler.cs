using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using FakeCQG;
using FakeCQG.Models;

[assembly: InternalsVisibleTo("UnitTestRealCQG")]

namespace DataCollectionForRealtime
{
    class QueryHandler
    {
        CQGDataManagement CqgDataManagement;

        Assembly CQGAssm;

        public List<QueryInfo> QueryList;
        HashSet<string> KeysOfQueriesInProcess;

        public Assembly CQGAssembly
        {
            get
            {
                return CQGAssm;
            }
        }

        public QueryHandler(CQGDataManagement cqgDM)
        {
            CqgDataManagement = cqgDM;
            QueryList = new List<QueryInfo>();
            KeysOfQueriesInProcess = new HashSet<string>();
            CQGAssm = cqgDM.CQGAssm;
        }

        internal QueryHandler(CQGDataManagement cqgDM, IList<QueryInfo> ql)
        {
            CqgDataManagement = cqgDM;
            QueryList = new List<QueryInfo>(ql);
            KeysOfQueriesInProcess = new HashSet<string>();
            CQGAssm = cqgDM.CQGAssm;
        }

        public void SetQueryList(List<QueryInfo> queries)
        {
            QueryList = queries;
        }

        public void CheckRequestsQueue()
        {
            FakeCQG.CQG.QueryHelper.ReadQueriesAsync(KeysOfQueriesInProcess);
        }

        public void ProcessQuery(QueryInfo query)
        {
            //Object where the data obtained after query execution is placed
            //This object will be sent to the DB
            AnswerInfo answer;

            //Handling of a request depending on its kind
            switch (query.TypeOfQuery)
            {
                case QueryInfo.QueryType.CallCtor:
                    {
                        string key;

                        switch (query.QueryName)
                        {
                            case "CQG.CQGCELClass":
                                key = CqgDataManagement.CEL_key;
                                break;
                            default:
                                //TODO: Make sure, that correct name of real CQG assembly passed to the CreateInstance method as arg below
                                object qObj = CQGAssm.CreateInstance(query.QueryName);
                                key = FakeCQG.CQG.CreateUniqueKey();
                                DataDictionaries.PutObjectToTheDictionary(key, qObj);
                                break;
                        }

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: key);
                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryInfo.QueryType.CallDtor:
                    {
                        DataDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryInfo.QueryType.GetProperty:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        var propV = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.GetProperty, null, qObj, args);

                        if (FakeCQG.CQG.IsSerializableType(propV.GetType()))
                        {
                            string answerKey = "value";
                            answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: answerKey, val: propV);
                        }
                        else
                        {
                            string answerKey = FakeCQG.CQG.CreateUniqueKey();
                            DataDictionaries.PutObjectToTheDictionary(answerKey, propV);
                            answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: answerKey);
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryInfo.QueryType.SetProperty:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        qObj.GetType().InvokeMember(query.QueryName, BindingFlags.SetProperty, null, qObj, args);

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryInfo.QueryType.CallMethod:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        object returnV = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.InvokeMethod, null, qObj, args);

                        if (!object.ReferenceEquals(returnV, null))
                        {
                            if (FakeCQG.CQG.IsSerializableType(returnV.GetType()))
                            {
                                var returnKey = "value";
                                answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey, val: returnV);
                            }
                            else
                            {
                                var returnKey = FakeCQG.CQG.CreateUniqueKey();
                                DataDictionaries.PutObjectToTheDictionary(returnKey, returnV);
                                answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey);
                            }
                        }
                        else
                        {
                            var returnKey = "true";
                            answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey);
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryInfo.QueryType.SubscribeToEvent:
                case QueryInfo.QueryType.UnsubscribeFromEvent:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                        EventInfo ei = qObj.GetType().GetEvent(query.QueryName);

                        // Find corresponding CQG delegate
                        Type delType = FindDelegateType(CQGAssm, query.QueryName);

                        // Instantiate the delegate with our own handler
                        string handlerName = string.Format("_ICQGCELEvents_{0}EventHandlerImpl", query.QueryName);

                        MethodInfo handlerInfo = typeof(CQGEventHandlers).GetMethod(handlerName);
                        Delegate d = Delegate.CreateDelegate(delType, handlerInfo);

                        if (query.TypeOfQuery == QueryInfo.QueryType.SubscribeToEvent)
                        {
                            // Subscribe our handler to CQG event
                            ei.AddEventHandler(qObj, d);
                        }
                        else if (query.TypeOfQuery == QueryInfo.QueryType.UnsubscribeFromEvent)
                        {
                            // Unsubscribe our handler from CQG event
                            ei.RemoveEventHandler(qObj, d);
                        }

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true, isEventQ: true);
                        PushAnswerAndDeleteQuery(answer);

                        if(query.QueryName == "DataConnectionStatusChanged")
                        {
                            CQGEventHandlers._ICQGCELEvents_DataConnectionStatusChangedEventHandlerImpl((FakeCQG.eConnectionStatus)CQG.eConnectionStatus.csConnectionUp);
                        }
                    }
                    break;

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        internal void PushAnswerAndDeleteQuery(AnswerInfo answer)
        {
            FakeCQG.CQG.AnswerHelper.PushAnswer(answer);
            FakeCQG.CQG.QueryHelper.DeleteProcessedQuery(answer.Key);
        }

        public void ProcessEntireQueryList()
        {
            foreach (QueryInfo query in QueryList)
            {
                ProcessQuery(query);
                KeysOfQueriesInProcess.Remove(query.Key);
            }
        }

        internal static Type FindDelegateType(Assembly assm, string eventName)
        {
            string delegateTypeName = string.Format("_ICQGCELEvents_{0}EventHandler", eventName);
            foreach (Type type in assm.ExportedTypes)
            {
                if (IsDelegate(type))
                {
                    MethodInfo minfo = type.GetMethod("Invoke");
                    if (type.Name == delegateTypeName)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        internal static bool IsDelegate(Type type)
        {
            return type.BaseType == typeof(MulticastDelegate);
        }

        static object[] ParseInputArgsFromQuery(QueryInfo query)
        {
            int numArgs = (query.ArgKeys != null ? query.ArgKeys.Count : 0) + (query.ArgValues != null ? query.ArgValues.Count : 0);
            var args = new object[numArgs];

            if (numArgs != 0)
            {
                if (query.ArgKeys != null)
                {
                    foreach (KeyValuePair<string, string> argPair in query.ArgKeys)
                    {
                        args[int.Parse(argPair.Key)] = DataDictionaries.GetObjectFromTheDictionary(argPair.Value);
                    }
                }

                if (query.ArgValues != null)
                {
                    foreach (KeyValuePair<string, object> argPair in query.ArgValues)
                    {
                        args[int.Parse(argPair.Key)] = argPair.Value;
                    }
                }
            }

            return args;
        }
    }
}
