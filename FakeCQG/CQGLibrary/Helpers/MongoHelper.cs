using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class MongoHelper
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<HandshakerModel> _collection;

        public IMongoCollection<HandshakerModel> GetCollection
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

        public MongoHelper()
        {
            _client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collection = _database.GetCollection<HandshakerModel>(ConnectionSettings.HandshakingCollectionName);
        }
    }
}
