using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FakeCQG.Internal.Models
{
    public enum QueryType
    {
        CallCtor,
        CallDtor,
        GetProperty,
        SetProperty,
        CallMethod,
        SubscribeToEvent,
        UnsubscribeFromEvent
    }

    public class QueryInfo
    {
        #region Serialized properties

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string QueryKey { get; set; }

        public QueryType QueryType { get; set; }

        public string MemberName { get; set; }

        public string ObjectKey { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, string> ArgKeys { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, object> ArgValues { get; set; }

        #endregion

        public QueryInfo(
            QueryType queryType,
            string queryKey,
            string objectKey = null,
            string memberName = null,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null)
        {
            QueryType = queryType;
            QueryKey = queryKey;
            MemberName = memberName;
            ObjectKey = objectKey;
            ArgKeys = argKeys;
            ArgValues = argValues;
        }

        public override string ToString()
        {
            return string.Format(
                "QUERY:" + Environment.NewLine +
                "    QueryType: {0}" + Environment.NewLine +
                "    QueryKey: {1}" + Environment.NewLine +
                "    MemberName: {2}" + Environment.NewLine +
                "    ObjectKey: {3}" + Environment.NewLine +
                "    ArgKeys: {4}" + Environment.NewLine +
                "    ArgValues: {5}",
                QueryType, QueryKey, MemberName, ObjectKey, ArgKeys, ArgValues);
        }
    }
}
