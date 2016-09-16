using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        // The methods with the next name will be skipped always
        static HashSet<string> SkippedMethodsNames = new HashSet<string>() { "get_Item" };

        // The methods with the next name prefixes will be skipped always
        static HashSet<string> SkippedMethodsPrefixes = new HashSet<string>() { "add_", "remove_", "Finalize" };

        // The methods with the next name prefixes will be skipped if they have the specified number of input arguments
        // (otherwise, only corresponding properties will be created)
        static Tuple<string, int>[] SkippedMethodsPrefixesNumArgs = new Tuple<string, int>[] { Tuple.Create("get_", 0), Tuple.Create("set_", 1) };

        static HashSet<string> ObjectMethods = new HashSet<string>() { "Equals", "GetHashCode", "GetType", "ToString" };

        static HashSet<string> IEnumerableMethods = new HashSet<string>() { "GetEnumerator" };

        public static void CreateMethods(Type type)
        {
            foreach (MethodInfo minfo in FilterSortMethods(type.GetMethods()))
            {
                CreateMethod(minfo, type.IsInterface, type.IsValueType);
            }
        }

        static void CreateMethod(MethodInfo minfo, bool isInterface, bool isStruct)
        {
            UpdateRegion(RegionType.Methods);

            bool isNew = ObjectMethods.Contains(minfo.Name) ||
                (isInterface && IEnumerableMethods.Contains(minfo.Name));

            Dictionary<int, string> nameSubstitutes = new Dictionary<int, string>();
            foreach (var param in minfo.GetParameters())
            {
                if (param.Name == null || param.Name.Length == 0)
                {
                    nameSubstitutes.Add(param.Position, "arg" + (param.Position + 1));
                }
            }
            if(nameSubstitutes.Keys.Count>0)
            {
                CreateMethodSignature(minfo, TypeToString(minfo.ReturnType), null, isInterface, isStruct, isNew, nameSubstitutes);
            }
            else
            {
                CreateMethodSignature(minfo, TypeToString(minfo.ReturnType), null, isInterface, isStruct, isNew);
            }

            if (!isInterface)
            {
                if (minfo.GetParameters().Length != 0)
                {
                    int i = 0;
                    File.Write(Indent2 + "object[] args = new object[" + minfo.GetParameters().Length + "] {");
                    foreach (var param in minfo.GetParameters())
                    {
                        string paramName = param.Name;
                        if (paramName == null || paramName.Length == 0)
                        {
                            paramName = nameSubstitutes[param.Position];
                        }
                        if (i == minfo.GetParameters().Length - 1)
                        {
                            File.Write(paramName);
                        }
                        else
                        {
                            File.Write(paramName + ", ");
                        }
                        i++;
                    }
                    File.WriteLine("};");
                }

                File.Write(Indent2 + "string name = \"" + minfo.Name + "\";" + Environment.NewLine + Indent2);
                if (minfo.ReturnType.Assembly.FullName.Substring(0, 8) == "mscorlib" || minfo.ReturnType.IsEnum)
                {
                    if (minfo.ReturnType != typeof(void))
                    {
                        File.Write("var result = (" + TypeToString(minfo.ReturnType) + ")");
                    }
                    else
                    {
                        File.Write("bool result = (bool)");
                    }

                    if (minfo.GetParameters().Length != 0)
                    {
                        File.WriteLine("CQG.ExecuteTheQuery(QueryInfo.QueryType.Method, thisObjUnqKey, name, args);");
                    }
                    else
                    {
                        File.WriteLine("CQG.ExecuteTheQuery(QueryInfo.QueryType.Method, thisObjUnqKey, name);");
                    }

                    if (minfo.ReturnType != typeof(void))
                    {
                        File.WriteLine(Indent2 + "return result;");
                    }
                }
                else
                {
                    if (minfo.ReturnType != typeof(void))
                    {
                        File.Write("string resultKey = (string)");
                    }
                    else
                    {
                        File.Write("bool resultKey = (bool)");
                    }

                    if (minfo.GetParameters().Length != 0)
                    {
                        File.WriteLine("CQG.ExecuteTheQuery(QueryInfo.QueryType.Method, thisObjUnqKey, name, args);");
                    }
                    else
                    {
                        File.WriteLine("CQG.ExecuteTheQuery(QueryInfo.QueryType.Method, thisObjUnqKey, name);");
                    }

                    if (minfo.ReturnType.IsInterface)
                    {
                        File.WriteLine(Indent2 + TypeToString(minfo.ReturnType) + "Class result = new " +
                        TypeToString(minfo.ReturnType) + "Class();");
                    }
                    else
                    {
                        File.WriteLine(Indent2 + TypeToString(minfo.ReturnType) + " result = new " +
                            TypeToString(minfo.ReturnType) + "();");
                    }
                    File.WriteLine(Indent2 + "object resultFlld = result;");
                    File.WriteLine(Indent2 + "CQG.GetPropertiesFromMatryoshka(ref resultFlld, thisObjUnqKey);");
                    File.WriteLine(Indent2 + "return (" + TypeToString(minfo.ReturnType) + ")resultFlld;");
                }
                
                MemberEnd();
            }
            else
            {
                File.WriteLine("");
            }
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