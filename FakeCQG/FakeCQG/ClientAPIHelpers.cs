using System.Runtime.CompilerServices;
using FakeCQG.Internal.Models;

namespace FakeCQG.Internal
{
    public static partial class Core
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string CallCtor(string fullTypeName, string typeName)
        {
            string dcObjKey = (string)ExecuteTheQuery(QueryType.CallCtor, dcObjType: typeName, memName: fullTypeName);
            return dcObjKey;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CallDtor(string dcObjKey)
        {
            ExecuteTheQuery(QueryType.CallDtor, dcObjKey: dcObjKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetProperty<T>(string dcObjKey, string dcObjType, string propName, object[] args = null)
        {
            T value = (T)ExecuteTheQuery(QueryType.GetProperty, dcObjType, dcObjKey, propName, args);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetProperty(string dcObjKey, string dcObjType, string propName, object value)
        {
            var args = new object[] { value };
            ExecuteTheQuery(QueryType.SetProperty, dcObjType, dcObjKey, propName, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CallMethod<T>(string dcObjKey, string dcObjType, string methodName, object[] args = null)
        {
            T obj = (T)ExecuteTheQuery(QueryType.CallMethod, dcObjType, dcObjKey, methodName, args);
            return obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CallVoidMethod(string dcObjKey, string dcObjType, string methodName, object[] args = null)
        {
            ExecuteTheQuery(QueryType.CallMethod, dcObjType, dcObjKey, methodName, args);
        }
    }
}