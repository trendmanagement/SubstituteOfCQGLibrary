using System;
using System.Collections.Generic;

namespace CodeGenerator
{
    partial class Program
    {
        static Dictionary<Type, string> TypeToStringDict = new Dictionary<Type, string> {
            {typeof(bool), "bool"},
            {typeof(uint), "uint"},
            {typeof(sbyte), "sbyte"},
            {typeof(short), "short"},
            {typeof(int), "int"},
            {typeof(double), "double"},
            {typeof(string), "string"},
            {typeof(object), "object"},
            {typeof(void), "void"}
        };

        /// <summary>
        /// Get the type name in short form, e.g. "int" instead of "Int32".
        /// This method is the bottleneck of the code generator if QuickTestMode is False.
        /// </summary>
        static string TypeToString(Type type)
        {
            if (QuickTestMode)
            {
                return type.Name;
            }
            else
            {
                try
                {
                    return TypeToStringDict[type];
                }
                catch (KeyNotFoundException)
                {
                    return type.Name;
                }
            }
        }
    }
}