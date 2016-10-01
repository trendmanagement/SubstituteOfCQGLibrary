using FakeCQG.Models;

namespace FakeCQG
{
    public static partial class CQG
    {
        public static string CallCtor(string typeName)
        {
            string dcObjKey = (string)ExecuteTheQuery(QueryType.CallCtor, memName: typeName);
            return dcObjKey;
        }

        public static void CallDtor(string dcObjKey)
        {
            ExecuteTheQuery(QueryType.CallDtor, dcObjKey: dcObjKey);
        }

        public static T GetProperty<T>(string dcObjKey, string propName, object[] args = null)
        {
            T value = (T)ExecuteTheQuery(QueryType.GetProperty, dcObjKey, propName, args);
            return value;
        }

        public static void SetProperty(string dcObjKey, string propName, object value)
        {
            var args = new object[] { value };
            ExecuteTheQuery(QueryType.SetProperty, dcObjKey, propName, args);
        }

        public static T CallMethod<T>(string dcObjKey, string methodName, object[] args = null)
        {
            T obj = (T)ExecuteTheQuery(QueryType.CallMethod, dcObjKey, methodName, args);
            return obj;
        }

        public static void CallVoidMethod(string dcObjKey, string methodName, object[] args = null)
        {
            ExecuteTheQuery(QueryType.CallMethod, dcObjKey, methodName, args);
        }
    }
}