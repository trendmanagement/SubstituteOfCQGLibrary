using System;
using System.IO;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateMethodSignature(
            MethodBase mb,
            string retTypeName = null,
            string delegateName = null,
            bool isInterface = false,
            bool isStruct = false)
        {
            string public_ = !isInterface ? "public " : string.Empty;
            File.Write(Indent1 + public_);
            if (mb.IsStatic)
            {
                File.Write("static ");
            }
            else if (ObjectOverridableMethods.Contains(mb.Name))
            {
                File.Write("override ");
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

            CreateMethodArguments(mb.GetParameters());

            if (delegateName == null && !isInterface)
            {
                File.WriteLine(")" + Environment.NewLine + Indent1 + "{");
            }
            else
            {
                File.WriteLine(");");
            }
        }

        static void CreateMethodArguments(ParameterInfo[] pinfos, bool isDCEventHandler = false)
        {
            StreamWriter file = isDCEventHandler ? DCEvHndlrFile : File;

            for (int i = 0; i < pinfos.Length; i++)
            {
                ParameterInfo pinfo = pinfos[i];

                if (i != 0)
                {
                    file.Write(", ");
                }

                string typeName = TypeToString(pinfo.ParameterType);

                if (isDCEventHandler && (!IsSerializableType(pinfo.ParameterType) || pinfo.ParameterType.IsEnum))
                {
                    typeName = "CQG." + typeName;
                }

                if (typeName.EndsWith("&"))
                {
                    typeName = typeName.Substring(0, typeName.Length - 1);
                }

                if (pinfo.ParameterType.IsByRef)
                {
                    typeName = "ref " + typeName;
                }

                string parName = GetParamName(pinfo);

                file.Write(typeName + " " + parName);

                // Default value
                if (pinfo.HasDefaultValue && pinfo.Name != null)
                {
                    file.Write(" = ");
                    if (object.ReferenceEquals(pinfo.DefaultValue, null))
                    {
                        file.Write("null");
                    }
                    else
                    {
                        if (pinfo.DefaultValue.GetType() != pinfo.ParameterType && pinfo.ParameterType == typeof(object))
                        {
                            file.Write("null");
                        }
                        else if (pinfo.ParameterType.IsEnum)
                        {
                            file.Write(typeName + "." + pinfo.DefaultValue);
                        }
                        else if (pinfo.ParameterType == typeof(bool))
                        {
                            file.Write(pinfo.DefaultValue.ToString().ToLower());
                        }
                        else if (pinfo.ParameterType == typeof(string))
                        {
                            file.Write("\"" + pinfo.DefaultValue + "\"");
                        }
                        else if (pinfo.ParameterType == typeof(DateTime))
                        {
                            file.Write("default(DateTime)");
                        }
                        else
                        {
                            file.Write(pinfo.DefaultValue);
                        }
                    }
                }
            }
        }
    }
}