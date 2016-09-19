using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQGLibrary.Models
{
    public class HandShakerModel
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            private set { _key = value; }
        }

        public HandShakerModel(string key)
        {
            _key = key;
        }
    }
}
