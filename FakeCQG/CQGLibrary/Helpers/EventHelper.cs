using System;
using System.Threading.Tasks;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
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

        public void FireEvent(EventInfo eventInfo)
        {
            try
            {
                Collection.InsertOne(eventInfo);
                lock (CQG.LogLock)
                {
                    CQG.OnLogChange("************************************************************");
                    CQG.OnLogChange(eventInfo.ToString());
                }
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
            }
        }

        public Task ClearEventsListAsync()
        {
            var filter = Builders<EventInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteMany(filter);
                    CQG.OnLogChange("Events list was cleared successfully");
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
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
                CQG.OnLogChange(ex.Message);
            }
        }

        public bool CheckWhetherEventHappened(string name, out object[] args)
        {
            string handlerName = string.Format("_ICQGCELEvents_{0}EventHandler", name);
            var filter = Builders<EventInfo>.Filter.Eq(Keys.EventName, handlerName);
            try
            {
                EventInfo eventInfo = Collection.Find(filter).First();
                args = CQG.ParseInputArgsFromEventInfo(eventInfo);
                RemoveEvent(eventInfo.EventKey);
                return true;
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
                args = null;
                return false;
            }
        }
    }
}