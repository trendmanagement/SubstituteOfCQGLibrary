using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FakeCQG;
using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace DataCollectionForRealtime
{
    public class QueryHandler
    {
        private CQGDataManagement cqgDataManagement;

        public static List<QueryInfo> QueryList;

        Assembly CQGAssm;

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

            //Object from real CQG used below for the query execution
            object qObj;

            //Object where the data obtained after query execution is placed
            //This object will be sent to the DB
            AnswerInfo answer;

            //Handling of a request depending on its kind
            switch (query.TypeOfQuery)
            {
                case QueryInfo.QueryType.Constructor:
                    
                    switch (query.QueryName)
                    {
                        case "CQG.CQGCELClass":
                            qObj = cqgDataManagement.M_CEL;
                            break;
                        default:
                            //TODO: Make sure, that correct name of real CQG assembly passed to the CreateInstance method as arg below
                            qObj = CQGAssm.CreateInstance(query.QueryName);
                            break;
                    }
                    
                    DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                    answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);
                    LoadInAnswer(answer);
                    break;

                case QueryInfo.QueryType.Property:
                    qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                    var value = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.GetProperty, null, qObj, null);

                    try
                    {
                        if (query.ArgValues.Count != 0 || query.ArgKeys != null)
                        {
                            var val = default(object);

                            //If there is an arg - it must be only the one!
                            if (query.ArgKeys != null)
                            {
                                val = DataDictionaries.GetAnswerFromTheDictionary(query.ArgKeys[0]);
                            }
                            else if (query.ArgValues != null)
                            {
                                val = query.ArgValues[0];
                            }
                            else
                            {
                                //TODO: To provide correct handling of all exception cases
                                throw new Exception();
                            }

                            qObj.GetType().InvokeMember(query.QueryName, BindingFlags.SetProperty, null, qObj, new object[] { val });
                            answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);
                        }
                        else
                        {
                            //TODO: Ensure that type of variable and its value will be correct
                            var propV = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.GetProperty, null, qObj, null);
                            if (propV.GetType().Assembly.FullName.Substring(0, 8) == "mscorlib" || propV.GetType().IsEnum)
                            {
                                var answerKey = "value";
                                answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: answerKey, val: propV);
                            }
                            else
                            {
                                var answerKey = Guid.NewGuid().ToString("D");
                                DataDictionaries.PutAnswerToTheDictionary(answerKey, propV);
                                answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: answerKey);
                            }
                            LoadInAnswer(answer);
                        }
                    }
                    catch (Exception ex)
                    {
                        AsyncTaskListener.LogMessage(ex.Message);
                    }

                    DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                    break;

                case QueryInfo.QueryType.Method:
                    qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                    //MethodInfo mi = qObj.GetType().GetMethod(query.QueryName);

                    int numOfArgs = query.ArgKeys.Count + query.ArgValues.Count;           

                    //TODO: Make sure, that correct type of object member passed to the CreateInstance method as arg below
                    object retunV = Activator.CreateInstance(qObj.GetType().GetMember(query.QueryName).GetType());
                    
                    if (numOfArgs != 0)
                    {
                        object[] methodArgs = new object[numOfArgs];

                        if (query.ArgKeys != null)
                        {
                            foreach (KeyValuePair<int, string> argPair in query.ArgKeys)
                            {
                                methodArgs[argPair.Key] = DataDictionaries.GetAnswerFromTheDictionary(argPair.Value);
                            }
                        }

                        if (query.ArgValues != null)
                        {
                            foreach (KeyValuePair<int, object> argPair in query.ArgValues)
                            {
                                methodArgs[argPair.Key] = argPair.Value;
                            }
                        }

                        //TODO: Ensure that type of variable and its value will be correct
                        retunV = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.InvokeMethod, null, qObj, methodArgs);
                    }
                    else
                    {
                        //TODO: Ensure that type of variable and its value will be correct
                        retunV = qObj.GetType().InvokeMember(query.QueryName, BindingFlags.InvokeMethod, null, qObj, null);
                    }
                    
 
                    if (retunV.GetType().Assembly.FullName.Substring(0, 8) != "mscorlib")
                    {
                        var returnKey = Guid.NewGuid().ToString("D");
                        DataDictionaries.PutAnswerToTheDictionary(returnKey, retunV);
                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey);
                        isSuccessful = true;
                    }
                    else
                    {
                        var returnKey = "value";
                        answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, vKey: returnKey, val: retunV);
                        isSuccessful = true;
                    }

                    DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                    LoadInAnswer(answer);
                    break;
                case QueryInfo.QueryType.Event:
                    qObj = DataDictionaries.GetObjectFromTheDictionary(query.ObjectKey);
                    EventInfo ei = qObj.GetType().GetEvent(query.QueryName);

                    if (query.ArgValues[0].ToString() != "+" || query.ArgValues[0].ToString() != "-")
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

                    if (query.ArgValues[0] == "+")
                    {
                        // Subscribe our handler to CQG event
                        ei.AddEventHandler(qObj, d);
                        //qObj.GetType().InvokeMember("add_" + query.QueryName, BindingFlags.InvokeMethod, null, qObj, new object[] { d });
                    }
                    else if (query.ArgValues[0] == "-")
                    {
                        // Unsubscribe our handler to CQG event
                        ei.RemoveEventHandler(qObj, d);
                        //qObj.GetType().InvokeMember("remove_" + query.QueryName, BindingFlags.InvokeMethod, null, qObj, new object[] { d });
                    }

                    DataDictionaries.PutObjectToTheDictionary(query.ObjectKey, qObj);
                    answer = new AnswerInfo(query.Key, query.ObjectKey, query.QueryName, val: true);
                    LoadInAnswer(answer);
                    break;
            }
 
            return isSuccessful;
        }

        public void ProcessEntireQueryList()
        {
            foreach (var temp in QueryList)
            {
                QueryProcessing(temp);
            }
        }

        public void LoadInAnswer(AnswerInfo answer)
        {
            try
            {
                AnswerHelper mongoA = new AnswerHelper();

                var collectionA = mongoA.GetCollection;
                collectionA.InsertOne(answer);
                AsyncTaskListener.LogMessage(string.Format("Answer [\"{0}\",\"{1}\"] to query \"{2}\" has been pushed into the MongoDB",
                    answer.Value, answer.ValueKey, answer.QueryName));

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
                collectionQ.DeleteOne(filter);
                AsyncTaskListener.LogMessage("Query was successfully deleted.");
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
            }
        }

        public static Type FindDelegateType(Assembly assm, string eventName)
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

        public static bool IsDelegate(Type type)
        {
            return type.BaseType == typeof(MulticastDelegate);
        }
    }
}
