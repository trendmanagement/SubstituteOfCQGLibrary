using System;
using System.Threading.Tasks;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Helpers
{
    public class AnswerHelper
    {
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

        public AnswerHelper()
        {
            Connect();
        }

        private void Disconnect()
        {
            Client?.DropDatabase(ConnectionSettings.MongoDBName);
        }

        public bool Connect()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionString);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<AnswerInfo>(ConnectionSettings.AnswerCollectionName);
            return Collection != null;
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
                lock (Core.LogLock)
                {
                    Core.OnLogChange("************************************************************");
                    Core.OnLogChange(answer.ToString());
                }
            }
            catch (Exception ex)
            {
                Core.OnLogChange(ex.Message);
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
                    Core.OnLogChange(ex.Message);
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
                Core.OnLogChange(answer.AnswerKey, answer.ValueKey, false);
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
                    Core.OnLogChange(id, "null", false);
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
                    Core.OnLogChange(answer.AnswerKey, answer.ValueKey, false);
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
                    Core.OnLogChange("Answers list was cleared successfully");
                }
                catch (Exception ex)
                {
                    Core.OnLogChange(ex.Message);
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
                    Core.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        RemoveAnswerAsync(key);
                    }
                }
            });
        }
    }
}
