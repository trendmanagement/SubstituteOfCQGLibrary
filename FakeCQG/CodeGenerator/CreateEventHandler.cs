using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeGenerator
{
    partial class Program
    {
        static void CreateEventHandler(MethodInfo minfo, Type type)
        {
            Dictionary<int, string> nameSubstitutes = new Dictionary<int, string>();
            foreach (var param in minfo.GetParameters())
            {
                if (param.Name == null || param.Name.Length == 0)
                {
                    nameSubstitutes.Add(param.Position, "arg" + (param.Position + 1));
                }
            }

            CreateEventHandlerSignature(minfo, type);

            if (minfo.GetParameters().Length != 0)
            {
                int i = 0;
                DCEvHndlrFile.Write(Indent1 + Indent2 + "object[] args = new object[" + (minfo.GetParameters().Length + 1) + "] {\"!\", ");
                foreach (var param in minfo.GetParameters())
                {
                    string paramName = param.Name;
                    //if (paramName == null || paramName.Length == 0)
                    //{
                    //    paramName = nameSubstitutes[param.Position];
                    //}
                    if (i == minfo.GetParameters().Length - 1)
                    {
                        DCEvHndlrFile.Write(paramName);
                    }
                    else
                    {
                        DCEvHndlrFile.Write(paramName + ", ");
                    }
                    i++;
                }
                DCEvHndlrFile.WriteLine("};");
                DCEvHndlrFile.WriteLine("");
            }

            DCEvHndlrFile.Write(Indent1 + Indent2 + "string name = \"" + type.Name + "\";" + Environment.NewLine + Indent2);

            if (minfo.GetParameters().Length != 0)
            {
                DCEvHndlrFile.WriteLine(Indent1 + "FakeCQG.CQG.CommonEventHandler(name, args);");
            }
            else
            {
                DCEvHndlrFile.WriteLine(Indent1 + "FakeCQG.CQG.CommonEventHandler(name);");
            }
            DCEvHndlrFile.WriteLine(Indent2 + "}");
            DCEvHndlrFile.WriteLine("");
        }

        static void CreateEventHandlerSignature(MethodBase mb, Type type, Dictionary<int, string> nameSubstitutes = null)
        {
            string modifs = "public static void ";
            DCEvHndlrFile.Write(Indent2 + modifs);

            string methodName = string.Format("{0}Impl", type.Name);

            DCEvHndlrFile.Write(methodName + "(");

            CreateEventHandlerArguments(mb.GetParameters(), nameSubstitutes);

            DCEvHndlrFile.WriteLine(")" + Environment.NewLine + Indent2 + "{");
        }

        static void CreateEventHandlerArguments(ParameterInfo[] pinfos, Dictionary<int, string> nameSubstitutes)
        {
            bool first = true;
            foreach (ParameterInfo pinfo in pinfos)
            {
                if (!first)
                {
                    DCEvHndlrFile.Write(", ");
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

                DCEvHndlrFile.Write(typeName + " " + parName);

                // Default value
                if (pinfo.HasDefaultValue && pinfo.Name != null)
                {
                    DCEvHndlrFile.Write(" = ");
                    if (object.ReferenceEquals(pinfo.DefaultValue, null))
                    {
                        DCEvHndlrFile.Write("null");
                    }
                    else
                    {
                        if (pinfo.DefaultValue.GetType() != pinfo.ParameterType && pinfo.ParameterType == typeof(object))
                        {
                            DCEvHndlrFile.Write("null");
                        }
                        else if (pinfo.ParameterType.IsEnum)
                        {
                            DCEvHndlrFile.Write(typeName + "." + pinfo.DefaultValue);
                        }
                        else if (pinfo.ParameterType == typeof(bool))
                        {
                            DCEvHndlrFile.Write(pinfo.DefaultValue.ToString().ToLower());
                        }
                        else if (pinfo.ParameterType == typeof(string))
                        {
                            DCEvHndlrFile.Write("\"" + pinfo.DefaultValue + "\"");
                        }
                        else if (pinfo.ParameterType == typeof(DateTime))
                        {
                            DCEvHndlrFile.Write("default(DateTime)");
                        }
                        else
                        {
                            DCEvHndlrFile.Write(pinfo.DefaultValue);
                        }
                    }
                }

                first = false;
            }
        }

    }
}
