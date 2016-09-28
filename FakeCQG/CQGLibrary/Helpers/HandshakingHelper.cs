using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class HandshakingrHelper
    {
        protected IMongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<HandshakerModel> Collection;

        public IMongoCollection<HandshakerModel> GetCollection
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

        public HandshakingrHelper()
        {
            Connect();
        }

        ~HandshakingrHelper()
        {
            Disconnect();
        }

        private void Disconnect()
        {
            Client.DropDatabase(ConnectionSettings.MongoDBName);
        }

        public bool Connect()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<HandshakerModel>(ConnectionSettings.HandshakingCollectionName);
            return (Collection != null) ? false : true;
        }
    }
}
