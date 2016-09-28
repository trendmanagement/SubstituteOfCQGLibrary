using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FakeCQG.Models
{
    public class QueryInfo
    {
        public enum QueryType
        {
            CallCtor,
            CallDtor,
            GetProperty,
            SetProperty,
            CallMethod,
            Event
        }

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Key { get; set; }
        public string ObjectKey { get; set; }
        public string QueryName { get; set; }
        public Dictionary<string, string> ArgKeys { get; set; }
        public Dictionary<string, object> ArgValues { get; set; }
        public QueryType TypeOfQuery { get; set; }

        public QueryInfo(
            QueryType qType,
            string key,
            string objKey,
            string qName, 
            Dictionary<string, string> argKeys = null,
            Dictionary<string, object> argVals = null)
        {
            Key = key;
            ObjectKey = objKey;
            QueryName = qName;
            ArgKeys = argKeys;
            ArgValues = argVals;
            TypeOfQuery = qType;
        }

        public override string ToString()
        {
            return string.Format(
                "QUERY:" + Environment.NewLine +
                "    Key: {0}" + Environment.NewLine +
                "    ObjectKey: {1}" + Environment.NewLine +
                "    QueryName: {2}" + Environment.NewLine +
                "    ArgKeys: {3}" + Environment.NewLine +
                "    ArgValues: {4}" + Environment.NewLine +
                "    TypeOfQuery: {5}",
                Key, ObjectKey, QueryName, ArgKeys, ArgValues, TypeOfQuery);
        }
    }
}
