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

        object QueriesProcessingLock = new object();

        public List<QueryInfo> QueryList;

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
            CQGAssm = cqgDM.CQGAssm;
        }

        internal QueryHandler(CQGDataManagement cqgDM, IList<QueryInfo> ql)
        {
            CqgDataManagement = cqgDM;
            QueryList = new List<QueryInfo>(ql);
            CQGAssm = cqgDM.CQGAssm;
        }

        public void SetQueryList(List<QueryInfo> queries)
        {
            QueryList = queries;
            Program.miniMonitor.SetNumberOfQueriesInLine(QueryList.Count);
        }

        public void CheckRequestsQueue()
        {
            FakeCQG.CQG.QueryHelper.ReadQueries();
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

                        if (query.MemberName == "CQG.CQGCELClass")
                        {
                            key = CqgDataManagement.CEL_key;
                        }
                        else
                        {
                            object qObj = CQGAssm.CreateInstance(query.MemberName);
                            key = FakeCQG.CQG.CreateUniqueKey();
                            ServerDictionaries.PutObjectToTheDictionary(key, qObj);
                        }

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: key);
                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.CallDtor:
                    {
                        if (query.ObjectKey != CqgDataManagement.CEL_key)
                        {
                            ServerDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);
                        }

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.GetProperty:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = FakeCQG.CQG.ParseInputArgsFromQueryInfo(query);

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

                        object[] args = FakeCQG.CQG.ParseInputArgsFromQueryInfo(query);

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

                        object[] args = FakeCQG.CQG.ParseInputArgsFromQueryInfo(query);

                        try
                        {
                            object returnV;
                            if (query.MemberName == "get_ItemById")
                            {
                                returnV = qObj.GetType().InvokeMember("ItemById", BindingFlags.GetProperty, null, qObj, args);
                            }
                            else
                            {
                                returnV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.InvokeMethod, null, qObj, args);
                            }      

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
                        System.Reflection.EventInfo ei = qObj.GetType().GetEvent(query.MemberName);

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

                        if (query.QueryType == QueryType.SubscribeToEvent &&
                            query.MemberName == "DataConnectionStatusChanged" &&
                            CqgDataManagement.CEL.IsStarted)
                        {
                            // Fire this event explicitly, because data collector connects to real CQG beforehand and does not fire it anymore
                            CQGEventHandlers._ICQGCELEvents_DataConnectionStatusChangedEventHandlerImpl(CQG.eConnectionStatus.csConnectionUp);
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
            FakeCQG.CQG.QueryHelper.DeleteProcessedQuery(answer.AnswerKey);
        }

        public void ProcessEntireQueryList()
        {
            lock (QueriesProcessingLock)
            {
                foreach (QueryInfo query in QueryList)
                {
                    ProcessQuery(query);
                }
                QueryList.Clear();
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
    }
}
