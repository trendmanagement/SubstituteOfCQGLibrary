using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class QueryHelper
    {
        public delegate void NewQueriesReadyHandler(List<QueryInfo> queries);
        public event NewQueriesReadyHandler NewQueriesReady;

        protected IMongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<QueryInfo> Collection;

        public IMongoCollection<QueryInfo> GetCollection
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

        public QueryHelper()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<QueryInfo>(ConnectionSettings.QueryCollectionName);
        }

        public Task PushQueryAsync(QueryInfo query)
        {
            return Task.Run(() =>
            {
                try
                {
                    Collection.InsertOne(query);
                    CQG.OnLogChange(query.QueryKey, query.MemberName, true);
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public Task<bool> CheckQueryAsync(string Id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Eq(Keys.QueryKey, Id);
                QueryInfo result = null;
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

        public Task ReadQueriesAsync(HashSet<string> keysOfQueriesInProcess)
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Empty;
                try
                {
                    // Select only the queries that are not being processed
                    var queries = Collection.Find(filter).ToEnumerable().Where(query => !keysOfQueriesInProcess.Contains(query.QueryKey)).ToList();

                    if (queries.Count != 0)
                    {
                        // Mark them as being processed
                        keysOfQueriesInProcess.UnionWith(queries.Select(query => query.QueryKey));

                        // Fire the event
                        NewQueriesReady(queries);
                    }

                    lock (CQG.LogLock)
                    {
                        CQG.OnLogChange("************************************************************");
                        CQG.OnLogChange(string.Format("{0} new quer(y/ies) in database at {1}", queries.Count, DateTime.Now));
                        foreach (QueryInfo query in queries)
                        {
                            CQG.OnLogChange(query.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public Task ClearQueriesListAsync()
        {
            return Task.Run(() =>
            {
                var filter = Builders<QueryInfo>.Filter.Empty;
                try
                {
                    Collection.DeleteMany(filter);
                    CQG.OnLogChange("Queries list was cleared successfully");
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
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
                    Collection.DeleteOne(filter);
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public void DeleteProcessedQuery(string key)
        {
            var filter = Builders<QueryInfo>.Filter.Eq(Keys.QueryKey, key);
            try
            {
                Collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
            }
        }
    }
}
