using System;
using System.Collections;
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
    partial class QueryHandler
    {
        CQGDataManagement CqgDataManagement;

        Assembly CQGAssm;

        object QueriesProcessingLock = new object();

        public static Hashtable UsedObjs;

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

        #region Ctors
        public QueryHandler(CQGDataManagement cqgDM)
        {
            CqgDataManagement = cqgDM;
            UsedObjs = new Hashtable();
            QueryList = new List<QueryInfo>();
            CQGAssm = cqgDM.CQGAssm;
        }

        internal QueryHandler(CQGDataManagement cqgDM, IList<QueryInfo> ql)
        {
            CqgDataManagement = cqgDM;
            UsedObjs = new Hashtable();
            QueryList = new List<QueryInfo>(ql);
            CQGAssm = cqgDM.CQGAssm;
        }
        #endregion

        // Main query processing method
        public void ProcessQuery(QueryInfo query)
        {
            // Object where the data obtained after query execution is placed
            // This object will be sent to the DB
            AnswerInfo answer;

            // Get a name of a symbol if it's CQGTimedBarsRequestClass object's request
            // and show it in MiniMonitor form
            if (query.ObjectType == "CQGTimedBarsRequestClass")
            {
                object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                string instrName = (string)qObj.GetType().InvokeMember("Symbol",
                    BindingFlags.GetProperty, null, qObj, null);
                if (!DCMiniMonitor.symbolsList.Contains(instrName))
                {
                    DCMiniMonitor.symbolsList.Add(instrName);
                    Program.MiniMonitor.SymbolsListsUpdate();
                }        
            }

            // Handling of the request depending on its kind
            switch (query.QueryType)
            {
                case QueryType.CallCtor:
                    {
                        try
                        {
                            string key;

                            // Handling of the ctor calling request and creation of an object depending on its type
                            switch (query.MemberName)
                            {
                                // Handling of CQGCEL ctor calling request
                                case "CQG.CQGCELClass":
                                    key = CqgDataManagement.CEL_key;
                                    break;

                                // Common case
                                default:
                                    object[] args = Core.GetArgsIntoArrayFromTwoDicts(query.ArgKeys, query.ArgValues);
                                    object qObj = CQGAssm.CreateInstance(query.MemberName, false,
                                        BindingFlags.CreateInstance, null, args, null, null);
                                    key = Core.CreateUniqueKey();

                                    ServerDictionaries.PutObjectToTheDictionary(key, qObj);
                                    UsedObjs.Add(key, qObj);
                                    break;
                            }

                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: key);
                            PushAnswerAndDeleteQuery(answer);
                        }
                        catch 
                        {
                            try
                            {
                                AutoGenQueryProcessing(query);
                            }
                            catch (Exception ex)
                            {
                                answer = CreateExceptionAnswer(ex, query);
                                PushAnswerAndDeleteQuery(answer);
                            }                           
                        }   
                    }
                    break;

                case QueryType.CallDtor:
                    {
                        if (query.ObjectKey != CqgDataManagement.CEL_key)
                        {
                            UsedObjs.Remove(query.ObjectKey);
                            ServerDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);
                        }

                        // Remove name of a symbol if it's CQG.CQGTimedBarsRequest object deleting
                        // from MiniMonitor form
                        if (query.MemberName == "CQG.CQGTimedBarsRequest")
                        {
                            object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                            string symbName = (string)qObj.GetType().InvokeMember("Symbol",
                                BindingFlags.GetProperty, null, qObj, null);
                            DCMiniMonitor.symbolsList.Remove(symbName);
                            Program.MiniMonitor.SymbolsListsUpdate();
                        }

                        answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);

                        PushAnswerAndDeleteQuery(answer);
                    }
                    break;

                case QueryType.GetProperty:
                    {
                        object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = Core.GetArgsIntoArrayFromTwoDicts(query.ArgKeys, query.ArgValues);

                        try
                        {
                            // Getting of property value
                            var propV = qObj.GetType().InvokeMember(query.MemberName, BindingFlags.GetProperty, null, qObj, args);

                            // Checking type of property value and returning value or value key 
                            // (second, if it's not able to be transmitted through the database)
                            answer = Core.IsSerializableType(propV.GetType()) ?
                                CreateValAnswer(query.QueryKey, query.ObjectKey, query.MemberName, propV) :
                                CreateKeyAnswer(query.QueryKey, query.ObjectKey, query.MemberName, propV);

                            PushAnswerAndDeleteQuery(answer);
                        }
                        catch
                        {
                            try
                            {
                                AutoGenQueryProcessing(query);
                            }
                            catch (Exception ex)
                            {
                                answer = CreateExceptionAnswer(ex, query);
                                PushAnswerAndDeleteQuery(answer);
                            }
                        }                       
                    }
                    break;

                case QueryType.SetProperty:
                    {
                        object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = Core.GetArgsIntoArrayFromTwoDicts(query.ArgKeys, query.ArgValues);

                        if(string.Concat(qObj.GetType()) == "CQG.CQGTimedBarsRequest" && query.MemberName == "Symbol")
                        {
                            DCMiniMonitor.symbolsList.Add(string.Concat(args[0]));
                            Program.MiniMonitor.SymbolsListsUpdate();
                        }

                        try
                        {
                            // Setting of property value
                            qObj.GetType().InvokeMember(query.MemberName, BindingFlags.SetProperty, null, qObj, args);
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true);

                            PushAnswerAndDeleteQuery(answer);
                        }
                        catch
                        {
                            try
                            {
                                AutoGenQueryProcessing(query);
                            }
                            catch (Exception ex)
                            {
                                answer = CreateExceptionAnswer(ex, query);
                                PushAnswerAndDeleteQuery(answer);
                            }
                        }     
                    }
                    break;

                case QueryType.CallMethod:
                    {
                        // Handling of Shutdown method calling by CQGCEL object. This method has not be called from client applications
                        if (query.MemberName == "Shutdown")
                        {
                            var returnKey = "true";
                            answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey);
                            PushAnswerAndDeleteQuery(answer);
                            break;
                        }

                        object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = Core.GetArgsIntoArrayFromTwoDicts(query.ArgKeys, query.ArgValues);

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
                                // Handling method call request depending of return value type
                                answer = Core.IsSerializableType(returnV.GetType()) ? 
                                    CreateValAnswer(query.QueryKey, query.ObjectKey, query.MemberName, returnV) :
                                    CreateKeyAnswer(query.QueryKey, query.ObjectKey, query.MemberName, returnV);
                            }
                            else
                            {
                                // Handling void method call
                                var returnKey = "true";
                                answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey);
                            }

                            PushAnswerAndDeleteQuery(answer);
                        }
                        catch
                        {
                            try
                            {
                                AutoGenQueryProcessing(query);
                            }
                            catch (Exception ex)
                            {
                                answer = CreateExceptionAnswer(ex, query);
                                PushAnswerAndDeleteQuery(answer);
                            }
                        }   
                    }
                    break;

                case QueryType.SubscribeToEvent:
                case QueryType.UnsubscribeFromEvent:
                    {
                        object qObj = UsedObjs.Contains(query.ObjectKey) ? UsedObjs[query.ObjectKey] : ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        try
                        {
                            System.Reflection.EventInfo ei = qObj.GetType().GetEvent(query.MemberName);

                            if(EventHandler.EventAppsSubscribersNum.ContainsKey(query.MemberName))
                            {
                                EventHandler.EventAppsSubscribersNum[query.MemberName] =
                                    query.QueryType == QueryType.SubscribeToEvent ?
                                    EventHandler.EventAppsSubscribersNum[query.MemberName] + 1 :
                                    EventHandler.EventAppsSubscribersNum[query.MemberName] - 1;
                            }
                            else
                            {
                                EventHandler.EventAppsSubscribersNum.Add(query.MemberName, 1);
                            }

                            // Find corresponding CQG delegate
                            Type delType = FindDelegateType(CQGAssm, query.MemberName);

                            // Instantiate the delegate with our own handler
                            var eventHandlersMethods = typeof(CQGEventHandlers).GetMethods();
                            MethodInfo handlerInfo = null;
                            
                            for(int i = 0; i < eventHandlersMethods.Length; i++)
                            {
                                if(eventHandlersMethods[i].Name.Contains(query.MemberName))
                                {
                                    handlerInfo = eventHandlersMethods[i];
                                }
                            }

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
                        }
                        catch
                        {
                            try
                            {
                                AutoGenQueryProcessing(query);
                            }
                            catch (Exception ex)
                            {
                                answer = CreateExceptionAnswer(ex, query);
                                PushAnswerAndDeleteQuery(answer);
                            }
                        }  
                        
                        if (query.QueryType == QueryType.SubscribeToEvent &&
                            query.MemberName == "DataConnectionStatusChanged" )
                        {
                            // Fire this event explicitly, because data collector connects to real CQG beforehand and does not fire it anymore
                            CQGEventHandlers._ICQGCELEvents_DataConnectionStatusChangedEventHandlerImpl(CqgDataManagement.currConnStat);
                        }
                    }
                    break;

                default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
            }
        }

        #region Helper methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetQueryList(List<QueryInfo> queries)
        {
            QueryList = queries;
        }

        // Initialization of databases access helpers
        public void HelpersInit(string connectionString = "")
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                ConnectionSettings.ConnectionString = connectionString;
            }

            Core.QueryHelper = new QueryHelper();
            ClearQueriesList();
            NewQueriesReady += SetQueryList;

            Core.AnswerHelper = new AnswerHelper();
            Core.AnswerHelper.ClearAnswersListAsync();

            Core.EventHelper = new EventHelper();
            EventHandler.ClearEventsListAsync();
        }

        public void ProcessEntireQueryList()
        {
            lock (QueriesProcessingLock)
            {
                for(int i = 0; i < QueryList.Count; i++)
                {
                    ProcessQuery(QueryList[i]);
                }
                QueryList.Clear();
            }
        }

        // Instruments of checking queries collection in DB and 
        // reading information from queries collection
        #region Check and read info about queries from DB
        // Checking of queries in DB and reading them if exist
        public void ReadQueries(bool isLog = true)
        {
            var filter = Builders<QueryInfo>.Filter.Empty;
            try
            {
                // Select all the queries
                var queries = Core.QueryHelper.GetCollection.Find(filter).ToList();

                if (queries.Count != 0)
                {
                    // Process the queries (fire event of this class)
                    NewQueriesReady(queries);
                }

                if(isLog)
                {
                    lock (Core.LogLock)
                    {
                        AsyncTaskListener.LogMessage(string.Concat(queries.Count, " new quer(y/ies) in database"));
                        for (int i = 0; i < queries.Count; i++)
                        {
                            AsyncTaskListener.LogMessage(queries[i].ToString());
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
                if (Core.QueryHelper.Connect())
                {
                    ReadQueries();
                }
            }
        }

        /// <summary>
        /// Task for checking queries by id
        /// </summary>
        /// <param name="Id">Queries id </param>
        /// <returns>Returns success of operation</returns>
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
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal AnswerInfo CreateValAnswer(string qKey, string objKey, string memberName, object val)
        {
            var returnKey = "value";
            return new AnswerInfo(qKey, objKey, memberName, valueKey: returnKey, value: val);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal AnswerInfo CreateKeyAnswer(string qKey, string objKey, string memberName, object val)
        {
            var returnKey = Core.CreateUniqueKey();
            ServerDictionaries.PutObjectToTheDictionary(returnKey, val);
            return new AnswerInfo(qKey, objKey, memberName, valueKey: returnKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void PushAnswerAndDeleteQuery(AnswerInfo answer)
        {
            AnswerHandler.PushAnswer(answer);
            DeleteProcessedQuery(answer.AnswerKey);
        }

        // Instruments of cleaning information about queries from DB
        #region Cleaning
        public void ClearQueriesList()
        {
            var filter = Builders<QueryInfo>.Filter.Empty;
            try
            {
                Core.QueryHelper.GetCollection.DeleteMany(filter);
                AsyncTaskListener.LogMessage("Queries list was cleared successfully");
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
                if (Core.QueryHelper.Connect())
                {
                    ClearQueriesList();
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
                    AsyncTaskListener.LogMessage("Queries list was cleared successfully");
                }
                catch (Exception ex)
                {
                    AsyncTaskListener.LogMessage(ex.Message);
                    if (Core.QueryHelper.Connect())
                    {
                        ClearQueriesListAsync();
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
        #endregion

        internal static Type FindDelegateType(Assembly assm, string eventName)
        {
            string delegateTypeName = string.Concat("_ICQGCELEvents_", eventName, "EventHandler");
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsDelegate(Type type)
        {
            return type.BaseType == typeof(MulticastDelegate);
        }

        /// <summary>
        /// Creating answer method with exception info
        /// </summary>
        /// <param name="ex">CQG exception</param>
        /// <param name="query">Query with data which threw exception</param>
        /// <returns>Answer with exception info</returns>
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
                CQGException = new ExceptionInfo()
                {
                    Message = ex.InnerException == null ? ex.Message : ex.InnerException.Message,
                    Source = ex.Source,
                }
            };
        }
        #endregion

    }
}
