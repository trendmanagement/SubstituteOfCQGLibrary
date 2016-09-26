using FakeCQG.Helpers;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG
{
    public class QueryHelper
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<QueryInfo> _collection;

        public IMongoCollection<QueryInfo> GetCollection
        {
            get
            {
                return _collection;
            }
        }

        public IMongoDatabase GetDefaultDB
        {
            get
            {
                return _database;
            }
        }

        public QueryHelper()
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<QueryInfo>(ConnectionSettings.QueryCollectionName);
        }

        public QueryHelper(string collectionName)
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<QueryInfo>(collectionName);
        }
    }
}
