using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateMethodSignature(MethodBase mb, string retTypeName = null, string delegateName = null, bool isInterface = false, bool isStruct = false, bool isNew = false, Dictionary<int, string> nameSubstitutes = null)
        {
            string public_ = !isInterface ? "public " : string.Empty;
            File.Write(Indent1 + public_);
            if (mb.IsStatic)
            {
                File.Write("static ");
            }

            if (isNew)
            {
                File.Write("new ");
            }
            else if (mb.IsVirtual && delegateName == null && !isInterface && !isStruct)
            {
                File.Write("virtual ");
            }

            if (delegateName != null)
            {
                File.Write("delegate ");
            }
            if (retTypeName != null)
            {
                if (QuickTestMode)
                {
                    if (retTypeName == "Void")
                    {
                        // "Void" cannot be used in C# like "void"
                        retTypeName = "void";
                    }
                }
                File.Write(retTypeName + " ");
            }

            string methodName = mb.Name;
            if (methodName == ".ctor")
            {
                // This is a COM class ctor
                methodName = mb.DeclaringType.Name;
            }
            if (delegateName != null)
            {
                methodName = delegateName;
            }
            File.Write(methodName + "(");

            CreateMethodArguments(mb.GetParameters(), nameSubstitutes);

            if (delegateName == null && !isInterface)
            {
                File.WriteLine(")" + Environment.NewLine + Indent1 + "{");
            }
            else
            {
                File.WriteLine(");");
            }
        }

        static void CreateMethodArguments(ParameterInfo[] pinfos, Dictionary<int, string> nameSubstitutes)
        {
            bool first = true;
            foreach (ParameterInfo pinfo in pinfos)
            {
                if (!first)
                {
                    File.Write(", ");
                }

                string typeName = TypeToString(pinfo.ParameterType);

                if (typeName.EndsWith("&"))
                {
                    typeName = typeName.Substring(0, typeName.Length - 1);
                }

                if (pinfo.ParameterType.IsByRef)
                {
                    typeName = "ref " + typeName;
                }
                
                string parName = pinfo.Name;
                if (parName == null || parName.Length == 0)
                {
                    parName = nameSubstitutes[pinfo.Position];
                }

                File.Write(typeName + " " + parName);

                // Default value
                if (pinfo.HasDefaultValue && pinfo.Name != null)
                {
                    File.Write(" = ");
                    if (object.ReferenceEquals(pinfo.DefaultValue, null))
                    {
                        File.Write("null");
                    }
                    else
                    {
                        if (pinfo.DefaultValue.GetType() != pinfo.ParameterType && pinfo.ParameterType == typeof(object))
                        {
                            File.Write("null");
                        }
                        else if (pinfo.ParameterType.IsEnum)
                        {
                            File.Write(typeName + "." + pinfo.DefaultValue);
                        }
                        else if (pinfo.ParameterType == typeof(bool))
                        {
                            File.Write(pinfo.DefaultValue.ToString().ToLower());
                        }
                        else if (pinfo.ParameterType == typeof(string))
                        {
                            File.Write("\"" + pinfo.DefaultValue + "\"");
                        }
                        else if (pinfo.ParameterType == typeof(DateTime))
                        {
                            File.Write("default(DateTime)");
                        }
                        else
                        {
                            File.Write(pinfo.DefaultValue);
                        }
                    }
                }

                first = false;
            }
        }
    }
}