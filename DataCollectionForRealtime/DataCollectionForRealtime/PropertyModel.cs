using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionForRealtime.Model
{
    public class PropertyModel<T>
    {
        public string Key { get; set; }
        public string PropName { get; set; }
        public T propertyValue { get; set; }
        public bool IsAnswer { get; set; }

        public PropertyModel(string key, string name)
        {
            Key = key;
            PropName = name;
        }
    }
}
