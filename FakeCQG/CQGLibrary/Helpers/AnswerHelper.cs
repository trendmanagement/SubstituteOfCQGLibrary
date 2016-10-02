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
            Connect();
        }

        ~AnswerHelper()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            Client.DropDatabase(ConnectionSettings.MongoDBName);
        }

        public bool Connect()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<AnswerInfo>(ConnectionSettings.QueryCollectionName);
            return (Collection != null) ? false : true;
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
                if (Connect())
                {
                    PushAnswer(answer);
                }
            }
        }

        public Task<bool> CheckAnswerAsync(string Id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<AnswerInfo>.Filter.Eq(Keys.AnswerKey, Id);

                AnswerInfo result = null;
                try
                {
                    result = Collection.Find(filter).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        CheckAnswerAsync(Id);
                    }
                }

                return (result != null);
            });
        }

        public AnswerInfo GetAnswerData(string id, out bool isAns)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.AnswerKey, id);
            try
            {
                AnswerInfo answer = Collection.Find(filter).First();
                CQG.OnLogChange(answer.AnswerKey, answer.ValueKey, false);
                RemoveAnswerAsync(answer.AnswerKey);
                isAns = true;
                return answer;
            }
            catch (Exception)
            {
                if (Connect())
                {
                    return GetAnswerData(id, out isAns);
                }
                else
                {
                    CQG.OnLogChange(id, "null", false);
                    isAns = false;
                    return null;
                }
            }
        }

        public AnswerInfo GetAnswerData(string id)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.AnswerKey, id);
            AnswerInfo answer = null;
            while (!ClientDictionaries.IsAnswer[id])
            {
                try
                {
                    answer = Collection.Find(filter).First();
                    CQG.OnLogChange(answer.AnswerKey, answer.ValueKey, false);
                    RemoveAnswerAsync(answer.AnswerKey);
                    ClientDictionaries.IsAnswer[id] = true;
                }
                catch (Exception)
                {
                    if (Connect())
                    {
                        return GetAnswerData(id);
                    }
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
                    if (Connect())
                    {
                        ClearAnswersListAsync();
                    }
                }
            });
        }

        public Task RemoveAnswerAsync(string key)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.AnswerKey, key);
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteOne(filter);
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        RemoveAnswerAsync(key);
                    }
                }
            });
        }

        public object[] CheckWhetherEventHappened(string name)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.QueryName, name);
            try
            {
                AnswerInfo answer = Collection.Find(filter).First();
                var argValues = answer.ArgValues;
                return (object[])argValues[1];
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
                if (Connect())
                {
                    return CheckWhetherEventHappened(name);
                }
                else
                {
                    return null;
                }
            }
        }

        public void CommonEventHandler(string name, object[] args = null)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.QueryName, name);
            try
            {
                AnswerInfo answer = Collection.Find(filter).First();
                var argValues = new Dictionary<int, object>();
                argValues.Add(0, "!");
                argValues.Add(1, args);
                var update = Builders<AnswerInfo>.Update.Set(Keys.ArgValues, argValues);
                //TODO: deserialize argValues from dictionary to bson
                //allAnswers.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
                if (Connect())
                {
                    CommonEventHandler(name, args);
                }
            }
        }
    }
}
