using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class AnswerHelper
    {
        private const string eventFiringId = "!EventHappened";

        protected IMongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<AnswerInfo> Collection;

        public IMongoCollection<AnswerInfo> GetCollection
        {
            get
            {
                return Collection;
            }
        }

        public IMongoDatabase GetDefaultDB
        {
            get
            {
                return Database;
            }
        }

        public string EventFiringId
        {
            get
            {
                return eventFiringId;
            }
        }

        public AnswerHelper()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<AnswerInfo>(ConnectionSettings.AnswerCollectionName);
        }

        public Task PushAnswerAsync(AnswerInfo answer)
        {
            return Task.Run(() => PushAnswer(answer));
        }

        public void PushAnswer(AnswerInfo answer)
        {
            try
            {
                Collection.InsertOne(answer);
                lock (CQG.LogLock)
                {
                    CQG.OnLogChange("************************************************************");
                    CQG.OnLogChange(answer.ToString());
                }
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
            }
        }

        public Task<bool> CheckAnswerAsync(string Id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<AnswerInfo>.Filter.Eq(Keys.IdName, Id);

                AnswerInfo result = null;
                try
                {
                    result = Collection.Find(filter).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }

                return (result != null);
            });
        }

        public AnswerInfo GetAnswerData(string id, out bool isAns)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.IdName, id);
            try
            {
                AnswerInfo answer = Collection.Find(filter).First();
                CQG.OnLogChange(answer.Key, answer.ValueKey, false);
                RemoveAnswerAsync(answer.Key);
                isAns = true;
                //if (answer.Key == "value")
                //{
                //    isVal = true;
                //    return answer.Value;
                //}
                //else
                //{
                //    isVal = false;
                //    return answer;
                //}
                return answer;
            }
            catch (Exception)
            {
                CQG.OnLogChange(id, "null", false);
                isAns = false;
                return null;
            }
        }

        public AnswerInfo GetAnswerData(string id)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.IdName, id);
            AnswerInfo answer = null;
            while (!DataDictionaries.IsAnswer[id])
            {
                try
                {
                    answer = Collection.Find(filter).First();
                    CQG.OnLogChange(answer.Key, answer.ValueKey, false);
                    RemoveAnswerAsync(answer.Key);
                    DataDictionaries.IsAnswer[id] = true;
                }
                catch (Exception)
                {
                    //OnLogChange(id, "null", false);
                    //if (!DataDictionaries.IsAnswer[id])
                    //{
                    //    return GetAnswerData(id);
                    //}
                    //else
                    //{
                    //    return answer;
                    //}

                    ////TODO: Create type of exception for  variant "no answer"
                    //throw new Exception("No answer in MongoDB");
                }
            }

            return answer;
        }

        public Task ClearAnswersListAsync()
        {
            var filter = Builders<AnswerInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteMany(filter);
                    CQG.OnLogChange("Answers list was cleared successfully");
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public Task RemoveAnswerAsync(string key)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.IdName, key);
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteOne(filter);
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public object[] CheckWhetherEventHappened(string name)
        {
            string handlerName = string.Format("_ICQGCELEvents_{0}EventHandler", name);
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.QueryName, handlerName);
            try
            {
                AnswerInfo answer = Collection.Find(filter).First();
                var argValues = answer.ArgValues;
                return (object[])argValues[0];
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
                return null;
            }
        }

        public void CommonEventHandler(string name, object[] args = null)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.QueryName, name);
            try
            {
                //AnswerInfo answer = Collection.Find(filter).First();
                
                var argValues = new Dictionary<int, object>();
                argValues.Add(0, args);

                AnswerInfo answer = new AnswerInfo(eventFiringId, null, name, argValues);
                PushAnswer(answer);
                //var update = Builders<AnswerInfo>.Update.Set(Keys.ArgValues, argValues);

                //TODO: deserialize argValues from dictionary to bson
                //allAnswers.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
            }
        }
    }
}
