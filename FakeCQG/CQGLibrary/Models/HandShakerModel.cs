using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGLibrary.Models
{
    public class HandShakerModel
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

        public HandShakerModel(string key)
        {
            _id = Guid.NewGuid();
            _key = key;
        }
    }
}
