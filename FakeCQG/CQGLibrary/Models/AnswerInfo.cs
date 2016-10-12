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

        public string ValueKey
        {
            get
            {
                if (IsCQGException)
                {
                    _exception.Invoke();
                    return string.Empty;
                }
                return _valueKey;
            }
            set
            {
                _valueKey = value;
            }
        }

        public object Value { get; set; }

        public Action CQGException
        {
            set
            {
                _exception = value;
            }
        }

        #endregion

        private string _valueKey;

        public bool IsCQGException;

        private Action _exception;

        public AnswerInfo(
            string answerKey,
            string objectKey,
            string memberName,
            Dictionary<int, string> argKeys = null,
            Dictionary<int, object> argValues = null,
            string valueKey = null,
            object value = null)
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
            return string.Format(
                "************************************************************" + Environment.NewLine +
                "ANSWER:" + Environment.NewLine +
                "    AnswerKey = {0}" + Environment.NewLine +
                "    ObjectKey = {1}" + Environment.NewLine +
                "    MemberName = {2}" + Environment.NewLine +
                "    ArgKeys = {3}" + Environment.NewLine +
                "    ArgValues = {4}" + Environment.NewLine +
                "    ValueKey = {5}" + Environment.NewLine +
                "    Value = {6}" + Environment.NewLine +
                "************************************************************",
                AnswerKey, ObjectKey, MemberName, ArgKeys, ArgValues, ValueKey, Value);
        }
    }
}
