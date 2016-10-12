using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeCQG.Internal;
using MongoDB.Driver;

namespace DataCollectionForRealtime
{
    static class EventHandler
    {
        // Common handler of CQG events
        // Handler make record in DB and passing args with it for Invoke method
        public static void CommonEventHandler(
            string name,
            object[] args = null)
        {
            Dictionary<int, string> argKeys;
            Dictionary<int, object> argVals;
            Core.PutArgsFromArrayIntoTwoDicts(args, false, out argKeys, out argVals);

            string eventKey = Core.CreateUniqueKey();

            var eventInfo = new FakeCQG.Internal.Models.EventInfo(eventKey, name, argKeys, argVals);

            Task.Run(() => FireEvent(eventInfo));
        }

        public static void FireEvent(FakeCQG.Internal.Models.EventInfo eventInfo)
        {
            try
            {
                Core.EventHelper.GetCollection.InsertOne(eventInfo);
                lock (Core.LogLock)
                {
                    AsyncTaskListener.LogMessage(eventInfo.ToString());
                }
            }
            catch (Exception ex)
            {
                AsyncTaskListener.LogMessage(ex.Message);
            }
        }

        public static Task ClearEventsListAsync()
        {
            var filter = Builders<FakeCQG.Internal.Models.EventInfo>.Filter.Empty;
            return Task.Run(() =>
            {
                try
                {
                    Core.EventHelper.GetCollection.DeleteMany(filter);
                    AsyncTaskListener.LogMessage("Events list was cleared successfully");
                }
                catch (Exception ex)
                {
                    AsyncTaskListener.LogMessage(ex.Message);
                }
            });
        }
    }
}
