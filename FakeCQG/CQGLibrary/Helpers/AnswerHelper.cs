using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class AnswerHelper
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<AnswerInfo> _collection;

        public IMongoCollection<AnswerInfo> GetCollection
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

        public AnswerHelper()
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<AnswerInfo>(ConnectionSettings.AnswerCollectionName);
        }

        public AnswerHelper(string collectionName)
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<AnswerInfo>(collectionName);
        }
    }
}
