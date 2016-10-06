using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace FakeCQG.Models
{
    public class HandshakingModel
    {
        #region Serialized properties

        [BsonId]
        Guid _id;
        public Guid ID
        {
            get { return _id; }
        }

        #endregion

        public List<string> ObjectKeys;

        public Dictionary<string, string> UnsubscribeEventList;

        public bool UnSubscribe { get; set; }

        public HandshakingModel(List<string> keys, Dictionary<string, string> unsubscribeEventList)
        {
            _id = Guid.NewGuid();
            ObjectKeys = keys;
            UnsubscribeEventList = unsubscribeEventList;
        }

        public HandshakingModel()
        {
            _id = Guid.NewGuid();
            ObjectKeys = new List<string>();
            UnsubscribeEventList = new Dictionary<string, string>();
        }
    }
}
