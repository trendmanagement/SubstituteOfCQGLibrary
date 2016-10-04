using System;
using MongoDB.Bson.Serialization.Attributes;

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

        public string Key
        {
            get { return _key; }
            private set { _key = value; }
        }

        #endregion

        private string _key;
        public bool UnSubscribe { get; set; }

        public HandshakingModel(string key)
        {
            _id = Guid.NewGuid();
            _key = key;
        }
    }
}
