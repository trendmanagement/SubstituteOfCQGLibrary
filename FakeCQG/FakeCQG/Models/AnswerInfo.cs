using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FakeCQG.Internal.Models
{
    public class AnswerInfo
    {
        #region Serialized properties

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string AnswerKey { get; set; }

        public string ObjectKey { get; set; }

        public string MemberName { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, string> ArgKeys { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, object> ArgValues { get; set; }

        public string ValueKey { get; set; }

        public object Value { get; set; }

        #endregion

        public bool IsCQGException;

        public ExceptionInfo CQGException { get; set; }

        public AnswerInfo(
            string answerKey,
            string objectKey,
            string memberName,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null,
            string valueKey = default(string),
            object value = default(object))
        {
            AnswerKey = answerKey;
            ObjectKey = objectKey;
            MemberName = memberName;
            ArgKeys = argKeys;
            ArgValues = argValues;
            ValueKey = valueKey;
            Value = value;
        }

        public override string ToString()
        {
            return string.Concat(
                "************************************************************", Environment.NewLine,
                "ANSWER:", Environment.NewLine,
                "    AnswerKey = ", AnswerKey, Environment.NewLine,
                "    ObjectKey = ", ObjectKey, Environment.NewLine,
                "    MemberName = ", MemberName, Environment.NewLine,
                "    ArgKeys = ", ArgKeys, Environment.NewLine,
                "    ArgValues = ", ArgValues, Environment.NewLine,
                "    ValueKey = ", ValueKey, Environment.NewLine,
                "    Value = ", Value, Environment.NewLine,
                "************************************************************");
        }
    }

    public class ExceptionInfo
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
    }
}
