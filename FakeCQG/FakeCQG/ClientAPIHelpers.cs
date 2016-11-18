using FakeCQG.Internal.Models;

namespace FakeCQG.Internal
{
    public static partial class Core
    {
        public static string CallCtor(string fullTypeName, string typeName)
        {
            string dcObjKey = (string)ExecuteTheQuery(QueryType.CallCtor, dcObjType: typeName, memName: fullTypeName);
            return dcObjKey;
        }

        public static void CallDtor(string dcObjKey)
        {
            ExecuteTheQuery(QueryType.CallDtor, dcObjKey: dcObjKey);
        }

        public static T GetProperty<T>(string dcObjKey, string dcObjType, string propName, object[] args = null)
        {
            T value = (T)ExecuteTheQuery(QueryType.GetProperty, dcObjType, dcObjKey, propName, args);
            return value;
        }

        public static void SetProperty(string dcObjKey, string dcObjType, string propName, object value)
        {
            var args = new object[] { value };
            ExecuteTheQuery(QueryType.SetProperty, dcObjType, dcObjKey, propName, args);
        }

        public static T CallMethod<T>(string dcObjKey, string dcObjType, string methodName, object[] args = null)
        {
            T obj = (T)ExecuteTheQuery(QueryType.CallMethod, dcObjType, dcObjKey, methodName, args);
            return obj;
        }

        public static void CallVoidMethod(string dcObjKey, string dcObjType, string methodName, object[] args = null)
        {
            ExecuteTheQuery(QueryType.CallMethod, dcObjType, dcObjKey, methodName, args);
        }
    }
}