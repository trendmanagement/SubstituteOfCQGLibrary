using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeCQG.Models;
using MongoDB.Driver;

namespace FakeCQG.Helpers
{
    public class EventHelper
    {
        private const string eventFiringId = "!EventHappened";

        protected IMongoClient Client;
        protected IMongoDatabase Database;
        protected IMongoCollection<EventAnswerInfo> Collection;

        public IMongoCollection<EventAnswerInfo> GetCollection
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

        public string EventFiringId
        {
            get
            {
                return eventFiringId;
            }
        }

        public EventHelper()
        {
            Client = new MongoClient(ConnectionSettings.ConnectionStringDefault);
            Database = Client.GetDatabase(ConnectionSettings.MongoDBName);
            Collection = Database.GetCollection<EventAnswerInfo>(ConnectionSettings.EventCollectionName);
        }

        public Task FireEventAsync(EventAnswerInfo eventInfo)
        {
            return Task.Run(() => FireEvent(eventInfo));
        }

        public void FireEvent(EventAnswerInfo eventInfo)
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

        public Task ClearAnswersListAsync()
        {
            var filter = Builders<EventAnswerInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteMany(filter);
                    CQG.OnLogChange("Answers list was cleared successfully");
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public Task RemoveAnswerAsync(string key)
        {
            var filter = Builders<EventAnswerInfo>.Filter.Eq(Keys.EventKey, key);
            return Task.Run(() =>
            {
                try
                {
                    Collection.DeleteOne(filter);
                }
                catch (Exception ex)
                {
                    CQG.OnLogChange(ex.Message);
                }
            });
        }

        public object[] CheckWhetherEventHappened(string name)
        {
            string handlerName = string.Format("_ICQGCELEvents_{0}EventHandler", name);
            var filter = Builders<EventAnswerInfo>.Filter.Eq(Keys.EventName, handlerName);
            try
            {
                EventAnswerInfo answer = Collection.Find(filter).First();
                var args = answer.Args;
                RemoveAnswerAsync(answer.EventKey);
                return args;
            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
                return null;
            }
        }

        public EventHelper(
            string name,
            object[] args = null,
            int[] nonSerArgsPos = null) :this()
        {
            var argValues = args;
            
            if(nonSerArgsPos != null)
            {
                foreach (int i in nonSerArgsPos)
                {
                    string paramKey = CQG.CreateUniqueKey();
                    ServerDictionaries.PutObjectToTheDictionary(paramKey, args[i]);
                    args[i] = paramKey;
                }
            }
            
            try
            {
                string eventKey = eventFiringId + CQG.CreateUniqueKey();
                var answer = new EventAnswerInfo(eventKey, name, args);
                FireEvent(answer);

            }
            catch (Exception ex)
            {
                CQG.OnLogChange(ex.Message);
            }
        }
    }
}