using CQGLibrary.Models;
using FakeCQG.Helpers;
using MongoDB.Driver;

namespace CQGLibrary.Helpers
{
    public class MongoHelper
    {
        protected IMongoClient _client;
        protected IMongoDatabase _database;
        protected IMongoCollection<HandShakerModel> _collection;

        public IMongoCollection<HandShakerModel> GetCollection
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
            _collection = _database.GetCollection<HandShakerModel>(ConnectionSettings.HandShakingCollectionName);
        }

    }
}
