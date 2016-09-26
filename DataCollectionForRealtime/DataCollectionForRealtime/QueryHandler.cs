using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using FakeCQG;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

[assembly: InternalsVisibleTo("UnitTestRealCQG")]

namespace DataCollectionForRealtime
{
    class QueryHandler
    {
        CQGDataManagement cqgDataManagement;

        Assembly CQGAssm;

        public static List<QueryInfo> QueryList;

        public Assembly CQGAssembly
        {
            get { return CQGAssm; }
        }

        public QueryHandler(RealtimeDataManagement rdm)
        {
            cqgDataManagement = new CQGDataManagement(rdm);
            QueryList = new List<QueryInfo>();
            LoadCQGAssembly();
        }

        public QueryHandler(CQGDataManagement cqgDM)
        {
            cqgDataManagement = cqgDM;
            QueryList = new List<QueryInfo>();
            LoadCQGAssembly();
        }

        public QueryHandler(RealtimeDataManagement rdm, IList<QueryInfo> ql)
        {
            cqgDataManagement = new CQGDataManagement(rdm);
            QueryList = new List<QueryInfo>(ql);
            LoadCQGAssembly();
        }

        public QueryHandler(CQGDataManagement cqgDM, IList<QueryInfo> ql)
        {
            cqgDataManagement = cqgDM;
            QueryList = new List<QueryInfo>(ql);
            LoadCQGAssembly();
        }

        public void LoadCQGAssembly()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assmPath = Path.Combine(path, "Interop.CQG.dll");
            CQGAssm = Assembly.LoadFile(assmPath);
        }

        public void SetQueryList(List<QueryInfo> queries)
        {
            QueryList = queries;
        }

        public void CheckRequestsQueue()
        {
            //ManualResetEvent _event = new ManualResetEvent(false);
            //ThreadPool.QueueUserWorkItem(o => QueryList = CQGLibrary.CQG.ReadQueries());
            //Task.Run(() =>
            //{
                //QueryList = CQGLibrary.CQG.ReadQueries();
            //});
            FakeCQG.CQG.ReadQueriesAsync();
        }

        public bool QueryProcessing(QueryInfo query)
        {
            bool isSuccessful = false;

            //Object where the data obtained after query execution is placed
            //This object will be sent to the DB
            AnswerInfo answer;

            //Handling of a request depending on its kind
            switch (query.TypeOfQuery)
            {
                case QueryInfo.QueryType.Ctor:
                    {
                        string key;

                        switch (query.QueryName)
                        {
                            case "CQG.CQGCELClass":
                                key = cqgDataManagement.CEL_key;
                                break;
                            default:
                                //TODO: Make sure, that correct name of real CQG assembly passed to the CreateInstance method as arg below
                                object qObj = CQGAssm.CreateInstance(query.QueryName);
                                key = FakeCQG.CQG.CreateUniqueKey();
                                DataDictionaries.PutObjectToTheDictionary(key, qObj);
                                break;
                        }

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: key);
                        PushAnswer(answer);
                    }
                case QueryInfo.QueryType.Destructor:
                    DataDictionaries.RemoveObject(query.ObjectKey);
                    break;
                case QueryInfo.QueryType.Constructor:
                    
                    //switch (query.QueryName)
                    //{
                    //    case "CQG.CQGCELClass":
                    //        qObj = cqgDataManagement.M_CEL;
                    //        break;
                    //    default:
                    //        //TODO: Make sure, that correct name of real CQG assembly passed to the CreateInstance method as arg below
                    //        qObj = CQGAssm.CreateInstance(query.QueryName);
                    //        break;
                    //}
                    qObj = CQGAssm.CreateInstance(query.QueryName);
                   
                    DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                    answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);
                    LoadInAnswer(answer);
                    break;

                case QueryInfo.QueryType.Dtor:
                    {
                        DataDictionaries.RemoveObjectFromTheDictionary(query.ObjectKey);
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
                        
                        PushAnswer(answer);
                    }
                    break;

                case QueryInfo.QueryType.SetProperty:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);

                        object[] args = ParseInputArgsFromQuery(query);

                        qObj.GetType().InvokeMember(query.QueryName, BindingFlags.SetProperty, null, qObj, args);

                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);

                        PushAnswer(answer);
                    }
                    break;

                case QueryInfo.QueryType.Method:
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
                            isSuccessful = true;
                        }
                        else
                        {
                            var returnKey = "true";
                            answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey);
                            isSuccessful = true;
                        }

                        DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                        PushAnswer(answer);
                    }
                    break;

                case QueryInfo.QueryType.Event:
                    {
                        object qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                        EventInfo ei = qObj.GetType().GetEvent(query.QueryName);

                        if ((string)query.ArgValues["0"] != "+" || (string)query.ArgValues["0"] != " - ")
                        {
                            AsyncTaskListener.LogMessage("Argument that describes event subtype is not valid.");
                            break;
                        }

                        // Find corresponding CQG delegate
                        Type delType = FindDelegateType(CQGAssm, query.QueryName);

                        // Instantiate the delegate with our own handler
                        string handlerName = string.Format("_ICQGCELEvents_{0}EventHandlerImpl", query.QueryName);

                        MethodInfo handlerInfo = typeof(CQGEventHandlers).GetMethod(handlerName);
                        Delegate d = Delegate.CreateDelegate(delType, handlerInfo);

                        if ((string)query.ArgValues["0"] == " + ")
                        {
                            // Subscribe our handler to CQG event
                            ei.AddEventHandler(qObj, d);
                            //qObj.GetType().InvokeMember("add_" + query.QueryName, BindingFlags.InvokeMethod, null, qObj, new object[] { d });
                        }
                        else if ((string)query.ArgValues["0"] == " - ")
                        {
                            // Unsubscribe our handler to CQG event
                            ei.RemoveEventHandler(qObj, d);
                            //qObj.GetType().InvokeMember("remove_" + query.QueryName, BindingFlags.InvokeMethod, null, qObj, new object[] { d });
                        }

                        DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);
                        PushAnswer(answer);
                    }
                    break;
            }
 
            return isSuccessful;
        }

        public void ProcessEntireQueryList()
        {
            foreach (QueryInfo temp in QueryList)
            {
                QueryProcessing(temp);
            }
        }

        public void PushAnswer(AnswerInfo answer)
        {
            try
            {
                AnswerHelper mongoA = new AnswerHelper();

                var collectionA = mongoA.GetCollection;
                collectionA.InsertOne(answer);
                AsyncTaskListener.LogMessageFormat(
                    "The following answer to query of type \"{0}\" was pushed into the MongoDB:" + Environment.NewLine +
                    "[Value: \"{1}\", Key: \"{2}\"]",
                    answer.QueryName,
                    answer.Value,
                    answer.ValueKey);

                DeleteProcessedQuery(answer.Key);
            }
            catch (Exception exc)
            {
                AsyncTaskListener.LogMessage(exc.Message);
            }
        }

        public void DeleteProcessedQuery(string key)
        {
            QueryHelper mongoQ = new QueryHelper();

            var collectionQ = mongoQ.GetCollection;
            var filter = Builders<QueryInfo>.Filter.Eq("Key", key);
            try
            {
                collectionQ.DeleteOneAsync(filter);
                AsyncTaskListener.LogMessage("Query was deleted successfully.");
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
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
