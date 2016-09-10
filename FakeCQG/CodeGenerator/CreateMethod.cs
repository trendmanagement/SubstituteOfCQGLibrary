using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
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
                    nameSubstitutes.Add(param.Position, "p" + Guid.NewGuid().ToString("N"));
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
                if (minfo.ReturnType != typeof(void))
                {
                    File.Write("var result = (" + TypeToString(minfo.ReturnType) + ")");
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
                MemberEnd();
            }
            else
            {
                File.WriteLine("");
            }
        }
    }
}