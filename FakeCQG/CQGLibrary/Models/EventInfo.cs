﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace FakeCQG.Models
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
            Dictionary<int, object> argValues = null)
        {
            EventKey = eventKey;
            EventName = eventName;
            ArgKeys = argKeys;
            ArgValues = argValues;
        }

        public override string ToString()
        {
            return string.Format(
                "EVENT:" + Environment.NewLine +
                "    EventKey = {0}" + Environment.NewLine +
                "    EventName = {1}" + Environment.NewLine +
                "    ArgKeys: {2}" + Environment.NewLine +
                "    ArgValues: {3}",
                EventKey, EventName, ArgKeys, ArgValues);
        }
    }
}