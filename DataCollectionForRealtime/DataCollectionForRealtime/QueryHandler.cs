using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FakeCQG.Internal;
using FakeCQG.Internal.Helpers;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

[assembly: InternalsVisibleTo("UnitTestRealCQG")]

namespace DataCollectionForRealtime
{
    class QueryHandler
    {
        CQGDataManagement CqgDataManagement;

        Assembly CQGAssm;

        object QueriesProcessingLock = new object();

        public List<QueryInfo> QueryList;

        public delegate void NewQueriesReadyHandler(List<QueryInfo> queries);
        public event NewQueriesReadyHandler NewQueriesReady;

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
        }

        public void CheckRequestsQueue()
        {
            ReadQueries();
        }

        public void ProcessQuery(QueryInfo query)
        {
            // Object where the data obtained after query execution is placed
            // This object will be sent to the DB
            AnswerInfo answer;

            // Handling of the request depending on its kind
            switch (query.QueryType)
            {
                case QueryType.CallCtor:
                    {
                        try
                        {
                            string key;

                            if (query.MemberName == "CQG.CQGCELClass")
                            {
                                key = CqgDataManagement.CEL_key;
                            }
                            else
                            {
                                object[] args = Core.ParseInputArgsFromQueryInfo(query);
                                object qObj = CQGAssm.CreateInstance(query.MemberName, false, BindingFlags.CreateInstance, null, args, null, null);
                                key = Core.CreateUniqueKey();
                                ServerDictionaries.PutObjectToTheDictionary(key, qObj);
                            }

                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: key);
                        }
                        catch (Exception ex)
                        {
                            answer = CreateExceptionAnswer(ex, query);
                        }

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

                        object[] args = Core.ParseInputArgsFromQueryInfo(query);

                        try
                        {
                            var propV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.GetProperty, null, qObj, args);

                            if (Core.IsSerializableType(propV.GetType()))
                            {
                                string answerKey = "value";
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: answerKey, value: propV);
                            }
                            else
                            {
                                string answerKey = Core.CreateUniqueKey();
                                ServerDictionaries.PutObjectToTheDictionary(answerKey, propV);
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: answerKey);
                            }
                        }
                        catch (Exception ex)
                        {
                            answer = CreateExceptionAnswer(ex, query);
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.SetProperty:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = Core.ParseInputArgsFromQueryInfo(query);

                        try
                        {
                            qObj.GetType().InvokeMember(query.MemberName, BindingFlags.SetProperty, null, qObj, args);
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);
                        }
                        catch (Exception ex)
                        {
                            answer = CreateExceptionAnswer(ex, query);
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.CallMethod:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = Core.ParseInputArgsFromQueryInfo(query);

                        try
                        {
                            object returnV;

                            bool isGetter = query.MemberName.StartsWith("get_");
                            bool isSetter = query.MemberName.StartsWith("set_");
                            if (isGetter || isSetter)
                            {
                                // Access property instead of calling method
                                string propName = query.MemberName.Substring(4);
                                BindingFlags invokeAttr = isGetter ? BindingFlags.GetProperty : BindingFlags.SetProperty;
                                returnV = qObj.GetType().InvokeMember(propName, invokeAttr, null, qObj, args);
                            }
                            else
                            {
                                returnV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.InvokeMethod, null, qObj, args);
                            }

                            if (!object.ReferenceEquals(returnV, null))
                            {
                                if (Core.IsSerializableType(returnV.GetType()))
                                {
                                    var returnKey = "value";
                                    answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey, value: returnV);
                                }
                                else
                                {
                                    var returnKey = Core.CreateUniqueKey();
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
                            answer = CreateExceptionAnswer(ex, query);
                        }

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.SubscribeToEvent:
                case QueryType.UnsubscribeFromEvent:
                    {
                        object qObj = ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        try
                        {
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
                        }
                        catch (Exception ex)
                        {
                            answer = CreateExceptionAnswer(ex, query);
                        }

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
            Core.AnswerHelper.PushAnswer(answer);
            DeleteProcessedQuery(answer.AnswerKey);
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

        public void HelpersInit(string connectionString = "")
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                ConnectionSettings.ConnectionString = connectionString;
            }

            Core.QueryHelper = new QueryHelper();
            ClearQueriesListAsync();
            NewQueriesReady += SetQueryList;

            Core.AnswerHelper = new AnswerHelper();
            Core.AnswerHelper.ClearAnswersListAsync();

            Core.EventHelper = new EventHelper();
            Core.EventHelper.ClearEventsListAsync();
        }

        public Task<bool> CheckQueryAsync(string Id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Eq(Keys.QueryKey, Id);
                QueryInfo result = null;
                try
                {
                    result = Core.QueryHelper.GetCollection.Find(filter).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    AsyncTaskListener.LogMessage(ex.Message);
                    if (Core.QueryHelper.Connect())
                    {
                        CheckQueryAsync(Id);
                    }
                }
                return (result != null);
            });
        }

        public void ReadQueries()
        {
            var filter = Builders<QueryInfo>.Filter.Empty;
            try
            {
                // Select all the queries
                var queries = Core.QueryHelper.GetCollection.Find(filter).ToList();

                if (queries.Count != 0)
                { 
                    if (Program.MiniMonitor != null)
                    {
                        Program.MiniMonitor.SetNumberOfQueriesInLine(queries.Count);
                    }

                    // Process the queries (fire event of this class)
                    NewQueriesReady(queries);
                }

                lock (Core.LogLock)
                {
                    //AsyncTaskListener.LogMessage("************************************************************");
                    //AsyncTaskListener.LogMessage(string.Format("{0} new quer(y/ies) in database at {1}", queries.Count, DateTime.Now));
                    foreach (QueryInfo query in queries)
                    {
                        //AsyncTaskListener.LogMessage(query.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //AsyncTaskListener.LogMessage(ex.Message);
                if (Core.QueryHelper.Connect())
                {
                    ReadQueries();
                }
            }
        }

        public Task ClearQueriesListAsync()
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Empty;
                try
                {
                    Core.QueryHelper.GetCollection.DeleteMany(filter);
                    //AsyncTaskListener.LogMessage("Queries list was cleared successfully");
                }
                catch (Exception ex)
                {
                    //AsyncTaskListener.LogMessage(ex.Message);
                    if (Core.QueryHelper.Connect())
                    {
                        ClearQueriesListAsync();
                    }
                }
            });
        }

        public Task RemoveQueryAsync(string key)
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Eq(Keys.QueryKey, key);
                try
                {
                    Core.QueryHelper.GetCollection.DeleteOne(filter);
                }
                catch (Exception ex)
                {
                    AsyncTaskListener.LogMessage(ex.Message);
                    if (Core.QueryHelper.Connect())
                    {
                        RemoveQueryAsync(key);
                    }
                }
            });
        }

        public void DeleteProcessedQuery(string key)
        {
            var filter = Builders<QueryInfo>.Filter.Eq(Keys.QueryKey, key);
            try
            {
                Core.QueryHelper.GetCollection.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
                if (Core.QueryHelper.Connect())
                {
                    DeleteProcessedQuery(key);
                }
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

        AnswerInfo CreateExceptionAnswer(Exception ex, QueryInfo query)
        {
            var tiex = ex as TargetInvocationException;
            if (tiex != null)
            {
                ex = tiex.InnerException;
            }

            return new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName)
            {
                IsCQGException = true,
                CQGException = new Action(() => { throw ex; })
            };
        }
    }
}
