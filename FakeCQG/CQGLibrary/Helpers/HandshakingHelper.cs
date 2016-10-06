using FakeCQG.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FakeCQG.Helpers
{
    public class HandshakingHelper
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected static IMongoCollection<HandshakingModel> _collectionSubscribers;
        protected static IMongoCollection<HandshakingModel> _collectionUnsubscribers;

        public IMongoCollection<HandshakingModel> GetCollectionSubscribers
        {
            get
            {
                return _collectionSubscribers;
            }
        }

        public IMongoCollection<HandshakingModel> GetCollectionUnsubscribers
        {
            get
            {
                return _collectionUnsubscribers;
            }
        }


        public IMongoDatabase GetDefaultDB
        {
            get
            {
                return _database;
            }
        }

        static HandshakingHelper()
        {
            Connect();
        }

        static bool Connect()
        {
            _client = new MongoClient(ConnectionSettings.ConnectionString);
            _database = _client.GetDatabase(ConnectionSettings.MongoDBName);
            _collectionSubscribers = _database.GetCollection<HandshakingModel>(ConnectionSettings.HandshakingCollectionName);
            _collectionUnsubscribers = _database.GetCollection<HandshakingModel>(ConnectionSettings.UnsubscribeHandshakingCollectionName);
            return _collectionSubscribers != null;
        }


        public Task ClearHandShackingListAsync()
        {
            var filter = Builders<HandshakingModel>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    _collectionSubscribers.DeleteMany(filter);
                    CQG.OnLogChange("Handshacking list was cleared successfully");
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        ClearHandShackingListAsync();
                    }
                }
            });
        }

    }
}
