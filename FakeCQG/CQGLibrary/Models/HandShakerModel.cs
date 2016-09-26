using System;
using MongoDB.Bson.Serialization.Attributes;

namespace FakeCQG.Models
{
    public class HandshakerModel
    {
        [BsonId]
        Guid _id;
        public Guid ID
        {
            get { return _id; }
        }

        private string _key;

        public string Key
        {
            get { return _key; }
            private set { _key = value; }
        }

        public HandshakerModel(string key)
        {
            _id = Guid.NewGuid();
            _key = key;
        }
    }
}
