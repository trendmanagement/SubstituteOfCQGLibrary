using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FakeCQG.Models
{
    public class QueryInfo
    {
        public enum QueryType {Property, Method, Constructor, Event, Destructor}

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Key { get; set; }
        public string ObjectKey { get; set; }
        public string QueryName { get; set; }
        public Dictionary<int, string> ArgKeys { get; set; }
        public Dictionary<int, object> ArgValues { get; set; }
        public QueryType TypeOfQuery { get; set; }

        public QueryInfo(QueryType qType, string key, string objKey, string qName, 
            Dictionary<int, string> argKeys = null, Dictionary<int, object> argVals = null)
        {
            Key = key;
            ObjectKey = objKey;
            QueryName = qName;
            ArgKeys = argKeys;
            ArgValues = argVals;
            TypeOfQuery = qType;
        }
    }
}
