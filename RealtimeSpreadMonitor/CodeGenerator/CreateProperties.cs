using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateProperties(PropertyInfo[] pinfos, bool isInterface)
        {
            bool intFound = false;
            bool stringFound = false;
            foreach (PropertyInfo pinfo in SortProperties(pinfos))
            {
                var ips = pinfo.GetIndexParameters();
                if (ips.Length == 1)
                {
                    if (ips[0].HasDefaultValue)
                    {
                        continue;
                    }
                    Type ipType = ips[0].ParameterType;
                    if ((ipType != typeof(int) || !intFound) && (ipType != typeof(string) || !stringFound))
                    {
                        CreateProperty(pinfo, isInterface);
                    }
                    if (ipType == typeof(int))
                    {
                        intFound = true;
                    }
                    else if (ipType == typeof(string))
                    {
                        stringFound = true;
                    }
                }
                else
                {
                    CreateProperty(pinfo, isInterface);
                }
            }
        }

        static void CreateProperty(PropertyInfo pinfo, bool isInterface)
        {
            UpdateRegion(RegionType.Properties);

            string public_ = !isInterface ? "public " : string.Empty;
            File.Write(Indent1 + public_);
            if (IsStaticProperty(pinfo))
            {
                File.Write("static ");
            }
            File.Write(TypeToString(pinfo.PropertyType) + " ");

            ParameterInfo[] indexParams = pinfo.GetIndexParameters();
            if (indexParams.Length == 0)
            {
                // This is a regular property
                File.WriteLine(pinfo.Name);
            }
            else
            {
                // This is an index property
                File.Write("this[");
                CreateMethodArguments(indexParams, null);
                File.WriteLine("]");
            }

            File.WriteLine(Indent1 + "{");

            string colon = isInterface ? ";" : string.Empty;

            if (pinfo.GetGetMethod() != null)
            {
                File.WriteLine(Indent2 + "get" + colon);
                if (!isInterface)
                {
                    File.WriteLine(Indent2 + "{");
                    File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                    File.WriteLine(Indent3 + "var result = (" + TypeToString(pinfo.PropertyType) + ")CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, thisObjUnqKey, name);");
                    File.WriteLine(Indent3 + "return result;");
                    File.WriteLine(Indent2 + "}" + Environment.NewLine);
                }
            }

            if (pinfo.GetSetMethod() != null)
            {
                File.WriteLine(Indent2 + "set" + colon);
                if (!isInterface)
                {
                    File.WriteLine(Indent2 + "{");
                    File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                    File.WriteLine(Indent3 + "CQG.SetProperty(name, thisObjUnqKey, value);");
                    File.WriteLine(Indent2 + "}");
                }
            }

            MemberEnd();
        }

        static bool IsStaticProperty(PropertyInfo pinfo)
        {
            MethodInfo minfo = pinfo.GetGetMethod();
            if (minfo != null && minfo.IsStatic)
            {
                return true;
            }

            minfo = pinfo.GetSetMethod();
            if (minfo != null && minfo.IsStatic)
            {
                return true;
            }

            return false;
        }

        static IEnumerable<PropertyInfo> SortProperties(PropertyInfo[] pinfos)
        {
            return pinfos.OrderBy(pinfo => pinfo.Name);
        }
    }
}