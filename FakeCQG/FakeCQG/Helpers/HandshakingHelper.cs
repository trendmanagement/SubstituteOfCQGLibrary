using System;
using System.Threading.Tasks;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Helpers
{
    public class HandshakingHelper
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        protected static IMongoCollection<HandshakingInfo> _collectionSubscribers;
        protected static IMongoCollection<HandshakingInfo> _collectionUnsubscribers;

        public IMongoCollection<HandshakingInfo> GetCollectionSubscribers
        {
            get
            {
                return _collectionSubscribers;
            }
        }

        public IMongoCollection<HandshakingInfo> GetCollectionUnsubscribers
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
            _collectionSubscribers = _database.GetCollection<HandshakingInfo>(ConnectionSettings.HandshakingCollectionName);
            _collectionUnsubscribers = _database.GetCollection<HandshakingInfo>(ConnectionSettings.UnsubscribeHandshakingCollectionName);
            return _collectionSubscribers != null;
        }

        public Task ClearHandShackingListAsync()
        {
            var filter = Builders<HandshakingInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    _collectionSubscribers.DeleteMany(filter);
                    Core.OnLogChange("Handshacking list was cleared successfully");
                }
                catch (Exception ex)
                {
                    Core.OnLogChange(ex.Message);
                    if (Connect())
                    {
                        ClearHandShackingListAsync();
                    }
                }
            });
        }
    }
}
