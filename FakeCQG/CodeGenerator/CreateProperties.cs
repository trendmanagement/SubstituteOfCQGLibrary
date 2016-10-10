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

            // Write property type
            string propTypeStr = TypeToString(pinfo.PropertyType);
            File.Write(propTypeStr + " ");

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
                CreateMethodArguments(indexParams);
                File.WriteLine("]");
            }

            File.WriteLine(Indent1 + "{");

            string colonOrEmpty = isInterface ? ";" : string.Empty;

            if (pinfo.GetGetMethod() != null)
            {
                // Create getter
                File.WriteLine(Indent2 + "get" + colonOrEmpty);
                if (!isInterface)
                {
                    File.WriteLine(Indent2 + "{");
                    File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");

                    string args;
                    if (indexParams.Length == 0)
                    {
                        args = "dcObjKey, name";
                    }
                    else
                    {
                        // Write a line of code that collects all the index parameters into object[] as following:
                        // var args = new object[] { arg1, arg2, ..., argn }
                        CreateArgsObjectArray(File, Indent3, indexParams);

                        args = "dcObjKey, name, args";
                    }

                    if (IsSerializableType(pinfo.PropertyType))
                    {
                        File.WriteLine(Indent3 + "var value = Internal.Core.GetProperty<" + propTypeStr + ">(" + args + ");");
                    }
                    else
                    {
                        File.WriteLine(Indent3 + "string key = Internal.Core.GetProperty<string>(" + args + ");");
                        File.Write(Indent3 + "var value = new " + propTypeStr);
                        if (pinfo.PropertyType.IsInterface)
                        {
                            File.Write("Class");
                        }
                        File.WriteLine("(key);");
                    }

                    File.WriteLine(Indent3 + "return value;");
                    File.WriteLine(Indent2 + "}" + Environment.NewLine);
                }
            }

            if (pinfo.GetSetMethod() != null)
            {
                // Create setter
                File.WriteLine(Indent2 + "set" + colonOrEmpty);
                if (!isInterface)
                {
                    if (indexParams.Length > 1)
                    {
                        // Such properties were not found in CQG 
                        throw new NotImplementedException();
                    }
                    File.WriteLine(Indent2 + "{");
                    File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                    File.WriteLine(Indent3 + "Internal.Core.SetProperty(dcObjKey, name, value);");
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