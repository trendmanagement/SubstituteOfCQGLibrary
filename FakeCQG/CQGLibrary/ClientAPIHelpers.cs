using FakeCQG.Models;

namespace FakeCQG
{
    public partial class CQG
    {
        public static string CallCtor(string name)
        {
            string dcObjKey = (string)ExecuteTheQuery(QueryInfo.QueryType.Ctor, null, name);
            return dcObjKey;
        }

        public static void CallDtor(string dcObjKey)
        {
            ExecuteTheQuery(QueryInfo.QueryType.Dtor, dcObjKey, null);
        }

        public static T GetProperty<T>(string dcObjKey, string name, object[] args = null)
        {
            dynamic value = ExecuteTheQuery(QueryInfo.QueryType.GetProperty, dcObjKey, name, args);
            return value;
        }

        public static void SetProperty(string dcObjKey, string name, object value)
        {
            var args = new object[] { value };
            ExecuteTheQuery(QueryInfo.QueryType.SetProperty, dcObjKey, name, args);
        }

        public static T CallMethod<T>(string dcObjKey, string name, object[] args = null)
        {
            dynamic obj = ExecuteTheQuery(QueryInfo.QueryType.Method, dcObjKey, name, args);
            return obj;
        }

        public static void CallVoidMethod(string dcObjKey, string name, object[] args = null)
        {
            ExecuteTheQuery(QueryInfo.QueryType.Method, dcObjKey, name, args);
        }
    }
}