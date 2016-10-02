using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FakeCQG.Models
{
    public class EventAnswerInfo
    {
        #region Serialized properties

        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string EventKey { get; set; }

        public string EventName { get; set; }

        //[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public object[] Args { get; set; }

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

        public EventAnswerInfo(
            string eventKey,
            string eventName,
            object[] args = null)
        {
            EventKey = eventKey;
            EventName = eventName;
            Args = args;
        }

        public override string ToString()
        {
            return string.Format(
                "ANSWER:" + Environment.NewLine +
                "    EventKey = {0}" + Environment.NewLine +
                "    EventName = {1}" + Environment.NewLine +
                "    Args = {2}",
                EventKey, EventName, Args);
        }
    }
}
