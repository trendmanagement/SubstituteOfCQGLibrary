using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FakeCQG.Internal.Models
{
    public class EventInfo
    {
        #region Serialized properties

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string EventKey { get; set; }

        public string EventName { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, string> ArgKeys { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, object> ArgValues { get; set; }

        public int NumOfSubscribers { get; set; }

        public Action CQGException
        {
            set
            {
                _exception = value;
            }
        }

        #endregion

        public bool IsCQGException;

        private Action _exception;

        public EventInfo(
            string eventKey,
            string eventName,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null,
            int numOfSubscribers = default(int))
        {
            EventKey = eventKey;
            EventName = eventName;
            ArgKeys = argKeys;
            ArgValues = argValues;
            NumOfSubscribers = numOfSubscribers;
        }

        public override string ToString()
        {
            return string.Format(
                "************************************************************" + Environment.NewLine +
                "EVENT:" + Environment.NewLine +
                "    EventKey = {0}" + Environment.NewLine +
                "    EventName = {1}" + Environment.NewLine +
                "    ArgKeys: {2}" + Environment.NewLine +
                "    ArgValues: {3}" + Environment.NewLine +
                "************************************************************",
                EventKey, EventName, ArgKeys, ArgValues);
        }
    }
}
