using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        // All generated implementations of getters for each property and type
        public static StringBuilder getProp = new StringBuilder();
        public static StringBuilder setProp = new StringBuilder();

        public static string propInterfName = default(string);
        public static string propTypeName = default(string);

        static void CreateProperties(PropertyInfo[] pinfos, bool isInterface, string pTypeName)
        {
            bool intFound = false;
            bool stringFound = false;

            propTypeName = pTypeName;
            
            propInterfName = propTypeName.Substring(0, propTypeName.Length - 5);

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

            var pins = pinfo.GetIndexParameters();

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

                    if (pinfo.Name == "IsStarted")
                    {
                        File.WriteLine(Indent3 + "if(isDCClosed)" + Environment.NewLine + Indent3 +
                            "{" + Environment.NewLine + Indent4 + "return false;" + Environment.NewLine + Indent3 + "}" +
                            Environment.NewLine + Indent3 + "else" + Environment.NewLine + Indent3 + "{");
                        File.WriteLine(Indent4 + "try" + Environment.NewLine + Indent4 + "{");
                        IncreaseIndent();
                        IncreaseIndent();
                    }

                    File.WriteLine(Indent3 + "string name = \"" + pinfo.Name + "\";");

                    string args;
                    if (indexParams.Length == 0)
                    {
                        args = "dcObjKey, dcObjType, name";
                    }
                    else
                    {
                        // Write a line of code that collects all the index parameters into object[] as following:
                        // var args = new object[] { arg1, arg2, ..., argn }
                        CreateArgsObjectArray(File, Indent3, indexParams);

                        args = "dcObjKey, dcObjType, name, args";
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

                    if (pinfo.Name == "IsStarted")
                    {
                        DecreaseIndent();
                        DecreaseIndent();
                        File.WriteLine(Indent4 + "}" + Environment.NewLine + Indent4 + "catch" + Environment.NewLine +
                            Indent4 + "{" + Environment.NewLine  + Indent3 + Indent1 + "return false;" + Environment.NewLine + 
                            Indent4 + "}");
                        File.WriteLine(Indent3 + "}");
                    }

                    File.WriteLine(Indent2 + "}" + Environment.NewLine);


                    // Getters query processing
                    getProp.Append(Indent2 + "private void Get" + propTypeName + pinfo.Name + "(QueryInfo query, object[] args)" + 
                        Environment.NewLine + Indent2 + "{" + Environment.NewLine);

                    hQPOfGettersDict.Append(Indent4 + "{ \"Get" + propTypeName + pinfo.Name + "\", this.Get" + propTypeName + pinfo.Name + "}," +
                        Environment.NewLine);

                    if (pinfo.Name == "IsStarted")
                    {
                        getProp.Append(Indent3 + "System.Boolean IsStartedPropV = CqgDataManagement.IsCQGStarted;" +
                            Environment.NewLine);
                    }
                    else if (pinfo.Name == "Configuration")
                    {
                        getProp.Append(Indent3 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                            ")qObj;" + Environment.NewLine);
                        getProp.Append(Indent3 + "CQG.CQGCELConfiguration ConfigurationPropV = ConfigurationObj.get_Configuration();" +
                            Environment.NewLine);
                    }
                    else if (pinfo.Name == "Value" && propInterfName == "CQGTradingSystemStatistics")
                    {
                        getProp.Append(Indent3 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                            ")qObj;" + Environment.NewLine);
                        getProp.Append(Indent3 + "System.Double ValuePropV = ValueObj[(CQG.eTradingSystemStatistic)args[0]];" +
                            Environment.NewLine);
                    }
                    else if (pins.Length < 1 ||
                        pins.Length >= 1 && pins[0].HasDefaultValue)
                    {
                        getProp.Append(Indent3 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                            ")qObj;" + Environment.NewLine);
                        getProp.Append(Indent3 + pinfo.PropertyType + " " + pinfo.Name + "PropV = " +
                        pinfo.Name + "Obj." + pinfo.Name + ";" + Environment.NewLine);
                    }
                    else
                    {
                        getProp.Append(Indent3 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                            ")qObj;" + Environment.NewLine);

                        if (pins.Length == 1 && pinfo.Name == "Item" || pinfo.Name == "Item")
                        {
                            getProp.Append(Indent3 + pinfo.PropertyType + " " + pinfo.Name + "PropV = " +
                                pinfo.Name + "Obj[");
                            for (int i = 0; i < pins.Length; i++)
                            {
                                getProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                                getProp.Append(i == pins.Length - 1 ? "];" + Environment.NewLine : ",");
                            }
                        }
                        else
                        {
                            getProp.Append(Indent3 + pinfo.PropertyType + " " + pinfo.Name + "PropV = " +
                                pinfo.Name + "Obj." + pinfo.Name + "[");
                            for (int i = 0; i < pins.Length; i++)
                            {
                                getProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                                getProp.Append(i == pins.Length - 1 ? "];" + Environment.NewLine : ",");
                            }
                        }
                    }

                    if (IsSerializableType(pinfo.PropertyType))
                    {
                        getProp.Append(Indent3 + "var " + pinfo.Name + "PropKey = \"value\";" + Environment.NewLine);
                        getProp.Append(Indent3 +
                            "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " +
                            pinfo.Name + "PropKey, value: " + pinfo.Name + "PropV));" +
                            Environment.NewLine);
                    }
                    else
                    {
                        getProp.Append(Indent3 + "var " + pinfo.Name + "PropKey = Core.CreateUniqueKey();" + Environment.NewLine);
                        getProp.Append(Indent3 + "ServerDictionaries.PutObjectToTheDictionary(" + pinfo.Name + "PropKey, " +
                            pinfo.Name + "PropV);" + Environment.NewLine);
                        getProp.Append(Indent3 +
                            "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " +
                            pinfo.Name + "PropKey));" + Environment.NewLine);
                    }

                    getProp.Append(Indent2 + "}" + Environment.NewLine + Environment.NewLine);
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
                    File.WriteLine(Indent3 + "Internal.Core.SetProperty(dcObjKey, dcObjType, name, value);");
                    File.WriteLine(Indent2 + "}");


                    // Setter query processing
                    setProp.Append(Indent2 + "private void Set" + propTypeName + pinfo.Name + "(QueryInfo query, object[] args)" +
                        Environment.NewLine + Indent2 + "{" + Environment.NewLine);

                    hQPOfSettersDict.Append(Indent4 + "{ \"Set" + propTypeName + pinfo.Name + "\", this.Set" + propTypeName + pinfo.Name + "}," + 
                        Environment.NewLine);

                    setProp.Append(Indent3 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                        ")ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);" + Environment.NewLine);
                    setProp.Append(Indent3 + "var " + pinfo.Name + "val = (" + pinfo.PropertyType +
                        ")(Core.ParseInputArgsFromQueryInfo(query)[0]);" + Environment.NewLine);

                    if (pinfo.Name == "Configuration")
                    {
                        setProp.Append(Indent3 + "ConfigurationObj.set_Configuration(ref Configurationval);" +
                            Environment.NewLine);
                    }
                    else if (pins.Length < 1 ||
                        pins.Length >= 1 && pins[0].HasDefaultValue)
                    {
                        setProp.Append(Indent3 + pinfo.Name + "Obj." + pinfo.Name + " = " + pinfo.Name + "val;" + Environment.NewLine);
                    }
                    else
                    {
                        setProp.Append(Indent3 + pinfo.Name + "Obj." + pinfo.Name + "[");
                        for (int i = 0; i < pins.Length; i++)
                        {
                            setProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                            setProp.Append(i == pins.Length - 1 ? "] = " + pinfo.Name + "val;" + Environment.NewLine : ",");
                        }
                    }

                    setProp.Append(Indent3 +
                        "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true));" +
                        Environment.NewLine);

                    setProp.Append(Indent2 + "}" + Environment.NewLine + Environment.NewLine);
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