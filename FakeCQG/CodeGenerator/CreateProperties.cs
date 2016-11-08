﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        public static StringBuilder getProp = new StringBuilder();
        public static StringBuilder setProp = new StringBuilder();
        public static string propInterfName = default(string);

        static void CreateProperties(PropertyInfo[] pinfos, bool isInterface, string propTypeName)
        {
            bool intFound = false;
            bool stringFound = false;

            int gettersNum = 0;
            int settersNum = 0;          

            foreach (var pinf in pinfos)
            {
                if(pinf.GetGetMethod() != null)
                {
                    gettersNum++;
                }
                if (pinf.GetSetMethod() != null)
                {
                    settersNum++;
                }
            }

            if (!isInterface)
            {
                propInterfName = propTypeName.Substring(0, propTypeName.Length - 5);

                if(gettersNum > 0)
                {
                    getProp.Append(Indent5 + "case \"CQG." + propTypeName + "\":" +
                        Environment.NewLine);
                    getProp.Append(Indent6 + "switch (query.MemberName)" +
                        Environment.NewLine + Indent6 + "{" + Environment.NewLine);
                }

                if (settersNum > 0)
                {
                    setProp.Append(Indent5 + "case \"CQG." + propTypeName + "\":" +
                        Environment.NewLine);
                    setProp.Append(Indent6 + "switch (query.MemberName)" +
                        Environment.NewLine + Indent6 + "{" + Environment.NewLine);
                }      
            }

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
            if (!isInterface)
            {
                if (gettersNum > 0)
                {
                    getProp.Append(Indent6 + "}" + Environment.NewLine);
                    getProp.Append(Indent6 + "break;" + Environment.NewLine);
                }

                if (settersNum > 0)
                {
                    setProp.Append(Indent6 + "}" + Environment.NewLine);
                    setProp.Append(Indent6 + "break;" + Environment.NewLine);
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
                    getProp.Append(Indent6 + "case \"" + pinfo.Name + "\":" + Environment.NewLine);
                    getProp.Append(Indent7 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName + 
                        ")ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);" + Environment.NewLine);

                    if(pinfo.Name == "Configuration")
                    {
                        getProp.Append(Indent7 + "CQG.CQGCELConfiguration ConfigurationpropV = ConfigurationObj.get_Configuration();" + 
                            Environment.NewLine);
                    }
                    else if (pinfo.Name == "Value" && propInterfName == "CQGTradingSystemStatistics")
                    {
                        getProp.Append(Indent7 + "System.Double ValuepropV = ValueObj[(CQG.eTradingSystemStatistic)args[0]];" + 
                            Environment.NewLine);
                    }
                    else if(pins.Length < 1 ||
                        pins.Length >= 1 && pins[0].HasDefaultValue)
                    {
                        getProp.Append(Indent7 + pinfo.PropertyType + " " + pinfo.Name + "propV = " +
                        pinfo.Name + "Obj." + pinfo.Name + ";" + Environment.NewLine);
                    }
                    else
                    {
                        if(pins.Length == 1 && pinfo.Name == "Item" || pinfo.Name == "Item")
                        {
                            getProp.Append(Indent7 + pinfo.PropertyType + " " + pinfo.Name + "propV = " +
                                pinfo.Name + "Obj[");
                            for (int i = 0; i < pins.Length; i++)
                            {
                                getProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                                getProp.Append(i == pins.Length - 1 ? "];" + Environment.NewLine : ",");
                            }
                        }
                        else
                        {
                            getProp.Append(Indent7 + pinfo.PropertyType + " " + pinfo.Name + "propV = " + 
                                pinfo.Name + "Obj." + pinfo.Name +"[");
                            for (int i = 0; i < pins.Length; i++)
                            {
                                getProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                                getProp.Append(i == pins.Length - 1 ? "];" + Environment.NewLine : ",");
                            }
                        }
                    }

                    if (IsSerializableType(pinfo.PropertyType))
                    {
                        getProp.Append(Indent7 + "var " + pinfo.Name + "PropKey = \"value\";" + Environment.NewLine);
                        getProp.Append(Indent7 +
                            "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " + 
                            pinfo.Name + "PropKey, value: " + pinfo.Name + "propV));" + 
                            Environment.NewLine);    
                    }
                    else
                    {
                        getProp.Append(Indent7 + "var " + pinfo.Name + "PropKey = Core.CreateUniqueKey();" + Environment.NewLine);
                        getProp.Append(Indent7 + "ServerDictionaries.PutObjectToTheDictionary(" + pinfo.Name + "PropKey, " +
                            pinfo.Name + "propV);" + Environment.NewLine);
                        getProp.Append(Indent7 +
                            "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " + 
                            pinfo.Name + "PropKey));" + Environment.NewLine);
                    }

                    getProp.Append(Indent7 + "break;" + Environment.NewLine);
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


                    // Setter query processing
                    setProp.Append(Indent6 + "case \"" + pinfo.Name + "\":" + Environment.NewLine);
                    setProp.Append(Indent7 + propInterfName + " " + pinfo.Name + "Obj = (" + propInterfName +
                        ")ServerDictionaries.GetObjectFromTheDictionary(query.ObjectKey);" + Environment.NewLine);
                    setProp.Append(Indent7 + "var " + pinfo.Name + "val = (" + pinfo.PropertyType +
                        ")(Core.ParseInputArgsFromQueryInfo(query)[0]);" + Environment.NewLine);

                    if (pinfo.Name == "Configuration")
                    {
                        setProp.Append(Indent7 + "ConfigurationObj.set_Configuration(ref Configurationval);" +
                            Environment.NewLine);
                    }
                    else if (pins.Length < 1 ||
                        pins.Length >= 1 && pins[0].HasDefaultValue)
                    {
                        setProp.Append(Indent7 + pinfo.Name + "Obj." + pinfo.Name + " = " + pinfo.Name + "val;" + Environment.NewLine);
                    }
                    else
                    {
                        setProp.Append(Indent7 + pinfo.Name + "Obj." + pinfo.Name + "[");
                        for (int i = 0; i < pins.Length; i++)
                        {
                            setProp.Append("(" + pins[i].ParameterType + ")args[" + i + "]");
                            setProp.Append(i == pins.Length - 1 ? "] = " + pinfo.Name + "val;" + Environment.NewLine : ",");
                        }
                    }

                    setProp.Append(Indent7 + 
                        "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true));" + 
                        Environment.NewLine);
                    setProp.Append(Indent7 + "break;" + Environment.NewLine);
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