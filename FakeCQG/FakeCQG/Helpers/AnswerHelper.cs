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

        // Recursive checking DB for a ready answer while connection exists.
        // Getting object with answer data if there's coincidence by query id.
        public AnswerInfo GetAnswerData(string id)
        {
            var filter = Builders<AnswerInfo>.Filter.Eq(Keys.AnswerKey, id);
            AnswerInfo answer = null;
            while (!ClientDictionaries.IsAnswer[id])
            {
                try
                {
                    //Get a selection by filter
                    var collection = Collection.Find(filter);
                    //If we have any item in selection answer proceed else repeat getting selection
                    if (collection.Any())
                    {
                        answer = collection.First();
                        RemoveAnswerAsync(answer.AnswerKey);
                        //Set value that answer is got
                        ClientDictionaries.IsAnswer[id] = true;
                        Core.OnLogChange(answer.AnswerKey, answer.ValueKey, false);
                    }
                }
                catch (Exception)
                {
                    if (Core.isDCClosed)
                    {
                        break;
                    }
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
