using FakeCQG.Models;

namespace FakeCQG
{
    public static partial class CQG
    {
        public static string CallCtor(string name)
        {
            string dcObjKey = (string)ExecuteTheQuery(QueryInfo.QueryType.CallCtor, null, name);
            return dcObjKey;
        }

        public static void CallDtor(string dcObjKey)
        {
            ExecuteTheQuery(QueryInfo.QueryType.CallDtor, dcObjKey, null);
        }

        public static T GetProperty<T>(string dcObjKey, string name, object[] args = null)
        {
            T value = (T)ExecuteTheQuery(QueryInfo.QueryType.GetProperty, dcObjKey, name, args);
            return value;
        }

        public static void SetProperty(string dcObjKey, string name, object value)
        {
            var args = new object[] { value };
            ExecuteTheQuery(QueryInfo.QueryType.SetProperty, dcObjKey, name, args);
        }

        public static T CallMethod<T>(string dcObjKey, string name, object[] args = null)
        {
            T obj = (T)ExecuteTheQuery(QueryInfo.QueryType.CallMethod, dcObjKey, name, args);
            return obj;
        }

        public static void CallVoidMethod(string dcObjKey, string name, object[] args = null)
        {
            ExecuteTheQuery(QueryInfo.QueryType.CallMethod, dcObjKey, name, args);
        }
    }
}