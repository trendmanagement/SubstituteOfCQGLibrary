using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using FakeCQG;
using FakeCQG.Models;
using System.Threading.Tasks;

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
            switch (query.QueryType)
            {
                case QueryType.CallCtor:
                    {
                        string key;

                        switch (query.MemberName)
                        {
                            case "CQG.CQGCELClass":
                                key = CqgDataManagement.CEL_key;
                                break;
                            default:
                                //TODO: Make sure, that correct name of real CQG assembly passed to the CreateInstance method as arg below
                                object qObj = CQGAssm.CreateInstance(query.MemberName);
                                key = FakeCQG.CQG.CreateUniqueKey();
                                ServerDictionaries.PutObjectToTheDictionary(key, qObj);
                                break;
                        }

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: key);
                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.CallDtor:
                    {
                        ServerDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.GetProperty:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        try
                        {
                            var propV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.GetProperty, null, qObj, args);

                            if (FakeCQG.CQG.IsSerializableType(propV.GetType()))
                            {
                                string answerKey = "value";
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: answerKey, value: propV);
                            }
                            else
                            {
                                string answerKey = FakeCQG.CQG.CreateUniqueKey();
                                ServerDictionaries.PutObjectToTheDictionary(answerKey, propV);
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: answerKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName)
                            {
                                IsCQGException = true,
                                CQGException = new Action(() => { throw ex; })
                            };
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.SetProperty:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        try
                        {
                            qObj.GetType().InvokeMember(query.MemberName, BindingFlags.SetProperty, null, qObj, args);
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);
                        }
                        catch (Exception ex)
                        {
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName)
                            {
                                IsCQGException = true,
                                CQGException = new Action(() => { throw ex; })
                            };
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.CallMethod:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        try
                        {
                            object returnV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.InvokeMethod, null, qObj, args);

                            if (!object.ReferenceEquals(returnV, null))
                            {
                                if (FakeCQG.CQG.IsSerializableType(returnV.GetType()))
                                {
                                    var returnKey = "value";
                                    answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey, value: returnV);
                                }
                                else
                                {
                                    var returnKey = FakeCQG.CQG.CreateUniqueKey();
                                    ServerDictionaries.PutObjectToTheDictionary(returnKey, returnV);
                                    answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey);
                                }
                            }
                            else
                            {
                                var returnKey = "true";
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName)
                            {
                                IsCQGException = true,
                                CQGException = new Action(() => { throw ex; })
                            };
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.SubscribeToEvent:
                case QueryType.UnsubscribeFromEvent:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                        EventInfo ei = qObj.GetType().GetEvent(query.MemberName);

                        // Find corresponding CQG delegate
                        Type delType = FindDelegateType(CQGAssm, query.MemberName);

                        // Instantiate the delegate with our own handler
                        string handlerName = string.Format("_ICQGCELEvents_{0}EventHandlerImpl", query.MemberName);

                        MethodInfo handlerInfo = typeof(CQGEventHandlers).GetMethod(handlerName);
                        Delegate d = Delegate.CreateDelegate(delType, handlerInfo);

                        if (query.QueryType == QueryType.SubscribeToEvent)
                        {
                            // Subscribe our handler to CQG event
                            ei.AddEventHandler(qObj, d);
                        }
                        else if (query.QueryType == QueryType.UnsubscribeFromEvent)
                        {
                            // Unsubscribe our handler from CQG event
                            ei.RemoveEventHandler(qObj, d);
                        }

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);
                        PushAnswerAndDeleteQuery(answer);

                        if (query.QueryType == QueryType.SubscribeToEvent && query.MemberName == "DataConnectionStatusChanged")
                        {
                            // Fire this event explicitly, because data collector connects to real CQG beforehand and does not fire it anymore
                            CQGEventHandlers._ICQGCELEvents_DataConnectionStatusChangedEventHandlerImpl(
                                CqgDataManagement.CEL.IsStarted ? CQG.eConnectionStatus.csConnectionUp: CQG.eConnectionStatus.csConnectionDown);
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
            if (!Task.Run(() => FakeCQG.CQG.AnswerHelper.CheckAnswerAsync(answer.AnswerKey)).GetAwaiter().GetResult())
            {
                FakeCQG.CQG.AnswerHelper.PushAnswer(answer);
                FakeCQG.CQG.QueryHelper.DeleteProcessedQuery(answer.AnswerKey);
            }
        }

        public void ProcessEntireQueryList()
        {
            foreach (QueryInfo query in QueryList)
            {
                ProcessQuery(query);
                KeysOfQueriesInProcess.Remove(query.QueryKey);
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
                    foreach (KeyValuePair<int, string> argPair in query.ArgKeys)
                    {
                        args[argPair.Key] = ServerDictionaries.GetObjectFromTheDictionary(argPair.Value);
                    }
                }

                if (query.ArgValues != null)
                {
                    foreach (KeyValuePair<int, object> argPair in query.ArgValues)
                    {
                        args[argPair.Key] = argPair.Value;
                    }
                }
            }

            return args;
        }
    }
}
