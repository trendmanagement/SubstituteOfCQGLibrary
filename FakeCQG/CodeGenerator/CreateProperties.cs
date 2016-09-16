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
                    if (pinfo.PropertyType.Assembly.FullName.Substring(0, 8) == "mscorlib" || pinfo.PropertyType.IsEnum)
                    {
                        File.WriteLine(Indent2 + "{");
                        File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                        File.WriteLine(Indent3 + "var result = (" + TypeToString(pinfo.PropertyType) + ")CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, thisObjUnqKey, name);");
                        File.WriteLine(Indent3 + "return result;");
                        File.WriteLine(Indent2 + "}" + Environment.NewLine);
                    }
                    else
                    {
                        //File.WriteLine(Indent2 + "{");
                        //File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                        //File.WriteLine(Indent3 + TypeToString(pinfo.PropertyType) + " prop = new " +
                        //    TypeToString(pinfo.PropertyType) + "((" + TypeToString(pinfo.PropertyType) +
                        //    ")CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, thisObjUnqKey, name));");
                        //File.WriteLine(Indent3 + "return prop;");
                        //File.WriteLine(Indent2 + "}" + Environment.NewLine);
                        File.WriteLine(Indent2 + "{");
                        File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");
                        File.WriteLine(Indent3 + "string " + pinfo.Name + 
                            "Key = (string)CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, thisObjUnqKey, name);");
                        if (pinfo.PropertyType.IsInterface)
                        {
                            File.WriteLine(Indent3 + TypeToString(pinfo.PropertyType) + "Class prop = new " +
                            TypeToString(pinfo.PropertyType)  + "Class();");
                        }
                        else
                        {
                            File.WriteLine(Indent3 + TypeToString(pinfo.PropertyType) + " prop = new " +
                                TypeToString(pinfo.PropertyType) + "();");
                        }
                        File.WriteLine(Indent3 + "object propFlld = prop;");
                        File.WriteLine(Indent3 + "CQG.GetPropertiesFromMatryoshka(ref propFlld, thisObjUnqKey);");
                        File.WriteLine(Indent3 + "return (" + TypeToString(pinfo.PropertyType) + ")propFlld;");
                        File.WriteLine(Indent2 + "}" + Environment.NewLine);
                    }
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
        //
        //static void GetPropertiesFromMatryoshka(PropertyInfo pinfo)
        //{
        //    foreach(var pi in pinfo.GetType().GetProperties())
        //    {
        //        string propType = TypeToString(pi.PropertyType);
        //        if (propType == "IEnumerable`1")
        //        {
        //            propType = "IEnumerable";
        //        }
        //        if (pi.PropertyType.Assembly.FullName.Substring(0, 8) == "mscorlib" || pi.PropertyType.IsEnum)
        //        {
        //            File.WriteLine(Environment.NewLine + Indent3 + "//////// " + pinfo.Name);
        //            File.WriteLine(Indent3 + "string " + pi.Name + "Name = \"" + pi.Name + "\";");       
        //            File.WriteLine(Indent3 + "var " + pi.Name + " = (" + propType + 
        //                ")CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, " + pinfo.Name + "Key, " + pi.Name + "Name);");
        //            File.WriteLine(Indent3 + "////////");
        //        }
        //        else
        //        {
        //            File.WriteLine(Environment.NewLine + Indent3 + "//////// " + pinfo.Name);
        //            File.WriteLine(Indent3 + "string " + pi.Name + "Name = \"" + pi.Name + "\";");
        //            File.WriteLine(Indent3 + "string " + pi.Name + "Key = (string)CQG.ExecuteTheQuery(QueryInfo.QueryType.Property, " + 
        //                pinfo.Name + "Key, " + pi.Name + "Name);");
        //            if (pi.PropertyType.IsInterface)
        //            {
        //                File.WriteLine(Indent3 + propType + "Class " + pi.Name + "Prop = new " + propType + "Class();");
        //            }
        //            else
        //            {
        //                File.WriteLine(Indent3 + propType + " " + pi.Name + "Prop = new " + propType + "();");
        //            }
        //            GetPropertiesFromMatryoshka(pi);
        //            File.WriteLine(Indent3 + "////////");
        //        }
        //    }
        //}

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