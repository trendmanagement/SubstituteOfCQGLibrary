using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Helpers
{
    public class QueryHelper
    {
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
            Collection = Database.GetCollection<QueryInfo>(ConnectionSettings.QueryCollectionName);
            return Collection != null;
        }

        public void PushQuery(QueryInfo query)
        {
            try
            {
                Collection.InsertOne(query);
                Core.OnLogChange(query.QueryKey, query.MemberName, true);
            }
            catch (Exception ex)
            {
                Core.OnLogChange(ex.Message);
                if (Connect())
                {
                    PushQuery(query);
                }
            }
        }

        public Task PushQueryAsync(QueryInfo query)
        {
            return Task.Run(() =>
            {
                try
                {
                    Collection.InsertOne(query);
                    Core.OnLogChange(query.QueryKey, query.MemberName, true);
                }
                catch (Exception ex)
                {
                    Core.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        PushQueryAsync(query);
                    }
                }
            });
        }
      
    }
}
