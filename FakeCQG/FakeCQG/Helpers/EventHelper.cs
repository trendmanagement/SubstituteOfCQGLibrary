using System;
using FakeCQG.Internal.Models;
using MongoDB.Driver;

namespace FakeCQG.Internal.Helpers
{
    public class EventHelper
    {
        protected IMongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<EventInfo> Collection;

        public IMongoCollection<EventInfo> GetCollection
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

        public EventHelper()
        {
            Connect();
        }

        public bool Connect(string connectionString = "")
        {
            Client = new MongoClient(ConnectionSettings.ConnectionString);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<EventInfo>(ConnectionSettings.EventCollectionName);
            return Collection != null;
        }
        
        public void RemoveEvent(string key)
        {
            var filter = Builders<EventInfo>.Filter.Eq(Keys.EventKey, key);
            try
            {
                Collection.DeleteOne(filter);
            }
            catch (Exception ex)
            {
                Core.OnLogChange(ex.Message);
            }
        }

        public bool CheckWhetherEventHappened(string name, out object[] args)
        {
            string handlerName = string.Format("_ICQGCELEvents_{0}EventHandler", name);
            var filter = Builders<EventInfo>.Filter.Eq(Keys.EventName, handlerName);
            try
            {
                EventInfo eventInfo = Collection.Find(filter).First();
                args = Core.ParseInputArgsFromEventInfo(eventInfo);
                RemoveEvent(eventInfo.EventKey);
                return true;
            }
            catch (Exception ex)
            {
                Core.OnLogChange(ex.Message);
                args = null;
                return false;
            }
        }
    }
}