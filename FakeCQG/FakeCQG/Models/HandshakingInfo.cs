using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace FakeCQG.Internal.Models
{
    public class HandshakingInfo
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

        public Dictionary<string, Dictionary<string, bool>> UnsubscribeEventList;

        public HandshakingInfo(List<string> keys, Dictionary<string, Dictionary<string, bool>> unsubscribeEventList)
        {
            _id = Guid.NewGuid();
            ObjectKeys = keys;
            UnsubscribeEventList = unsubscribeEventList;
        }

        public HandshakingInfo()
        {
            _id = Guid.NewGuid();
            ObjectKeys = new List<string>();
            UnsubscribeEventList = new Dictionary<string, Dictionary<string, bool>>();
        }
    }
}
