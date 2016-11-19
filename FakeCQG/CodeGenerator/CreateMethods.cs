using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    partial class Program
    {
        // All generated implementations of methods calling for each type
        public static StringBuilder methodCall = new StringBuilder();

        public static string methodTypeName = default(string);
        public static string methodInterfName = default(string);

        // The methods with the next name will be skipped always
        static HashSet<string> SkippedMethodsNames = new HashSet<string>() { "get_Item", "Finalize" };

        // The methods with the next name prefixes will be skipped always
        static HashSet<string> SkippedMethodsPrefixes = new HashSet<string>() { "add_", "remove_" };

        // The methods with the next name prefixes will be skipped if they have the specified number of input arguments
        // (otherwise, only corresponding properties will be created)
        static Tuple<string, int>[] SkippedMethodsPrefixesNumArgs = new Tuple<string, int>[] { Tuple.Create("get_", 0), Tuple.Create("set_", 1) };

        static HashSet<string> ObjectNonOverridableMethods = new HashSet<string>() { "Equals", "GetType" };
        static HashSet<string> ObjectOverridableMethods = new HashSet<string>() { "GetHashCode", "ToString" };

        static HashSet<string> IEnumerableMethods = new HashSet<string>() { "GetEnumerator" };

        static HashSet<string> ComMethods = new HashSet<string>() { "CreateObjRef", "GetLifetimeService", "InitializeLifetimeService" };

        public static void CreateMethods(Type type)
        {
            methodTypeName = type.Name;
            methodInterfName = methodTypeName.Substring(0, methodTypeName.Length - 5);

            foreach (MethodInfo minfo in FilterSortMethods(type.GetMethods()))
            {
                CreateMethod(minfo, type.IsInterface, type.IsValueType);
            }
            
        }

        static void CreateMethod(MethodInfo minfo, bool isInterface, bool isStruct)
        {
            UpdateRegion(RegionType.Methods);

            if (ComMethods.Contains(minfo.Name) ||
                ObjectNonOverridableMethods.Contains(minfo.Name) ||
                (isStruct && ObjectOverridableMethods.Contains(minfo.Name)) ||
                (isInterface && IEnumerableMethods.Contains(minfo.Name)))
            {
                // Skip this method
                return;
            }

            var argins = minfo.GetParameters();

            CreateMethodSignature(minfo, TypeToString(minfo.ReturnType), null, isInterface, isStruct);

            if (isInterface)
            {
                File.WriteLine(string.Empty);
                return;
            }

            if (minfo.Name == "Shutdown")
            {
                File.WriteLine(Indent2 + "if(!isDCClosed)" + Environment.NewLine + Indent2 + "{");
                IncreaseIndent();
            }

            File.WriteLine(Indent2 + "string name = \"" + minfo.Name + "\";");

            if (minfo.GetParameters().Length != 0)
            {
                // Write a line of code that collects all the arguments into object[] as following:
                // var args = new object[] { arg1, arg2, ..., argn }
                CreateArgsObjectArray(File, Indent2, minfo.GetParameters());
            }

            if (IsSerializableType(minfo.ReturnType))
            {
                bool isNonVoid = minfo.ReturnType != typeof(void);
                
                if (isNonVoid)
                {
                    File.Write(Indent2 + "var result = Internal.Core.CallMethod<" + TypeToString(minfo.ReturnType) + ">");
                }
                else
                {
                    File.Write(Indent2 + "Internal.Core.CallVoidMethod");
                }

                // Add arguments
                File.Write("(dcObjKey, dcObjType, name");
                if (minfo.GetParameters().Length != 0)
                {
                    File.Write(", args");
                }
                File.WriteLine(");");

                if (isNonVoid)
                {
                    File.WriteLine(Indent2 + "return result;");
                }
            }
            else
            {
                File.Write(Indent2 + "string key = Internal.Core.CallMethod<string>(dcObjKey, dcObjType, name");
                if (minfo.GetParameters().Length != 0)
                {
                    File.Write(", args");
                }
                File.WriteLine(");");

                File.Write(Indent2 + "var result = new " + minfo.ReturnType.Name);
                if (minfo.ReturnType.IsInterface)
                {
                    File.Write("Class");
                }
                File.WriteLine("(key);");
                File.WriteLine(Indent2 + "return result;");
            }

            if (minfo.Name == "Shutdown")
            {
                DecreaseIndent();
                File.WriteLine(Indent2 + "}");
            }

            // Methods call query processing
            if (!methodTypeName.StartsWith("CQGAlgoSupplier"))
            {
                methodCall.Append(Indent2 + "private void Method" + methodTypeName + minfo.Name + "(QueryInfo query, object[] args)" +
                Environment.NewLine + Indent2 + "{" + Environment.NewLine);

                hQPOfMethodsDict.Append(Indent4 + "{ \"Method" + methodTypeName + minfo.Name + "\", this.Method" + methodTypeName + minfo.Name + "}," +
                    Environment.NewLine);

                if (methodTypeName.StartsWith("_ICQGCELGeneralEvents"))
                {
                    methodInterfName = "_ICQGCELGeneralEvents";
                }
                else if (methodTypeName.StartsWith("_ICQGCELEvents"))
                {
                    methodInterfName = "_ICQGCELEvents";
                }
                else if (methodTypeName.StartsWith("_ICQGCELInstrumentEvents"))
                {
                    methodInterfName = "_ICQGCELInstrumentEvents";
                }

                if (minfo.Name == "Shutdown")
                {
                    methodCall.Append(Indent3 + "var returnKey = \"true\";" + Environment.NewLine + Indent3 +
                        "var answer = new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: returnKey);" +
                        "PushAnswerAndDeleteQuery(answer);" + Environment.NewLine);
                }
                else
                {
                    if (argins.Length > 0)
                    {
                        for (int i = 0; i < argins.Length; i++)
                        {
                            if (argins[i].ParameterType.FullName.EndsWith("&"))
                            {
                                methodCall.Append(Indent3 + "var arg" + i + " = (" +
                                    argins[i].ParameterType.FullName.Substring(0, argins[i].ParameterType.FullName.Length - 1) +
                                    ")args[" + i + "];" + Environment.NewLine);
                            }
                        }
                    }

                    methodCall.Append(Indent3 + methodInterfName + " " + minfo.Name + "Obj = (" + methodInterfName +
                                ")qObj;" + Environment.NewLine + Indent3);

                    if (string.Concat(minfo.ReturnType.Name) != "void" &&
                        string.Concat(minfo.ReturnType.Name) != "Void")
                    {
                        methodCall.Append(minfo.ReturnType + " " + minfo.Name + "ReturnV = ");

                    }

                    var startOfCall = string.Concat("." + minfo.Name + "(");
                    var endtOfCall = string.Concat(");" + Environment.NewLine);

                    if (minfo.Name == "get_Value")
                    {
                        startOfCall = "[";
                        endtOfCall = string.Concat("];" + Environment.NewLine);
                    }

                    methodCall.Append(minfo.Name + "Obj" + startOfCall);

                    if (argins.Length > 0)
                    {
                        for (int i = 0; i < argins.Length; i++)
                        {
                            if (argins[i].ParameterType.FullName.EndsWith("&"))
                            {
                                methodCall.Append("out arg" + i);
                            }
                            else
                            {
                                methodCall.Append("(" + argins[i].ParameterType + ")args[" + i + "]");
                            }

                            methodCall.Append(i == argins.Length - 1 ? endtOfCall : ",");
                        }
                    }
                    else
                    {
                        methodCall.Append(");" + Environment.NewLine);
                    }


                    if (string.Concat(minfo.ReturnType.Name) != "void" &&
                       string.Concat(minfo.ReturnType.Name) != "Void")
                    {
                        if (IsSerializableType(minfo.ReturnType))
                        {
                            methodCall.Append(Indent3 + "var " + minfo.Name + "ValKey = \"value\";" + Environment.NewLine);
                            methodCall.Append(Indent3 +
                                "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " +
                                minfo.Name + "ValKey, value: " + minfo.Name + "ReturnV));" +
                                Environment.NewLine);
                        }
                        else
                        {
                            methodCall.Append(Indent3 + "var " + minfo.Name + "ValKey = Core.CreateUniqueKey();" + Environment.NewLine);
                            methodCall.Append(Indent3 + "ServerDictionaries.PutObjectToTheDictionary(" + minfo.Name + "ValKey, " +
                                minfo.Name + "ReturnV);" + Environment.NewLine);
                            methodCall.Append(Indent3 +
                                "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, valueKey: " +
                                minfo.Name + "ValKey));" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        methodCall.Append(Indent3 +
                            "PushAnswerAndDeleteQuery(new AnswerInfo(query.QueryKey, query.ObjectKey, query.MemberName, value: true));" +
                            Environment.NewLine);
                    }
                }

                methodCall.Append(Indent2 + "}" + Environment.NewLine + Environment.NewLine);
            }
            MemberEnd();
        }

        static IEnumerable<MethodInfo> FilterSortMethods(MethodInfo[] minfos)
        {
            return FilterMethods(minfos).OrderBy(minfo => minfo.Name);
        }

        static IEnumerable<MethodInfo> FilterMethods(MethodInfo[] minfos)
        {
            foreach (MethodInfo minfo in minfos)
            {
                foreach (string name in SkippedMethodsNames)
                {
                    if (minfo.Name == name)
                    {
                        goto Label;
                    }
                }
                foreach (string prefix in SkippedMethodsPrefixes)
                {
                    if (minfo.Name.StartsWith(prefix))
                    {
                        goto Label;
                    }
                }
                foreach (var prefixNumArgs in SkippedMethodsPrefixesNumArgs)
                {
                    if (minfo.Name.StartsWith(prefixNumArgs.Item1) && (minfo.GetParameters().Length == prefixNumArgs.Item2))
                    {
                        goto Label;
                    }
                }
                yield return minfo;
                Label:
                ;
            }
        }
    }
}