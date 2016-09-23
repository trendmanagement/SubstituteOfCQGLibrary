using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FakeCQG.Models
{
    public class AnswerInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Key { get; set; }
        public string ObjectKey { get; set; }
        public string QueryName { get; set; }
        public Dictionary<int, object> ArgValues { get; set; }
        public string ValueKey
        {
            get
            {
                if (IsCQGException)
                {
                    _exception.Invoke();
                    return string.Empty;
                }
                return ValueKey;
            }
            set
            {
                ValueKey = value;
            }
        }
        public object Value { get; set; }
        public bool isEventQuery { get; set; }

        public bool IsCQGException;

        private Action _exception;

        public Action CQGException
        {
            set
            {
                _exception = value;
            }
        }

        public AnswerInfo(string key, string objKey, string name, Dictionary<int, object> argVals = null, string vKey = null, object val = null,
            bool isEventQ = false)
        {
            Key = key;
            ObjectKey = objKey;
            QueryName = name;
            ArgValues = argVals;
            ValueKey = vKey;
            Value = val;
            isEventQuery = isEventQ;
        }
    }
}
