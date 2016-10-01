using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class HandshakingHelper
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<HandshakingModel> _collection;

        public IMongoCollection<HandshakingModel> GetCollection
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

        public HandshakingHelper()
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<HandshakingModel>(ConnectionSettings.HandshakingCollectionName);
        }
    }
}
