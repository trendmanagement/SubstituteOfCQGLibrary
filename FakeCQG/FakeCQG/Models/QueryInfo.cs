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

        public string ObjectType { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, string> ArgKeys { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, object> ArgValues { get; set; }

        #endregion

        public QueryInfo(
            QueryType queryType,
            string queryKey,
            string objectType = null,
            string objectKey = null,
            string memberName = null,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null)
        {
            QueryType = queryType;    
            QueryKey = queryKey;
            MemberName = memberName;
            ObjectType = objectType;
            ObjectKey = objectKey;
            ArgKeys = argKeys;
            ArgValues = argValues;
        }

        public override string ToString()
        {
            return string.Concat(
                "************************************************************", Environment.NewLine,
                "QUERY:", Environment.NewLine,
                "    QueryType: ", QueryType, Environment.NewLine,
                "    QueryKey: ", QueryKey, Environment.NewLine,
                "    MemberName: ", MemberName, Environment.NewLine,
                "    ObjectType: ", ObjectType, Environment.NewLine,
                "    ObjectKey: ", ObjectKey, Environment.NewLine,
                "    ArgKeys: ", ArgKeys, Environment.NewLine,
                "    ArgValues: ", ArgValues, Environment.NewLine,
                "************************************************************");
        }
    }
}
