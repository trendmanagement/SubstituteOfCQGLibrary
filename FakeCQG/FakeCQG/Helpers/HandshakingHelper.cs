using System;
using System.Threading.Tasks;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Helpers
{
    public class HandshakingHelper
    {
        protected static IMongoClient Client;
        protected static IMongoDatabase Database;
        protected static IMongoCollection<HandshakingInfo> CollectionSubscribers;
        protected static IMongoCollection<HandshakingInfo> CollectionUnsubscribers;

        public IMongoCollection<HandshakingInfo> GetCollectionSubscribers
        {
            get
            {
                return CollectionSubscribers;
            }
        }

        public IMongoCollection<HandshakingInfo> GetCollectionUnsubscribers
        {
            get
            {
                return CollectionUnsubscribers;
            }
        }

        public IMongoDatabase GetDefaultDB
        {
            get
            {
                return Database;
            }
        }

        static HandshakingHelper()
        {
            Connect();
        }

        static bool Connect()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionString);
            Database = Client?.GetDatabase(ConnectionSettings.MongoDBName);
            CollectionSubscribers = Database?.GetCollection<HandshakingInfo>(ConnectionSettings.HandshakingCollectionName);
            CollectionUnsubscribers = Database?.GetCollection<HandshakingInfo>(ConnectionSettings.UnsubscribeHandshakingCollectionName);
            return CollectionSubscribers != null;
        }

        public Task ClearHandShackingListAsync()
        {
            var filter = Builders<HandshakingInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    CollectionSubscribers.DeleteMany(filter);
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
